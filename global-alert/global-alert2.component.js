var confirmMessage = (function(){

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

        document.getElementById(options.targetId).classList.add('in');

        closeBtn.addEventListener('click', function(){
            handleClose(options);
        });

        setTimeout(function(){
            handleClose(options);
        }, options.timeout);
    };

    var handleClose = function(options){
        document.getElementById(options.targetId).classList.remove('in');
        document.getElementById(options.targetId).innerHTML = '';
        return;
    };



    var init = function(options){
        options = options || {};
        options.targetId = options.targetId;
        options.message = options.message;
        options.type = options.type || 'default';
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

    confirmMessage.init({
        targetId: 'alert1',
        message: 'I have an important message for you',
        type: 'info',
        timeout: 3000,
    });

}