using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Timereg.Models;
using Timereg.Models.Timeregdata;
using Radzen;

namespace Timereg
{
    public class GlobalsService
    {

    }

    public class PropertyChangedEventArgs
    {
        public string Name { get; set; }
        public object NewValue { get; set; }
        public object OldValue { get; set; }
        public bool IsGlobal { get; set; }
    }
    public abstract class CheckboxList
    {
        public bool IsSelected { get; set; }
    }

    public class CheckboxList<T> : CheckboxList
    {
        public T Value { get; set; }
    }
    public class MonthResult
    {
        public int MonthNum { get; set; }
        public string MonthName { get; set; }
    }
}
    