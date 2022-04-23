using Xalendar.Sample.ViewModels;
using Xamarin.Forms;

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
