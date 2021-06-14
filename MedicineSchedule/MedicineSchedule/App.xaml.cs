using Xamarin.Forms;

using Plugin.LocalNotification;

using MedicineSchedule.Pages;

namespace MedicineSchedule
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			NotificationCenter.Current.NotificationReceived += OnLocalNotificationReceived;
			NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

			MainPage = new MainPage();
		}

		private void OnLocalNotificationReceived(NotificationReceivedEventArgs e)
		{
			MainPage.Navigation.PushModalAsync(new NotificationPage());
		}

		private void OnLocalNotificationTapped(NotificationTappedEventArgs e)
		{
			MainPage.Navigation.PushModalAsync(new NotificationPage());
		}
	}
}
