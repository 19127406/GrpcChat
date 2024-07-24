using GrpcChatClient.Command;
using GrpcChatClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcChatClient.Navigation
{
    /// <summary>
    /// A mediator to observe changes in current viewmodel -> applied to UI PropertyChanged to update view 
    /// </summary>
    public class NavigationMediator
    {
        public event Action? CurrentViewModelChanged;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

    }
}
