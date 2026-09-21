using System.ComponentModel.DataAnnotations;

namespace PushPushServer.DTO
{
    public class LoginRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 4)]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "영문, 숫자, _만 사용할 수 있습니다.")]
        public string UserId { get; set; } = "";        
    }

    public class LoginResponse : BaseResponse
    {
        public string token = "";
        public string friendCode = "";
        public string nickName = "";
    };

    public class CreateRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 4)]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "영문, 숫자, _만 사용할 수 있습니다.")]
        public string UserId { get; set; } = "";

        [Required]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "닉네임은 {2}-{1}자로 입력해 주세요.")]
        [RegularExpression("^[가-힣a-zA-Z0-9_]+$", ErrorMessage = "한글, 영문, 숫자, _만 사용할 수 있습니다.")]
        public string NickName { get; set; } = "";
    }

}
