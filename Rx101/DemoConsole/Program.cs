using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Observable.Range(1, 10)
                .Subscribe(x => Console.WriteLine(x));

            var subscription =
                Observable.Interval(TimeSpan.FromSeconds(1))
                    .Subscribe(x => Console.WriteLine(x));




            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();


        }
    }
}
