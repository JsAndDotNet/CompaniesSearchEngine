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

            IStorageProvider sp = new TableStorage();
            var companiesDal = new CompanyAndWebsiteInfoDal(sp);
            var companies = await companiesDal.GetAllCompanies();


            foreach(var comp in companies)
            {

            }



            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;


            return new OkObjectResult("Companies Search Started");
        }
    }
}
