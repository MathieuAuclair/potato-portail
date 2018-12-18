var listEnonceElement = [];

function initCompetence() {
    const selectCompetence = document.getElementById("dropEnonce");
    const option = document.createElement("option");
    option.text = "Sélectionnez une compétence";
    selectCompetence.insertBefore(option, selectCompetence.firstChild);
    desactiverOptionCompetence(0, false);
    reinitialiserCompetenceIndex();
}

function initElement(idEnonce) {
    const elementdiv = document.getElementById("elementSelectListEnonce");
    const selectElement = document.getElementById(`enonce${idEnonce}_elements`);
    const option = document.createElement("option");
    option.text = "Sélectionnez un élément";
    option.disabled = true;
    selectElement.insertBefore(option, selectElement.firstChild);
    selectElement.selectedIndex = "0";
    const divEnonce = document.createElement("div");
    divEnonce.id = `div_enonce_${idEnonce}`;
    elementdiv.insertBefore(divEnonce, selectElement);
}

function ajouterCompetence() {
    const selectCompetence = document.getElementById("dropEnonce");
    var option = selectCompetence.options[selectCompetence.selectedIndex];

    $("#divEnonce").append(`<div class="div-enonce" id='divEnonce${option.value}'></div>`);

    var $divEnonce = $(`#divEnonce${option.value}`);

    $divEnonce.append(buildTitreEnonce(option.value, option.text, selectCompetence.selectedIndex));

    $.ajax({
        url: "../PlanCadre/GetElement",
        type: "GET",
        data: {
            idCompetence: option.value
        },
        dataType: "html",
        success: function(data) {
            const $container = $(`<div id='listeElement${option.value}'>${data}</div>`);
            $divEnonce.append($container);
            desactiverOptionCompetence(selectCompetence.selectedIndex);
            reinitialiserCompetenceIndex();
            initElement(option.value);
        }
    });

    listEnonceElement.push(new EnonceElement(option.value));
}

function ajouterElement(idCompetence) {
    const selectCompetence = document.getElementById(`enonce${idCompetence}_elements`);
    const option = selectCompetence.options[selectCompetence.selectedIndex];
    const $divEnonce = $(`#divEnonce${idCompetence}`);

    $divEnonce.append(buildTitreElement(option.value, option.text, selectCompetence.selectedIndex, idCompetence));

    option.disabled = true;
    selectCompetence.selectedIndex = "0";

    for (let enonceElement in listEnonceElement) {
        if (listEnonceElement.hasOwnProperty(enonceElement)) {
            if (listEnonceElement[enonceElement].idEnonce === idCompetence.toString()) {
                listEnonceElement[enonceElement].idElements.push(option.value);
            }
        }
    }
}

function EnonceElement(idEnonce) {
    this.idEnonce = idEnonce,
        this.idElements = [];
}


function buildTitreEnonce(id, text, indexSelected) {

    const html =
        `<div id ='titreEnonce${id}'class='child-field clearfix'><p>${text}</p><span onclick='supprimerCompetence(${
            indexSelected},${id})' class='glyphicon glyphicon-remove'></span></div>`;
    return html;

}

function buildTitreElement(idElement, text, indexSelected, idEnonce) {

    const html =
        `<div id ='titreElement${idElement}'class='child-field clearfix indent'><p>${text
            }</p><span onclick='supprimerElement(${indexSelected},${idElement},${idEnonce
            })' class='glyphicon glyphicon-remove'></span></div>`;
    return html;
}

function supprimerCompetence(index, id) {
    $(`#divEnonce${id}`).remove();
    document.getElementById("dropEnonce").options[index].disabled = false;

    for (let elementEnonce in listEnonceElement) {
        if (!listEnonceElement.hasOwnProperty(elementEnonce)) continue;
        if (listEnonceElement[elementEnonce].idEnonce !== id.toString()) continue;
        listEnonceElement.splice(elementEnonce, 1);
        break;
    }
}

function supprimerElement(index, idElement, idEnonce) {
    $(`#titreElement${idElement}`).remove();
    document.getElementById(`enonce${idEnonce}_elements`).options[index].disabled = false;

    for (let enonceElement in listEnonceElement) {
        if (!listEnonceElement.hasOwnProperty(enonceElement)) continue;

        if (listEnonceElement[enonceElement].idEnonce !== idEnonce.toString()) continue;

        const listElement = listEnonceElement[enonceElement].idElements;

        for (let element in listElement) {
            if (!listElement.hasOwnProperty(element)) continue;
            if (listElement[element] !== idElement.toString()) continue;
            listEnonceElement[enonceElement].idElements.splice(element, 1);
            break; 
        }
        break;
    }

}

function desactiverOptionCompetence(index) {
    document.getElementById("dropEnonce").options[index].disabled = true;
}

function reinitialiserCompetenceIndex() {
    document.getElementById("dropEnonce").selectedIndex = "0";
}