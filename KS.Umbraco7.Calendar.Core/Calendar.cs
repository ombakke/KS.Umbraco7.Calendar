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
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).TypedContentAtRoot().First());
            var nodes = node.Descendants();
            return getEventList(startDate, endDate, propertyType, nodes);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="documentType">Alias of the document type</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var node = new DynamicPublishedContent(new UmbracoHelper(UmbracoContext.Current).TypedContentAtRoot().First());
            return getEventList(startDate, endDate, propertyType, node.Descendants(documentType));
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="documentType">Alias of the document type</param>
        ///<param name="startNode">DynamicPublishedConent to look for events in</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType, DynamicPublishedContent startNode)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var nodes = startNode.Descendants(documentType);
            return getEventList(startDate, endDate, propertyType, nodes);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="startNode">DynamicPublishedConent to look for events in</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public static List<CalendarEvent> getEvents(DateTime startDate, DateTime endDate, string propertyType, DynamicPublishedContent startNode)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            var nodes = startNode.Descendants();
            return getEventList(startDate, endDate, propertyType, nodes);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="nodes">DynamicPublishedContentlist holding the nodes where we will be looking for events</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        private static List<CalendarEvent> getEventList(DateTime startDate, DateTime endDate, string propertyType, DynamicPublishedContentList nodes)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            foreach (var node in nodes)
            {
                if (node.HasValue(propertyType))
                {
                    CalendarEvent e = Newtonsoft.Json.JsonConvert.DeserializeObject<CalendarEvent>(node.GetPropertyValue(propertyType).ToString());
                    if (((startDate <= e.startDate || e.recurring > 1) && e.startDate <= endDate) && (string.IsNullOrEmpty(e.endDate.ToString()) || startDate <= e.endDate))
                    {
                        DateTime eEndDate = (e.endDate == null ? endDate : (e.endDate.Value < endDate ? e.endDate.Value : endDate));
                        e.content = node;
                        switch (e.recurring)
                        {
                            case 1:
                                //none recurring
                                events.Add(e);
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
                                        ce.recurring = e.recurring;
                                        ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                        ce.endDate = e.endDate;
                                        ce.content = e.content;
                                        //ce.Debug = "Enddate: " + eEndDate.ToString("dd.MM.yyyy HH:mm");
                                        events.Add(ce);
                                    }
                                }
                                break;
                            case 3:
                                //repeat weekly
                                DateTime wStartDate = startDate.Date <= e.startDate.Date ? e.startDate.Date : startDate.Date;
                                //loop through all weeks from startdate to enddate, e.interval tell if event sould occure every week, every other week, every thrid week etc.
                                for (DateTime w = wStartDate; w <= eEndDate; w = w.AddDays(7 * e.interval))
                                {
                                    DateTime wEndDate = eEndDate < w.AddDays(7) ? eEndDate : w.AddDays(7);
                                    //looping each day in the actual week and adding the event to the list on the correct day
                                    for (DateTime d = w; d <= wEndDate; d = d.AddDays(1))
                                    {
                                        if (e.days.Contains((int)d.DayOfWeek))
                                        {
                                            CalendarEvent ce = new CalendarEvent();
                                            ce.recurring = e.recurring;
                                            ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                            ce.endDate = e.endDate;
                                            ce.content = e.content;
                                            //ce.Debug = "Her";
                                            events.Add(ce);
                                        }
                                    }
                                }
                                break;
                            case 4:
                                //repeat monthly
                                if (e.monthYearOption == 1)
                                {
                                    //use startdate every month
                                    for (DateTime d = startDate; d <= eEndDate; d = d.AddMonths(1))
                                    {
                                        CalendarEvent ce = new CalendarEvent();
                                        ce.recurring = e.recurring;
                                        ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                        ce.endDate = e.endDate;
                                        ce.content = e.content;
                                        events.Add(ce);
                                    }
                                }
                                else
                                {
                                    //specify
                                    //looping every month from startdate to enddate
                                    for (DateTime d = startDate.AddDays((startDate.Day - 1) * -1); d <= eEndDate; d = d.AddMonths(1))
                                    {

                                        if (e.interval < 6)
                                        {
                                            //1st - 5th weekday this month
                                            DateTime ed = d.GetNthWeekofMonth(e.interval, (DayOfWeek)e.weekDay);
                                            //c.Debug += " " + ed.ToString("dd.MM.yyyy hh:mm");
                                            //adding the event to the list
                                            if (startDate.Date <= ed.Date && d.Month == ed.Month)
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurring = e.recurring;
                                                ce.startDate = e.startDate.AddDays(ed.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = e.endDate;
                                                ce.content = e.content;
                                                //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                                events.Add(ce);
                                            }
                                        }
                                        else
                                        {
                                            //last weekDay of month
                                            DateTime lwd = d.Last((DayOfWeek)e.weekDay);
                                            //adding event to list
                                            if (lwd <= eEndDate)
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurring = e.recurring;
                                                ce.startDate = e.startDate.AddDays(lwd.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = e.endDate;
                                                ce.content = e.content;
                                                //ce.Debug = "Siste " + (DayOfWeek)e.weekDay + " i måneden";
                                                //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                                events.Add(ce);
                                            }
                                        }
                                    }
                                }

                                break;
                            case 5:
                                //repeat yearly
                                if (e.monthYearOption == 1)
                                {
                                    //use startdate
                                    for (DateTime d = startDate; d <= eEndDate; d = d.AddYears(1))
                                    {
                                        CalendarEvent ce = new CalendarEvent();
                                        ce.recurring = e.recurring;
                                        ce.startDate = e.startDate.AddDays(d.Date.Subtract(e.startDate.Date).Days);
                                        ce.endDate = e.endDate;
                                        ce.content = e.content;
                                        events.Add(ce);
                                    }
                                }
                                else
                                {
                                    //specify
                                    for (DateTime d = startDate.AddDays((startDate.Day - 1) * -1); d <= eEndDate; d = d.AddYears(1))
                                    {
                                        d = d.AddMonths((d.Month - 1) * -1).AddDays((d.Day - 1) * -1).AddMonths(e.month - 1);

                                        if (e.interval < 6)
                                        {
                                            //1st - 5th weekday in month
                                            DateTime ed = d.GetNthWeekofMonth(e.interval, (DayOfWeek)e.weekDay);
                                            //c.Debug += " " + ed.ToString("dd.MM.yyyy hh:mm");
                                            if (startDate.Date <= ed.Date && d.Month == ed.Month)
                                            {
                                                CalendarEvent ce = new CalendarEvent();
                                                ce.recurring = e.recurring;
                                                ce.startDate = e.startDate.AddDays(ed.Date.Subtract(e.startDate.Date).Days);
                                                ce.endDate = e.endDate;
                                                ce.content = e.content;
                                                //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                                events.Add(ce);
                                            }
                                        }
                                        else
                                        {
                                            //last weekDay in month
                                            DateTime lwd = d.Last((DayOfWeek)e.weekDay);
                                            CalendarEvent ce = new CalendarEvent();
                                            ce.recurring = e.recurring;
                                            ce.startDate = e.startDate.AddDays(lwd.Date.Subtract(e.startDate.Date).Days);
                                            ce.endDate = e.endDate;
                                            ce.content = e.content;
                                            //ce.Debug = "Siste " + (DayOfWeek)e.weekDay + " i måneden";
                                            //ce.Debug = "d: " + d.ToString("dd.MM.yyyy hh:mm") + "  -  " + "ed.month: " + ed.Month + "  d.month: " + d.Month + "  ";
                                            events.Add(ce);
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
