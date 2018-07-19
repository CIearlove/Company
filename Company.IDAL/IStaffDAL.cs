using Company.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.IDAL
{
    public interface IStaffDAL:IBaseDAL<Staff>
    {
        //批量删除，并返回删除的个数
        int DeleteRange(IEnumerable<Staff> t);
        
    }
}
