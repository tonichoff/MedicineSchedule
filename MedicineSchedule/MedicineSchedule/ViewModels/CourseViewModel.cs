using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using MedicineSchedule.Models;
using MedicineSchedule.Services;

namespace MedicineSchedule.ViewModels
{
	public class CourseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public Page ParentPage { get; set; }

		public Command CreateCourseCommand { get; set; }
		public Command UpdateCourseCommand { get; set; }
		public Command DeleteCourseCommand { get; set; }
		public Command CancelCommand { get; set; }

		public Course Course { get; private set; }
		public List<Reception> Receptions { get; private set; }
		public List<TimeSpan> Times { get; set; }

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

		public string MedicineValue
		{
			get => Course == null ? Course.MedicineValue.ToString() : medicineValue.ToString();
			set
			{
				if (double.TryParse(value, out double temp)) {
					if (Course == null) {
						medicineValue = temp;
					} else {
						Course.MedicineValue = temp;
					}
				}
			}
		}

		public string ReceptionsInDay
		{
			get => (Course == null ? Course.ReceptionsInDayCount : receptionsInDayCount).ToString();
			set
			{
				if (int.TryParse(value, out int temp)) {
					if (Course == null) {
						receptionsInDayCount = temp;
					} else {
						Course.ReceptionsInDayCount = temp;
					}
				} else {
					if (Course == null) {
						receptionsInDayCount = 0;
					} else {
						Course.ReceptionsInDayCount = 0;
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

		public int ReceptionMode
		{
			get => (int)(Course == null ? Course.ReceptionMode : receptionMode);
			set
			{
				if (Course == null) {
					receptionMode = (ReceptionMode)value;
				} else {
					Course.ReceptionMode = (ReceptionMode)value;
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public string DaysCount
		{
			get => (Course == null ? Course.DaysCount : daysCount).ToString();
			set
			{
				if (int.TryParse(value, out int temp)) {
					if (Course == null) {
						daysCount = temp;
					} else {
						Course.DaysCount = temp;
					}
				} else {
					if (Course == null) {
						daysCount = 0;
					} else {
						Course.DaysCount = 0;
					}
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public string ReceptionsCount
		{
			get => (Course == null ? Course.ReceptionsCount : receptionsCount).ToString();
			set
			{
				if (int.TryParse(value, out int temp)) {
					if (Course == null) {
						receptionsCount = temp;
					} else {
						Course.ReceptionsCount = temp;
					}
				} else {
					if (Course == null) {
						receptionsCount = 0;
					} else {
						Course.ReceptionsCount = 0;
					}
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public int DaysMode
		{
			get => (int)(Course == null ? Course.DaysMode : daysMode);
			set
			{
				if (Course == null) {
					daysMode = (DaysMode)value;
				} else {
					Course.DaysMode = (DaysMode)value;
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public string DaysInterval
		{
			get => (Course == null ? Course.DaysInterval : daysInterval).ToString();
			set
			{
				if (int.TryParse(value, out int temp)) {
					if (Course == null) {
						daysInterval = temp;
					} else {
						Course.DaysInterval = temp;
					}
				} else {
					if (Course == null) {
						daysInterval = 0;
					} else {
						Course.DaysInterval = 0;
					}
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		public string DaysSkip
		{
			get => (Course == null ? Course.DaysSkip : daysSkip).ToString();
			set
			{
				if (int.TryParse(value, out int temp)) {
					if (Course == null) {
						daysSkip = temp;
					} else {
						Course.DaysSkip = temp;
					}
				} else {
					if (Course == null) {
						daysSkip = 0;
					} else {
						Course.DaysSkip = 0;
					}
				}
				CreateCourseCommand.ChangeCanExecute();
				UpdateCourseCommand.ChangeCanExecute();
			}
		}

		private string name;
		private MedicineType medicineType;
		private FoodRelation foodRelation;
		private Measuring measuring;
		private double medicineValue;
		private int receptionsInDayCount;
		private DateTime startDate;
		private ReceptionMode receptionMode;
		private int daysCount;
		private int receptionsCount;
		private DaysMode daysMode;
		private int daysInterval;
		public int daysSkip;

		private readonly DataBase dataBase = new DataBase();

		public CourseViewModel(Course course = null, List<Reception> receptions = null)
		{
			Course = course;
			Receptions = receptions;
			Times = new List<TimeSpan>();
			if (Course != null) {
				name = Course.MedicineName;
				medicineType = Course.MedicineType;
				foodRelation = Course.FoodRelation;
				receptionsInDayCount = Course.ReceptionsInDayCount;
				startDate = Course.StartDate;
				measuring = Course.Measuring;
				medicineValue = Course.MedicineValue;
				receptionMode = Course.ReceptionMode;
				daysCount = Course.DaysCount;
				receptionsCount = Course.ReceptionsCount;
				daysMode = Course.DaysMode;
				daysInterval = Course.DaysInterval;
				daysSkip = Course.DaysSkip;
			} else {
				startDate = DateTime.Now - DateTime.Now.TimeOfDay;
				daysCount = 1;
				receptionsCount = 1;
				daysInterval = 1;
				daysSkip = 1;
			}

			if (Receptions == null) {
				Receptions = new List<Reception>();
			}

			for (int i = 0; i < 12; ++i) {
				Times.Add(
					(i < receptionsInDayCount && i < Receptions.Count)
					? Receptions[i].Time
					: new TimeSpan(12, 0, 0)
				);
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
				Course = new Course() {
					MedicineName = name,
					MedicineType = medicineType,
					FoodRelation = foodRelation,
					ReceptionsInDayCount = receptionsInDayCount,
					StartDate = startDate,
					Measuring = measuring,
					MedicineValue = medicineValue,
					ReceptionMode = receptionMode,
					DaysCount = daysCount,
					ReceptionsCount = receptionsCount,
					DaysMode = daysMode,
					DaysInterval = daysInterval,
					DaysSkip = daysSkip,
				};
				var creatingTask = dataBase.CreateCourseAndGetId(Course);
				creatingTask.Wait();
				int courseId = Course.Id;
				var receptions = new List<Reception>();
				for (int i = 0; i < Course.ReceptionsInDayCount; ++i) {
					receptions.Add(new Reception() {
						CourseId = courseId,
						Time = Times[i],
					});
				}
				Receptions = receptions.OrderBy(r => r.Time).ToList();
				await dataBase.CreateReceptions(Receptions);
				await Task.Run(() => {
					var info = new NextReceptionInfo() {
						CourseId = Course.Id,
						NextDate = Course.StartDate,
						NextTime = Receptions[0].Time,
						ReceptionsCount = 0,
						InDayReceptionIndex = 0,
						DaysCount = 0,
						InIntervalDayIndex = 0,
					};
					var createTask = dataBase.CreateNextReceptionInfo(info);
					createTask.Wait();
					NotificationManager.CreateNotification(info);
				});
			}
			await ParentPage.Navigation.PopModalAsync();
		}

		private async void UpdateCourse()
		{
			if (Course != null) {
				await dataBase.UpdateCourse(Course);
			}
			await ParentPage.Navigation.PopModalAsync();
		}

		private async void DeleteCourse()
		{
			if (Course != null) {
				bool userIsSure = await ParentPage.DisplayAlert("Вы уверены?", "Вернуть курс будет нельзя", "Да", "Нет");
				if (userIsSure) {
					await Task.Run(async () => {
						await dataBase.DeleteCourse(Course);
						foreach (var reception in Receptions) {
							await dataBase.DeleteReception(reception);
						}
						var nextReceptionInfo = dataBase.GetNextReceptionInfoAtCourseId(Course.Id).Result;
						await dataBase.DeleteNextReceptionInfo(nextReceptionInfo);
						NotificationManager.DeleteNotification(nextReceptionInfo);
					});
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
					(receptionsInDayCount >= 1 && receptionsInDayCount <= 12) &&
					(receptionMode != Models.ReceptionMode.DaysCount || daysCount >= 1) &&
					(receptionMode != Models.ReceptionMode.ReceptionCount || receptionsCount >= 1) &&
					(daysMode != Models.DaysMode.Interval || (daysInterval >= 1 && daysSkip >= 1));
			} else {
				return
					!string.IsNullOrEmpty(Course.MedicineName) &&
					(Course.ReceptionsInDayCount >= 1 && Course.ReceptionsInDayCount <= 12) &&
					(Course.ReceptionMode != Models.ReceptionMode.DaysCount || Course.DaysCount >= 1) &&
					(Course.ReceptionMode != Models.ReceptionMode.ReceptionCount || Course.ReceptionsCount >= 1) &&
					(Course.DaysMode != Models.DaysMode.Interval || (Course.DaysInterval >= 1 && Course.DaysSkip >= 1));
			}
		}
	}
}
