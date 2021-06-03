using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MedicineSchedule.ViewModels;

namespace MedicineSchedule.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CourseView : ContentPage
	{
		public CourseViewModel Course { get; private set; }

		public CourseView(CourseViewModel course)
		{
			InitializeComponent();

			Course = course;
			BindingContext = Course;
		}
	}
}
