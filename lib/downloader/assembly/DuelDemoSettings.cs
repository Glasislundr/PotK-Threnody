// Decompiled with JetBrains decompiler
// Type: DuelDemoSettings
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
using UnitRegulation;
using UnityEngine;

#nullable disable
[Serializable]
public class DuelDemoSettings
{
  [SerializeField]
  [Tooltip("再生BGM")]
  private string bgmName;
  [SerializeField]
  private float bgmFadeDuration;
  [SerializeField]
  [Tooltip("地面モデル")]
  private int mapID;
  [SerializeField]
  [Tooltip("相手パラメータ")]
  private BattleStageEnemy mob;
  [SerializeField]
  [Tooltip("攻撃時ダメージ")]
  private DuelDemoSettings.Damages attackDamages = new DuelDemoSettings.Damages(999);
  [SerializeField]
  [Tooltip("防御時ダメージ")]
  private DuelDemoSettings.Damages defenseDamages = new DuelDemoSettings.Damages(0);

  public bool isSetupedLightmaps { get; private set; }

  public Future<DuelResult> makeResult(PlayerUnit target, PlayerUnitSkills koyuSkill)
  {
    return new Future<DuelResult>((Func<Promise<DuelResult>, IEnumerator>) (promis => this.doMakeResult(promis, target, koyuSkill)));
  }

  private IEnumerator doMakeResult(
    Promise<DuelResult> promis,
    PlayerUnit target,
    PlayerUnitSkills koyuSkill)
  {
    DuelDemoSettings duelDemoSettings = this;
    if (koyuSkill != null)
    {
      BattleLogicInitializer initializer = new BattleLogicInitializer();
      DuelResult ret = new DuelResult();
      PlayerUnit unitTarget = target.Clone();
      Future<BL.Unit> unitCreater = initializer.createUnitByPlayerUnit(unitTarget, 0, false, 0, (Checker) null);
      yield return (object) unitCreater.Wait();
      unitCreater.Result.hp = unitCreater.Result.parameter.Hp;
      BL.Skill ds = new BL.Skill();
      ds.id = koyuSkill.skill_id;
      ds.level = koyuSkill.level;
      ds.useTurn = 1;
      ds.remain = new int?(10);
      Future<BL.Unit> mobCreater = initializer.createUnitByPlayerUnit(PlayerUnit.FromEnemy(duelDemoSettings.mob), 0, false, 0, (Checker) null);
      yield return (object) mobCreater.Wait();
      mobCreater.Result.hp = mobCreater.Result.parameter.Hp;
      BattleskillGenre? genre1 = ds.genre1;
      BattleskillGenre battleskillGenre = BattleskillGenre.attack;
      bool isAttack = genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
      ret.isPlayerAttack = isAttack;
      BattleLandform battleLandform = MasterData.BattleLandform.First<KeyValuePair<int, BattleLandform>>().Value;
      AttackStatus attackStatus = new AttackStatus();
      attackStatus.duelParameter = Judgement.BeforeDuelParameter.CreateSingle(isAttack ? (BL.ISkillEffectListUnit) unitCreater.Result : (BL.ISkillEffectListUnit) mobCreater.Result, (BL.MagicBullet) null, battleLandform, new BL.Unit[0], !isAttack ? (BL.ISkillEffectListUnit) unitCreater.Result : (BL.ISkillEffectListUnit) mobCreater.Result, (BL.MagicBullet) null, battleLandform, new BL.Unit[0], isAttack);
      attackStatus.elementAttackRate = 1f;
      if (isAttack)
      {
        ret.moveUnit = ret.attack = unitCreater.Result;
        ret.defense = mobCreater.Result;
      }
      else
      {
        ret.moveUnit = ret.attack = mobCreater.Result;
        ret.defense = unitCreater.Result;
      }
      BL.DuelTurn duelTurn = new BL.DuelTurn();
      duelTurn.ailmentEffects = new BattleskillAilmentEffect[0];
      duelTurn.attackerAilmentEffects = new BattleskillAilmentEffect[0];
      duelTurn.invokeDuelSkills = new BL.Skill[0];
      duelTurn.isAtackker = true;
      duelTurn.attackStatus = attackStatus;
      int real;
      int disp;
      if (isAttack)
      {
        duelTurn.invokeDuelSkills = new BL.Skill[1]{ ds };
        duelTurn.invokeDefenderDuelSkills = new BL.Skill[0];
        real = duelDemoSettings.attackDamages.real;
        disp = duelDemoSettings.attackDamages.disp;
        duelTurn.attackerRestHp = unitCreater.Result.hp;
        duelTurn.defenderRestHp = 0;
      }
      else
      {
        duelTurn.invokeDuelSkills = new BL.Skill[0];
        duelTurn.invokeDefenderDuelSkills = new BL.Skill[1]
        {
          ds
        };
        real = duelDemoSettings.defenseDamages.real;
        disp = duelDemoSettings.defenseDamages.disp;
        duelTurn.attackerRestHp = mobCreater.Result.hp;
        duelTurn.defenderRestHp = unitCreater.Result.hp;
      }
      duelTurn.damage = real;
      duelTurn.realDamage = real;
      duelTurn.dispDamage = disp;
      duelTurn.isHit = true;
      if (duelTurn.invokeDuelSkills.Length != 0)
      {
        BattleskillEffect[] effects = duelTurn.invokeDuelSkills[0].skill.Effects;
        BattleskillEffect battleskillEffect = Array.Find<BattleskillEffect>(effects, (Predicate<BattleskillEffect>) (x => x.HasKey(BattleskillEffectLogicArgumentEnum.attack_count)));
        if (battleskillEffect != null)
        {
          List<BL.SuiseiResult> suiseiResultList = new List<BL.SuiseiResult>();
          int num = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_count);
          for (int index = 0; index < num; ++index)
            suiseiResultList.Add(new BL.SuiseiResult()
            {
              isHit = true,
              isCritical = false,
              damage = real,
              realDamage = real,
              dispDamage = disp,
              invokeDuelSkills = new BL.Skill[0],
              invokeDefenderDuelSkills = new BL.Skill[0]
            });
          duelTurn.suiseiResults = suiseiResultList;
        }
        // ISSUE: reference to a compiler-generated method
        if (((IEnumerable<BattleskillEffect>) effects).Any<BattleskillEffect>(new Func<BattleskillEffect, bool>(duelDemoSettings.\u003CdoMakeResult\u003Eb__12_1)))
        {
          duelTurn.drainDamage = real;
          duelTurn.dispDrainDamage = disp;
        }
      }
      duelTurn.attackCount = 1;
      ret.turns = new BL.DuelTurn[1]{ duelTurn };
      ret.distance = isAttack ? duelDemoSettings.getDistance(unitTarget.equippedWeaponGearOrInitial.kind.Enum) : 1;
      promis.Result = ret;
    }
  }

  private int getDistance(GearKindEnum kind)
  {
    switch (kind)
    {
      case GearKindEnum.bow:
      case GearKindEnum.gun:
      case GearKindEnum.staff:
        return 2;
      default:
        return 1;
    }
  }

  private bool isDrainEffect(BattleskillEffect effect)
  {
    BattleskillEffectLogicArgumentEnum[] logicArgumentEnumArray = new BattleskillEffectLogicArgumentEnum[5]
    {
      BattleskillEffectLogicArgumentEnum.percentage_drain,
      BattleskillEffectLogicArgumentEnum.invoke_drain,
      BattleskillEffectLogicArgumentEnum.drain,
      BattleskillEffectLogicArgumentEnum.drain_ratio,
      BattleskillEffectLogicArgumentEnum.percentage_drain_ratio
    };
    foreach (BattleskillEffectLogicArgumentEnum key in logicArgumentEnumArray)
    {
      if (effect.HasKey(key))
        return true;
    }
    return false;
  }

  private BattleStage battleStage
  {
    get
    {
      return Array.Find<BattleStage>(MasterData.BattleStageList, (Predicate<BattleStage>) (x => x.map_BattleMap == this.mapID));
    }
  }

  public IEnumerator preSetupLightmaps()
  {
    if (!this.isSetupedLightmaps)
    {
      BattleStage battleStage = this.battleStage;
      LightmapSettings.lightmapsMode = (LightmapsMode) 0;
      Future<Texture2D> duelFarF = battleStage.map.LoadDuelFarLightmap();
      yield return (object) duelFarF.Wait();
      LightmapSettings.lightmaps = new LightmapData[1]
      {
        new LightmapData() { lightmapColor = duelFarF.Result }
      };
      this.isSetupedLightmaps = true;
    }
  }

  public void clearLightmaps()
  {
    if (!this.isSetupedLightmaps)
      return;
    LightmapSettings.lightmaps = (LightmapData[]) null;
    this.isSetupedLightmaps = false;
  }

  public DuelEnvironment makeEnvironment()
  {
    return new DuelEnvironment()
    {
      stage = new BL.Stage(this.battleStage.ID),
      isDemoMode = true
    };
  }

  public void playBGM()
  {
    NGSoundManager instance;
    if (string.IsNullOrEmpty(this.bgmName) || Object.op_Equality((Object) (instance = Singleton<NGSoundManager>.GetInstance()), (Object) null))
      return;
    instance.playBGMWithCrossFade(this.bgmName, this.bgmFadeDuration);
  }

  [Serializable]
  private class Damages
  {
    [Tooltip("実ダメージ")]
    public int real;
    [Tooltip("表示ダメージ")]
    public int disp;

    public Damages(int v)
    {
      this.real = v;
      this.disp = v;
    }
  }
}
