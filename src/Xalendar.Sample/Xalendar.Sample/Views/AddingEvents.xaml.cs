using Xalendar.Sample.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

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
