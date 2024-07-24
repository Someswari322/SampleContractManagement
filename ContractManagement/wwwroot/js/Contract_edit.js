$(function () {
    $('#editContract').click(function (e) {
        e.preventDefault();
        var formData = new FormData();
        formData.append('ContractName', $('#ContractName').val());
        formData.append('ClientName', $('#ClientName').val());
        formData.append('PartitionKey', $('#PartitionKey').val());
        formData.append('RowKey', $('#RowKey').val());
        var fileInput = document.getElementById('File');
        if (fileInput.files.length > 0) {
            var file = fileInput.files[0];
            var fileName = file.name;
            formData.append('file', file)
            formData.append('fileName', fileName)
            //formData.append('partitionKey', 'Contracts');
            //formData.append('RowKey', new Date().getTime().toString());

            //var fileInfoDiv = document.getElementById('fileInfo');
            //fileInfoDiv.innerHTML = '<p>File Name: ' + fileName + '</p>';

            // Note: file path cannot be obtained for security reasons
        } else {
            alert('Please select a file.');
            return;
        }
        var contractName = $('#ContractName').val();
        var clientName = $('#ClientName').val();
        var Blobname = fileName;
        //var formData = new FormData($('#createForm')[0]);
        if (!contractName || !clientName) {
            alert('Please fill in all fields.');
            return;
        }

        var contract = {
            PartitionKey: $('#PartitionKey').val(),
            RowKey: $('#RowKey').val(),
            ContractName: contractName,
            ClientName: clientName, BlobName: Blobname
            // file: new FormData($('#createForm')[2])
        };


        $.ajax({
            url: '/Contract/Edit',
            type: 'POST',
            contentType: false,
            processData: false,
            //contentType: 'application/json',
            data: formData,
            success: function () {
                alert('Contract updated successfully.');
                window.location.href = '/Contract/Index';
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('Error updating contract: ' + textStatus + ' - ' + errorThrown);
                alert('Error updating contract.');
            }
        });
    });
});
