using Company.BLLContainer;
using Company.Common;
using Company.Common.Staff;
using Company.IBLL;
using Company.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Company.UI.Controllers
{
    public class HomeController : DefaultController
    {
        private IHome_NavService Home_NavService = Container.Resolve<IHome_NavService>();

        public ActionResult Index()
        {
            ViewData["User"] = Session["Manager"];
            return View();
        }

        #region 登出
        public ActionResult LoginOut()
        {
            Session["Manager"] = null;
            return RedirectToAction("Login", "Login");
        }
        #endregion

        #region 导航栏
        public JsonResult GetHomeNav(int? id)
        {
            int? Id;
            if (id != null)
            {
                Id = id;
            }
            else
            {
                Id = 0;
            }

            IQueryable<Home_Nav> home_Navs = Home_NavService.GetModels(p => p.Nid == Id);
            List<Home_Nav> list = new List<Home_Nav>();
            list = home_Navs.ToList();

            var json = list.Select(home_nav => new
            {
                id = home_nav.Id,
                text = home_nav.Text,
                state = home_nav.State,
                iconCls = home_nav.IconCls,
                url = home_nav.Url,
            }).ToArray();

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}