using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Shared.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzure.Storage
{
    public interface IStorageProvider
    {
        Task Initialize(string connectionString, string tableName);
        Task<List<CompanyAndWebsiteInfo>> GetAllCompanies();
        Task<T> GetByRowKeys<T>(string partitionKey, string rowKey) where T : AzEntityBase;
        Task<T> UpsertAsnyc<T>(T entity) where T : AzEntityBase;
        Task BatchUpsertAsync<T>(IEnumerable<T> data) where T : AzEntityBase;
        
    }

    public class TableStorage : IStorageProvider
    {
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;
        CloudTable _table;

        private string _conn;
        private string _tableName;

        public async Task Initialize(string connectionString, string tableName)
        {
            _conn = connectionString;
            _tableName = tableName;

            _storageAccount = CloudStorageAccount.Parse(_conn);


            _tableClient = _storageAccount.CreateCloudTableClient();
            _table = _tableClient.GetTableReference(_tableName);
            await _table.CreateIfNotExistsAsync();
        }


        /// <summary>
        /// Todo - make this generic soon.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyAndWebsiteInfo>> GetAllCompanies()
        {
            var entities = new List<CompanyAndWebsiteInfo>();

            var query = new TableQuery<CompanyAndWebsiteInfo>();
            TableContinuationToken continuationToken = null;
            do
            {
                var page = await _table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = page.ContinuationToken;
                entities.AddRange(page.Results);
            }
            while (continuationToken != null);

            return entities.ToList(); ;
        }


        public async Task<T> GetByRowKeys<T>(string partitionKey, string rowKey) where T: AzEntityBase
        {
            var ops = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var query = await _table.ExecuteAsync(ops);
            return query.Result as T;
        }




        public async Task<T> UpsertAsnyc<T>(T entity) where T : AzEntityBase
        {
            TableResult result = null;

            TableOperation insertOperation = TableOperation.InsertOrReplace(entity);

            result = await _table.ExecuteAsync(insertOperation);

            Console.WriteLine("Created item in database: {0}", result.Result.ToString());

            return entity;
        }



        public async Task BatchUpsertAsync<T>(IEnumerable<T> data) where T : AzEntityBase
        {

            int rowOffset = 0;

            while (rowOffset < data.Count())
            {
                // next batch
                var rows = data.Skip(rowOffset).Take(100).ToList();

                var countPartitionKeys = rows.Select(_ => _.PartitionKey).Distinct().Count();

                // Should not be hit in theory - move of a dev aid!
                if(countPartitionKeys > 1)
                {
                    throw new InvalidOperationException("Cannot handle batches with mixed partitionkeys");
                }

                rowOffset += rows.Count;

                var batch = new TableBatchOperation();

                foreach (var row in rows)
                {
                    batch.InsertOrReplace(row);
                }

                // submit
                await _table.ExecuteBatchAsync(batch);
            }
        }
    }
}
