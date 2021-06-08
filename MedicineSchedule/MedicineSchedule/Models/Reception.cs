using System;

namespace MedicineSchedule.Models
{
	public class Reception
	{
		public int Id { get; set; }

		public TimeSpan Time { get; set; }

		public int CourseId { get; set; }

		public double MedicineValue { get; set; }
	}
}
