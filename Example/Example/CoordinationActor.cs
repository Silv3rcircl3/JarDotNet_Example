using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Example.Messages;

namespace Example
{
    class CoordinationActor : ReceiveActor
    {
        private readonly List<Step4Item> _step3Results = new List<Step4Item>();
        private readonly IActorRef _calculationActor;
        private IActorRef _originalSender;

        public CoordinationActor()
        {
            _calculationActor = Context.System.ActorOf<CalculationActor>("Calculator");

            Receive<StartProcess>(process =>
            {
                _originalSender = Sender;
                Become(ProcessingStep1(process.Count));
            });
        }

        private Receive ProcessingStep1(int count)
        {
            for (int i = 0; i < count; i++)
                _calculationActor.Tell(new Step1Item());
            _calculationActor.Tell(new Finished());

            return message =>
            {
                var result = message as Step2Item;
                if (result != null)
                {
                    _calculationActor.Tell(new Step3Item(result.Value));
                    return true;
                }

                var finished = message as Finished;
                if (finished != null)
                {
                    Become(ProcessingStep3);
                    _calculationActor.Tell(new Finished());
                }

                return false;
            };
        }

        private void ProcessingStep3()
        {
            int lowest = -1;
            int highest = -1;

            Receive<Step4Item>(item =>
            {
                //make some calculations here based on the incoming messages 
                if (lowest == -1 && highest == -1)
                {
                    lowest = item.Value;
                    highest = item.Value;
                }
                else
                {
                    if (lowest > item.Value)
                        lowest = item.Value;
                    else if (highest < item.Value)
                        highest = item.Value;
                }
                
                _step3Results.Add(item);
                
            });
            Receive<Finished>(_ =>
            {
                var repositoryResults = Repository.Fetch(_step3Results.Select(s => s.Value).ToList());
                _originalSender.Tell(new Result(repositoryResults, lowest, highest));
            });
        }
    }
}
