using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebChat.Damain;
using WebChat.Damain.Interfaces;
using WeChat.SocketsManager;

namespace WeChat.Handlers
{
    public class WebSocketsMessageHandler : SocketHandlers
    {
        public WebSocketsMessageHandler(IConnectionManager connection) : base(connection)
        {
        }

        public override async Task OnConnected(User user)
        {
            await base.OnConnected(user);
            var message = $" entrou na sala";
            var obj = new { user, message };
            await SendMessageToAll(JsonSerializer.Serialize(obj));
        }
        public override async Task Receive(User user, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = $" disse: {Encoding.UTF8.GetString(buffer,0,result.Count)}";
            var obj = new { user,message};
            await SendMessageToAll(JsonSerializer.Serialize(obj));
        }
    }
}
