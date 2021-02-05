const BaseServerUrl = "https://localhost:44303/api";
const BaseUrl = window.location.href.slice(0, window.location.href.lastIndexOf('/'));
function getUrlVars() {
    let vars = [], hash;
    let hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }

    return vars;
}

$("#disciplinesButton").on("click", () => {
    window.location.href = BaseUrl + "/disciplines.html";
});

$("#semestersButton").on("click", () => {
    window.location.href = BaseUrl + "/semesters.html";
});

$("#studentsButton").on("click", () => {
    window.location.href = BaseUrl + "/students.html";
});

$("#aggregatedDataButton").on("click", () => {
    window.location.href = BaseUrl + "/aggregatedData.html";
});

function renderDate(data) {
    let startDate = new Date(data);
    let month = (startDate.getMonth() + 1) < 10 ? "0" + (startDate.getMonth() + 1) : (startDate.getMonth() + 1);
    let day = startDate.getDate() < 10 ? "0" + startDate.getDate() : startDate.getDate();
    return startDate.getFullYear() + "-" + month + "-" + day;
}
