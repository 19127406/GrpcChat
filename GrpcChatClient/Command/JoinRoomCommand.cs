using Grpc.ChatServer;
using GrpcChatClient.Model;
using GrpcChatClient.Store;
using GrpcChatClient.ViewModel;
using System.Windows;

namespace GrpcChatClient.Command
{
    public class JoinRoomCommand : BaseCommand
    {
        private readonly ChatViewModel _chatViewModel;
        private readonly AccountStore _accountStore;
        private readonly Chat.ChatClient _chatClient;

        private string _roomName => _chatViewModel.AddRoom;

        public JoinRoomCommand(ChatViewModel chatViewModel, AccountStore accountStore)
        {
            _chatViewModel = chatViewModel;
            _accountStore = accountStore;
            _chatClient = _accountStore.CurrentUser.ChatClient;
        }

        public override void Execute(object? parameter)
        {
            if (!string.IsNullOrEmpty(_roomName))
            {
                CheckResponse check = _chatClient.IsRoomOrDirect(new CheckRequest { Name = _roomName });

                if (check.IsRoom)
                {
                    MessageBoxResult result = MessageBox.Show($"User not found, do you want to create/join a room name {_roomName} ?", "Add Room Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        MessageResponse reply = _chatClient.JoinRoom(new EnterRequest
                        {
                            Name = _accountStore.CurrentUser.Username,
                            Room = _roomName,
                            IsPrivate = false
                        });

                        if (reply.Success)
                        {
                            _chatViewModel.Rooms.Add(new RoomModel
                            {
                                Name = _roomName,
                                CommunicateName = _roomName,
                                ImageSource = "/images/icons/groups.png",
                                Messages = []
                            });

                            if (!_accountStore.CurrentUser.IsEntered)
                            {
                                _chatViewModel.OnUserConnect(false);
                                _accountStore.CurrentUser.IsEntered = true;
                            }
                        }
                        else
                        {
                            // ..:: Show error when user sending request somehow does not exist ::..
                            MessageBox.Show(reply.Info, "Join Room Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        _chatViewModel.AddRoom = "";
                    }
                }
                else
                {
                    MessageResponse reply = _chatClient.JoinRoom(new EnterRequest
                    {
                        Name = _accountStore.CurrentUser.Username,
                        Room = $"p,{_accountStore.CurrentUser.Username},{_roomName}",
                        IsPrivate = true
                    });

                    if (reply.Success)
                    {
                        _chatViewModel.Rooms.Add(new RoomModel
                        {
                            Name = _roomName,
                            CommunicateName = $"p,{_accountStore.CurrentUser.Username},{_roomName}",
                            ImageSource = "/images/icons/groups.png",
                            Messages = []
                        });

                        if (!_accountStore.CurrentUser.IsEntered)
                        {
                            _chatViewModel.OnUserConnect(true);
                            _accountStore.CurrentUser.IsEntered = true;
                        }
                    }
                    else
                    {
                        // ..:: Show error when user sending request somehow does not exist ::..
                        MessageBox.Show(reply.Info, "Join Room Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    _chatViewModel.AddRoom = "";
                }
            }
            else
            {
                MessageBox.Show("Room name must not be empty", "Join Room Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
