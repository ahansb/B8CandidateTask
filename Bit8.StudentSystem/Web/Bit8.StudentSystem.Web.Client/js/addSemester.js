$(document).ready(function () {
    let semester = { disciplines: [] };
    semesterForm.addEventListener("submit", (event) => {
        event.preventDefault();

        semester.name = $("#nameInput").val();
        semester.startDate = $("#startDateInput").val();
        semester.endDate = $("#endDateInput").val();

        $.ajax({
            type: "POST",
            url: BaseServerUrl + "/semester",
            data: JSON.stringify(semester),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (data) => {
                window.location.href = BaseUrl + "/semesters.html";
            },
            error: (errMsg) => {
                debugger;
                let message = "";
                if (errMsg.responseJSON != undefined) {
                    message = errMsg.responseJSON.message;
                }
                alert("Status Code " + errMsg.status + " " + message);
            }
        });
    });

    disciplineForm.addEventListener("submit", (event) => {
        event.preventDefault();

        let discipline = {};
        discipline.disciplineName = $("#disciplineNameInput").val();
        discipline.professorName = $("#professorNameInput").val();

        semester.disciplines.push(discipline);

        let disciplinesList = $("#createdDisciplinesForSemester");
        disciplinesList.html(disciplinesList.html() + "<li>" + discipline.disciplineName + " - " + discipline.professorName + "</li>");

        $("#disciplineNameInput").val(null);
        $("#professorNameInput").val(null);
    });
});