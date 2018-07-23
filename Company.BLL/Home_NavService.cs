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
    public class Home_NavService : BaseService<Home_Nav>, IHome_NavService
    {
        private IHome_NavDAL Home_NavDAL = Container.Resolve<IHome_NavDAL>();
        public override void SetDal()
        {
            Dal = Home_NavDAL;
        }
    }
}
