// Decompiled with JetBrains decompiler
// Type: CorpsQuestUnitListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CorpsQuestUnitListScene : NGSceneBase
{
  private CorpsQuestUnitListMenu menu;
  private CorpsSetting setting;
  private Modified<PlayerUnit[]> mPlayerUnitObserver;

  public static void ChangeScene(int periodId)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_unit_list", true, (object) periodId);
  }

  public override IEnumerator onInitSceneAsync()
  {
    CorpsQuestUnitListScene questUnitListScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    questUnitListScene.menu = ((Component) questUnitListScene.menuBase).GetComponent<CorpsQuestUnitListMenu>();
    IEnumerator e = questUnitListScene.menu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    questUnitListScene.mPlayerUnitObserver = new Modified<PlayerUnit[]>(0L);
    questUnitListScene.mPlayerUnitObserver.Commit();
  }

  public IEnumerator onStartSceneAsync(int periodId)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e = this.InitializeMenu(periodId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onBackSceneAsync(int periodId)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e1;
    if (this.mPlayerUnitObserver.IsChangedOnce())
    {
      Future<WebAPI.Response.QuestCorpsTop> f = WebAPI.QuestCorpsTop(periodId, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
        yield return (object) null;
      }
      f = (Future<WebAPI.Response.QuestCorpsTop>) null;
    }
    e1 = this.InitializeMenu(periodId);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private IEnumerator InitializeMenu(int periodId)
  {
    CorpsQuestUnitListScene questUnitListScene = this;
    PlayerCorps playerCorps = ((IEnumerable<PlayerCorps>) SMManager.Get<PlayerCorps[]>()).First<PlayerCorps>((Func<PlayerCorps, bool>) (x => x.period_id == periodId));
    MasterData.CorpsSetting.TryGetValue(playerCorps.corps_id, out questUnitListScene.setting);
    IEnumerator e = questUnitListScene.menu.InitializeAsync(playerCorps.entry_player_unit_ids, playerCorps.used_player_unit_ids);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (questUnitListScene.setting != null && !string.IsNullOrEmpty(questUnitListScene.setting.bgm_file))
    {
      questUnitListScene.bgmFile = questUnitListScene.setting.bgm_file;
      questUnitListScene.bgmName = questUnitListScene.setting.bgm_name;
    }
  }

  public void onStartScene(int periodId) => Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
}
