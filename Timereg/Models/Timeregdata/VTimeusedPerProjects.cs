using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Models.Timeregdata
{
    public  class VTimeusedPerProj
    {
     
        public string projectname { get; set; }
    
        public int totalminuts { get; set; }
    }
    public class VTimeusedPerProj2
    {

        public string projectname { get; set; }

        public string totalminuts { get; set; }
    }
    public class TimeusedPerProjMonth
    {
        public string month { get; set; }
        public int workmonth { get; set; }
        public string projectname { get; set; }
      
        public double totalhours { get; set; }
        public int approved { get; set; }
        public int registered { get; set; }
    }
    public class PPUSerMonth
    {
        public string country { get; set; }
        public int litres { get; set; }
        public List<subData> subDatas { get; set; }

    }
    public class subData
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    public class PPUSerMonth2
    {
        public string country { get; set; }
        public int litres { get; set; }
        public List<subData2> subDatas { get; set; }

    }
    public class subData2
    {
        public string name { get; set; }
        public int value { get; set; }
    }

}
