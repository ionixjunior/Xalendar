using Xalendar.Sample.ViewModels;
using Xamarin.Forms;

namespace Xalendar.Sample.Views
{
    public partial class AddingEvents : ContentPage
    {
        public AddingEvents()
        {
            InitializeComponent();
            BindingContext = new AddingEventsViewModel();
        }
    }
}
