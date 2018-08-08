using Company.BLLContainer;
using Company.Common;
using Company.Common.Staff;
using Company.IBLL;
using Company.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Company.UI.Controllers
{
    public class StaffController : DefaultController
    {
        private IStaffService StaffService = Container.Resolve<IStaffService>();

        //private ISysLogService SysLogService = Container.Resolve<ISysLogService>();

        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        #region 列表（初始化+查询）
        [HttpPost]
        public JsonResult GetList(GridPager pager, StaffSearch staffSearch)
        {
            List<Staff> list = new List<Staff>();
            DateTime defaultDateTime = new DateTime(1, 1, 1, 0, 0, 0);
            //staffSearch.name = "";
            /*
             * 初始化页面:只有pager接收到浏览器传来的参数，staffSearch中的属性均为默认值。
             */
            if (staffSearch.name == null && staffSearch.date_from.Value == defaultDateTime && staffSearch.date_to == defaultDateTime)
            {
                pager.totalRows = StaffService.GetModels(c => true).ToList().Count();
                //初始化页面根据Id排序
                list = StaffService.GetModelsByPage(pager.rows, pager.page, pager.isAsc, c => c.Id, c => true).ToList();
            }
            /*
             * 查询页面：staffSearch接收到浏览器传来的值，staffSearch中的属性均变为自定义的默认值。
             */
            else
            {
                //先想一想SQL怎么写，再写Lambda表达式
                Expression<Func<Staff, bool>> whereLambda = u =>
               (u.CreateTime >= staffSearch.date_from && u.CreateTime <= staffSearch.date_to)
               && u.Name.Contains(staffSearch.name);
                //查询页面根据时间增序排列
                Expression<Func<Staff, DateTime?>> orderByLambda = c => c.CreateTime;

                pager.totalRows = StaffService.GetModels(whereLambda).ToList().Count();
                list = StaffService.GetModelsByPage(pager.rows, pager.page, pager.isAsc, orderByLambda, whereLambda).ToList();

            }

            var json = new
            {
                total = pager.totalRows,
                /*
                rows = (from r in list
                        select new Staff()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Age = r.Age,
                            Sex = r.Sex,
                            CreateTime = r.CreateTime,
                        }).ToArray()
                        */

                rows = list.Select(staff => new
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Age = staff.Age,
                    Sex = staff.Sex,
                    CreateTime = staff.CreateTime
                }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 批量删除
        public JsonResult Delete(int[] Ids)
        {
            IQueryable<Staff> staffs = StaffService.GetModels(p => Ids.Contains(p.Id));
            int count = StaffService.DeleteRange(staffs);

            var json = new
            {
                deletedNum = count,
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新增
        public JsonResult Add(Staff staff)
        {
            if (StaffService.Add(staff))
            {
                LogHandler.WriteServiceLog("虚拟用户", "Id:" + staff.Id + ",Name:" + staff.Name, "成功", "创建", "样例程序");
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHandler.WriteServiceLog("虚拟用户", "Id:" + staff.Id + ",Name:" + staff.Name, "失败", "创建", "样例程序");
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 修改   
        public JsonResult Update(Staff staff)
        {
            if (StaffService.Update(staff))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        # region 导出
        public void Export(StaffSearch staffExport)
        {
            List<Staff> list = new List<Staff>();

            Expression<Func<Staff, bool>> whereLambda = u =>
               (u.CreateTime >= staffExport.date_from && u.CreateTime <= staffExport.date_to)
               && u.Name.Contains(staffExport.name);

            list = StaffService.GetModels(whereLambda).ToList();

            //XSSFWorkbook 用于创建  .xlsx

            //1.创建EXCEL中的Workbook
            IWorkbook myXSSFworkbook = new XSSFWorkbook();

            //2.创建Workbook中的Sheet
            ISheet mysheetXSSF = myXSSFworkbook.CreateSheet("sheet1");

            //3.给sheet1添加第一行的头部标题
            IRow row1 = mysheetXSSF.CreateRow(0);
            row1.CreateCell(0).SetCellValue("名称");
            row1.CreateCell(1).SetCellValue("年龄");
            row1.CreateCell(2).SetCellValue("性别");
            row1.CreateCell(3).SetCellValue("进入公司时间");
            //设置单元格格式
            ICellStyle style1 = myXSSFworkbook.CreateCellStyle();
            style1.Alignment = HorizontalAlignment.Center;//【Center】水平居中
            style1.VerticalAlignment = VerticalAlignment.Center;//【Center】垂直居中
            for (int jj = 0; jj < 4; jj++)
            {
                row1.GetCell(jj).CellStyle = style1;
            }
            
            //设置单元格格式
            ICellStyle style = myXSSFworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;//【Center】水平居中
            style.VerticalAlignment = VerticalAlignment.Center;//【Center】垂直居中

            //4.将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                IRow rowtemp = mysheetXSSF.CreateRow(i + 1);
                
                rowtemp.CreateCell(0).SetCellValue(list[i].Name);
                rowtemp.CreateCell(1).SetCellValue(list[i].Age);
                rowtemp.CreateCell(2).SetCellValue(list[i].Sex);
                rowtemp.CreateCell(3).SetCellValue(list[i].CreateTime.Value.ToString());

                for (int j = 0; j < 4; j++)
                {
                    rowtemp.GetCell(j).CellStyle = style;
                }
            }
            //5.设置格式

            //设置列宽   SetColumnWidth(列的索引号从0开始, N * 256) 第二个参数的单位是1/256个字符宽度。例：将第四列宽度设置为了30个字符。
            mysheetXSSF.SetColumnWidth(3, 30 * 256);

            //设置默认行高   Height的单位是1/20个点。例：设置高度为50个点
            mysheetXSSF.DefaultRowHeight = 50 * 20;

            //6.保存
            FileStream fileXSSF = new FileStream(@"D:\Work\Codes\JITC#_Exercises\TestExport\myXSSFworkbook.xlsx", FileMode.Create);
            myXSSFworkbook.Write(fileXSSF);
            fileXSSF.Close();
        }
        #endregion

    }
}