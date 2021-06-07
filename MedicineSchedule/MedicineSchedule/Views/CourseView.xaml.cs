using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.ViewModels;
using System.ComponentModel;
using System;

namespace MedicineSchedule.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CourseView : ContentPage
	{
		public CourseViewModel ViewModel { get; private set; }

		public CourseView(CourseViewModel viewModel)
		{
			ViewModel = viewModel;

			InitializeComponent();
			
			ViewModel.ParentPage = this;
			BindingContext = ViewModel;
			if (ViewModel.Course == null) {
				DeleteBtn.IsVisible = false;
				SaveBtn.IsVisible = false;
				MedPicker.SelectedIndex = 0;
				FoodPicker.SelectedIndex = 0;
				MesPicker.SelectedIndex = 0;
				RecModPicker.SelectedIndex = 0;
			} else {
				CreateBtn.IsVisible = false;
			}
		}

		private void CheckCountEntry(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(ReceptionsInDayEntry.Text)) {
				ReceptionsInDayErrorMsg.Text = "Вы забыли ввести число";
				ReceptionsInDayErrorMsg.IsVisible = true;
			} else if (!int.TryParse(ReceptionsInDayEntry.Text, out int count)) {
				ReceptionsInDayErrorMsg.Text = "Введите целое число";
				ReceptionsInDayErrorMsg.IsVisible = true;
			} else if (count < 1 || count > 12) {
				ReceptionsInDayErrorMsg.Text = "Введите число от 1 до 12";
				ReceptionsInDayErrorMsg.IsVisible = true;
			} else {
				ReceptionsInDayErrorMsg.Text = "";
				ReceptionsInDayErrorMsg.IsVisible = false;
				ViewModel.ReceptionsInDay = count.ToString();
			}
		}

		private void ReceptionModeChanged(object sender, PropertyChangedEventArgs eventArgs)
		{

		}
	}
}
