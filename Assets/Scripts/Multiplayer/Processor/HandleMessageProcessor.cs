using Multiplayer.Enum;
using RiptideNetworking;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.impl;
using UnityEngine;

namespace Multiplayer.Processor
{
    public class HandleMessageProcessor : EventCommand
    {
        [MessageHandler((ushort)ClientToServerId.test)]
        private static void Test(ushort fromClientId, Message message)
        {
            Debug.Log(message.GetString());
        }
    }
}