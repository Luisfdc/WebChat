using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Damain;
using WebChat.Damain.Interfaces;

namespace WeChat.SocketsManager
{
    public abstract class SocketHandlers
    {
        public IConnectionManager Connection { get; set; }

        public SocketHandlers(IConnectionManager connection)
        {
            Connection = connection;
        }

        public virtual async Task OnConnected(User user)
        {
            await Task.Run(() => { Connection.AddSocket(user); });
        }

        public virtual async Task OnDisconnected(WebSocket sokect)
        {
            await Connection.RemoveSocketAsync(Connection.GetId(sokect));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessage(string id, string message)
        {
            var user = Connection.GetSocketById(id);

            await SendMessage(user.WebSocket, message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var conn in Connection.GetAllConnections())
                await SendMessage(conn.Value.WebSocket, message);
        }

        public abstract Task Receive(User user, WebSocketReceiveResult result, byte[] buffer);
    }
}
