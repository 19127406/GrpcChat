using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcChatClient.Model
{
    public class RoomModel
    {
        public string Name { get; set; }
        public string CommunicateName { get; set; }
        public string ImageSource { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public bool IsDirect = false;
    }
}
