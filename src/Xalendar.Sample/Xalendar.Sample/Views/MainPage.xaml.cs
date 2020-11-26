using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xalendar.Api.Interfaces;
using Xalendar.Sample.ViewModels;
using Xalendar.View.Controls;
using Xamarin.Forms;

namespace Xalendar.Sample.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
        
        private void OnRandomButtonClick(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.AddRandomEvent();
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.RemoveEvent();
        }

        private void OnMonthChanged(MonthRange args)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.GetEventsByRange(args.Start, args.End);
        }

        private void OnDaySelected(DaySelected args)
        {
            System.Diagnostics.Debug.WriteLine($"Dia selecionado: {args.DateTime}; Eventos: {args.Events.Count()}");
        }
    }
}
