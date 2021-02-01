$(document).ready(function () {
    $('#table_id').DataTable({
        ajax: {
            url: 'https://localhost:44303/api/discipline',
            dataSource: 'data'
        },
        columns: [
            { data: 'id', name: 'Id', title: 'Id' },
            { data: 'disciplineName', name: 'Discipline name', title: 'Discipline name' },
            { data: 'professorName', name: 'Professor name', title: 'Professor name'}
        ]
    });
});