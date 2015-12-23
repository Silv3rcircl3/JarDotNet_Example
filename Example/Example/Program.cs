using System;
using System.Linq;
using Akka.Actor;
using Example.Messages;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sys = ActorSystem.Create("Example"))
            {
                var writer = sys.ActorOf<ConsolenActor>();
                var coordinator = sys.ActorOf<CoordinationActor>("Coordinator");
                coordinator.Tell(new StartProcess(10), writer);

                sys.AwaitTermination();
            }
        }
    }

    class ConsolenActor : ReceiveActor
    {
        public ConsolenActor()
        {
            Receive<Result>(result =>
            {
                Console.WriteLine("Processing finished, results: ");
                Console.WriteLine($"Highest value: {result.Highest}  |  lowest value: {result.Lowest}");
                Console.WriteLine($"Processed items: {result.Value.Aggregate("", (s, i) => s + i + ", ")}");
            });
        }
    }
}
