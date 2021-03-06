﻿using System;
using System.Collections.Generic;
using Xalendar.Sample.Models;
using Xalendar.Sample.ViewModels;
using Xamarin.Forms;

namespace Xalendar.Sample.Views
{
    public partial class ChoosingTheme : ContentPage
    {
        public ChoosingTheme()
        {
            InitializeComponent();
            BindingContext = new ChoosingThemeViewModel();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is SamplePage samplePage)
            {
                if (Activator.CreateInstance(samplePage.Type) is Page page)
                {
                    page.Title = samplePage.Name;
                    await Navigation.PushAsync(page);
                }
            }

            if (sender is ListView listView)
                listView.SelectedItem = null;
        }
    }
}
