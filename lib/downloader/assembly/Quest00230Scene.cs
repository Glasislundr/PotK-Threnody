// Decompiled with JetBrains decompiler
// Type: Quest00230Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00230Scene : NGSceneBase
{
  [SerializeField]
  private Quest00230Menu menu;

  public static void ChangeScene(bool stack, int period_id)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_30", (stack ? 1 : 0) != 0, (object) period_id);
  }

  public static void ChangeScene(bool stack, EventInfo eventInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_30", (stack ? 1 : 0) != 0, (object) eventInfo);
  }

  public IEnumerator onStartSceneAsync(EventInfo eventInfo)
  {
    Quest00230Scene quest00230Scene = this;
    quest00230Scene.continueBackground = true;
    PeriodBackground[] array = MasterData.PeriodBackground.Where<KeyValuePair<int, PeriodBackground>>((Func<KeyValuePair<int, PeriodBackground>, bool>) (x => x.Value.ID == eventInfo.period_id)).Select<KeyValuePair<int, PeriodBackground>, PeriodBackground>((Func<KeyValuePair<int, PeriodBackground>, PeriodBackground>) (x => x.Value)).ToArray<PeriodBackground>();
    string name = (string) null;
    if (array.Length != 0)
      name = array[0].background_image_name;
    IEnumerator e1;
    if (name != null)
    {
      e1 = ((Component) quest00230Scene).GetComponent<BGChange>().ExtraBGprefabCreate(name);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    if (eventInfo.IsGuild())
    {
      quest00230Scene.bgmFile = "BgmGuild";
      quest00230Scene.bgmName = "bgm085";
      quest00230Scene.currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.Closed;
    }
    else
    {
      quest00230Scene.bgmFile = "";
      quest00230Scene.bgmName = "bgm001";
      Singleton<CommonRoot>.GetInstance().guildChatManager.CloseGuildChat();
      quest00230Scene.currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.Closed;
    }
    Future<WebAPI.Response.EventTop> request = WebAPI.EventTop(eventInfo.period_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    e1 = request.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.EventTop result = request.Result;
    if (result != null)
    {
      if (Object.op_Inequality((Object) quest00230Scene.menu, (Object) null))
      {
        e1 = quest00230Scene.menu.Init(eventInfo, result);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public IEnumerator onStartSceneAsync(int period_id)
  {
    Future<WebAPI.Response.QuestProgressExtra> extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = extra.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (extra.Result != null)
    {
      int? nullable = ((IEnumerable<EventInfo>) extra.Result.event_infos).FirstIndexOrNull<EventInfo>((Func<EventInfo, bool>) (x => x.period_id == period_id));
      if (!nullable.HasValue)
      {
        Debug.LogError((object) ("event not found period_id = " + (object) period_id));
      }
      else
      {
        e = this.onStartSceneAsync(extra.Result.event_infos[nullable.Value]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }
}
