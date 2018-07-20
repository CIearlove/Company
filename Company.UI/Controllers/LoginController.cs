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
            return View();
        }

        public JsonResult Validate(LoginInfo loginInfo)
        {
            if (LoginService.GetModels(p => p.Manager == loginInfo.manager && p.Password == loginInfo.password).Count() > 0)
            {
                return  Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}