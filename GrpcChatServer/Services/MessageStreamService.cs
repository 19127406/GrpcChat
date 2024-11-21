using Grpc.ChatServer;
using Grpc.Core;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace GrpcChat.Services
{
    public class MessageStreamService
    {
        private readonly List<ActiveClient> _clients;
        private readonly ConcurrentDictionary<string, List<ActiveClient>> _rooms;
        private readonly ConcurrentDictionary<string, List<ActiveClient>> _privateRooms;

        public MessageStreamService()
        {
            _clients = [];
            _rooms = new();
            _privateRooms = new();
        }

        public bool Login(string username)
        {
            var c = _clients.Find(x => x.Name == username);
            if (c == null)
            {
                _clients.Add(new ActiveClient { Name = username });
                return true;
            }
            return false;
        }

        public void Logout(string username)
        {
            RemoveActiveClient(username);
            RemoveActiveClientFromRooms(username);
        }

        private bool RemoveActiveClientFromRooms(string username)
        {
            var c = _clients.Find(x => x.Name == username);
            if (c == null) return false;
            foreach (var room in _rooms.Values)
            {
                room.Remove(c);
            }
            return true;
        }

        private bool RemoveActiveClient(string username)
        {
            var c = _clients.Find(x => x.Name == username);
            return c != null && _clients.Remove(c);
        }

        public bool IsRoom(string room, out string info)
        {
            var c = _clients.Find(x => x.Name == room);
            if (c != null)
            {
                info = string.Empty;
                return false;
            }
            else
            {
                info = "User not exist";
                return true;
            }
        }

        public bool Enter(string name, IServerStreamWriter<ChatMessage> stream, out string info)
        {
            var c = _clients.Find(x => x.Name == name);
            if (c != null)
            {
                c.Stream = stream;

                foreach (var r in _rooms.Values)
                {
                    c = r.Find(x => x.Name == name);
                    if (c != null)
                        c.Stream = stream;
                }

                foreach(var pr in _privateRooms.Values)
                {
                    c = pr.Find(x => x.Name == name);
                    if (c != null)
                        c.Stream = stream;
                }

                info = string.Empty;
                return true;
            }

            info = "User not exist";
            return false;
        }

        public bool JoinRoom(string name, string room, bool isPrivate, out string info)
        {
            var c = _clients.Find(x => x.Name == name);
            if (c != null)
            {
                if (isPrivate)
                {
                    string[] splitName = room.Split(',');
                    if (splitName.Length == 3 &&  splitName[0] == "p")
                    {
                        // --! Should make this a little better --
                        if (_privateRooms.ContainsKey($"p,{splitName[1]},{splitName[2]}"))
                            _privateRooms[$"p,{splitName[1]},{splitName[2]}"].Add(c);
                        else if (_privateRooms.ContainsKey($"p,{splitName[2]},{splitName[1]}"))
                            _privateRooms[$"p,{splitName[2]},{splitName[1]}"].Add(c);
                        else
                            _privateRooms.TryAdd(room, [c]);
                    }
                    else
                    {
                        info = "Invalid private room name";
                        return false;
                    }
                }
                else
                {
                    if (_rooms.ContainsKey(room))
                        _rooms[room].Add(c);
                    else
                        _rooms.TryAdd(room, [c]);
                }

                info = string.Empty;
                return true;
            }

            info = "User not exist";
            return false;
        }

        public async Task SendMessage(ChatMessage message)
        {
            if (_rooms.ContainsKey(message.Receiver))
            {
                await Parallel.ForEachAsync(_rooms[message.Receiver], async (client, ctx) =>
                {
                    if (client.Stream != null && client.Name != message.Sender)
                    {
                        await client.Stream.WriteAsync(message, CancellationToken.None);
                    }
                });
            }
        }

        public async Task SendMessageDirect(ChatMessage message)
        {
            List<string> roomNames = TryGetPrivateRoom(message.Receiver);

            if (roomNames.Count > 0)
            {
                if (_privateRooms.TryGetValue($"p,{roomNames[0]},{roomNames[1]}", out var room))
                {
                    foreach (var client in room)
                    {
                        if (client.Stream != null && client.Name != message.Sender)
                        {
                            await client.Stream.WriteAsync(message, CancellationToken.None);
                        }
                    }
                }
                else
                {
                    foreach (var client in _privateRooms[$"p,{roomNames[1]},{roomNames[0]}"])
                    {
                        if (client.Stream != null && client.Name != message.Sender)
                        {
                            await client.Stream.WriteAsync(message, CancellationToken.None);
                        }
                    }
                }
            }
        }

        // ..:: Private methods ::..
        private List<string> TryGetPrivateRoom(string roomName)
        {
            string[] splitName = roomName.Split(',');
            if (splitName.Length == 3 && splitName[0] == "p")
            {
                List<string> result = [splitName[1], splitName[2]];
                return result;
            }
            else
                return [];
        }
    }

    public class ActiveClient
    {
        public IServerStreamWriter<ChatMessage>? Stream { get; set; }
        public required string Name { get; set; }
    }
}
