using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace KS.Umbraco7.Calendar.Core
{
    public class CalendarEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saving += ContentService_Saving;
        }

        void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            if (UmbracoContext.Current == null) return;

            foreach (var node in e.SavedEntities)
            {
                // Property editor alias: KS.Umbraco7.Calendar

                try
                {
                    if (node.PropertyTypes.Any(x => x.PropertyEditorAlias == "KS.Umbraco7.Calendar"))
                    {
                        var pt = node.PropertyTypes.First(x => x.PropertyEditorAlias == "KS.Umbraco7.Calendar");
                        //LogHelper.Info(typeof(CalendarEventHandler), "PropertyTypeAlias Alias: " + pt.Alias + " PropertyEditorAlias: " + pt.PropertyEditorAlias);
                        var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

                        IDictionary<string, PreValue> pvs = dataTypeService.GetPreValuesCollectionByDataTypeId(pt.DataTypeDefinitionId).PreValuesAsDictionary;

                        if (pvs.Any(x => x.Key == "startDateField" && x.Value.Value != null))
                        {
                            if (node.HasProperty(pvs["startDateField"].Value))
                            {
                                string calJson = node.GetValue<string>(pt.Alias);
                                CalendarEvent cal = JsonConvert.DeserializeObject<CalendarEvent>(calJson);

                                var saveToPT = node.PropertyTypes.First(x => x.Alias == pvs["startDateField"].Value);
                                var saveToDT = dataTypeService.GetDataTypeDefinitionById(saveToPT.DataTypeDefinitionId);

                                if (saveToDT.DatabaseType == DataTypeDatabaseType.Date)
                                {
                                    var currentUser = UmbracoContext.Current.Security.CurrentUser;

                                    node.SetValue(saveToPT.Alias, cal.StartDate);
                                    sender.Save(node, currentUser?.Id ?? 0, false);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<CalendarEventHandler>("Could not save events for node.", ex);
                }
            }
        }
    }
}
