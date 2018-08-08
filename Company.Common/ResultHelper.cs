using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Common
{
    public class ResultHelper
    {
        /// <summary>
        /// 创建一个唯一的ID
        /// </summary>
        /// <returns>ID</returns>
        public static string NewId
        {
            get
            {
                string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                id += guid.Substring(0, 10);
                return id;
            }
        }
        public static string NewTimeId
        {
            get
            {
                string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                return id;
            }
        }

        //获取当前时间
        public static DateTime NowTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 将日期转换成字符串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>字符串</returns>
        public static string DateTimeConvertString(DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                return Convert.ToDateTime(dt.ToString()).ToShortDateString();
            }
        }

        /// <summary>
        /// 将字符串转换成日期
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>日期</returns>
        public static DateTime? StringConvertDatetime(string str)
        {
            if (str == null)
            {
                return null;
            }
            else
            {
                try
                {
                    return Convert.ToDateTime(str);
                }
                catch
                {
                    return null;
                }
            }
        }
        /*
        public static string GetUserIP()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        */
    }
}
