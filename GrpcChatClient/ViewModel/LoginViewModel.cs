using Grpc.Net.Client;
using GrpcChatClient.Command;
using GrpcChatClient.Navigation;
using GrpcChatClient.Store;
using System.Windows.Input;

namespace GrpcChatClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            { 
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AccountStore accountStore, NavigationMediator navigationMediator, GrpcChannel channel)
        {
            // ..:: Instantiate private fields ::..
            _username = string.Empty;

            NavigationService<ChatViewModel> navigationService = new NavigationService<ChatViewModel>(
                navigationMediator,
                () => new ChatViewModel(accountStore, navigationMediator)
            );

            LoginCommand = new LoginCommand(this, accountStore, navigationService, channel);
        }
    }
}
