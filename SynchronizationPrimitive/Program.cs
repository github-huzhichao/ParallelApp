using System;
using System.Collections.Generic;
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
                Task.WaitAll(_tasks);
                Console.WriteLine("All the phases were executed");
                _barrier.Dispose();
            });

            finalTask.Wait();

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
