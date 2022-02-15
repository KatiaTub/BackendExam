using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackendExam
{
    class Program
    {
        static async Task Main(string[] args)
        {

                do
                {
                    Console.WriteLine("Press Enter to Start");

                } while (Console.ReadKey().Key != ConsoleKey.Enter);


                var token = new CancellationTokenSource();
                var process = new ProducerConsumer(token.Token);

                do
                {
                    Console.WriteLine("Press ESC to Terminate");

                } while (Console.ReadKey().Key != ConsoleKey.Escape);
            try
            {
                token.Cancel();
            }
            catch (OperationCanceledException) // includes TaskCanceledException
            {
                await process.WaitForCompletion();
            }   
            Console.WriteLine("Process Ended");    
          
        }
    }
}
