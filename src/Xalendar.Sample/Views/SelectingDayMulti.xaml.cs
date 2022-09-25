using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.Sample.Views
{
    public partial class SelectingDayMulti : ContentPage
    {
        public SelectingDayMulti()
        {
            InitializeComponent();
        }

        private void OnDayTapped(Xalendar.Api.Models.DayTapped dayTapped)
        {
            TappedDay.Text = dayTapped.DateTime.ToString("G");
            State.Text = dayTapped.State.ToString();
            AmountEvents.Text = dayTapped.Events.Count().ToString();
            
            SelectedDays.Children.Clear();
            foreach (var dateTime in Calendar.SelectedDates)
                SelectedDays.Children.Add(new Label { Text = dateTime.ToString("G") });
        }
    }
}
