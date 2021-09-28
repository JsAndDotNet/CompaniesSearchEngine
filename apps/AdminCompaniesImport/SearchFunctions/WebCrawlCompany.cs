using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SharedAzure.Dal;
using SharedAzure.Storage;

namespace SearchFunctions
{
    public static class WebCrawlCompany
    {
        [FunctionName("WebCrawlCompany")]
        public static async Task Run([ServiceBusTrigger("webcrawl", Connection = "Az-ServiceBus-Connection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var keys = myQueueItem.Split('|');
            var partitionKey = keys[0];
            var rowKey = keys[1];

            var storageConn = Environment.GetEnvironmentVariable("Az-Storage-Connection-Companies");
            var sbConn = Environment.GetEnvironmentVariable("Az-ServiceBus-Connection");

            IStorageProvider sp = new TableStorage();
            await sp.Initialize(storageConn, Shared.ENVIROVAR.STORAGE_COMPANIES_TABLE);
            var companiesDal = new CompanyAndWebsiteInfoDal(sp);
            var companyToSearch = await companiesDal.GetCompanyFromKeys(partitionKey, rowKey);


            log.LogInformation($"Searching: {companyToSearch.StockCode}");
            log.LogInformation($"Name: {companyToSearch.CompanyName}");

        }
    }
}
