
using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace ContractManagement.Services
{
 

    public class QueueStorageService
    {
        private readonly QueueServiceClient _queueServiceClient;

        public QueueStorageService(string connectionString)
        {
            _queueServiceClient = new QueueServiceClient(connectionString);
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            try
            {
                var queueClient = _queueServiceClient.GetQueueClient(queueName);
               // await queueClient.CreateIfNotExistsAsync();
                await queueClient.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UploadFileAsync: {ex.Message}");
            }
        }

    }

}
