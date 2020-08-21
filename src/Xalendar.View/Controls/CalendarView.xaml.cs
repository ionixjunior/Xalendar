using System;
using System.Threading.Tasks;
using Xalendar.Api.Extensions;
using Xalendar.Api.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xalendar.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        private readonly MonthContainer _monthContainer;
        
        public CalendarView()
        {
            InitializeComponent();
            
            _monthContainer = new MonthContainer(DateTime.Today);
            BindableLayout.SetItemsSource(CalendarDaysContainer, _monthContainer.Days);
            BindableLayout.SetItemsSource(CalendarDaysOfWeekContainer, _monthContainer.DaysOfWeek);
            MonthName.Text = _monthContainer.GetName();
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            await Task.Run(() => _monthContainer.Previous());
            
            BindableLayout.SetItemsSource(CalendarDaysContainer, _monthContainer.Days);
            MonthName.Text = _monthContainer.GetName();
        }

        private async void OnNextMonthClick(object sender, EventArgs e)
        {
            await Task.Run(() => _monthContainer.Next());

            BindableLayout.SetItemsSource(CalendarDaysContainer, _monthContainer.Days);
            MonthName.Text = _monthContainer.GetName();
        }
    }
}

