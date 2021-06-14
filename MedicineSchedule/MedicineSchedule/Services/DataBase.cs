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

		public async void UpdateCourse(Course course)
		{
			await connection.UpdateAsync(course);
		}

		public async Task UpdateReception(Reception reception)
		{
			await connection.UpdateAsync(reception);
		}

		public async void DeleteCourse(Course course)
		{
			await connection.DeleteAsync(course);
		}

		public async void DeleteReception(Reception reception)
		{
			await connection.DeleteAsync(reception);
		} 

		public async Task<List<(Course, List<Reception>)>> GetAllCoursesWithReceptions()
		{
			var result = new List<(Course, List<Reception>)>();
			var courses = connection.Table<Course>().ToListAsync();
			foreach (var course in courses.Result) {
				result.Add(
					(
						course,
						connection.Table<Reception>()
							.Where(r => r.CourseId == course.Id)
							.ToListAsync()
							.Result
					)
				);
			}
			return result;
		}

		public async Task<Course> GetCourseAtId(int id)
		{
			return await connection.GetAsync<Course>(id);
		}

		public async Task<List<Reception>> GetReceptionsByDate(DateTime date)
		{
			var courses = connection.Table<Course>().Where(c => c.StartDate <= date).ToListAsync();
			var receptions = new List<Reception>();
			foreach (var course in courses.Result) {
				switch (course.ReceptionMode) {
					case ReceptionMode.Regular:
						receptions.AddRange(GetReceptionsHelper(course));
						break;
					case ReceptionMode.ReceptionCount:
						//if (course.ReceptionsWasTakenCount < course.ReceptionsCount) {
						//	receptions.AddRange(GetReceptionsHelper(course));
						//}
						break;
					case ReceptionMode.DaysCount:
						//if (course.DaysWasTakenCount <= course.DaysCount) {
						//	receptions.AddRange(GetReceptionsHelper(course));
						//}
						break;
				}
			}
			return receptions;

			IEnumerable<Reception> GetReceptionsHelper(Course course)
			{
				bool validate = false;
				switch (course.DaysMode) {
					case DaysMode.EveryDay:
						validate = true;
						break;
					case DaysMode.Interval:
						int daysAgo = (int)(date - course.StartDate).TotalDays;
						int interval = course.DaysInterval + 1;
						validate = daysAgo % interval == 0;
						break;
				}
				if (validate) {
					var receptions = connection
						.Table<Reception>()
						.Where(r => r.CourseId == course.Id)
						.ToListAsync()
						.Result;
					foreach (var reception in receptions) {
						reception.CourseName = course.MedicineName;
						reception.MedicineType = course.MedicineType;
						yield return reception;
					}
				}
				yield break;
			}
		}
	}
}
