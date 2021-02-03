$.ajax({
    type: "GET",
    url: BaseServerUrl + '/semester',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: (response) => {
        let semesterInput = $("#semesterIdInput");
        response.data.forEach(semester => {
            let option = '<option value="' + semester.id + '">' + semester.name + '</option>';
            semesterInput.html(semesterInput.html() + option);
        });
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
                let message = "";
                if (errMsg.responseJSON != undefined) {
                    message = errMsg.responseJSON.message;
                }
                alert("Status Code " + errMsg.status + " " + message);
            }
        });
    });
});