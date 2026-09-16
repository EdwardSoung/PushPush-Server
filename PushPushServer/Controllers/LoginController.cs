using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PushPushServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //로그인 userId DB 조회
        //존재하면 음... 뭔가 유저 고유 키값 필요..
        //없으면 만들어줌
        //토큰 만들어서 내려줌
        //유저 고유 키값으로 나머지 데이터 처리 필요함...
    }
}
