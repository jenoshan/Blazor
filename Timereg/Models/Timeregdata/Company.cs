using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Timereg.Models.Timeregdata
{
    [Table("company", Schema = "public")]
    public partial class Company
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int companyid { get; set; }

        public string companyno { get; set; }
        public ICollection<Project> Project { get; set; }
        public ICollection<Invoice> Invoice { get; set; }
        public string companyname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string postalcode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string bankname { get; set; }
        public string branch { get; set; }
        public string accountname { get; set; }
        public string accountno { get; set; }
        public string swiftcode { get; set; }
        public string webaddress { get; set; }
    }
}
