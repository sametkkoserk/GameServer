using Network.Enum;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Network.Processor
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
            
            dispatcher.Dispatch(NetworkEvent.SendResponse, vo);
        }
    }
}