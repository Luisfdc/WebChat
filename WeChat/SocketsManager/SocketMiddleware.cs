using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Damain;

namespace WeChat.SocketsManager
{
    internal class SocketMiddleware
    {
        private readonly RequestDelegate _next;

        public SocketMiddleware(RequestDelegate next, SocketHandlers handlers)
        {
            _next = next;
            Handler = handlers;
        }

        private SocketHandlers Handler { get; set; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            User user = new() {
                Name = context.Request.Path.ToString().Replace("%20"," ").Replace("/", " "),
                WebSocket = socket
            };

            await Handler.OnConnected(user);

            await Receive(socket, async (result, buffer) => 
            {
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    await Handler.Receive(user, result, buffer);
                }
                else if(result.MessageType ==  WebSocketMessageType.Close)
                {
                    await Handler.OnDisconnected(socket);
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult,byte[]> messageHandle)
        {
            var buffer = new byte[1024 * 4];
            while(socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandle(result, buffer);
            }
        }
    }
}