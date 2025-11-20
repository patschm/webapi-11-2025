namespace Services;

public class Counter : ICounter
{
    private int _counter = 0;

    public void Increment()
    {
        Console.WriteLine(++_counter);
    }
    public int Value
    {
        get { return _counter; }
    }
}
