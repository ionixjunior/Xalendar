using System;
using System.Collections.Generic;
using Xalendar.Sample.Models;
using Xalendar.Sample.Views;

namespace Xalendar.Sample.ViewModels
{
    public class ChoosingThemeViewModel
    {
        public IList<SamplePage> Pages { get; }

        public ChoosingThemeViewModel()
        {
            Pages = new List<SamplePage>
            {
                new SamplePage { Name = "Planning theme", Type = typeof(PlanningTheme) },
                new SamplePage { Name = "Task Management theme", Type = typeof(TaskManagementTheme) },
                new SamplePage { Name = "Amazonas theme", Type = typeof(AmazonasTheme) },
                new SamplePage { Name = "Custom theme", Type = typeof(CustomTheme) }
            };
        }
    }
}
