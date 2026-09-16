using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PushPushServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        [HttpPost(Name ="SetScore")]
        [HttpPost("SetScore")]
        public void SetScore(int objects)
        {

        }
    }
}
