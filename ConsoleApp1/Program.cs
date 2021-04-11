using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ConsoleApp1
{

    
    class Program
    {
        static async Task WorkThenWait() {
            Thread.Sleep(1000);
            Console.WriteLine("work");
            await Task.Delay(1000);
        }

        
        static async Task Main(string[] args)
        {
            //var child = 
            Console.WriteLine("started");
            //await Task.Run(WorkThenWait);

            //child.Wait();
            Console.WriteLine("completed");
            
        }
    }
}