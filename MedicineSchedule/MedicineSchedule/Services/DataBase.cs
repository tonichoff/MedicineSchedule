using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using SQLite;

using MedicineSchedule.Models;

namespace MedicineSchedule.Services
{
	public class DataBase
	{
		private static string DbPath => Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Personal), "db.db"
		);

		private static SQLiteAsyncConnection connection;

		private static bool tablesChecked;

		public DataBase()
		{
			connection ??= new SQLiteAsyncConnection(DbPath);
			CheckTables();
		}

		private async void CheckTables()
		{
			if (!tablesChecked) {
				await connection.CreateTableAsync<Course>();
				await connection.CreateTableAsync<Reception>();
				await connection.CreateTableAsync<NextReceptionInfo>();
				tablesChecked = true;
			}
		}

		public Task<int> CreateCourseAndGetId(Course course)
		{
			return connection.InsertAsync(course);
		}

		public async Task CreateReceptions(List<Reception> receptions)
		{
			foreach (var reception in receptions) {
				await connection.InsertAsync(reception);
			}
		}

		public async Task CreateReception(Reception reception)
		{
			await connection.InsertAsync(reception);
		}

		public async Task CreateNextReceptionInfo(NextReceptionInfo info)
		{
			await connection.InsertAsync(info);
		}

		public async Task UpdateCourse(Course course)
		{
			await connection.UpdateAsync(course);
		}

		public async Task UpdateReception(Reception reception)
		{
			await connection.UpdateAsync(reception);
		}

		public async Task UpdateNextReceptionInfo(NextReceptionInfo info)
		{
			await connection.UpdateAsync(info);
		}

		public async Task DeleteCourse(Course course)
		{
			await connection.DeleteAsync(course);
		}

		public async Task DeleteReception(Reception reception)
		{
			await connection.DeleteAsync(reception);
		}

		public async Task DeleteNextReceptionInfo(NextReceptionInfo info)
		{
			await connection.DeleteAsync(info);
		}

		public List<(Course, List<Reception>)> GetAllCoursesWithReceptions()
		{
			var result = new List<(Course, List<Reception>)>();
			var courses = connection.Table<Course>().ToListAsync();
			foreach (var course in courses.Result) {
				result.Add((
					course,
					connection.Table<Reception>()
						.Where(r => r.CourseId == course.Id)
						.ToListAsync()
						.Result
				));
			}
			return result;
		}

		public async Task<Course> GetCourseAtId(int id)
		{
			return await connection.GetAsync<Course>(id);
		}

		public async Task<Reception> GetReceptionAtId(int id)
		{
			return await connection.GetAsync<Reception>(id);
		}

		public async Task<List<Reception>> GetReceptionsAtCourseId(int courseId)
		{
			return await connection.Table<Reception>().Where(r => r.CourseId == courseId).ToListAsync();
		}

		public async Task<NextReceptionInfo> GetNextReceptionAtId(int id)
		{
			return await connection.GetAsync<NextReceptionInfo>(id);
		}

		public async Task<NextReceptionInfo> GetNextReceptionInfoAtCourseId(int courseId)
		{
			return await connection.GetAsync<NextReceptionInfo>((info) => info.CourseId == courseId);
		}

		public List<Reception> GetReceptionsByDate(DateTime date)
		{
			var clearDate = date - date.TimeOfDay;
			var courses = connection.Table<Course>().Where(c => c.StartDate <= clearDate).ToListAsync();
			var receptions = new List<Reception>();
			foreach (var course in courses.Result) {
				bool validationPassed = false;
				int daysPassed = 0;
				int receptionLeft = 0;
				switch (course.DaysMode) {
					case DaysMode.EveryDay:
						validationPassed = true;
						daysPassed = (int) Math.Round((clearDate - course.StartDate).TotalDays);
						receptionLeft = course.ReceptionsCount - daysPassed * course.ReceptionsInDayCount;
						break;
					case DaysMode.Interval:
						int daysAgo = (int)Math.Round((clearDate - course.StartDate).TotalDays);
						int interval = course.DaysInterval + course.DaysSkip;
						int remainder = daysAgo % interval;
						if (remainder < course.DaysInterval) {
							validationPassed = true;
							daysPassed = (daysAgo / interval) * course.DaysInterval + remainder;
							receptionLeft = course.ReceptionsCount - daysPassed * course.ReceptionsInDayCount;
						}
						break;
				}
				if (!validationPassed) {
					continue;
				}
				switch (course.ReceptionMode) {
					case ReceptionMode.Regular:
						validationPassed = true;
						break;
					case ReceptionMode.ReceptionCount:
						validationPassed = receptionLeft > 0;
						break;
					case ReceptionMode.DaysCount:
						validationPassed = daysPassed < course.DaysCount;
						break;
				}
				if (validationPassed) {
					var newReceptions = GetReceptionsAtCourseId(course.Id).Result;
					foreach (var reception in newReceptions) {
						if (course.ReceptionMode == ReceptionMode.ReceptionCount) {
							if (receptionLeft-- <= 0) {
								break;
							}
						}
						reception.CourseName = course.MedicineName;
						reception.MedicineType = course.MedicineType;
						receptions.Add(reception);
					}
				}
			}
			return receptions;
		}
	}
}
