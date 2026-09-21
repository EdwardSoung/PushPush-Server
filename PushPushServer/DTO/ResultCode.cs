namespace PushPushServer.DTO
{
    public enum ResultCode
    {
        Success = 0,

        // 공통 (1~999)
        InvalidRequest = 1,      // 요청 형식/검증 오류
        ServerError = 2,
        Unauthorized = 3,        // 토큰 없음/만료

        // 계정 (1000~)
        UserNotFound = 1001,
        DuplicateUserId = 1002,
        CreateUserFail = 1003,
    }
}
