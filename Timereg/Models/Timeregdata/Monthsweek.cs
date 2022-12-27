using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Models.Timeregdata
{
    public class Monthsweek
    {
        public string week { get; set; }
        public int weekid { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
    public class Year
    {
        public string Yearname { get; set; }
        public int Yearid { get; set; }
    
    }
}

