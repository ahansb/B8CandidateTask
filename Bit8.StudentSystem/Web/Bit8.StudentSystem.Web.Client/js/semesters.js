$(document).ready(function () {
    let myTable = $('#tableId').DataTable({
        ajax: {
            url: BaseServerUrl + '/semester',
            dataSource: 'data'
        },
        columns: [
            { data: 'id', name: 'Id', title: 'Id' },
            { data: 'name', name: 'Name', title: 'Name' },
            {
                data: 'startDate',
                name: 'Start Date',
                title: 'Start Date',
                render: function (data, type, row) {
                    return renderDate(data);
                }
            },
            {
                data: 'endDate', 
                name: 'End Date', 
                title: 'End Date', 
                render: function (data, type, row) {
                    return renderDate(data);
                }
            },
            {
                data: "disciplines",
                render: function (data, type, row) {
                    let result = "";
                    data.forEach(discipline => {
                        result += discipline.id + " " + discipline.disciplineName + " " + discipline.professorName;
                        result += "<br>";
                    });
                    return result;
                }
            }
        ]
    });

    // $('#tableId').on('dblclick', 'tbody tr', function () {
    //     let link = BaseUrl + '/discipline.html?id=' + myTable.row(this).data().id;
    //     window.location.href = link;
    // });

    $('#semesterAddButton').on('click', ()=>{
        window.location.href = BaseUrl + '/addSemester.html';
    });
});

