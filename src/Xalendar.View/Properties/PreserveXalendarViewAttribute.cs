using System;
using System.ComponentModel;

namespace Xalendar.View
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class PreserveXalendarViewAttribute : Attribute
    {
    }
}
