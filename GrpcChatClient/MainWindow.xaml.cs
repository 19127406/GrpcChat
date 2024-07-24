using Grpc.ChatServer;
using GrpcChatClient.Store;
using System.Windows;

namespace GrpcChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AccountStore _accountStore;

        public MainWindow(AccountStore accountStore)
        {
            InitializeComponent();
            _accountStore = accountStore;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show($"Current user: {_accountStore.CurrentUser.Username}");
            _accountStore.CurrentUser.ChatClient.Logout(new LogoutRequest
            {
                Name = _accountStore.CurrentUser.Username
            });
        }
    }
}
