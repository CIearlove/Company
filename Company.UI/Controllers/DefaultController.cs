using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.UI.Controllers
{
    /// <summary>
    /// 添加DefaultController控制器，重写OnActionExecuting方法，每次访问控制器前触发
    /// </summary>
    public class DefaultController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var userName = Session["Manager"] as String;
            if (String.IsNullOrEmpty(userName))
            {
                //重定向至登录页面
                filterContext.Result = RedirectToAction("Login", "Login", new { url = Request.RawUrl });
            }
        }
    }
}