using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Data
{
    public class Sample
    {
        public int SampleId { get; set; }
        public string sampleName { get; set; }
        public double    Price { get; set; }
        public double    Sale { get; set; }
        public DateTime    Date { get; set; }

        public string Profit => ((this.Sale - this.Price)*100)/this.Price+"%";
    }
}
