using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PushPushServer.DTO;
using PushPushServer.Services;

namespace PushPushServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ItemService _itemService;
        private readonly SessionService _sessionService;

        public ItemController(UserService userService, ItemService itemService, SessionService sessionService)
        {
            _userService = userService;
            _itemService = itemService;
            _sessionService = sessionService;
        }

        [HttpPost("getitems")]
        public async Task<ActionResult<GetItemResponse>> GetUserInfo([FromBody] GetItemRequest request)
        {
            var uid = await _sessionService.GetUidAsync(request.token);

            if (uid == null)
            {
                //토큰 만료
                return BaseResponse.Fail<GetItemResponse>(ResultCode.Unauthorized);
            }

            long uidValue = uid.Value;
            var user = await _userService.GetUserData(uidValue);

            if (user == null)
                return BaseResponse.Fail<GetItemResponse>(ResultCode.UserNotFound);

            var items = await _itemService.GetItemData(user.UserId);

            return new GetItemResponse { Items = items };
        }
    }
}
