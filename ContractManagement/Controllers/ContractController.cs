using ContractManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using ContractManagement.Models;
using Azure.Data.Tables;
using System.Collections.Concurrent;

namespace ContractManagement.Controllers
{
    public class ContractController : Controller
    {
        private readonly TableStorageService _tableStorageService;
        private readonly BlobStorageService _blobStorageService;
        private readonly QueueStorageService _queueStorageService;
        public ContractController(TableStorageService tableStorageService, BlobStorageService blobStorageService,QueueStorageService queueStorageService)
        {
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
            _queueStorageService = queueStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _tableStorageService.GetContractsAsync();
            return View(contracts);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var contract = await _tableStorageService.GetContractAsync(partitionKey, rowKey);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [HttpPost]
        public async Task<IActionResult> PostCreate()
        {
            var contract = new Contract()
            {
                ClientName = Request.Form["ClientName"].ToString(),
                BlobName = Request.Form["FileName"].ToString(),
                ContractName = Request.Form["ContractName"].ToString(),
                PartitionKey = Request.Form["PartitionKey"].ToString(),
                RowKey = Request.Form["RowKey"].ToString()
                
            };
            if (contract.BlobName != null)
            {
                var file = Request.Form.Files[0];
                var filePath = Path.Combine(Path.GetTempPath(), contract.BlobName);
                using (var stream1 = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream1);
                }
                using var stream = file.OpenReadStream();
                await _blobStorageService.UploadFileAsync("contracts", contract.BlobName, stream);
                contract.BlobName = contract.BlobName;
            }
            //return;
            contract.PartitionKey = "Contract";
            contract.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddOrUpdateContractAsync(contract);
            await _queueStorageService.SendMessageAsync("contracts-queue", $"Contract {contract.RowKey} created.");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            //CloudTable table = GetCloudTable();
            //TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(partitionKey, rowKey);
            var contract = await _tableStorageService.GetContractAsync(partitionKey, rowKey);
            //Contract contract = result.Result as Contract;

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            //CloudTable table = GetCloudTable();
            //TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(partitionKey, rowKey);
            var contract = await _tableStorageService.GetContractAsync(partitionKey, rowKey);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }
        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            var contract = new Contract()
            {
                ClientName = Request.Form["ClientName"].ToString(),
                BlobName = Request.Form["FileName"].ToString(),
                ContractName = Request.Form["ContractName"].ToString(),
                PartitionKey = Request.Form["PartitionKey"].ToString(),
                RowKey = Request.Form["RowKey"].ToString()

            };
            if (contract.BlobName != null)
            {
                var file = Request.Form.Files[0];
                var filePath = Path.Combine(Path.GetTempPath(), contract.BlobName);
                using (var stream1 = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream1);
                }
                using var stream = file.OpenReadStream();
                await _blobStorageService.UploadFileAsync("contracts", contract.BlobName, stream);
                contract.BlobName = contract.BlobName;
            }
            //return;
            //var tableClient = _tableStorageService.GetContractAsync(partitionKey, rowKey);
            //await tableClient.UpdateEntityAsync(contract, contract.ETag, TableUpdateMode.Replace);

            await _tableStorageService.AddOrUpdateContractAsync(contract);
            await _queueStorageService.SendMessageAsync("contracts-queue", $"Contract {contract.RowKey} updated.");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var contracts = new Contract()
            {
                //ClientName = Request.Form["ClientName"].ToString(),
               
                //ContractName = Request.Form["ContractName"].ToString(),
                PartitionKey = Request.Form["PartitionKey"].ToString(),
                RowKey = Request.Form["RowKey"].ToString(),
                BlobName = Request.Form["BlobName"].ToString()

            };
            var contract = await _tableStorageService.GetContractAsync(contracts.PartitionKey, contracts.RowKey);
            await _queueStorageService.SendMessageAsync("contracts-queue", $"Contract {contract.RowKey} deleted.");


            if (contract != null)
            {
                await _tableStorageService.DeleteContractAsync(contracts.PartitionKey, contracts.RowKey);
                if (!string.IsNullOrEmpty(contract.BlobName))
                {
                    await _blobStorageService.DeleteFileAsync("contracts", contract.BlobName);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
