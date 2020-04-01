using KS.Umbraco7.Calendar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace KS.Umbraco7.Calendar.Composers
{
    [RuntimeLevel(MinLevel = Umbraco.Core.RuntimeLevel.Run)]
    public class Composers : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterUnique<CalendarService>();
        }
    }
}
