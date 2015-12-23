using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Example.Messages;

namespace Example
{
    class CoordinationActor : ReceiveActor
    {
        private readonly HashSet<Step2Item> _step1Results = new HashSet<Step2Item>();
        private readonly HashSet<Step4Item> _step2Results = new HashSet<Step4Item>();
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
                    _step1Results.Add(result);
                    return true;
                }

                var finished = message as Finished;
                if (finished != null)
                {
                    Become(ProcessingStep2(count));
                }

                return false;
            };
        }

        private Receive ProcessingStep2(int count)
        {
            var repositoryResults = Repository.Fetch(_step1Results.Select(s => s.Value).ToList());

            for (int i = 0; i < count; i++)
                _calculationActor.Tell(new Step3Item(i, repositoryResults));
            _calculationActor.Tell(new Finished());

            return message =>
            {
                var result = message as Step4Item;
                if (result != null)
                {
                    _step2Results.Add(result);
                    return true;
                }

                var finished = message as Finished;
                if (finished != null)
                {
                    int lowest = repositoryResults.Min(x => x);
                    int highest = repositoryResults.Max(x => x);
                    _originalSender.Tell(new Result(repositoryResults, lowest, highest));
                }

                return false;
            };
        }
    }
}
