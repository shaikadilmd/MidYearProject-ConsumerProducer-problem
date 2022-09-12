// See https://aka.ms/new-console-template for more information

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MidYearProject_ConsumerProducer_problem
{
    static class MainClass
    {
        private static Queue<int> numbers = new Queue<int>();
        private static Random rand = new Random(250);
        private const int NumThreads = 3;
        private static int[] sums = new int[NumThreads];

        static void ProduceNumbers()
        {
            for (int i = 0; i < 10; i++)
            {
                int numToEnqueue = rand.Next(10);
                Console.WriteLine("Producing thread adding "+ +numToEnqueue+ " to the queue");
                lock (numbers)
                    numbers.Enqueue(numToEnqueue);
                Thread.Sleep(rand.Next(1000));
            }
        }

        static void sumNumbers(object threadNumber)
        {
            DateTime startTime  = DateTime.Now;
            int mySum = 0;
            while ((DateTime.Now - startTime).Seconds < 11)
            {
                int numToSum = -1;
                lock (numbers)
                {
                    if (numbers.Count != 0)
                        numToSum = numbers.Dequeue();
                }

                if (numToSum != -1)
                {
                    mySum += numToSum;
                    Console.WriteLine("Consuming thread# "+threadNumber+" adding "+numToSum+" to its total sum making "+numToSum+" for the thread total");
                }
            }
            sums[(int)threadNumber] = mySum;
        }
        static void Main()
        {
            var producingThread = new Thread(ProduceNumbers);
            producingThread.Start();
            Thread[] threads = new Thread[NumThreads];
            for (int i = 0; i < NumThreads; i++)
            {
                threads[i] = new Thread(sumNumbers);
                threads[i].Start(i);
            }

            for (int i = 0; i < NumThreads; i++)
            {
                threads[i].Join();
            }

            int totalSum = 0;
            for (int i = 0; i < NumThreads; i++)
            {
                totalSum += sums[i];
            }
            Console.WriteLine("Done Adding.Total is "+ totalSum);
        }

    }
}
