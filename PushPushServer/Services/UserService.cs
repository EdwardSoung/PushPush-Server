using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PushPushServer.Data;
using PushPushServer.Models;

namespace PushPushServer.Services
{
    public class UserService
    {
        private const int MaxCreateRetry = 5;
        private readonly GameDBContext _db;

        public UserService(GameDBContext db) => _db = db;

        public async Task<User?> LoginAsync(string userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return null;

            user.LastLoginAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> CreateAsync(string userId, string nickName)
        {
            //충돌나면 재시도 처리하게 반복
            for (var i = 0; i < MaxCreateRetry; i++)
            {
                var now = DateTime.UtcNow;
                var newUser = new User
                {
                    UserId = userId,
                    FriendCode = FriendCodeGenerator.Generate(),
                    NickName = nickName,
                    CreatedAt = now,
                    LastLoginAt = now,
                };
                _db.Users.Add(newUser);

                try
                {
                    await _db.SaveChangesAsync();
                    return newUser;
                }
                catch (DbUpdateException ex) when (IsDuplicateKey(ex))
                {
                    _db.ChangeTracker.Clear();   // 실패한 엔티티 추적 해제

                    // 같은 userId로 동시에 요청이 들어와 먼저 생성된 경우 → 그 유저 사용
                    var existing = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                    if (existing != null) return existing;

                    // 아니면 FriendCode 충돌 → 새 코드로 재시도
                }
            }

            throw new InvalidOperationException("유저 생성 실패: 친구코드 발급 재시도 횟수 초과");
        }

        private static bool IsDuplicateKey(DbUpdateException ex)
            => ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.DuplicateKeyEntry };

        public async Task<User?> GetUserData(long uid)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Uid == uid);
            if (user == null) return null;

            return user;
        }
    }
}
