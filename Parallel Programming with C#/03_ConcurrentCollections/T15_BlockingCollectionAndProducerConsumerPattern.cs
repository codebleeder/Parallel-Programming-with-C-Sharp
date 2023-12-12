using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_._03_ConcurrentCollections
{
    internal class T15_BlockingCollectionAndProducerConsumerPattern
    {
        public static void Run()
        {
            // 10 specifies how many items we can have in the collection:
            var messages = new BlockingCollection<int>(new ConcurrentBag<int>(), 10);

            var cts = new CancellationTokenSource();
            var random = new Random();

            var runProducer = () =>
            {
                while(true)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    var i = random.Next(100);
                    messages.Add(i);
                    Console.WriteLine($"+{i}");
                    Thread.Sleep(500);
                }
            };

            var runConsumer = () =>
            {
                foreach( var i in messages.GetConsumingEnumerable() )
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"-{i}");
                    Thread.Sleep(1000);
                }
            };

            var produceAndConsume = () =>
            {
                var producer = Task.Factory.StartNew(runProducer);
                var consumer = Task.Factory.StartNew(runConsumer);

                try
                {
                    Task.WaitAll(new[] { producer, consumer }, cts.Token);
                }
                catch( AggregateException ex )
                {
                    ex.Handle(e => true);
                }
            };
            
            // main thread
            Task.Factory.StartNew(() => produceAndConsume(), cts.Token);

            Console.ReadKey();
            cts.Cancel();
        }

        public static void RunProducer(BlockingCollection<int> messages)
        {

        }

        public static void RunConsumer()
        {

        }
    }
}
