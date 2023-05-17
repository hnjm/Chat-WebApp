using Chat.Api.Core.Models;

namespace Chat.Api.ContactService.Commands
{
    public class AddContactCommand : ACommand
    {
        public string ContactEmail {get; set;} = string.Empty;
        public string UserId {get; set;} = string.Empty;
        
        public override void ValidateCommand()
        {
            if (string.IsNullOrEmpty(ContactEmail))
            {
                throw new Exception("ContactEmail not set");
            }
            if (string.IsNullOrEmpty(UserId))
            {
                throw new Exception("UserId not set");
            }
        }
    }
}