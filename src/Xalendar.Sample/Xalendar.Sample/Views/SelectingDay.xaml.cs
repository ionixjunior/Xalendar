using System;
using System.Collections.Generic;
using System.Linq;
using Xalendar.View.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.Sample.Views
{
    public partial class SelectingDay : ContentPage
    {
        public SelectingDay()
        {
            InitializeComponent();
        }

        private void OnDaySelected(DaySelected daySelected)
        {
            SelectedDay.Text = daySelected.DateTime.ToString("G");
            AmountEvents.Text = daySelected.Events.Count().ToString();
        }
    }
}
