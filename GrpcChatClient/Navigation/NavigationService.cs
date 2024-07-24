using GrpcChatClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcChatClient.Navigation
{
    public class NavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly NavigationMediator _navigationMediator;
        private Func<TViewModel> _createViewModel;

        public NavigationService(NavigationMediator navigationMediator, Func<TViewModel> createViewModel)
        {
            _navigationMediator = navigationMediator;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationMediator.CurrentViewModel = _createViewModel();
        }
    }
}
