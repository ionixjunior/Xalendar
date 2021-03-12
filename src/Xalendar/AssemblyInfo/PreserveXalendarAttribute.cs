using System;
using System.ComponentModel;

namespace Xalendar
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class PreserveXalendarAttribute : Attribute
    {
    }
}
