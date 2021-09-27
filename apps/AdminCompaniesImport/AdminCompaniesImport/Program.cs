

using AdminCompaniesImport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared.Classes;
using SharedAzure.Dal;
using SharedAzure.Storage;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Task.Run(() => MainTask());
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.ReadLine();
        }
        
    }


    private static async Task MainTask()
    {
        try
        {
            Console.WriteLine("Start");

            ICsvFileHandler fileHandler = new CsvFileHandler();
            IStorageProvider sp = new TableStorage();
            ICompanyAndWebsiteInfoDal dal = new CompanyAndWebsiteInfoDal(sp);

            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            var secretProvider = config.Providers.First();
            if (!secretProvider.TryGet("Az-Storage-Connection-Companies", out var conn))
            {
                Console.WriteLine("Could not init storage");
                Console.ReadLine();
                return;
            }


            // Check file path validity
            string filePath = @"C:\Git\CompaniesSearchEngine\apps\AdminCompaniesImport\AdminCompaniesImport\input-example\Company_And_Websites_truncated.csv";
            //bool isValid = false;
            //do
            //{
            //    Console.WriteLine("Enter the file path for the URLs");
            //    filePath = Console.ReadLine();
            //    if (!File.Exists(filePath))
            //    {
            //        Console.WriteLine("File does not exist. Try again.");
            //        Console.WriteLine();
            //        continue;
            //    }

            //    isValid = true;

            //} while (!isValid);


            // Get companies
            var companies = fileHandler.GetCompaniesFromFile(filePath);

            await dal.SaveCompanies(conn, companies);

            Console.WriteLine("Done");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.ReadLine();
        }
    }
}

