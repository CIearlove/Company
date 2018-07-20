using Company.DAL;
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
    public class LoginService : BaseService<LoginManager>, ILoginService
    {
        private ILoginDAL LoginDAL = Container.Resolve<ILoginDAL>();
        public override void SetDal()
        {
            Dal = LoginDAL;
        }
    }
}
