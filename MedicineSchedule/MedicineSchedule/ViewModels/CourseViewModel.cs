using System.ComponentModel;

using MedicineSchedule.Models;

namespace MedicineSchedule.ViewModels
{
	public class CourseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public Course Course { get; private set; }

		public CourseViewModel(Course course = null)
		{
			Course = course;
		}
	}
}
