$.ajax({
    type: "GET",
    url: BaseServerUrl + '/semester',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: (response) => {
        let semesterInput = $("#semesterInput");
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
    let student = { semesters: [] };
    studentForm.addEventListener("submit", (event) => {
        event.preventDefault();

        student.name = $("#nameInput").val();
        student.surname = $("#surnameInput").val();
        student.dob = $("#dateOfBirthInput").val();

        $.ajax({
            type: "POST",
            url: BaseServerUrl + "/student",
            data: JSON.stringify(student),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (data) => {
                window.location.href = BaseUrl + "/students.html";
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

    semesterForm.addEventListener("submit", (event) => {
        event.preventDefault();

        let semesterInput = $("#semesterInput");
        let semesterId = semesterInput.val();
        student.semesters.push(semesterId);

        let semesterOption = $("#semesterInput option[value='" + semesterId + "']");
        let semesterName = semesterOption.html();
        semesterOption.remove();

        let semesterList = $("#createdSemestersForStudent");
        semesterList.html(semesterList.html() + "<li id='semesterListItem" + semesterId + "'><span class='semesterNameWrapper'>" + semesterName + "</span> - <a class='deleteButton' id='deleteSemester" + semesterId + "'href='javascript:void(0);' data-id='" + semesterId + "'> Delete</a></li>");

        $("a.deleteButton").click(function () {
            var semesterDeleteId = $(this).data("id");
            semesterInput.html(semesterInput.html() + '<option value="' + semesterDeleteId + '">' + $(this).siblings(".semesterNameWrapper").html() + '</option>');
            $("#semesterListItem" + semesterDeleteId).remove();
            student.semesters = student.semesters.filter((id) => {
                return id != semesterDeleteId;
            });
        });
    });
});