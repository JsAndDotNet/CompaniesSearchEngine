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
        Task<List<CompanyAndWebsiteInfo>> GetAllCompanies();

        Task SaveCompanies(
            string connectionString,
            List<CompanyAndWebsiteInfo> companies);
    }

    public class CompanyAndWebsiteInfoDal : ICompanyAndWebsiteInfoDal
    {
        IStorageProvider _storageProvider;

        public CompanyAndWebsiteInfoDal(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task SaveCompanies(string connectionString,
            List<CompanyAndWebsiteInfo> companies)
        {
            _storageProvider.Initialize(connectionString, Shared.ENVIROVAR.STORAGE_COMPANIES_TABLE);

            //foreach(var company in companies)
            //{
            //    _storageProvider.UpsertAsnyc<CompanyAndWebsiteInfo>(company);
            //}


            _storageProvider.BatchUpsertAsync<CompanyAndWebsiteInfo>(companies);

            return Task.FromResult(companies);
        }

        public async Task<List<CompanyAndWebsiteInfo>> GetAllCompanies()
        {
            return await _storageProvider.GetAllCompanies();
        }
    }
}
