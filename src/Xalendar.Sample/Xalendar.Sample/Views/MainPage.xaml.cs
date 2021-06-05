using System;
using Xalendar.Sample.Models;
using Xalendar.Sample.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.Sample.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
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
