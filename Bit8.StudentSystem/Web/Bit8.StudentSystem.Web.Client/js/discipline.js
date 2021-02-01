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
        }
    });

    $("#saveEdittedDiscipline").on("click", () => {
        let professorName = $("#professorNameInput").val();
        originalObject.professorName = professorName;
        $.ajax({
            type: "PUT",
            url: BaseServerUrl + "/discipline/" + originalObject.id,
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify({ professorName }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) { alert(data); },
            error: function (errMsg) {
                alert(errMsg);
            }
        });
    });
});

