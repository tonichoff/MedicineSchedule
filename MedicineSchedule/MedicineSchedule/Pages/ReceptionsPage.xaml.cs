using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.ViewModels;

namespace MedicineSchedule.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReceptionsPage : ContentPage
	{
		private ReceptionsListViewModel viewModel;

		public ReceptionsPage()
		{
			InitializeComponent();

			viewModel = new ReceptionsListViewModel() {
				Navigation = Navigation
			};

			BindingContext = viewModel;
		}
	}
}