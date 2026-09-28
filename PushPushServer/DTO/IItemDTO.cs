namespace PushPushServer.DTO
{
    public class ItemDTO
    {
        public long Uid { get; set; } = 0;
        public int DataId { get; set; } = 0;
        public long Amount { get; set; } = 0;
    }
    public class GetItemRequest
    {
        public string token { get; set; } = "";
    }

    public class GetItemResponse : BaseResponse
    {
        public List<ItemDTO> Items { get; set; } = new();
    }
}
