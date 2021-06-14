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
				pickersCount = 1;
			} else {
				CreateBtn.IsVisible = false;
				pickersCount = ViewModel.Course.ReceptionsInDayCount;
			}

			for (int i = 0; i < pickersCount; ++i) {
				timePickers[i].IsVisible = true;
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
				UpdateTimesPickers(count);
				ReceptionsInDayErrorMsg.Text = "";
				ReceptionsInDayErrorMsg.IsVisible = false;
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
			IntervalLabel.IsVisible = DaysModPicker.SelectedIndex == 1;
			IntervalEntry.IsVisible = DaysModPicker.SelectedIndex == 1;
			IntervalErrorMsg.IsVisible = DaysModPicker.SelectedIndex == 1;

			DaysSkipLabel.IsVisible = DaysModPicker.SelectedIndex == 1;
			DaysSkipEntry.IsVisible = DaysModPicker.SelectedIndex == 1;
			DaysSkipErrorMsg.IsVisible = DaysModPicker.SelectedIndex == 1;
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

		private void CheckDaysSkipEntry(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(DaysSkipEntry.Text)) {
				DaysSkipErrorMsg.Text = "Вы забыли ввести число";
				DaysSkipErrorMsg.IsVisible = true;
			} else if (!int.TryParse(DaysSkipEntry.Text, out int count) || count <= 0) {
				DaysSkipErrorMsg.Text = "Введите целое число больше 0";
				DaysSkipErrorMsg.IsVisible = true;
			} else {
				DaysSkipErrorMsg.Text = "";
				DaysSkipErrorMsg.IsVisible = false;
			}
		}
	}
}
