using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
    [Table("invoice", Schema = "public")]
    public partial class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int invoiceid { get; set; }
        public int? invoiceno { get; set; }
        public string orderno { get; set; }
        public double amount { get; set; }
        public bool? islock { get; set; }
        public string remark { get; set; }
        public int createdby { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }
        public DateTime createddate { get; set; }
        public int? invoiceyear { get; set; }

        public Company Company { get; set; }

        public int? companyid { get; set; }
        public int? venderid { get; set; }
        public DateTime invoicedate { get; set; }
        public DateTime duedate { get; set; }
        public string billingperiod { get; set; }
        public int? tocompanyid { get; set; }
        public bool? includetime { get; set; }
        public string currencycode { get; set; }
        public string imgname { get; set; }
    }
}
