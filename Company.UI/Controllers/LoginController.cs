using Company.BLLContainer;
using Company.Common.LoginManager;
using Company.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.UI.Controllers
{
    public class LoginController : Controller
    {
        private ILoginService LoginService = Container.Resolve<ILoginService>();
        // GET: Login
        public ActionResult Login()
        {
            //有种实现了自动登录的感觉
            if(Session["Manager"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public JsonResult Validate(LoginInfo loginInfo)
        {
            //验证通过
            if (LoginService.GetModels(p => p.Manager == loginInfo.manager && p.Password == loginInfo.password).Count() > 0)
            {
                Session["Manager"] = loginInfo.manager;
                return  Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}