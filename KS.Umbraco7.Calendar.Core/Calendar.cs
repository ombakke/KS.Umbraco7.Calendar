using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace KS.Umbraco7.Calendar.Core
{
    /// <summary>
    /// Functions for getting calendar-events
    /// </summary>
    /// 
    public static class Calendar
    {
        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).TypedContentAtRoot().First());
            var nodes = node.Descendants();
            return getEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="documentType">Alias of the document type</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).TypedContentAtRoot().First());
            return getEventList(startDate, endDate, propertyType, node.Descendants(documentType), splitNoneRecurring);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="documentType">Alias of the document type</param>
        ///<param name="startNode">DynamicPublishedConent to look for events in</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType, DynamicPublishedContent startNode, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var nodes = startNode.Descendants(documentType);
            return getEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="documentType">Alias of the document type</param>
        ///<param name="nodeId">Id of node to look for events in descendants</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType, int nodeId, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();dynamic node;
            try
            {
                node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).Content(nodeId));
            }
            catch (Exception ex) {
                node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).TypedContentAtRoot().First());
            }
            if (node != null)
            {
                var nodes = node.Descendants(documentType);
                return getEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
            }
            return null;
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="startNode">DynamicPublishedConent to look for events in</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, DynamicPublishedContent startNode, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var nodes = startNode.Descendants();
            return getEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="nodeId">Id of node to look for events in descendants</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, int nodeId, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            dynamic node;
            try
            {
                node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).Content(nodeId));
            }
            catch (Exception ex) {
                node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).TypedContentAtRoot().First());
            }
            if (node != null)
            {
                //var node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).Content(nodeId));
                var nodes = node.Descendants();
                return getEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
            }
            return null;
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="nodes">DynamicPublishedContentlist holding the nodes where we will be looking for events</param>
        ///
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        private static List<CalendarEvent> getEventList(DateTime startDate, DateTime endDate, string propertyType, DynamicPublishedContentList nodes, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            foreach (var node in nodes)
            {
                if (node.HasValue(propertyType))
                {
                    CalendarEvent e = Newtonsoft.Json.JsonConvert.DeserializeObject<CalendarEvent>(node.GetPropertyValue(propertyType).ToString());
                    if (e.exceptDates == null) {
                        e.exceptDates = new List<DateTime>();
                    }
                    if (e.days == null) { 
                        e.days = new int?[0];
                    }
                    if (e.months == null) {
                        e.months = new int?[0];
                    }
                    if ((startDate <= e.startDate || (e.recurrence > 1 && !(e.recurUntil.HasValue && e.recurUntil.Value <= startDate)) && e.startDate <= endDate))
                    {
                        int durationMinutes = 0;
                        if (e.endDate.HasValue) {
                            durationMinutes = (int)e.endDate.Value.Subtract(e.startDate).TotalMinutes;
                        }

                        DateTime eEndDate = (!e.recurUntil.HasValue ? endDate : (e.recurUntil.Value < endDate ? e.recurUntil.Value.AddDays(1).AddSeconds(-1) : endDate));
                        e.content = node;
                        switch (e.recurrence)
                        {
                            case 1:
                                //no recurrence
                                if (e.endDate.HasValue && e.startDate.Date < e.endDate.Value.Date && splitNoneRecurring)
                                {
                                    //event spanning several days
                                    DateTime dSDate = startDate.Date <= e.startDate.Date ? e.startDate.Date : startDate.Date;
                                    for (DateTime d = dSDate; d <= e.endDate.Value.Date ; d = d.AddDays(1))
                                    {
                                        CalendarEvent ce = new CalendarEvent();
                                        if (e.startDate.Date < d.Date)
                                        {
                                            ce.startDate = d.Date;
                                        }
                                        else {
                                            //TimeSpan ts = d.Date - e.startDate;
                                            //ce.startDate = e.startDate.AddDays(ts.Days *-1);
                                            ce.startDate = e.startDate;
                                        }
                                        if (d.Date < e.endDate.Value.Date)
                                        {
                                            ce.endDate = d.Date.AddDays(1).AddSeconds(-1);
                                        }
                                        else {
                                            ce.endDate = e.endDate.Value;
                                        }
                                        ce.recurrence = e.recurrence;
                                        ce.content = e.content;
                                        events.Add(ce);
                                    }
                                }
                                else
                                {
                                    events.Add(e);
                                }
                                break;
                            case 2:
                                //repeat daily
                                DateTime dStartDate = startDate.Date <= e.startDate.Date ? e.startDate.Date : startDate.Date;
                                //loop through all days from startdate to enddate
                                for (DateTime d = dStartDate; d <= eEndDate; d = d.AddDays(1))
                                {
                                    //if the event is selected for the actual day, add it to the list
                                    if (e.days.Contains((int)d.DayOfWeek))
                                    {
                                        CalendarEvent ce = new CalendarEvent();
                                        ce.recurrence = e.recurrence;
                                        ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                        ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                        ce.content = e.content;
                                        //ce.Debug = "Enddate: " + eEndDate.ToString("dd.MM.yyyy HH:mm");
                                        if (!e.exceptDates.Contains(ce.startDate.Date))
                                        {
                                            events.Add(ce);
                                        }
                                    }
                                }
                                break;
                            case 3:
                                //repeat weekl
                                //DateTime wStartDate = startDate.Date <= e.startDate.Date ? e.startDate.Date : startDate.Date;
                                //loop through all weeks from startdate to enddate, e.weekInterval tell if event sould occure every week, every other week, every thrid week etc.
                                //for (DateTime w = wStartDate; w <= eEndDate; w = w.AddDays(7 * e.weekInterval))
                                if (e.startDate < endDate)
                                {
                                    for (DateTime w = e.startDate.Date; w <= eEndDate; w = w.AddDays(7 * e.weekInterval))
                                    {
                                        DateTime wEndDate = (eEndDate < w.AddDays(7) ? eEndDate : w.AddDays(6));
                                        //looping each day in the actual week and adding the event to the list on the correct day
                                        for (DateTime d = w; d <= wEndDate; d = d.AddDays(1))
                                        {
                                            if ((e.days.Contains((int)d.DayOfWeek) && (startDate <= d && d <= endDate)))
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurrence = e.recurrence;
                                                ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                                ce.content = e.content;
                                                //ce.Debug = "Her";
                                                if (!e.exceptDates.Contains(ce.startDate.Date))
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 4:
                                //repeat monthly
                                DateTime mStartDate;
                                if (startDate < e.startDate)
                                {
                                    //bruk e.startdate
                                    mStartDate = e.startDate;
                                }
                                else {
                                    if (e.startDate.Day < startDate.Day)
                                    {
                                        mStartDate = startDate.AddMonths(1).AddDays(((startDate.Day - e.startDate.Day) * -1) + 1);
                                    }
                                    else {
                                        mStartDate = startDate.AddDays(e.startDate.Day - startDate.Day);
                                    }
                                }
                                if (e.monthYearOption == 1)
                                {
                                    //use startdate every month
                                    for (DateTime d = mStartDate; d <= eEndDate; d = d.AddMonths(1))
                                    {
                                        if (e.monthOption.Value != 2 || (e.monthOption.Value == 2 && e.months.Contains(d.Month)))
                                        {
                                            CalendarEvent ce = new CalendarEvent();
                                            ce.recurrence = e.recurrence;
                                            ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                            ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                            ce.content = e.content;
                                            if (!e.exceptDates.Contains(ce.startDate.Date))
                                            {
                                                events.Add(ce);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //specify
                                    //looping every month from startdate to enddate
                                    //DateTime mStartDate = e.startDate < startDate ? startDate : e.startDate

                                    //for (DateTime d = startDate.AddDays((startDate.Day - 1) * -1); d <= eEndDate; d = d.AddMonths(1))
                                    for (DateTime d = mStartDate; d <= eEndDate; d = d.AddMonths(1))
                                    {

                                        if (e.interval < 6)
                                        {
                                            //1st - 5th weekday this month
                                            DateTime ed = d.GetNthWeekofMonth(e.interval, (DayOfWeek)e.weekDay);
                                            //c.Debug += " " + ed.ToString("dd.MM.yyyy hh:mm");
                                            //adding the event to the list
                                            if (startDate.Date <= ed.Date && d.Month == ed.Month && (e.monthOption.Value != 2 || (e.monthOption.Value == 2 && e.months.Contains(ed.Month))))
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurrence = e.recurrence;
                                                ce.startDate = e.startDate.AddDays(ed.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                                ce.content = e.content;
                                                //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                                if (!e.exceptDates.Contains(ce.startDate.Date))
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //last weekDay of month
                                            DateTime lwd = d.Last((DayOfWeek)e.weekDay);
                                            //adding event to list
                                            if (lwd <= eEndDate && (e.monthOption.Value != 2 || (e.monthOption.Value == 2 && e.months.Contains(d.Month))))
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurrence = e.recurrence;
                                                ce.startDate = e.startDate.AddDays(lwd.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                                ce.content = e.content;
                                                //ce.Debug = "Siste " + (DayOfWeek)e.weekDay + " i måneden";
                                                //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                                if (!e.exceptDates.Contains(ce.startDate.Date))
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            case 5:
                                //repeat yearly
                                DateTime yStartDate;
                                if (startDate < e.startDate)
                                {
                                    //bruk e.startdate
                                    yStartDate = e.startDate;
                                }
                                else
                                {
                                    if (e.startDate.Day < startDate.Day)
                                    {
                                        yStartDate = startDate.AddMonths(1).AddDays(((startDate.Day - e.startDate.Day) * -1) + 1);
                                    }
                                    else
                                    {
                                        yStartDate = startDate.AddDays(e.startDate.Day - startDate.Day);
                                    }
                                }
                                if (e.monthYearOption == 1)
                                {
                                    //use startdate

                                    for (DateTime d = yStartDate; d <= eEndDate; d = d.AddYears(1))
                                    {
                                        CalendarEvent ce = new CalendarEvent();
                                        ce.recurrence = e.recurrence;
                                        ce.startDate = e.startDate.AddYears(d.Year - e.startDate.Year);
                                        ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                        ce.content = e.content;
                                        if (startDate <= ce.startDate && ce.startDate <= endDate && !e.exceptDates.Contains(ce.startDate.Date))
                                        {
                                            events.Add(ce);
                                        }
                                    }
                                }
                                else
                                {
                                    //specify
                                    for (DateTime d = yStartDate.AddDays((startDate.Day - 1) * -1); d <= eEndDate; d = d.AddYears(1))
                                    {
                                        d = d.AddMonths((d.Month - 1) * -1).AddDays((d.Day - 1) * -1).AddMonths(e.month - 1);

                                        if (e.interval < 6)
                                        {
                                            //1st - 5th weekday in month
                                            DateTime ed = d.GetNthWeekofMonth(e.interval, (DayOfWeek)e.weekDay);
                                            //c.Debug += " " + ed.ToString("dd.MM.yyyy hh:mm");
                                            if (startDate.Date <= ed.Date && ed <= endDate && d.Month == ed.Month)
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurrence = e.recurrence;
                                                ce.startDate = e.startDate.AddDays(ed.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                                ce.content = e.content;
                                                //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                                if (!e.exceptDates.Contains(ce.startDate.Date))
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //last weekDay in month
                                            DateTime lwd = d.Last((DayOfWeek)e.weekDay);
                                            CalendarEvent ce = new CalendarEvent();
                                            ce.recurrence = e.recurrence;
                                            ce.startDate = e.startDate.AddDays(lwd.Date.Subtract(e.startDate.Date).Days);
                                            ce.endDate = (e.endDate.HasValue ? ce.startDate.AddMinutes(durationMinutes) : e.endDate);
                                            ce.content = e.content;
                                            //ce.Debug = "Siste " + (DayOfWeek)e.weekDay + " i måneden";
                                            //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                            if (startDate <= ce.startDate && ce.startDate <= endDate && !e.exceptDates.Contains(ce.startDate.Date))
                                            {
                                                events.Add(ce);
                                            }
                                        }
                                    }
                                }
                                break;
                            default: break;

                        }
                    }
                }
            }
            //order event list and return
            return events.OrderBy(x => x.startDate).ToList();
        }
    }
}
