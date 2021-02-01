$(document).ready(function () {
    let edittingDisciplineId = getUrlVars()["id"];

    $.get( "ajax/test.html", function( data ) {
        $( ".result" ).html( data );
        alert( "Load was performed." );
      });


    $("#idInput").val(edittingDisciplineId);
    let header = $("#disciplineHeader");
    let headerHtml = header.html();
    header.html(headerHtml + edittingDisciplineId);
});

