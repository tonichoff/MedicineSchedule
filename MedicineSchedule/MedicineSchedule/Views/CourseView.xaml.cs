using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.ViewModels;

namespace MedicineSchedule.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CourseView : ContentPage
	{
		public CourseViewModel ViewModel { get; private set; }

		private int pickersCount;
		private readonly List<TimePicker> timePickers = new List<TimePicker>();

		public CourseView(CourseViewModel viewModel)
		{
			ViewModel = viewModel;

			InitializeComponent();

			for (int i = 0; i < 12; ++i) {
				var newTimePicker = new TimePicker() { Time = new TimeSpan(12, 0, 0) };
				timePickers.Add(newTimePicker);
				ReceptionsFrameLayout.Children.Add(timePickers[i]);
				timePickers[i].IsVisible = false;
				timePickers[i].Time = ViewModel.Times[i];
				int index = i;
				timePickers[i].PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
					var picker = (TimePicker)sender;
					ViewModel.Times[index] = picker.Time;
				};
			}

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
				pickersCount = ViewModel.Course.ReceptionsInDayCount;
			}

			for (int i = 0; i < pickersCount; ++i) {
				timePickers[i].IsVisible = true;
			}
		}

		private void UpdateTimesPickers(int newCount)
		{
			if (newCount > pickersCount) {
				for (int i = pickersCount; i < newCount; ++i) {
					timePickers[i].IsVisible = true;
				}
			} else if (newCount < pickersCount) {
				for (int i = newCount; i < pickersCount; ++i) {
					timePickers[i].IsVisible = false;
				}
			}
			pickersCount = newCount;
		}

		private void ReceptionModeChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			DaysCountEntry.IsVisible = RecModPicker.SelectedIndex == 1;
			DaysCountErrorMsg.IsVisible = RecModPicker.SelectedIndex == 1;

			ReceptionsCountEntry.IsVisible = RecModPicker.SelectedIndex == 2;
			ReceptionsCountErrorMsg.IsVisible = RecModPicker.SelectedIndex == 2;
		}

		private void DaysModeChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			IntervalLabel.IsVisible = DaysModPicker.SelectedIndex == 1;
			IntervalEntry.IsVisible = DaysModPicker.SelectedIndex == 1;
			IntervalErrorMsg.IsVisible = DaysModPicker.SelectedIndex == 1;

			DaysSkipLabel.IsVisible = DaysModPicker.SelectedIndex == 1;
			DaysSkipEntry.IsVisible = DaysModPicker.SelectedIndex == 1;
			DaysSkipErrorMsg.IsVisible = DaysModPicker.SelectedIndex == 1;
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
				UpdateTimesPickers(count);
				ReceptionsInDayErrorMsg.Text = "";
				ReceptionsInDayErrorMsg.IsVisible = false;
			}
		}

		private void CheckMedicineValueEntry(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(MedicineValueEntry.Text)) {
				MedicineValueErrorMsg.Text = "Вы забыли ввести число";
				MedicineValueErrorMsg.IsVisible = true;
			} else if (!double.TryParse(MedicineValueEntry.Text, out double _)) {
				MedicineValueErrorMsg.Text = "Введите целое или дробное число";
				MedicineValueErrorMsg.IsVisible = true;
			} else {
				MedicineValueErrorMsg.Text = "";
				MedicineValueErrorMsg.IsVisible = false;
			}
		}

		private void CheckDaysCountEntry(object sender, EventArgs eventArgs)
		{
			CheckTextEntry(DaysCountEntry, DaysCountErrorMsg);
		}

		private void CheckReceptionsCountEntry(object sender, EventArgs eventArgs)
		{
			CheckTextEntry(ReceptionsCountEntry, ReceptionsCountErrorMsg);
		}

		private void CheckIntervalEntry(object sender, EventArgs eventArgs)
		{
			CheckTextEntry(IntervalEntry, IntervalErrorMsg);
		}

		private void CheckDaysSkipEntry(object sender, EventArgs eventArgs)
		{
			CheckTextEntry(DaysSkipEntry, DaysSkipErrorMsg);
		}

		private void CheckTextEntry(Entry entry, Label errorLabel)
		{
			if (string.IsNullOrEmpty(entry.Text)) {
				errorLabel.Text = "Вы забыли ввести число";
				errorLabel.IsVisible = true;
			} else if (!int.TryParse(entry.Text, out int count) || count <= 0) {
				errorLabel.Text = "Введите целое число больше 0";
				errorLabel.IsVisible = true;
			} else {
				errorLabel.Text = "";
				errorLabel.IsVisible = false;
			}
		}
	}
}
