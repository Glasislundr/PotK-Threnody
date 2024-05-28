// Decompiled with JetBrains decompiler
// Type: Quest00220Scene
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
[AddComponentMenu("Scenes/QuestExtra/S_Scene")]
public class Quest00220Scene : NGSceneBase
{
  private static bool isInit;
  public Quest00220Menu menu;
  public BGChange bgchange;
  private static bool keyQuest;

  public static void ChangeScene00220(
    bool stack,
    int L,
    int M,
    bool Guerrilla = false,
    bool isKeyQuest = false,
    bool isForces = false)
  {
    Quest00220Scene.isInit = true;
    Quest00220Scene.keyQuest = isKeyQuest;
    Quest00220SceneData quest00220SceneData = new Quest00220SceneData(L, M, -1, isForces, Guerrilla);
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_20", (stack ? 1 : 0) != 0, (object) quest00220SceneData);
  }

  public static void ChangeScene00220(int sid, bool isKeyQuest = false, bool isForces = false)
  {
    Quest00220Scene.isInit = true;
    Quest00220Scene.keyQuest = isKeyQuest;
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_20", false, (object) sid, (object) isForces);
  }

  public IEnumerator onStartSceneAsync(int id, bool isForces)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e;
    if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
    {
      Future<WebAPI.Response.QuestProgressExtra> Extra = WebAPI.QuestProgressExtra();
      e = Extra.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Extra.Result != null)
        WebAPI.SetLatestResponsedAt("QuestProgressExtra");
      Extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
    }
    PlayerExtraQuestS playerExtraQuestS = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s != null && x.quest_extra_s.ID == id)).FirstOrDefault<PlayerExtraQuestS>();
    e = this.onStartSceneAsync(new Quest00220SceneData(playerExtraQuestS.quest_extra_s.quest_l_QuestExtraL, playerExtraQuestS.quest_extra_s.quest_m_QuestExtraM, id, isForces, playerExtraQuestS.seek_type == PlayerExtraQuestS.SeekType.M));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Quest00220SceneData data)
  {
    Quest00220Scene quest00220Scene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e;
    if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
    {
      e = WebAPI.QuestProgressExtra().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      WebAPI.SetLatestResponsedAt("QuestProgressExtra");
    }
    PlayerExtraQuestS[] playerExtraQuestSArray = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).S(data.L, data.M);
    if (playerExtraQuestSArray == null || playerExtraQuestSArray.Length == 0)
    {
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
    }
    else
    {
      PlayerExtraQuestS[] ExtraData = SMManager.Get<PlayerExtraQuestS[]>();
      PlayerExtraQuestS[] extraData = ((IEnumerable<PlayerExtraQuestS>) ExtraData).S(data.L, data.M);
      if (Quest00220Scene.isInit)
      {
        Quest00220Scene.isInit = false;
        quest00220Scene.menu.tweenSettingDefault();
      }
      if (extraData[0].seek_type == PlayerExtraQuestS.SeekType.M)
      {
        e = ((Component) quest00220Scene).GetComponent<BGChange>().ExtraBGprefabCreate(extraData[0].quest_extra_s.quest_l.background_image_name);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else if (extraData[0].seek_type == PlayerExtraQuestS.SeekType.L)
      {
        quest00220Scene.bgchange.getCurrentBG();
        if (Object.op_Equality((Object) quest00220Scene.bgchange.Current, (Object) null))
        {
          e = quest00220Scene.bgchange.ExtraBGprefabCreate(extraData[0].quest_extra_s.quest_l.background_image_name);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      quest00220Scene.menu.startAllpostween = new List<TweenPosition>();
      quest00220Scene.menu.startAllalphatween = new List<TweenAlpha>();
      // ISSUE: reference to a compiler-generated method
      ((IEnumerable<TweenPosition>) ((Component) quest00220Scene).GetComponentsInChildren<TweenPosition>()).ForEach<TweenPosition>(new Action<TweenPosition>(quest00220Scene.\u003ConStartSceneAsync\u003Eb__7_0));
      // ISSUE: reference to a compiler-generated method
      ((IEnumerable<TweenAlpha>) ((Component) quest00220Scene).GetComponentsInChildren<TweenAlpha>()).ForEach<TweenAlpha>(new Action<TweenAlpha>(quest00220Scene.\u003ConStartSceneAsync\u003Eb__7_1));
      quest00220Scene.menu.startAllpostween.ForEach((Action<TweenPosition>) (x =>
      {
        if (((UITweener) x).tweenGroup != -11 && ((UITweener) x).tweenGroup != -12)
          return;
        ((Component) x).gameObject.transform.localPosition = x.to;
      }));
      quest00220Scene.menu.startAllalphatween.ForEach((Action<TweenAlpha>) (x =>
      {
        if (((UITweener) x).tweenGroup != -11 && ((UITweener) x).tweenGroup != -12)
          return;
        ((UIRect) ((Component) x).GetComponent<UIWidget>()).alpha = x.to;
      }));
      e = quest00220Scene.menu.Initialize(ExtraData, data.L, data.M, data.S, data.Forcus, Quest00220Scene.keyQuest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (extraData.Length != 0)
        quest00220Scene.menu.isSideQuest = extraData[0].IsSideQuest();
    }
  }

  public void onStartScene(int id, bool isForces)
  {
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), this.sceneName), this.sceneName);
    Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, id);
    this.menu.HscrollButtonsAction();
    this.menu.SceneStart = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(Quest00220SceneData data)
  {
    PlayerExtraQuestS[] playerExtraQuestSArray = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).S(data.L, data.M);
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), this.sceneName), this.sceneName);
    if (playerExtraQuestSArray != null && playerExtraQuestSArray.Length != 0)
      Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, playerExtraQuestSArray[0]._quest_extra_s);
    this.menu.SceneStart = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void onEndScene()
  {
    foreach (GameObject hscrollButton in this.menu.hscrollButtons)
      hscrollButton.GetComponent<Quest0022Hscroll>().centerAnimation(false);
    this.menu.indicator.SeEnable = false;
    this.menu.nowCenterObj = (GameObject) null;
    this.menu.SceneStart = false;
    this.menu.ButtonMove = false;
    this.menu.onEndScene();
  }
}
