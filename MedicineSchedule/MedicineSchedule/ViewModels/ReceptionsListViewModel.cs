using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;

using MedicineSchedule.Models;
using MedicineSchedule.Services;

namespace MedicineSchedule.ViewModels
{
	public class ReceptionsListViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Reception> Receptions { get; set; } =
			new ObservableCollection<Reception>();

		public Command LoadReceptionsCommand { get; set; }

		public INavigation Navigation { get; set; }

		public DateTime Date
		{
			get => dateTime;
			set
			{
				dateTime = value;
				UpdateReceptions();
			}
		}

		public bool IsUpdating
		{
			get => isUpdating;
			set
			{
				isUpdating = value;
				OnPropertyChanged("IsUpdating");
			}
		}

		private DateTime dateTime;
		private bool isUpdating;

		private readonly DataBase dataBase = new DataBase();

		public ReceptionsListViewModel()
		{
			Date = DateTime.Now;
			LoadReceptionsCommand = new Command(async () => await ExecuteLoadReceptionsCommand());
		}

		private async Task ExecuteLoadReceptionsCommand()
		{
			IsUpdating = true;
			try {
				UpdateReceptions();
			}
			catch (Exception exception) {
				Debug.WriteLine(
					$"Thrown exception while trying load courses in CoursesList. Exception message: {exception.Message}"
				);
			}
			finally {
				IsUpdating = false;
			}
		}

		private async void UpdateReceptions()
		{
			Receptions.Clear();
			var receptionTask = dataBase.GetReceptionsByDate(Date);
			receptionTask.Wait();
			foreach (var reception in receptionTask.Result) {
				Receptions.Add(reception);
			}
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
