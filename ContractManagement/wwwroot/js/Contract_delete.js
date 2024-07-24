$(document).ready(function () {
    $('#deleteButton').click(function (e) {
        e.preventDefault();
        var formData = new FormData();
        formData.append('PartitionKey', $('#PartitionKey').val());
        formData.append('RowKey', $('#RowKey').val());
        formData.append('BlobName', $('#BlobName').val());
        //var contracts = {
           
        //    PartitionKey: $('#PartitionKey').val(),
        //    RowKey: $('#RowKey').val(), BlobName: $('#BlobName')
        //};

        $.ajax({
            url: '/Contract/DeleteConfirmed',
            type: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function () {
                alert('Contract deleted successfully.');
                window.location.href = '/Contract/Index';
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('Error deleting contract: ' + textStatus + ' - ' + errorThrown);
                alert('Error deleting contract.');
            }
        });
    });
});