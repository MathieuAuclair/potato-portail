
function ajoutSRessource(parent, idRessourceDidactique) {

    var idSousRessource = window.listIdRessourceDidactique[idRessourceDidactique];

    var sousRessource = '<li class="form-group inline" id="ap' + idRessourceDidactique + 'li' + idSousRessource + '"><input id="DidactiqueSousDidactiques_' + idRessourceDidactique + '__SousRessourceDidactique_' + idSousRessource + '__nomSousRessource" class="form-control medium inline" type="text" name="DidactiqueSousDidactiques[' + idRessourceDidactique + '].sousRessourceDidactiques[' + idSousRessource + '].nomSousRessource"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idRessourceDidactique + '_' + idSousRessource + '" onclick="enleverSRessource(' + idRessourceDidactique + ', ' + idSousRessource + ')"><span class="glyphicon glyphicon-trash"></span></a></li>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", sousRessource);

    window.listIdRessourceDidactique[idRessourceDidactique]++;
}

function enleverSRessource(indexRessource, indexSousRessource) {

    $(`#ap${indexRessource}li${indexSousRessource} input:text`).val("");

    $(`#ap${indexRessource}li${indexSousRessource}`).hide();
}
function ajoutAPrincipal(parent) {

    var ressourcePrincipale = '<div id="ap' + window.idRessource + '"><input id="DidactiqueSousDidactique' + window.idRessource + '__Ressource_nomRessource" data-val="true" type="text" name="DidactiqueSousDidactiques[' + window.idRessource + '].ressourceDidactique.nomRessource" class="form-control inline valid" data-val-required="Le champ Ressource Didactique est requis."><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + window.idRessource + '" onclick="enleverAPrincipal(this.id,' + window.idRessource + ')"><span class="glyphicon glyphicon-trash"></span></a><ul id="listSA0"><a class="fancy-button" id="btnPlus' + window.idRessource + '" onclick="ajoutSRessource(this.id,' + window.idRessource + ')"><span class="glyphicon glyphicon-plus"></span>Ajouter une sous-ressource</a></ul></div>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", ressourcePrincipale);
    window.idRessource++;
    window.listIdRessourceDidactique.push("0");
}

function enleverAPrincipal(nom, idValue) {

    $(`#ap${idValue} input:text`).val("");

    $(`#ap${idValue}`).hide();
}

function init() { }