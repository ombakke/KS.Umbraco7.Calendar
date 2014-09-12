using System;
using System.Collections.Generic;
using Umbraco.Web.Models;

namespace KS.Umbraco7.Calendar.Core
{
    public class CalendarEvent
    {
        public int recurrence { get; set; } // 1 - not recurring, 2 - repeat daily, 3 - repeat weekly, 4 - repeat monthly, 5 - repeat yearly
        public int weekInterval { get; set; } //If repeat weekly this tells you if its every week, every 2nd week, every 3rd week etc.
        public int monthYearOption { get; set; } //1  - Use startdate, 2 - Specify weekday etc. for event
        public int? monthOption { get; set; } // 1 - Evert month, 2 - Choose months
        public int interval { get; set; } //interval of weekly events
        public int weekDay { get; set; } //weekDay for specified monthly and yearly events
        public int month { get; set; } //month for specified monthly and yearly events
        public int?[] days { get; set; } //array of days 0-6 for daily events
        public int?[] months { get; set; }//array of monts 1-12 for monthly events
        public List<DateTime> exceptDates { get; set; } //List of exception dates for a recurring event
        public DateTime startDate { get; set; } //start date of the event
        public DateTime? endDate { get; set; } //end date of the event (endDate - startDate = duration)
        public DateTime? recurUntil { get; set; } //last occurance of recurring event
        public DynamicPublishedContent content { get; set; } //the node containing the event
    }
}
