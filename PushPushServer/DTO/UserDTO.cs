namespace PushPushServer.DTO
{
    public class UserInfoRequest
    {
        public string token = "";
    }
    public class UserInfoResponse : BaseResponse
    {
        public string friendCode = "";
        public string nickName = "";
        public long exp = 0;
    }
}
