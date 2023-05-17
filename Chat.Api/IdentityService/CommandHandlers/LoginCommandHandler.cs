using System.Composition;
using Chat.Api.ActivityService.Commands;
using Chat.Api.Core.Constants;
using Chat.Api.Core.Helpers;
using Chat.Api.Core.Interfaces;
using Chat.Api.Core.Models;
using Chat.Api.Core.Services;
using Chat.Api.IdentityService.Interfaces;
using Chat.Api.IdentityService.Models;

namespace Chat.Api.IdentityService.Commands
{
    [Export(typeof(ICommandHandler))]
    [Export("LoginCommandHandler", typeof(ICommandHandler))]
    [Shared]
    public class LoginCommandHandler : ACommandHandler<LoginCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ICommandService _commandService;

        public LoginCommandHandler()
        {
            _userRepository = DIService.Instance.GetService<IUserRepository>();
            _tokenService = DIService.Instance.GetService<ITokenService>();
            _commandService = DIService.Instance.GetService<ICommandService>();
        }

        public override async Task<CommandResponse> OnExecuteAsync(LoginCommand command)
        {
            var response = command.CreateResponse();
            var user = await _userRepository.GetUserAsync(command.Email, command.Password);
            
            if (user == null) 
            {
                throw new Exception("Email or Password error!!");
            }

            var userProfile = user.ToUserProfile();
            var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);

            if (token == null)
            {
                throw new Exception("Token Creation Failed");
            }
            
            response.SetData("Token", token);
            response.Message = "Logged in successfully";

            await UpdateLastSeenActivity(user);

            return response;
        }

        private async Task UpdateLastSeenActivity(UserModel user)
        {
            var updateLastSeenCommand = new UpdateLastSeenCommand
            {
                UserId = user.Id
            };
            await _commandService.ExecuteCommandAsync(updateLastSeenCommand);
        }
    }
}