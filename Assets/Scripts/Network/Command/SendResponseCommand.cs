using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;
using Network.Enum;
using Network.Vo;

namespace Network.Command
{
    public class SendResponseCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.response);
            message.AddString("okay");
            networkManager.Server.Send(message,vo.fromId);
            Debug.Log("Message sent");
            
            
        }
    }
}