using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
    [ProtoContract]
    public class InfoVo
    {
        [ProtoMember(1)] 
        public string message;
        
        [ProtoIgnore]
        public Dictionary<ushort, ClientVo> clients;
    }
}