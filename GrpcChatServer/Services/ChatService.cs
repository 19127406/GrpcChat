using Grpc.ChatServer;
using Grpc.Core;

namespace GrpcChat.Services
{
    public class ChatService : Chat.ChatBase
    {
        private readonly ILogger<ChatService> _logger;
        private readonly MessageStreamService _streamingService;

        public ChatService(ILogger<ChatService> logger, MessageStreamService streamingService)
        {
            _logger = logger;
            _streamingService = streamingService;
        }

        public override async Task<MessageResponse> Login(LoginRequest request, ServerCallContext context)
        {
            bool check = _streamingService.Login(request.Name);
            if (check)
            {
                _logger.LogInformation("User {Name} logged in", request.Name);

                return await Task.FromResult(new MessageResponse { Success = true });
            }
            else
            {
                return await Task.FromResult(new MessageResponse { Success = false, Info = "User already existed" });
            }
        }

        public override async Task<MessageResponse> Logout(LogoutRequest request, ServerCallContext context)
        {
            _streamingService.Logout(request.Name);

            _logger.LogInformation("User {Name} logged out", request.Name);

            return await Task.FromResult(new MessageResponse { Success = true });
        }

        public override async Task<CheckResponse> IsRoomOrDirect(CheckRequest request, ServerCallContext context)
        {
            bool check = _streamingService.IsRoom(request.Name, out string info);
            if (string.IsNullOrEmpty(info))
                return await Task.FromResult(new CheckResponse { IsRoom = check });
            else
                return await Task.FromResult(new CheckResponse { IsRoom = check, Info = info });
        }

        public override async Task EnterChat(EnterRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            bool e = _streamingService.Enter(request.Name, responseStream, out string info);

            if (e)
            {
                _logger.LogInformation("{Name} entered the chat", request.Name);

                await _streamingService.SendMessage(new ChatMessage
                {
                    Sender = request.Name,
                    Receiver = request.Room,
                    Datetime = DateTime.UtcNow.ToString(),
                    Message = $"User with name {request.Name} entered the chat",
                    IsEnter = true
                });

                WaitForMessages();
            }
            else
            {
                _logger.LogError(info);
            }
        }

        public override async Task<MessageResponse> JoinRoom(EnterRequest request, ServerCallContext context)
        {
            bool isSuccess = _streamingService.JoinRoom(request.Name, request.Room, request.IsPrivate, out string info);

            if (isSuccess)
            {
                _logger.LogInformation("{Name} joined room {Room}", request.Name, request.Room);

                await _streamingService.SendMessage(new ChatMessage
                {
                    Sender = request.Name,
                    Receiver = request.Room,
                    Datetime = DateTime.UtcNow.ToString(),
                    Message = $"User with name {request.Name} just joined the room",
                    IsEnter = false
                });

                return new MessageResponse { Success = true };
            }
            else
            {
                _logger.LogError(info);
                return await Task.FromResult(new MessageResponse { Success = false, Info = info });
            }
        }

        public override async Task<MessageResponse> SendMessage(ChatMessage request, ServerCallContext context)
        {
            await _streamingService.SendMessage(request);
            return new MessageResponse() { Success = true };
        }

        public override async Task<MessageResponse> SendMessageDirect(ChatMessage request, ServerCallContext context)
        {
            await _streamingService.SendMessageDirect(request);
            return new MessageResponse { Success = true };
        }

        public void WaitForMessages()
        {
            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
