using Grpc.ChatServer;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcChatClient.Model
{
    public class UserModel
    {
        public required string Username { get; set; }
        public required Chat.ChatClient ChatClient { get; set; }
        public bool IsEntered = false;
    }
}
