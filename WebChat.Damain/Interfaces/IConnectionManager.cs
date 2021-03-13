using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;
namespace WebChat.Damain.Interfaces
{
    public interface IConnectionManager
    {
        void AddSocket(User user);
        ConcurrentDictionary<string, User> GetAllConnections();
        string GetId(WebSocket socket);
        User GetSocketById(string id);
        Task RemoveSocketAsync(string id);
    }
}
