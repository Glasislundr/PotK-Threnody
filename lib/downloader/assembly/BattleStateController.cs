// Decompiled with JetBrains decompiler
// Type: BattleStateController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleStateController : BattleMonoBehaviour
{
  private const string effect_condition_for_victory = "Condition_forVictory";
  private const string effect_player_phase = "PlayerPhase";
  private const string effect_neutral_phase = "NeutralPhase";
  private const string effect_enemy_phase = "EnemyPhase";
  private const string effect_stage_clear = "StageClear";
  private const string effect_wave_clear = "WaveClear";
  private const string effect_gameover = "GameOver";
  private const string effect_turnover = "dir_TurnOver";
  private const string pvpMatchStartPrefab_path = "Prefabs/battle/dir_PvpMatchStart";
  private const string gvgMatchStartPrefab_path = "Prefabs/battle/dir_PvpMatchStart_for_guild";
  private const float effect_stage_clear_time = 5f;
  private const float effect_stage_clear_time_auto = 3f;
  private const float effect_skip_stage_clear_time = 2f;
  private BL.BattleModified<BL.PhaseState> phaseStateModified;
  private BL.BattleModified<BL.ClassValue<List<BL.UnitPosition>>> playerListModified;
  private BL.BattleModified<BL.ClassValue<List<BL.UnitPosition>>> neutralListModified;
  private BL.BattleModified<BL.ClassValue<List<BL.UnitPosition>>> enemyListModified;
  private BL.BattleModified<BL.ClassValue<List<BL.UnitPosition>>> completedListModified;
  private BL.UnitPosition lastCompletedUnit;
  private BL.BattleModified<BL.ClassValue<List<BL.UnitPosition>>> spawnUnitListModified;
  private BL.BattleModified<BL.StructValue<bool>> isAutoBattleModified;
  private BL.UnitPosition currentUnitPosition;
  private int currentUnitActionCount;
  private BattleAIController aiController;
  private BattleTimeManager btm;
  private GameObject popupAllDeadPlayerPrefab;
  private GameObject spawnEffectPrefab;
  private GameObject healPrefab;
  private Dictionary<string, GameObject> panelEffectPrefabs;
  private GameObject pvpMatchStartPrefab;
  private Battle01SelectNode uiNode;
  private PVPManager _pvpManager;
  private BL.StructValue<bool> waitCurrentAIActionCancel = new BL.StructValue<bool>(false);
  private bool isStageClear;
  private bool isGameOver;
  private bool isTurnOver;
  private bool isSpawning;
  private BattleFinalize finalize;

  public void setUiNode(Battle01SelectNode node) => this.uiNode = node;

  private PVPManager pvpManager
  {
    get
    {
      if (Object.op_Equality((Object) this._pvpManager, (Object) null))
        this._pvpManager = Singleton<PVPManager>.GetInstanceOrNull();
      return this._pvpManager;
    }
  }

  public bool isWaitCurrentAIActionCancel
  {
    get => this.waitCurrentAIActionCancel.value;
    private set
    {
      if (this.waitCurrentAIActionCancel.value == value)
        return;
      this.waitCurrentAIActionCancel.value = value;
    }
  }

  public BL.StructValue<bool> instWaitCurrentAIActionCancel => this.waitCurrentAIActionCancel;

  protected override IEnumerator Start_Original()
  {
    BattleStateController battleStateController = this;
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    Future<GameObject> f = Res.Prefabs.popup.popup_022_8__anim_popup01.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleStateController.popupAllDeadPlayerPrefab = f.Result;
    f = rm.Load<GameObject>("BattleEffects/field/ef035_enemy_sporn");
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleStateController.spawnEffectPrefab = f.Result;
    HashSet<string> stringSet = new HashSet<string>();
    battleStateController.panelEffectPrefabs = new Dictionary<string, GameObject>();
    for (int row = 0; row < battleStateController.env.core.getFieldHeight(); ++row)
    {
      for (int column = 0; column < battleStateController.env.core.getFieldWidth(); ++column)
      {
        foreach (BattleLandformIncr battleLandformIncr in battleStateController.env.core.getFieldPanel(row, column).landform.GetAllIncr())
        {
          BattleLandformEffectGroup effectGroup = battleLandformIncr.effect_group;
          if (effectGroup != null)
            stringSet.Add(effectGroup.play_prefab_file_name);
        }
      }
    }
    foreach (string path in stringSet)
    {
      f = rm.LoadOrNull<GameObject>(string.Format("BattleEffects/field/{0}", (object) path));
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) f.Result, (Object) null))
        battleStateController.panelEffectPrefabs.Add(path, f.Result);
    }
    f = rm.Load<GameObject>("BattleEffects/field/ef009_fixed_heal");
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleStateController.healPrefab = f.Result;
    if (battleStateController.battleManager.isPvp || battleStateController.battleManager.isPvnpc)
    {
      f = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle/dir_PvpMatchStart");
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleStateController.pvpMatchStartPrefab = f.Result;
    }
    else if (battleStateController.battleManager.isGvg)
    {
      f = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle/dir_PvpMatchStart_for_guild");
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleStateController.pvpMatchStartPrefab = f.Result;
    }
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleStateController battleStateController = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleStateController.phaseStateModified = BL.Observe<BL.PhaseState>(battleStateController.env.core.phaseState);
    battleStateController.playerListModified = BL.Observe<BL.ClassValue<List<BL.UnitPosition>>>(battleStateController.env.core.playerActionUnits);
    battleStateController.neutralListModified = BL.Observe<BL.ClassValue<List<BL.UnitPosition>>>(battleStateController.env.core.neutralActionUnits);
    battleStateController.enemyListModified = BL.Observe<BL.ClassValue<List<BL.UnitPosition>>>(battleStateController.env.core.enemyActionUnits);
    battleStateController.completedListModified = BL.Observe<BL.ClassValue<List<BL.UnitPosition>>>(battleStateController.env.core.completedActionUnits);
    battleStateController.spawnUnitListModified = BL.Observe<BL.ClassValue<List<BL.UnitPosition>>>(battleStateController.env.core.spawnUnits);
    battleStateController.isAutoBattleModified = BL.Observe<BL.StructValue<bool>>(battleStateController.env.core.isAutoBattle);
    battleStateController.aiController = ((Component) battleStateController).gameObject.AddComponent<BattleAIController>();
    battleStateController.btm = battleStateController.battleManager.getManager<BattleTimeManager>();
    if (battleStateController.battleManager.hasSavedEnvironment())
    {
      BL.Panel panel = battleStateController.env.core.fieldCurrent.value;
      if (panel != null)
        battleStateController.battleManager.getController<BattleCameraController>().setLookAtTarget(panel);
    }
    return false;
  }

  private bool startStroyWithNextState(BL.Story story, BL.Phase state)
  {
    if (story != null && story.scriptId >= 0)
    {
      Singleton<NGBattleManager>.GetInstance().startStory(story);
      this.btm.setPhaseState(state);
      return true;
    }
    this.btm.setPhaseState(state);
    return false;
  }

  private bool startStroyWithNextState(BL.StoryType stype, BL.Phase state)
  {
    return this.startStroyWithNextState(this.env.core.getStory(stype), state);
  }

  private void setStartTarget(BL.UnitPosition up, bool nonCurrent)
  {
    if (nonCurrent)
      this.btm.setTargetPanel(this.env.core.getFieldPanel(up), 0.1f);
    else
      this.btm.setCurrentUnit(up, 0.1f);
  }

  private void executeSkillEffects(List<BL.UnitPosition> upl)
  {
    bool isWait = false;
    this.btm.setScheduleAction((Action) (() =>
    {
      bool flag = false;
      foreach (BL.UnitPosition up in upl)
      {
        foreach (BL.ExecuteSkillEffectResult executeSkillEffect in this.env.core.executeSkillEffects(up))
        {
          if (executeSkillEffect.targets.Count > 0)
          {
            this.doExecuteSkillEffects(up, executeSkillEffect);
            isWait = true;
            if (executeSkillEffect.targets.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.hp <= 0)))
              flag = true;
          }
        }
      }
      if (flag)
        this.btm.setEnableWait(1.5f);
      float wait = 0.0f;
      Dictionary<BL.UnitPosition, int> prevHp = new Dictionary<BL.UnitPosition, int>();
      List<BattleUnitParts> source = new List<BattleUnitParts>();
      foreach (BL.UnitPosition key in this.env.core.unitPositions.value)
      {
        prevHp[key] = key.unit.hp;
        BattleUnitParts unitParts = this.env.unitResource[key.unit].unitParts_;
        unitParts.SetEffectMode(true);
        if (!unitParts.checkEffectCompleted())
          source.Add(unitParts);
      }
      BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(this);
      foreach (BL.UnitPosition up1 in upl)
      {
        foreach (BL.ExecuteSkillEffectResult es1 in this.env.core.executePhaseSkillEffects(up1).ToList<BL.ExecuteSkillEffectResult>())
        {
          if (es1.targets.Count > 0 || es1.targetPanels.Count > 0)
          {
            Quaternion? nullable;
            if (es1.invokedEffectVector.HasValue)
            {
              Vector2 vector2 = es1.invokedEffectVector.Value;
              nullable = new Quaternion?(Quaternion.Euler(0.0f, Mathf.Atan2(vector2.x, vector2.y) * 57.29578f, 0.0f));
            }
            else
              nullable = new Quaternion?();
            facilitySkillDeads.Add(es1);
            BL.UnitPosition up2 = up1;
            BL.ExecuteSkillEffectResult es2 = es1;
            List<BL.Unit> invokedTargetUnits = new List<BL.Unit>();
            invokedTargetUnits.Add(up1.unit);
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
            this.doExecuteFacilitySkillEffects(up2, es2, invokedTargetUnits, invokedEffectRotate: invokedEffectRotate);
            this.btm.setScheduleAction((Action) null, 0.5f);
            wait = 1f;
          }
        }
      }
      facilitySkillDeads.Execute();
      Dictionary<BL.UnitPosition, int> afterHp = new Dictionary<BL.UnitPosition, int>();
      foreach (BL.UnitPosition key in this.env.core.unitPositions.value)
      {
        afterHp[key] = key.unit.hp;
        if (key.unit.hp != prevHp[key])
          key.unit.hp = prevHp[key];
      }
      if (source.Any<BattleUnitParts>())
      {
        foreach (BattleUnitParts battleUnitParts in source)
          this.btm.setEnableWait(new Func<bool>(battleUnitParts.checkEffectCompleted));
      }
      this.btm.setScheduleAction((Action) null, wait, (Action) (() =>
      {
        foreach (BL.UnitPosition key in this.env.core.unitPositions.value)
        {
          if (prevHp[key] != afterHp[key])
            key.unit.hp = afterHp[key];
          this.env.unitResource[key.unit].unitParts_.SetEffectMode(false);
        }
      }));
      this.btm.setScheduleAction((Action) (() =>
      {
        foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
          this.uiNode.hpCheckWithDeadEffects(unitPosition.unit, true);
      }), isInsertMode: true);
    }), isInsertMode: true);
    if (isWait)
      this.btm.setEnableWait(0.1f);
    else
      this.btm.setScheduleAction((Action) (() => this.battleManager.isBattleEnable = true));
  }

  private void executeJumpSkillEffects(IEnumerable<BL.UnitPosition> ups, BL.ForceID phaseForce = BL.ForceID.none)
  {
    this.btm.setScheduleAction((Action) (() =>
    {
      BL.UnitPosition[] array1 = ups.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
      {
        if (!x.unit.IsJumping || x.unit.hp < 1 || !x.unit.isEnable)
          return false;
        return phaseForce == BL.ForceID.none || (BL.ForceID) x.unit.skillEffects.Where(BattleskillEffectLogicEnum.jump).First<BL.SkillEffect>().work[2] == phaseForce;
      })).OrderBy<BL.UnitPosition, float>((Func<BL.UnitPosition, float>) (x => x.unit.skillEffects.Where(BattleskillEffectLogicEnum.jump).First<BL.SkillEffect>().work[1])).ToArray<BL.UnitPosition>();
      if (!((IEnumerable<BL.UnitPosition>) array1).Any<BL.UnitPosition>())
        return;
      foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
        this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(true);
      List<BattleUnitParts> source = new List<BattleUnitParts>();
      BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(this);
      foreach (BL.UnitPosition unitPosition in array1)
      {
        BL.UnitPosition jumpUnitUp = unitPosition;
        BL.Unit jumpUnit = jumpUnitUp.unit;
        BL.SkillEffect jumpEffect = jumpUnit.skillEffects.Where(BattleskillEffectLogicEnum.jump).First<BL.SkillEffect>();
        BL.UnitPosition targetUp = this.env.core.unitPositions.value.FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => (double) x.id == (double) jumpEffect.work[0]));
        bool flag = false;
        jumpUnit.skillEffects.RemoveEffect(1001836, 0, 0, this.env.core, (BL.ISkillEffectListUnit) jumpUnit);
        if (targetUp != null && targetUp.unit.hp >= 1 && !targetUp.unit.IsJumping)
        {
          BL.ExecuteSkillEffectResult[] array2 = this.env.core.executeJumpSkillEffects(jumpUnitUp, targetUp).ToArray<BL.ExecuteSkillEffectResult>();
          if (array2.Length >= 1)
          {
            BL.ExecuteSkillEffectResult es1 = array2[0];
            flag = true;
            facilitySkillDeads.Add(es1);
            BL.UnitPosition up = jumpUnitUp;
            BL.ExecuteSkillEffectResult es2 = es1;
            List<BL.Unit> invokedTargetUnits = new List<BL.Unit>();
            invokedTargetUnits.Add(targetUp.unit);
            Action invokedAction = (Action) (() =>
            {
              this.env.core.lookDirection(jumpUnitUp, targetUp);
              this.env.unitResource[jumpUnit].unitParts_.jumpReturn();
              BattleFuncs.getPanel(jumpUnitUp.row, jumpUnitUp.column).isJumping = false;
            });
            this.doExecuteFacilitySkillEffects(up, es2, invokedTargetUnits, invokedAction);
            this.btm.setScheduleAction((Action) null, 1f);
          }
        }
        if (!flag)
        {
          this.btm.setTargetUnit(jumpUnitUp, 0.1f, isWaitCameraMove: true);
          this.btm.setScheduleAction((Action) (() =>
          {
            this.env.unitResource[jumpUnit].unitParts_.jumpMiss();
            BattleFuncs.getPanel(jumpUnitUp.row, jumpUnitUp.column).isJumping = false;
          }), 1f);
        }
        source.Add(this.env.unitResource[jumpUnit].unitParts_);
        BattleFuncs.removeJumpEffects((BL.ISkillEffectListUnit) jumpUnit, (BL.ISkillEffectListUnit) targetUp.unit, jumpEffect);
      }
      facilitySkillDeads.Execute();
      if (source.Any<BattleUnitParts>())
      {
        foreach (BattleUnitParts battleUnitParts in source)
          this.btm.setEnableWait(new Func<bool>(battleUnitParts.checkEffectCompleted));
      }
      this.btm.setScheduleAction((Action) (() =>
      {
        foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
          this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(false);
      }));
    }), isInsertMode: true);
  }

  private void executeCallEntryEffects()
  {
    if (this.env.core.phaseState.absoluteTurnCount != 1)
      return;
    int num = this.env.core.playerUnits.value[0].isPlayerForce ? 1 : 0;
    BL.Unit[] array1 = (num != 0 ? (IEnumerable<BL.Unit>) this.env.core.enemyUnits.value : (IEnumerable<BL.Unit>) this.env.core.playerUnits.value).Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.isCallEntryReserve && x.hp >= 1 && x.isEnable)).ToArray<BL.Unit>();
    bool flag = false;
    if (((IEnumerable<BL.Unit>) array1).Any<BL.Unit>())
    {
      this.btm.setTargetPanel(this.env.core.getFieldPanel(this.env.core.getUnitPosition(array1[0])), 0.0f, isWaitCameraMove: true);
      this.SetCallEntrySchedule(array1);
      flag = true;
    }
    List<BL.Unit> source = num != 0 ? this.env.core.playerUnits.value : this.env.core.enemyUnits.value;
    BL.Unit[] array2 = source.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.isCallEntryReserve && x.hp >= 1 && x.isEnable)).ToArray<BL.Unit>();
    if (((IEnumerable<BL.Unit>) array2).Any<BL.Unit>())
    {
      this.btm.setTargetPanel(this.env.core.getFieldPanel(this.env.core.getUnitPosition(array2[0])), 0.0f, isWaitCameraMove: true);
      this.SetCallEntrySchedule(array2);
    }
    else
    {
      if (!flag)
        return;
      this.btm.setTargetPanel(this.env.core.getFieldPanel(this.env.core.getUnitPosition(source[0])), 0.0f, isWaitCameraMove: true);
    }
  }

  private void SetCallEntrySchedule(BL.Unit[] callUnitUps)
  {
    this.btm.setScheduleAction((Action) (() =>
    {
      foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
        this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(true);
      List<BattleUnitParts> source = new List<BattleUnitParts>();
      foreach (BL.Unit callUnitUp in callUnitUps)
      {
        BattleUnitParts unitParts = this.env.unitResource[callUnitUp].unitParts_;
        this.btm.setScheduleAction((Action) (() => unitParts.callEntry()));
        source.Add(unitParts);
        callUnitUp.isCallEntryReserve = false;
      }
      if (source.Any<BattleUnitParts>())
      {
        foreach (BattleUnitParts battleUnitParts in source)
          this.btm.setEnableWait(new Func<bool>(battleUnitParts.checkEffectCompleted));
      }
      this.btm.setScheduleAction((Action) (() =>
      {
        foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
          this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(false);
      }));
    }), isInsertMode: true);
  }

  private void executeTurnInitSkillEffects(List<BL.UnitPosition> upl, int turn)
  {
    bool isWait = false;
    this.btm.setScheduleAction((Action) (() =>
    {
      foreach (BL.UnitPosition up in upl)
      {
        foreach (BL.ExecuteSkillEffectResult turnInitSkillEffect in this.env.core.executeTurnInitSkillEffects(up, turn))
        {
          if (turnInitSkillEffect.targets.Count > 0)
          {
            this.doExecuteSkillEffects(up, turnInitSkillEffect);
            isWait = true;
          }
        }
      }
    }));
    this.btm.setScheduleAction((Action) (() =>
    {
      List<BattleUnitParts> source = new List<BattleUnitParts>();
      foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
      {
        BattleUnitParts unitParts = this.env.unitResource[unitPosition.unit].unitParts_;
        unitParts.SetEffectMode(true);
        if (!unitParts.checkEffectCompleted())
          source.Add(unitParts);
      }
      float wait = 0.0f;
      XorShift random = this.battleManager.isPvp ? this.pvpManager.random : (XorShift) null;
      foreach (BL.UnitPosition up in upl)
      {
        foreach (BL.ExecuteSkillEffectResult es in this.env.core.executeTurnInitSkillEffects2(up, turn, random).Item1)
        {
          this.doExecuteSkillEffects(up, es);
          this.btm.setScheduleAction((Action) null, 0.5f);
          wait = 1f;
        }
      }
      if (source.Any<BattleUnitParts>())
      {
        foreach (BattleUnitParts battleUnitParts in source)
        {
          BattleUnitParts bup = battleUnitParts;
          this.btm.setScheduleAction((Action) null, comleteCheckFunc: (Func<bool>) (() => bup.checkEffectCompleted()));
        }
      }
      this.btm.setScheduleAction((Action) (() =>
      {
        foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
          this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(false);
      }), wait);
    }), isInsertMode: true);
    if (!Singleton<NGBattleManager>.GetInstance().isOvo)
      this.btm.setScheduleAction((Action) (() =>
      {
        List<BattleUnitParts> source = new List<BattleUnitParts>();
        foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
        {
          BattleUnitParts unitParts = this.env.unitResource[unitPosition.unit].unitParts_;
          unitParts.SetEffectMode(true);
          if (!unitParts.checkEffectCompleted())
            source.Add(unitParts);
        }
        List<List<BL.ExecuteSkillEffectResult>> result;
        List<BL.UnitPosition> unitPositionList = this.env.core.executeTurnSkillEffects(out result);
        float wait = 0.0f;
        BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(this);
        for (int index = 0; index < unitPositionList.Count; ++index)
        {
          BL.UnitPosition up = unitPositionList[index];
          foreach (BL.ExecuteSkillEffectResult es in result[index])
          {
            if (es.targets.Count > 0 || es.targetPanels.Count > 0)
            {
              facilitySkillDeads.Add(es);
              this.doExecuteFacilitySkillEffects(up, es);
              this.btm.setScheduleAction((Action) null, 0.5f);
              wait = 1f;
            }
          }
        }
        facilitySkillDeads.Execute();
        if (source.Any<BattleUnitParts>())
        {
          foreach (BattleUnitParts battleUnitParts in source)
          {
            BattleUnitParts bup = battleUnitParts;
            this.btm.setScheduleAction((Action) null, comleteCheckFunc: (Func<bool>) (() => bup.checkEffectCompleted()));
          }
        }
        this.btm.setScheduleAction((Action) (() =>
        {
          foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
            this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(false);
        }), wait);
      }), isInsertMode: true);
    if (isWait)
      this.btm.setEnableWait(0.1f);
    else
      this.btm.setScheduleAction((Action) (() => this.battleManager.isBattleEnable = true));
  }

  private void doExecuteSkillEffects(BL.UnitPosition up, BL.ExecuteSkillEffectResult es)
  {
    List<BL.Unit> targets = new List<BL.Unit>();
    foreach (BL.UnitPosition target in es.targets)
    {
      targets.Add(target.unit);
      if (this.battleManager.useGameEngine)
        this.battleManager.gameEngine.applyDeadUnit(target.unit, (BL.Unit) null);
    }
    bool isRebirth = ((IEnumerable<BattleskillEffect>) es.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.self_rebirth));
    bool isRebirth2 = ((IEnumerable<BattleskillEffect>) es.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.self_rebirth2));
    BattleskillFieldEffect passiveEffect = es.skill.passive_effect;
    GameObject effectPrefab = (GameObject) null;
    GameObject targetEffectPrefab = this.healPrefab;
    GameObject invokedEffectPrefab = (GameObject) null;
    if (passiveEffect != null)
    {
      BE.SkillResource skillResource = this.env.skillResource[es.skill.passive_effect.ID];
      effectPrefab = skillResource.effectPrefab;
      invokedEffectPrefab = skillResource.invokedEffectPrefab;
      targetEffectPrefab = skillResource.targetEffectPrefab;
    }
    this.battleManager.battleEffects.skillFieldEffectStartCore(passiveEffect, up.unit, targets, effectPrefab, invokedEffectPrefab, targetEffectPrefab, (Action) (() =>
    {
      if (isRebirth2)
      {
        up.unit.rebirthBE(this.env);
        this.env.unitResource[up.unit].unitParts_.setHpGauge(up.unit.hp);
      }
      else
      {
        for (int index = 0; index < es.targets.Count; ++index)
        {
          if (isRebirth)
            es.targets[index].unit.rebirthBE(this.env);
          else
            this.dispHpNumberAnime(es.targets[index].unit, es.target_prev_hps[index], es.target_hps[index]);
        }
      }
    }), (Action) null, (List<BL.Unit>) null);
  }

  public void doExecuteFacilitySkillEffects(
    BL.UnitPosition up,
    BL.ExecuteSkillEffectResult es,
    List<BL.Unit> invokedTargetUnits = null,
    Action invokedAction = null,
    List<Quaternion?> invokedEffectRotate = null)
  {
    List<BL.Unit> tl = es.targets.Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToList<BL.Unit>();
    BattleskillFieldEffect passiveEffect = es.skill.passive_effect;
    GameObject gameObject1 = (GameObject) null;
    GameObject gameObject2 = this.healPrefab;
    GameObject gameObject3 = (GameObject) null;
    if (passiveEffect != null)
    {
      BE.SkillResource skillResource = this.env.skillResource[es.skill.passive_effect.ID];
      gameObject1 = skillResource.effectPrefab;
      gameObject3 = skillResource.invokedEffectPrefab;
      gameObject2 = skillResource.targetEffectPrefab;
    }
    BattleEffects battleEffects = this.battleManager.battleEffects;
    BattleskillFieldEffect fe = passiveEffect;
    BL.Unit unit1 = up.unit;
    List<BL.Unit> targets = tl;
    GameObject effectPrefab = gameObject1;
    GameObject invokedEffectPrefab = gameObject3;
    GameObject targetEffectPrefab = gameObject2;
    List<BL.Unit> invokedTargetUnits1 = invokedTargetUnits;
    Action<BL.Unit> targetEndAction = (Action<BL.Unit>) (targetUnit =>
    {
      int index = tl.IndexOf(targetUnit);
      if (index < 0 || !es.disp_target_hps[index])
        return;
      BL.Unit unit2 = es.targets[index].unit;
      if (!unit2.isView)
        return;
      BE.UnitResource unitResource = this.env.unitResource[unit2];
      unitResource.unitParts_.dispHpNumber(es.target_prev_hps[index], es.target_hps[index]);
      unitResource.unitParts_.setHpGauge(es.target_prev_hps[index], es.target_hps[index]);
    });
    Action action = invokedAction;
    List<Quaternion?> invokedEffectRotate1 = invokedEffectRotate;
    Action invokedAction1 = action;
    List<BL.Panel> targetPanels = es.targetPanels;
    battleEffects.skillFieldEffectStartCore(fe, unit1, targets, effectPrefab, invokedEffectPrefab, targetEffectPrefab, (Action) null, (Action) null, invokedTargetUnits1, targetEndAction, 1, invokedEffectRotate1, invokedAction1, targetPanels);
  }

  private void completedSkillEffects(BL.UnitPosition up)
  {
    HashSet<BL.ISkillEffectListUnit> deads = new HashSet<BL.ISkillEffectListUnit>();
    List<BL.ExecuteSkillEffectResult> esrl = this.env.core.completedExecuteSkillEffects(up, deads);
    Action effectAction = (Action) (() =>
    {
      bool flag = esrl.Any<BL.ExecuteSkillEffectResult>((Func<BL.ExecuteSkillEffectResult, bool>) (x => x.targets.Count > 0));
      Battle01SelectNode.MaskContinuer mc = (Battle01SelectNode.MaskContinuer) null;
      if (flag)
        mc = this.uiNode.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
      if (esrl.Any<BL.ExecuteSkillEffectResult>((Func<BL.ExecuteSkillEffectResult, bool>) (x => x.targets.Count > 0)) && this.battleManager.useGameEngine)
      {
        this.battleManager.gameEngine.applyDeadUnit(up.unit, (BL.Unit) null);
        foreach (BL.Unit attack in deads.Select<BL.ISkillEffectListUnit, BL.Unit>((Func<BL.ISkillEffectListUnit, BL.Unit>) (x => x.originalUnit)))
        {
          if (attack != up.unit)
            this.battleManager.gameEngine.applyDeadUnit(attack, (BL.Unit) null);
        }
      }
      foreach (BL.ExecuteSkillEffectResult es in esrl)
      {
        if (es.targets.Count > 0)
          this.doExecuteCompletedSkillEffects(up, es);
      }
      if (!flag)
        return;
      this.uiNode.setMaskActive(false, mc);
    });
    if (!this.battleManager.isPvp)
    {
      effectAction();
    }
    else
    {
      Action waitAction = (Action) null;
      waitAction = (Action) (() =>
      {
        if (!this.battleManager.isAfterDuelEffectWaiting)
          effectAction();
        else
          this.btm.setScheduleAction((Action) (() => waitAction()));
      });
      waitAction();
    }
  }

  private void doExecuteCompletedSkillEffects(BL.UnitPosition up, BL.ExecuteSkillEffectResult es)
  {
    List<BL.Unit> targets = new List<BL.Unit>();
    foreach (BL.UnitPosition target in es.targets)
      targets.Add(target.unit);
    if (es.skill.skill_type == BattleskillSkillType.ailment)
    {
      this.btm.setTargetUnit(up, 0.0f, isWaitCameraMove: true);
      this.btm.setTargetUnit(up, 1.3f, func: (Action) (() =>
      {
        for (int index = 0; index < es.targets.Count; ++index)
        {
          if (es.targets[index].unit.isView)
          {
            BE.UnitResource unitResource = this.env.unitResource[es.targets[index].unit];
            unitResource.unitParts_.dispHpNumber(es.target_prev_hps[index], es.target_hps[index]);
            unitResource.unitParts_.setHpGauge(es.target_hps[index], es.target_hps[index]);
          }
        }
      }));
      if (!es.second_targets.Any<BL.Unit>())
        return;
      Action<int> dispHpOne = (Action<int>) (idx =>
      {
        BL.Unit secondTarget = es.second_targets[idx];
        if (!secondTarget.isView)
          return;
        BE.UnitResource unitResource = this.env.unitResource[secondTarget];
        unitResource.unitParts_.dispHpNumber(es.second_target_prev_hps[idx], es.second_target_hps[idx]);
        unitResource.unitParts_.setHpGauge(es.second_target_hps[idx], es.second_target_hps[idx]);
      });
      Action targetAction = (Action) null;
      Action<BL.Unit> targetEndAction = (Action<BL.Unit>) null;
      BattleskillFieldEffect fe = es.skill.passive_effect;
      GameObject targetEffectPrefab = fe != null ? this.env.skillResource[fe.ID].targetEffectPrefab : (GameObject) null;
      if (Object.op_Inequality((Object) targetEffectPrefab, (Object) null))
      {
        targetEndAction = (Action<BL.Unit>) (targetUnit =>
        {
          int num = es.second_targets.IndexOf(targetUnit);
          if (num < 0)
            return;
          dispHpOne(num);
        });
      }
      else
      {
        fe = (BattleskillFieldEffect) null;
        targetEffectPrefab = this.healPrefab;
        targetAction = (Action) (() =>
        {
          for (int index = 0; index < es.second_targets.Count; ++index)
            dispHpOne(index);
        });
      }
      this.battleManager.battleEffects.skillFieldEffectStartCore(fe, (BL.Unit) null, es.second_targets, (GameObject) null, (GameObject) null, targetEffectPrefab, (Action) null, targetAction, (List<BL.Unit>) null, targetEndAction);
      if (!es.second_targets.Any<BL.Unit>((Func<BL.Unit, bool>) (x => x.hp <= 0)))
        return;
      this.btm.setScheduleAction((Action) null, 1.5f);
    }
    else
    {
      BattleskillFieldEffect passiveEffect = es.skill.passive_effect;
      GameObject effectPrefab = (GameObject) null;
      GameObject targetEffectPrefab = this.healPrefab;
      GameObject invokedEffectPrefab = (GameObject) null;
      if (passiveEffect != null)
      {
        BE.SkillResource skillResource = this.env.skillResource[es.skill.passive_effect.ID];
        effectPrefab = skillResource.effectPrefab;
        targetEffectPrefab = skillResource.targetEffectPrefab;
        invokedEffectPrefab = skillResource.invokedEffectPrefab;
      }
      this.battleManager.battleEffects.skillFieldEffectStartCore(passiveEffect, (BL.Unit) null, targets, effectPrefab, invokedEffectPrefab, targetEffectPrefab, (Action) null, (Action) (() =>
      {
        for (int index = 0; index < es.targets.Count; ++index)
          this.dispHpNumberAnime(es.targets[index].unit, es.target_prev_hps[index], es.target_hps[index]);
      }), (List<BL.Unit>) null);
    }
  }

  private void completedPositionSkillEffects(BL.UnitPosition up)
  {
    List<List<BL.ExecuteSkillEffectResult>> esrll;
    List<BL.UnitPosition> fupl = this.env.core.completedPositionExecuteSkillEffects(up, out esrll);
    Action effectAction = (Action) (() =>
    {
      float wait = 0.0f;
      bool flag = false;
      Battle01SelectNode.MaskContinuer mc = (Battle01SelectNode.MaskContinuer) null;
      if (esrll != null && esrll.Any<List<BL.ExecuteSkillEffectResult>>((Func<List<BL.ExecuteSkillEffectResult>, bool>) (x => x.Any<BL.ExecuteSkillEffectResult>((Func<BL.ExecuteSkillEffectResult, bool>) (y => y.targets.Count > 0)))))
      {
        flag = true;
        mc = this.uiNode.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
        this.btm.setScheduleAction((Action) (() =>
        {
          foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
            this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(true);
        }));
      }
      BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(this);
      int num = 0;
      foreach (BL.UnitPosition up1 in fupl)
      {
        foreach (BL.ExecuteSkillEffectResult es in esrll[num++])
        {
          if (es.targets.Count > 0 || es.targetPanels.Count > 0)
          {
            facilitySkillDeads.Add(es);
            this.doExecuteFacilitySkillEffects(up1, es);
            this.btm.setScheduleAction((Action) null, 0.5f);
            wait = 1f;
          }
        }
      }
      facilitySkillDeads.Execute();
      if (!flag)
        return;
      foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
      {
        BattleUnitParts unitParts = this.env.unitResource[unitPosition.unit].unitParts_;
        if (!unitParts.IsEffectMode())
          unitParts.SetEffectMode(true);
      }
      this.btm.setScheduleAction((Action) null, wait, (Action) (() =>
      {
        foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
          this.env.unitResource[unitPosition.unit].unitParts_.SetEffectMode(false);
      }));
      this.uiNode.setMaskActive(false, mc);
    });
    if (!this.battleManager.isPvp)
    {
      effectAction();
    }
    else
    {
      Action waitAction = (Action) null;
      waitAction = (Action) (() =>
      {
        if (!this.battleManager.isAfterDuelEffectWaiting)
          effectAction();
        else
          this.btm.setScheduleAction((Action) (() => waitAction()));
      });
      waitAction();
    }
  }

  private void setStateWithEffect(
    string effect,
    float time,
    BL.Phase state,
    Action endAction,
    bool isBattleEnableControl,
    bool isSkippedVoiceStop = false,
    Action<ScheduleEnumerator> onClickMask = null)
  {
    this.btm.setSchedule((Schedule) new BattleStateController.EffectAIWait(this));
    this.battleManager.battleEffects.startEffect(effect, time, (Action) (() =>
    {
      if (endAction != null)
        endAction();
      this.btm.setPhaseState(state);
    }), isBattleEnableControl, isSkippedVoiceStop, onClickMask);
  }

  private void SkipEffect(ScheduleEnumerator scheduleEnumerator)
  {
    if (!(scheduleEnumerator is BattleEffects.StartEffect startEffect) || (double) startEffect.deltaTime < 2.0)
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!instance.IsVoiceStopAll())
      instance.stopVoice();
    startEffect.StopEffect();
  }

  private bool deadCheck()
  {
    if (this.env.core.condition.type == BL.ConditionType.bossdown && this.env.core.getBossUnit().isDead)
    {
      this.btm.setPhaseState(this.battleManager.isWave ? BL.Phase.stageclear_pre : BL.Phase.stageclear, true);
      return true;
    }
    if (this.env.core.getGamevoerType() != BL.GameoverType.alldown)
    {
      if (this.env.core.getGamevoerType() == BL.GameoverType.guestdown)
      {
        if (this.env.core.playerUnits.value.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.playerUnit.is_guest && !x.isDead)) == 0)
        {
          this.btm.setPhaseState(BL.Phase.all_dead_player);
          return true;
        }
      }
      else if (this.env.core.getGamevoerType() == BL.GameoverType.playerdown && this.env.core.playerUnits.value.Count<BL.Unit>((Func<BL.Unit, bool>) (x => !x.playerUnit.is_guest && !x.isDead)) == 0)
      {
        this.btm.setPhaseState(BL.Phase.all_dead_player);
        return true;
      }
    }
    if (this.env.core.getLoseUnitList().Count > 0 && this.env.core.getLoseUnitList().Any<BL.Unit>((Func<BL.Unit, bool>) (x => x.isDead)))
    {
      this.btm.setPhaseState(BL.Phase.gameover, true);
      return true;
    }
    if (this.env.core.battleInfo.isEarthMode)
    {
      if (!this.env.core.battleInfo.isExtraEarthQuest)
      {
        BL.Unit unit = this.env.core.playerUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.is_leader)).FirstOrDefault<BL.Unit>();
        if (unit != (BL.Unit) null && unit.isDead)
        {
          this.btm.setPhaseState(BL.Phase.gameover, true);
          return true;
        }
      }
      else
      {
        BL.Unit unit = this.env.core.playerUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.unit.same_character_id == 11002)).FirstOrDefault<BL.Unit>();
        if (unit != (BL.Unit) null && unit.isDead)
        {
          this.btm.setPhaseState(BL.Phase.gameover, true);
          return true;
        }
      }
    }
    int num = this.env.core.allDeadUnitsp(BL.ForceID.player) ? 1 : 0;
    bool flag = this.env.core.allDeadUnitsp(BL.ForceID.enemy);
    if (num != 0 && !flag)
    {
      this.btm.setPhaseState(BL.Phase.all_dead_player);
      return true;
    }
    if (!flag || Singleton<NGBattleManager>.GetInstance().checkReinforceUnitForSmash())
      return false;
    this.btm.setPhaseState(BL.Phase.all_dead_enemy);
    return true;
  }

  private bool checkCharmOnly(List<BL.UnitPosition> l)
  {
    if (l.Count == 0)
      return false;
    foreach (BL.UnitPosition unitPosition in l)
    {
      if (!unitPosition.unit.IsCharm)
        return false;
    }
    return true;
  }

  private void updateNormal()
  {
    if (!this.battleManager.isBattleEnable)
      return;
    switch (this.phaseStateModified.value.state)
    {
      case BL.Phase.player:
        if (this.btm.isRunning || this.deadCheck())
          break;
        if (this.env.core.playerActionUnits.value.Count == 0)
        {
          this.env.core.playerActionUnits.commit();
          break;
        }
        bool isCharm1 = this.checkCharmOnly(this.env.core.playerActionUnits.value);
        if (isCharm1)
        {
          if (this.aiController.isCompleted)
          {
            this.aiController.startAIAction();
            break;
          }
          this.aiController.startAI(this.env.core.playerActionUnits.value, isCharm1);
          break;
        }
        if (this.isAutoBattleModified.isChangedOnce())
        {
          if (this.isAutoBattleModified.value.value)
          {
            if (this.env.core.unitCurrent.unit != (BL.Unit) null)
              this.env.core.currentUnitPosition.cancelMove(this.env);
            if (this.aiController.startAI(this.env.core.playerActionUnits.value, isCharm1))
              break;
            this.isAutoBattleModified.notifyChanged();
            break;
          }
          this.aiController.stopAIAction();
          if (!this.aiController.isAction)
            break;
          this.startWaitCurrentAIActionCancel();
          break;
        }
        if (!this.env.core.isAutoBattle.value || !this.aiController.isCompleted)
          break;
        this.aiController.startAIAction();
        break;
      case BL.Phase.neutral:
        if (this.btm.isRunning || this.deadCheck())
          break;
        if (this.env.core.neutralActionUnits.value.Count == 0)
        {
          this.env.core.neutralActionUnits.commit();
          break;
        }
        bool isCharm2 = this.checkCharmOnly(this.env.core.neutralActionUnits.value);
        if (this.aiController.isCompleted)
        {
          this.aiController.startAIAction();
          break;
        }
        this.aiController.startAI(this.env.core.neutralActionUnits.value, isCharm2);
        break;
      case BL.Phase.enemy:
        if (this.btm.isRunning || this.deadCheck())
          break;
        if (this.env.core.enemyActionUnits.value.Count == 0)
        {
          this.env.core.enemyActionUnits.commit();
          break;
        }
        bool isCharm3 = this.checkCharmOnly(this.env.core.enemyActionUnits.value);
        if (this.aiController.isCompleted)
        {
          this.aiController.startAIAction();
          break;
        }
        this.aiController.startAI(this.env.core.enemyActionUnits.value, isCharm3);
        break;
    }
  }

  private void startWaitCurrentAIActionCancel()
  {
    string str = "doWaitCurrentAIActionCancel";
    this.StopCoroutine(str);
    this.StartCoroutine(str);
  }

  private IEnumerator doWaitCurrentAIActionCancel()
  {
    this.isWaitCurrentAIActionCancel = true;
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = true;
    do
    {
      yield return (object) null;
    }
    while (this.aiController.isAction);
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = false;
    this.isWaitCurrentAIActionCancel = false;
  }

  private void updateOVO()
  {
    switch (this.phaseStateModified.value.state)
    {
      case BL.Phase.player:
        if (this.lastCompletedUnit != null)
        {
          this.lastCompletedUnit.clearMovePanelCache();
          this.lastCompletedUnit = (BL.UnitPosition) null;
        }
        bool isCharm = this.checkCharmOnly(this.env.core.playerActionUnits.value);
        if (isCharm)
        {
          if (!this.aiController.startAI(this.env.core.playerActionUnits.value, isCharm, 1))
            break;
          this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
          break;
        }
        bool flag = this.battleManager.isPvp && this.env.core.playerActionUnits.value.Count == 0 && this.aiController.isCompleted;
        if (this.isAutoBattleModified.isChangedOnce() | flag && (!this.battleManager.isPvp || !this.battleManager.isPvnpc || PerformanceConfig.GetInstance().EnablePvPAutoButton))
        {
          if (this.isAutoBattleModified.value.value | flag)
          {
            if (this.env.core.unitCurrent.unit != (BL.Unit) null)
              this.env.core.currentUnitPosition.cancelMove(this.env);
            if (this.battleManager.isPvp)
            {
              if (this.isAutoBattleModified.value.value)
                this.battleManager.gameEngine.autoOnRequest();
              this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
              break;
            }
            if (!this.aiController.startAI(this.env.core.playerActionUnits.value, isCharm, 1))
              break;
            if (this.battleManager.isGvg || this.battleManager.isPvnpc)
              this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
            this.isAutoBattleModified.notifyChanged();
            break;
          }
          this.aiController.stopAIAction();
          if (!this.aiController.isAction)
            break;
          this.startWaitCurrentAIActionCancel();
          break;
        }
        if (!this.env.core.isAutoBattle.value || !this.aiController.isCompleted)
          break;
        if (this.battleManager.isPvp)
        {
          this.battleManager.gameEngine.autoOnRequest();
          this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
          break;
        }
        this.aiController.startAIAction(0.0f, (Action) (() => this.battleManager.gameEngine.actionUnitCompleted()));
        break;
      case BL.Phase.enemy:
        if (this.aiController.isCompleted)
        {
          this.aiController.startAIAction(0.0f, (Action) (() => this.battleManager.gameEngine.actionUnitCompleted()));
          break;
        }
        if (!this.battleManager.isPvnpc && !this.battleManager.isGvg)
          break;
        this.aiController.startAI(this.env.core.enemyActionUnits.value, this.checkCharmOnly(this.env.core.enemyActionUnits.value), 1);
        break;
      case BL.Phase.pvp_move_unit_waiting:
        if (!this.aiController.isCompleted)
          break;
        this.aiController.startAIAction(0.0f, (Action) (() => this.battleManager.gameEngine.actionUnitCompleted()));
        break;
    }
  }

  protected override void Update_Battle()
  {
    if (this.battleManager.isOvo)
      this.updateOVO();
    else
      this.updateNormal();
  }

  private void spawnUnit(BL.UnitPosition up)
  {
    up.unit.isSpawned = true;
    this.btm.setTargetPanel(this.env.core.getFieldPanel(up), 0.3f, (Action) (() => this.battleManager.battleEffects.fieldEffectsStart(this.spawnEffectPrefab, up.unit)), (Action) (() => up.unit.spawn(this.env, true)));
    this.btm.setScheduleAction((Action) null, 0.6f);
  }

  private void dispHpNumberAnime(BL.Unit unit, int prev_hp, int new_hp)
  {
    if (!unit.isView)
      return;
    this.env.unitResource[unit].unitParts_.dispHpNumber(prev_hp, new_hp);
  }

  private bool execPanelLandformIncr(BL.Panel panel, BL.UnitPosition up)
  {
    BattleLandformIncr incr = panel.landform.GetIncr(up.unit);
    if (!up.unit.isEnable || up.unit.isDead || up.unit.hp == up.unit.parameter.Hp || (double) incr.hp_healing_ratio <= 0.0)
      return false;
    this.btm.setTargetUnit(up, 0.0f, this.healPrefab, endAction: (Action) (() => up.unit.hp += Mathf.CeilToInt((float) up.unit.parameter.Hp * incr.hp_healing_ratio)), isWaitCameraMove: true);
    this.btm.setScheduleAction((Action) null, 0.5f);
    return true;
  }

  private bool execPanelLandformEffect(
    BL.Panel panel,
    BL.UnitPosition up,
    BattleLandformEffectPhase phase)
  {
    if (up.unit.isFacility || up.unit.IsJumping)
      return false;
    this.btm.setScheduleAction((Action) (() =>
    {
      BattleLandformIncr incr = panel.landform.GetIncr(up.unit);
      IEnumerable<BattleLandformEffect> landformEffects = incr.GetLandformEffects(phase);
      List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
      BattleFuncs.GetLandBlessingSkillAdd(skillParams, (BL.ISkillEffectListUnit) up.unit, (BL.ISkillEffectListUnit) null, BattleskillEffectLogicEnum.land_blessing_fix_hp_healing, panel.landform, phase);
      BattleFuncs.GetLandBlessingSkillMul(skillParams, (BL.ISkillEffectListUnit) up.unit, (BL.ISkillEffectListUnit) null, BattleskillEffectLogicEnum.land_blessing_ratio_hp_healing, panel.landform, phase);
      int num1 = BattleFuncs.calcSkillParamAdd(skillParams);
      if (!landformEffects.Any<BattleLandformEffect>() && num1 == 0 || !up.unit.isEnable || up.unit.isDead)
        return;
      float num2 = landformEffects.Where<BattleLandformEffect>((Func<BattleLandformEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.fix_heal)).Sum<BattleLandformEffect>((Func<BattleLandformEffect, float>) (x => x.GetFloat(BattleskillEffectLogicArgumentEnum.value)));
      float num3 = landformEffects.Where<BattleLandformEffect>((Func<BattleLandformEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.ratio_heal)).Sum<BattleLandformEffect>((Func<BattleLandformEffect, float>) (x => x.GetFloat(BattleskillEffectLogicArgumentEnum.percentage)));
      float num4 = landformEffects.Where<BattleLandformEffect>((Func<BattleLandformEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.fix_damage)).Sum<BattleLandformEffect>((Func<BattleLandformEffect, float>) (x => x.GetFloat(BattleskillEffectLogicArgumentEnum.value)));
      int damage = Mathf.CeilToInt((float) up.unit.parameter.Hp * landformEffects.Where<BattleLandformEffect>((Func<BattleLandformEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.ratio_damage)).Sum<BattleLandformEffect>((Func<BattleLandformEffect, float>) (x => x.GetFloat(BattleskillEffectLogicArgumentEnum.percentage))) + num4);
      int num5 = Mathf.Clamp(up.unit.hp + Mathf.CeilToInt((float) up.unit.parameter.Hp * num3 * BattleFuncs.calcSkillParamMul(skillParams) + num2 + (float) num1) - damage, 0, up.unit.parameter.Hp);
      int num6 = BattleFuncs.applyDamageCut(3, damage, (BL.ISkillEffectListUnit) up.unit, invokePanel: panel);
      List<BattleFuncs.SkillParam> skillParamList = new List<BattleFuncs.SkillParam>();
      int off_healswap_damage = num6;
      foreach (BattleFuncs.SkillParam skillParam in skillParams)
      {
        if (BattleFuncs.getHealSwapEffects((BL.ISkillEffectListUnit) up.unit, panel, skillParam.effect.baseSkill.skill_type).Any<BL.SkillEffect>())
          skillParamList.Add(skillParam);
      }
      if (BattleFuncs.getHealSwapEffects((BL.ISkillEffectListUnit) up.unit, panel).Any<BL.SkillEffect>())
      {
        num6 += (int) Math.Ceiling((Decimal) up.unit.parameter.Hp * (Decimal) num3 * (Decimal) BattleFuncs.calcSkillParamMul(skillParamList) + (Decimal) num2 + (Decimal) BattleFuncs.calcSkillParamAdd(skillParamList));
        num2 = 0.0f;
        num3 = 0.0f;
      }
      else
      {
        if (skillParamList.Any<BattleFuncs.SkillParam>())
        {
          int num7 = Mathf.CeilToInt((float) up.unit.parameter.Hp * num3 + num2);
          int num8 = Mathf.CeilToInt((float) up.unit.parameter.Hp * num3 * BattleFuncs.calcSkillParamMul(skillParamList) + num2 + (float) BattleFuncs.calcSkillParamAdd(skillParamList));
          if (num8 > num7)
            num6 += num8 - num7;
        }
        if (!up.unit.CanHeal())
        {
          num2 = 0.0f;
          num3 = 0.0f;
        }
      }
      skillParamList.Clear();
      foreach (BattleFuncs.SkillParam skillParam in skillParams)
      {
        if (!BattleFuncs.getHealSwapEffects((BL.ISkillEffectListUnit) up.unit, panel, skillParam.effect.baseSkill.skill_type).Any<BL.SkillEffect>() && up.unit.CanHeal(skillParam.effect.baseSkill.skill_type))
          skillParamList.Add(skillParam);
      }
      int newChangeHp = Mathf.Clamp(up.unit.hp + Mathf.CeilToInt((float) up.unit.parameter.Hp * num3 * BattleFuncs.calcSkillParamMul(skillParamList) + num2 + (float) BattleFuncs.calcSkillParamAdd(skillParamList)) - num6, 0, up.unit.parameter.Hp);
      off_healswap_damage = num6 > off_healswap_damage ? num6 - off_healswap_damage : 0;
      if (num5 == up.unit.hp && newChangeHp == up.unit.hp)
        return;
      int prevHp = up.unit.hp;
      this.btm.setTargetUnit(up, 0.0f, isWaitCameraMove: true);
      this.btm.setTargetUnit(up, 0.0f, incr.effect_group == null || !this.panelEffectPrefabs.ContainsKey(incr.effect_group.play_prefab_file_name) ? (GameObject) null : this.panelEffectPrefabs[incr.effect_group.play_prefab_file_name], (Action) (() =>
      {
        up.unit.hp = newChangeHp;
        this.dispHpNumberAnime(up.unit, prevHp, newChangeHp);
        up.unit.addReceivedLandformDamage(Mathf.Max(0, prevHp - up.unit.hp - off_healswap_damage));
      }));
      this.btm.setEnableWait(1.3f);
      this.btm.setScheduleAction((Action) (() =>
      {
        if (this.battleManager.useGameEngine)
          this.battleManager.gameEngine.applyDeadUnit(up.unit, (BL.Unit) null);
        this.uiNode.hpCheckWithDeadEffects(up.unit, true);
      }), isInsertMode: true);
    }), isInsertMode: true);
    return false;
  }

  private void startBattleStartEffect()
  {
    BL.Story story = this.env.core.getFirstTurnStart();
    bool isBattleEnableControl = story != null && story.scriptId >= 0;
    ConditionForVictory conditionForVictory = this.battleManager.battleEffects.getConditionForVictory("Condition_forVictory");
    if (Object.op_Inequality((Object) conditionForVictory, (Object) null))
    {
      int wave = 0;
      int maxWave = 0;
      if (this.battleManager.isWave)
      {
        wave = this.env.core.currentWave + 1;
        maxWave = this.battleManager.waveLength;
      }
      conditionForVictory.Initialize(this.env.core.condition.condition, wave, maxWave);
    }
    this.battleManager.battleEffects.startEffect("Condition_forVictory", 2.8f, (Action) (() => this.startStroyWithNextState(story, BL.Phase.turn_initialize)), isBattleEnableControl, true);
  }

  private void startBattlePvpStartEffect()
  {
    this.battleManager.battleEffects.startEffect((Transform) null, 5f, (Action) (() => this.battleManager.gameEngine.startMain()), true, this.pvpMatchStartPrefab, cloneE: (BattleEffects.CloneEnumlator) new BattleStateController.PvpMatchEffect(this));
  }

  private void startBattleFieldEffect(BL.FieldEffectType type)
  {
    this.btm.setScheduleAction((Action) (() =>
    {
      foreach (BL.FieldEffect fieldEffect in this.env.core.getFieldEffects(type))
        this.battleManager.battleEffects.startEffect(fieldEffect);
    }), isInsertMode: true);
  }

  private void battleStartOvo()
  {
    this.btm.setScheduleAction((Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      BattleCameraFilter.Active();
      this.battleManager.gameEngine.readyComplited();
    }));
  }

  private void battleStartNormal()
  {
    this.btm.setScheduleAction((Action) (() =>
    {
      BL.Story story = this.env.core.getStory(BL.StoryType.battle_start);
      if (story == null || story.scriptId == 0)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        BattleCameraFilter.Active();
      }
      this.startBattleFieldEffect(BL.FieldEffectType.battle_start);
      this.startStroyWithNextState(BL.StoryType.battle_start, BL.Phase.battle_start_init);
    }));
  }

  private float getEffectStageClearTime()
  {
    return !Singleton<NGGameDataManager>.GetInstance().questAutoLap ? 5f : 3f;
  }

  private bool isEffectStageClearSkippedVoiceStop()
  {
    return Singleton<NGGameDataManager>.GetInstance().questAutoLap;
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.battleManager.isPvp && !this.battleManager.isBattleEnable)
      return;
    if (!this.isStageClear && !this.isGameOver)
    {
      if (!this.battleManager.isOvo)
      {
        this.checkScriptUnitInArea();
        if (this.completedListModified.isChanged && this.completedListModified.value.value.Count > 0)
        {
          List<int> scripts = new List<int>();
          foreach (BL.UnitPosition unitPosition in this.completedListModified.value.value)
          {
            List<int> scripts1 = unitPosition.getScripts();
            if (scripts1 != null && scripts1.Count > 0)
            {
              scripts.AddRange((IEnumerable<int>) scripts1);
              unitPosition.resetScript();
            }
          }
          if (scripts.Count > 0)
          {
            this.btm.setScheduleAction((Action) (() =>
            {
              scripts.ForEach((Action<int>) (i => this.battleManager.startStory(new BL.Story(i, BL.StoryType.unit_in_area, (object[]) null))));
              this.btm.setPhaseState(this.env.core.phaseState.state);
            }));
            return;
          }
        }
      }
      if (this.completedListModified.isChangedOnce() && this.completedListModified.value.value.Count > 0)
      {
        foreach (BL.UnitPosition up in this.completedListModified.value.value)
        {
          if (!up.unit.isDead && up.unit.hp > 0)
          {
            BL.Phase changePhaseToPanel = this.env.core.getChangePhaseToPanel(up);
            if (changePhaseToPanel == BL.Phase.none)
            {
              if (!up.unit.mIsExecCompletedSkillEffect)
              {
                this.completedSkillEffects(up);
                this.completedPositionSkillEffects(up);
              }
              int[] ids = this.env.core.getReinforcementIDsToPanel(up);
              if (ids != null)
              {
                foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.playerUnit.reinforcement != null && !x.unit.isDead && !x.unit.isEnable && ((IEnumerable<int>) ids).Contains<int>(x.unit.playerUnit.reinforcement.ID))))
                {
                  if (!this.env.core.spawnUnits.value.Contains(unitPosition))
                  {
                    this.env.core.spawnUnits.value.Add(unitPosition);
                    this.env.core.spawnUnits.commit();
                  }
                }
              }
            }
            else
            {
              this.btm.setPhaseState(changePhaseToPanel, true);
              return;
            }
          }
        }
        this.lastCompletedUnit = this.completedListModified.value.value.Count > 0 ? this.completedListModified.value.value.Last<BL.UnitPosition>() : (BL.UnitPosition) null;
        this.completedListModified.value.value.Clear();
        this.completedListModified.commit();
      }
      if (this.spawnUnitListModified.isChangedOnce() && this.spawnUnitListModified.value.value.Count > 0)
        this.btm.setScheduleAction((Action) (() =>
        {
          this.battleManager.getController<BattleInputObserver>().isTouchEnable = false;
          this.isSpawning = true;
          foreach (BL.UnitPosition up in this.spawnUnitListModified.value.value)
            this.spawnUnit(up);
          this.btm.setScheduleAction((Action) (() =>
          {
            this.battleManager.getController<BattleInputObserver>().isTouchEnable = true;
            this.isSpawning = false;
          }));
          this.env.core.spawnUnits.value.Clear();
          this.env.core.spawnUnits.commit();
          if (Persist.tutorial.Data.IsFinishTutorial())
            return;
          this.btm.setScheduleAction((Action) null, 2f);
          this.btm.setScheduleAction((Action) (() =>
          {
            Singleton<TutorialRoot>.GetInstance().showAdvice(Consts.GetInstance().integralnoah_battle_tutorial3);
            Singleton<TutorialRoot>.GetInstance().isTutorialBattleFirstSpawn = true;
            this.btm.setEnableWait(new Func<bool>(Singleton<TutorialRoot>.GetInstance().checkAdviceCompleted));
          }));
        }));
      if (!this.battleManager.isOvo)
      {
        if (this.playerListModified.isChangedOnce())
        {
          this.btm.setScheduleAction((Action) (() =>
          {
            if (this.env.core.phaseState.state != BL.Phase.player)
              return;
            if (this.playerListModified.value.value.Count == 0)
            {
              this.btm.setPhaseState(BL.Phase.player_end);
            }
            else
            {
              if (!this.env.core.currentPhaseUnitp(this.env.core.unitCurrent.unit))
                return;
              this.btm.setCurrentUnit((BL.Unit) null);
            }
          }));
          return;
        }
        if (this.neutralListModified.isChangedOnce())
        {
          this.btm.setScheduleAction((Action) (() =>
          {
            if (this.env.core.phaseState.state != BL.Phase.neutral || this.neutralListModified.value.value.Count != 0)
              return;
            this.btm.setPhaseState(BL.Phase.neutral_end);
          }));
          return;
        }
        if (this.enemyListModified.isChangedOnce())
        {
          this.btm.setScheduleAction((Action) (() =>
          {
            if (this.env.core.phaseState.state != BL.Phase.enemy || this.enemyListModified.value.value.Count != 0)
              return;
            this.btm.setPhaseState(BL.Phase.enemy_end);
          }));
          return;
        }
      }
    }
    if (!this.phaseStateModified.isChangedOnce())
      return;
    switch (this.phaseStateModified.value.state)
    {
      case BL.Phase.player_start:
        this.battleManager.saveEnvironment();
        this.env.core.battleLogger.PlayerPhaseStart();
        this.aiController.stopAIAction();
        Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.env.core.stage.stage.field_player_bgm_file, this.env.core.stage.stage.field_player_bgm);
        this.startBattleFieldEffect(BL.FieldEffectType.player_start);
        if (this.env.core.playerActionUnits.value.Count > 0)
          this.setStartTarget(this.env.core.playerActionUnits.value[0], true);
        this.btm.setScheduleAction((Action) (() =>
        {
          this.setStateWithEffect("PlayerPhase", 1.5f, BL.Phase.player_start_post, (Action) null, true, true);
          this.isAutoBattleModified.notifyChanged();
        }));
        foreach (BL.UnitPosition up in this.env.core.playerActionUnits.value)
          this.execPanelLandformEffect(this.env.core.getFieldPanel(up), up, BattleLandformEffectPhase.phase_start);
        IEnumerable<BL.UnitPosition> second1 = this.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == BL.ForceID.player && x.isEnable && !x.isDead && !x.facility.isSkillUnit)).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (x => this.env.core.getUnitPosition(x)));
        IEnumerable<BL.UnitPosition> second2 = this.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == BL.ForceID.player && x.isEnable && !x.isDead && x.facility.isSkillUnit)).OrderBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.facilitySpawnOrder)).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (x => this.env.core.getUnitPosition(x)));
        this.executeSkillEffects(this.env.core.playerActionUnits.value.Concat<BL.UnitPosition>(second1).Concat<BL.UnitPosition>(second2).ToList<BL.UnitPosition>());
        this.executeJumpSkillEffects((IEnumerable<BL.UnitPosition>) this.env.core.unitPositions.value, BL.ForceID.player);
        this.btm.setScheduleAction((Action) (() =>
        {
          BL.UnitPosition[] array = this.env.core.playerActionUnits.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.IsJumping)).ToArray<BL.UnitPosition>();
          if (!((IEnumerable<BL.UnitPosition>) array).Any<BL.UnitPosition>())
            return;
          foreach (BL.UnitPosition unitPosition in array)
          {
            this.env.core.playerActionUnits.value.Remove(unitPosition);
            unitPosition.recoveryCompleteUnit();
          }
          this.env.core.playerActionUnits.commit();
        }), isInsertMode: true);
        this.btm.setScheduleAction((Action) (() => this.env.core.createDangerAria()));
        break;
      case BL.Phase.neutral_start:
        if (this.neutralListModified.value.value.Count == 0)
        {
          this.neutralListModified.notifyChanged();
          break;
        }
        if (this.isSpawning || this.spawnUnitListModified.value.value.Count >= 1)
        {
          this.phaseStateModified.notifyChanged();
          break;
        }
        this.aiController.stopAIAction();
        this.env.core.resetActionList(BL.ForceID.neutral);
        this.startBattleFieldEffect(BL.FieldEffectType.neutral_start);
        this.setStartTarget(this.env.core.neutralActionUnits.value[0], false);
        this.btm.setScheduleAction((Action) (() =>
        {
          this.battleManager.saveEnvironment();
          this.setStateWithEffect("NeutralPhase", 1.5f, BL.Phase.neutral_start_post, (Action) null, true, true);
        }));
        foreach (BL.UnitPosition up in this.env.core.neutralActionUnits.value)
          this.execPanelLandformEffect(this.env.core.getFieldPanel(up), up, BattleLandformEffectPhase.phase_start);
        IEnumerable<BL.UnitPosition> second3 = this.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == BL.ForceID.neutral && x.isEnable && !x.isDead && !x.facility.isSkillUnit)).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (x => this.env.core.getUnitPosition(x)));
        IEnumerable<BL.UnitPosition> second4 = this.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == BL.ForceID.neutral && x.isEnable && !x.isDead && x.facility.isSkillUnit)).OrderBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.facilitySpawnOrder)).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (x => this.env.core.getUnitPosition(x)));
        this.executeSkillEffects(this.env.core.neutralActionUnits.value.Concat<BL.UnitPosition>(second3).Concat<BL.UnitPosition>(second4).ToList<BL.UnitPosition>());
        this.executeJumpSkillEffects((IEnumerable<BL.UnitPosition>) this.env.core.unitPositions.value, BL.ForceID.neutral);
        this.btm.setScheduleAction((Action) (() =>
        {
          BL.UnitPosition[] array = this.env.core.neutralActionUnits.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.IsJumping)).ToArray<BL.UnitPosition>();
          if (!((IEnumerable<BL.UnitPosition>) array).Any<BL.UnitPosition>())
            return;
          foreach (BL.UnitPosition unitPosition in array)
          {
            this.env.core.neutralActionUnits.value.Remove(unitPosition);
            unitPosition.recoveryCompleteUnit();
          }
          this.env.core.neutralActionUnits.commit();
        }), isInsertMode: true);
        break;
      case BL.Phase.enemy_start:
        if (this.isSpawning || this.spawnUnitListModified.value.value.Count >= 1)
        {
          this.phaseStateModified.notifyChanged();
          break;
        }
        this.aiController.stopAIAction();
        this.env.core.resetActionList(BL.ForceID.enemy);
        Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.env.core.stage.stage.field_enemy_bgm_file, this.env.core.stage.stage.field_enemy_bgm);
        this.startBattleFieldEffect(BL.FieldEffectType.enemy_start);
        this.setStartTarget(this.env.core.enemyActionUnits.value[0], false);
        this.btm.setScheduleAction((Action) (() =>
        {
          this.battleManager.saveEnvironment();
          this.env.core.battleLogger.EnemyPhaseStart();
          this.setStateWithEffect("EnemyPhase", 1.5f, BL.Phase.enemy_start_post, (Action) null, true, true);
        }));
        foreach (BL.UnitPosition up in this.env.core.enemyActionUnits.value)
          this.execPanelLandformEffect(this.env.core.getFieldPanel(up), up, BattleLandformEffectPhase.phase_start);
        IEnumerable<BL.UnitPosition> second5 = this.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == BL.ForceID.enemy && x.isEnable && !x.isDead && !x.facility.isSkillUnit)).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (x => this.env.core.getUnitPosition(x)));
        IEnumerable<BL.UnitPosition> second6 = this.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == BL.ForceID.enemy && x.isEnable && !x.isDead && x.facility.isSkillUnit)).OrderBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.facilitySpawnOrder)).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (x => this.env.core.getUnitPosition(x)));
        this.executeSkillEffects(this.env.core.enemyActionUnits.value.Concat<BL.UnitPosition>(second5).Concat<BL.UnitPosition>(second6).ToList<BL.UnitPosition>());
        this.executeJumpSkillEffects((IEnumerable<BL.UnitPosition>) this.env.core.unitPositions.value, BL.ForceID.enemy);
        this.btm.setScheduleAction((Action) (() =>
        {
          BL.UnitPosition[] array = this.env.core.enemyActionUnits.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.IsJumping)).ToArray<BL.UnitPosition>();
          if (!((IEnumerable<BL.UnitPosition>) array).Any<BL.UnitPosition>())
            return;
          foreach (BL.UnitPosition unitPosition in array)
          {
            this.env.core.enemyActionUnits.value.Remove(unitPosition);
            unitPosition.recoveryCompleteUnit();
          }
          this.env.core.enemyActionUnits.commit();
        }), isInsertMode: true);
        break;
      case BL.Phase.player_end:
        if (this.deadCheck())
          break;
        if (this.env.core.condition.isTurn && this.env.core.phaseState.turnCount + 1 > this.env.core.condition.turn)
        {
          if (this.env.core.battleInfo.quest_type == CommonQuestType.GuildRaid)
            this.isTurnOver = true;
          this.btm.setPhaseState(BL.Phase.gameover);
          break;
        }
        if (this.env.core.neutralUnits.value.Any<BL.Unit>((Func<BL.Unit, bool>) (x => !x.isDead && x.isEnable)) || this.spawnUnitListModified.value.value.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.env.core.getForceID(x.unit) == BL.ForceID.neutral)))
        {
          Resources.UnloadUnusedAssets();
          this.btm.setPhaseState(BL.Phase.neutral_start);
          break;
        }
        if (!this.env.core.enemyUnits.value.Any<BL.Unit>((Func<BL.Unit, bool>) (x => !x.isDead && x.isEnable)) && !this.spawnUnitListModified.value.value.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.env.core.getForceID(x.unit) == BL.ForceID.enemy)))
          break;
        Resources.UnloadUnusedAssets();
        this.btm.setPhaseState(BL.Phase.enemy_start);
        break;
      case BL.Phase.neutral_end:
        if (this.deadCheck())
          break;
        Resources.UnloadUnusedAssets();
        this.btm.setPhaseState(BL.Phase.enemy_start);
        break;
      case BL.Phase.enemy_end:
        if (this.deadCheck())
          break;
        Resources.UnloadUnusedAssets();
        this.btm.setPhaseState(BL.Phase.turn_initialize);
        break;
      case BL.Phase.turn_initialize:
        this.env.core.battleLogger.TurnStart();
        this.aiController.stopAIAction();
        this.btm.setCurrentUnit((BL.Unit) null);
        if (this.env.core.condition.isElapsedTurn && this.env.core.phaseState.turnCount > this.env.core.condition.elapsedTurn)
        {
          this.btm.setPhaseState(this.battleManager.isWave ? BL.Phase.stageclear_pre : BL.Phase.stageclear, true);
          break;
        }
        this.btm.setScheduleAction((Action) (() =>
        {
          this.env.core.nextRandom();
          this.startBattleFieldEffect(BL.FieldEffectType.turn_start);
          foreach (BL.UnitPosition up in this.env.core.unitPositions.value)
          {
            if (up.unit.spawnTurn == this.env.core.phaseState.turnCount)
              this.spawnUnit(up);
          }
          this.btm.setScheduleAction((Action) (() =>
          {
            this.env.core.resetActionList(BL.ForceID.player);
            this.env.core.resetActionList(BL.ForceID.neutral);
            this.env.core.resetActionList(BL.ForceID.enemy);
            this.env.core.createDangerAria();
          }));
        }), isInsertMode: true);
        this.executeCallEntryEffects();
        foreach (BL.UnitPosition up in this.env.core.unitPositions.value)
          this.execPanelLandformEffect(this.env.core.getFieldPanel(up), up, BattleLandformEffectPhase.turn_start);
        this.executeTurnInitSkillEffects(this.env.core.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.isSpawned)).ToList<BL.UnitPosition>(), this.env.core.phaseState.absoluteTurnCount);
        if (Singleton<NGBattleManager>.GetInstance().isOvo)
        {
          BL.UnitPosition[] array = this.env.core.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => !x.unit.isDead && x.unit.isEnable)).ToArray<BL.UnitPosition>();
          this.executeSkillEffects(((IEnumerable<BL.UnitPosition>) array).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => !x.unit.isFacility || !x.unit.facility.isSkillUnit)).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) ((IEnumerable<BL.UnitPosition>) array).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.isFacility && x.unit.facility.isSkillUnit)).OrderBy<BL.UnitPosition, int>((Func<BL.UnitPosition, int>) (x => x.unit.facilitySpawnOrder))).ToList<BL.UnitPosition>());
          this.executeJumpSkillEffects((IEnumerable<BL.UnitPosition>) this.env.core.unitPositions.value);
        }
        this.env.core.playerCallSkillState.isSomeAction = false;
        this.env.core.enemyCallSkillState.isSomeAction = false;
        if (!Singleton<NGBattleManager>.GetInstance().isPvp && !Singleton<NGBattleManager>.GetInstance().isEarth)
        {
          this.env.core.playerCallSkillState.callSkillPoint = BattleFuncs.lotteryCallSkill(this.env.core.playerCallSkillState, this.env.core.phaseState.turnCount);
          this.env.core.enemyCallSkillState.callSkillPoint = BattleFuncs.lotteryCallSkill(this.env.core.enemyCallSkillState, this.env.core.phaseState.turnCount);
        }
        if (!Singleton<NGBattleManager>.GetInstance().isEarth)
        {
          if (this.env.core.playerCallSkillState.isCanUseCallSkill && !this.env.core.playerCallSkillState.isChargedCallGauge)
          {
            if (this.env.core.enemyCallSkillState.isCanUseCallSkill && !this.env.core.enemyCallSkillState.isChargedCallGauge)
              this.btm.setScheduleAction((Action) (() =>
              {
                this.StartCoroutine(this.uiNode.dispCallSkillChargedOwn());
                this.StartCoroutine(this.uiNode.dispCallSkillChargedEnemy(true));
              }), 2f);
            else
              this.btm.setScheduleAction((Action) (() => this.StartCoroutine(this.uiNode.dispCallSkillChargedOwn())), 2f);
          }
          else if (this.env.core.enemyCallSkillState.isCanUseCallSkill && !this.env.core.enemyCallSkillState.isChargedCallGauge)
            this.btm.setScheduleAction((Action) (() => this.StartCoroutine(this.uiNode.dispCallSkillChargedEnemy(false))), 2f);
        }
        this.btm.setScheduleAction((Action) (() =>
        {
          this.env.core.createDangerAria();
          if (this.battleManager.isOvo)
            this.battleManager.gameEngine.turnInitializeCompleted();
          else
            this.btm.setPhaseState(BL.Phase.player_start);
        }));
        break;
      case BL.Phase.win_finalize:
        this.btm.setScheduleAction((Action) (() => this.startStroyWithNextState(BL.StoryType.battle_win, BL.Phase.finalize)), isInsertMode: true);
        break;
      case BL.Phase.finalize:
        Time.timeScale = 1f;
        if (Object.op_Inequality((Object) this.uiNode, (Object) null))
          this.uiNode.SavePVPConfig();
        this.btm.setScheduleAction((Action) (() =>
        {
          this.battleManager.isSuspend = false;
          if (this.battleManager.isPvp)
            ((Component) this).gameObject.AddComponent<BattlePvpFinalize>();
          else if (this.battleManager.isPvnpc)
            ((Component) this).gameObject.AddComponent<BattlePvnpcFinalize>();
          else if (this.battleManager.isGvg)
            ((Component) this).gameObject.AddComponent<BattleGvgFinalize>();
          else if (this.battleManager.isWave)
            ((Component) this).gameObject.AddComponent<BattleWaveFinalize>();
          else if (this.battleManager.isRaid)
            ((Component) this).gameObject.AddComponent<BattleRaidFinalize>();
          else if (this.battleManager.isTower)
            ((Component) this).gameObject.AddComponent<BattleTowerFinalize>();
          else if (this.battleManager.isCorps)
          {
            ((Component) this).gameObject.AddComponent<BattleCorpsFinalize>();
          }
          else
          {
            if (Object.op_Equality((Object) this.finalize, (Object) null))
              this.finalize = ((Component) this).gameObject.AddComponent<BattleFinalize>();
            this.StartCoroutine(this.finalize.BattleResultChange());
          }
          App.SetAutoSleep(true);
        }));
        break;
      case BL.Phase.suspend:
        if (!this.battleManager.isEarth)
          break;
        Time.timeScale = 1f;
        this.btm.setScheduleAction((Action) (() =>
        {
          this.battleManager.isSuspend = true;
          App.SetAutoSleep(true);
          Singleton<EarthDataManager>.GetInstance().SuspendEarthMode();
        }));
        break;
      case BL.Phase.player_start_post:
        List<BL.Story> sl1 = this.env.core.battleInfo.isWave ? this.env.core.getStoryWaveOffense(this.env.core.phaseState.turnCount, this.env.core.nowWaveNo) : this.env.core.getStoryOffense(this.env.core.phaseState.turnCount);
        if (sl1 != null && sl1.Count > 0)
        {
          sl1[0].isRead = true;
          this.btm.setScheduleAction((Action) (() => this.startStroyWithNextState(sl1[0], BL.Phase.player)));
          break;
        }
        this.btm.setPhaseState(BL.Phase.player);
        break;
      case BL.Phase.neutral_start_post:
        this.btm.setPhaseState(BL.Phase.neutral);
        break;
      case BL.Phase.enemy_start_post:
        List<BL.Story> sl2 = this.env.core.battleInfo.isWave ? this.env.core.getStoryWaveDefense(this.env.core.phaseState.turnCount, this.env.core.nowWaveNo) : this.env.core.getStoryDefense(this.env.core.phaseState.turnCount);
        if (sl2 != null && sl2.Count > 0)
        {
          sl2[0].isRead = true;
          this.btm.setScheduleAction((Action) (() => this.startStroyWithNextState(sl2[0], BL.Phase.enemy)));
          break;
        }
        this.btm.setPhaseState(BL.Phase.enemy);
        break;
      case BL.Phase.all_dead_player:
        this.aiController.stopAIAction();
        this.btm.setScheduleAction((Action) (() =>
        {
          if (this.env.core.battleInfo.isContinueEnable)
          {
            this.battleManager.popupOpen(this.popupAllDeadPlayerPrefab);
            this.btm.setPhaseState(BL.Phase.none);
          }
          else
            this.btm.setPhaseState(BL.Phase.gameover);
        }));
        break;
      case BL.Phase.all_dead_neutral:
        this.aiController.stopAIAction();
        break;
      case BL.Phase.all_dead_enemy:
        this.aiController.stopAIAction();
        this.btm.setPhaseState(this.battleManager.isWave ? BL.Phase.stageclear_pre : BL.Phase.stageclear, true);
        break;
      case BL.Phase.stageclear_pre:
        this.aiController.stopAIAction();
        List<BL.Story> sl3 = this.battleManager.isWave ? this.env.core.getStoryWaveClear(this.env.core.nowWaveNo) : (List<BL.Story>) null;
        if (sl3 != null && sl3.Count > 0)
        {
          sl3[0].isRead = true;
          this.btm.setScheduleAction((Action) (() => this.startStroyWithNextState(sl3[0], BL.Phase.stageclear)));
          break;
        }
        this.btm.setPhaseState(BL.Phase.stageclear);
        break;
      case BL.Phase.stageclear:
        if (this.isStageClear || this.isGameOver)
          break;
        this.isStageClear = true;
        this.aiController.stopAIAction();
        if (this.battleManager.isWave && this.env.core.nowWaveNo < this.battleManager.waveLength)
        {
          this.startBattleFieldEffect(BL.FieldEffectType.waveclear);
          this.setStateWithEffect("WaveClear", 2.5f, BL.Phase.wave_start, (Action) null, false);
          break;
        }
        if (PerformanceConfig.GetInstance().IsQuestResult && !this.battleManager.isWave && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() && (this.env.core.battleInfo.quest_type == CommonQuestType.Story || this.env.core.battleInfo.quest_type == CommonQuestType.Extra))
        {
          this.env.core.isWin = true;
          this.finalize = ((Component) this).gameObject.AddComponent<BattleFinalize>();
          this.StartCoroutine(this.finalize.DoAPI());
        }
        this.btm.setScheduleAction((Action) (() =>
        {
          Singleton<NGSoundManager>.GetInstance().playVoiceByID(this.env.core.playerUnits.value[0].getVoicePattern(), 71, 0);
          this.startBattleFieldEffect(BL.FieldEffectType.stageclear);
          this.setStateWithEffect("StageClear", this.getEffectStageClearTime(), BL.Phase.win_finalize, (Action) (() => this.env.core.isWin = true), false, this.isEffectStageClearSkippedVoiceStop(), new Action<ScheduleEnumerator>(this.SkipEffect));
        }));
        break;
      case BL.Phase.gameover:
        if (this.isStageClear || this.isGameOver)
          break;
        this.isGameOver = true;
        float effectTime = this.getEffectStageClearTime();
        bool isSkippedVoiceStop = this.isEffectStageClearSkippedVoiceStop();
        Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
        bool needNabi = !this.battleManager.isOvo && !this.env.core.battleInfo.isEarthMode && this.env.core.battleInfo.quest_type != CommonQuestType.GuildRaid && this.env.core.battleInfo.isFirstAllDead && this.env.core.allDeadUnitsp(BL.ForceID.player);
        this.aiController.stopAIAction();
        string effectName = this.isTurnOver ? "dir_TurnOver" : "GameOver";
        this.btm.setScheduleAction((Action) (() =>
        {
          if (!this.isTurnOver)
            Singleton<NGSoundManager>.GetInstance().playVoiceByID(this.env.core.playerUnits.value[0].getVoicePattern(), 72, 0);
          this.setStateWithEffect(effectName, effectTime, BL.Phase.finalize, (Action) (() => this.env.core.isWin = false), needNabi, isSkippedVoiceStop, new Action<ScheduleEnumerator>(this.SkipEffect));
        }));
        break;
      case BL.Phase.surrender:
        if (this.isStageClear || this.isGameOver)
          break;
        this.isGameOver = true;
        this.aiController.stopAIAction();
        this.btm.setSchedule((Schedule) new BattleStateController.EffectAIWait(this));
        this.env.core.isWin = false;
        this.btm.setPhaseState(BL.Phase.finalize);
        break;
      case BL.Phase.pvp_player_start:
        Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.env.core.stage.stage.field_player_bgm_file, this.env.core.stage.stage.field_player_bgm);
        this.startBattleFieldEffect(BL.FieldEffectType.pvp_change_player);
        BL.UnitPosition unitPosition1 = this.env.core.playerActionUnits.value.FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.cantChangeCurrent));
        BL.UnitPosition up1 = (BL.UnitPosition) null;
        if (unitPosition1 != null)
        {
          up1 = unitPosition1;
          this.env.core.unitCurrent.commit();
        }
        else
        {
          if (this.env.core.playerActionUnits.value.Count > 0)
            up1 = this.env.core.playerActionUnits.value[0];
          this.btm.setCurrentUnit((BL.Unit) null);
        }
        if (up1 != null)
          this.setStartTarget(up1, true);
        if (PerformanceConfig.GetInstance().EnablePvPAutoButton)
          this.isAutoBattleModified.notifyChanged();
        this.btm.setPhaseState(BL.Phase.player);
        break;
      case BL.Phase.pvp_enemy_start:
        Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.env.core.stage.stage.field_enemy_bgm_file, this.env.core.stage.stage.field_enemy_bgm);
        this.startBattleFieldEffect(BL.FieldEffectType.pvp_change_enemy);
        if (this.env.core.enemyActionUnits.value.Count > 0)
          this.setStartTarget(this.env.core.enemyActionUnits.value[0], false);
        this.btm.setPhaseState(BL.Phase.enemy);
        break;
      case BL.Phase.pvp_result:
        if (Object.op_Inequality((Object) this.pvpManager, (Object) null))
          this.pvpManager.isResult = true;
        if (Object.op_Inequality((Object) Singleton<PVNpcManager>.GetInstanceOrNull(), (Object) null))
          Singleton<PVNpcManager>.GetInstance().isResult = true;
        if (Object.op_Inequality((Object) Singleton<GVGManager>.GetInstanceOrNull(), (Object) null))
          Singleton<GVGManager>.GetInstance().isResult = true;
        this.btm.setPhaseState(BL.Phase.finalize);
        break;
      case BL.Phase.pvp_start_init:
        this.btm.setScheduleAction((Action) (() => this.startBattlePvpStartEffect()));
        break;
      case BL.Phase.pvp_exception:
        NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
        if (sm.sceneName != this.battleManager.topScene)
          this.btm.setScheduleAction((Action) (() => sm.backScene()));
        if (this.pvpManager.errorCode != null)
        {
          Consts instance = Consts.GetInstance();
          this.pvpManager.PopupBattleReceiveError(instance.VERSUS_02694POPUP_TITLE, instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION + "\n" + instance.VERSUS_GOOD_CONNECTION_RETRY, this.pvpManager.errorCode);
          this.pvpManager.errorCode = (string) null;
        }
        else
          ModalWindow.ShowYesNo(Consts.GetInstance().VERSUS_02694POPUP_TITLE, this.pvpManager.getErrorMessage(this.pvpManager.exception), (Action) (() => this.pvpManager.errorRecovery()), (Action) (() => this.btm.setPhaseState(BL.Phase.finalize)));
        this.btm.setPhaseState(BL.Phase.none);
        break;
      case BL.Phase.wave_start:
        this.btm.setSchedule((Schedule) new BattleStateController.WaveStart(this, this.env, this.env.core.nowWaveNo));
        this.btm.setPhaseState(BL.Phase.battle_start_init);
        break;
      case BL.Phase.battle_start:
        this.battleManager.saveEnvironment();
        this.btm.setTargetPanel(this.env.core.getFieldPanel(this.env.core.getUnitPosition(this.env.core.playerUnits.value[0])), 0.0f, isWaitCameraMove: true);
        if (this.battleManager.isOvo)
        {
          this.battleStartOvo();
          break;
        }
        this.battleStartNormal();
        break;
      case BL.Phase.battle_start_init:
        this.battleManager.saveEnvironment();
        this.isStageClear = false;
        this.isGameOver = false;
        this.isTurnOver = false;
        this.startBattleFieldEffect(BL.FieldEffectType.first_turn_start);
        this.btm.setScheduleAction((Action) (() => this.startBattleStartEffect()));
        break;
    }
  }

  private void updateCurrentUnitPosition()
  {
    if (this.currentUnitPosition == this.env.core.currentUnitPosition)
      return;
    this.currentUnitPosition = this.env.core.currentUnitPosition;
    this.currentUnitActionCount = this.currentUnitPosition != null ? this.currentUnitPosition.unit.skillEffects.GetCompleteCount() : -1;
  }

  private bool isCurrentUnitActionCountedOnce
  {
    get
    {
      bool actionCountedOnce = false;
      if (this.currentUnitPosition != null)
      {
        actionCountedOnce = this.currentUnitActionCount != this.currentUnitPosition.completedCount;
        this.currentUnitActionCount = this.currentUnitPosition.completedCount;
      }
      return actionCountedOnce;
    }
  }

  private void checkScriptUnitInArea()
  {
    if (this.env.core.unitCurrent.unit == (BL.Unit) null)
      return;
    this.updateCurrentUnitPosition();
    switch (this.phaseStateModified.value.state)
    {
      case BL.Phase.player:
        if (!this.isCurrentUnitActionCountedOnce)
          break;
        BL.UnitPosition currentUnitPosition1 = this.env.core.currentUnitPosition;
        using (List<BL.Story>.Enumerator enumerator = this.env.core.getStoryOffenseInArea(currentUnitPosition1.originalRow, currentUnitPosition1.originalColumn).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            BL.Story current = enumerator.Current;
            current.isRead = true;
            currentUnitPosition1.setScript(current.scriptId);
          }
          break;
        }
      case BL.Phase.neutral:
        int num = this.isCurrentUnitActionCountedOnce ? 1 : 0;
        break;
      case BL.Phase.enemy:
        if (!this.isCurrentUnitActionCountedOnce)
          break;
        BL.UnitPosition currentUnitPosition2 = this.env.core.currentUnitPosition;
        using (List<BL.Story>.Enumerator enumerator = this.env.core.getStoryDefenseInArea(currentUnitPosition2.originalRow, currentUnitPosition2.originalColumn).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            BL.Story current = enumerator.Current;
            current.isRead = true;
            currentUnitPosition2.setScript(current.scriptId);
          }
          break;
        }
    }
  }

  public class ApplyFacilitySkillDeads
  {
    private BattleStateController bsc;
    private HashSet<BL.Unit> units;

    public ApplyFacilitySkillDeads(BattleStateController bsc)
    {
      this.bsc = bsc;
      this.units = bsc.battleManager.useGameEngine ? new HashSet<BL.Unit>() : (HashSet<BL.Unit>) null;
    }

    public void Add(BL.ExecuteSkillEffectResult es)
    {
      if (this.units == null)
        return;
      foreach (BL.UnitPosition target in es.targets)
        this.units.Add(target.unit);
    }

    public void Execute()
    {
      if (this.units == null)
        return;
      foreach (BL.Unit unit in this.units)
        this.bsc.battleManager.gameEngine.applyDeadUnit(unit, (BL.Unit) null);
    }
  }

  private class EffectAIWait : ScheduleEnumerator
  {
    private BattleStateController parent;

    public EffectAIWait(BattleStateController parent) => this.parent = parent;

    public override IEnumerator doBody()
    {
      BattleStateController.EffectAIWait effectAiWait = this;
      while (effectAiWait.parent.aiController.isAction)
        yield return (object) null;
      effectAiWait.isCompleted = true;
    }
  }

  private class PvpMatchEffect : BattleEffects.CloneEnumlator
  {
    private BattleStateController parent;

    public PvpMatchEffect(BattleStateController parent) => this.parent = parent;

    public override IEnumerator doBody(GameObject o)
    {
      PopupPvpStart component = o.GetComponent<PopupPvpStart>();
      IGameEngine gameEngine = this.parent.battleManager.gameEngine;
      if (Object.op_Inequality((Object) component, (Object) null) && gameEngine != null)
      {
        string pGuild = (string) null;
        string eGuild = (string) null;
        if (gameEngine is GVGManager)
        {
          GVGManager gvgManager = gameEngine as GVGManager;
          pGuild = gvgManager.playerGuildName;
          eGuild = gvgManager.enemyGuildName;
        }
        IEnumerator e = component.Initialize(gameEngine.playerName, gameEngine.enemyName, gameEngine.playerEmblem, gameEngine.enemyEmblem, this.parent.env.core.playerUnits.value, this.parent.env.core.enemyUnits.value, pGuild, eGuild);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private class WaveStart : ScheduleEnumerator
  {
    private BattleStateController parent;
    private BE env;
    private int wave;

    public WaveStart(BattleStateController parent, BE env, int wave)
    {
      this.parent = parent;
      this.env = env;
      this.wave = wave;
    }

    public override IEnumerator doBody()
    {
      BattleStateController.WaveStart waveStart = this;
      NGBattleManager bm = waveStart.parent.battleManager;
      NGBattle3DObjectManager m3d = bm.getManager<NGBattle3DObjectManager>();
      CommonRoot cr = Singleton<CommonRoot>.GetInstance();
      cr.isLoading = true;
      yield return (object) null;
      waveStart.env.pushWaveStageDatas();
      m3d.setRootActive(false);
      if (waveStart.env.core.battleInfo.isWave)
      {
        Battle3DRoot objectOfType = Object.FindObjectOfType<Battle3DRoot>();
        if (Object.op_Inequality((Object) objectOfType, (Object) null))
          ((Component) objectOfType.mapPoint).gameObject.SetActive(true);
      }
      yield return (object) null;
      IEnumerator e = new BattleLogicInitializer().initializeWave(waveStart.wave, waveStart.env.core.battleInfo, waveStart.env.core);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      e = m3d.loadStage(waveStart.env);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      e = m3d.loadUnitResources(waveStart.env);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      e = m3d.spawns(waveStart.env.core.unitPositions.value, waveStart.env, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      BattleFuncs.createAsterNodeCache(waveStart.env.core);
      waveStart.env.core.clearRouteCache();
      waveStart.parent.aiController.clearCache();
      waveStart.parent.uiNode.initializeStage();
      BattleCameraController controller = bm.getController<BattleCameraController>();
      BL.Panel fieldPanel = waveStart.env.core.getFieldPanel(waveStart.env.core.getUnitPosition(waveStart.env.core.playerUnits.value[0]));
      if (fieldPanel != null)
      {
        controller.setLookAtTarget(fieldPanel, true);
        waveStart.env.core.setCurrentField(fieldPanel);
      }
      m3d.setRootActive(true);
      yield return (object) null;
      cr.isLoading = false;
      BattleCameraFilter.Active();
      waveStart.isCompleted = true;
    }
  }
}
