using Riptide;
using Runtime.Network.Enum;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Network.Processor
{
    public class HandleMessageProcessor : EventCommand
    {
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            ushort fromId = vo.fromId;
            Message message = vo.message;
            string testMessage = message.GetString();
            
            Debug.Log(fromId);
            Debug.Log(testMessage);
            
            dispatcher.Dispatch(NetworkEvent.SEND_RESPONSE, vo);
        }
    }
}