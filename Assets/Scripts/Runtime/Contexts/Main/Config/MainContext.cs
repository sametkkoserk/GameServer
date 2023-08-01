using Runtime.Contexts.Main.Command;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Processor;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.Discord.View.Behaviour;
using Runtime.Modules.Core.GeneralContext;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Contexts.Main.Config
{
    public class MainContext : GeneralContext
    {
        public MainContext (MonoBehaviour view) : base(view)
        {
        }

        public MainContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            base.mapBindings();
            
            injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton().CrossContext();

            mediationBinder.Bind<DiscordBehaviourView>().To<DiscordBehaviourMediator>();
            
            commandBinder.Bind(ContextEvent.START).To<LoadNetworkSceneCommand>();

            commandBinder.Bind(MainEvent.CheckUserInfo).To<CheckUserInfoCommand>();
            
            commandBinder.Bind(ClientToServerId.Register).To<GetRegisterInfoProcessor>();
        }
    }
}

