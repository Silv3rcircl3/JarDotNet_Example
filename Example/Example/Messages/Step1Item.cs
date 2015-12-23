using System;

namespace Example.Messages
{
    class Step1Item
    {
        private static readonly Random Generator = new Random();

        public Step1Item()
        {
            Value = Generator.Next(100);
        }

        public int Value { get; private set; }
    }
}