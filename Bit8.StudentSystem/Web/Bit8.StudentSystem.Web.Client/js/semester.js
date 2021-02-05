$(document).ready(function () {
    let edittingSemesterId = getUrlVars()["id"];
    let originalObject = {};
    $.ajax({
        url: BaseServerUrl + "/semester/" + edittingSemesterId,
        success: (data) => {
            originalObject = data;
            let header = $("#disciplineHeader");
            let headerHtml = header.html();
            header.html(headerHtml + data.id);

            $("#idInput").val(data.id);
            $("#nameInput").val(data.name);
            $("#startDateInput").val(renderDate(data.startDate));
            $("#endDateInput").val(renderDate(data.endDate));

            let disciplinesList = $("#disciplinesList");
            data.disciplines.forEach(discipline => {
                let disciplineDeleteId = "deleteDiscipline" + discipline.id;
                let item = "<li>Id: " + discipline.id + ", Name: " + discipline.disciplineName + ", Professor: " + discipline.professorName
                    + " - <a class='deleteButton' id='" + disciplineDeleteId + "'href='javascript:void(0);' data-id='" + discipline.id + "'> Delete</a></li>";

                disciplinesList.html(disciplinesList.html() + item);
            });

            $("a.deleteButton").click(function () {
                var disciplineForDeleteId = $(this).data("id");
                $.ajax({
                    type: "DELETE",
                    url: BaseServerUrl + "/discipline/" + disciplineForDeleteId,
                    contentType: "application/json; charset=utf-8",
                    success: (data) => {
                        window.location.href = BaseUrl + "/semester.html?id=" + edittingSemesterId;
                    },
                    error: (errMsg) => {
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
                discipline.semesterId = +edittingSemesterId;

                $.ajax({
                    type: "POST",
                    url: BaseServerUrl + "/discipline",
                    data: JSON.stringify(discipline),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (data) => {
                        window.location.href = BaseUrl + "/semester.html?id=" + edittingSemesterId;
                    },
                    error: (errMsg) => {
                        let message = "";
                        if (errMsg.responseJSON != undefined) {
                            message = errMsg.responseJSON.message;
                        }
                        alert("Status Code " + errMsg.status + " " + message);
                    }
                });
            });
        }
    });

    editSemesterForm.addEventListener("submit", (event) => {
        event.preventDefault();
        let name = $("#nameInput").val();
        let startDate = $("#startDateInput").val();
        let endDate = $("#endDateInput").val();

        if (semester.endDate > semester.startDate) {
            if (name != originalObject.name || startDate != originalObject.startDate || endDate != originalObject.endDate) {
                $.ajax({
                    type: "PUT",
                    url: BaseServerUrl + "/semester/" + originalObject.id,
                    data: JSON.stringify({ name, startDate, endDate }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (data) => {
                        window.location.href = BaseUrl + "/semesters.html";
                    },
                    error: (errMsg) => {
                        let message = "";
                        if (errMsg.responseJSON != undefined) {
                            message = errMsg.responseJSON.message;
                        }
                        alert("Status Code " + errMsg.status + " " + message);
                    }
                });
            } else {
                window.location.href = BaseUrl + "/semesters.html";
            }
        } else {
            alert("Start date should be lower than the end date!");
        }
    });
});

