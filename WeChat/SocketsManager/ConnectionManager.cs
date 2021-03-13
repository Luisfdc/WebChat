using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Damain;
using WebChat.Damain.Interfaces;

namespace WeChat.SocketsManager.Extentions
{
    public class ConnectionManager : IConnectionManager
    {
        private ConcurrentDictionary<string, User> _connections = new ConcurrentDictionary<string, User>();

        public User GetSocketById(string id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value;
        }

        public ConcurrentDictionary<string, User> GetAllConnections()
        {
            return _connections;
        }

        public string GetId(WebSocket socket)
        {
            return _connections.FirstOrDefault(x => x.Value.WebSocket == socket).Key;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _connections.TryRemove(id, out var user);
            await user.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, " sokect connection closed ", CancellationToken.None);
        }

        public void AddSocket(User user)
        {
            user.Id = GetConnectionId();

            _connections.TryAdd(GetConnectionId(), user);
        }

        private string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
