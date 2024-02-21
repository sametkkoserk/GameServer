using System.Collections;
using Runtime.Modules.Core.Discord.Enum;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Networking;

namespace Runtime.Modules.Core.Discord.View.Behaviour
{
  public class DiscordBehaviourMediator : EventMediator
  {
    [Inject]
    public DiscordBehaviourView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(DiscordEvent.NewPlayerJoined, SendDiscordMessage);
    }
    
    private void SendDiscordMessage(IEvent payload)
    {
      string UserName = (string)payload.data;
      StartCoroutine(SendWebHook(DiscordLinkKey.newPlayer, $"{UserName} Joined The Game!"));
    }
    
    public IEnumerator SendWebHook(string link, string message)
    {
      yield return new WaitForSeconds(2f);

      WWWForm form = new();
      form.AddField("content", message);

      using UnityWebRequest www = UnityWebRequest.Post(link, form);
      yield return www.SendWebRequest();

      // Debug.Log(www.result == UnityWebRequest.Result.Success ? "Success!" : www.error);
    }

    public override void OnRemove()
    {
      dispatcher.AddListener(DiscordEvent.NewPlayerJoined, SendDiscordMessage);
    }
  }
}