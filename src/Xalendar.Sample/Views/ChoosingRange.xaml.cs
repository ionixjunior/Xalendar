using Xalendar.Sample.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.Sample.Views
{
    public partial class ChoosingRange : ContentPage
    {
        public ChoosingRange()
        {
            InitializeComponent();
            BindingContext = new ChoosingRangeViewModel();
        }

        private void OnDayTapped(Xalendar.Api.Models.DayTapped _)
        {
        }
    }
}
