using System;
using Xalendar.Api.Extensions;
using Xalendar.Api.Models;
using Xalendar.View.ViewModels;
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

            BindingContext = new CalendarViewModel();
        }
    }
}

