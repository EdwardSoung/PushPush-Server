using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PushPushServer.Models;

namespace PushPushServer.Data.Configuration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> e)
        {
            e.ToTable("items");
            e.HasKey(x => x.Uid);
            e.Property(x => x.Uid).ValueGeneratedOnAdd();
            e.Property(x => x.UserId).HasMaxLength(10).IsRequired();
            e.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
