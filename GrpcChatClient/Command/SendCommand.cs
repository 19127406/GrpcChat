using Grpc.ChatServer;
using GrpcChatClient.Model;
using GrpcChatClient.Store;
using GrpcChatClient.ViewModel;
using System.Windows;

namespace GrpcChatClient.Command
{
    public class SendCommand : BaseCommand
    {
        private readonly ChatViewModel _chatViewModel;
        private readonly AccountStore _accountStore;
        private readonly Chat.ChatClient _chatClient;

        private string _message => _chatViewModel.Message;
        private string _datetime = string.Empty;

        public SendCommand(ChatViewModel chatViewModel, AccountStore accountStore)
        {
            _chatViewModel = chatViewModel;
            _accountStore = accountStore;
            _chatClient = accountStore.CurrentUser.ChatClient;
        }

        public override void Execute(object? parameter)
        {
            if (!string.IsNullOrEmpty(_message) && _chatViewModel.SelectedRoom != null)
            {
                MessageResponse reply;
                _datetime = DateTime.UtcNow.ToString();

                // --! Loop receiving double income message (check room + received message) --
                CheckResponse check = _chatClient.IsRoomOrDirect(new CheckRequest { Name = _chatViewModel.SelectedRoom.Name });

                if (check.IsRoom)
                {
                    reply = _chatClient.SendMessage(new ChatMessage
                    {
                        Sender = _accountStore.CurrentUser.Username,
                        Receiver = _chatViewModel.SelectedRoom.CommunicateName,
                        Datetime = _datetime,
                        Message = _message,
                        IsEnter = false
                    });
                }
                else
                {
                    reply = _chatClient.SendMessageDirect(new ChatMessage
                    {
                        Sender = _accountStore.CurrentUser.Username,
                        Receiver = _chatViewModel.SelectedRoom.CommunicateName,
                        Datetime = _datetime,
                        Message = _message,
                        IsEnter = false
                    });
                }

                if (reply.Success)
                {
                    _chatViewModel.SelectedRoom.Messages.Add(new MessageModel
                    {
                        Sender = _accountStore.CurrentUser.Username,
                        DateTime = _datetime,
                        Message = _message,
                    });

                    _chatViewModel.Message = "";
                }
                else
                {
                    MessageBox.Show(reply.Info, "Send Message Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
