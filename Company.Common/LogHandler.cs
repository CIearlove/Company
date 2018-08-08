using Company.BLL;
using Company.DAL;
using Company.DALContainer;
using Company.IBLL;
using Company.IDAL;
using Company.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Company.Common
{
    public static class LogHandler
    {
        //public static ISysLogService SysLogService { get; set; }
        //private ISysLogService SysLogService = Container.Resolve<ISysLogService>();
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="oper">操作人</param>
        /// <param name="mes">操作信息</param>
        /// <param name="result">结果</param>
        /// <param name="type">类型</param>
        /// <param name="module">操作模块</param>
        public static void WriteServiceLog(string oper, string mes, string result, string type, string module)
        {


            SysLog entity = new SysLog();
            entity.Operator = oper;
            entity.Message = mes;
            entity.Result = result;
            entity.Type = type;
            entity.Module = module;
            entity.CreateTime = ResultHelper.NowTime;

            SysLogService log = new SysLogService();
            log.Add(entity);
           
            
        }
    }
}
