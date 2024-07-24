using GrpcChatClient.Navigation;
using GrpcChatClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcChatClient.Command
{
    /// <summary>
    /// Command to navigate between different views
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel represents different view</typeparam>
    public class NavigateCommand<TViewModel> : BaseCommand
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
