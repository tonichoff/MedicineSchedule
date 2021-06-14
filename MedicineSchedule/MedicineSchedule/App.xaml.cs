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

			NotificationCenter.Current.NotificationReceived += (eventArgs) => {
				ShowNotificationPage(eventArgs.Request.NotificationId);
			};
			NotificationCenter.Current.NotificationTapped += (eventArgs) => {
				ShowNotificationPage(eventArgs.Request.NotificationId);
			};

			MainPage = new MainPage();

			void ShowNotificationPage(int notificationId)
			{
				Device.BeginInvokeOnMainThread(() => {
					MainPage.Navigation.PushModalAsync(new NotificationPage(notificationId));
				});
			}
		}
	}
}
