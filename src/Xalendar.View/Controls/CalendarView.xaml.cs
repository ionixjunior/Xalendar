using System;
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

        private void OnPreviousMonthClick(object sender, EventArgs e)
        {
            _monthContainer.Previous();
            
            BindableLayout.SetItemsSource(CalendarDaysContainer, _monthContainer.Days);
            MonthName.Text = _monthContainer.GetName();
        }

        private void OnNextMonthClick(object sender, EventArgs e)
        {
            _monthContainer.Next();
            
            BindableLayout.SetItemsSource(CalendarDaysContainer, _monthContainer.Days);
            MonthName.Text = _monthContainer.GetName();
        }
    }
}

