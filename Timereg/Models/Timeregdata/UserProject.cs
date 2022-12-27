using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
    [Table("userproject", Schema = "public")]
    public partial class UserProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int upid
        {
            get;
            set;
        }
        public int? projectid
        {
            get;
            set;
        }
        public Project Project { get; set; }
        public int userid
        {
            get;
            set;
        }
        public Employee Employee { get; set; }
    }
}
