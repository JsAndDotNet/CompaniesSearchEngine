using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedAzure.Storage;
using SharedAzure.Dal;
using SharedAzure.ServiceBus;

namespace SearchFunctions
{
    public static class StartWebCrawl
    {
        [FunctionName("StartWebCrawl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var storageConn = Environment.GetEnvironmentVariable("Az-Storage-Connection-Companies");
            var sbConn = Environment.GetEnvironmentVariable("Az-ServiceBus-Connection");
            
            IStorageProvider sp = new TableStorage();
            await sp.Initialize(storageConn, Shared.ENVIROVAR.STORAGE_COMPANIES_TABLE);
            var companiesDal = new CompanyAndWebsiteInfoDal(sp);
            SBSender sbSender = new SBSender(sbConn, "webcrawl");



            var companies = await companiesDal.GetAllCompanies();

            List<string> result = new List<string>();

            foreach (var comp in companies)
            {
                result.Add(comp.PartitionKey + "|" +  comp.RowKey);
            }


            sbSender.SendAsync(result);


            return new OkObjectResult("Companies Search Started");
        }
    }
}
