namespace PushPushServer.Models
{
    public class Item
    {
        public long Uid { get; set; }
        public string UserId { get; set; } = "";
        public int DataId {  get; set; }
        public long Amount { get; set; }
    }
}
