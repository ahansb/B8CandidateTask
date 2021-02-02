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
            $("#startDateInput").val(renderDate( data.startDate));
            $("#endDateInput").val(renderDate(data.endDate));

            let disciplinesList = $("#disciplinesList");
            data.disciplines.forEach(discipline => {
                let disciplineDeleteId = "deleteDiscipline" + discipline.id;
                let item = "<li>Id: " + discipline.id + ", Name: " + discipline.disciplineName + ", Professor: " + discipline.professorName
                    + " - <a class='deleteButton' id='" + disciplineDeleteId + "'href='' data-id='" + discipline.id + "'> Delete</a></li>";

                disciplinesList.html(disciplinesList.html() + item);
            });

            $("a.deleteButton").click(function (){
                var disciplineForDeleteId =  $(this).data("id");
                debugger;
                $.ajax({
                    type: "DELETE",
                    url: BaseServerUrl + "/discipline/" + disciplineForDeleteId,
                    // data: JSON.stringify({ professorName }),
                    contentType: "application/json; charset=utf-8",
                   // dataType: "json",
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
        let professorName = $("#professorNameInput").val();
        if (professorName != originalObject.professorName) {
            $.ajax({
                type: "PUT",
                url: BaseServerUrl + "/discipline/" + originalObject.id,
                data: JSON.stringify({ professorName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (data) => {
                    window.location.href = BaseUrl + "/disciplines.html";
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
            window.location.href = BaseUrl + "/disciplines.html";
        }

    });
});

