

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        var secretProvider = config.Providers.First();
        if (!secretProvider.TryGet("JJ-TestSecret", out var secretPass)) return;


        bool isValid = false;


        // List<CompanyInfo> companyInfo = new List<CompanyInfo>();

        do
        {
            Console.WriteLine("Enter the file path for the URLs");
            var filePath = Console.ReadLine();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist. Try again.");
                Console.WriteLine();
                continue;
            }


            var file = File.ReadAllLines(filePath);

            // Csv in format
            // Indices	Stock Code	Yh Code	Company Name	Website	Description

            foreach(var row in file)
            {
                
                var split = row.Split(',');
                if(split != null && split.Length == 6)
                {
                    var indices = split[0];
                    if(indices.ToLower() == "indices")
                    {
                        continue;
                    }

                    var stockCode = split[1];
                    var yhCode = split[2];
                    var companyName = split[3];
                    var website = split[4];
                    var description = split[5];

                    Uri uriResult;
                    bool isValidUrl = Uri.TryCreate(website, UriKind.Absolute, out uriResult)
    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if(!isValidUrl)
                    {
                        Console.WriteLine($"Invalid URL {companyName}, {website}");
                        Console.WriteLine($"Press enter to continue, or close window to abort.");
                        Console.ReadLine();
                    }




                }

                isValid = true;

            }





        } while (!isValid);







    }
}