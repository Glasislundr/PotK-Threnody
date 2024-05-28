// Decompiled with JetBrains decompiler
// Type: BattleUI05ClearBonusMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI05ClearBonusMenu : ResultMenuBase
{
  private List<BattleResultBonusInfo> RewardList;
  private GameObject bonus;
  private GameObject ClearBonusPrefab;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    BattleUI05ClearBonusMenu ui05ClearBonusMenu = this;
    ui05ClearBonusMenu.RewardList = new List<BattleResultBonusInfo>();
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<int>) ((IEnumerable<BattleEndStage_clear_rewards>) result.stage_clear_rewards).Select<BattleEndStage_clear_rewards, int>((Func<BattleEndStage_clear_rewards, int>) (x => x.id)).ToArray<int>()).ForEach<int>(new Action<int>(ui05ClearBonusMenu.\u003CInit\u003Eb__3_1));
    IEnumerator e = ui05ClearBonusMenu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator Init(BattleInfo info, WebAPI.Response.TowerBattleFinish result)
  {
    BattleUI05ClearBonusMenu ui05ClearBonusMenu = this;
    ui05ClearBonusMenu.RewardList = new List<BattleResultBonusInfo>();
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<int>) ((IEnumerable<WebAPI.Response.TowerBattleFinishStage_clear_rewards>) result.stage_clear_rewards).Select<WebAPI.Response.TowerBattleFinishStage_clear_rewards, int>((Func<WebAPI.Response.TowerBattleFinishStage_clear_rewards, int>) (x => x.id)).ToArray<int>()).ForEach<int>(new Action<int>(ui05ClearBonusMenu.\u003CInit\u003Eb__4_1));
    IEnumerator e = ui05ClearBonusMenu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator Init(BattleInfo info, WebAPI.Response.QuestCorpsBattleFinish result)
  {
    BattleUI05ClearBonusMenu ui05ClearBonusMenu = this;
    ui05ClearBonusMenu.RewardList = new List<BattleResultBonusInfo>();
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<int>) ((IEnumerable<WebAPI.Response.QuestCorpsBattleFinishStage_clear_rewards>) result.stage_clear_rewards).Select<WebAPI.Response.QuestCorpsBattleFinishStage_clear_rewards, int>((Func<WebAPI.Response.QuestCorpsBattleFinishStage_clear_rewards, int>) (x => x.id)).ToArray<int>()).ForEach<int>(new Action<int>(ui05ClearBonusMenu.\u003CInit\u003Eb__5_1));
    IEnumerator e = ui05ClearBonusMenu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadResources()
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.ClearBonusPrefab, (Object) null))
    {
      Future<GameObject> ClearBonusPrefabF = Res.Prefabs.battle.Quest_ClearBonus.Load<GameObject>();
      e = ClearBonusPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ClearBonusPrefab = ClearBonusPrefabF.Result;
      ClearBonusPrefabF = (Future<GameObject>) null;
    }
    this.bonus = this.ClearBonusPrefab.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    this.bonus.SetActive(false);
    e = this.bonus.GetComponent<BattleUI05ClearBonusSetting>().CreateClearBonusIcon(this.RewardList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator Run()
  {
    BattleUI05ClearBonusMenu ui05ClearBonusMenu = this;
    ui05ClearBonusMenu.bonus.SetParent(((Component) ui05ClearBonusMenu).gameObject);
    ui05ClearBonusMenu.bonus.SetActive(true);
    ui05ClearBonusMenu.bonus.GetComponent<BattleUI05ClearBonusSetting>().SetClearBonusInfo(ui05ClearBonusMenu.RewardList);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1034");
    IEnumerator e = ui05ClearBonusMenu.SetColorBackground();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bool toNext = false;
    GameObject touchObj = ui05ClearBonusMenu.CreateTouchObject((EventDelegate.Callback) (() => this.isSkip = toNext = true));
    while (!toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        toNext = true;
      }
      yield return (object) null;
    }
    Object.DestroyObject((Object) ui05ClearBonusMenu.bonus);
    Object.DestroyObject((Object) touchObj);
  }

  private IEnumerator CreateBonusObject()
  {
    BattleUI05ClearBonusMenu ui05ClearBonusMenu = this;
    Future<GameObject> loader = Res.Prefabs.battle.Quest_ClearBonus.Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui05ClearBonusMenu.bonus = loader.Result.Clone(((Component) ui05ClearBonusMenu).gameObject.transform);
  }

  private IEnumerator SetColorBackground()
  {
    BattleUI05ClearBonusMenu ui05ClearBonusMenu = this;
    Future<Sprite> textureLoader = Res.Prefabs.BackGround.black.Load<Sprite>();
    IEnumerator e = textureLoader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Resolution currentResolution = Screen.currentResolution;
    GameObject gameObject = new GameObject("Color Layer");
    gameObject.transform.parent = ui05ClearBonusMenu.bonus.transform;
    gameObject.layer = ((Component) ui05ClearBonusMenu).gameObject.layer;
    UIPanel uiPanel = gameObject.AddComponent<UIPanel>();
    UI2DSprite ui2Dsprite = gameObject.AddComponent<UI2DSprite>();
    uiPanel.depth = 20;
    ui2Dsprite.sprite2D = textureLoader.Result;
    ((UIRect) ui2Dsprite).alpha = 0.75f;
    ((UIWidget) ui2Dsprite).height = ((Resolution) ref currentResolution).height;
    ((UIWidget) ui2Dsprite).width = ((Resolution) ref currentResolution).width;
  }
}
