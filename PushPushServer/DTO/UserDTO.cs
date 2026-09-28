namespace PushPushServer.DTO
{
    public class UserInfoRequest
    {
        public string token { get; set; } = "";
    }
    public class UserInfoResponse : BaseResponse
    {
        public string friendCode { get; set; } = ""; 
        public string nickName { get; set; } = "";
        public long exp { get; set; } = 0;
    }
}
