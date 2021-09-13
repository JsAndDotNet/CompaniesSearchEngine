using Shared.Classes;
using SharedAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzure.Dal
{
    public interface ICompanyAndWebsiteInfoDal
    {
        Task SaveCompanies(
            string connectionString,
            List<CompanyAndWebsiteInfo> companies);
    }

    public class CompanyAndWebsiteInfoDal : ICompanyAndWebsiteInfoDal
    {
        IStorageProvider _storageProvider;
        string _tableName = "companyandwebsiteinfo";

        public CompanyAndWebsiteInfoDal(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }





        public Task SaveCompanies(string connectionString,
            List<CompanyAndWebsiteInfo> companies)
        {
            _storageProvider.Initialize(connectionString, _tableName);

            _storageProvider.BatchUpsertAsync<CompanyAndWebsiteInfo>(companies);

            return Task.FromResult(companies);
        }
    }
}
