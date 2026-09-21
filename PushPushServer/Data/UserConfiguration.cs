using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PushPushServer.Models;

namespace PushPushServer.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> e)
        {
            e.ToTable("users");
            e.HasKey(x => x.Uid);
            e.Property(x => x.Uid).ValueGeneratedOnAdd();
            e.Property(x => x.UserId).HasMaxLength(10).IsRequired();
            e.HasIndex(x => x.UserId).IsUnique();
            e.Property(x => x.FriendCode).HasMaxLength(8).IsRequired();
            e.HasIndex(x => x.FriendCode).IsUnique();
            e.Property(x => x.NickName).HasMaxLength(8).IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            e.Property(x => x.LastLoginAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }
    }
}
