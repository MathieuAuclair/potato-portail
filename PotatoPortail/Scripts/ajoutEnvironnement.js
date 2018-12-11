
function ajoutSEnvironnement(parent, idEnvironnementPhysique) {

    var idSousEnvironnement = window.ListIdEnvironnementPhysique[idEnvironnementPhysique];

    var sousEnvironnement = '<li class="form-group inline" id="ap' + idEnvironnementPhysique + 'li' + idSousEnvironnement + '"><input id="EnvironnementSousEnvironnements_' + idEnvironnementPhysique + '__SousEnvironnementPhysiques_' + idSousEnvironnement + '__nomSousEnvPhys" class="form-control medium inline" type="text" name="EnvironnementSousEnvironnements[' + idEnvironnementPhysique + '].SousEnvironnementPhysiques[' + idSousEnvironnement + '].nomSousEnvPhys"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idEnvironnementPhysique + '_' + idSousEnvironnement + '" onclick="enleverSEnvironnement(' + idEnvironnementPhysique + ', ' + idSousEnvironnement + ')"><span class="glyphicon glyphicon-trash"></span></a></li>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", sousEnvironnement);

    window.ListIdEnvironnementPhysique[idEnvironnementPhysique]++;
}

function enleverSEnvironnement(indexEnvironnement, indexSousEnvironnement) {

    $(`#ap${indexEnvironnement}li${indexSousEnvironnement} input:text`).val("");

    $(`#ap${indexEnvironnement}li${indexSousEnvironnement}`).hide();
}
function ajoutAPrincipal(parent) {

    var environnementPrincipale = '<div id="ap' + window.idEnvironnement + '"><input id="EnvironnementSousEnvironnements' + window.idEnvironnement + '__EnvironnementPhysique_nomEnvPhys" data-val="true" type="text" name="EnvironnementSousEnvironnements[' + window.idEnvironnement + '].EnvironnementPhysique.nomEnvPhys" class="form-control inline valid" data-val-required="Le champ environnement physique est requis."><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + window.idEnvironnement + '" onclick="enleverAPrincipal(this.id,' + window.idEnvironnement + ')"><span class="glyphicon glyphicon-trash"></span></a><ul id="listSA0"><a class="fancy-button" id="btnPlus' + window.idEnvironnement + '" onclick="ajoutSEnvironnement(this.id,' + window.idEnvironnement + ')"><span class="glyphicon glyphicon-plus"></span>Ajouter un sous-environnement</a></ul></div>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", environnementPrincipale);
    window.idEnvironnement++;
    window.ListIdEnvironnementPhysique.push("0");
}

function enleverAPrincipal(nom, idValue) {

    $(`#ap${idValue} input:text`).val("");

    $(`#ap${idValue}`).hide();
}

function init() { }