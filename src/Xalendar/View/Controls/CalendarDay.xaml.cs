using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xalendar.Api.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xalendar.View.Controls
{
    public partial class CalendarDay : ContentView
    {

        public event Action<CalendarDay?>? DaySelected;
        public Day? Day { get; set; }
        
        public CalendarDay()
        {
            InitializeComponent();
        }
        
        private void OnDaySelected(object _, EventArgs __) => DaySelected?.Invoke(this);

        public void Select()
        {
            const string state = "Selected";
            VisualStateManager.GoToState(DayFrame, state);
            VisualStateManager.GoToState(DayElement, state);
        }

        public void UnSelect() => StartState();

        internal void StartState()
        {
            VisualStateManager.GoToState(DayFrame, GetStateOfDayFrame());
            VisualStateManager.GoToState(DayElement, GetStateOfDayElement());
        }

        private string GetStateOfDayFrame()
        {
            if (Day is { })
            {
                if (Day.IsToday)
                    return "IsToday";

                if (Day.HasEvents)
                    return "HasEvents";
            }

            return "UnSelected";
        }

        private string GetStateOfDayElement()
        {
            if (Day is { })
            {
                if (Day.IsToday)
                    return "IsToday";

                if (Day.IsWeekend)
                    return "IsWeekend";

                if (Day.HasEvents)
                    return "HasEvents";

                if (Day.IsPreview)
                    return "IsPreview";
            }

            return "IsNotPreview";
        }

        internal void UpdateData(Day? day)
        {
            Day = day;
            HasEventsElement.IsVisible = day?.HasEvents ?? false;
            DayElement.Text = day?.ToString();

            if (day is { })
                AutomationProperties.SetName(DayFrame, GetTextForAccessibility(day));

            StartState();
        }

        private string GetTextForAccessibility(Day day)
        {
            var text = new StringBuilder(day.DateTime.ToString("D"));

            if (day.HasEvents)
                text.Append(", there are events in this day");

            return text.ToString();
        }
    }
}

