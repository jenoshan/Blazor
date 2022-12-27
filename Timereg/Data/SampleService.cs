using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Data
{
    public class SampleService
    {
        List<Sample> prodect = new List<Sample>()
        {
            new Sample(){SampleId=1,sampleName="Computer",Price=5000,Sale=5000,Date=Convert.ToDateTime("01-Mar-2019")},
            new Sample(){SampleId=2,sampleName="Laptop",Price=2000,Sale=3200,Date=Convert.ToDateTime("01-Mar-2019")},
            new Sample(){SampleId= 3,sampleName="Key",Price=2000,Sale=4500,Date=Convert.ToDateTime("01-Mar-2019")},
            new Sample(){SampleId=4,sampleName="Boart",Price=3000,Sale=36000,Date=Convert.ToDateTime("01-Mar-2019")},
            new Sample(){SampleId=5,sampleName="Boart3",Price=2000,Sale=3000,Date=Convert.ToDateTime("01-Mar-2019")},
            new Sample(){SampleId=6,sampleName="Boart2",Price=4000,Sale=5000,Date=Convert.ToDateTime("01-Mar-2019")},
            new Sample(){SampleId=7,sampleName="Boart1",Price=5000,Sale=5300,Date=Convert.ToDateTime("01-Mar-2019")},
        }; 

        public  async Task <List<Sample>> ProdectList()
        {
            return await Task.FromResult(prodect);
        }
    }

}
