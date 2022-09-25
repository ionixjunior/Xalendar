using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.Sample.Views
{
    public partial class SelectingDaySingle : ContentPage
    {
        public SelectingDaySingle()
        {
            InitializeComponent();
        }

        private void OnDayTapped(Xalendar.Api.Models.DayTapped dayTapped)
        {
            TappedDay.Text = dayTapped.DateTime.ToString("G");
            State.Text = dayTapped.State.ToString();
            AmountEvents.Text = dayTapped.Events.Count().ToString();
        }
    }
}
