$(document).ready(function () {
    let myTable = $('#tableId').DataTable({
        ajax: {
            url: BaseServerUrl + '/discipline',
            dataSource: 'data'
        },
        columns: [
            { data: 'id', name: 'Id', title: 'Id' },
            { data: 'disciplineName', name: 'Discipline name', title: 'Discipline name' },
            { data: 'professorName', name: 'Professor name', title: 'Professor name' },
            {
                data: 'semester',
                name: 'Semester',
                title: 'Semester',
                render: function (semester, type, row) {
                    let result = "";
                    result += semester.id + " " + semester.name + " " + renderDate(semester.startDate) + "/" + renderDate(semester.endDate);
                    result += "<br>";

                    return result;
                }
            }
        ]
    });

    $('#tableId').on('dblclick', 'tbody tr', function () {
        let link = BaseUrl + '/discipline.html?id=' + myTable.row(this).data().id;
        window.location.href = link;
    });

    $('#disciplineAddButton').on('click', () => {
        window.location.href = BaseUrl + '/addDiscipline.html';
    });
});