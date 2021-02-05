$('#topStudentsTable').DataTable({
    ajax: {
        url: BaseServerUrl + '/aggregatedData/GetTopStudents',
        dataSource: 'data'
    },
    columns: [
        { data: 'id', name: 'Id', title: 'Id' },
        { data: 'name', name: 'Name', title: 'Name' },
        { data: 'surname', name: 'Surname', title: 'Surname' },
        {
            data: 'dob',
            name: 'DOB',
            title: 'Date of Birth',
            render: function (data, type, row) {
                return renderDate(data);
            }
        },
        { data: 'avarageScore', name: 'AvarageScore', title: 'Score' }
    ],
    ordering: false
});

$('#noScoresStudentsTable').DataTable({
    ajax: {
        url: BaseServerUrl + '/aggregatedData/GetNoScoresStudents',
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
    ],
    ordering: false
});