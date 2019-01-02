using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTest01
{
    class Program
    {
        
        static volatile string str = "";

        static void Main(string[] args)
        {
            //Interlocked
            //var name = Environment.MachineName;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            cancellationToken.ThrowIfCancellationRequested();
            //TaskCreationOptions creationOptions = new TaskCreationOptions();

            Task task = new Task(() => { });
            Task.Factory.StartNew(() =>
            {
                return string.Empty;
            }).ContinueWith((str) =>
            {
                
            });
            //task.Id
            //Task.CurrentId
            //TaskStatus.
            //task.RunSynchronously
            

            //TaskFactory factory = new TaskFactory();
            //factory.StartNew(() => { }).Start();
            Stopwatch stopwatch = Stopwatch.StartNew();

            ParallelAESHash();
            ParallelMd5Hash();

            Console.WriteLine($"Total: {stopwatch.ElapsedMilliseconds}");


            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            //queue.


            ConcurrentStack<string> stack = new ConcurrentStack<string>();
            
            //stack.TryPopRange()
            //stack.CopyTo()
            //stack.ToLookup
            BlockingCollection<string> collection = new BlockingCollection<string>();
            //collection.is
            //collection.BoundedCapacity
            collection.GetConsumingEnumerable();
            foreach (var col in collection.GetConsumingEnumerable())
            {

            }

            //PipelineFilter

            //SpinWait spin = new SpinWait();
            //SpinWait.SpinUntil()
            //SpinLock

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;

            Parallel.ForEach(Partitioner.Create(0, 10000), parallelOptions, range =>
            {
                
            });
            //lock (this)
            //{

            //}

            Barrier barrier = new Barrier(1);
            CountdownEvent countdownEvent = new CountdownEvent(2);
            ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim();
            
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
            SpinLock spinLock = new SpinLock();
            SpinWait spinWait = new SpinWait();

            ConcurrentDictionary<string, string> pairs = new ConcurrentDictionary<string, string>(); 
            //pairs.AddOrUpdate("","",)


            Console.ReadKey();

        }

        static void ParallelMd5Hash()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.For(0, 1000000, (i) =>
            {
                MD5 md5 = MD5.Create();
                //md5.ComputeHash()
                byte[] buffer = Encoding.UTF8.GetBytes("Md5 Hash" + i.ToString());
                byte[] data = md5.ComputeHash(buffer);
            });
            Console.WriteLine($"MD5: {stopwatch.ElapsedMilliseconds}");
        }

        static void ParallelAESHash()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.For(0, 1000000, (i) =>
            {
                AesManaged aesManaged = new AesManaged();
                aesManaged.GenerateKey();
                byte[] data = aesManaged.Key;
            });
            Console.WriteLine($"AES: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
