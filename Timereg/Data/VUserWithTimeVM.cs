using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Data
{
    public class VUserWithTimeVM
    {
        public int userid { get; set; }
        public string username { get; set; }
        public double UserTime { get; set; }
        public int workyear { get; set; }
        public string workmonth { get; set; }
        public int workmonthnum { get; set; }
        public int Approved { get; set; }
        public int registered { get; set; }
        public bool IsApproved { get; set; }
    }
}
