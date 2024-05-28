// Decompiled with JetBrains decompiler
// Type: Battle0181Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle0181Menu : BackButtonMenuBase
{
  [SerializeField]
  protected Battle0181CharacterStatus player;
  [SerializeField]
  protected Battle0181CharacterStatus enemy;
  [SerializeField]
  private GameObject colosseumSkillOwnRoot;
  [SerializeField]
  private GameObject colosseumSkillEnemyRoot;
  private BL.UnitPosition attacker;
  private BL.UnitPosition defender;
  private GameObject mColosseumSkillActivity_Prefab;
  private GameObject mDirColosseumSkillOwn_Prefab;
  private GameObject mDirColosseumSkillEnemy_Prefab;
  private GameObject mUpEffectPrefab;
  private GameObject mDownEffectPrefab;
  private const float SKILLDISP_OFFSET_Y = -52f;

  public IEnumerator Init(
    BL.UnitPosition attack,
    AttackStatus attackStatus,
    int attackFirstAttack,
    BL.UnitPosition defense,
    AttackStatus defenseStatus,
    int defenseFirstAttack,
    bool isColosseum,
    bool isDemoMode)
  {
    this.attacker = attack;
    this.defender = defense;
    yield return (object) this.player.Init(attack, attackStatus, attackFirstAttack, isColosseum, isDemoMode);
    yield return (object) this.enemy.Init(defense, defenseStatus, defenseFirstAttack, isColosseum, isDemoMode);
  }

  public void ChangeStatus(
    AttackStatus attackStatus,
    int attackFirstAttack,
    AttackStatus defenseStatus,
    int defenseFirstAttack)
  {
    this.player.ChangeStatus(this.attacker, attackStatus, attackFirstAttack);
    this.enemy.ChangeStatus(this.defender, defenseStatus, defenseFirstAttack);
  }

  public IEnumerator onStartSceneAsync(bool isColosseum)
  {
    if (isColosseum)
    {
      Future<Tuple<GameObject, GameObject, GameObject, GameObject, GameObject>> f = Future.WhenAll<GameObject, GameObject, GameObject, GameObject, GameObject>(Res.Prefabs.colosseum.colosseum_skill_activity.Load<GameObject>(), Res.Prefabs.colosseum.dir_ColosseumSkill_Own.Load<GameObject>(), Res.Prefabs.colosseum.dir_ColosseumSkill_Enemy.Load<GameObject>(), Res.Prefabs.colosseum.colosseum_Number_up.Load<GameObject>(), Res.Prefabs.colosseum.colosseum_Number_down.Load<GameObject>());
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mColosseumSkillActivity_Prefab = f.Result.Item1;
      this.mDirColosseumSkillOwn_Prefab = f.Result.Item2;
      this.mDirColosseumSkillEnemy_Prefab = f.Result.Item3;
      this.mUpEffectPrefab = f.Result.Item4;
      this.mDownEffectPrefab = f.Result.Item5;
      f = (Future<Tuple<GameObject, GameObject, GameObject, GameObject, GameObject>>) null;
    }
  }

  public virtual void OnDuelWin(float dispTime)
  {
  }

  public virtual void OnDuelLose(float dispTime)
  {
  }

  public virtual void OnUnitStartAttack(int attackerIndex, List<BL.DuelTurn> turns)
  {
  }

  public void backToBattle() => this.backScene();

  public BL.UnitPosition Attacker => this.attacker;

  public BL.UnitPosition Defender => this.defender;

  public IEnumerator createColosseumDuelInvokeSkillDisp(
    BL.Unit playerUnit,
    BL.Unit enemyUnit,
    AttackStatus playerStatus,
    AttackStatus newPlayerStatus,
    int playerFirstAttack,
    AttackStatus enemyStatus,
    AttackStatus newEnemyStatus,
    int enemyFirstAttack)
  {
    List<NGTweenParts> tweenPartsList = new List<NGTweenParts>();
    List<ParticleSystem> effectList = new List<ParticleSystem>();
    List<Future<Sprite>> futures = new List<Future<Sprite>>();
    if (Object.op_Inequality((Object) this.mDirColosseumSkillOwn_Prefab, (Object) null))
    {
      List<BL.Skill> skillList = new List<BL.Skill>();
      if (playerUnit.hasOugi)
        skillList.Add(playerUnit.ougi);
      foreach (BL.Skill skill in playerUnit.skills)
        skillList.Add(skill);
      for (int index = 0; index < skillList.Count; ++index)
      {
        BL.Skill skill = skillList[index];
        if (skill != null)
        {
          GameObject gameObject = this.mDirColosseumSkillOwn_Prefab.Clone(new GameObject("own_root" + (object) index)
          {
            transform = {
              parent = this.colosseumSkillOwnRoot.transform,
              localRotation = Quaternion.identity,
              localScale = Vector3.one,
              localPosition = new Vector3(0.0f, -52f * (float) index, 0.0f)
            }
          }.transform);
          DuelColosseumSkillDisp component1 = gameObject.GetComponent<DuelColosseumSkillDisp>();
          NGTweenParts component2 = gameObject.GetComponent<NGTweenParts>();
          component2.resetActive(false);
          tweenPartsList.Add(component2);
          futures.Add(component1.Init(skill));
          effectList.Add(this.mColosseumSkillActivity_Prefab.CloneAndGetComponent<ParticleSystem>(gameObject.transform));
        }
      }
    }
    if (Object.op_Inequality((Object) this.mDirColosseumSkillEnemy_Prefab, (Object) null))
    {
      List<BL.Skill> skillList = new List<BL.Skill>();
      if (enemyUnit.hasOugi)
        skillList.Add(enemyUnit.ougi);
      foreach (BL.Skill skill in enemyUnit.skills)
        skillList.Add(skill);
      for (int index = 0; index < skillList.Count; ++index)
      {
        BL.Skill skill = skillList[index];
        if (skill != null)
        {
          GameObject gameObject = this.mDirColosseumSkillEnemy_Prefab.Clone(new GameObject("enemy_root" + (object) index)
          {
            transform = {
              parent = this.colosseumSkillEnemyRoot.transform,
              localRotation = Quaternion.identity,
              localScale = Vector3.one,
              localPosition = new Vector3(0.0f, -52f * (float) index, 0.0f)
            }
          }.transform);
          DuelColosseumSkillDisp component3 = gameObject.GetComponent<DuelColosseumSkillDisp>();
          NGTweenParts component4 = gameObject.GetComponent<NGTweenParts>();
          component4.resetActive(false);
          tweenPartsList.Add(component4);
          futures.Add(component3.Init(skill));
          effectList.Add(this.mColosseumSkillActivity_Prefab.CloneAndGetComponent<ParticleSystem>(gameObject.transform));
        }
      }
    }
    if (tweenPartsList.Count > 0)
    {
      if (futures.Count > 0)
      {
        IEnumerator e = futures.Sequence<Sprite>().Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.player.colosseumParameterChangeEffect(playerStatus, newPlayerStatus, playerFirstAttack, this.mUpEffectPrefab, this.mDownEffectPrefab);
      this.enemy.colosseumParameterChangeEffect(enemyStatus, newEnemyStatus, enemyFirstAttack, this.mUpEffectPrefab, this.mDownEffectPrefab);
      yield return (object) new WaitForSeconds(0.5f);
      foreach (NGTweenParts ngTweenParts in tweenPartsList)
        ngTweenParts.isActive = true;
      yield return (object) new WaitForSeconds(0.5f);
      foreach (ParticleSystem particleSystem in effectList)
        particleSystem.Play(true);
      yield return (object) new WaitForSeconds(0.5f);
      this.player.startColosseumParameterChangeEffect(playerStatus, newPlayerStatus);
      this.enemy.startColosseumParameterChangeEffect(enemyStatus, newEnemyStatus);
      yield return (object) new WaitForSeconds(2f);
      foreach (NGTweenParts ngTweenParts in tweenPartsList)
        ngTweenParts.isActive = false;
    }
  }

  public override void onBackButton() => this.showBackKeyToast();
}
