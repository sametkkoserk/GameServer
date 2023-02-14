using Network.Enum;
using Network.Vo;
using Riptide;
using Riptide.Utils;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Network.Services.NetworkManager
{
    public class NetworkManagerService : INetworkManagerService 
    {
        
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher{ get; set;}
        
        public Server Server { get; private set; }

        private ushort port;
        private ushort maxClientCount;

        public void StartServer(ushort _port ,ushort _maxClientCount)
        {
            port = _port;
            maxClientCount = _maxClientCount;
            
            RiptideLogger.Initialize(Debug.Log,Debug.Log,Debug.LogWarning,Debug.LogError,false);
            Server = new Server();
            Server.Start(port,maxClientCount);
            
            Server.MessageReceived+= MessageHandler;
        }

        public void Ticker()
        {
            if (Server != null)
            {
                Server.Update();
            }
        }
        
        public void MessageHandler(object sender, MessageReceivedEventArgs messageArgs)
        {
            MessageReceivedVo vo = new MessageReceivedVo();
            vo.fromId = messageArgs.FromConnection.Id;
            vo.message = messageArgs.Message;
            dispatcher.Dispatch((ClientToServerId)messageArgs.MessageId,vo);
        }

        public void OnQuit()
        {
            Server.Stop();
        }
    }
}
