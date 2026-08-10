using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;

namespace Panel.Controllers
{
    public class CustomCacheAttribute : OutputCacheAttribute
    {
        public Enum_OutputCache cacheType { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var cache = BaseCache.GetByLabel(cacheType);
            this.Duration = cache.Duration;
        }


    }
}