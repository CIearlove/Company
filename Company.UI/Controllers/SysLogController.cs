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
    public class SysLogController : DefaultController
    {
        private ISysLogService SysLogService = Container.Resolve<ISysLogService>();
        // GET: SysLog
        public ActionResult Index()
        {
            return View();
        }

        #region 列表（初始化）
        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            List<SysLog> list = new List<SysLog>();
           
            pager.totalRows = SysLogService.GetModels(c => true).ToList().Count();
            //初始化页面根据Id排序
            list = SysLogService.GetModelsByPage(pager.rows, pager.page, pager.isAsc, c => c.Id, c => true).ToList();
            
            var json = new
            {
                total = pager.totalRows,
                rows = list.Select(syslog => new
                {
                    Id = syslog.Id,
                    Operator = syslog.Operator,
                    Message = syslog.Message,
                    Result = syslog.Result,
                    Type = syslog.Type,
                    Module = syslog.Module,
                    CreateTime = syslog.CreateTime,
                }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 详细页面
        public JsonResult GetDetail(int Id)
        {
            List<SysLog> list = new List<SysLog>();
            list = SysLogService.GetModels(c => c.Id == Id).ToList();

            var json = new
            {
                row = list.Select(c => new
                {
                    Id = c.Id,
                    Operator = c.Operator,
                    Message = c.Message,
                    Result = c.Result,
                    Type = c.Type,
                    Module = c.Module,
                    CreateTime = c.CreateTime,
                })
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}