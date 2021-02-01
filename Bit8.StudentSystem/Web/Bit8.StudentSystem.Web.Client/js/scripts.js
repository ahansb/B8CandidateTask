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
