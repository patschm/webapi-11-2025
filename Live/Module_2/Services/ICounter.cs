namespace Services
{
    public interface ICounter
    {
        int Value { get; }

        void Increment();
    }
}