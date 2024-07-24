using Grpc.ChatServer;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcChatClient.Model;
using GrpcChatClient.Navigation;
using GrpcChatClient.Store;
using GrpcChatClient.ViewModel;
using System.Windows;

namespace GrpcChatClient.Command
{
    /// <summary>
    /// Command used to log user in ONLY
    /// See NavigateCommand to navigate between views
    /// </summary>
    public class LoginCommand : BaseCommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly AccountStore _accountStore;
        private readonly NavigationService<ChatViewModel> _navigationService;

        // ..:: GRPC connection fields ::..
        private GrpcChannel _channel;
        private Chat.ChatClient? _chatClient;

        public LoginCommand(LoginViewModel loginViewModel, AccountStore accountStore, NavigationService<ChatViewModel> navigationService, GrpcChannel channel)
        {
            _loginViewModel = loginViewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
            _channel = channel;
        }

        public override void Execute(object? parameter)
        {
            // ..:: Login logic ::..
            _chatClient = new Chat.ChatClient(_channel);

            if (!string.IsNullOrEmpty(_loginViewModel.Username))
            {
                MessageResponse reply = _chatClient.Login(new LoginRequest { Name = _loginViewModel.Username });

                // ..:: Start navigate to chat view when login successfully ::..
                if (reply.Success)
                {
                    UserModel inputUser = new()
                    {
                        Username = _loginViewModel.Username,
                        ChatClient = _chatClient
                    };

                    _accountStore.CurrentUser = inputUser;
                    _navigationService.Navigate();
                }
                else
                {
                    MessageBox.Show(reply.Info, "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Username must not be empty", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
