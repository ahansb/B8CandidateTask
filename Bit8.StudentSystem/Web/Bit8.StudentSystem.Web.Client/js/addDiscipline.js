$(document).ready(function () {
    disciplineForm.addEventListener("submit", (event) => {
        event.preventDefault();
        let discipline = {};
        discipline.disciplineName = $("#disciplineNameInput").val();
        discipline.professorName = $("#professorNameInput").val();
        discipline.semesterId = +$("#semesterIdInput").val();

        $.ajax({
            type: "POST",
            url: BaseServerUrl + "/discipline",
            data: JSON.stringify(discipline),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (data) => {
                window.location.href = BaseUrl + "/disciplines.html";
            },
            error: (errMsg) => {
                debugger;
                alert("Status Code " + errMsg.status + " " + errMsg.responseJSON.message);
            }
        });
    });
});