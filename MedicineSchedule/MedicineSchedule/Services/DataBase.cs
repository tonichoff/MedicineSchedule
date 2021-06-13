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
			var receptions = connection.Table<Reception>().Where(r => r.CourseId == course.Id);
			if (receptions != null) {
				await connection.DeleteAsync(receptions);
			}
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
	}
}
