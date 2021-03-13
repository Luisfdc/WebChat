using System;
using System.Net.WebSockets;

namespace WebChat.Damain
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public WebSocket WebSocket { get; set; }
    }
}
