using Multiplayer.Enum;
using Riptide;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Multiplayer.Processor
{
    public class HandleMessageProcessor : EventCommand
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher{ get; set;}
        [MessageHandler((ushort)ClientToServerId.test)]
        private static void Test(ushort fromClientId, Message message)
        {
            Debug.Log(message.GetString());
            //dispatcher.Dispatch(NetworkEvent.SendResponse);
        }
    }
}