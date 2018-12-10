
function ajoutSousElement(parent, idElement) {

    var idSousElement = window.listIdElementSousElement[idElement];

    var sousElement = '<li class="form-group inline" id="ap' + idElement + 'li' + idSousElement + '"><input id="ConnaissanceSousElements_' + idElement + '__SousElementConnaissances_' + idSousElement + '__descSousElement" class="form-control medium inline" type="text" name="ConnaissanceSousElements[' + idElement + '].SousElementConnaissances[' + idSousElement + '].descSousElement"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idElement + '_' + idSousElement + '" onclick="enleverSousElement(' + idElement + ', ' + idSousElement + ')"><span class="glyphicon glyphicon-trash"></span></a></li>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", sousElement);

    window.listIdElementSousElement[idElement]++;
}

function enleverSousElement(indexActivite, indexSousActivite) {

    $(`#ap${indexActivite}li${indexSousActivite} input:text`).val("");

    $(`#ap${indexActivite}li${indexSousActivite}`).hide();
}
function ajoutElementConnaissance(parent) {

    var elementConnaissance = '<div id="ap' + window.idElement + '"><input id="ConnaissanceSousElements_' + window.idElement + '__ElementConnaissance_description" data-val="true" type="text" name="ConnaissanceSousElements[' + window.idElement + '].ElementConnaissance.description" class="form-control inline valid" data-val-required="Le champ Activité d\'apprentissage est requis."><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + window.idElement + '" onclick="enleverElement(this.id,' + window.idElement + ')"><span class="glyphicon glyphicon-trash"></span></a><ul id="listSA0"><a class="fancy-button" id="btnPlus' + window.idElement + '" onclick="ajoutSousElement(this.id,' + window.idElement + ')"><span class="glyphicon glyphicon-plus"></span>Ajouter une sous-activité</a></ul></div>';
    document.getElementById(parent).insertAdjacentHTML("beforebegin", elementConnaissance);
    window.idElement++;
    window.listIdElementSousElement.push("0");
}

function enleverElement(nom, idValue) {

    $(`#ap${idValue} input:text`).val("");

    $(`#ap${idValue}`).hide();
}

function init() { }