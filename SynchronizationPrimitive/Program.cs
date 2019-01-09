using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitive
{
    class Program
    {
        private static int _participants = Environment.ProcessorCount;
        private static Task[] _tasks;
        private static Barrier _barrier;

        static void Main(string[] args)
        {
            SpinLockTest spinLockTest = new SpinLockTest();
            spinLockTest.SpinLockMethod();


            //Monitor.Enter("");
            //Monitor.Exit("");
            //Monitor.Pulse()
            //Monitor.Wait()

            Stopwatch stopwatch = Stopwatch.StartNew();
            _tasks = new Task[_participants];
            _barrier = new Barrier(_participants, (barrier) =>
            {
                Console.WriteLine($"Current phase: {barrier.CurrentPhaseNumber}");
            });
            for (int i = 0; i < _participants; i++)
            {
                _tasks[i] = Task.Factory.StartNew(num =>
                {
                    var participantNumber = (int)num;
                    for (int j = 0; j < 10; j++)
                    {
                        CreatePlanets(participantNumber);
                        _barrier.SignalAndWait();
                        //_barrier.SignalAndWait()
                        CreateStarts(participantNumber);
                        _barrier.SignalAndWait();
                        CheckCollisionsBetweenPlanets(participantNumber);
                        _barrier.SignalAndWait();
                        CheckCollisionsBetweenStars(participantNumber);
                        _barrier.SignalAndWait();
                        RenderCollisions(participantNumber);
                        _barrier.SignalAndWait();
                    }
                }, i);
            }
            var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
            {
                //Task.WaitAll(tasks);
                Task.WaitAll(_tasks);
                Console.WriteLine("All the phases were executed");
                _barrier.Dispose();
            });
            //var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
            //{
            //    Task.WaitAll(_tasks);
            //    Console.WriteLine("All the phases were executed");
            //    _barrier.Dispose();
            //});
            finalTask.Wait();

            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            #region MyRegion
            //Stopwatch stopwatch1 = Stopwatch.StartNew();

            //Task[] _tasks1 = new Task[_participants];

            //for (int i = 0; i < _participants; i++)
            //{
            //    var t = Task.Factory.StartNew(() =>
            //    {
            //        for (int j = 0; j < 10; j++)
            //        {
            //            CreatePlanets(j);
            //        }
            //    }).ContinueWith((task) =>
            //    {
            //        for (int j = 0; j < 10; j++)
            //        {
            //            CreateStarts(i);
            //        }
            //    }).ContinueWith((task) =>
            //    {
            //        for (int j = 0; j < 10; j++)
            //        {
            //            CheckCollisionsBetweenPlanets(i);
            //        }
            //    }).ContinueWith((task) =>
            //    {
            //        for (int j = 0; j < 10; j++)
            //        {
            //            CheckCollisionsBetweenStars(i);
            //        }
            //    }).ContinueWith((task) =>
            //    {
            //        for (int j = 0; j < 10; j++)
            //        {
            //            RenderCollisions(i);
            //        }
            //    });
            //    _tasks1[i] = t;

            //}

            //var finalTask1 = Task.Factory.ContinueWhenAll(_tasks1, (tasks) =>
            //{
            //    Task.WaitAll(tasks);
            //    Console.WriteLine("All the phases were executed");
            //});
            ////var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
            ////{
            ////    Task.WaitAll(_tasks);
            ////    Console.WriteLine("All the phases were executed");
            ////    _barrier.Dispose();
            ////});
            //finalTask1.Wait();
            //Console.WriteLine(stopwatch1.ElapsedMilliseconds); 
            #endregion


            Console.ReadLine();

        }

        static void CreatePlanets(int participantNum)
        {
            Console.WriteLine($"Creating planets. Participant #{participantNum}");
        }

        static void CreateStarts(int participantNum)
        {
            Console.WriteLine($"Creating starts. Participant #{participantNum}");
        }

        static void CheckCollisionsBetweenPlanets(int participantNum)
        {
            Console.WriteLine($"Checking collisions between planets .Participant # {participantNum}");
        }

        static void CheckCollisionsBetweenStars(int participantNum)
        {
            Console.WriteLine($"Checking collisions between stars. Participant # {participantNum}");
        }

        static void RenderCollisions(int participantNum)
        {
            Console.WriteLine($"Rendering collisions . Participant # {participantNum}");
        }
    }
}
