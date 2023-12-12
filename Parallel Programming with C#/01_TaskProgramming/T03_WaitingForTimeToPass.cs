using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    // we can wait for cancellation for a certain time: 
    internal class T03_WaitingForTimeToPass
    {
        public static void Run()
        {
            var tokenSrc = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                Console.WriteLine("you have 5 mins to disarm the bomb");
                // wait for 5 seconds
                // waitOne returns bool - indicates if the token was cancelled within
                // specified time or not: 
                var cancelled = tokenSrc.Token.WaitHandle.WaitOne(5000);
                Console.WriteLine(cancelled ? "bomb disarmed" : "BOOM!!");
            }, tokenSrc.Token);            

            Console.ReadKey();
            tokenSrc.Cancel();

            Console.ReadKey();
            Console.WriteLine("main program done");
            Console.ReadKey();
        }
    }
}
