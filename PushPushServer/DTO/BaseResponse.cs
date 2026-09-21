namespace PushPushServer.DTO
{
    public class BaseResponse
    {
        public ResultCode ResultCode { get; set; } = ResultCode.Success;
        public string? Message { get; set; }

        public static T Fail<T>(ResultCode code, string? message = null) where T : BaseResponse, new()
            => new T { ResultCode = code, Message = message };
    }
}
