using System.Collections.Generic;

namespace Example.Messages
{
    class Result
    {
        public Result(List<int> value, int lowest, int highest)
        {
            Value = value;
            Lowest = lowest;
            Highest = highest;
        }

        public List<int> Value { get; private set; }

        public int Lowest { get; private set; }

        public int Highest { get; private set; }
    }
}