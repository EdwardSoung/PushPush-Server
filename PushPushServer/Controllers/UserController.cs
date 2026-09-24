using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PushPushServer.DTO;
using PushPushServer.Services;

namespace PushPushServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly SessionService _sessionService;

        public UserController(UserService userService, SessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpPost("getuserinfo")]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo([FromBody] UserInfoRequest request)
        {
            var uid = await _sessionService.GetUidAsync(request.token);

            if (uid == null)
            {
                //토큰 만료
                return BaseResponse.Fail<UserInfoResponse>(ResultCode.Unauthorized);
            }

            long uidValue = uid.Value;
            var user = await _userService.GetUserData(uidValue);

            if (user == null)
                return BaseResponse.Fail<UserInfoResponse>(ResultCode.UserNotFound);

            return new UserInfoResponse { nickName = user.NickName, friendCode = user.FriendCode, exp = user.Exp };
        }
    }
}
