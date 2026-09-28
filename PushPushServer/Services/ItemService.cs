using Microsoft.EntityFrameworkCore;
using PushPushServer.Data;
using PushPushServer.DTO;
using PushPushServer.Models;

namespace PushPushServer.Services
{
    public class ItemService
    {
        private readonly GameDBContext _db;

        public ItemService(GameDBContext db) => _db = db;

        public async Task<List<ItemDTO>> GetItemData(string userId)
        {
            var items = await _db.Items
                .Where(i=> i.UserId == userId)
                .Select(i => new ItemDTO { Uid = i.Uid, DataId = i.DataId, Amount = i.Amount})
                .ToListAsync();

            if (items == null) return new List<ItemDTO>();

            return items;
        }
    }
}
