// Decompiled with JetBrains decompiler
// Type: Explore033ChallengeScene
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
public class Explore033ChallengeScene : NGSceneBase
{
  private const int START_DUEL_COUNT = 0;
  private int duelCount;
  [SerializeField]
  private Explore033ChallengeMenu menu;
  private GameCore.ColosseumResult battle_result;
  private ColosseumUtility.Info info;
  private Gladiator gladiator;
  public TextAsset test_assets;

  public static void changeScene(ChallengeNpc gladiator, GameCore.ColosseumResult battle_result)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_Challenge", false, (object) gladiator, (object) battle_result);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Explore033ChallengeScene explore033ChallengeScene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/ExploreChallengeBackground").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033ChallengeScene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(ChallengeNpc gladiator, GameCore.ColosseumResult battle_result)
  {
    this.gladiator = new Gladiator();
    this.gladiator.name = gladiator.name;
    this.gladiator.player_level = gladiator.player_level;
    this.gladiator.total_power = gladiator.total_power;
    this.gladiator.player_id = gladiator.player_id;
    this.gladiator.leader_unit_id = gladiator.leader_unit_id;
    this.gladiator.leader_unit_level = gladiator.leader_unit_level;
    this.battle_result = battle_result;
    IEnumerator e = this.onStartSceneAsync(battle_result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GameCore.ColosseumResult result)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource(((IEnumerable<DuelColosseumResult>) this.battle_result.duelResult).Where<DuelColosseumResult>((Func<DuelColosseumResult, bool>) (x => x.opponent != (BL.Unit) null)).SelectMany<DuelColosseumResult, string>((Func<DuelColosseumResult, IEnumerable<string>>) (x => (IEnumerable<string>) rm.PathsFromUnit(x.opponent.unit))), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.Initialize(result, this.gladiator, this.duelCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) Singleton<NGDuelDataManager>.GetInstance().PreloadCommonDuelEffect();
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.duelCount == 0)
      Singleton<CommonRoot>.GetInstance().WhiteFadeOut();
    this.menu.StartToBeginning(this.duelCount);
    ++this.duelCount;
  }

  public void onStartScene(ChallengeNpc gladiator, GameCore.ColosseumResult battle_result)
  {
    this.onStartScene();
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public void Reinitialize()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.menu.StartToBeginning(this.duelCount);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    ++this.duelCount;
  }

  public void ReplayScene()
  {
    this.duelCount = 0;
    this.onStartScene();
  }
}
