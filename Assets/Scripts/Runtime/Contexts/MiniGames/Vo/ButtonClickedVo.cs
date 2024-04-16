using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Vo
{
    [ProtoContract]
    public class ClickedButtonsVo
    {
        [ProtoMember(1)]
        public List<string> clickedButtons;
        [ProtoMember(2)]
        public List<string> releasedButtons;
        
        [ProtoMember(3)]
        public float verticalAxis;
        [ProtoMember(4)]
        public float horizontalAxis;
        
        
        [ProtoMember(5)]
        public string lobbyCode;
    }
}