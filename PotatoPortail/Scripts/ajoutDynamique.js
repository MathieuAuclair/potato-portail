var idSP = [0, 0, 0, 0];
var idPP = 4;

function ajoutSPoint(nom, idValue) { //Permet l'ajout dynamique d'un sous point
    idSP[idValue]++;
    var souspoint = '<li class="form-group" id="' + idValue + 'li' + idSP[idValue] + '"><input name="listeSousPoint" id="' + idValue + 'sp' + idSP[idValue] + '" class="form-control medium inline" type="text"><input name="listeIdSousPointCache" id="' + idValue + 'sp' + idSP[idValue] + '" class="form-control" type="hidden" value="' + idValue + '"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idValue + '" onclick="enleverSPoint(' + idValue + ', ' + idSP[idValue] + ')"><span class="glyphicon glyphicon-trash"></span></a></li>';
    document.getElementById(nom).insertAdjacentHTML("beforebegin", souspoint);
}

function enleverSPoint(numLI, numSP) { //Permet d'enlever un sous-point selon le id
        var node = document.getElementById(numLI + "li" + numSP);
        node.parentNode.removeChild(node);
        idSP[numLI]--;
}

function ajoutPPrincipal(nom) { //Permet l'ajout dynamique d'un point principal
    var pointprincipal = '<div id="pp' + idPP + '"><input type="text" name="listPP" class="form-control inline"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idPP + '" onclick="enleverPPrincipal(this.id,' + idPP + ')"><span class="glyphicon glyphicon-trash"></span></a><ul id="listSP0"><a class="fancy-button" id="btnPlus' + idPP + '" onclick="ajoutSPoint(this.id,' + idPP + ')"><span class="glyphicon glyphicon-plus"></span></a></ul></div>';
    document.getElementById(nom).insertAdjacentHTML("beforebegin", pointprincipal);
    idSP.push(0);
    idPP++;
}

function enleverPPrincipal(nom, idValue) { //Permet d'enlever un pointprincipal selon le id
    var node = document.getElementById("pp" + idValue);
    node.parentNode.removeChild(node);
}

function init() { //--------- WARNING (Inutiliser??????) ------------------
    var dates = document.getElementById("dateReunion");
}