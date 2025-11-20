namespace Services;

public class MisCounter : ICounter
{
    private int _counter = 0;

    public void Increment()
    {
        Console.WriteLine(--_counter);
    }
    public int Value
    {
        get { return _counter; }
    }
}
