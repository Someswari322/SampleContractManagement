function loadContracts() {
    $.ajax({
        url: '/Contract/Index',
        method: 'GET',
        success: function (data) {
            var tableBody = $('#contractTableBody');
            tableBody.empty();
            data.forEach(function (contract) {
                var row = `<tr>
                    <td>${contract.ContractName}</td>
                    <td>${contract.ClientName}</td>
                    <td>${contract.BlobName}</td>
                    <td>${contract.Queuemessage}</td>
                    <td>
                        <button onclick="deleteContract('${contract.PartitionKey}', '${contract.RowKey}')">Delete</button>
                    </td>
                </tr>`;
                tableBody.append(row);
            });
        }
    });
}

function createContract() {
    var formData = new FormData($('#createContractForm')[0]);
    $.ajax({
        url: '/Contract/Create',
        method: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            window.location.href = '/Contract/Index';
        }
    });
}

function deleteContract(partitionKey, rowKey) {
    $.ajax({
        url: `/Contract/Delete?partitionKey=${partitionKey}&rowKey=${rowKey}`,
        method: 'POST',
        success: function () {
            loadContracts();
        }
    });
}

function editContract() {
    var formData = $('#editContractForm').serialize();
    $.ajax({
        url: '/Contract/Edit',
        method: 'POST',
        data: formData,
        success: function () {
            window.location.href = '/Contract/Index';
        }
    });
}
