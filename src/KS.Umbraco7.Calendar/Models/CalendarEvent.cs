using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Core.Models;

namespace KS.Umbraco7.Calendar.Models
{
    [DataContract]
    public class CalendarEvent
    {
        /// <summary>
        /// 1 - not recurring, 2 - repeat daily, 3 - repeat weekly, 4 - repeat monthly, 5 - repeat yearly
        /// </summary>
        [DataMember(Name = "recurrence")]
        public int Recurrence { get; set; }

        /// <summary>
        /// If repeat weekly this tells you if its every week, every 2nd week, every 3rd week etc.
        /// </summary>
        [DataMember(Name = "weekInterval")]
        public int WeekInterval { get; set; }

        /// <summary>
        /// 1 - Use startdate, 2 - Specify weekday etc. for event
        /// </summary>
        [DataMember(Name = "monthYearOption")]
        public int MonthYearOption { get; set; }

        /// <summary>
        /// 1 - Every month, 2 - Choose months
        /// </summary>
        [DataMember(Name = "monthOption")]
        public int? MonthOption { get; set; }

        /// <summary>
        /// Interval of weekly events
        /// </summary>
        [DataMember(Name = "interval")]
        public int Interval { get; set; }

        /// <summary>
        /// Week day for specified monthly and yearly events
        /// </summary>
        [DataMember(Name = "weekDay")]
        public int WeekDay { get; set; }

        /// <summary>
        /// Month for specified monthly and yearly events
        /// </summary>
        [DataMember(Name = "month")]
        public int Month { get; set; }

        /// <summary>
        /// Array of days 0-6 for daily events
        /// </summary>
        [DataMember(Name = "days")]
        public int?[] Days { get; set; } 

        /// <summary>
        /// Array of monts 1-12 for monthly events
        /// </summary>
        [DataMember(Name = "months")]
        public int?[] Months { get; set; }

        /// <summary>
        /// List of exception dates for a recurring event
        /// </summary>
        [DataMember(Name = "exceptDates")]
        public List<DateTime> ExceptDates { get; set; }

        /// <summary>
        /// Start date of the event
        /// </summary>
        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the event (endDate - startDate = duration)
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Last occurance of recurring event
        /// </summary>
        [DataMember(Name = "recurUntil")]
        public DateTime? RecurUntil { get; set; }

        /// <summary>
        /// Duration of event
        /// </summary>
        public TimeSpan? Duration { get; internal set; }

        /// <summary>
        /// The node containing the event
        /// </summary>
        [DataMember(Name = "content")]
        public IPublishedContent Content { get; set; }

        public CalendarEvent()
        {
            this.Duration = EndDate - StartDate;
        }
    }
}
