using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.ViewModels;

namespace MedicineSchedule.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CoursesPage : ContentPage
	{
		private CoursesListViewModel viewModel;

		public CoursesPage()
		{
			InitializeComponent();

			viewModel = new CoursesListViewModel() {
				Navigation = Navigation
			};

			BindingContext = viewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.OnAppearing();
		}
	}
}
