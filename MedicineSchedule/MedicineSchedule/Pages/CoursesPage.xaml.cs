using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.ViewModels;

namespace MedicineSchedule.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CoursesPage : ContentPage
	{
		public CoursesPage()
		{
			InitializeComponent();

			BindingContext = new CoursesListViewModel() {
				Navigation = Navigation
			};
		}
	}
}
