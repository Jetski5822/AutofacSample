using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Framework.Internal;
using Microsoft.AspNet.Mvc.Razor;

namespace MvcSample.Web
{
    public class TestViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["foo"] = Guid.NewGuid().ToString();
        }

        public virtual IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            yield return "/Views/Home/Not-Index.cshtml";
        }
    }
}
