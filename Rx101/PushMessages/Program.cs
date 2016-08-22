using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PushMessages
{
    class Program
    {
        static void Main(string[] args)
        {
            var mgr = new SocialNetworksManager();

            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Loading messages");
            var messages = mgr.LoadMessages("Rx");
            foreach (var msg in messages)
            {
                Console.WriteLine($"Iterated:{msg} \t after {stopwatch.Elapsed}");
            }

            Console.WriteLine("--------------------");
            Console.WriteLine("Interactive messages");
            stopwatch.Restart();
            var interactiveMgr = new InteractiveSocialNetworksManager();
            interactiveMgr.MessageAvailable +=
                (sender, msg) => Console.WriteLine($"Observed:{msg} \t after {stopwatch.Elapsed}");
            interactiveMgr.LoadMessages("Rx");


            Console.WriteLine("--------------------");
            Console.WriteLine("Observing messages");
            stopwatch.Restart();
            
            mgr.ObserveLoadedMessages("Rx")
                .Subscribe(msg => Console.WriteLine($"Observed:{msg} \t after {stopwatch.Elapsed}"),
                    ex => { /*OnError*/ },
                    () => { /*OnCompleted*/ });

            Console.ReadLine();
        }
    }
}
