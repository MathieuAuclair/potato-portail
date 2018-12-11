
function ajoutSActivite(parent, idActiviteParent) {

    var idSousActivite = window.listIdActiviteSousActivite[idActiviteParent];

    var sousActivite = '<li class="form-group inline" id="ap' + idActiviteParent + 'li' + idSousActivite + '"><input id="ActiviteSousActivites_' + idActiviteParent + '__SousActivites_' + idSousActivite + '__nomSousActivite" class="form-control medium inline" type="text" name="ActiviteSousActivites[' + idActiviteParent + '].SousActivites[' + idSousActivite + '].nomSousActivite"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idActiviteParent + '_' + idSousActivite + '" onclick="enleverSActivite(' + idActiviteParent + ', ' + idSousActivite + ')"><span class="glyphicon glyphicon-trash"></span></a></li>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", sousActivite);

    window.listIdActiviteSousActivite[idActiviteParent]++;
}

function enleverSActivite(indexActivite, indexSousActivite) {

    $(`#ap${indexActivite}li${indexSousActivite} input:text`).val("");

    $(`#ap${indexActivite}li${indexSousActivite}`).hide();
}
function ajoutAPrincipal(parent) {

    var activiteprincipale = '<div id="ap' + window.idActivite + '"><input id="ActiviteSousActivites_' + window.idActivite + '__Activite_descActivite" data-val="true" type="text" name="ActiviteSousActivites[' + window.idActivite + '].Activite.descActivite" class="form-control inline valid" data-val-required="Le champ Activité d\'apprentissage est requis."><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + window.idActivite + '" onclick="enleverAPrincipal(this.id,' + window.idActivite + ')"><span class="glyphicon glyphicon-trash"></span></a><ul id="listSA0"><a class="fancy-button" id="btnPlus' + window.idActivite + '" onclick="ajoutSActivite(this.id,' + window.idActivite + ')"><span class="glyphicon glyphicon-plus"></span>Ajouter une sous-activité</a></ul></div>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", activiteprincipale);
    window.idActivite++;
    window.listIdActiviteSousActivite.push("0");
}

function enleverAPrincipal(nom, idValue) {

    $(`#ap${idValue} input:text`).val("");

    $(`#ap${idValue}`).hide();
}

function init() { }