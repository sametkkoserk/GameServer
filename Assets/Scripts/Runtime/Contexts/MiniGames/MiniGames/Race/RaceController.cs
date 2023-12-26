using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.MiniGames.Vo;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MiniGames.MiniGames.Race
{
    public class RaceController : MiniGameController
    {
        private Dictionary<ushort, CarController> carControllers=new Dictionary<ushort, CarController>();
        
        public override void OnButtonClick(ushort clientId, ClickedButtonsVo vo)
        {
            carControllers[clientId]
                .SetValues(vo.verticalAxis, vo.horizontalAxis, false);
        }
        public override void CreatePlayers()
        {
            for (int i = 0; i < lobbyVo.clients.Count; i++)
            {
                int index = i;
                Addressables.InstantiateAsync(keys.player, playerContainer).Completed+= handle =>
                {
                    if (handle.Status==AsyncOperationStatus.Succeeded)
                    {
                        GameObject obj = handle.Result;
                        players[lobbyVo.clients.ElementAt(index).Value.id] = obj;
                        carControllers[lobbyVo.clients.ElementAt(index).Value.id] = obj.GetComponent<CarController>();
                        carControllers[lobbyVo.clients.ElementAt(index).Value.id].clientId =
                            lobbyVo.clients.ElementAt(index).Value.id;
                    }
                } ;
            }
        }


    }
}