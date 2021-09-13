using Shared.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCompaniesImport
{
    public interface ICsvFileHandler
    {
        List<CompanyAndWebsiteInfo> GetCompaniesFromFile(string filePath);
    }

    public class CsvFileHandler : ICsvFileHandler
    {

        public List<CompanyAndWebsiteInfo> GetCompaniesFromFile(string filePath)
        {
            List<CompanyAndWebsiteInfo> companies = new List<CompanyAndWebsiteInfo>();

            if (!File.Exists(filePath))
            {
                return companies;
            }

            var file = File.ReadAllLines(filePath);

            // Csv in format
            // Indices	Stock Code	Yh Code	Company Name	Website	Description

            foreach (var row in file)
            {
                var split = row.Split(',');
                if (split != null && split.Length == 6)
                {
                    var indices = split[0];
                    if (indices.ToLower() == "indices")
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

                    if (!isValidUrl)
                    {
                        Console.WriteLine($"Invalid URL {companyName}, {website}");
                        Console.WriteLine($"Press enter to continue, or close window to abort.");
                        Console.ReadLine();
                    }


                    CompanyAndWebsiteInfo co = new CompanyAndWebsiteInfo(indices, stockCode, companyName, website, description);
                    companies.Add(co);
                }
            }

            return companies;
        }

    }
}
