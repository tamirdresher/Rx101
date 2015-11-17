using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;

namespace HistoricalSchedulerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Install-Package Rx-Testing

            Subject<int> numbers = new Subject<int>();
            var historicalScheduler = new HistoricalScheduler(new DateTime(2015, 11, 21, 17, 10, 0));
            historicalScheduler.Schedule(new DateTime(2015, 11, 21, 17, 10, 0), () => numbers.OnNext(1));
            historicalScheduler.Schedule(new DateTime(2015, 11, 21, 17, 11, 0), () => numbers.OnNext(2));
            historicalScheduler.Schedule(new DateTime(2015, 11, 21, 17, 32, 0), () => numbers.OnNext(3));
            historicalScheduler.Schedule(new DateTime(2015, 11, 21, 17, 39, 0), () => numbers.OnNext(4));
            historicalScheduler.Schedule(new DateTime(2015, 11, 21, 17, 51, 0), () => historicalScheduler.Stop());

            numbers.AsObservable()
                .Buffer(TimeSpan.FromMinutes(20),historicalScheduler)
                .Subscribe((buffer) =>
                {
                    Console.WriteLine("time is: {0}",historicalScheduler.Now);
                    Console.WriteLine("Buffer:");
                    foreach (var x in buffer)
                    {
                        Console.WriteLine("\t{0}", x);
                    }
                });

            historicalScheduler.Start();

            Console.WriteLine("Press <enter> to continue");
            Console.ReadLine();
        }
    }
}
