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

		public async void CreateCourseWithReceptions(Course course, List<Reception> receptions)
		{
			await connection.InsertAsync(course);
			if (receptions != null) {
				foreach (var recept in receptions) {
					await connection.InsertAsync(recept);
				}
			}
		}

		public async void UpdateCourse(Course course)
		{
			await connection.UpdateAsync(course);
		}

		public async void UpdateReception(Reception reception)
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

		public async Task<IEnumerable<Course>> GetAllCourses()
		{
			return await connection.Table<Course>().ToListAsync();
		}
	}
}
