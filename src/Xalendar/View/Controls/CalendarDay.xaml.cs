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

        public void Select() => VisualStateManager.GoToState(DayFrame, "Selected");

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
            }

            return "IsNotPreview";
        }
    }
}

