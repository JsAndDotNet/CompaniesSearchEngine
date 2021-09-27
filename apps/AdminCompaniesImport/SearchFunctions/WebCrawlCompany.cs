using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SearchFunctions
{
    public static class WebCrawlCompany
    {
        [FunctionName("WebCrawlCompany")]
        public static void Run([ServiceBusTrigger("webcrawl", Connection = "Az-ServiceBus-Connection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
