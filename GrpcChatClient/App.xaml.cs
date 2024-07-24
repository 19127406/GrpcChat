using Grpc.Net.Client;
using GrpcChatClient.Navigation;
using GrpcChatClient.Store;
using GrpcChatClient.ViewModel;
using System.Windows;

namespace GrpcChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AccountStore accountStore = new();
            NavigationMediator navigationMediator = new();

            // ..:: Create a channel connect to server ::..
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7165");

            navigationMediator.CurrentViewModel = new LoginViewModel(accountStore, navigationMediator, channel);

            MainWindow = new MainWindow(accountStore)
            {
                DataContext = new MainViewModel(navigationMediator)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
