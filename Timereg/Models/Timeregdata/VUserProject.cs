using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
  [Table("v_userproject", Schema = "public")]
  public partial class VUserProject
  {
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
        public int? companyid
        {
            get;
            set;
        }
        public int userid
    {
      get;
      set;
    }
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
  }
}
