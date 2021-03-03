﻿using System;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.View.Controls
{
    public partial class CalendarDayOfWeek : Label
    {
        public CalendarDayOfWeek()
        {
            InitializeComponent();
        }

        internal void UpdateData(DayOfWeekName dayOfWeekName)
        {
            Text = dayOfWeekName.Name;
            VisualStateManager.GoToState(this, GetState(dayOfWeekName.DayOfWeek));
        }

        private string GetState(DayOfWeek dayOfWeek)
        {
            if (IsWeekend(dayOfWeek))
                return "IsWeekend";

            return "IsNotWeekend";
        }

        private bool IsWeekend(DayOfWeek dayOfWeek) => dayOfWeek switch
        {
            DayOfWeek.Saturday => true,
            DayOfWeek.Sunday => true,
            _ => false
        };
    }
}
