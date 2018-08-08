using Company.DALContainer;
using Company.IBLL;
using Company.IDAL;
using Company.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL
{
    public class SysExceptionService : BaseService<SysException>, ISysExceptionService
    {
        private ISysExceptionDAL sysExceptionDAL = Container.Resolve<ISysExceptionDAL>();
        public override void SetDal()
        {
            Dal = sysExceptionDAL;
        }
    }
}
