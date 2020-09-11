using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var events = new List<Event>();

            for (var index = 1; index <= 10; index++)
            {
                var eventDate = new DateTime(2020, 9, index);
                events.Add(new Event(index, "Nome evento", eventDate, eventDate, false));
            }
            
            Calendar.Events = events;
        }
    }
}
