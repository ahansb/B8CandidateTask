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

    $("#saveEdittedDiscipline").on("click", () => {
        let professorName = $("#professorNameInput").val();
        if (professorName != originalObject.professorName) {
            $.ajax({
                type: "PUT",
                url: BaseServerUrl + "/discipline/" + originalObject.id,
                // The key needs to match your method's input parameter (case-sensitive).
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

