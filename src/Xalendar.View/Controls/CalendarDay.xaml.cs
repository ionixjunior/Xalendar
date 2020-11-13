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
        public event Action<CalendarDay?> DaySelected;
        public Day? Day { get; set; }
        
        public CalendarDay()
        {
            InitializeComponent();
        }
        
        private void OnDaySelected(object _, EventArgs __) => DaySelected?.Invoke(this);

        public void Select() => VisualStateManager.GoToState(this, "Selected");

        public void UnSelect() => VisualStateManager.GoToState(this, "UnSelected");
    }
}

