//Role

function initRoleDropDown() {
    var selectList = document.getElementById('role');
    var option = document.createElement("option");
    option.text = "Selectionnez un rôle";
    selectList.insertBefore(option, selectList.firstChild);
    roleOptionEnabled(0, false);
    resetRoleIndex();
}

function resetRoleIndex() {
    document.getElementById('role').selectedIndex = "0";
}

function roleOptionEnabled(index, state) {
    document.getElementById("role").options[index].disabled = !state;
}

function rcpFormVisible(state) {
    if (state)
        document.getElementById("RCPForm").style.display = "block";
    else
        document.getElementById("RCPForm").style.display = "none";
}

function checkRCP(option, state) {
    if (option.value === "RCP")
        rcpFormVisible(state);
}

function addRole() {
    var selectList = document.getElementById('role');
    var option = selectList.options[selectList.selectedIndex];
    document.getElementById("roleBox").innerHTML += buildRoleChildHtml(option.text, option.index);
    roleOptionEnabled(selectList.selectedIndex, false);
    checkRCP(option, true);
    resetRoleIndex();
}

function buildRoleChildHtml(nom, index) {
    id = "role" + index;
    return html =
        "<div id='" + id + "'class='child-field clearfix'>" +
        "<p>" + nom + "</p>" +
        "<span  onclick='removeRole(" + index + ")' class='glyphicon glyphicon-remove'></span>" +
        "<input type='hidden' name='role' value='" + nom + "' />" +
        "</div>";
}

function removeRole(index) {
    var roleNode = document.getElementById("role" + index);
    var option = document.getElementById('role').options[index];
    checkRCP(option, false);
    roleOptionEnabled(index, true);
    roleNode.remove();
}

function loadRole(role) {
    var option = findRoleOptionByValue(role);
    roleOptionEnabled(option.index, false);
    document.getElementById('role').selectedIndex = option.index;
    addRole();
}

function findRoleOptionByValue(value) {
    var options = document.querySelectorAll("#role option");
    for (var i = 0; i < options.length; i++) {
        if (options[i].value === value)
            return options[i];
    }
}

//Code programme

function initCodeProgrammeDropDown() {
    var selectList = document.getElementById('codeProgramme');
    var option = document.createElement("option");
    option.text = "Selectionnez un programme";
    selectList.insertBefore(option, selectList.firstChild);
    codeProgrammeOptionEnabled(0, false);
    resetCodeProgrammeIndex();
}

function resetCodeProgrammeIndex() {
    document.getElementById('codeProgramme').selectedIndex = "0";
}

function codeProgrammeOptionEnabled(index, state) {
    document.getElementById("codeProgramme").options[index].disabled = !state;
}

function addCodeProgramme() {
    var selectList = document.getElementById('codeProgramme');
    var option = selectList.options[selectList.selectedIndex];
    document.getElementById("codeProgrammeBox").innerHTML += buildCodeProgrammeChildHtml(option.text, option.value, option.index);
    codeProgrammeOptionEnabled(selectList.selectedIndex, false);
    checkRCP(option, true);
    resetCodeProgrammeIndex();
}

function buildCodeProgrammeChildHtml(nom, value, index) {
    id = "codeProgramme" + index;
    return html =
        "<div id='" + id + "'class='child-field clearfix'>" +
        "<p>" + nom + "</p>" +
        "<span  onclick='removeCodeProgramme(" + index + ")' class='glyphicon glyphicon-remove'></span>" +
        "<input type='hidden' name='codeProgramme' value='" + value + "' />" +
        "</div>";
}

function removeCodeProgramme(index) {
    var codeProgrammeNode = document.getElementById("codeProgramme" + index);
    var option = document.getElementById('codeProgramme').options[index];
    checkRCP(option, false);
    codeProgrammeOptionEnabled(index, true);
    codeProgrammeNode.remove();
}

function loadCodeProgramme(codeProgramme) {
    var option = findCodeProgrammeOptionByValue(codeProgramme);
    codeProgrammeOptionEnabled(option.index, false);
    document.getElementById('codeProgramme').selectedIndex = option.index;
    addCodeProgramme();
}

function loadCodeProgramme(codeProgramme) {
    var option = findCodeProgrammeOptionByValue(codeProgramme);
    codeProgrammeOptionEnabled(option.index, false);
    document.getElementById('codeProgramme').selectedIndex = option.index;
    addCodeProgramme();
}

function findCodeProgrammeOptionByValue(value) {
    var options = document.querySelectorAll("#codeProgramme option");
    for (var i = 0; i < options.length; i++) {
        if (options[i].value === value)
            return options[i];
    }
}