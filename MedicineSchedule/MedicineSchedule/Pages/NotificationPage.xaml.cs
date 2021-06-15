using System;
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
		private NextReceptionInfo receptionInfo;
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

		private void SetDescription(int receptionid)
		{
			receptionInfo = dataBase.GetNextReceptionAtId(receptionid).Result;
			course =  dataBase.GetCourseAtId(receptionInfo.CourseId).Result;
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
			});
		}
	}
}
