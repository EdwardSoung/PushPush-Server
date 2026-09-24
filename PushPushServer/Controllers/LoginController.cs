using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PushPushServer.DTO;
using PushPushServer.Services;

namespace PushPushServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly SessionService _sessionService;

        public LoginController(UserService userService, SessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.LoginAsync(request.UserId);
            if (user == null)
                return BaseResponse.Fail<LoginResponse>(ResultCode.UserNotFound, "계정이 존재하지 않습니다");

            //계정 있으면 로그인
            var userToken = await _sessionService.CreateAsync(user.Uid);

            return new LoginResponse { token = userToken };
        }

        [HttpPost("create")]
        public async Task<ActionResult<LoginResponse>> Create([FromBody] CreateRequest request)
        {
            var user = await _userService.CreateAsync(request.UserId, request.NickName);

            if (user == null)
                return BaseResponse.Fail<LoginResponse>(ResultCode.CreateUserFail);

            var userToken = await _sessionService.CreateAsync(user.Uid);

            return new LoginResponse { token = userToken};
        }
    }
}
