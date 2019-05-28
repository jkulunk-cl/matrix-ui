// FOR PROTOTYPE ONLY

var views = document.getElementsByClassName('view-btn');
var subHeader = document.getElementById('m_pnlSubHeader');
var view;

for (i=0; i < views.length; i++){
    views[i].addEventListener('click', function(){
        view = this.getAttribute('value');
        subHeader.classList.remove('searchActive');
        subHeader.classList.remove('mapActive');
        subHeader.classList.remove('resultsActive');
        subHeader.classList.add(view);
    });
}