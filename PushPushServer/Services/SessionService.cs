using StackExchange.Redis;
using System.Security.Cryptography;

namespace PushPushServer.Services
{
    public class SessionService
    {
        private static readonly TimeSpan SessionTtl = TimeSpan.FromHours(24);
        private readonly IDatabase _redis;

        public SessionService(IConnectionMultiplexer redis) => _redis = redis.GetDatabase();

        public async Task<string> CreateAsync(long uid)
        {
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var userKey = $"user:{uid}:session";

            // 이전 로그인 토큰이 있으면 무효화 (중복 로그인 방지)
            var oldToken = await _redis.StringGetAsync(userKey);
            if (oldToken.HasValue)
                await _redis.KeyDeleteAsync($"session:{oldToken}");

            await _redis.StringSetAsync($"session:{token}", uid, SessionTtl);
            await _redis.StringSetAsync(userKey, token, SessionTtl);
            return token;
        }

        // 다음 API들에서 토큰 → Uid 확인할 때 사용
        public async Task<long?> GetUidAsync(string token)
        {
            var value = await _redis.StringGetAsync($"session:{token}");
            return value.HasValue ? (long)value : null;
        }
    }
}
