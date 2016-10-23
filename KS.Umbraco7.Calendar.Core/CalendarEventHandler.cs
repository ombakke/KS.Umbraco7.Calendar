using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace KS.Umbraco7.Calendar.Core
{
    public class CalendarEventHandler : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saving += ContentService_Saving;
        }

        void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var node in e.SavedEntities)
            {

                /* calendar - KS.Umbraco7.Calendar */
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
                                string calJson = node.GetValue(pt.Alias).ToString();
                                CalendarEvent cal = Newtonsoft.Json.JsonConvert.DeserializeObject<CalendarEvent>(calJson);

                                var saveToPT = node.PropertyTypes.First(x => x.Alias == pvs["startDateField"].Value);
                                var saveToDT = dataTypeService.GetDataTypeDefinitionById(saveToPT.DataTypeDefinitionId);

                                //if (saveToDT.DatabaseType == DataTypeDatabaseType.Nvarchar || saveToDT.DatabaseType == DataTypeDatabaseType.Ntext)
                                //{
                                //    node.SetValue(pvs["startDateField"].Value, cal.startDate.ToString("yyyy-MM-dd HH:mm"));
                                //    sender.SaveAndPublishWithStatus(node, 0, false);
                                //}
                                //else
                                if (saveToDT.DatabaseType == DataTypeDatabaseType.Date)
                                {
                                    node.SetValue(saveToPT.Alias, cal.startDate);
                                    sender.Save(node, 0, false);
                                }
                            }
                            //LogHelper.Info(typeof(MyEventHandler), "Saved something");

                            //if (node.Parent().ContentType.Alias != "DateFolderTypeNameFromPreValue")
                            //{
                            //    //move and save
                            //}
                            //else
                            //{
                            //    //if mismatch between startdate and year/month-folder => move and save
                            //}

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Info(typeof(CalendarEventHandler), ex.ToString());
                }
            }
        }
    }
}
