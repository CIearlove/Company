using Company.IDAL;
using Company.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL
{
    public class StaffDAL : BaseDAL<Staff>, IStaffDAL
    {
        //private DbContext dbContext = DbContextFactory.Create();
        public int DeleteRange(IEnumerable<Staff> t)
        {
            dbContext.Set<Staff>().RemoveRange(t);
            return dbContext.SaveChanges();
        }
    }
}
