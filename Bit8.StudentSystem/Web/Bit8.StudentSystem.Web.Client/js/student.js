
    let edittingStudentId = getUrlVars()["id"];
    let originalObject = {};
    $.ajax({
        url: BaseServerUrl + "/student/" + edittingStudentId,
        success: (data) => {
            originalObject = data;
            let header = $("#studentHeader");
            let headerHtml = header.html();
            header.html(headerHtml + data.id);

            $("#idInput").val(data.id);
            $("#nameInput").val(data.name);
            $("#surnameInput").val(data.surname);
            $("#dateOfBirthInput").val(renderDate(data.dob));

            $.ajax({
                type: "GET",
                url: BaseServerUrl + '/semester',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (response) => {
                    let semesterInput = $("#semesterInput");
                    let filteredSemesters = response.data.filter((responseSemester) => {
                        return !data.semesters.some((studentSemesters) => {
                            return responseSemester.id == studentSemesters.id;
                        });
                    });

                    filteredSemesters.forEach(semester => {
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

            let semesterList = $("#studentSemesters");
            data.semesters.forEach(semester => {
                let semesterDeleteId = "deleteDiscipline" + semester.id;
                let item = "<li>Id: " + semester.id + ", Name: " + semester.name + ", " + renderDate(semester.startDate) + "/" + renderDate(semester.endDate)
                    + " - <a class='deleteButton' id='semesterDelete" + semesterDeleteId + "'href='javascript:void(0);' data-id='" + semester.id
                    + "'> Delete</a><hr><div class='margin-top margin-bottom'>Disciplines</div><ul id='semesterDisciplines'>";

                let disableDelete = false;
                semester.disciplines.forEach(discipline => {
                    if (+discipline.score > 0) {
                        disableDelete = true;
                    }

                    item += "<li>Id: " + discipline.id + ", Name: " + discipline.disciplineName + ", Professor: " + discipline.professorName
                        + " " + getScoreInput(discipline.id, discipline.score) + " </li>";
                });

                item += "</ul></li><br>";

                semesterList.html(semesterList.html() + item);
                if (disableDelete) {
                    $("#semesterDelete" + semesterDeleteId).addClass("disabled");
                }
            });

            $("a.deleteButton").click(function () {
                var semesterDeleteId = $(this).data("id");
                $.ajax({
                    type: "DELETE",
                    url: BaseServerUrl + '/student/' + edittingStudentId + '/semester/' + semesterDeleteId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (response) => {
                        let link = BaseUrl + '/student.html?id=' + edittingStudentId;
                        window.location.href = link;
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
        }
    });

    semesterForm.addEventListener("submit", (event) => {
        event.preventDefault();
        let semesterForAddingId = +$("#semesterInput").val();
        $.ajax({
            type: "POST",
            url: BaseServerUrl + "/student/" + edittingStudentId + "/semester",
            data: JSON.stringify({ id: semesterForAddingId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (data) => {
                let link = BaseUrl + '/student.html?id=' + edittingStudentId;
                window.location.href = link;
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

    $(document).on('submit', '.scoreForm', function (e) {
        e.preventDefault();
        let form = $(e.target);
        let formId = form.attr("id");
        let disciplineId = formId.slice(formId.indexOf("-") + 1);
        let input = $(form.children("input")[0]);
        let button = $(form.children("input")[1]);
        let score = +input.val();
        let isCreating = button.val() == "Save";
        let requestType = isCreating ? "POST" : "PUT";
        $.ajax({
            type: requestType,
            url: BaseServerUrl + "/student/" + edittingStudentId + "/disciplineScore",
            data: JSON.stringify({ disciplineId: +disciplineId, score: +score }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (data) => {
                let link = BaseUrl + '/student.html?id=' + edittingStudentId;
                window.location.href = link;
            },
            error: (errMsg) => {
                let message = "";
                if (errMsg.responseJSON != undefined) {
                    message = errMsg.responseJSON.message;
                }
                alert("Status Code " + errMsg.status + " " + message);
                let link = BaseUrl + '/student.html?id=' + edittingStudentId;
                window.location.href = link;
            }
        });
    });

    function getScoreInput(disciplineId, score) {
        let scoreInput = "<form id='scoreForm-" + disciplineId + "' class='scoreForm inline'>";
        scoreInput += "<label for='disciplineScore" + disciplineId + "'>Score: </label>"
        scoreInput += "<input type='number' id='disciplineScore" + disciplineId + "' value='" + score + "' required/>";
        let actionButton = "";
        if (score == null) {
            actionButton += '<input id="addDisciplineScore' + disciplineId + '" type="submit" value="Save">';
        } else {
            actionButton += '<input id="addDisciplineScore' + disciplineId + '" type="submit" value="Edit">';
        }
    
        scoreInput += actionButton;
        scoreInput += "</form>";
        scoreInput += "<button onclick='deleteScore("+disciplineId+")'>Delete</button>";
        return scoreInput;
    }


function deleteScore(disciplineId){
    $.ajax({
        type: "DELETE",
        url: BaseServerUrl + "/student/" + edittingStudentId + "/disciplineScore",
        data: JSON.stringify({ disciplineId: +disciplineId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: (data) => {
            let link = BaseUrl + '/student.html?id=' + edittingStudentId;
            window.location.href = link;
        },
        error: (errMsg) => {
            let message = "";
            if (errMsg.responseJSON != undefined) {
                message = errMsg.responseJSON.message;
            }
            alert("Status Code " + errMsg.status + " " + message);
            let link = BaseUrl + '/student.html?id=' + edittingStudentId;
            window.location.href = link;
        }
    });
}



