$(document).ready(function () {
    let edittingDisciplineId = getUrlVars()["id"];
    let originalObject = {};
    $.ajax({
        url: BaseServerUrl + "/discipline/" + edittingDisciplineId,
        success: (data) => {
            originalObject = data;
            let header = $("#disciplineHeader");
            let headerHtml = header.html();
            header.html(headerHtml + data.id);

            $("#idInput").val(data.id);
            $("#disciplineNameInput").val(data.disciplineName);
            $("#professorNameInput").val(data.professorName);
            $("#semesterIdInput").val(data.semesterId);
        }
    });

    editDisciplineForm.addEventListener("submit", (event) => {
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
                    alert("Status Code " + errMsg.status + " " + errMsg.responseJSON.message);
                }
            });
        } else {
            window.location.href = BaseUrl + "/disciplines.html";
        }

    });
});
