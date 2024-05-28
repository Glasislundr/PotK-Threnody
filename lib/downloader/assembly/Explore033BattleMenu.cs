// Decompiled with JetBrains decompiler
// Type: Explore033BattleMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Explore033BattleMenu : Battle0181Menu
{
  [SerializeField]
  private GameObject mScreenEffectAnchor;
  [SerializeField]
  private GameObject mTransitionAnchor;
  [SerializeField]
  private ExploreFooter mExploreFooter;
  private Animator mTransitionAnime;
  private GameObject mDuelEndEff;
  private GameObject mDuelEndEffPrefab;
  private string mEnemyName;

  public IEnumerator InitAsync()
  {
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/dir_transition");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mTransitionAnime = loader.Result.CloneAndGetComponent<Animator>(this.mTransitionAnchor.transform);
  }

  public IEnumerator onStartSceneAsync(DuelResult duelResult)
  {
    Explore033BattleMenu explore033BattleMenu = this;
    BL.UnitPosition playerUnitPos = new BL.UnitPosition();
    BL.UnitPosition enemyUnitPos = new BL.UnitPosition();
    playerUnitPos.unit = duelResult.playerUnit();
    enemyUnitPos.unit = duelResult.enemyUnit();
    explore033BattleMenu.mEnemyName = enemyUnitPos.unit.unit.name;
    yield return (object) explore033BattleMenu.loadDuelEndEffect(duelResult.isDieDefense);
    yield return (object) explore033BattleMenu.Init(playerUnitPos, duelResult.playerAttackStatus(), 0, enemyUnitPos, duelResult.enemyAttackStatus(), 0, false, false);
    yield return (object) explore033BattleMenu.onStartSceneAsync(false);
    (explore033BattleMenu.enemy as Explore033StatusEnemy).SetWeakPoint(Singleton<ExploreDataManager>.GetInstance().CurrentWeakPoint);
    yield return (object) explore033BattleMenu.mExploreFooter.UpdateDeckUnitIconsAsync();
    yield return (object) explore033BattleMenu.mExploreFooter.Initialize();
  }

  public void onStartScene() => this.mTransitionAnime.Play("explore_transition_out_anim");

  public void onEndScene()
  {
    if (!Object.op_Inequality((Object) this.mDuelEndEff, (Object) null))
      return;
    Object.Destroy((Object) this.mDuelEndEff);
  }

  private IEnumerator loadDuelEndEffect(bool isWin)
  {
    Future<GameObject> loader;
    IEnumerator e;
    if (isWin)
    {
      loader = new ResourceObject("Prefabs/colosseum/colosseum023-4-4/023-4-4_win").Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mDuelEndEffPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    else
    {
      loader = new ResourceObject("Prefabs/colosseum/colosseum023-4-4/023-4-4_lose").Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mDuelEndEffPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public override void OnDuelWin(float dispTime)
  {
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.mDuelEndEff = this.mDuelEndEffPrefab.Clone(this.mScreenEffectAnchor.transform);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_2408");
    this.StartCoroutine(this.transitionIn(dispTime));
  }

  public override void OnDuelLose(float dispTime)
  {
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.mDuelEndEff = this.mDuelEndEffPrefab.Clone(this.mScreenEffectAnchor.transform);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_2409");
    this.StartCoroutine(this.transitionIn(dispTime));
  }

  public override void OnUnitStartAttack(int attackerIndex, List<BL.DuelTurn> turns)
  {
    if (attackerIndex % 2 == 0)
      Singleton<ExploreDataManager>.GetInstance().AddLog(string.Format("{0}に[fa0000]{1}[-]ダメージを与えた", (object) this.mEnemyName, (object) turns[0].dispDamage), Color.white);
    else
      Singleton<ExploreDataManager>.GetInstance().AddLog(string.Format("{0}から[fa0000]{1}[-]ダメージを受けた", (object) this.mEnemyName, (object) turns[0].dispDamage), Color.white);
  }

  private IEnumerator transitionIn(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this.mTransitionAnime.Play("explore_transition_in_anim");
  }
}
