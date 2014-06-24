using System;
using Umbraco.Web.Models;

namespace KS.Umbraco7.Calendar.Core
{
    public class CalendarEvent
    {
        public int recursive { get; set; }
        public int weekInterval { get; set; }
        public int monthYearOption { get; set; }
        public int interval { get; set; }
        public int weekDay { get; set; }
        public int month { get; set; }
        public int?[] days { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DynamicPublishedContent content { get; set; }
        public string Debug { get; set; }
    }
}
