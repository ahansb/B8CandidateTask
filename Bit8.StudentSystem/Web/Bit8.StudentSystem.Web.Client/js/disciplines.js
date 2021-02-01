$(document).ready(function () {
    let myTable = $('#table_id').DataTable({
        ajax: {
            url: BaseServerUrl + '/discipline',
            dataSource: 'data'
        },
        columns: [
            { data: 'id', name: 'Id', title: 'Id' },
            { data: 'disciplineName', name: 'Discipline name', title: 'Discipline name' },
            { data: 'professorName', name: 'Professor name', title: 'Professor name' }
        ]
    });

    $('#table_id').on('dblclick', 'tbody tr', function () {
        let link = BaseUrl + '/discipline.html?id=' + myTable.row(this).data().id;
        window.location.href = link;
    });
});