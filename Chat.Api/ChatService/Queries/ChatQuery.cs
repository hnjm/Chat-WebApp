using Chat.Api.Core.Models;

namespace Chat.Api.ChatService.Queries
{
    public class ChatQuery : AQuery
    {
        public string UserId {get; set;} = string.Empty;
        public string SendTo {get; set;} = string.Empty;

        public override void ValidateQuery()
        {
            if (string.IsNullOrEmpty(UserId)) 
            {
                throw new Exception("UserId not set");
            }
            if (string.IsNullOrEmpty(SendTo))
            {
                throw new Exception("SendTo not set");
            }
        }
    }
}