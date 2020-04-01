using KS.Umbraco7.Calendar.Extensions;
using KS.Umbraco7.Calendar.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace KS.Umbraco7.Calendar.Services
{
    public class CalendarService
    {
        private readonly IUmbracoContextFactory context;
        private ILogger _logger;

        public CalendarService(IUmbracoContextFactory context, ILogger logger)
        {
            this.context = context;
            _logger = logger;
        }

        public List<CalendarEvent> GetNodeEvents(DateTime date, IPublishedContent node, string propertyType, bool splitNoneRecurring = true)
        {
            try
            {
                return GetEventList(date.Date, date.Date.AddDays(1).AddTicks(-1), propertyType, node, splitNoneRecurring);
            }
            catch (Exception ex)
            {
                _logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Could not get node events", ex);
            }

            return new List<CalendarEvent>();
        }

        ///<summary>Get a list of calendar events from a single node</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the events calendar datatype</param>
        ///<param name="node">The event node</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public List<CalendarEvent> GetNodeEvents(DateTime startDate, DateTime endDate, IPublishedContent node, string propertyType, bool splitNoneRecurring = true)
        {
            try
            {
                return GetEventList(startDate, endDate, propertyType, node, splitNoneRecurring);
            }
            catch (Exception ex)
            {
                _logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Could not get node events", ex);
            }

            return new List<CalendarEvent>();
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the events calendar datatype</param>
        ///<param name="documentType">Alias of the document type</param>
        ///<param name="startNode">Start node to look for events in</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public List<CalendarEvent> GetEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType, IPublishedContent startNode, bool splitNoneRecurring = true)
        {
            using (var cref = context.EnsureUmbracoContext())
            {
                var contentCache = cref.UmbracoContext.Content;
                try
                {
                    var nodes = contentCache.GetByXPath($"//*[@isDoc][@id = {startNode.Id}]//{documentType}");

                    return GetEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
                }
                catch (Exception ex)
                {
                    _logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Could not get node events", ex);
                }
            }
            return new List<CalendarEvent>();
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the events calendar datatype</param>
        ///<param name="startNode">DynamicPublishedConent to look for events in</param>
        ///<param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        public List<CalendarEvent> GetEvents(DateTime startDate, DateTime endDate, string propertyType, IPublishedContent startNode, bool splitNoneRecurring = true)
        {
            using (var cref = context.EnsureUmbracoContext())
            {
                var contentCache = cref.UmbracoContext.Content;
                try
                {
                    var nodes = contentCache.GetByXPath($"//*[@isDoc][@id = {startNode.Id}]//*[@isDoc]");

                    return GetEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
                }
                catch (Exception ex)
                {
                    _logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Could not get node events", ex);
                }
            }

            return new List<CalendarEvent>();
        }

        /// <summary>Get a list of calendar events</summary>
        /// <param name="startDate">Start date for event list</param>
        /// <param name="endDate">End date for event list</param>
        /// <param name="propertyType">Alias of the property holding the events calendar datatype</param>
        /// <param name="node">Node where we will be looking for events</param>
        /// <param name="splitNoneRecurring">Optional: Split none recurring events by day, true by default</param>
        /// <returns>An ordered List with CalendarEvents ordered by startDate</returns>
        private List<CalendarEvent> GetEventList(DateTime startDate, DateTime endDate, string propertyType, IPublishedContent node, bool splitNoneRecurring = true)
        {
            List<IPublishedContent> nodes = new List<IPublishedContent>
            {
                node
            };

            return GetEventList(startDate, endDate, propertyType, nodes, splitNoneRecurring);
        }

        ///<summary>Get a list of calendar events</summary>
        ///<param name="startDate">Start date for event list</param>
        ///<param name="endDate">End date for event list</param>
        ///<param name="propertyType">Alias of the property holding the Datatype KS.Umbraco7.Calendar</param>
        ///<param name="nodes">Nodes where we will be looking for events</param>
        ///<returns>An ordered List with CalendarEvents ordered by startDate</returns>
        private List<CalendarEvent> GetEventList(DateTime startDate, DateTime endDate, string propertyType, IEnumerable<IPublishedContent> nodes, bool splitNoneRecurring = true)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();

            if (nodes == null || nodes.Any() == false)
                return events;

            foreach (var node in nodes)
            {
                if (node.HasValue(propertyType))
                {
                    var propValue = node.Value<string>(propertyType);

                    CalendarEvent e = JsonConvert.DeserializeObject<CalendarEvent>(propValue);

                    if (e.ExceptDates == null)
                    {
                        e.ExceptDates = new List<DateTime>();
                    }
                    if (e.Days == null)
                    {
                        e.Days = new int?[0];
                    }
                    if (e.Months == null)
                    {
                        e.Months = new int?[0];
                    }

                    if ((startDate <= e.StartDate || (e.Recurrence > 1 && (e.RecurUntil.HasValue && startDate <= e.RecurUntil.Value)) || (e.Recurrence > 1 && !e.RecurUntil.HasValue) || (e.EndDate.HasValue && startDate <= e.EndDate.Value)) && e.StartDate <= endDate)
                    {
                        int durationMinutes = 0;
                        if (e.EndDate.HasValue)
                        {
                            durationMinutes = (int)e.EndDate.Value.Subtract(e.StartDate).TotalMinutes;
                        }

                        DateTime eEndDate = (!e.RecurUntil.HasValue ? endDate : (e.RecurUntil.Value < endDate ? e.RecurUntil.Value.AddDays(1).AddSeconds(-1) : endDate));
                        e.Content = node;
                        switch (e.Recurrence)
                        {
                            case 1:
                                // No recurrence

                                if (e.EndDate.HasValue && e.StartDate.Date < e.EndDate.Value.Date && splitNoneRecurring)
                                {
                                    // Event spanning several days
                                    DateTime dSDate = startDate.Date <= e.StartDate.Date ? e.StartDate.Date : startDate.Date;
                                    for (DateTime d = dSDate; d <= e.EndDate.Value.Date; d = d.AddDays(1))
                                    {
                                        CalendarEvent ce = new CalendarEvent();
                                        if (e.StartDate.Date < d.Date)
                                        {
                                            ce.StartDate = d.Date;
                                        }
                                        else
                                        {
                                            ce.StartDate = e.StartDate;
                                        }
                                        if (d.Date < e.EndDate.Value.Date)
                                        {
                                            ce.EndDate = d.Date.AddDays(1).AddSeconds(-1);
                                        }
                                        else
                                        {
                                            ce.EndDate = e.EndDate.Value;
                                        }
                                        ce.Recurrence = e.Recurrence;
                                        ce.Content = e.Content;
                                        if (ce.StartDate <= endDate)
                                        {
                                            events.Add(ce);
                                        }
                                    }
                                }
                                else
                                {
                                    events.Add(e);
                                }
                                break;
                            case 2:
                                // Repeat daily

                                DateTime dStartDate = startDate.Date <= e.StartDate.Date ? e.StartDate.Date : startDate.Date;
                                // Loop through all days from startdate to enddate
                                for (DateTime d = dStartDate; d <= eEndDate; d = d.AddDays(1))
                                {
                                    // If the event is selected for the actual day, add it to the list
                                    if (e.Days.Contains((int)d.DayOfWeek))
                                    {
                                        CalendarEvent ce = new CalendarEvent
                                        {
                                            Recurrence = e.Recurrence,
                                            StartDate = e.StartDate.AddDays(d.Date.Subtract(e.StartDate.Date).Days)
                                        };

                                        ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                        ce.Content = e.Content;

                                        if (!e.ExceptDates.Contains(ce.StartDate.Date) && ce.StartDate <= endDate)
                                        {
                                            events.Add(ce);
                                        }
                                    }
                                }
                                break;
                            case 3:
                                // Repeat weekly

                                // DateTime wStartDate = startDate.Date <= e.startDate.Date ? e.startDate.Date : startDate.Date;
                                // loop through all weeks from startdate to enddate, e.weekInterval tell if event sould occure every week, every other week, every thrid week etc.
                                // for (DateTime w = wStartDate; w <= eEndDate; w = w.AddDays(7 * e.weekInterval))
                                if (e.StartDate < endDate)
                                {
                                    for (DateTime w = e.StartDate.Date; w <= eEndDate; w = w.AddDays(7 * e.WeekInterval))
                                    {
                                        DateTime wEndDate = (eEndDate < w.AddDays(7) ? eEndDate : w.AddDays(6));
                                        //looping each day in the actual week and adding the event to the list on the correct day
                                        for (DateTime d = w; d <= wEndDate; d = d.AddDays(1))
                                        {
                                            if ((e.Days.Contains((int)d.DayOfWeek) && (startDate <= d && d <= endDate)))
                                            {
                                                CalendarEvent ce = new CalendarEvent
                                                {
                                                    Recurrence = e.Recurrence,
                                                    StartDate = e.StartDate.AddDays(d.Date.Subtract(e.StartDate.Date).Days)
                                                };

                                                ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                                ce.Content = e.Content;
                                                if (!e.ExceptDates.Contains(ce.StartDate.Date) && ce.StartDate <= endDate)
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 4:
                                // Repeat monthly

                                DateTime mStartDate;

                                if (e.MonthYearOption == 1)
                                {
                                    // Use startdate every month
                                    if (startDate < e.StartDate)
                                    {
                                        mStartDate = e.StartDate;
                                    }
                                    else
                                    {
                                        if (e.StartDate.Day < startDate.Day)
                                        {
                                            mStartDate = startDate.AddMonths(1).AddDays(((startDate.Day - e.StartDate.Day) * -1) + 1);
                                        }
                                        else
                                        {
                                            mStartDate = startDate.AddDays(e.StartDate.Day - startDate.Day);
                                        }
                                    }

                                    for (DateTime d = mStartDate; d <= eEndDate; d = d.AddMonths(1))
                                    {
                                        if (e.MonthOption.Value != 2 || (e.MonthOption.Value == 2 && e.Months.Contains(d.Month)))
                                        {
                                            CalendarEvent ce = new CalendarEvent
                                            {
                                                Recurrence = e.Recurrence,
                                                StartDate = e.StartDate.AddDays(d.Date.Subtract(e.StartDate.Date).Days)
                                            };

                                            ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                            ce.Content = e.Content;
                                            if (!e.ExceptDates.Contains(ce.StartDate.Date) && ce.StartDate <= endDate)
                                            {
                                                events.Add(ce);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Looping every month from start date to end date                           
                                    mStartDate = startDate < e.StartDate ? e.StartDate : startDate;
                                    for (DateTime d = mStartDate; d <= eEndDate; d = d.AddMonths(1))
                                    {
                                        if (e.Interval < 6)
                                        {
                                            // 1st - 5th weekday this month
                                            DateTime ed = d.GetNthWeekofMonth(e.Interval, (DayOfWeek)e.WeekDay);

                                            if (startDate.Date <= ed.Date && d.Month == ed.Month && (e.MonthOption.Value != 2 || (e.MonthOption.Value == 2 && e.Months.Contains(ed.Month))))
                                            {
                                                CalendarEvent ce = new CalendarEvent
                                                {
                                                    Recurrence = e.Recurrence,
                                                    StartDate = e.StartDate.AddDays(ed.Date.Subtract(e.StartDate.Date).Days)
                                                };

                                                ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                                ce.Content = e.Content;

                                                if (!e.ExceptDates.Contains(ce.StartDate.Date) && ce.StartDate <= endDate)
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Last weekday of month
                                            DateTime lwd = d.Last((DayOfWeek)e.WeekDay);

                                            if (lwd <= eEndDate && (e.MonthOption.Value != 2 || (e.MonthOption.Value == 2 && e.Months.Contains(d.Month))))
                                            {
                                                CalendarEvent ce = new CalendarEvent
                                                {
                                                    Recurrence = e.Recurrence,
                                                    StartDate = e.StartDate.AddDays(lwd.Date.Subtract(e.StartDate.Date).Days)
                                                };

                                                ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                                ce.Content = e.Content;

                                                if (!e.ExceptDates.Contains(ce.StartDate.Date) && ce.StartDate <= endDate)
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            case 5:
                                // Repeat yearly

                                DateTime yStartDate;
                                if (startDate < e.StartDate)
                                {
                                    yStartDate = e.StartDate;
                                }
                                else
                                {
                                    if (e.StartDate.Day < startDate.Day)
                                    {
                                        yStartDate = startDate.AddMonths(1).AddDays(((startDate.Day - e.StartDate.Day) * -1) + 1);
                                    }
                                    else
                                    {
                                        yStartDate = startDate.AddDays(e.StartDate.Day - startDate.Day);
                                    }
                                }
                                if (e.MonthYearOption == 1)
                                {
                                    // Use start date
                                    for (DateTime d = yStartDate; d <= eEndDate; d = d.AddYears(1))
                                    {
                                        CalendarEvent ce = new CalendarEvent
                                        {
                                            Recurrence = e.Recurrence,
                                            StartDate = e.StartDate.AddYears(d.Year - e.StartDate.Year)
                                        };

                                        ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                        ce.Content = e.Content;
                                        if (startDate <= ce.StartDate && ce.StartDate <= endDate && !e.ExceptDates.Contains(ce.StartDate.Date))
                                        {
                                            events.Add(ce);
                                        }
                                    }
                                }
                                else
                                {
                                    for (DateTime d = yStartDate.AddDays((startDate.Day - 1) * -1); d <= eEndDate; d = d.AddYears(1))
                                    {
                                        d = d.AddMonths((d.Month - 1) * -1).AddDays((d.Day - 1) * -1).AddMonths(e.Month - 1);

                                        if (e.Interval < 6)
                                        {
                                            // 1st - 5th weekday in month
                                            DateTime ed = d.GetNthWeekofMonth(e.Interval, (DayOfWeek)e.WeekDay);

                                            if (startDate.Date <= ed.Date && ed <= endDate && d.Month == ed.Month)
                                            {
                                                CalendarEvent ce = new CalendarEvent
                                                {
                                                    Recurrence = e.Recurrence,
                                                    StartDate = e.StartDate.AddDays(ed.Date.Subtract(e.StartDate.Date).Days)
                                                };

                                                ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                                ce.Content = e.Content;

                                                if (!e.ExceptDates.Contains(ce.StartDate.Date) && ce.StartDate <= endDate)
                                                {
                                                    events.Add(ce);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Last weekday in month
                                            DateTime lwd = d.Last((DayOfWeek)e.WeekDay);
                                            CalendarEvent ce = new CalendarEvent
                                            {
                                                Recurrence = e.Recurrence,
                                                StartDate = e.StartDate.AddDays(lwd.Date.Subtract(e.StartDate.Date).Days)
                                            };

                                            ce.EndDate = (e.EndDate.HasValue ? ce.StartDate.AddMinutes(durationMinutes) : e.EndDate);
                                            ce.Content = e.Content;

                                            if (startDate <= ce.StartDate && ce.StartDate <= endDate && !e.ExceptDates.Contains(ce.StartDate.Date))
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

            return events.OrderBy(x => x.StartDate).ToList();
        }
    }
}
