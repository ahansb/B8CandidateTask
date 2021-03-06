$(document).ready(function () {
    let myTable = $('#tableId').DataTable({
        ajax: {
            url: BaseServerUrl + '/student',
            dataSource: 'data'
        },
        columns: [
            { data: 'id', name: 'Id', title: 'Id' },
            { data: 'name', name: 'Name', title: 'Name' },
            { data: 'surname', name: 'Surname', title: 'Surname' },
            {
                data: 'dob',
                name: 'Date of birth',
                title: 'Date of birth',
                render: function (data, type, row) {
                    return renderDate(data);
                }
            },
            {
                data: 'semesters', 
                name: 'Semesters', 
                title: 'Semesters', 
                render: function (data, type, row) {
                    let result = "";
                    data.forEach(semester => {
                        result+= "<b>";
                        result+= semester.name + " " + renderDate( semester.startDate) + "/" + renderDate( semester.endDate);
                        result+="</b><hr>";
                        semester.disciplines.forEach(discipline => {
                            let score = discipline.score == null ? '-': discipline.score;
                            result += discipline.id + " " + discipline.disciplineName + " " + discipline.professorName + " Mark: " + score;
                            result += "<br>";
                        });
                    });
                    return result;
                }
            }
        ]
    });

    $('#tableId').on('dblclick', 'tbody tr', function () {
        let link = BaseUrl + '/student.html?id=' + myTable.row(this).data().id;
        window.location.href = link;
    });

    $('#studentAddButton').on('click', ()=>{
        window.location.href = BaseUrl + '/addStudent.html';
    });
});

