// Decompiled with JetBrains decompiler
// Type: CorpsQuestTopScene
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
[AddComponentMenu("Scenes/CorpsQuest/TopScene")]
public class CorpsQuestTopScene : NGSceneBase
{
  private CorpsQuestTopMenu menu;
  private bool playStory;
  private PlayerCorps mCorpsData;
  private CommonCorpsHeader mCorpsHeader;
  private CorpsPeriod masterPeriod;
  private CorpsSetting masterSetting;
  private Modified<PlayerCorps[]> mCorpsObserver;

  public static void ChangeScene(int periodId, bool isAllClear = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_top", false, (object) periodId, (object) isAllClear);
  }

  public override IEnumerator onInitSceneAsync()
  {
    CorpsQuestTopScene corpsQuestTopScene = this;
    corpsQuestTopScene.menu = corpsQuestTopScene.menuBase as CorpsQuestTopMenu;
    corpsQuestTopScene.mCorpsHeader = Singleton<CommonRoot>.GetInstance().GetCorpsHeader();
    corpsQuestTopScene.mCorpsObserver = new Modified<PlayerCorps[]>(0L);
    IEnumerator e = corpsQuestTopScene.menu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int periodId, bool isAllClear)
  {
    CorpsQuestTopScene corpsQuestTopScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    Future<WebAPI.Response.QuestCorpsTop> f = WebAPI.QuestCorpsTop(periodId, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result == null)
    {
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      yield return (object) null;
    }
    corpsQuestTopScene.mCorpsData = ((IEnumerable<PlayerCorps>) f.Result.player_corps_list).First<PlayerCorps>();
    corpsQuestTopScene.mCorpsObserver.Commit();
    MasterData.CorpsPeriod.TryGetValue(periodId, out corpsQuestTopScene.masterPeriod);
    corpsQuestTopScene.masterSetting = corpsQuestTopScene.masterPeriod?.setting;
    if (corpsQuestTopScene.masterPeriod == null)
    {
      ModalWindow.Show("エラー", "軍団戦のデータがありません。", (Action) (() => { }));
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      yield return (object) null;
    }
    e1 = corpsQuestTopScene.mCorpsHeader.SetInfo(corpsQuestTopScene.masterPeriod.trade_coin_id);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    corpsQuestTopScene.mCorpsHeader.SetAreaName(corpsQuestTopScene.masterSetting.battlefield_name);
    e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), corpsQuestTopScene.sceneName), corpsQuestTopScene.sceneName);
    corpsQuestTopScene.playStory = Persist.eventStoryPlay.Data.PlayEventScript(corpsQuestTopScene.sceneName, 0);
    if (!corpsQuestTopScene.playStory)
    {
      e1 = corpsQuestTopScene.menu.InitializeAsync(corpsQuestTopScene.masterPeriod, corpsQuestTopScene.mCorpsData);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (corpsQuestTopScene.masterSetting != null && !string.IsNullOrEmpty(corpsQuestTopScene.masterSetting.bgm_file))
      {
        corpsQuestTopScene.bgmFile = corpsQuestTopScene.masterSetting.bgm_file;
        corpsQuestTopScene.bgmName = corpsQuestTopScene.masterSetting.bgm_name;
      }
    }
  }

  public IEnumerator onBackSceneAsync(int periodId, bool isAllClear)
  {
    CorpsQuestTopScene corpsQuestTopScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    if (corpsQuestTopScene.mCorpsObserver.IsChangedOnce())
    {
      corpsQuestTopScene.mCorpsData = ((IEnumerable<PlayerCorps>) corpsQuestTopScene.mCorpsObserver.Value).First<PlayerCorps>((Func<PlayerCorps, bool>) (x => x.period_id == periodId));
      corpsQuestTopScene.menu.ResetEntryUnits(corpsQuestTopScene.mCorpsData);
    }
    corpsQuestTopScene.mCorpsHeader.UpdateInfo();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), corpsQuestTopScene.sceneName), corpsQuestTopScene.sceneName);
    corpsQuestTopScene.playStory = Persist.eventStoryPlay.Data.PlayEventScript(corpsQuestTopScene.sceneName, 0);
    if (!corpsQuestTopScene.playStory && corpsQuestTopScene.masterSetting != null && !string.IsNullOrEmpty(corpsQuestTopScene.masterSetting.bgm_file))
    {
      corpsQuestTopScene.bgmFile = corpsQuestTopScene.masterSetting.bgm_file;
      corpsQuestTopScene.bgmName = corpsQuestTopScene.masterSetting.bgm_name;
    }
  }

  public void onStartScene(int periodId, bool isAllClear)
  {
    if (this.playStory)
      return;
    if (this.CheckNeedManual())
    {
      CorpsQuestManualScene.ChangeScene(this.masterSetting);
    }
    else
    {
      this.menu.OnStartScene(isAllClear);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
  }

  public void onBackScene(int periodId, bool isAllClear)
  {
    if (this.playStory)
      return;
    if (this.CheckNeedManual())
    {
      CorpsQuestManualScene.ChangeScene(this.masterSetting);
    }
    else
    {
      this.menu.OnStartScene(isAllClear);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
  }

  public override void onEndScene() => this.playStory = false;

  public override IEnumerator onDestroySceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    return base.onDestroySceneAsync();
  }

  private bool CheckNeedManual()
  {
    bool flag = false;
    try
    {
      if (!Persist.corpsSetting.Exists)
      {
        Persist.corpsSetting.Data.reset();
        Persist.corpsSetting.Flush();
      }
      if (Persist.corpsSetting.Data.isFirstTime)
      {
        flag = Persist.corpsSetting.Data.isFirstTime;
        Persist.corpsSetting.Data.isFirstTime = false;
        Persist.corpsSetting.Flush();
      }
    }
    catch (Exception ex)
    {
      Persist.corpsSetting.Delete();
      return false;
    }
    return flag;
  }
}
