// Decompiled with JetBrains decompiler
// Type: BE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[Serializable]
public class BE
{
  [NonSerialized]
  private BE.DefaultDict<BL.DropData, BE.DropDataResource> dropDataResource_;
  public BL core;
  [NonSerialized]
  private BE.DefaultDict<BL.Panel, BE.PanelResource> panelResource_;
  [NonSerialized]
  private BE.DefaultDict<BL.Stage, BE.StageResource> stageResource_;
  [NonSerialized]
  private BE.DefaultDict<int, BE.ItemResource> itemResource_;
  [NonSerialized]
  private BE.DefaultDict<int, BE.SkillResource> skillResource_;
  [NonSerialized]
  private BE.DefaultDict<int, BE.AilmentSkillResource> ailmentSkillResource_;
  [NonSerialized]
  private BE.DefaultDict<BL.Unit, BE.UnitResource> unitResource_;
  public Stack<List<BL.Unit>> waveEnemiesStack = new Stack<List<BL.Unit>>();
  public Stack<List<Tuple<BL.DropData, int>>> waveDropStack = new Stack<List<Tuple<BL.DropData, int>>>();
  [NonSerialized]
  private BE.DefaultDict<BL.Weapon, BE.WeaponResource> weaponResource_;

  public BE.DefaultDict<BL.DropData, BE.DropDataResource> dropDataResource
  {
    get => this.getDefaultDict<BL.DropData, BE.DropDataResource>(ref this.dropDataResource_);
  }

  public BE.DefaultDict<TKey, TValue> getDefaultDict<TKey, TValue>(
    ref BE.DefaultDict<TKey, TValue> v)
    where TValue : new()
  {
    if (v == null)
      v = new BE.DefaultDict<TKey, TValue>();
    return v;
  }

  public BE.DefaultDict<BL.Panel, BE.PanelResource> panelResource
  {
    get => this.getDefaultDict<BL.Panel, BE.PanelResource>(ref this.panelResource_);
  }

  public BE.DefaultDict<BL.Stage, BE.StageResource> stageResource
  {
    get => this.getDefaultDict<BL.Stage, BE.StageResource>(ref this.stageResource_);
  }

  public Vector3 limitFieldPosition(Vector3 v)
  {
    BE.PanelResource panelResource1 = this.panelResource[this.core.getFieldPanel(0, 0)];
    BE.PanelResource panelResource2 = this.panelResource[this.core.getFieldPanel(this.core.getFieldHeight() - 1, 0)];
    BE.PanelResource panelResource3 = this.panelResource[this.core.getFieldPanel(0, this.core.getFieldWidth() - 1)];
    BE.PanelResource panelResource4 = (BE.PanelResource) null;
    BE.PanelResource panelResource5 = (BE.PanelResource) null;
    if ((double) v.x < (double) panelResource1.gameObject.transform.position.x)
      panelResource4 = panelResource1;
    else if ((double) v.x > (double) panelResource3.gameObject.transform.position.x)
      panelResource4 = panelResource3;
    if ((double) v.z < (double) panelResource1.gameObject.transform.position.z)
      panelResource5 = panelResource1;
    else if ((double) v.z > (double) panelResource2.gameObject.transform.position.z)
      panelResource5 = panelResource2;
    double num1 = panelResource4 == null ? (double) v.x : (double) panelResource4.gameObject.transform.position.x;
    float y = panelResource1.gameObject.transform.position.y;
    float num2 = panelResource5 == null ? v.z : panelResource5.gameObject.transform.position.z;
    double num3 = (double) y;
    double num4 = (double) num2;
    return new Vector3((float) num1, (float) num3, (float) num4);
  }

  public BE.DefaultDict<int, BE.ItemResource> itemResource
  {
    get => this.getDefaultDict<int, BE.ItemResource>(ref this.itemResource_);
  }

  public void useItem(BL.Item item, BL.Unit unit, BattleTimeManager btm)
  {
    btm.setScheduleAction((Action) (() =>
    {
      int prev_hp = unit.hp;
      bool isRebirth = item.item.skill.target_type == BattleskillTargetType.dead_player_single && unit.isDead;
      this.core.useItemWith(item, unit, (Action<List<BL.Unit>>) (effectTargets =>
      {
        Debug.LogWarning((object) (" ==== useItem#effectTargets:" + (object) effectTargets + " isRebirth:" + isRebirth.ToString()));
        Singleton<NGBattleManager>.GetInstance().battleEffects.itemFieldEffectStart(effectTargets, item, (Action) (() =>
        {
          if (isRebirth)
          {
            unit.rebirthBE(this);
          }
          else
          {
            if (prev_hp == unit.hp)
              return;
            if (unit.hp <= 0)
              unit.deadByItem = true;
            if (!unit.isView)
              return;
            this.unitResource[unit].unitParts_.dispHpNumber(prev_hp, unit.hp);
          }
        }));
      }), this.core);
    }));
  }

  public void useMagicBullet(
    BL.MagicBullet mb,
    int attack,
    BL.Unit unit,
    List<BL.Unit> targets,
    BattleTimeManager btm)
  {
    btm.setScheduleAction((Action) (() =>
    {
      this.core.setSomeAction();
      this.core.useMagicBulletWith(mb, attack, unit, targets, (Action<Dictionary<BL.Unit, int>, List<BL.Unit>, List<BL.Unit>>) ((target_prev_hps, allEffectTargets, allDisplayNumberTargets) =>
      {
        bool flag = false;
        foreach (BL.Unit displayNumberTarget in allDisplayNumberTargets)
        {
          if (displayNumberTarget.isView && target_prev_hps.ContainsKey(displayNumberTarget))
          {
            this.unitResource[displayNumberTarget].unitParts_.dispHpNumber(target_prev_hps[displayNumberTarget], displayNumberTarget.hp);
            if (displayNumberTarget.hp <= 0)
              flag = true;
          }
        }
        Singleton<NGBattleManager>.GetInstance().battleEffects.mbFieldEffectStart(unit, mb, allEffectTargets);
        if (!flag)
          return;
        btm.setEnableWait(1.5f);
      }), this.core);
      BL.UnitPosition unitPosition = this.core.getUnitPosition(unit);
      if (unitPosition.completedCount < 1)
        return;
      List<List<BL.ExecuteSkillEffectResult>> result;
      List<BL.UnitPosition> unitPositionList = this.core.completedPositionExecuteSkillEffects(unitPosition, out result);
      BattleStateController controller = Singleton<NGBattleManager>.GetInstance().getController<BattleStateController>();
      int num = 0;
      bool flag1 = false;
      BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(controller);
      foreach (BL.UnitPosition up in unitPositionList)
      {
        foreach (BL.ExecuteSkillEffectResult es in result[num++])
        {
          if (es.targets.Count > 0 || es.targetPanels.Count > 0)
          {
            facilitySkillDeads.Add(es);
            controller.doExecuteFacilitySkillEffects(up, es);
            flag1 = true;
          }
        }
      }
      facilitySkillDeads.Execute();
      if (!flag1)
        return;
      btm.setEnableWait(1.5f);
    }));
  }

  public BE.DefaultDict<int, BE.SkillResource> skillResource
  {
    get => this.getDefaultDict<int, BE.SkillResource>(ref this.skillResource_);
  }

  public BE.DefaultDict<int, BE.AilmentSkillResource> ailmentSkillResource
  {
    get => this.getDefaultDict<int, BE.AilmentSkillResource>(ref this.ailmentSkillResource_);
  }

  public void useSkill(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels,
    BL.BattleSkillResult bsr,
    BattleTimeManager btm,
    XorShift random = null)
  {
    this.core.battleLogger.SkillUse(unit, skill, targets, panels);
    unit.countSkillUses(skill.id);
    if (!Singleton<NGBattleManager>.GetInstance().noDuelScene && skill.skill.duel_effect_BattleskillDuelEffect.HasValue)
    {
      btm.changeSceneWithReturnWait("battleCutScene", true, (Action) (() =>
      {
        Singleton<NGBattleManager>.GetInstance().getManager<NGBattle3DObjectManager>()?.setRootActive(false);
        Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGM(2f, 1f);
      }), (Action) null, (Action) null, (object) unit, (object) skill);
      btm.setEnableWait(0.1f);
    }
    Battle01SelectNode uiNode = Singleton<NGBattleManager>.GetInstance().getUiNode();
    if (!Singleton<NGBattleManager>.GetInstance().noDuelScene && skill.isSEA)
    {
      btm.setScheduleAction((Action) (() => uiNode.doSEASkillCutin(unit.playerUnit)), 1f);
      btm.setEnableWait(new Func<bool>(uiNode.checkSEASkillCutinCompleted));
    }
    btm.setScheduleAction((Action) (() =>
    {
      Battle01SelectNode.MaskContinuer mc = uiNode.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
      Dictionary<BL.Unit, int> target_prev_hps = new Dictionary<BL.Unit, int>();
      Dictionary<BL.Unit, bool> isRebirths = new Dictionary<BL.Unit, bool>();
      Dictionary<BL.Unit, bool> isJumping = new Dictionary<BL.Unit, bool>();
      foreach (BL.Unit target in targets)
        isRebirths.Add(target, (skill.targetType == BattleskillTargetType.dead_player_single || skill.targetType == BattleskillTargetType.dead_player_range) && target.isDead);
      foreach (BL.ISkillEffectListUnit allUnit in BattleFuncs.getAllUnits(false, true, includeJumping: true))
      {
        target_prev_hps.Add(allUnit.originalUnit, allUnit.hp);
        isJumping.Add(allUnit.originalUnit, allUnit.IsJumping);
      }
      foreach (BL.Unit key in isRebirths.Keys)
      {
        if (!target_prev_hps.ContainsKey(key))
          target_prev_hps.Add(key, key.hp);
      }
      Tuple<int, int> usePosition = (Tuple<int, int>) null;
      if (unit != (BL.Unit) null)
      {
        BL.UnitPosition unitPosition = this.core.getUnitPosition(unit);
        usePosition = Tuple.Create<int, int>(unitPosition.row, unitPosition.column);
      }
      int nowUseCount = skill.nowUseCount;
      this.core.useSkillWith(unit, skill, targets, panels, bsr, (Action<BL.UseSkillWithResult>) (r =>
      {
        List<Tuple<BE.UnitResource, int, int>> dispHp = new List<Tuple<BE.UnitResource, int, int>>();
        foreach (BL.Unit displayNumberTarget in r.displayNumberTargets)
        {
          if (isRebirths.ContainsKey(displayNumberTarget) && isRebirths[displayNumberTarget])
          {
            displayNumberTarget.rebirthBE(this, true);
            this.unitResource[displayNumberTarget].unitParts_.setHpGauge(displayNumberTarget.hp);
          }
          else if (displayNumberTarget.isView && target_prev_hps.ContainsKey(displayNumberTarget))
          {
            BE.UnitResource unitResource = this.unitResource[displayNumberTarget];
            dispHp.Add(Tuple.Create<BE.UnitResource, int, int>(unitResource, target_prev_hps[displayNumberTarget], displayNumberTarget.hp));
          }
        }
        if (r.dispHpUnit != (BL.Unit) null && r.dispHpUnit.isView && !r.displayNumberTargets.Contains(r.dispHpUnit))
        {
          BE.UnitResource unitResource = this.unitResource[r.dispHpUnit];
          dispHp.Add(Tuple.Create<BE.UnitResource, int, int>(unitResource, r.prevHp, r.dispHpUnit.hp));
        }
        foreach (BL.Unit key in target_prev_hps.Keys)
        {
          if (key.isView)
            this.unitResource[key].unitParts_.SetEffectMode(true);
        }
        foreach (BL.Unit destructFacility in r.destructFacilities)
        {
          BattleUnitParts unitParts = this.unitResource[destructFacility].unitParts_;
          if (destructFacility.isView)
            unitParts.SetEffectMode(true, true);
          unitParts.destructFacility();
          btm.setScheduleAction((Action) null, comleteCheckFunc: (Func<bool>) (() => unitParts.checkEffectCompleted()));
        }
        bool dontCreateFacility = true;
        Action action4 = (Action) (() =>
        {
          foreach (Tuple<BE.UnitResource, int, int> tuple in dispHp)
          {
            BattleUnitParts unitParts = tuple.Item1.unitParts_;
            unitParts.dispHpNumber(tuple.Item2, tuple.Item3);
            unitParts.setHpGauge(tuple.Item2, tuple.Item3);
          }
          if (dontCreateFacility)
            return;
          foreach (BL.Unit createFacility in r.createFacilities)
          {
            BattleUnitParts unitParts = this.unitResource[createFacility].unitParts_;
            unitParts.createFacility(skill.skill.field_effect.target_wait_seconds);
            unitParts.setHpGauge(createFacility.hp);
          }
        });
        BE.SkillResource skillResource = this.skillResource[skill.skill.field_effect.ID];
        if (!r.lateDispHp && Object.op_Equality((Object) skillResource.invokedEffectPrefab, (Object) null))
        {
          action4();
          action4 = (Action) (() =>
          {
            foreach (BL.Unit createFacility in r.createFacilities)
            {
              BattleUnitParts unitParts = this.unitResource[createFacility].unitParts_;
              unitParts.createFacility(skill.skill.field_effect.target_wait_seconds);
              unitParts.setHpGauge(createFacility.hp);
            }
          });
        }
        dontCreateFacility = false;
        BL.Unit unit5 = !skill.skill.field_effect.invoked_effect_target || targets.Count < 1 ? unit : targets[0];
        Quaternion? nullable = new Quaternion?();
        if (unit != (BL.Unit) null && ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.skill_chain && x.checkLevel(skill.level) && x.checkUseSkillCount(nowUseCount) && x.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) == 3)))
        {
          BL.UnitPosition unitPosition3 = this.core.getUnitPosition(unit);
          if (targets.Any<BL.Unit>())
          {
            BL.UnitPosition unitPosition4 = this.core.getUnitPosition(targets[0]);
            nullable = new Quaternion?(Quaternion.Euler(0.0f, Mathf.Atan2((float) (unitPosition4.column - unitPosition3.column), (float) (unitPosition4.row - unitPosition3.row)) * 57.29578f, 0.0f));
          }
          else if (panels.Any<BL.Panel>())
            nullable = new Quaternion?(Quaternion.Euler(0.0f, Mathf.Atan2((float) (panels[0].column - unitPosition3.column), (float) (panels[0].row - unitPosition3.row)) * 57.29578f, 0.0f));
        }
        else if (unit != (BL.Unit) null && ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.skill_chain && x.checkLevel(skill.level) && x.checkUseSkillCount(nowUseCount) && x.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) == 4)))
        {
          BL.UnitPosition unitPosition = this.core.getUnitPosition(unit);
          nullable = new Quaternion?(Quaternion.Euler(0.0f, Mathf.Atan2((float) (unitPosition.column - usePosition.Item2), (float) (unitPosition.row - usePosition.Item1)) * 57.29578f, 0.0f));
        }
        Action action5 = (Action) null;
        List<BL.Unit> newJumpUnits = new List<BL.Unit>();
        foreach (BL.Unit key in isJumping.Keys)
        {
          if (!isJumping[key] && key.IsJumping)
            newJumpUnits.Add(key);
        }
        if (newJumpUnits.Any<BL.Unit>())
          action5 = (Action) (() =>
          {
            foreach (BL.Unit unit3 in newJumpUnits)
            {
              this.unitResource[unit3].unitParts_.jumpUp();
              BL.UnitPosition unitPosition = this.core.getUnitPosition(unit3);
              BattleFuncs.getPanel(unitPosition.row, unitPosition.column).isJumping = true;
            }
          });
        BattleEffects battleEffects = Singleton<NGBattleManager>.GetInstance().battleEffects;
        BL.Unit unit6 = unit;
        BL.Skill skill2 = skill;
        List<BL.Unit> effectTargets = r.effectTargets;
        List<BL.Unit> invokeUnits = new List<BL.Unit>();
        invokeUnits.Add(unit5);
        Action targetAction = action4;
        List<Quaternion?> invokedEffectRotate;
        if (!nullable.HasValue)
        {
          invokedEffectRotate = (List<Quaternion?>) null;
        }
        else
        {
          invokedEffectRotate = new List<Quaternion?>();
          invokedEffectRotate.Add(nullable);
        }
        Action action6 = action5;
        List<BL.Panel> effectPanelTargets = r.effectPanelTargets;
        battleEffects.skillFieldEffectStart(unit6, skill2, effectTargets, invokeUnits, targetAction, invokedEffectRotate, action6, effectPanelTargets);
        if (!unit.isPlayerForce)
          uiNode.PlaySkillNotice(skill);
        btm.setEnableWait(1.5f);
        foreach (BL.Unit createFacility in r.createFacilities)
        {
          BattleUnitParts unitParts = this.unitResource[createFacility].unitParts_;
          btm.setScheduleAction((Action) null, comleteCheckFunc: (Func<bool>) (() => unitParts.checkEffectCompleted()));
        }
        if (((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.effect_logic_BattleskillEffectLogic == 1001677)))
          btm.setEnableWait(new Func<bool>(this.unitResource[unit].unitParts_.checkEffectCompleted));
        if (newJumpUnits.Any<BL.Unit>())
        {
          foreach (BL.Unit key in newJumpUnits)
            btm.setEnableWait(new Func<bool>(this.unitResource[key].unitParts_.checkEffectCompleted));
        }
        btm.setScheduleAction((Action) (() =>
        {
          foreach (BL.Unit key in target_prev_hps.Keys)
          {
            if (key.isView)
              this.unitResource[key].unitParts_.SetEffectMode(false);
          }
          foreach (BL.Unit destructFacility in r.destructFacilities)
          {
            if (destructFacility.isView)
              this.unitResource[destructFacility].unitParts_.SetEffectMode(false);
          }
        }));
      }), this.core, random);
      BL.UnitPosition unitPosition5 = this.core.getUnitPosition(unit);
      if (unitPosition5.completedCount >= 1)
      {
        List<List<BL.ExecuteSkillEffectResult>> result;
        List<BL.UnitPosition> unitPositionList = this.core.completedPositionExecuteSkillEffects(unitPosition5, out result);
        BattleStateController controller = Singleton<NGBattleManager>.GetInstance().getController<BattleStateController>();
        int num = 0;
        bool flag = false;
        BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(controller);
        foreach (BL.UnitPosition up in unitPositionList)
        {
          foreach (BL.ExecuteSkillEffectResult es in result[num++])
          {
            if (es.targets.Count > 0 || es.targetPanels.Count > 0)
            {
              facilitySkillDeads.Add(es);
              controller.doExecuteFacilitySkillEffects(up, es);
              flag = true;
            }
          }
        }
        facilitySkillDeads.Execute();
        if (flag)
          btm.setEnableWait(1.5f);
      }
      uiNode.setMaskActive(false, mc);
      if (!(unit != (BL.Unit) null) || !unit.isPlayerForce || !((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.random_choice && x.checkLevel(skill.level))))
        return;
      Singleton<NGBattleManager>.GetInstance().saveEnvironment();
    }));
  }

  public void useCallSkill(
    BL.Skill skill,
    List<BL.Unit> targets,
    BattleTimeManager btm,
    bool isPlayer = true,
    XorShift random = null)
  {
    this.core.battleLogger.CallSkillUse(skill, targets);
    int same_character_id = 0;
    if (isPlayer)
    {
      this.core.playerCallSkillState.callSkillPoint = 0M;
      this.core.playerCallSkillState.isUsedCallSkill = true;
      same_character_id = this.core.battleInfo.playerCallSkillParam.same_character_id;
    }
    else
    {
      this.core.enemyCallSkillState.callSkillPoint = 0M;
      this.core.enemyCallSkillState.isUsedCallSkill = true;
      same_character_id = this.core.battleInfo.enemyCallSkillParam.same_character_id;
    }
    Battle01SelectNode uiNode = Singleton<NGBattleManager>.GetInstance().getUiNode();
    btm.setScheduleAction((Action) (() => uiNode.doCallSkillCutin(same_character_id)), 2.5f);
    btm.setScheduleAction((Action) (() =>
    {
      Battle01SelectNode.MaskContinuer mc = uiNode.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
      Dictionary<BL.Unit, int> target_prev_hps = new Dictionary<BL.Unit, int>();
      foreach (BL.ISkillEffectListUnit allUnit in BattleFuncs.getAllUnits(false, true, includeJumping: true))
        target_prev_hps.Add(allUnit.originalUnit, allUnit.hp);
      this.core.useCallSkillWith(skill, targets, (Action<BL.UseSkillWithResult>) (r =>
      {
        List<Tuple<BE.UnitResource, int, int>> dispHp = new List<Tuple<BE.UnitResource, int, int>>();
        foreach (BL.Unit displayNumberTarget in r.displayNumberTargets)
        {
          if (displayNumberTarget.isView && target_prev_hps.ContainsKey(displayNumberTarget))
            dispHp.Add(Tuple.Create<BE.UnitResource, int, int>(this.unitResource[displayNumberTarget], target_prev_hps[displayNumberTarget], displayNumberTarget.hp));
        }
        foreach (BL.Unit key in target_prev_hps.Keys)
        {
          if (key.isView)
            this.unitResource[key].unitParts_.SetEffectMode(true);
        }
        Action targetAction = (Action) (() =>
        {
          foreach (Tuple<BE.UnitResource, int, int> tuple in dispHp)
          {
            BattleUnitParts unitParts = tuple.Item1.unitParts_;
            unitParts.dispHpNumber(tuple.Item2, tuple.Item3);
            unitParts.setHpGauge(tuple.Item2, tuple.Item3);
          }
        });
        BE.SkillResource skillResource = this.skillResource[skill.skill.field_effect.ID];
        if (!r.lateDispHp && Object.op_Equality((Object) skillResource.invokedEffectPrefab, (Object) null))
        {
          targetAction();
          targetAction = (Action) null;
        }
        Singleton<NGBattleManager>.GetInstance().battleEffects.skillFieldEffectStart((BL.Unit) null, skill, r.effectTargets, (List<BL.Unit>) null, targetAction);
        uiNode.PlaySkillNotice(skill, isPlayer, 4f);
        if (isPlayer)
          uiNode.doDelayBacktoTopForCallSkill();
        btm.setEnableWait(new Func<bool>(uiNode.checkSkillNoticeCompleted));
        btm.setScheduleAction((Action) (() =>
        {
          foreach (BL.Unit key in target_prev_hps.Keys)
          {
            if (key.isView)
              this.unitResource[key].unitParts_.SetEffectMode(false);
          }
        }));
      }), this.core, random, isPlayer);
      uiNode.setMaskActive(false, mc);
    }));
  }

  public BE.DefaultDict<BL.Unit, BE.UnitResource> unitResource
  {
    get => this.getDefaultDict<BL.Unit, BE.UnitResource>(ref this.unitResource_);
  }

  public void rebirthUnits(List<BL.Unit> units, BattleTimeManager btm)
  {
    btm.setScheduleAction((Action) (() =>
    {
      foreach (BL.Unit unit in units)
      {
        unit.rebirth(this.core);
        unit.rebirthBE(this);
      }
    }));
  }

  public void setCurrentUnit_(BL.Unit unit)
  {
    this.core.setCurrentUnitWith(unit, new Action<BL.UnitPosition>(this.cancelMovePrevCurrentUnit));
  }

  public void setCurrentUnitByPlayerInput(BL.Unit unit)
  {
    this.core.setCurrentUnitByPlayerInput(unit, new Action<BL.UnitPosition>(this.cancelMovePrevCurrentUnit));
  }

  private void cancelMovePrevCurrentUnit(BL.UnitPosition prevUp)
  {
    if (prevUp == null || this.core.phaseState.state == BL.Phase.pvp_disposition || this.core.phaseState.state == BL.Phase.pvp_wait_preparing || this.core.phaseState.state == BL.Phase.enemy)
      return;
    prevUp.cancelMove(this);
  }

  public void pushWaveStageDatas()
  {
    this.pushWaveEnemies();
    this.pushWaveDropData();
  }

  private void pushWaveEnemies() => this.waveEnemiesStack.Push(this.core.enemyUnits.value);

  private void pushWaveDropData()
  {
    List<Tuple<BL.DropData, int>> tupleList = new List<Tuple<BL.DropData, int>>();
    for (int row = 0; row < this.core.getFieldHeight(); ++row)
    {
      for (int column = 0; column < this.core.getFieldWidth(); ++column)
      {
        BL.Panel fieldPanel = this.core.getFieldPanel(row, column);
        if (fieldPanel.hasEvent && fieldPanel.fieldEvent.isCompleted)
          tupleList.Add(new Tuple<BL.DropData, int>(fieldPanel.fieldEvent, fieldPanel.fieldEventId));
      }
    }
    this.waveDropStack.Push(tupleList);
  }

  public BE.DefaultDict<BL.Weapon, BE.WeaponResource> weaponResource
  {
    get => this.getDefaultDict<BL.Weapon, BE.WeaponResource>(ref this.weaponResource_);
  }

  [Serializable]
  public class DropDataResource
  {
    [NonSerialized]
    private GameObject mPrefab;

    public GameObject prefab
    {
      get => this.mPrefab;
      set => this.mPrefab = value;
    }
  }

  public class DefaultDict<TKey, TValue> where TValue : new()
  {
    private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

    public bool ContainsKey(TKey key) => this.dict.ContainsKey(key);

    public TValue this[TKey key]
    {
      get
      {
        TValue obj1;
        if (this.dict.TryGetValue(key, out obj1))
          return obj1;
        TValue obj2 = new TValue();
        this.dict.Add(key, obj2);
        return obj2;
      }
      set => this.dict[key] = value;
    }

    public void cleanup() => this.dict.Clear();
  }

  public class PanelResource
  {
    private GameObject mGameObject;

    public GameObject gameObject
    {
      get => this.mGameObject;
      set => this.mGameObject = value;
    }
  }

  public class StageResource
  {
    private GameObject mPrefab;

    public GameObject prefab
    {
      get => this.mPrefab;
      set => this.mPrefab = value;
    }
  }

  public class ItemResource
  {
    [NonSerialized]
    private GameObject mTargetEffectPrefab;

    public GameObject targetEffectPrefab
    {
      get => this.mTargetEffectPrefab;
      set => this.mTargetEffectPrefab = value;
    }
  }

  public class SkillResource
  {
    [NonSerialized]
    private GameObject mEffectPrefab;
    [NonSerialized]
    private GameObject mTargetEffectPrefab;
    [NonSerialized]
    private GameObject mInvokedEffectPrefab;

    public GameObject effectPrefab
    {
      get => this.mEffectPrefab;
      set => this.mEffectPrefab = value;
    }

    public GameObject targetEffectPrefab
    {
      get => this.mTargetEffectPrefab;
      set => this.mTargetEffectPrefab = value;
    }

    public GameObject invokedEffectPrefab
    {
      get => this.mInvokedEffectPrefab;
      set => this.mInvokedEffectPrefab = value;
    }
  }

  public class AilmentSkillResource
  {
    [NonSerialized]
    private GameObject mTargetEffectPrefab;

    public GameObject targetEffectPrefab
    {
      get => this.mTargetEffectPrefab;
      set => this.mTargetEffectPrefab = value;
    }
  }

  public class UnitResource
  {
    [NonSerialized]
    private GameObject mGameObject;
    [NonSerialized]
    private GameObject mPrefab;
    [NonSerialized]
    private GameObject mAttachPointPrefab;
    [NonSerialized]
    private GameObject mEquipPrefab_a;
    [NonSerialized]
    private GameObject mEquipPrefab_b;
    [NonSerialized]
    private GameObject mBikePrefab;
    [NonSerialized]
    private BE.UnitResource.Attachment mUnitEffect;
    [NonSerialized]
    private Material mFaceMaterial;
    [NonSerialized]
    private BattleUnitParts mUnitParts;
    [NonSerialized]
    private bool isSelectVoice;

    public GameObject gameObject
    {
      get => this.mGameObject;
      set => this.mGameObject = value;
    }

    public GameObject prefab
    {
      get => this.mPrefab;
      set => this.mPrefab = value;
    }

    public GameObject attachPointPrefab
    {
      get => this.mAttachPointPrefab;
      set => this.mAttachPointPrefab = value;
    }

    public GameObject equipPrefab_a
    {
      get => this.mEquipPrefab_a;
      set => this.mEquipPrefab_a = value;
    }

    public GameObject equipPrefab_b
    {
      get => this.mEquipPrefab_b;
      set => this.mEquipPrefab_b = value;
    }

    public GameObject bikePrefab
    {
      get => this.mBikePrefab;
      set => this.mBikePrefab = value;
    }

    public ref BE.UnitResource.Attachment unitEffect => ref this.mUnitEffect;

    public Material faceMaterial
    {
      get => this.mFaceMaterial;
      set => this.mFaceMaterial = value;
    }

    public BattleUnitParts unitParts_
    {
      get
      {
        if (Object.op_Equality((Object) this.gameObject, (Object) null))
          return (BattleUnitParts) null;
        if (Object.op_Equality((Object) this.mUnitParts, (Object) null))
          this.mUnitParts = this.gameObject.GetComponent<BattleUnitParts>();
        return this.mUnitParts;
      }
    }

    public void PlayVoiceDuelStart(BL.Unit unit)
    {
      if (this.isSelectVoice || Singleton<NGSoundManager>.GetInstance().PlayVoicePriorityFirst(unit.unit, 75, 0) == -1)
        return;
      this.isSelectVoice = true;
    }

    public void cleanup()
    {
      this.mGameObject = (GameObject) null;
      this.mPrefab = (GameObject) null;
      this.mEquipPrefab_a = (GameObject) null;
      this.mEquipPrefab_b = (GameObject) null;
      this.mBikePrefab = (GameObject) null;
      this.mUnitEffect = new BE.UnitResource.Attachment(string.Empty, (GameObject) null);
      this.mFaceMaterial = (Material) null;
      this.mUnitParts = (BattleUnitParts) null;
    }

    public struct Attachment
    {
      [NonSerialized]
      public string node;
      [NonSerialized]
      public GameObject prefab;

      public Attachment(string n, GameObject p)
      {
        this.node = n;
        this.prefab = p;
      }
    }
  }

  public class WeaponResource
  {
    [NonSerialized]
    private GameObject mPrefab;

    public GameObject prefab
    {
      get => this.mPrefab;
      set => this.mPrefab = value;
    }
  }
}
