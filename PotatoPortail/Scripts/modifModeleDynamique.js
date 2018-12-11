var idPP = 4;

function ajoutPPrincipal(nom) { //Permet l'ajout dynamique d'un point principal dans la page modele
    idPP++;
    var pointprincipal = '<div id="pp' + idPP + '"><input type="text" name="listPP" class="form-control inline"><a class="fancy-button rouge vertical-align margin-left" id="btnMoins' + idPP + '" onclick="enleverPPrincipal(this.id,' + idPP + ')"><span class="glyphicon glyphicon-trash"></span></a></div>';
    document.getElementById(nom).insertAdjacentHTML("beforebegin", pointprincipal);
}

function enleverPPrincipal(nom, idValue) { //Permet d'enlever un pointprincipal selon le id dans la page modele
    var node = document.getElementById("PointPrincipal_" + idValue);
    node.parentNode.removeChild(node);
}