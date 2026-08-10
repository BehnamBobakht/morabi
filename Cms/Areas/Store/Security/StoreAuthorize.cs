
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.Base;
using DataLayer.Entities;

namespace Cms.Areas.Store.Security
{
    public class AuthorizeFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Enum_StorePanel Enum_StorePanel = Enum_StorePanel.StoreState;
        public int Id = 0;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UnitOfWork context = new UnitOfWork();
            var cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var currentUrl = filterContext.HttpContext.Request.Url.AbsolutePath;

            if (currentAccount == null)
            {
                filterContext.Result = new RedirectResult("/Store/Error/Error?back=" + currentUrl, false);

            }
            else
            {
                switch (Enum_StorePanel)
                {
                    case Enum_StorePanel.StoreState:
                        {
                            if (!BaseStore.StoreState(context, currentAccount))
                                filterContext.Result = new RedirectResult("/Store/Error/Error?back=" + currentUrl, false);
                            break;
                        }
                    case Enum_StorePanel.StoreProductId:
                        {
                            if (!BaseStore.StoreProductId_IsValid(Id, context))
                                filterContext.Result = new RedirectResult("/Store/Error/Error?back=" + currentUrl, false);
                            break;
                        }
                    case Enum_StorePanel.StoreOrderId:
                        {
                            if (!BaseStore.StoreOrderId_IsValid(Id, context))
                                filterContext.Result = new RedirectResult("/Store/Error/Error?back=" + currentUrl, false);
                            break;
                        }
                    case Enum_StorePanel.StoreDiscount:
                        {
                            if (!BaseStore.StoreDiscount_IsValid(Id, context))
                                filterContext.Result = new RedirectResult("/Store/Error/Error?back=" + currentUrl, false);
                            break;
                        }
                }

            }
            context.Dispose();
        }
    }
}