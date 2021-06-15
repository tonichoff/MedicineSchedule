using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.Models;
using MedicineSchedule.Services;

namespace MedicineSchedule.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationPage : ContentPage
	{
		private NextReceptionInfo nextReceptionInfo;
		private Course course;

		private readonly DataBase dataBase = new DataBase();

		public NotificationPage(int infoId)
		{
			InitializeComponent();
			SetDescriptionAsync(infoId);
		}

		private async void SetDescriptionAsync(int infoId)
		{
			await Task.Run(() => SetDescription(infoId));
		}

		private void SetDescription(int receptionInfoId)
		{
			nextReceptionInfo = dataBase.GetNextReceptionAtId(receptionInfoId).Result;
			course = dataBase.GetCourseAtId(nextReceptionInfo.CourseId).Result;
			string description;
			string buttonText;
			switch (course.MedicineType) {
				case MedicineType.Tablet:
				case MedicineType.Capsule:
				case MedicineType.Drops:
				case MedicineType.Potion:
				case MedicineType.Salve:
				case MedicineType.Injection:
					description = $"Пора принимать {course.MedicineName} ";
					description += $"в количестве {course.MedicineValue}";
					switch (course.Measuring) {
						case Measuring.Mg:
							description += " миллиграмм";
							break;
						case Measuring.Ml:
							description += " миллилитров";
							break;
						case Measuring.Piece:
							description += " штук";
							break;
					}
					switch (course.FoodRelation) {
						case FoodRelation.Before:
							description += " до еды";
							break;
						case FoodRelation.During:
							description += " во время еды";
							break;
						case FoodRelation.After:
							description += " после еды";
							break;
					}
					buttonText = "Я принялъ";
					break;
				case MedicineType.Procedure:
					description = $"Время для процедуры {course.MedicineName}";
					buttonText = "Процедурюсь";
					break;
				default:
					description = "Пора что-то принимать";
					buttonText = "Я принялъ";
					break;
			}
			EatLabel.Text = description;
			EatButton.Text = buttonText;
		}

		private void EatButtonClicked(object sender, EventArgs eventArgs)
		{
			Task.Run(async () => {
				await Navigation.PopModalAsync();
				NotificationManager.DeleteNotification(nextReceptionInfo);
				if (TryUpdateNextReceptionInfo()) {
					NotificationManager.CreateNotification(nextReceptionInfo);
				}
			});
		}

		private bool TryUpdateNextReceptionInfo()
		{
			if (nextReceptionInfo == null) {
				return false;
			}
			if (course == null) {
				course = dataBase.GetCourseAtId(nextReceptionInfo.CourseId).Result;
			}
			++nextReceptionInfo.ReceptionsCount;
			++nextReceptionInfo.InDayReceptionIndex;
			if (
				course.ReceptionMode == ReceptionMode.ReceptionCount &&
				course.ReceptionsCount == nextReceptionInfo.ReceptionsCount
			) {
				return false;
			}
			if (course.ReceptionsInDayCount == nextReceptionInfo.InDayReceptionIndex) {
				++nextReceptionInfo.DaysCount;
				++nextReceptionInfo.InIntervalDayIndex;
				nextReceptionInfo.InDayReceptionIndex = 0;
			}
			if (
				course.ReceptionMode == ReceptionMode.DaysCount &&
				course.DaysCount == nextReceptionInfo.DaysCount
			) {
				return false;
			}
			if (nextReceptionInfo.InDayReceptionIndex == 0) {
				if (course.DaysMode == DaysMode.EveryDay) {
					nextReceptionInfo.NextDate = nextReceptionInfo.NextDate.AddDays(1);
				} else if (course.DaysMode == DaysMode.Interval) {
					if (course.DaysInterval == nextReceptionInfo.InIntervalDayIndex) {
						nextReceptionInfo.NextDate = nextReceptionInfo.NextDate.AddDays(course.DaysSkip);
					} else {
						nextReceptionInfo.NextDate = nextReceptionInfo.NextDate.AddDays(1);
					}
				}
			}
			if (course.ReceptionsInDayCount != 1) {
				var receptions = dataBase
					.GetReceptionsAtCourseId(course.Id)
					.Result
					.OrderBy(r => r.Time)
					.ToList();
				nextReceptionInfo.NextTime = receptions[nextReceptionInfo.InDayReceptionIndex].Time;
			}
			var task = dataBase.UpdateNextReceptionInfo(nextReceptionInfo);
			task.Wait();
			return true;
		}
	}
}
