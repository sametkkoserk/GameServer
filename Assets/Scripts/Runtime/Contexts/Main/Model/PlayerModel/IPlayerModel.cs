using System.Collections.Generic;
using Runtime.Contexts.Main.Vo;

namespace Runtime.Contexts.Main.Model.PlayerModel
{
    public interface IPlayerModel
    {
        public Dictionary<ushort, RegisterInfoVo> userList { get; set; }
    }
}