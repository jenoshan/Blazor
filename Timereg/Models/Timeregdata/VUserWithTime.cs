using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Models.Timeregdata
{
    [Table("v_userwithtime", Schema = "public")]
    public partial class VUserWithTime
    {
        public int userid { get; set; }
        //  public int manageruserid { get; set; }
        public string username { get; set; }
        public int usertime { get; set; }
        public int workyear { get; set; }
        public int workmonth { get; set; }
        public int weekno { get; set; }
        public int? companyid { get; set; }

        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
}
