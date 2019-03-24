KS.Umbraco7.Calendar
====================

Calendar property editor for Umbraco 7

Download Umbraco 7 package at: http://our.umbraco.org/projects/website-utilities/ksumbraco7calendar


Getting started:
 - Install the package in Umbraco7
 - Create a data type based on the property editor KS.Umbraco7.Calendar
 - Add the data type to a document type
 - Create some content
 - Use the namespace KS.Umbraco7.Calendar.Core 
 - Get list of CalendarEvents with Calendar.getEvents(DateTime startDate, DateTime endDate, string propertyType)

More info can be found at: http://our.umbraco.org/projects/website-utilities/ksumbraco7calendar

## Change log

### Version 1.0.0
Nuget package build
Fixes performance issues in get events methods and using XPath instead.
Added new signature to pass in `IPublishedContent` and remove support for `DynamicPublishedContent`.

### Version 0.1.1

Added danish language-file (Thanks to Chriztian Steinmeier)
Bugfix in editor.html (selcting recurrence didn't show options)

### Version 0.1.2

Added some css
Added validation to ensure startdate is less than enddate
Some bugfixing on how the start- and enddates are updated to the model

### Version 0.1.3

Bugfixing on viewing recurring events.
Version 0.1.4

2 new methods where you can use int nodeId as a parameter

getEvents(DateTime startDate, DateTime endDate, string propertyType, int nodeId)
getEvents(DateTime startDate, DateTime endDate, string propertyType, string documentType, int nodeId)

### Version 0.1.5

Bugfixing monthly and yearly recurring events.

### Version 0.1.6

Fixed listing of non-recurring events that spans several days
Added optional boolean parameter splitNoneRecurring with default value true. If set to false none recurring events spanning several days will only occur once in the result list with its original start and end date. Else it will occur once for each day it spans.
Added possibility to choose months for monthly-recurring events.


### Version 0.1.7

Added field "Recur until", this means that endDate is now optional, and is used to determine the duration of the event. The duration of the event will be enddate - startdate for every event. The event will recur until the optional date recur until. If the recur until is not provided the event will recur forever.
Added fields Except days. Here you can provide a list of dates where recurring events will not happen.
IMPORTANT: This version has new fields in the model and the field enddate gets a new meaning. This means that if you update for an earlier version you may have to make changes to you existing events. After upgrade you might have to clear you browser cache.

### Version 0.1.7.3

Upgraded the datatimepicker-script. If you upgrade from an earlier install you might need to restart your application pool (touch the web.config) and clear your browser cache.

### Version 0.1.7.4
Added two new functions, getNodeEvents, to get event information from a single event node.
