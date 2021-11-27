using System;
using System.Collections.Generic;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.View.Controls
{
    internal class CalendarDay : ContentView
    {
        public event Action<CalendarDay?>? DayTapped;
        internal Day? Day { get; set; }

        private Label _dayElement;
        private BoxView _hasEventsElement;
        private Frame _dayFrame;
        
        internal CalendarDay()
        {
            _dayElement = new Label
            {
                StyleClass = new List<string> { "CalendarDayNumber" },
            };

            _hasEventsElement = new BoxView
            {
                StyleClass = new List<string> { "CalendarDayEvent" },
            };

            _dayFrame = new Frame
            {
                StyleClass = new List<string> { "CalendarDay" },
                Content = new FlexLayout
                {
                    StyleClass = new List<string> { "CalendarDayLayout" },
                    Children =
                    {
                        _dayElement,
                        _hasEventsElement
                    }
                }
            };

            Content = _dayFrame;

            var tap = new TapGestureRecognizer();
            tap.Tapped += OnDayTapped;
            GestureRecognizers.Add(tap);
        }

        private void OnDayTapped(object _, EventArgs __)
        {
            if (Day is { IsInRange: true })
                DayTapped?.Invoke(this);
        }

        public void Select()
        {
            Day?.Select();
            const string state = "Selected";
            VisualStateManager.GoToState(_dayFrame, state);
            VisualStateManager.GoToState(_dayElement, state);
        }

        public void UnSelect() => StartState();

        internal void StartState()
        {
            Day?.UnSelect();
            VisualStateManager.GoToState(_dayFrame, GetStateOfDayFrame());
            VisualStateManager.GoToState(_dayElement, GetStateOfDayElement());
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

                if (Day.IsInRange == false)
                    return "IsNotInRange";

                if (Day.IsWeekend)
                    return "IsWeekend";

                if (Day.HasEvents)
                    return "HasEvents";
            }

            return "IsNotPreview";
        }

        internal void UpdateData(Day? day, bool shouldSelect)
        {
            Day = day;
            _hasEventsElement.IsVisible = day?.HasEvents ?? false;
            _dayElement.Text = day?.ToString();
            
            if (shouldSelect)
                Select();
            else
                StartState();
        }

        public void SwitchSelectedState()
        {
            if (Day?.IsSelected ?? false)
                UnSelect();
            else
                Select();
        }
    }
}

