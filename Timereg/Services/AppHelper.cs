using System.Collections.Generic;

namespace Timereg.Services
{
    public static class AppHelper
    {
        public static string DbUpdateString { get; set; }
        public static bool Isdbupdate { get; set; }

        public static Dictionary<int, string> CurrencyList()
        {
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            keyValuePairs.Add(1, "LKR");
            keyValuePairs.Add(2, "NOK");
            keyValuePairs.Add(3, "USD");
            keyValuePairs.Add(4, "CAD");
            keyValuePairs.Add(5, "SGD");

            return keyValuePairs;
        }

        public static Dictionary<int, string> LogoList()
        {
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            keyValuePairs.Add(1, "esys_solution.jpeg");
            keyValuePairs.Add(2, "esys.jpeg");
            keyValuePairs.Add(3, "swairi.png");

            return keyValuePairs;
        }
    }
}
