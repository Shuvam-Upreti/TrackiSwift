﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = new DataTable('#tblData', {
        ajax: {
            url: "/Admin/Order/GetAll"
        },
        columns: [
            { data: 'orderId', width: "5%" },
            { data: 'receiverName', width: "25%" },
            { data: 'receiverNumber', width: "25%" },
            { data: 'createdDateTime', width: "25%" },
            { data: 'deliveryAddress', width: "20%" },
            { data: 'weight', width: "10%" },
            { data: 'amount', width: "10%" },
            { data: 'deliveryStatus', width: "10%" },
            { data: 'paymentStatus', width: "10%" },

            {
                "data": "orderId",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/Order/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "40%"
            }
        ]
    });
}