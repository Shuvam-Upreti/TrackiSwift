var dataTable;

$(document).ready(function () {
    var url = window.location.search;
        loadDataTable();
    
});

function loadDataTable(status) {

    dataTable = new DataTable('#tblData', {
        ajax: {
            url: "/Admin/user/getall"
        },
        columns: [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phone", "width": "15%" },
            { "data": "role", "width": "15%" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/User/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/Order/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "10%"
            }
        ]
    });
}