using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitive
{
    class SpinLockTest
    {
        private const string s = "";

        private int _participantNumber = Environment.ProcessorCount;
        private Barrier _barrier;
        private Task[] _tasks;

        public void SpinLockMethod()
        {
            this._tasks = new Task[this._participantNumber];
            this._barrier = new Barrier(this._participantNumber, (barrier) =>
            {
                Console.WriteLine($"Current phase :{barrier.CurrentPhaseNumber}");
            });
            var sl = new SpinLock(false);
            var sb = new StringBuilder();

            for (int i = 0; i < this._participantNumber; i++)
            {
                this._tasks[i] = Task.Factory.StartNew((num) =>
                {
                    var participantNumber = (int)num;
                    for (int j = 0; j < 10; j++)
                    {
                        this.CreatePlanets(participantNumber);
                        this._barrier.SignalAndWait();
                        this.CreateStarts(participantNumber);
                        this._barrier.SignalAndWait();
                        this.CheckCollisionsBetweenPlanets(participantNumber);
                        this._barrier.SignalAndWait();
                        this.CheckCollisionsBetweenStars(participantNumber);
                        this._barrier.SignalAndWait();
                        this.RenderCollisions(participantNumber);
                        this._barrier.SignalAndWait();

                        var logLine = $"Time: {DateTime.Now.TimeOfDay}, Phase: {this._barrier.CurrentPhaseNumber}, Participant: {participantNumber}, Phase completed OK";

                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            sb.Append(logLine);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                sl.Exit(false);
                            }
                        }
                    }
                }, i);
            }

            var finalTask = Task.Factory.ContinueWhenAll(this._tasks, (tasks) =>
            {
                Task.WaitAll(this._tasks);
                Console.WriteLine("All the phase were executed.");
                Console.WriteLine(sb);
                this._barrier.Dispose();
            });

            finalTask.Wait();
            Console.ReadKey();

        }


        private void CreatePlanets(int participantNum)
        {
            Console.WriteLine($"Creating planets. Participant #{participantNum}");
        }

        private void CreateStarts(int participantNum)
        {
            Console.WriteLine($"Creating starts. Participant #{participantNum}");
        }

        private void CheckCollisionsBetweenPlanets(int participantNum)
        {
            Console.WriteLine($"Checking collisions between planets .Participant # {participantNum}");
        }

        private void CheckCollisionsBetweenStars(int participantNum)
        {
            Console.WriteLine($"Checking collisions between stars. Participant # {participantNum}");
        }

        private void RenderCollisions(int participantNum)
        {
            Console.WriteLine($"Rendering collisions . Participant # {participantNum}");
        }

    }
}
