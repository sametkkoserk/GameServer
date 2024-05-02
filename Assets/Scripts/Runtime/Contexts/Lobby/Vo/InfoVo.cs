using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
    [ProtoContract]
    public class InfoVo
    {
        [ProtoMember(1)]
        public Dictionary<ushort, ClientVo> clients;
    }
}