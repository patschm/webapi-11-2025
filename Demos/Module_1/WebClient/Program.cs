namespace WebClient;

public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.Create(args);
        
        app.UseStaticFiles();

        app.Run();
    }
}
