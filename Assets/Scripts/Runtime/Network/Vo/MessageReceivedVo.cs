using Newtonsoft.Json;
using Riptide;

namespace Runtime.Network.Vo
{
    public class MessageReceivedVo
    {
        public ushort fromId { get; set; }
        public string message { get; set; }
        
    }
}