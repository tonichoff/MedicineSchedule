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
				DaysCountEntry.Text = "1";
				ReceptionsCountEntry.Text = "1";
				DaysModPicker.SelectedIndex = 0;
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
			}
		}

		private void ReceptionModeChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			DaysCountEntry.IsVisible = RecModPicker.SelectedIndex == 1;
			DaysCountErrorMsg.IsVisible = RecModPicker.SelectedIndex == 1;

			ReceptionsCountEntry.IsVisible = RecModPicker.SelectedIndex == 2;
			ReceptionsCountErrorMsg.IsVisible = RecModPicker.SelectedIndex == 2;
		}

		private void CheckDaysCountEntry(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(DaysCountEntry.Text)) {
				DaysCountErrorMsg.Text = "Вы забыли ввести число";
				DaysCountErrorMsg.IsVisible = true;
			} else if (!int.TryParse(DaysCountEntry.Text, out int count) || count <= 0) {
				DaysCountErrorMsg.Text = "Введите целое число больше 0";
				DaysCountErrorMsg.IsVisible = true;
			} else {
				DaysCountErrorMsg.Text = "";
				DaysCountErrorMsg.IsVisible = false;
			}
		}

		private void CheckReceptionsCountEntry(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(ReceptionsCountEntry.Text)) {
				ReceptionsCountErrorMsg.Text = "Вы забыли ввести число";
				ReceptionsCountErrorMsg.IsVisible = true;
			} else if (!int.TryParse(ReceptionsCountEntry.Text, out int count) || count <= 0) {
				ReceptionsCountErrorMsg.Text = "Введите целое число больше 0";
				ReceptionsCountErrorMsg.IsVisible = true;
			} else {
				ReceptionsCountErrorMsg.Text = "";
				ReceptionsCountErrorMsg.IsVisible = false;
			}
		}

		private void DaysModeChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			IntervalEntry.IsVisible = DaysModPicker.SelectedIndex == 1;
			IntervalErrorMsg.IsVisible = DaysModPicker.SelectedIndex == 1;
		}

		private void CheckIntervalEntry(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(IntervalEntry.Text)) {
				IntervalErrorMsg.Text = "Вы забыли ввести число";
				IntervalErrorMsg.IsVisible = true;
			} else if (!int.TryParse(IntervalEntry.Text, out int count) || count <= 0) {
				IntervalErrorMsg.Text = "Введите целое число больше 0";
				IntervalErrorMsg.IsVisible = true;
			} else {
				IntervalErrorMsg.Text = "";
				IntervalErrorMsg.IsVisible = false;
			}
		}
	}
}
