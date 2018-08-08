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
    public class SysLogService : BaseService<SysLog>, ISysLogService
    {
        private ISysLogDAL SysLogDAL = Container.Resolve<ISysLogDAL>();
        public override void SetDal()
        {
            Dal = SysLogDAL;
        }
    }
}
