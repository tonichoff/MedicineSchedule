using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;

using MedicineSchedule.Models;
using MedicineSchedule.Services;
using MedicineSchedule.Views;

namespace MedicineSchedule.ViewModels
{
	public class CoursesListViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Course> Courses { get; set; }
		public Dictionary<int, List<Reception>> Receptions { get; set; }
		public List<TimeSpan> Times { get; set; }

		public Command LoadCoursesCommand { get; set; }
		public Command CreateCourseCommand { get; set; }
		public Command<Course> ShowCourseDetailsCommand { get; }
		public Command DebugCommand { get; set; }

		public INavigation Navigation { get; set; }

		public bool IsUpdating
		{
			get => isUpdating;
			set
			{
				isUpdating = value;
				OnPropertyChanged("IsUpdating");
			}
		}

		private readonly DataBase dataBase = new DataBase();

		private bool isUpdating;
		private bool isBusy;

		public CoursesListViewModel()
		{
			Courses = new ObservableCollection<Course>();
			Receptions = new Dictionary<int, List<Reception>>();
			LoadCoursesCommand = new Command(async () => await Task.Run(() => ExecuteLoadCoursesCommand()));
			CreateCourseCommand = new Command(CreateCourse);
			ShowCourseDetailsCommand = new Command<Course>(ShowCourseDetails);
			DebugCommand = new Command(ShowNotificationPage);
		}

		public void OnAppearing()
		{
			IsUpdating = true;
			isBusy = false;
		}

		private void ExecuteLoadCoursesCommand()
		{
			IsUpdating = true;
			try {
				Courses.Clear();
				Receptions.Clear();
				var coursesAndReceptions = dataBase.GetAllCoursesWithReceptions();
				foreach (var item in coursesAndReceptions) {
					var (course, receptions) = item;
					Courses.Add(course);
					Receptions[course.Id] = receptions;
				}
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
			if (!isBusy) {
				isBusy = true;
				await Navigation.PushModalAsync(new CourseView(new CourseViewModel()));
			}
		}

		private async void ShowCourseDetails(Course course)
		{
			if (!isBusy) {
				isBusy = true;
				await Navigation.PushModalAsync(
					new CourseView(
						new CourseViewModel(course, Receptions[course.Id])
					)
				);
			}
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ShowNotificationPage()
		{
			try {
				Navigation.PushModalAsync(new Pages.NotificationPage(0));
			} catch (Exception exception) {
				Debug.WriteLine(exception.Message);
			}
		}
	}
}
