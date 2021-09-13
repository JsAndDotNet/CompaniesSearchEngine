

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        var secretProvider = config.Providers.First();
        if (!secretProvider.TryGet("JJ-TestSecret", out var secretPass)) return;

        Console.WriteLine(secretPass);
        Console.ReadLine();
    }
}