using Riptide;

namespace Network.Vo
{
    public class MessageReceivedVo
    {
        public ushort fromId { get; set; }
        public Message message { get; set; }
    }
}