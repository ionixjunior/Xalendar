using System;
using Xalendar.Api.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xalendar.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        public CalendarView()
        {
            InitializeComponent();

            BindingContext = new MonthContainer(DateTime.Today);
        }
    }
}

