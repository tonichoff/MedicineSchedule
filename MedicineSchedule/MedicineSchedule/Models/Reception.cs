using System;

using SQLite;

namespace MedicineSchedule.Models
{
	public class Reception
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public int Id { get; set; }

		public TimeSpan Time { get; set; }

		public int CourseId { get; set; }

		public double MedicineValue { get; set; }

		[Ignore]
		public string CourseName { get; set; }

		[Ignore]
		public MedicineType MedicineType { get; set; }
	}
}
