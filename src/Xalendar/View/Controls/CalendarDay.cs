using System;
using System.Collections.Generic;
using Xalendar.Api.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Xalendar.View.Controls
{
    internal class CalendarDay : ContentView
    {
        public event Action<CalendarDay?>? DaySelected;
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
            tap.Tapped += OnDaySelected;
            GestureRecognizers.Add(tap);
        }
        
        private void OnDaySelected(object _, EventArgs __) => DaySelected?.Invoke(this);

        public void Select()
        {
            const string state = "Selected";
            VisualStateManager.GoToState(_dayFrame, state);
            VisualStateManager.GoToState(_dayElement, state);
        }

        public void UnSelect() => StartState();

        internal void StartState()
        {
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

                if (Day.IsWeekend)
                    return "IsWeekend";

                if (Day.HasEvents)
                    return "HasEvents";
            }

            return "IsNotPreview";
        }

        internal void UpdateData(Day? day)
        {
            Day = day;
            _hasEventsElement.IsVisible = day?.HasEvents ?? false;
            _dayElement.Text = day?.ToString();
            StartState();
        }
    }
}

