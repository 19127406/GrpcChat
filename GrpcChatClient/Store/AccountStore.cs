using GrpcChatClient.Model;

namespace GrpcChatClient.Store
{
    public class AccountStore
    {
        private UserModel _currentUser;
        public UserModel CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
    }
}
