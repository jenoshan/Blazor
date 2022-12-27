using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
    [Table("employee", Schema = "public")]
    public partial class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userid
        {
            get;
            set;
        }

        public ICollection<Timeused> Timeuseds { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public string username
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
        public double? salary { get; set; }
    }
}
