using System;
using System.Collections.Generic;
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
            var result = await Task.Run(() =>
            {
                _monthContainer.Previous();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();

                return (days, monthName);
            });

            MonthName.Text = result.monthName;
            RecycleDays(result.days);
        }

        private async void OnNextMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                _monthContainer.Next();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();

                return (days, monthName);
            });
            
            MonthName.Text = result.monthName;
            RecycleDays(result.days);
        }

        private void RecycleDays(IReadOnlyList<Day?> days)
        {
            for (var index = 0; index < CalendarDaysContainer.Children.Count; index++)
            {
                var dayContainer = days[index];
                var dayView = CalendarDaysContainer.Children[index];
                dayView.BindingContext = dayContainer;
            }
        }
    }
}

