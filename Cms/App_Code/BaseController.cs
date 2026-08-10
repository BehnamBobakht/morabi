using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Policy;

namespace Cms
{
    public class BaseController
    {
        public static string GetView(Controller controller)
        {
            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();
            string action = controller.ControllerContext.RouteData.Values["action"].ToString();
            return "~/Views/" + BaseWebsite.WebsiteLabel + "/" + controllerName + "/" + action + ".cshtml";
        }

        public static string GetView(Controller controller, string view = null)
        {
            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();
            if (view == null)
                view = controller.ControllerContext.RouteData.Values["action"].ToString();
            return "~/Views/" + BaseWebsite.WebsiteLabel + "/" + controllerName + "/" + view + ".cshtml";
        }

        public static ActionResult GetView(Controller controller = null, Enum_ResultType result = Enum_ResultType.RESULT_TYPE_CONTROLLER, string viewName = null, object model = null)
        {
            switch (result)
            {
                case Enum_ResultType.RESULT_TYPE_CONTROLLER:
                    {
                        return new PartialViewResult()
                        {
                            ViewName = GetView(controller),
                            ViewData = new ViewDataDictionary(model)
                        };
                    }
                case Enum_ResultType.RESULT_TYPE_VIEWNAME:
                    {
                        if (viewName != null)
                        {
                            return new PartialViewResult()
                            {
                                ViewName = GetView(controller, viewName),
                                ViewData = new ViewDataDictionary(model)
                            };
                        }
                        else
                        {
                            return new PartialViewResult()
                            {
                                ViewName = GetView(controller),
                                ViewData = new ViewDataDictionary(model)
                            };
                        }
                    }
                case Enum_ResultType.RESULT_TYPE_JSON:
                    {
                        JsonResult output = new JsonResult()
                        {
                            Data = model
                        };
                        return output;
                    }
                default:
                    return null;
            }
        }

        public static string RedirectToError(int errorCode = 301)
        {
            var errorAddress = BaseWebsite.ShopUrl + "error/" + errorCode;

            if (HttpContext.Current.Response.StatusCode == 301)
            {
                errorAddress = HttpContext.Current.Response.Headers.Get("Location");
            }
            else
            {
                HttpContext.Current.Response.StatusCode = errorCode;
                HttpContext.Current.Response.TrySkipIisCustomErrors = true;
                HttpContext.Current.Response.AddHeader("Location", errorAddress);
            }
            return errorAddress;
        }
    }
}