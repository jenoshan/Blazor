using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timereg.Models.Timeregdata
{
  [Table("v_timeused", Schema = "public")]
  public partial class VTimeused
  {
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
    public int userid
    {
      get;
      set;
    }
    public TimeSpan usedtime
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
    public string color
    {
      get;
      set;
    }
       // public string username { get; set; }
    }
}
