using System;

using SQLite;

namespace MedicineSchedule.Models
{
	public class NextReceptionInfo
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public int Id { get; set; }
		public int CourseId { get; set; }
		public DateTime NextDateTime { get; set; }
		public int ReceptionsCount { get; set; }
		public int InDayReceptionIndex { get; set; }
		public int DaysCount { get; set; }
		public int InIntervalDayIndex { get; set; }
	}
}
