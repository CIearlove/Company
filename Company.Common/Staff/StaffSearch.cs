using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Common.Staff
{
    public class StaffSearch
    {
        private string _name { get; set; }//查询条件：名称
        public string name
        {
            get
            {
                return _name;
            }
            set {
                if (value == null)
                {
                    _name = "";
                }
                else
                {
                    _name = value;
                }
            }
        }
        private DateTime _date_from { get; set; }
        public DateTime? date_from
        {
            get { return _date_from; }
            set
            {
                if (value == null)
                {
                    _date_from = DateTime.Now.AddYears(-100);
                }
                else
                {
                    _date_from = value ?? DateTime.Now;
                }
            }
        }//查询条件：进入公司时间从

        private DateTime _date_to { get; set; }
        public DateTime? date_to
        {
            get { return _date_to; }
            set
            {
                if (value == null)
                {
                    _date_to = DateTime.Now.AddYears(100);
                }
                else
                {
                    _date_to = value ?? DateTime.Now;
                }
            }
        }//查询条件：到进入公司时间
    }
}
