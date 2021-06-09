using System;

using SQLite;

namespace MedicineSchedule.Models
{
	public enum MedicineType
	{
		None,
		Tablet,
		Capsule,
		Drops,
		Potion,
		Salve,
		Injection,
		Procedure,
	}

	public enum Measuring
	{
		None,
		Piece,
		Mg,
		Ml,
	}

	public enum FoodRelation
	{
		None,
		Before,
		During,
		After,
	}

	public enum ReceptionMode
	{
		Regular,
		DaysCount,
		ReceptionCount,
	}

	public enum DaysMode
	{
		EveryDay,
		Interval,
		ConcreateDays,
	}

	[Flags]
	public enum DaysOfWeek
	{
		None = 0,
		Monday = 1 << 0,
		Tuesday = 1 << 1,
		Wednesday = 1 << 2,
		Thursday = 1 << 3,
		Friday = 1 << 4,
		Saturday = 1 << 5,
		Sunday = 1 << 6,
		All = ~None
	}


	public class Course
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public int Id { get; set; }
		public string MedicineName { get; set; }
		public MedicineType MedicineType { get; set; }
		public Measuring Measuring { get; set; }
		public DateTime StartDate { get; set; }
		public ReceptionMode ReceptionMode { get; set; }
		public DaysMode DaysMode { get; set; }
		public int ReceptionsInDayCount { get; set; }
		public int DaysInterval { get; set; }
		//public DaysOfWeek DaysOfWeek { get; set; }
		public int DaysCount { get; set; }
		public int ReceptionsCount { get; set; }
		public FoodRelation FoodRelation { get; set; }
	}
}
