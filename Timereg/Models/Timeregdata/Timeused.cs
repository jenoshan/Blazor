using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
    [Table("timeused", Schema = "public")]
    public partial class Timeused
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tid
        {
            get;
            set;
        }
        public DateTime workdate
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
        public TimeSpan usedtime
        {
            get;
            set;
        }
        public int? approved  //alter TABLE [dbo].[Timeused] add Approved int
        {
            get;
            set;
        }
        public DateTime? timefrom
        {
            get;
            set;
        }
        public DateTime? timeto
        {
            get;
            set;
        }
        public string description { get; set; }
    }
}
