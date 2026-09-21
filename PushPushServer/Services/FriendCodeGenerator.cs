using System.Security.Cryptography;

namespace PushPushServer.Services
{
    public static class FriendCodeGenerator
    {
        private const string Chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

        public static string Generate(int length = 8)
            => RandomNumberGenerator.GetString(Chars, length);
    }
}
