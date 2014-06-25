using System;
using Umbraco.Web.Models;

namespace KS.Umbraco7.Calendar.Core
{
    public class CalendarEvent
    {
        public int recurrence { get; set; } // 1 - not recurring, 2 - repeat daily, 3 - repeat weekly, 4 - repeat monthly, 5 - repeat yearly
        public int weekInterval { get; set; } //If repeat weekly this tells you if its every week, every 2nd week, every 3rd week etc.
        public int monthYearOption { get; set; } //0  - Use startdate, 1 - Specify weekday etc. for event
        public int interval { get; set; } //interval of weekly events
        public int weekDay { get; set; } //weekDay for specified monthly and yearly events
        public int month { get; set; } //month for specified monthly and yearly events
        public int?[] days { get; set; } //array of days 0-6 for daily events
        public DateTime startDate { get; set; } //start date of the event
        public DateTime? endDate { get; set; } //end date of the event
        public DynamicPublishedContent content { get; set; } //the node containing the event
        public string Debug { get; set; } 
    }
}
