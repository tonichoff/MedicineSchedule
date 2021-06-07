using System;
using System.ComponentModel;

using Xamarin.Forms;

using MedicineSchedule.Models;

namespace MedicineSchedule.ViewModels
{
	public class CourseViewModel : INotifyPropertyChanged
	{
		public static Course StaticCourse;

		public event PropertyChangedEventHandler PropertyChanged;

		public Page ParentPage { get; set; }

		public Command CreateCourseCommand { get; set; }
		public Command UpdateCourseCommand { get; set; }
		public Command DeleteCourseCommand { get; set; }
		public Command CancelCommand { get; set; }

		public Course Course { get; private set; }

		public string Name
		{
			get => Course == null ? name : Course.MedicineName;
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					if (Course == null) {
						name = value;
					} else {
						Course.MedicineName = value;
					}
					OnPropertyChanged("Name");
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public int MedicineType
		{
			get => (int)(Course == null ? Course.MedicineType : medicineType);
			set
			{
				if (Course == null) {
					medicineType = (MedicineType)value;
				} else {
					Course.MedicineType = (MedicineType)value;
				}
			}
		}

		public int FoodRelation
		{
			get => (int)(Course == null ? Course.FoodRelation : foodRelation);
			set
			{
				if (Course == null) {
					foodRelation = (FoodRelation)value;
				} else {
					Course.FoodRelation = (FoodRelation)value;
				}
			}
		}

		public int Measuring
		{
			get => (int)(Course == null ? Course.Measuring : measuring);
			set
			{
				if (Course == null) {
					measuring = (Measuring)value;
				} else {
					Course.Measuring = (Measuring)value;
				}
			}
		}

		public string ReceptionsInDay
		{
			get => (Course == null ? Course.ReceptionInDayCount : receptionsInDayCount).ToString();
			set
			{
				if (int.TryParse(value, out int temp)) {
					if (Course == null) {
						receptionsInDayCount = temp;
					} else {
						Course.ReceptionInDayCount = temp;
					}
				} else {
					if (Course == null) {
						receptionsInDayCount = 0;
					} else {
						Course.ReceptionInDayCount = 0;
					}
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public DateTime StartDate
		{
			get => Course == null ? Course.StartDate : startDate;
			set
			{
				if (Course == null) {
					startDate = value;
				} else {
					Course.StartDate = value;
				}
			}
		}

		private string name;
		private MedicineType medicineType;
		private FoodRelation foodRelation;
		private Measuring measuring;
		private int receptionsInDayCount;
		private DateTime startDate;

		public CourseViewModel(Course course = null)
		{
			Course = course;
			if (course == null && StaticCourse != null) {
				Course = StaticCourse;
			}
			if (Course != null) {
				name = Course.MedicineName;
				medicineType = Course.MedicineType;
				foodRelation = Course.FoodRelation;
				receptionsInDayCount = Course.ReceptionInDayCount;
				startDate = Course.StartDate;
				measuring = Course.Measuring;
			} else {
				StartDate = DateTime.Now;
			}
			
			CreateCourseCommand = new Command(CreateCourse, Validate);
			UpdateCourseCommand = new Command(UpdateCourse, Validate);
			DeleteCourseCommand = new Command(DeleteCourse);
			CancelCommand = new Command(Cancel);
		}

		protected void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		private async void CreateCourse()
		{
			if (Course == null) {
				StaticCourse = new Course() {
					MedicineName = name,
					MedicineType = medicineType,
					FoodRelation = foodRelation,
					ReceptionInDayCount = receptionsInDayCount,
					StartDate = startDate,
					Measuring = measuring,
				};
			}
			await ParentPage.Navigation.PopModalAsync();
		}

		private async void UpdateCourse()
		{
			if (Course != null) {
				StaticCourse = Course;
			}
			await ParentPage.Navigation.PopModalAsync();
		}

		private async void DeleteCourse()
		{
			if (Course != null) {
				bool userIsSure = await ParentPage.DisplayAlert("Вы уверены?", "Вернуть курс будет нельзя", "Да", "Нет");
				if (userIsSure) {
					Course = null;
					StaticCourse = null;
					await ParentPage.Navigation.PopModalAsync();
				}
			}
		}

		private async void Cancel()
		{
			bool userIsSure = await ParentPage.DisplayAlert("Вы уверены?", "Изменения не будут сохранены", "Да", "Нет");
			if (userIsSure) {
				await ParentPage.Navigation.PopModalAsync();
			}
		}

		private bool Validate()
		{
			if (Course == null) {
				return
					!string.IsNullOrEmpty(name) &&
					(receptionsInDayCount >= 1 && receptionsInDayCount <= 12);
			} else {
				return
					!string.IsNullOrEmpty(Course.MedicineName) &&
					(Course.ReceptionInDayCount >= 1 && Course.ReceptionInDayCount <= 12);
			}
		}
	}
}
