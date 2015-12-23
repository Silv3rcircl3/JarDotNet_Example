namespace Example.Messages
{
    class StartProcess
    {
        public StartProcess(int count)
        {
            Count = count;
        }

        public int Count { get; private set; }
    }
}