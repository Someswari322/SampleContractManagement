// wwwroot/js/contract.js

$(function () {
    debugger;
    $('#File').on('change', function () {
        var fileName = $(this).val().split('\\').pop();
        console.log('File Name:', fileName);
    });
    $('#getFileInfo').click(function (e) {
        var fileInput = document.getElementById('File');
        if (fileInput.files.length > 0) {
            var file = fileInput.files[0];
            var fileName = file.name;

            var fileInfoDiv = document.getElementById('fileInfo');
            fileInfoDiv.innerHTML = '<p>File Name: ' + fileName + '</p>';

            // Note: file path cannot be obtained for security reasons
        } else {
            alert('Please select a file.');
            return ;
        }
    });

    $('#createButton').click(function (e) {
        e.preventDefault();
        var contractName = $('#ContractName').val();
        var clientName = $('#ClientName').val();

        //var formData = new FormData($('#createForm')[0]);
        if (!contractName || !clientName) {
            alert('Please fill in all fields.');
            return;
        }
        var formData = new FormData();
        formData.append('ContractName', $('#ContractName').val());
        formData.append('ClientName', $('#ClientName').val());
        var fileInput = document.getElementById('File');
        if (fileInput.files.length > 0) {
            var file = fileInput.files[0];
            var fileName = file.name;
            formData.append('file', file)
            formData.append('fileName', fileName)
            formData.append('partitionKey', 'Contracts');
            formData.append('RowKey', new Date().getTime().toString());

            //var fileInfoDiv = document.getElementById('fileInfo');
            //fileInfoDiv.innerHTML = '<p>File Name: ' + fileName + '</p>';

            // Note: file path cannot be obtained for security reasons
        } else {
            alert('Please select a file.');
            return;
        }
        
        var Blobname = fileName;
        var contract = {
            PartitionKey: "Contracts",
            RowKey: new Date().getTime().toString(),
            ContractName: contractName,
            ClientName: clientName, BlobName: Blobname
           // file: new FormData($('#createForm')[2])
        };

        $.ajax({
            url: '/Contract/PostCreate',
            type: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function () {
                alert('Contract created successfully.');
                window.location.href = '/Contract/Index';
            },
            error: function () {
                alert('Error creating contract.');
            }
        });
    });
});
