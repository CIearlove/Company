using Company.Common;
using Company.DAL;
using Company.DALContainer;
using Company.IDAL;
using Company.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Company.BLL.Core
{
    /// <summary>
    /// 写入一个异常错误
    /// </summary>
    /// <param name="ex">异常</param>
    public static class ExceptionHander
    {
        private static ISysExceptionDAL sysExDAL = Container.Resolve<ISysExceptionDAL>();
        /// <summary>
        /// 加入异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        public static void WriteException(Exception ex)
        {
           
            try
            {
                using (sysExDAL)
                {
                    SysException model = new SysException()
                    {
                        HelpLink = ex.HelpLink,
                        Message = ex.Message,
                        Source = ex.Source,
                        StackTrace = ex.StackTrace,
                        TargetSite = ex.TargetSite.ToString(),
                        Data = ex.Data.ToString(),
                        CreateTime = ResultHelper.NowTime

                    };
                    sysExDAL.Add(model);
                    sysExDAL.SaveChanges();
                }
            }
            catch (Exception ep)
            {
                try
                {
                    //异常失败写入txt
                    string path = @"~/exceptionLog.txt";
                    
                    string txtPath = HttpContext.Current.Server.MapPath(path);//获取绝对路径
                    using (StreamWriter sw = new StreamWriter(txtPath, true, Encoding.Default))
                    {
                        sw.WriteLine((ex.Message + "|" + ex.StackTrace + "|" + ep.Message + "|" + DateTime.Now.ToString()).ToString());
                        sw.Dispose();
                        sw.Close();
                    }
                    return;
                    
                }
                catch { return; }
            }



        }
    }

}
