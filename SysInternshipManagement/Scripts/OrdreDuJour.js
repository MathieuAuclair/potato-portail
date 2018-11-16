var idSP = [0, 0, 0, 0];
var idPP = 4;
//Crée un tableau de id pour les 4 par default
//ensuite lorsque tu clic sur ajouter un point principal ajoute un nouvel élément au tableau de id
//du point principal, tu utilisera le tableau de id pour les sous-points


function addSPoint(nom, idValue) {
    /*<script id='template' type="text/template">
        @Html.EditorFor(model => Model.souspointsujet[ + idValue + ].sujetSousPoint, new {htmlAttributes = new { @class = "form - control" } })
    </script>*/
    idSP[idValue]++;
    //var souspoint = '<li>@Html.EditorFor(model => Model.souspointsujet[' + idValue + '].sujetSousPoint, new { htmlAttributes = new { @class = "form-control" } })</li>';
    var souspoint = '<li class="form-group" id="' + idValue + 'li' + idSP[idValue] + '"><input name="listSP" id="' + idValue + 'sp' + idSP[idValue] + '" class="form-control small" type="text"><input name="listHSP" id="' + idValue + 'sp' + idSP[idValue] + '" class="form-control" type="hidden" value="' + idValue + '"><a class="fancy-button rouge" id="btnPlus2" onclick="removeSPoint(this.id, ' + idValue + ')"><span class="glyphicon glyphicon-trash"></span></a></li>';
    document.getElementById(nom).insertAdjacentHTML("beforebegin", souspoint);
    /*
    var node = document.createElement("LI");                 // Create a <li> node
    var textnode = document.createTextNode(souspoint);         // Create a text node
    node.appendChild(textnode);                              // Append the text to <li>
    document.getElementById("listSP" + idValue).appendChild(node);     // Append <li> to <ul> with 
    */
    /*
    var newNode = document.createElement("li");
    var nodep = document.createElement("p");
    var textp = sp1 + ". ";
    nodep.nodeValue(textp);
    newNode.appendChild(nodep);
    var parentNode = document.getElementById("liste").parentNode;
    var child = document.getElementById(nom);
    parentNode.insertBefore(newNode, child);
    */
    /*
    var nodeli = document.createElement("li");
    var nodep = document.createElement("p");
    var nodeinput = document.createElement("input");
    nodeinput.id = "sp" + sp1;
    nodeinput.type = "text";
    var textp = sp1 + ". ";
    nodep.nodeValue(textp);
    alert("node.appendchild");
    nodeli.appendChild(nodep);
    var element = document.getElementById(nom);
    element.insertBefore(nodeli, document.getElementById(nom));
    sp1++;
    alert("Eventlistener " + nom + sp1);
    */
}

function removeSPoint(nom, idValue) {
    if (idSP[idValue] > 0) {
        var node = document.getElementById(idValue + "li" + idSP[idValue]);
        node.parentNode.removeChild(node);
        idSP[idValue]--;
    }
}

function ajoutPPrincipal(nom, idValue){
    var souspoint = '<li id="' + idValue + 'li' + idSP[idValue] + '"><input name="listPP" id="'  + idValue + ' class="form-control" type="text"></li>';
    document.getElementById(nom).insertAdjacentHTML("beforebegin", souspoint);
}

function init() {
    var dates = document.getElementById("dateReunion");
};

/*$(document).ready(function () {
    $("#ordreDuJour.dateOdJ").datepicker({
        minDate: -0
    });
});*/