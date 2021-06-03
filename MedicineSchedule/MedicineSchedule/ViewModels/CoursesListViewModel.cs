using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using MedicineSchedule.Models;
using MedicineSchedule.Views;

namespace MedicineSchedule.ViewModels
{
	public class CoursesListViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Course> Courses { get; set; }

		public Command LoadCoursesCommand { get; set; }
		public Command CreateCourseCommand { get; set; }
		public Command<Course> ShowCourseDetailsCommand { get; }

		public INavigation Navigation { get; set; }

		public bool IsUpdating { get; set; }

		public CoursesListViewModel()
		{
			Courses = new ObservableCollection<Course>();
			LoadCoursesCommand = new Command(async () => await ExecuteLoadCoursesCommand());
			CreateCourseCommand = new Command(CreateCourse);
			ShowCourseDetailsCommand = new Command<Course>(ShowCourseDetails);
		}

		private async Task ExecuteLoadCoursesCommand()
		{
			IsUpdating = true;
			try {
				Courses.Clear();
				var courses = new ObservableCollection<Course>() {

				};
			} catch (Exception exception) {
				Debug.WriteLine(
					$"Thrown exception while trying load courses in CoursesList. Exception message: {exception.Message}"
				);
			} finally {
				IsUpdating = false;
			}
		}

		private async void CreateCourse()
		{
			await Navigation.PushModalAsync(new CourseView(new CourseViewModel()));
		}

		private async void ShowCourseDetails(Course course)
		{
			await Navigation.PushModalAsync(new CourseView(new CourseViewModel((Course)course)));
		}
	}
}
