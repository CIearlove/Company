using Company.BLLContainer;
using Company.Common;
using Company.IBLL;
using Company.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.UI.Controllers
{
    public class SysExceptionController : DefaultController
    {
        private ISysExceptionService SysExceptionService = Container.Resolve<ISysExceptionService>();
        // GET: SysException
        public ActionResult Index()
        {
            return View();
        }

        #region 列表（初始化）
        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            List<SysException> list = new List<SysException>();

            pager.totalRows = SysExceptionService.GetModels(c => true).ToList().Count();
            //初始化页面根据Id排序
            list = SysExceptionService.GetModelsByPage(pager.rows, pager.page, pager.isAsc, c => c.Id, c => true).ToList();

            var json = new
            {
                total = pager.totalRows,
                rows = list.Select(sysException => new
                {
                    Id = sysException.Id,
                    HelpLink = sysException.HelpLink,
                    Message = sysException.Message,
                    Source = sysException.Source,
                    StackTrace = sysException.StackTrace,
                    TargetSite = sysException.TargetSite,
                    Data = sysException.Data,
                    CreateTime = sysException.CreateTime,
                }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}