using System;
using System.Collections.Generic;
using System.Threading;

namespace Multi_Threading
{
    class Program
    {
        private Int32 SharedInteger { get; set; } = 0;
        private Mutex Mutex = new Mutex();
        private static int FirstHorseNb { get; set; } = 0;
        private static string FirstHorseName { get; set; } = null;

        static void Main(string[] args)
        {
            var program = new Program();
            var threadStartDelegate = new ThreadStart(program.OnThreadStart);

            var threads = new List<Thread> {
                new Thread(threadStartDelegate){Name = "Spirit"},
                new Thread(threadStartDelegate){Name = "Maximus"}
            };

            foreach (var t in threads)
            {
                t.Start(); 
                
            }

            foreach (var t in threads)
            {
                t.Join(); 

            }

            Console.WriteLine("The Winner is Horse n° {0} : The Magnificient {1}", FirstHorseNb, FirstHorseName);


        }

        private void OnThreadStart()
        {
            var random = new Random();
            var executionTime = random.Next(1000);

            Console.WriteLine("Horse n° {0} : {1} starts to run", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
            Thread.Sleep(executionTime);
            Mutex.WaitOne();

            if (FirstHorseNb == 0)
            {
                FirstHorseNb = Thread.CurrentThread.ManagedThreadId;
                FirstHorseName = Thread.CurrentThread.Name;
            }

            Console.WriteLine("Horse n° {0} : {1} cross the finish line !!!", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);

            Mutex.ReleaseMutex();
            Console.WriteLine("Result : {0} time = {1}", Thread.CurrentThread.Name, executionTime);


        }
    }
}
