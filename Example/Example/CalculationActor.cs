using System;
using System.Linq;
using Akka.Actor;
using Example.Messages;

namespace Example
{
    class CalculationActor : ReceiveActor
    {
        public CalculationActor()
        {
            Receive<Finished>(f => Sender.Tell(f));
            Receive<Step1Item>(item =>
            {
                //Make some calculations here
                Sender.Tell(new Step2Item(item.Value));
            });

            Receive<Step3Item>(item =>
            {
                //Make some calculations here
                var total = item.Items.Sum(x => x);
                Sender.Tell(new Step4Item(item.Value + 1 + total));
            });
        }
    }
}
