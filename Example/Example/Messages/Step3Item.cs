using System.Collections.Generic;
using System.Linq;

namespace Example.Messages
{
    class Step3Item
    {
        public Step3Item(int value, IEnumerable<int> items)
        {
            Value = value;
            Items = items.ToArray();
        }

        public int Value { get; private set; }
        public IEnumerable<int> Items { get; private set; }
    }
}
