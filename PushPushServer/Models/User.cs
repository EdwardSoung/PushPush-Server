namespace PushPushServer.Models
{
    public class User
    {
        public long Uid { get; set; }
        public string UserId { get; set; } = "";
        public string FriendCode { get; set; } = "";
        public string NickName { get; set; } = "";
        public long Exp { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
    }
}
