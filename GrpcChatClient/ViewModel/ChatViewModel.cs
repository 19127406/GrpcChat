using Grpc.ChatServer;
using Grpc.Core;
using GrpcChatClient.Command;
using GrpcChatClient.Model;
using GrpcChatClient.Navigation;
using GrpcChatClient.Store;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GrpcChatClient.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;

        // ..:: Get properties from current logged in user ::..
        public string Username => _accountStore.CurrentUser.Username;
        private Chat.ChatClient _chatClient => _accountStore.CurrentUser.ChatClient;

        // ..:: Observable collections for different rooms and messages for each room ::..
        public ObservableCollection<MessageModel> Messages { get; private set; }
        public ObservableCollection<RoomModel> Rooms { get; private set; }

        // ..:: Keep track of which room is currently selected by user ::..
        private RoomModel _selectedRoom;
        public RoomModel SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }

        // ..:: Keep track of message currently being typed in by user ::..
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        // ..:: Keep track of add room searchbox being typed in by user ::..
        private string _addRoom;
        public string AddRoom
        {
            get { return _addRoom; }
            set
            {
                _addRoom = value;
                OnPropertyChanged(nameof(AddRoom));
            }
        }

        // ..:: Commands ::..
        public ICommand JoinRoomCommand { get; }
        public ICommand SendCommand { get; }

        public ChatViewModel(AccountStore accountStore, NavigationMediator mediator)
        {
            _accountStore = accountStore;

            _message = string.Empty;
            _addRoom = string.Empty;

            Messages = [];
            Rooms = [];

            JoinRoomCommand = new JoinRoomCommand(this, accountStore);
            SendCommand = new SendCommand(this, accountStore);
        }

        public void OnUserConnect(bool isPrivate)
        {
            if (Rooms.Count > 0)
            {
                ConnectUser(isPrivate);
            }
        }

        private async void ConnectUser(bool isPrivate)
        {
            // ..:: Connect user to a previously joined room ::..
            AsyncServerStreamingCall<ChatMessage> streamingCall = _chatClient.EnterChat(new EnterRequest
            {
                Name = Username,
                Room = Rooms.Last().Name,
                IsPrivate = isPrivate
            });

            // ..:: Waiting for new chat arrived ::..
            try
            {
                await foreach (var call in streamingCall.ResponseStream.ReadAllAsync())
                {
                    // ..:: Update chat list when new chat arrived ::..

                    // ..:: Try getting room name, if it not an empty string -> this is private room (direct chat) and room name need to be taken from call.Receiver ::..
                    int index;
                    string receiverName = TryGetReceiverName(call.Receiver);

                    if (string.IsNullOrEmpty(receiverName))
                    {
                        index = FindRoomIndex(call.Receiver);
                    }
                    else
                    {
                        index = FindRoomIndex(receiverName);
                    }

                    if (index < 0)
                    {
                        MessageBox.Show("Receiver not exist", "Update Message Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (call.IsEnter)
                        {
                            Rooms[index].Messages.Add(new MessageModel
                            {
                                Sender = call.Sender,
                                Message = call.Message
                            });
                        }
                        else
                        {
                            Rooms[index].Messages.Add(new MessageModel
                            {
                                Sender = call.Sender,
                                DateTime = call.Datetime,
                                Message = call.Message
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                streamingCall.Dispose();
            }
        }

        private int FindRoomIndex(string roomName)
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].Name == roomName)
                    return i;
            }
            return -1;
        }

        private string TryGetReceiverName(string name)
        {
            string[] sp = name.Split(',');
            if (sp.Length == 3 && sp[0] == "p")
            {
                if (sp[1] == Username)
                    return sp[2];
                else
                    return sp[1];
            }
            return string.Empty;
        }
    }
}
