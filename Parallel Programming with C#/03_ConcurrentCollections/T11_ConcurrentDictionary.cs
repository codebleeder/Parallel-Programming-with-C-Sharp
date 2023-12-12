using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_.ConcurrentCollections
{
    internal class T11_ConcurrentDictionary
    {

        public static void Run()
        {
            var dict = new ConcurrentDictionary<string, string>();

            // adding a value:
            dict.TryAdd("India", "Patliputra");
            dict.TryAdd("India", "Hampi"); // will not add as there is already an entry
            var val = "";
            var isPresent = false;
            isPresent = dict.TryGetValue("India", out val);
            if (isPresent)
            {
                Console.WriteLine(val);
            }

            // add directly
            dict["Russia"] = "Moscow";
            Console.WriteLine(dict["Russia"]);
            dict["Russia"] = "St. Petersburg"; // This will replace Moscow 
            Console.WriteLine(dict["Russia"]);

            // add or update
            dict["Israel"] = "Tel Aviv";
            var newValue = "Jerusalem";
            // if key Israel doesn't exist, then 
            dict.AddOrUpdate("Israel", newValue,
                (key, old) => old + " " + newValue);
            Console.WriteLine(dict["Israel"]);
        }
    }
}
