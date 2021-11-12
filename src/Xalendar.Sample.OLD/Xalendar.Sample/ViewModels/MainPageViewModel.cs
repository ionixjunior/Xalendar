using System.Collections.Generic;
using Xalendar.Sample.Models;
using Xalendar.Sample.Views;

namespace Xalendar.Sample.ViewModels
{
    public class MainPageViewModel
    {
        public IList<SamplePage> Pages { get; }

        public MainPageViewModel()
        {
            Pages = new List<SamplePage>
            {
                new SamplePage { Name = "Basic sample", Type = typeof(Basic) },
                new SamplePage { Name = "Adding events", Type = typeof(AddingEvents) },
                new SamplePage { Name = "Changing month", Type = typeof(ChangingMonth) },
                new SamplePage { Name = "Selecting day (legacy)", Type = typeof(SelectingDayLegacy) },
                new SamplePage { Name = "Selecting day - single mode", Type = typeof(SelectingDaySingle) },
                new SamplePage { Name = "Selecting day - multi mode", Type = typeof(SelectingDayMulti) },
                new SamplePage { Name = "Choosing theme", Type = typeof(ChoosingTheme) }
            };
        }
    }
}
