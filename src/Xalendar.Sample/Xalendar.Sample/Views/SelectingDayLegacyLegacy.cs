﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xalendar.View.Controls;
using Xamarin.Forms;

namespace Xalendar.Sample.Views
{
    public partial class SelectingDayLegacy : ContentPage
    {
        public SelectingDayLegacy()
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
