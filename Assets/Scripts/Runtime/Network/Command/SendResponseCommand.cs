using Riptide;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Network.Command
{
    public class SendResponseCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.RESPONSE);
            message.AddString("okay");
            networkManager.Server.Send(message,vo.fromId);
            Debug.Log("Message sent");
            
            
        }
    }
}