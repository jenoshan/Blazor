using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
    [Table("projects", Schema = "public")]
    public partial class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id
        {
            get;
            set;
        }

        public ICollection<Timeused> Timeuseds { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public string projectname
        {
            get;
            set;
        }
        public string projectcode
        {
            get;
            set;
        }
        public string color
        {
            get;
            set;
        }
        public DateTime validfrom
        {
            get;
            set;
        }
        public DateTime? validto
        {
            get;
            set;
        }

        public int? companyid
        {
            get;
            set;
        }

        public int? projecttype
        {
            get;
            set;
        }

        public Company company
        {
            get; set;
        }
    }
}
