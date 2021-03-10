using System;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.View.Controls
{
    public partial class CalendarDay : ContentView
    {

        public event Action<CalendarDay?>? DaySelected;
        internal Day? Day { get; set; }
        
        internal CalendarDay()
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

                if (Day.IsPreview)
                    return "IsPreview";

                if (Day.IsWeekend)
                    return "IsWeekend";

                if (Day.HasEvents)
                    return "HasEvents";
            }

            return "IsNotPreview";
        }
    }
}

