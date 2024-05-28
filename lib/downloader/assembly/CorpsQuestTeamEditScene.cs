// Decompiled with JetBrains decompiler
// Type: CorpsQuestTeamEditScene
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
[AddComponentMenu("Scenes/CorpsQuest/TeamEditScene")]
public class CorpsQuestTeamEditScene : NGSceneBase
{
  private CorpsQuestTeamEditMenu menu;
  private Modified<PlayerUnit[]> mPlayerUnitObserver;

  public static void ChangeScene(int periodId)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_team_edit", true, (object) periodId);
  }

  public override IEnumerator onInitSceneAsync()
  {
    CorpsQuestTeamEditScene questTeamEditScene = this;
    questTeamEditScene.menu = questTeamEditScene.menuBase as CorpsQuestTeamEditMenu;
    IEnumerator e = questTeamEditScene.menu.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    questTeamEditScene.mPlayerUnitObserver = new Modified<PlayerUnit[]>(0L);
    questTeamEditScene.mPlayerUnitObserver.Commit();
  }

  public IEnumerator onStartSceneAsync(int periodId)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e = this.Initialize(periodId);
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
    e1 = this.Initialize(periodId);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  public IEnumerator Initialize(int periodId)
  {
    CorpsQuestTeamEditScene questTeamEditScene = this;
    PlayerCorps corps = ((IEnumerable<PlayerCorps>) SMManager.Get<PlayerCorps[]>()).First<PlayerCorps>((Func<PlayerCorps, bool>) (x => x.period_id == periodId));
    IEnumerator e = questTeamEditScene.menu.InitializeAsync(corps);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    CorpsSetting corpsSetting;
    if (MasterData.CorpsSetting.TryGetValue(corps.corps_id, out corpsSetting) && !string.IsNullOrEmpty(corpsSetting.bgm_file))
    {
      questTeamEditScene.bgmFile = corpsSetting.bgm_file;
      questTeamEditScene.bgmName = corpsSetting.bgm_name;
    }
  }

  public void onStartScene(int periodId) => Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
}
