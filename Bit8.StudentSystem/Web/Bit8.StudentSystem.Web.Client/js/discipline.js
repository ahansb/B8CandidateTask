$(document).ready(function () {
    let edittingDisciplineId = getUrlVars()["id"];

    $.ajax({
        url: BaseServerUrl + "/discipline/" + edittingDisciplineId,
        success: (data) => {
            let header = $("#disciplineHeader");
            let headerHtml = header.html();
            header.html(headerHtml + data.id);

            $("#idInput").val(data.id);
            $("#disciplineNameInput").val(data.disciplineName);
            $("#professorNameInput").val(data.professorName);
        }
    });
});

