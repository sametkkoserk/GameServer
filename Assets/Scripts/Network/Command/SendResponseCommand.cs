using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;
using Network.Enum;

namespace Network.Command
{
    public class SendResponseCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.response);
            message.AddString("okay");
            networkManager.Server.Send(message,1);
            Debug.Log("Message sent");
            
            
        }
    }
}