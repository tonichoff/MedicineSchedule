using Xamarin.Forms;

using Plugin.LocalNotification;

using MedicineSchedule.Pages;

namespace MedicineSchedule
{
	public partial class App : Application
	{
		private bool IsSleep;

		public App()
		{
			InitializeComponent();

			NotificationCenter.Current.NotificationReceived += (eventArgs) => {
				if (!IsSleep) {
					ShowNotificationPage(eventArgs.Request.NotificationId);
				}
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

		protected override void OnSleep()
		{
			base.OnSleep();
			IsSleep = true;
		}

		protected override void OnResume()
		{
			base.OnResume();
			IsSleep = false;
		}
	}
}
