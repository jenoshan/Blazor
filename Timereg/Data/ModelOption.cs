using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Data
{
    public class ModelOption
    {
        public string Position { get; set; }
        public string Style { get; set; }
        public bool? DisableBackgrountCancel { get; set; }
        public bool? HidenHeader { get; set; }
        public bool? HideClouseButton { get; set; }
    }
}
