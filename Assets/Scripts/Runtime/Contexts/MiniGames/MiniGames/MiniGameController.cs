using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Vo;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MiniGames.MiniGames
{
  public abstract class MiniGameController : MonoBehaviour
  {
    public MiniGameMediator miniGameMediator;
    public MiniGameKeyScriptable keys;
    public LobbyVo lobbyVo;
    public Transform playerContainer;
    public MapGenerator mapGenerator;
    public GameStartController gameStartController;


    public List<ushort> leaderBoard = new List<ushort>();


    public Dictionary<string, GameObject> objs = new ();
    public Dictionary<string, GameObject> newObjs = new ();
    public Dictionary<string, GameObject> changedObjs = new ();
    public List<string> removedObjs = new ();
    
    public Dictionary<string, Vector3> targets = new ();
    public Dictionary<string, Vector3> newTargets = new ();

    public Dictionary<ushort, GameObject> players = new ();
    private SendPacketToLobbyVo<MiniGameStateVo> stateVo = new();

    private int currentId=0;
    protected bool anythingChanged;

    public virtual void Init()
    {
      if (mapGenerator)
      {
        MiniGameMapGenerationVo vo = mapGenerator.SetMap();
        miniGameMediator.SendMap(vo);
        gameStartController=GetComponentInChildren<GameStartController>();
      }
      CreatePlayers();
    }

    public virtual void OnButtonClick(ushort clientId, ClickedButtonsVo vo)
    {
    }

    private void LateUpdate()
    {
      if (!anythingChanged)return;
      SetStateVo();
      miniGameMediator.SendState(stateVo);
    }

    private void SetStateVo()
    {
      stateVo.clients = lobbyVo.clients;
      stateVo.mainClass = new MiniGameStateVo()
      {
        changedPositions = changedObjs.ToDictionary(pair => pair.Key, pair => new Vector3Vo(pair.Value.transform.position)),
        changedRotations = changedObjs.ToDictionary(pair => pair.Key, pair => new QuaternionVo(pair.Value.transform.rotation)),

        newPositions = newObjs.ToDictionary(pair => pair.Key, pair => new Vector3Vo(pair.Value.transform.position)),
        newRotations = newObjs.ToDictionary(pair => pair.Key, pair => new QuaternionVo(pair.Value.transform.rotation)),
        
        removedObjs = removedObjs,
        
        newTargets = newTargets.ToDictionary(pair=>pair.Key,pair=>new Vector3Vo(pair.Value)),

        playerPositions = players.ToDictionary(pair => pair.Key, pair => new Vector3Vo(pair.Value.transform.position)),
        playerRotations = players.ToDictionary(pair => pair.Key, pair => new QuaternionVo(pair.Value.transform.rotation)),
      };
      ResetObjs();
    }

    private void ResetObjs()
    {
      objs.AddRange(newObjs);
      for (int i = 0; i < removedObjs.Count; i++)
      {
        objs.Remove(removedObjs[i]);
      }
      
      targets.AddRange(newTargets);
      
      newObjs.Clear();
      removedObjs.Clear();
      changedObjs.Clear();
      newTargets.Clear();
      anythingChanged = false;
    }

    public void ClientFinished(ushort clientId)
    {
      leaderBoard.Add(clientId);
      if (leaderBoard.Count == players.Count)
      {
        miniGameMediator.EndTheGame(leaderBoard);
      }
    }


    public virtual void CreateObject(string key, Transform transform)
    {
      Addressables.InstantiateAsync(key, transform).Completed += handle =>
      {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
          GameObject obj = handle.Result;
          newObjs[key + "_" + currentId] = obj;
          MiniGameObjController controller = obj.GetComponent<MiniGameObjController>();
          controller.key = key + "_" + currentId;
          controller.miniGameController = this;
          currentId += 1;
          anythingChanged = true;

        }
      };
    }

    public virtual void CreatePlayers()
    {
      for (int i = 0; i < lobbyVo.clients.Count; i++)
      {
        int index = i;

        Addressables.InstantiateAsync(keys.player, playerContainer).Completed += handle =>
        {
          if (handle.Status != AsyncOperationStatus.Succeeded) return;
          GameObject obj = handle.Result;
          obj.transform.position=gameStartController.GetNextPoint();
          players[lobbyVo.clients.ElementAt(index).Value.id] = obj;
          anythingChanged = true;
        };
      }
    }
  }
}