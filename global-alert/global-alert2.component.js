// For use with BootStrap Alert styles
// without jQuery

var alertMessage = (function(){

    var setTimer;

    var showAlert = function(options){

        // build message
        var message = document.createElement('span');
        message.innerText = options.message;

        // build close button
        var closeBtn = document.createElement('button');
        closeBtn.setAttribute('class','close');
        closeBtn.setAttribute('data-target', options.targetId);
        closeBtn.setAttribute('id', options.targetId + '-close');
        closeBtn.innerText = 'x';

        document.getElementById(options.targetId).appendChild(message);
        document.getElementById(options.targetId).appendChild(closeBtn);
        document.getElementById(options.targetId).classList.add(options.type);
        document.getElementById(options.targetId).classList.add('in');

        closeBtn.addEventListener('click', function(){
            handleClose(options);
        });

        setTimer = setTimeout(function(){
            handleClose(options);
        }, options.timeout);

    };

    var handleClose = function(options){
        document.getElementById(options.targetId).classList.remove('in');
        document.getElementById(options.targetId).innerHTML = '';
        clearTimeout(setTimer);
        return;
    };

    var init = function(options){
        options.type = options.type || 'alert-warning';
        options.timeout = options.timeout || 10000;

        if (options.targetId) {
            showAlert(options);
        }
        else {
            console.error('No target ID provided');
        }

    };

    return {
        init: init
    };

}());

function triggerAlert() {

    alertMessage.init({
        targetId: 'alert1',
        message: 'I have an important message for you. Closing in 10...9...8...',
        type: 'alert-info',
        timeout: 10000,
    });

}