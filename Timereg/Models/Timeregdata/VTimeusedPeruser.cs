using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Models.Timeregdata
{
    [Table("v_timeusedperuser", Schema = "public")]
    public partial class VTimeusedPeruser
    {
        public int workyear { get; set; }
        public int workmonth { get; set; }
        public int weekno { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string projectname { get; set; }
        public string projectcode { get; set; }
        public string username { get; set; }
        public int totalminuts { get; set; }
        public int userid { get; set; }
        public int? companyid { get; set; }
        public int? invoiceid { get; set; }
        //  public int manageruserid { get; set; }
        public int approved { get; set; }
        public int registered { get; set; }
    }

}
