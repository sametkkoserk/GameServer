using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.MiniGames.Vo;
using Unity.VisualScripting;
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
            anythingChanged = true;
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
                        obj.transform.position=gameStartController.GetNextPoint();

                        players[lobbyVo.clients.ElementAt(index).Value.id] = obj;
                        
                        CarController carController=obj.GetComponent<CarController>();
                        carControllers[lobbyVo.clients.ElementAt(index).Value.id] =carController;
                        carController.clientId = lobbyVo.clients.ElementAt(index).Value.id;
                        carController.miniGameController = this;
                        
                        playerStates[lobbyVo.clients.ElementAt(index).Value.id] = -1;
                        anythingChanged = true;

                    }
                } ;
            }
        }


    }
}