using System;
using System.Collections.Generic;
using Xalendar.View.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.Sample.Views
{
    public partial class ChangingMonth : ContentPage
    {
        public ChangingMonth()
        {
            InitializeComponent();
        }

        private void OnMonthChanged(MonthRange monthRange)
        {
            StartDate.Text = monthRange.Start.ToString("G");
            EndDate.Text = monthRange.End.ToString("G");
        }
    }
}
