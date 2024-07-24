using Azure;
using Azure.Data.Tables;
using System;
//using System.ComponentModel.DataAnnotations;

namespace ContractManagement.Models
{
    public class Contract : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ContractName { get; set; }
        public string ClientName { get; set; }
        public string BlobName { get; set; }

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
