using GrpcChatClient.Navigation;

namespace GrpcChatClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationMediator _navigationMediator;
        public ViewModelBase CurrentViewModel => _navigationMediator.CurrentViewModel;

        public MainViewModel(NavigationMediator mediator)
        {
            _navigationMediator = mediator;
            _navigationMediator.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
