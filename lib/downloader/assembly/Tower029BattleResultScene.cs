// Decompiled with JetBrains decompiler
// Type: Tower029BattleResultScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029BattleResultScene : NGSceneBase
{
  [SerializeField]
  private GameObject touchToNext;
  private List<ResultMenuBase> sequences;
  private bool isInitialized;
  private bool isStarted;
  private bool toNextSequence;
  private static DateTime serverTime;
  private ResultMenuBase nowPlayBase;
  [SerializeField]
  private GameObject bg;

  private IEnumerator SetBackground(int tower_id, int floor)
  {
    Tower029BattleResultScene battleResultScene = this;
    string path = string.Empty;
    TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).FirstOrDefault<TowerStage>((Func<TowerStage, bool>) (x => x.tower_id == tower_id && x.floor == floor));
    if (towerStage != null)
      path = towerStage.GetBackgroundPath();
    Future<GameObject> bgF = Res.Prefabs.BackGround.ResultBackGround.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleResultScene.bg = bgF.Result;
    Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
    e = bgSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
    {
      battleResultScene.bg.GetComponent<UI2DSprite>().sprite2D = bgSpriteF.Result;
      UI2DSprite component = battleResultScene.bg.GetComponent<UI2DSprite>();
      component.sprite2D = bgSpriteF.Result;
      Rect textureRect1 = bgSpriteF.Result.textureRect;
      ((UIWidget) component).width = Mathf.FloorToInt(((Rect) ref textureRect1).width);
      Rect textureRect2 = bgSpriteF.Result.textureRect;
      ((UIWidget) component).height = Mathf.FloorToInt(((Rect) ref textureRect2).height);
      battleResultScene.backgroundPrefab = battleResultScene.bg;
    }
  }

  private IEnumerator InitMenus(
    BattleInfo info,
    WebAPI.Response.TowerBattleFinish result,
    bool isShowDamage)
  {
    this.touchToNext.SetActive(false);
    foreach (ResultMenuBase seq in this.sequences)
    {
      if (Object.op_Inequality((Object) seq, (Object) null))
      {
        IEnumerator e = seq.Init(info, result);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (seq is Tower029ResultScore)
          ((Tower029ResultScore) seq).IsShowDamage = isShowDamage;
      }
    }
  }

  private IEnumerator RunMenus(BattleInfo info, WebAPI.Response.TowerBattleFinish result)
  {
    List<ResultMenuBase>.Enumerator seqe = this.sequences.GetEnumerator();
    IEnumerator e;
    while (seqe.MoveNext())
    {
      this.nowPlayBase = seqe.Current;
      if (!Object.op_Equality((Object) this.nowPlayBase, (Object) null))
      {
        this.touchToNext.SetActive(true);
        e = this.nowPlayBase.Run();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.toNextSequence = false;
        while (!this.toNextSequence)
          yield return (object) null;
        this.toNextSequence = false;
        e = this.nowPlayBase.OnFinish();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        break;
    }
    this.nowPlayBase = (ResultMenuBase) null;
    this.touchToNext.SetActive(true);
    ((Collider) this.touchToNext.GetComponent<BoxCollider>()).enabled = false;
    while (seqe.MoveNext())
    {
      e = seqe.Current.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Tower029BattleResultScene.serverTime = ServerTime.NowAppTime();
    Tower029TopScene.ChangeScene(true);
  }

  public static void ChangeScene(
    BattleInfo info,
    WebAPI.Response.TowerBattleFinish result,
    bool isShowDamage)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    instance.changeScene("tower029_battle_result_status", false, (object) info, (object) result, (object) isShowDamage);
  }

  public void IbtnTouchToNext()
  {
    this.toNextSequence = true;
    if (!Object.op_Inequality((Object) this.nowPlayBase, (Object) null))
      return;
    this.nowPlayBase.isSkip = true;
  }

  public override void onEndScene() => this.sequences.Clear();

  public IEnumerator onStartSceneAsync(
    BattleInfo info,
    WebAPI.Response.TowerBattleFinish result,
    bool isShowDamage)
  {
    Tower029BattleResultScene battleResultScene = this;
    if (!battleResultScene.isInitialized)
    {
      IEnumerator e = battleResultScene.SetBackground(result.tower_id, result.floor);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleResultScene.isInitialized = true;
      battleResultScene.sequences = new List<ResultMenuBase>()
      {
        (ResultMenuBase) ((Component) battleResultScene).GetComponent<Tower029ResultScore>(),
        (ResultMenuBase) ((Component) battleResultScene).GetComponent<BattleUI05RewardMenu>(),
        (ResultMenuBase) null
      };
      TowerTower towerTower;
      if (MasterData.TowerTower.TryGetValue(result.tower_id, out towerTower) && !towerTower.ranking_flag)
        battleResultScene.sequences.RemoveAt(0);
      if (result.stage_clear_rewards != null && result.stage_clear_rewards.Length != 0)
        battleResultScene.sequences.Add((ResultMenuBase) ((Component) battleResultScene).GetComponent<BattleUI05ClearBonusMenu>());
      if (((IEnumerable<TowerStage>) MasterData.TowerStageList).Count<TowerStage>((Func<TowerStage, bool>) (s => s.tower_id == result.tower_id)) == result.floor)
        battleResultScene.sequences.Add((ResultMenuBase) ((Component) battleResultScene).GetComponent<Tower029TowerClearAnim>());
      e = battleResultScene.InitMenus(info, result, isShowDamage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      foreach (TowerBgm towerBgm in ((IEnumerable<TowerBgm>) MasterData.TowerBgmList).Where<TowerBgm>((Func<TowerBgm, bool>) (x => x.period_id == result.period_id)).OrderByDescending<TowerBgm, int>((Func<TowerBgm, int>) (x => x.floor)).ToList<TowerBgm>())
      {
        if (result.floor >= towerBgm.floor)
        {
          battleResultScene.bgmFile = towerBgm.bgm_file;
          battleResultScene.bgmName = towerBgm.bgm_name;
          break;
        }
      }
    }
  }

  public void onStartScene(
    BattleInfo info,
    WebAPI.Response.TowerBattleFinish result,
    bool isShowDamage)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.StartCoroutine(this.RunMenus(info, result));
  }
}
