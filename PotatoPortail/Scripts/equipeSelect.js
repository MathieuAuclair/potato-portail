
//******* GESTION ENTRAINEURS *******//
function initEntraineurDropDown() {
    var selectList = document.getElementById('entraineur');
    var option = document.createElement("option");
    option.text = "Associez un entraineur";
    selectList.insertBefore(option, selectList.firstChild);
    entraineurOptionEnabled(0, false);
    reinitialiserEntraineurIndex();
}

function reinitialiserEntraineurIndex() {
    document.getElementById('entraineur').selectedIndex = "0";
}

function entraineurOptionEnabled(index, statut) {
    document.getElementById('entraineur').options[index].disabled = !statut;
}

function ajouterEntraineur() {
    var selectList = document.getElementById('entraineur');
    var option = selectList.options[selectList.selectedIndex];
    document.getElementById("entraineurBox").innerHTML += buidEntraineurChildHtml(option.text, option.index);
    entraineurOptionEnabled(selectList.selectedIndex, false);
    reinitialiserEntraineurIndex();
}

function buidEntraineurChildHtml(nom, index) {
    id = "entraineur" + index;
    return html = 
        "<div id ='" + id + "'class='child-field clearfix'>" +
        "<p>" + nom + "</p>" + 
        "<span onclick='supprimerEntraineur(" + index + ")' class='glyphicon glyphicon-remove'></span>" +
        "<input type='hidden' name='entraineur' value='" + nom + "' />" +
        "</div>";
}

function supprimerEntraineur(index) {
    var entraineurNode = document.getElementById("entraineur" + index);
    var option = document.getElementById('entraineur').options[index];
    entraineurOptionEnabled(index, true);
    entraineurNode.remove();
}

function chargerEntraineur(entraineur) {
    var option = trouverEntraineurParValeur(entraineur);
    entraineurOptionEnabled(option.index, false);
    document.getElementById('entraineur').selectedIndex = option.index;
    ajouterEntraineur();
}

function trouverEntraineurParValeur(valeur) {
    var options = document.querySelectorAll("#entraineur option");
    for (var i = 0; i < options.length; i++) {
        if (options[i].textContent === valeur) {
            return options[i];
        }
    }
}

//******* GESTION JOUEURS *******//
function initJoueurDropDown() {
    var selectList = document.getElementById('joueurs');
    var option = document.createElement("option");
    option.text = "Associez un joueurs";
    selectList.insertBefore(option, selectList.firstChild);
    joueursOptionEnabled(0, false);
    reinitialiserJoueurIndex();
}

function reinitialiserJoueurIndex() {
    document.getElementById('joueurs').selectedIndex = "0";
}

function joueursOptionEnabled(index, statut) {
    document.getElementById('joueurs').options[index].disabled = !statut;
}

function ajouterJoueur() {
    var selectList = document.getElementById('joueurs');
    var option = selectList.options[selectList.selectedIndex];
    document.getElementById("joueursBox").innerHTML += buidJoueurChildHtml(option.text, option.index);
    joueursOptionEnabled(selectList.selectedIndex, false);
    reinitialiserJoueurIndex();
}

function buidJoueurChildHtml(nom, index) {
    id = "joueurs" + index;
    return html =
        "<div id ='" + id + "'class='child-field clearfix'>" +
        "<p>" + nom + "</p>" +
        "<span onclick='supprimerJoueur(" + index + ")' class='glyphicon glyphicon-remove'></span>" +
        "<input type='hidden' name='joueurs' value='" + nom + "' />" +
        "</div>";
}

function supprimerJoueur(index) {
    var joueursNode = document.getElementById("joueurs" + index);
    var option = document.getElementById('joueurs').options[index];
    joueursOptionEnabled(index, true);
    joueursNode.remove();
}

function chargerJoueur(joueurs) {
    var option = trouverJoueurParValeur(joueurs);
    joueursOptionEnabled(option.index, false);
    document.getElementById('joueurs').selectedIndex = option.index;
    ajouterJoueur();
}

function trouverJoueurParValeur(valeur) {
    var options = document.querySelectorAll("#joueurs option");
    for (var i = 0; i < options.length; i++) {
        if (options[i].textContent === valeur) {
            return options[i];
        }
    }
}
