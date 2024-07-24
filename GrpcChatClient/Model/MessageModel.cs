using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcChatClient.Model
{
    public class MessageModel
    {
        public string? DateTime {  get; set; }
        public required string Sender {  get; set; }
        public required string Message { get; set; }
    }
}
