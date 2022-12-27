using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{

    [Table("invoiceline", Schema = "public")]
    public partial class Invoiceline
    {

        [Key, Column(Order = 0)]
        public int invoiceid { get; set; }

        [Key, Column(Order = 0)]
        public int invoicelineid { get; set; }
        public double? amount { get; set; }
        public bool? islock { get; set; }
        public string description { get; set; }
        public int? employeeid { get; set; }
        public int? qty { get; set; }

        public Employee employee { get; set; }

    }
}
