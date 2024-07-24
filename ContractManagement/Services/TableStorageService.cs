using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
//using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ContractManagement.Models;

namespace ContractManagement.Services
{
    public class TableStorageService
    {

        private readonly TableClient _tableClient;

        public TableStorageService(string connectionString, string tableName)
        {
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient(tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<List<Contract>> GetContractsAsync()
        {
            var contracts = new List<Contract>();
            await foreach (var contract in _tableClient.QueryAsync<Contract>())
            {
                contracts.Add(contract);
            }
            return contracts;
        }

        public async Task<Contract> GetContractAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<Contract>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task AddOrUpdateContractAsync(Contract contract)
        {
            await _tableClient.UpsertEntityAsync(contract);
        }
        public async Task UpdateEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
        {
           // var tableClient = _tableServiceClient.GetTableClient(tableName);
            await _tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        }
        public async Task DeleteContractAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
