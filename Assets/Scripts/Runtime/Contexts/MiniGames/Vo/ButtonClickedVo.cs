using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Vo
{
    [ProtoContract]
    public class ClickedButtonsVo
    {
        [ProtoMember(1)]
        public List<string> buttons;
        
        [ProtoMember(2)]
        public float verticalAxis;
        [ProtoMember(3)]
        public float horizontalAxis;
        
        
        [ProtoMember(4)]
        public string lobbyCode;
    }
}