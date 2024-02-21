using System.Collections.Generic;
using Runtime.Contexts.Main.Vo;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Model.PlayerModel
{
    public class PlayerModel : IPlayerModel
    {
        public Dictionary<ushort, RegisterInfoVo> userList { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            userList = new Dictionary<ushort, RegisterInfoVo>();
        }
    }
}