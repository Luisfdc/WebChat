

    var uri = "ws://localhost:5000/ws/";
    var socket;
    var idUserSend;
    function connction(name) {
        socket = new WebSocket(uri + name);
        socket.onopen = function (e) {
            console.log("Conexão estabelecida");
            mostarChat();
        };
        socket.onclose = function (e) {
            console.log("Conexão fechada");
        };

        socket.onmessage = function (e) {
            appendItem(e);
        };
    };

    var list = document.getElementById("messages");
    var button = document.getElementById("sendMessage");
    var buttonLogin = document.getElementById("enter");

    function appendItem(e) {
        let data = JSON.parse(e.data);

        let CreateDate = (new Date().toLocaleString());
        let element = document.createElement('div');
        element.innerHTML = '<div class="row"> <div class="col-lg-12"> <strong>' + data.user.Name + ':</strong> <span class="pull-right date-span">' + CreateDate + '</span> <p>' + data.message + '</p> </div> </div>';

        document.getElementById("messages").appendChild(element);

        var wtf = document.getElementById("messages");
        var height = wtf.scrollHeight;
        wtf.scrollTop = height;
    }

    function mostarChat() {
        let divLogin = document.getElementById("login");
        let divChat = document.getElementById("chat");

        divLogin.style.display = "none";
        divChat.style.display = "block";
    }

    function sendMessage(message) {
        console.log("Mensagem enviada........");
        socket.send(message);
    }

    button.addEventListener("click", function () {
        let message = document.getElementById("messageToSend");
        sendMessage(message.value);
    });


    buttonLogin.addEventListener("click", function () {
        let name = document.getElementById("name");
        connction(name.value);
    });
