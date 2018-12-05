$(document).ready(function () {
    var hour = new Date().getHours();
    console.log(hour);
    if (hour < 6 || hour >= 18) {
        $('#portail').css('background-image', 'url(../Content/Images/cegep2.jpg)');
        $('#main').css('background-image', 'url(../Content/Images/cegep2-frost.jpg)');
    } else {
        $('#portail').css('background-image', 'url(../Content/Images/cegep.jpg)');
        $('#main').css('background-image', 'url(../Content/Images/cegep-frost.jpg)');
    }
});
