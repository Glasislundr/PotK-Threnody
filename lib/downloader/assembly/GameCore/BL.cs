// Decompiled with JetBrains decompiler
// Type: GameCore.BL
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using AI.Logic;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniLinq;
using UnityEngine;

#nullable disable
namespace GameCore
{
  [Serializable]
  public class BL
  {
    public BL.ClassValue<List<BL.AIUnit>> aiUnitPositions = new BL.ClassValue<List<BL.AIUnit>>((List<BL.AIUnit>) null);
    public BL.ClassValue<List<BL.AIUnit>> aiUnits = new BL.ClassValue<List<BL.AIUnit>>((List<BL.AIUnit>) null);
    public BL.ClassValue<List<BL.AIUnit>> aiActionUnits = new BL.ClassValue<List<BL.AIUnit>>((List<BL.AIUnit>) null);
    public BL.ClassValue<Queue<BL.AIUnit>> aiActionOrder = new BL.ClassValue<Queue<BL.AIUnit>>((Queue<BL.AIUnit>) null);
    public BL.StructValue<bool> isAutoItemMove = new BL.StructValue<bool>(true);
    public int aiActionMax;
    public BL.Condition condition;
    [NonSerialized]
    private List<BL.Unit> loseUnitList;
    [NonSerialized]
    private BattleVictoryAreaCondition[] _winAreaCache;
    [NonSerialized]
    private BattleVictoryAreaCondition[] _loseAreaCache;
    public BL.StructValue<long> dropMoney = new BL.StructValue<long>(0L);
    public BL.StructValue<int> dropItem = new BL.StructValue<int>(0);
    public BL.StructValue<int> dropUnit = new BL.StructValue<int>(0);
    public BL.ClassValue<List<BL.UnitPosition>> unitPositions;
    [SerializeField]
    private BL.Panel[,] fieldMatrix;
    public BL.Stage stage;
    public BL.ClassValue<BL.Panel> fieldCurrent = new BL.ClassValue<BL.Panel>((BL.Panel) null);
    public BL.ClassValue<List<BL.FieldEffect>> fieldEffectList;
    public int playerPoint;
    public int enemyPoint;
    public int playerPointView;
    public int enemyPointView;
    public int playerPointGauge;
    public int enemyPointGauge;
    public int playerAnnihilationCount;
    public int enemyAnnihilationCount;
    private BL.Intimate intimate = new BL.Intimate();
    public BL.StructValue<bool> isAutoBattle = new BL.StructValue<bool>(false);
    public BL.StructValue<bool> isSkillUseConfirmation = new BL.StructValue<bool>(true);
    public BL.StructValue<bool> isTouchWait = new BL.StructValue<bool>(true);
    public BL.ClassValue<List<BL.Item>> itemList;
    public BL.ClassValue<List<BL.Item>> itemListInBattle;
    public BL.BattleLogger battleLogger;
    public BattleInfo battleInfo;
    public XorShift randomBase = new XorShift(DateTime.Now);
    public int randomCount;
    public XorShift random = new XorShift();
    public int continueCount;
    public bool isWin;
    public int currentWave;
    [NonSerialized]
    private Dictionary<Tuple<bool, int, int, int, int, int, int>, List<BL.AttackStatusCacheContainer>> mAttackStatusCacheDic = new Dictionary<Tuple<bool, int, int, int, int, int, int>, List<BL.AttackStatusCacheContainer>>();
    [NonSerialized]
    private Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>> mRouteDic;
    [NonSerialized]
    private Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>> mRouteDic_IM;
    public BL.PhaseState phaseState = new BL.PhaseState();
    public BL.CallSkillState playerCallSkillState = new BL.CallSkillState();
    public BL.CallSkillState enemyCallSkillState = new BL.CallSkillState();
    public BL.StructValue<bool> firstCompleted = new BL.StructValue<bool>();
    public BL.ClassValue<List<BL.UnitPosition>> playerActionUnits = new BL.ClassValue<List<BL.UnitPosition>>(new List<BL.UnitPosition>());
    public BL.ClassValue<List<BL.UnitPosition>> neutralActionUnits = new BL.ClassValue<List<BL.UnitPosition>>(new List<BL.UnitPosition>());
    public BL.ClassValue<List<BL.UnitPosition>> enemyActionUnits = new BL.ClassValue<List<BL.UnitPosition>>(new List<BL.UnitPosition>());
    public BL.ClassValue<List<BL.UnitPosition>> completedActionUnits = new BL.ClassValue<List<BL.UnitPosition>>(new List<BL.UnitPosition>());
    public BL.ClassValue<List<BL.UnitPosition>> spawnUnits = new BL.ClassValue<List<BL.UnitPosition>>(new List<BL.UnitPosition>());
    public BL.ClassValue<List<BL.Story>> storyList;
    private const int NO_WAVE = 0;
    public BL.ClassValue<List<BL.Unit>> playerUnits;
    public BL.ClassValue<List<BL.Unit>> neutralUnits;
    public BL.ClassValue<List<BL.Unit>> enemyUnits;
    public BL.ClassValue<List<BL.Unit>> facilityUnits;
    public BL.CurrentUnit unitCurrent = new BL.CurrentUnit();
    public BL.StructValue<bool> isViewDengerArea = new BL.StructValue<bool>(false);
    public BL.StructValue<int> sight = new BL.StructValue<int>(0);
    public BL.StructValue<bool> isViewUnitType = new BL.StructValue<bool>(false);
    private BL.ForceID[] playerTarget = new BL.ForceID[2]
    {
      BL.ForceID.neutral,
      BL.ForceID.enemy
    };
    private List<BL.Panel> mDangerAria;

    public List<BL.AIUnit> getTargetAIUnits(
      BL.AIUnit aiUnit,
      BL.Unit.TargetAttribute ta,
      List<BL.Unit> searchTargets = null)
    {
      return this.getTargetAIUnits(this.getTargetForce(aiUnit.unitPosition.unit, aiUnit.IsCharm), ta, searchTargets);
    }

    public List<BL.AIUnit> getTargetAIUnits(
      BL.ForceID[] forceIds,
      BL.Unit.TargetAttribute ta,
      List<BL.Unit> searchTargets = null)
    {
      List<BL.AIUnit> targetAiUnits = new List<BL.AIUnit>();
      foreach (BL.AIUnit aiUnit in this.aiUnitPositions.value)
      {
        if (aiUnit.checkTargetAttribute(ta) && aiUnit.hp > 0 && !aiUnit.IsJumping && ((IEnumerable<BL.ForceID>) forceIds).Contains<BL.ForceID>(aiUnit.getForceID(this)) && (searchTargets == null || searchTargets.Contains(aiUnit.unit)))
          targetAiUnits.Add(aiUnit);
      }
      return targetAiUnits;
    }

    public bool isMoveOKAI(
      BL.Panel panel,
      BL.Unit unit,
      bool isRebirth,
      bool enabledIgnoreMoveCost,
      int moveCost)
    {
      if (!this.isMoveOKPanel(panel, unit, enabledIgnoreMoveCost, moveCost))
        return false;
      if (isRebirth)
        return true;
      BL.UnitPosition[] fieldUnitsAi = this.getFieldUnitsAI(panel);
      if (fieldUnitsAi != null)
      {
        foreach (BL.UnitPosition unitPosition in fieldUnitsAi)
        {
          if ((!(unitPosition is BL.AIUnit aiUnit) || aiUnit.unit == unit || aiUnit.unit.isPutOn ? 1 : (aiUnit.unit.isFacility ? 0 : (aiUnit.getForceID(this) == this.getForceID(unit) ? 1 : 0))) == 0)
            return false;
        }
      }
      return true;
    }

    public static int panelIndex(BL.AIUnit unit) => BL.panelIndex(unit.row, unit.column);

    public static int panelIndex(BL.Panel panel) => BL.panelIndex(panel.row, panel.column);

    public static int panelIndex(int row, int column) => row * 10000 + column;

    public BL.UnitPosition getFieldUnitAI(
      int row,
      int column,
      bool isOriginal = false,
      bool includeJumping = false)
    {
      foreach (BL.AIUnit fieldUnitAi in this.aiUnitPositions.value)
      {
        if (!fieldUnitAi.isDead && (includeJumping || !fieldUnitAi.IsJumping))
        {
          if (isOriginal)
          {
            if (fieldUnitAi.originalRow == row && fieldUnitAi.originalColumn == column)
              return (BL.UnitPosition) fieldUnitAi;
          }
          else if (fieldUnitAi.row == row && fieldUnitAi.column == column)
            return (BL.UnitPosition) fieldUnitAi;
        }
      }
      return (BL.UnitPosition) null;
    }

    public BL.UnitPosition getFieldUnitAI(BL.Panel panel, bool isOriginal = false, bool includeJumping = false)
    {
      return this.getFieldUnitAI(panel.row, panel.column, isOriginal, includeJumping);
    }

    public BL.UnitPosition[] getFieldUnitsAI(
      int row,
      int column,
      bool original = false,
      bool isDead = false,
      bool includeJumping = false)
    {
      List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
      foreach (BL.AIUnit aiUnit in this.aiUnitPositions.value)
      {
        int num1 = original ? aiUnit.originalRow : aiUnit.row;
        int num2 = original ? aiUnit.originalColumn : aiUnit.column;
        bool isDead1 = aiUnit.isDead;
        if ((!isDead && !isDead1 || isDead & isDead1) && num1 == row && num2 == column && (includeJumping || !aiUnit.IsJumping))
          unitPositionList.Add((BL.UnitPosition) aiUnit);
      }
      return unitPositionList.Count <= 0 ? (BL.UnitPosition[]) null : unitPositionList.ToArray();
    }

    public BL.UnitPosition[] getFieldUnitsAI(
      BL.Panel panel,
      bool original = false,
      bool isDead = false,
      bool includeJumping = false)
    {
      return this.getFieldUnitsAI(panel.row, panel.column, original, isDead, includeJumping);
    }

    public BL.AIUnit getAIUnit(BL.Unit unit)
    {
      foreach (BL.AIUnit aiUnit in this.aiUnitPositions.value)
      {
        if (aiUnit.unit == unit)
          return aiUnit;
      }
      return (BL.AIUnit) null;
    }

    public int getAroundEnemyUnitsCount(BL.AIUnit aiUnit, int range)
    {
      return BattleFuncs.getForceUnitsWithinRange(aiUnit.row, aiUnit.column, range, this.getTargetForce(aiUnit.unitPosition.unit, aiUnit.IsCharm), true).Count;
    }

    public static int fieldDistance(BL.AIUnit p1, BL.AIUnit p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public static int fieldDistance(BL.AIUnit p1, BL.Panel p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public static int fieldDistance(BL.Panel p1, BL.AIUnit p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public BL.Unit getBossUnit()
    {
      if (this.condition.type != BL.ConditionType.bossdown)
        return (BL.Unit) null;
      foreach (BL.Unit bossUnit in this.enemyUnits.value)
      {
        if (bossUnit.playerUnit.id == this.condition.bossId)
          return bossUnit;
      }
      return (BL.Unit) null;
    }

    public bool bossUnitp(BL.Unit unit) => this.getBossUnit() == unit;

    public BL.GameoverType getGamevoerType()
    {
      if (this.condition.condition.gameover_type_guest == 0)
        return BL.GameoverType.alldown;
      return this.condition.condition.gameover_type_guest != 1 ? BL.GameoverType.playerdown : BL.GameoverType.guestdown;
    }

    public List<BL.Unit> getLoseUnitList()
    {
      if (this.loseUnitList == null)
      {
        this.loseUnitList = new List<BL.Unit>();
        if (!string.IsNullOrEmpty(this.condition.condition.lose_on_unit_dead))
        {
          int result;
          IEnumerable<int> unitIDList = ((IEnumerable<string>) this.condition.condition.lose_on_unit_dead.Split(',')).Select<string, int>((Func<string, int>) (x => int.TryParse(x, out result) ? result : 0)).Where<int>((Func<int, bool>) (id => id > 0));
          this.loseUnitList.AddRange(this.playerUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.playerUnit.is_enemy && !x.playerUnit.is_guest && unitIDList.Contains<int>(x.index + 1))));
        }
        if (!string.IsNullOrEmpty(this.condition.condition.lose_on_gesut_dead))
        {
          int result;
          IEnumerable<int> gesutIDList = ((IEnumerable<string>) this.condition.condition.lose_on_gesut_dead.Split(',')).Select<string, int>((Func<string, int>) (x => int.TryParse(x, out result) ? result : 0)).Where<int>((Func<int, bool>) (id => id > 0));
          this.loseUnitList.AddRange(this.playerUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.playerUnit.is_enemy && x.playerUnit.is_guest && gesutIDList.Contains<int>(x.playerUnit.id))));
        }
      }
      return this.loseUnitList;
    }

    public List<BL.UnitPosition> getWinAreaUnitPositions()
    {
      if (this._winAreaCache == null)
        this._winAreaCache = this.condition.winAreaConditoin;
      return this.getConditionAreaUnitPositions(this._winAreaCache);
    }

    public List<BL.UnitPosition> getLoseAreaUnitPositions()
    {
      if (this._loseAreaCache == null)
        this._loseAreaCache = this.condition.loseAreaConditoin;
      return this.getConditionAreaUnitPositions(this._loseAreaCache);
    }

    public List<BL.UnitPosition> getConditionAreaUnitPositions(
      BattleVictoryAreaCondition[] area_condition)
    {
      List<BL.UnitPosition> areaUnitPositions = new List<BL.UnitPosition>();
      if (area_condition != null && area_condition.Length != 0)
      {
        foreach (BattleVictoryAreaCondition victoryAreaCondition in area_condition)
        {
          BL.UnitPosition fieldUnit = this.getFieldUnit(victoryAreaCondition.area_y, victoryAreaCondition.area_x);
          if (fieldUnit != null)
            areaUnitPositions.Add(fieldUnit);
        }
      }
      return areaUnitPositions;
    }

    public BL.UnitPosition currentUnitPosition
    {
      get
      {
        return this.unitCurrent.unit == (BL.Unit) null ? new BL.UnitPosition() : this.getUnitPosition(this.unitCurrent.unit);
      }
    }

    public void initializeFeild(int stageId)
    {
      this.stage = new BL.Stage(stageId);
      this.fieldMatrix = new BL.Panel[this.stage.stage.map_height, this.stage.stage.map_width];
    }

    public void setFeildPanel(
      int index,
      int row,
      int column,
      int landformId,
      int fieldEventId,
      BL.DropData fieldEvent,
      BattleVictoryAreaCondition[] winArea = null,
      BattleVictoryAreaCondition[] loseArea = null,
      BattleReinforcement[] battleReinforcements = null)
    {
      this.fieldMatrix[row, column] = new BL.Panel(index, row, column, landformId, fieldEventId, fieldEvent, winArea, loseArea, battleReinforcements);
    }

    public void setCurrentField(int row, int column)
    {
      this.fieldCurrent.value = this.fieldMatrix[row, column];
    }

    public void setCurrentField(BL.Panel panel) => this.setCurrentField(panel.row, panel.column);

    public BL.UnitPosition getFieldUnit(int row, int column, bool original = false, bool includeJumping = false)
    {
      foreach (BL.UnitPosition fieldUnit in this.unitPositions.value)
      {
        int num1 = original ? fieldUnit.originalRow : fieldUnit.row;
        int num2 = original ? fieldUnit.originalColumn : fieldUnit.column;
        if (fieldUnit.unit.isEnable && !fieldUnit.unit.isDead && num1 == row && num2 == column && (includeJumping || !fieldUnit.unit.IsJumping))
          return fieldUnit;
      }
      return (BL.UnitPosition) null;
    }

    public BL.UnitPosition getFieldUnit(BL.Panel panel, bool original = false, bool includeJumping = false)
    {
      return this.getFieldUnit(panel.row, panel.column, original, includeJumping);
    }

    public BL.UnitPosition[] getFieldUnits(
      int row,
      int column,
      bool original = false,
      bool isDead = false,
      bool includeJumping = false)
    {
      List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
      foreach (BL.UnitPosition unitPosition in this.unitPositions.value)
      {
        int num1 = original ? unitPosition.originalRow : unitPosition.row;
        int num2 = original ? unitPosition.originalColumn : unitPosition.column;
        if (unitPosition.unit.isEnable && (!isDead && !unitPosition.unit.isDead || isDead && unitPosition.unit.isDead) && num1 == row && num2 == column && (includeJumping || !unitPosition.unit.IsJumping))
          unitPositionList.Add(unitPosition);
      }
      return unitPositionList.Count <= 0 ? (BL.UnitPosition[]) null : unitPositionList.ToArray();
    }

    public BL.UnitPosition[] getFieldUnits(BL.Panel panel, bool original = false, bool isDead = false)
    {
      return this.getFieldUnits(panel.row, panel.column, original, isDead);
    }

    public BL.UnitPosition getUnitPositionById(int id)
    {
      foreach (BL.UnitPosition unitPositionById in this.unitPositions.value)
      {
        if (unitPositionById.id == id)
          return unitPositionById;
      }
      return (BL.UnitPosition) null;
    }

    public List<BL.Unit> getForceUnitList(BL.ForceID forceID)
    {
      switch (forceID)
      {
        case BL.ForceID.player:
          return this.playerUnits.value;
        case BL.ForceID.neutral:
          return this.neutralUnits.value;
        case BL.ForceID.enemy:
          return this.enemyUnits.value;
        default:
          return (List<BL.Unit>) null;
      }
    }

    public int getFieldHeight() => this.fieldMatrix.GetLength(0);

    public int getFieldWidth() => this.fieldMatrix.GetLength(1);

    public BL.Panel getFieldPanel(int row, int column)
    {
      return row < 0 || column < 0 || row >= this.getFieldHeight() || column >= this.getFieldWidth() ? (BL.Panel) null : this.fieldMatrix[row, column];
    }

    public IEnumerable<BL.Panel> getAllPanel() => ((IEnumerable) this.fieldMatrix).Cast<BL.Panel>();

    public BL.Panel getFieldPanel(BL.UnitPosition up, bool original = false)
    {
      return original ? this.getFieldPanel(up.originalRow, up.originalColumn) : this.getFieldPanel(up.row, up.column);
    }

    public bool isMoveOKPanel(
      BL.Panel panel,
      BL.Unit unit,
      bool enabledIgnoreMoveCost,
      int moveCost)
    {
      return panel != null && BattleFuncs.getMoveCost(panel, unit, enabledIgnoreMoveCost) <= moveCost;
    }

    public bool isMoveOK(
      BL.Panel panel,
      BL.Unit unit = null,
      bool isRebirth = false,
      bool enabledIgnoreMoveCost = false,
      int moveCost = -1)
    {
      if (unit == (BL.Unit) null)
        unit = this.unitCurrent.unit;
      if (moveCost == -1)
        moveCost = this.getUnitPosition(unit).moveCost;
      if (!this.isMoveOKPanel(panel, unit, enabledIgnoreMoveCost, moveCost))
        return false;
      if (isRebirth)
        return true;
      BL.UnitPosition fieldUnit = this.getFieldUnit(panel.row, panel.column, true);
      if (fieldUnit == null || fieldUnit.unit == unit || fieldUnit.unit.isPutOn)
        return true;
      return !fieldUnit.unit.isFacility && this.getForceID(fieldUnit.unit) == this.getForceID(unit);
    }

    public bool isMoveOK(
      int row,
      int column,
      BL.Unit unit = null,
      bool isRebirth = false,
      bool enabledIgnoreMoveCost = false)
    {
      return this.isMoveOK(this.getFieldPanel(row, column), unit, isRebirth, enabledIgnoreMoveCost);
    }

    public BL.UnitPosition getUnitPosition(BL.Unit unit)
    {
      return this.unitPositions.value.Find((Predicate<BL.UnitPosition>) (up => up.unit == unit));
    }

    public float? getLookDirection(int sRow, int sColumn, int dRow, int dColumn, bool isFacility)
    {
      if (isFacility)
        return new float?();
      if (sRow == dRow)
      {
        if (sColumn == dColumn)
          return new float?();
        return sColumn > dColumn ? new float?(270f) : new float?(90f);
      }
      return sRow > dRow ? new float?(180f) : new float?(0.0f);
    }

    public void lookDirection(BL.UnitPosition s, BL.UnitPosition d)
    {
      float? lookDirection = this.getLookDirection(s.row, s.column, d.row, d.column, s.unit.isFacility);
      if (!lookDirection.HasValue)
        return;
      s.direction = lookDirection.Value;
    }

    public void lookDirection(BL.UnitPosition s, BL.Unit d)
    {
      this.lookDirection(s, this.getUnitPosition(d));
    }

    public void lookDirection(BL.Unit s, BL.Unit d)
    {
      this.lookDirection(this.getUnitPosition(s), this.getUnitPosition(d));
    }

    public static int fieldDistance(BL.Panel p1, BL.Panel p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public static int fieldDistance(BL.UnitPosition p1, BL.UnitPosition p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public static int fieldDistance(BL.UnitPosition p1, BL.Panel p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public static int fieldDistance(BL.Panel p1, BL.UnitPosition p2)
    {
      return BL.fieldDistance(p1.row, p1.column, p2.row, p2.column);
    }

    public static int fieldDistance(int r1, int c1, int r2, int c2)
    {
      return Mathf.Abs(r1 - r2) + Mathf.Abs(c1 - c2);
    }

    public BL.UnitPosition fieldForceUnit(
      int row,
      int column,
      BL.ForceID[] targetForce,
      bool isAI)
    {
      BL.UnitPosition unitPosition = isAI ? this.getFieldUnitAI(row, column) : this.getFieldUnit(row, column);
      if (unitPosition == null)
        return (BL.UnitPosition) null;
      return !((IEnumerable<BL.ForceID>) targetForce).Contains<BL.ForceID>(this.getForceID(unitPosition.unit)) ? (BL.UnitPosition) null : unitPosition;
    }

    public BL.Phase getChangePhaseToPanel(BL.UnitPosition up)
    {
      BL.Panel fieldPanel = this.getFieldPanel(up.originalRow, up.originalColumn);
      return fieldPanel != null ? fieldPanel.getChangePhaseToPanel(this.getForceID(up.unit)) : BL.Phase.none;
    }

    public int[] getReinforcementIDsToPanel(BL.UnitPosition up)
    {
      return this.getFieldPanel(up.originalRow, up.originalColumn)?.getReinforcementIDsToPanel(this.getForceID(up.unit));
    }

    public void removeZocPanels(BL.ISkillEffectListUnit u, int r, int c, bool isAI = false)
    {
      foreach (BL.Panel zocPanel in this.getZocPanels(u, r, c, enableOnly: false))
        zocPanel.removeZocUnit(u.originalUnit, isAI);
    }

    public void addZocPanels(BL.ISkillEffectListUnit u, int r, int c, bool isAI = false)
    {
      foreach (BL.Panel zocPanel in this.getZocPanels(u, r, c, true))
        zocPanel.addZocUnit(u.originalUnit, isAI);
    }

    public void resetZocPanels(
      BL.ISkillEffectListUnit u,
      int r0,
      int c0,
      int r1,
      int c1,
      bool isAI = false)
    {
      this.removeZocPanels(u, r0, c0, isAI);
      this.addZocPanels(u, r1, c1, isAI);
    }

    public IEnumerable<BL.Panel> getZocPanels(
      BL.ISkillEffectListUnit unit,
      int row,
      int column,
      bool useGearFilter = false,
      bool enableOnly = true)
    {
      return BattleFuncs.getSkillZocPanels(unit, row, column, useGearFilter, enableOnly);
    }

    public static bool equalPanelSkillEffectList(List<BL.SkillEffect> l0, List<BL.SkillEffect> l1)
    {
      if (l0 == l1)
        return true;
      if (l0.Count != l1.Count)
        return false;
      foreach (BL.SkillEffect skillEffect in l0)
      {
        BL.SkillEffect se = skillEffect;
        if (!l1.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.uniqueId == se.uniqueId && x.parentUnit == se.parentUnit)))
          return false;
      }
      return true;
    }

    public virtual void resetUnitStatus(BL.UnitPosition up, int row, int column, float direction)
    {
      up.row = row;
      up.column = column;
      up.direction = direction;
      up.resetOriginalPosition((BL) null);
      up.resetSpawnPosition(isAI: up is BL.AIUnit);
      up.row = up.originalRow;
      up.column = up.originalColumn;
    }

    public List<BL.FieldEffect> getFieldEffects(BL.FieldEffectType type)
    {
      List<BL.FieldEffect> fieldEffects = new List<BL.FieldEffect>();
      if (this.fieldEffectList == null)
        return fieldEffects;
      foreach (BL.FieldEffect fieldEffect in this.fieldEffectList.value)
      {
        if (fieldEffect.type == type)
          fieldEffects.Add(fieldEffect);
      }
      return fieldEffects;
    }

    public void initializeIntimate()
    {
      BL.Unit[] array = this.playerUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.is_helper && !x.playerUnit.is_guest)).ToArray<BL.Unit>();
      foreach (BL.Unit unit1 in array)
      {
        BL.Unit unit = unit1;
        this.intimate.add(this.getForceID(unit), unit, ((IEnumerable<BL.Unit>) array).Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.index != unit.index)).ToArray<BL.Unit>(), 5);
      }
    }

    public void initializeIntimatePvp()
    {
      BL.Unit[] array1 = this.playerUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.is_helper && !x.playerUnit.is_guest)).ToArray<BL.Unit>();
      foreach (BL.Unit unit1 in array1)
      {
        BL.Unit unit = unit1;
        this.intimate.add(this.getForceID(unit), unit, ((IEnumerable<BL.Unit>) array1).Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.index != unit.index)).ToArray<BL.Unit>(), 5);
      }
      BL.Unit[] array2 = this.enemyUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.is_helper && !x.playerUnit.is_guest)).ToArray<BL.Unit>();
      foreach (BL.Unit unit2 in array2)
      {
        BL.Unit unit = unit2;
        this.intimate.add(this.getForceID(unit), unit, ((IEnumerable<BL.Unit>) array2).Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.index != unit.index)).ToArray<BL.Unit>(), 5);
      }
    }

    public void updateIntimateByAttack(BL.UnitPosition attackerPosition)
    {
      List<BL.UnitPosition> neighbors = BattleFuncs.getNeighbors(attackerPosition);
      this.intimate.add(this.getForceID(attackerPosition.unit), attackerPosition.unit, neighbors.Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>(), 5);
    }

    public void updateIntimateByDefense(BL.Unit deadUnit)
    {
      if (deadUnit.hp > 0)
        return;
      BL.ForceID forceId = this.getForceID(deadUnit);
      List<BL.Unit> source = forceId == BL.ForceID.player ? this.playerUnits.value : this.enemyUnits.value;
      this.intimate.add(forceId, deadUnit, source.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.index != deadUnit.index && !x.is_helper && !x.playerUnit.is_guest)).ToArray<BL.Unit>(), 2);
    }

    public Tuple<int, int, int>[] getPlayerIntimateResult()
    {
      return this.intimate.intimateDic.Where<KeyValuePair<Tuple<BL.ForceID, int, int>, int>>((Func<KeyValuePair<Tuple<BL.ForceID, int, int>, int>, bool>) (x => x.Key.Item1 == BL.ForceID.player)).Select<KeyValuePair<Tuple<BL.ForceID, int, int>, int>, Tuple<int, int, int>>((Func<KeyValuePair<Tuple<BL.ForceID, int, int>, int>, Tuple<int, int, int>>) (x => Tuple.Create<int, int, int>(x.Key.Item2, x.Key.Item3, x.Value))).ToArray<Tuple<int, int, int>>();
    }

    public Tuple<int, int, int>[] getEnemyIntimateResult()
    {
      return this.intimate.intimateDic.Where<KeyValuePair<Tuple<BL.ForceID, int, int>, int>>((Func<KeyValuePair<Tuple<BL.ForceID, int, int>, int>, bool>) (x => x.Key.Item1 == BL.ForceID.enemy)).Select<KeyValuePair<Tuple<BL.ForceID, int, int>, int>, Tuple<int, int, int>>((Func<KeyValuePair<Tuple<BL.ForceID, int, int>, int>, Tuple<int, int, int>>) (x => Tuple.Create<int, int, int>(x.Key.Item2, x.Key.Item3, x.Value))).ToArray<Tuple<int, int, int>>();
    }

    public void useItemWith(BL.Item item, BL.Unit unit, Action<List<BL.Unit>> f, BL env)
    {
      if (item.amount == 0)
        return;
      int num = 1 + BattleFuncs.gearSkillEffectFilter(unit, unit.enabledSkillEffect(BattleskillEffectLogicEnum.add_supply_level).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        Tuple<int, int> unitCell = BattleFuncs.getUnitCell(unit);
        BL.Panel panel = BattleFuncs.getPanel(unitCell.Item1, unitCell.Item2);
        return x.effect.GetPackedSkillEffect().CheckLandTag(panel, false);
      }))).Sum<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.baseSkillLevel));
      BattleskillSkill skill = item.item.skill;
      int level = num;
      List<BL.ISkillEffectListUnit> targets = new List<BL.ISkillEffectListUnit>();
      targets.Add((BL.ISkillEffectListUnit) unit);
      List<BL.Panel> panels = new List<BL.Panel>();
      BL env1 = env;
      bool? callIsPlayer = new bool?();
      List<BL.Unit> unitList = this.setSkillEffect(skill, level, targets, panels, (BL.BattleSkillResult) null, env1, needEffectTargets: true, callIsPlayer: callIsPlayer).Item1;
      f(unitList);
      --item.amount;
      if (item.amount != 0)
        return;
      this.itemListInBattle.value.Remove(item);
      this.itemListInBattle.commit();
    }

    public List<BL.Unit> getItemTargetUnits(BL.Item item)
    {
      List<BL.Unit> itemTargetUnits = new List<BL.Unit>();
      if (item.item.skill.target_type != BattleskillTargetType.dead_player_single)
      {
        foreach (BL.Unit unit in this.playerUnits.value)
        {
          if (unit.isEnable && !unit.isDead && !unit.skillEffects.HasEffect(item.item.skill))
            itemTargetUnits.Add(unit);
        }
      }
      else
      {
        foreach (BL.Unit unit in this.playerUnits.value)
        {
          if (unit.isEnable && unit.isDead)
            itemTargetUnits.Add(unit);
        }
      }
      return itemTargetUnits;
    }

    public void nextRandom()
    {
      ++this.randomCount;
      this.random = new XorShift(this.randomBase);
      for (int index = 0; index < this.randomCount * 10; ++index)
      {
        int num = (int) this.random.Next();
      }
    }

    public int nowWaveNo => this.currentWave + 1;

    private Dictionary<Tuple<bool, int, int, int, int, int, int>, List<BL.AttackStatusCacheContainer>> attackStatusCacheDic
    {
      get
      {
        if (this.mAttackStatusCacheDic == null)
          this.mAttackStatusCacheDic = new Dictionary<Tuple<bool, int, int, int, int, int, int>, List<BL.AttackStatusCacheContainer>>();
        return this.mAttackStatusCacheDic;
      }
    }

    public void clearAttackStatusCache(bool isAI)
    {
      if (this.mAttackStatusCacheDic == null)
        return;
      foreach (KeyValuePair<Tuple<bool, int, int, int, int, int, int>, List<BL.AttackStatusCacheContainer>> keyValuePair in this.mAttackStatusCacheDic)
      {
        if (keyValuePair.Key.Item1 == isAI)
          keyValuePair.Value.Clear();
      }
    }

    public void attackStatusCacheGC()
    {
      if (this.mAttackStatusCacheDic == null)
        return;
      foreach (List<BL.AttackStatusCacheContainer> statusCacheContainerList in this.mAttackStatusCacheDic.Values)
      {
        HashSet<BL.AttackStatusCacheContainer> statusCacheContainerSet = new HashSet<BL.AttackStatusCacheContainer>();
        foreach (BL.AttackStatusCacheContainer statusCacheContainer in statusCacheContainerList)
        {
          if (statusCacheContainer.checkReadCount(0))
            statusCacheContainer.resetReadCount();
          else
            statusCacheContainerSet.Add(statusCacheContainer);
        }
        foreach (BL.AttackStatusCacheContainer statusCacheContainer in statusCacheContainerSet)
          statusCacheContainerList.Remove(statusCacheContainer);
      }
    }

    private bool getAttackStatusCacheDicList(
      bool isAI,
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      int move_distance,
      int move_range,
      out List<BL.AttackStatusCacheContainer> dl)
    {
      return this.attackStatusCacheDic.TryGetValue(this.makeAttackStatusCacheKey(isAI, attack, attackPanel, defense, defensePanel, move_distance, move_range), out dl);
    }

    private void addAttackStatusCacheDicList(
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      int attackHp,
      bool isAttack,
      bool isHeal,
      int move_distance,
      int move_range,
      bool isAI,
      AttackStatus[] data)
    {
      Tuple<bool, int, int, int, int, int, int> key = this.makeAttackStatusCacheKey(isAI, attack, attackPanel, defense, defensePanel, move_distance, move_range);
      List<BL.AttackStatusCacheContainer> statusCacheContainerList = (List<BL.AttackStatusCacheContainer>) null;
      BL.ClassValue<List<BL.SkillEffect>> skillEffects1 = attackPanel.getSkillEffects(isAI);
      BL.ClassValue<List<BL.SkillEffect>> skillEffects2 = defensePanel.getSkillEffects(isAI);
      if (this.attackStatusCacheDic.TryGetValue(key, out statusCacheContainerList))
      {
        foreach (BL.AttackStatusCacheContainer statusCacheContainer in statusCacheContainerList)
        {
          if (statusCacheContainer.checkBaseValues(isAttack, isHeal, skillEffects1, skillEffects2, attackNeighbors, defenseNeighbors))
          {
            statusCacheContainer.setData(attack, defense, attackNeighbors, defenseNeighbors, attackHp, skillEffects1, skillEffects2, data);
            return;
          }
        }
        statusCacheContainerList.Add(new BL.AttackStatusCacheContainer(attack, defense, attackNeighbors, defenseNeighbors, attackHp, isAttack, isHeal, skillEffects1, skillEffects2, data));
      }
      else
        this.mAttackStatusCacheDic[key] = new List<BL.AttackStatusCacheContainer>()
        {
          new BL.AttackStatusCacheContainer(attack, defense, attackNeighbors, defenseNeighbors, attackHp, isAttack, isHeal, skillEffects1, skillEffects2, data)
        };
    }

    public bool getAttackStatusCache(
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      int attackHp,
      bool isAttack,
      bool isHeal,
      int move_distance,
      int move_range,
      bool isAI,
      out AttackStatus[] ret)
    {
      if (BattleFuncs.hasEnabledOnemanChargeEffects(attack) || BattleFuncs.hasEnabledOnemanChargeEffects(defense) || BattleFuncs.hasEnabledTargetCountEffects(attack) || BattleFuncs.hasEnabledTargetCountEffects(defense) || BattleFuncs.getEnabledCharismaEffects(attack).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.HasKey(BattleskillEffectLogicArgumentEnum.excluding_slanting) && x.effect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) != 0)) || BattleFuncs.getEnabledCharismaEffects(defense).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.HasKey(BattleskillEffectLogicArgumentEnum.excluding_slanting) && x.effect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) != 0)))
      {
        ret = (AttackStatus[]) null;
        return false;
      }
      List<BL.AttackStatusCacheContainer> dl;
      if (!this.getAttackStatusCacheDicList(isAI, attack, attackPanel, defense, defensePanel, move_distance, move_range, out dl))
      {
        ret = (AttackStatus[]) null;
        return false;
      }
      BL.ClassValue<List<BL.SkillEffect>> skillEffects1 = attackPanel.getSkillEffects(isAI);
      BL.ClassValue<List<BL.SkillEffect>> skillEffects2 = defensePanel.getSkillEffects(isAI);
      foreach (BL.AttackStatusCacheContainer statusCacheContainer in dl)
      {
        if (statusCacheContainer.checkBaseValues(isAttack, isHeal, skillEffects1, skillEffects2, attackNeighbors, defenseNeighbors))
        {
          ret = statusCacheContainer.data;
          return statusCacheContainer.checkValues(attack, defense, attackHp);
        }
      }
      ret = (AttackStatus[]) null;
      return false;
    }

    public AttackStatus[] setAttackStatusCache(
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      int attackHp,
      bool isAttack,
      bool isHeal,
      int move_distance,
      int move_range,
      bool isAI,
      AttackStatus[] data)
    {
      this.addAttackStatusCacheDicList(attack, attackPanel, attackNeighbors, defense, defensePanel, defenseNeighbors, attackHp, isAttack, isHeal, move_distance, move_range, isAI, data);
      return data;
    }

    private int makePanelId(BL.Panel panel) => panel.landformID;

    private int makeUnitId(BL.ISkillEffectListUnit unit)
    {
      return unit is BL.AIUnit ? (unit as BL.AIUnit).id : this.getUnitPosition(unit.originalUnit).id;
    }

    private Tuple<bool, int, int, int, int, int, int> makeAttackStatusCacheKey(
      bool isAI,
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      int move_distance,
      int move_range)
    {
      return new Tuple<bool, int, int, int, int, int, int>(isAI, this.makeUnitId(attack), this.makeUnitId(defense), this.makePanelId(attackPanel), this.makePanelId(defensePanel), BL.fieldDistance(attackPanel, defensePanel), this.makeDistanceKey(move_distance, move_range));
    }

    private int makeDistanceKey(int move, int range) => move * 1000 + range;

    public HashSet<BL.ISkillEffectListUnit> getCharismaTargetUnits(BL.ISkillEffectListUnit unit)
    {
      HashSet<BL.ISkillEffectListUnit> charismaTargetUnits = new HashSet<BL.ISkillEffectListUnit>();
      IEnumerable<BL.SkillEffect> enabledCharismaEffects = BattleFuncs.getEnabledCharismaEffects(unit);
      BL.ForceID[][] forceId = new BL.ForceID[2][];
      if (this.getForceID(unit.originalUnit) == BL.ForceID.player)
      {
        forceId[0] = BattleFuncs.ForceIDArrayPlayer;
        forceId[1] = BattleFuncs.ForceIDArrayPlayerTarget;
      }
      else
      {
        forceId[0] = BattleFuncs.ForceIDArrayEnemy;
        forceId[1] = BattleFuncs.ForceIDArrayEnemyTarget;
      }
      for (int effectTarget = 0; effectTarget <= 1; effectTarget++)
      {
        if (enabledCharismaEffects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) ? x.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == effectTarget : effectTarget == 1)))
        {
          if (unit is BL.AIUnit)
          {
            foreach (BL.AIUnit aiUnit in this.aiUnitPositions.value.Where<BL.AIUnit>((Func<BL.AIUnit, bool>) (x => ((IEnumerable<BL.ForceID>) forceId[effectTarget]).Contains<BL.ForceID>(this.getForceID(x.originalUnit)))))
              charismaTargetUnits.Add((BL.ISkillEffectListUnit) aiUnit);
          }
          else
          {
            foreach (BL.Unit unit1 in ((IEnumerable<BL.ForceID>) forceId[effectTarget]).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) this.getForceUnitList(x))))
              charismaTargetUnits.Add((BL.ISkillEffectListUnit) unit1);
          }
        }
      }
      return charismaTargetUnits;
    }

    public HashSet<BL.ISkillEffectListUnit> getOnemanChargeUnits(BL.ISkillEffectListUnit unit)
    {
      HashSet<BL.ISkillEffectListUnit> onemanChargeUnits = new HashSet<BL.ISkillEffectListUnit>();
      BL.ForceID[][] forceId = new BL.ForceID[2][];
      if (this.getForceID(unit.originalUnit) == BL.ForceID.player)
      {
        forceId[0] = BattleFuncs.ForceIDArrayPlayer;
        forceId[1] = BattleFuncs.ForceIDArrayPlayerTarget;
      }
      else
      {
        forceId[0] = BattleFuncs.ForceIDArrayEnemy;
        forceId[1] = BattleFuncs.ForceIDArrayEnemyTarget;
      }
      for (int searchTarget = 0; searchTarget <= 1; searchTarget++)
      {
        foreach (BL.ISkillEffectListUnit unit1 in !(unit is BL.AIUnit) ? (IEnumerable<BL.ISkillEffectListUnit>) ((IEnumerable<BL.ForceID>) forceId[searchTarget]).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) this.getForceUnitList(x))) : (IEnumerable<BL.ISkillEffectListUnit>) this.aiUnitPositions.value.Where<BL.AIUnit>((Func<BL.AIUnit, bool>) (x => ((IEnumerable<BL.ForceID>) forceId[searchTarget]).Contains<BL.ForceID>(this.getForceID(x.originalUnit)))))
        {
          if (BattleFuncs.getEnabledOnemanChargeEffects(unit1).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.search_target) == searchTarget || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.search_target) == 2)))
          {
            onemanChargeUnits.Add(unit1);
          }
          else
          {
            foreach (BL.SkillEffect skillEffect in unit1.skillEffects.All())
            {
              if (skillEffect.effect.IsExistOnemanChargeExtArg())
              {
                BattleFuncs.PackedSkillEffect packedSkillEffect = skillEffect.effect.GetPackedSkillEffect();
                if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_range))
                {
                  onemanChargeUnits.Add(unit1);
                  break;
                }
                if (searchTarget == 0)
                {
                  if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_range))
                  {
                    onemanChargeUnits.Add(unit1);
                    break;
                  }
                }
                else if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_range))
                {
                  onemanChargeUnits.Add(unit1);
                  break;
                }
              }
            }
          }
        }
      }
      return onemanChargeUnits;
    }

    private Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>> routeDic
    {
      get
      {
        if (this.mRouteDic == null)
          this.mRouteDic = new Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>>();
        return this.mRouteDic;
      }
    }

    private Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>> routeDic_IM
    {
      get
      {
        if (this.mRouteDic_IM == null)
          this.mRouteDic_IM = new Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>>();
        return this.mRouteDic_IM;
      }
    }

    public void clearRouteCache()
    {
      this.mRouteDic = (Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>>) null;
      this.mRouteDic_IM = (Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>>) null;
    }

    public Tuple<List<BL.Panel>, int> getTargetRouteWithCache(
      BL.UnitPosition u,
      BL.Panel panel,
      BL.Panel target,
      bool im)
    {
      Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>> dictionary = im ? this.routeDic_IM : this.routeDic;
      UnitMoveType moveType = u.unit.job.move_type;
      if (dictionary.ContainsKey(moveType))
      {
        if (dictionary[moveType].ContainsKey(panel.id))
        {
          if (dictionary[moveType][panel.id].ContainsKey(target.id))
            return dictionary[moveType][panel.id][target.id];
        }
        else if (dictionary[moveType].ContainsKey(target.id) && dictionary[moveType][target.id].ContainsKey(panel.id))
        {
          Tuple<List<BL.Panel>, int> tuple = dictionary[moveType][target.id][panel.id];
          return this.setRouteDic(moveType, panel.id, target.id, new Tuple<List<BL.Panel>, int>(tuple.Item1.AsEnumerable<BL.Panel>().Reverse<BL.Panel>().ToList<BL.Panel>(), tuple.Item2), im);
        }
      }
      int cost;
      Tuple<List<BL.Panel>, int> r = new Tuple<List<BL.Panel>, int>(this.fieldDistanceShortestPath(u, panel, target, im, out cost), cost);
      return this.setRouteDic(moveType, panel.id, target.id, r, im);
    }

    private Tuple<List<BL.Panel>, int> setRouteDic(
      UnitMoveType mtype,
      int id1,
      int id2,
      Tuple<List<BL.Panel>, int> r,
      bool im)
    {
      Dictionary<UnitMoveType, Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>> dictionary = im ? this.routeDic_IM : this.routeDic;
      if (!dictionary.ContainsKey(mtype))
      {
        dictionary[mtype] = new Dictionary<int, Dictionary<int, Tuple<List<BL.Panel>, int>>>();
        dictionary[mtype][id1] = new Dictionary<int, Tuple<List<BL.Panel>, int>>();
      }
      else if (!dictionary[mtype].ContainsKey(id1))
        dictionary[mtype][id1] = new Dictionary<int, Tuple<List<BL.Panel>, int>>();
      dictionary[mtype][id1][id2] = r;
      return r;
    }

    private List<BL.Panel> fieldDistanceShortestPath(
      BL.UnitPosition up,
      BL.Panel start,
      BL.Panel goal,
      bool enabledIgnoreMoveCost,
      out int cost)
    {
      BattleFuncs.AsterNode[] nodes = up.asterNodeCache[enabledIgnoreMoveCost ? 1 : 0];
      int startIdx;
      int goalIdx;
      BattleFuncs.getNodesStartAndGoal(nodes, start, goal, out startIdx, out goalIdx);
      return BattleFuncs.createRouteWithCost(up.unit, nodes, goalIdx, startIdx, out cost, enabledIgnoreMoveCost);
    }

    private Tuple<List<BL.Panel>, int> getRouteTuple(
      BL.UnitPosition up,
      BL.Panel start,
      BL.Panel goal,
      HashSet<BL.Panel> movepanels,
      HashSet<BL.Panel> completePanels = null,
      bool isCache = true)
    {
      BL.Panel panel = (BL.Panel) null;
      float num1 = 1E+17f;
      if (completePanels == null)
        completePanels = movepanels;
      if (!completePanels.Contains(goal))
      {
        foreach (BL.Panel completePanel in completePanels)
        {
          if (completePanel == goal)
            panel = goal;
          int num2 = completePanel.column - goal.column;
          int num3 = completePanel.row - goal.row;
          int num4 = num2 * num2 + num3 * num3;
          if ((double) num4 < (double) num1)
          {
            panel = completePanel;
            num1 = (float) num4;
          }
        }
        goal = panel;
      }
      bool flag = up.unit.HasEnabledSkillEffect(BattleskillEffectLogicEnum.ignore_move_cost);
      if (isCache)
        return this.getTargetRouteWithCache(up, start, goal, flag);
      int startIdx;
      int goalIdx;
      BattleFuncs.AsterNode[] nodes = BattleFuncs.createNodes((IEnumerable<BL.Panel>) movepanels, up.unit, start, goal, out startIdx, out goalIdx, flag);
      int cost;
      return new Tuple<List<BL.Panel>, int>(BattleFuncs.createRouteWithCost(up.unit, nodes, goalIdx, startIdx, out cost, flag), cost);
    }

    public List<BL.Panel> getRouteWithCache(
      BL.UnitPosition up,
      BL.Panel start,
      BL.Panel goal,
      HashSet<BL.Panel> movepanels,
      HashSet<BL.Panel> completePanels = null)
    {
      return this.getRouteTuple(up, start, goal, movepanels, completePanels).Item1;
    }

    public int getRouteCostWithCache(
      BL.UnitPosition up,
      BL.Panel start,
      BL.Panel goal,
      HashSet<BL.Panel> movepanels,
      HashSet<BL.Panel> completePanels = null)
    {
      return this.getRouteTuple(up, start, goal, movepanels, completePanels).Item2;
    }

    public List<BL.Panel> getRouteNonCache(
      BL.UnitPosition up,
      BL.Panel start,
      BL.Panel goal,
      HashSet<BL.Panel> movepanels,
      HashSet<BL.Panel> completePanels = null)
    {
      return this.getRouteTuple(up, start, goal, movepanels, completePanels, false).Item1;
    }

    public int getRouteCostNonCache(
      BL.UnitPosition up,
      BL.Panel start,
      BL.Panel goal,
      HashSet<BL.Panel> movepanels,
      HashSet<BL.Panel> completePanels = null)
    {
      return this.getRouteTuple(up, start, goal, movepanels, completePanels, false).Item2;
    }

    public void useMagicBulletWith(
      BL.MagicBullet mb,
      int attack,
      BL.Unit unit,
      List<BL.Unit> targets,
      Action<Dictionary<BL.Unit, int>, List<BL.Unit>, List<BL.Unit>> f,
      BL env)
    {
      Dictionary<BL.Unit, int> dictionary = new Dictionary<BL.Unit, int>();
      foreach (BL.ISkillEffectListUnit allUnit in BattleFuncs.getAllUnits(false, true, includeJumping: true))
        dictionary.Add(allUnit.originalUnit, allUnit.hp);
      unit.hp -= mb.cost;
      dictionary[unit] = unit.hp;
      Tuple<List<BL.Unit>, int, List<BL.Unit>, bool, bool, List<BL.Panel>, Tuple<List<BL.Unit>, List<BL.Unit>>> tuple = this.setSkillEffect(mb.skill, 1, targets.Select<BL.Unit, BL.ISkillEffectListUnit>((Func<BL.Unit, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) x)).ToList<BL.ISkillEffectListUnit>(), new List<BL.Panel>(), (BL.BattleSkillResult) null, env, (BL.ISkillEffectListUnit) unit, true, true);
      List<BL.Unit> second1 = tuple.Item1;
      List<BL.Unit> second2 = tuple.Item3;
      foreach (BL.Unit target in targets)
      {
        foreach (BattleskillEffect effect in mb.skill.Effects)
        {
          if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.power_heal)
          {
            BL.UnitPosition unitPosition = env.getUnitPosition(target);
            BL.Panel fieldPanel = env.getFieldPanel(unitPosition);
            int healValue = BattleFuncs.getHealValue((BL.ISkillEffectListUnit) target, fieldPanel, attack, mb.skill.skill_type);
            if (target.hp >= 1)
            {
              target.hp += healValue;
              if (target.hp <= 0)
              {
                NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
                if (instance.useGameEngine)
                  instance.gameEngine.applyDeadUnit(target, (BL.Unit) null);
              }
            }
          }
        }
      }
      List<BL.Unit> list = targets.Concat<BL.Unit>((IEnumerable<BL.Unit>) second1).Distinct<BL.Unit>().ToList<BL.Unit>();
      int healHpTotal = 0;
      foreach (BL.Unit key in list)
      {
        if (dictionary.ContainsKey(key) && key.hp > dictionary[key])
          healHpTotal += key.hp - dictionary[key];
      }
      int hp = unit.hp;
      if (hp >= 1 && BattleFuncs.applyServantsJoy((BL.ISkillEffectListUnit) unit, healHpTotal) && !second2.Contains(unit))
        second2.Add(unit);
      if (hp >= 1 && unit.hp <= 0)
      {
        NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
        if (instance.useGameEngine)
          instance.gameEngine.applyDeadUnit(unit, (BL.Unit) null);
      }
      f(dictionary, list, targets.Concat<BL.Unit>((IEnumerable<BL.Unit>) second2).Distinct<BL.Unit>().ToList<BL.Unit>());
      this.getUnitPosition(unit).actionActionUnit(this);
    }

    public virtual BL.SkillResultUnit createSkillResultUnit(BL.UnitPosition up)
    {
      return new BL.SkillResultUnit(up, this);
    }

    private List<BL.UnitPosition> getExecuteSkillEffectsTargets(
      BL.UnitPosition up,
      int[] range,
      BL.ForceID forceId,
      bool isAI = false,
      bool includeJumping = false)
    {
      return range[1] == 999 || range[1] == 1000 ? BattleFuncs.getForceUnits(forceId, isAI, true, includeJumping: range[1] == 1000 | includeJumping).Select<BL.ISkillEffectListUnit, BL.UnitPosition>((Func<BL.ISkillEffectListUnit, BL.UnitPosition>) (x => BattleFuncs.iSkillEffectListUnitToUnitPosition(x))).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (u => range[0] == 0 || u != up)).ToList<BL.UnitPosition>() : BattleFuncs.getTargets(up.row, up.column, range, BattleFuncs.getForceIDArray(forceId), BL.Unit.TargetAttribute.all, isAI, nonFacility: true, includeJumping: includeJumping);
    }

    public List<BL.ExecuteSkillEffectResult> executeTurnInitSkillEffects(
      BL.UnitPosition up,
      int turn)
    {
      List<BL.ExecuteSkillEffectResult> skillEffectResultList = new List<BL.ExecuteSkillEffectResult>();
      if (up.unit.IsJumping)
        return skillEffectResultList;
      foreach (BL.SkillEffect skillEffect in up.unit.skillEffects.Where(BattleskillEffectLogicEnum.self_rebirth))
      {
        if (up.unit.isDead)
        {
          int num1 = up.unit.deadTurn.Max();
          if (turn - num1 >= skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn))
          {
            BL.ExecuteSkillEffectResult skillEffectResult = new BL.ExecuteSkillEffectResult()
            {
              skill = skillEffect.baseSkill
            };
            float num2 = skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
            skillEffectResult.target_prev_hps.Add(up.unit.hp);
            up.unit.hp += Mathf.CeilToInt((float) up.unit.parameter.Hp * num2);
            up.unit.rebirth(this, false, skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_reset_completed) != 0);
            skillEffectResult.targets.Add(up);
            skillEffectResult.target_hps.Add(up.unit.hp);
            skillEffectResultList.Add(skillEffectResult);
          }
        }
      }
      return skillEffectResultList;
    }

    public Tuple<List<BL.ExecuteSkillEffectResult>, bool> executeTurnInitSkillEffects2(
      BL.UnitPosition up,
      int turn,
      XorShift random = null)
    {
      List<BL.ExecuteSkillEffectResult> skillEffectResultList = new List<BL.ExecuteSkillEffectResult>();
      BL.Unit unit = up.unit;
      if (unit.IsJumping)
        return Tuple.Create<List<BL.ExecuteSkillEffectResult>, bool>(skillEffectResultList, false);
      if (!unit.isDead)
        return Tuple.Create<List<BL.ExecuteSkillEffectResult>, bool>(skillEffectResultList, false);
      IEnumerable<BL.SkillEffect> source = BattleFuncs.gearSkillEffectFilter(unit, unit.skillEffects.Where(BattleskillEffectLogicEnum.self_rebirth2, (Func<BL.SkillEffect, bool>) (x =>
      {
        int? nullable;
        if (x.useRemain.HasValue)
        {
          nullable = x.useRemain;
          int num = 0;
          if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
            return false;
        }
        BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
        BattleFuncs.PackedSkillEffect pse = packedSkillEffect;
        BL.Unit unit1 = unit;
        nullable = new int?();
        int? colosseumTurn = nullable;
        nullable = new int?();
        int? unitHp = nullable;
        nullable = new int?();
        int? targetHp = nullable;
        if (!BattleFuncs.checkInvokeSkillEffect(pse, (BL.ISkillEffectListUnit) unit1, colosseumTurn: colosseumTurn, unitHp: unitHp, targetHp: targetHp) || !packedSkillEffect.CheckLandTag(BattleFuncs.getPanel(up.row, up.column), false) || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_seal) == 0 && BattleFuncs.isSealedSkillEffect((BL.ISkillEffectListUnit) unit, x))
          return false;
        int num1 = turn - unit.lastDeadTurn;
        int num2 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.rebirth_start_turn);
        int num3 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.rebirth_end_turn);
        return (num2 == 0 || num1 >= num2) && (num3 == 0 || num1 <= num3);
      })));
      if (random == null)
        random = this.random;
      BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects = new BattleFuncs.ApplyChangeSkillEffects(false);
      IOrderedEnumerable<BL.SkillEffect> orderedEnumerable = source.OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => MasterData.BattleskillSkill[x.baseSkillId].weight)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId));
      bool forceResetCompleted = false;
      foreach (BL.SkillEffect investEffect in (IEnumerable<BL.SkillEffect>) orderedEnumerable)
      {
        Decimal num4 = (Decimal) investEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.rebirth_percentage);
        if (!(num4 != 0M) || !(num4 * 1000000M <= (Decimal) random.NextFixed(1000000U)))
        {
          BL.ExecuteSkillEffectResult skillEffectResult = new BL.ExecuteSkillEffectResult()
          {
            skill = investEffect.baseSkill
          };
          unit.hp = investEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + investEffect.baseSkillLevel * investEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value_skill_ratio) + (int) Math.Ceiling((Decimal) unit.originalUnit.parameter.Hp * ((Decimal) investEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) investEffect.baseSkillLevel * (Decimal) investEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_skill_ratio)));
          forceResetCompleted = investEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_reset_completed) != 0;
          unit.rebirth(this, false, false, forceResetCompleted);
          if (!forceResetCompleted)
            up.recoveryCompleteUnit();
          int skillId = investEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
          if (skillId != 0)
          {
            BL.Skill skill = new BL.Skill() { id = skillId };
            foreach (BL.UnitPosition skillTargetUnit in this.getSkillTargetUnits(up, skill))
            {
              BattleFuncs.investSkillEffect((BL.ISkillEffectListUnit) unit, (BL.ISkillEffectListUnit) skillTargetUnit.unit, skillId, investEffect, false, applyChangeSkillEffects);
              skillEffectResult.targets.Add(skillTargetUnit);
            }
          }
          skillEffectResultList.Add(skillEffectResult);
          if (investEffect.useRemain.HasValue)
          {
            int? useRemain1 = investEffect.useRemain;
            int num5 = 1;
            if (useRemain1.GetValueOrDefault() >= num5 & useRemain1.HasValue)
            {
              BL.SkillEffect skillEffect = investEffect;
              int? useRemain2 = skillEffect.useRemain;
              skillEffect.useRemain = useRemain2.HasValue ? new int?(useRemain2.GetValueOrDefault() - 1) : new int?();
              break;
            }
            break;
          }
          break;
        }
      }
      applyChangeSkillEffects.execute();
      return Tuple.Create<List<BL.ExecuteSkillEffectResult>, bool>(skillEffectResultList, forceResetCompleted);
    }

    public List<BL.ExecuteSkillEffectResult> executeSkillEffects(BL.UnitPosition up)
    {
      List<BL.ExecuteSkillEffectResult> skillEffectResultList = new List<BL.ExecuteSkillEffectResult>();
      if (up.unit.IsJumping || up.unit.hp <= 0)
        return skillEffectResultList;
      List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
      BL.Panel panel = BattleFuncs.getPanel(up);
      foreach (BL.SkillEffect effect in up.unit.skillEffects.Where(BattleskillEffectLogicEnum.self_recovery))
      {
        if (effect.effect.GetPackedSkillEffect().CheckLandTag(panel, false))
        {
          if (BattleFuncs.getTargets(up.row, up.column, new int[2]
          {
            1,
            effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range)
          }, BattleFuncs.getForceIDArray(this.getForceID(up.unit)), BL.Unit.TargetAttribute.all, nonFacility: true).Count == 0)
            skillParams.Add(BattleFuncs.SkillParam.Create(up.unit, effect));
        }
      }
      foreach (BL.SkillEffect effect in up.unit.skillEffects.Where(BattleskillEffectLogicEnum.range_recovery))
      {
        if (effect.effect.GetPackedSkillEffect().CheckLandTag(panel, false))
          skillParams.Add(BattleFuncs.SkillParam.Create(up.unit, effect));
      }
      Action<BL.SkillEffect, Action<BL.UnitPosition>> action = (Action<BL.SkillEffect, Action<BL.UnitPosition>>) ((effect, act) =>
      {
        BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
        pse.SetIgnoreHeader(true);
        if (!BattleFuncs.checkInvokeSkillEffectCommon(pse) || !BattleFuncs.checkInvokeSkillEffectSelf(pse, (BL.ISkillEffectListUnit) up.unit) || !pse.CheckLandTag(panel, false))
          return;
        foreach (BL.UnitPosition skillEffectsTarget in this.getExecuteSkillEffectsTargets(up, new int[2]
        {
          effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
          effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
        }, this.getForceID(up.unit)))
        {
          if ((effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == skillEffectsTarget.unit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == skillEffectsTarget.unit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == skillEffectsTarget.unit.playerUnit.GetElement()) && BattleFuncs.checkInvokeSkillEffectTarget(pse, (BL.ISkillEffectListUnit) skillEffectsTarget.unit) && BattleFuncs.checkInvokeSkillEffectBoth(pse, (BL.ISkillEffectListUnit) up.unit, (BL.ISkillEffectListUnit) skillEffectsTarget.unit))
            act(skillEffectsTarget);
        }
      });
      foreach (BL.SkillEffect skillEffect in up.unit.skillEffects.Where(BattleskillEffectLogicEnum.ratio_recovery))
      {
        BL.SkillEffect effect = skillEffect;
        action(effect, (Action<BL.UnitPosition>) (target =>
        {
          float num = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) ((double) effect.baseSkillLevel * (double) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio) / 100.0);
          skillParams.Add(BattleFuncs.SkillParam.CreateParam(up.unit, effect, (object) Tuple.Create<BL.UnitPosition, int>(target, (int) Math.Ceiling((Decimal) ((float) target.unit.parameter.Hp * num)))));
        }));
      }
      foreach (BL.SkillEffect skillEffect in up.unit.skillEffects.Where(BattleskillEffectLogicEnum.fix_recovery))
      {
        BL.SkillEffect effect = skillEffect;
        action(effect, (Action<BL.UnitPosition>) (target =>
        {
          int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
          skillParams.Add(BattleFuncs.SkillParam.CreateParam(up.unit, effect, (object) Tuple.Create<BL.UnitPosition, int>(target, num)));
        }));
      }
      skillParams = BattleFuncs.gearSkillParamFilter(skillParams).ToList<BattleFuncs.SkillParam>();
      foreach (BattleFuncs.SkillParam skillParam in skillParams.Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.self_recovery)))
      {
        if (up.unit.hp <= 0)
          return skillEffectResultList;
        BL.SkillEffect effect = skillParam.effect;
        BL.ExecuteSkillEffectResult skillEffectResult = new BL.ExecuteSkillEffectResult()
        {
          skill = effect.baseSkill
        };
        float num = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) (effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio)) / 100f;
        skillEffectResult.target_prev_hps.Add(up.unit.hp);
        up.unit.hp += BattleFuncs.getHealValue((BL.ISkillEffectListUnit) up.unit, panel, Mathf.CeilToInt((float) up.unit.parameter.Hp * num), effect.baseSkill.skill_type);
        skillEffectResult.targets.Add(up);
        skillEffectResult.target_hps.Add(up.unit.hp);
        skillEffectResultList.Add(skillEffectResult);
      }
      foreach (BattleFuncs.SkillParam skillParam in skillParams.Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.range_recovery)))
      {
        if (up.unit.hp <= 0)
          return skillEffectResultList;
        BL.SkillEffect effect = skillParam.effect;
        List<BL.UnitPosition> targets = BattleFuncs.getTargets(up.row, up.column, new int[2]
        {
          effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.min_range) ? effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) : 1,
          effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range)
        }, BattleFuncs.getForceIDArray(this.getForceID(up.unit)), BL.Unit.TargetAttribute.all, nonFacility: true);
        BL.ExecuteSkillEffectResult skillEffectResult = new BL.ExecuteSkillEffectResult()
        {
          skill = effect.baseSkill
        };
        foreach (BL.UnitPosition up1 in targets)
        {
          if (up1.unit.hp > 0)
          {
            int healValue = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
            skillEffectResult.target_prev_hps.Add(up1.unit.hp);
            BL.Panel fieldPanel = this.getFieldPanel(up1);
            up1.unit.hp += BattleFuncs.getHealValue((BL.ISkillEffectListUnit) up1.unit, fieldPanel, healValue, effect.baseSkill.skill_type);
            skillEffectResult.targets.Add(up1);
            skillEffectResult.target_hps.Add(up1.unit.hp);
          }
        }
        skillEffectResultList.Add(skillEffectResult);
      }
      if (up.unit.hp <= 0)
        return skillEffectResultList;
      Dictionary<BattleskillSkill, BL.ExecuteSkillEffectResult> dictionary = new Dictionary<BattleskillSkill, BL.ExecuteSkillEffectResult>();
      foreach (BattleFuncs.SkillParam skillParam in skillParams.Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_recovery || x.effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_recovery)))
      {
        BL.SkillEffect effect = skillParam.effect;
        BL.UnitPosition up2 = ((Tuple<BL.UnitPosition, int>) skillParam.param).Item1;
        int healValue = ((Tuple<BL.UnitPosition, int>) skillParam.param).Item2;
        if (up2.unit.hp > 0)
        {
          if (!dictionary.ContainsKey(effect.baseSkill))
            dictionary[effect.baseSkill] = new BL.ExecuteSkillEffectResult()
            {
              skill = effect.baseSkill
            };
          BL.ExecuteSkillEffectResult skillEffectResult = dictionary[effect.baseSkill];
          int index = skillEffectResult.targets.IndexOf(up2);
          BL.Panel fieldPanel = this.getFieldPanel(up2);
          if (index == -1)
          {
            skillEffectResult.target_prev_hps.Add(up2.unit.hp);
            up2.unit.hp += BattleFuncs.getHealValue((BL.ISkillEffectListUnit) up2.unit, fieldPanel, healValue, effect.baseSkill.skill_type);
            skillEffectResult.targets.Add(up2);
            skillEffectResult.target_hps.Add(up2.unit.hp);
          }
          else
          {
            up2.unit.hp += BattleFuncs.getHealValue((BL.ISkillEffectListUnit) up2.unit, fieldPanel, healValue, effect.baseSkill.skill_type);
            skillEffectResult.target_hps[index] = up2.unit.hp;
          }
        }
      }
      foreach (BattleskillSkill key in dictionary.Keys)
        skillEffectResultList.Add(dictionary[key]);
      return skillEffectResultList;
    }

    public IEnumerable<BL.ExecuteSkillEffectResult> executeJumpSkillEffects(
      BL.UnitPosition up,
      BL.UnitPosition targetUp)
    {
      return this.executeFacilitySkill(new BL.FacilitySkillLogicEffect[3]
      {
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectJumpEffectImmediateAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectJumpEffectEnemyInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectJumpEffectPlayerInvestSkilleffect()
      }, up, extData: (object) targetUp);
    }

    private IEnumerable<BL.SkillEffect> getFacilitySkillPhaseEffects(
      BL.FacilitySkillLogicEffect[] logicEffects,
      BL.ISkillEffectListUnit unit,
      int phase)
    {
      foreach (BL.SkillEffect skillPhaseEffect in ((IEnumerable<BL.FacilitySkillLogicEffect>) logicEffects).SelectMany<BL.FacilitySkillLogicEffect, BL.SkillEffect>((Func<BL.FacilitySkillLogicEffect, IEnumerable<BL.SkillEffect>>) (le => unit.skillEffects.Where(le.logicEnum()))))
      {
        if ((skillPhaseEffect.effect.HasKey(BattleskillEffectLogicArgumentEnum.phase) ? skillPhaseEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.phase) : 0) == phase)
          yield return skillPhaseEffect;
      }
    }

    public IEnumerable<BL.ExecuteSkillEffectResult> executePhaseSkillEffects(BL.UnitPosition up)
    {
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      if (!iskillEffectListUnit.IsJumping)
      {
        BL.FacilitySkillLogicEffect[] logicEffects = new BL.FacilitySkillLogicEffect[13]
        {
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectRatioHeal(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectFixHeal(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectRatioAttack(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectFixAttack(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemyInvestSkilleffect(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerInvestSkilleffect(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemyRemoveSkilleffect(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerRemoveSkilleffect(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectInvestLandTag(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemySteal(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerSteal(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemyProvide(),
          (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerProvide()
        };
        IEnumerable<BL.SkillEffect> skillPhaseEffects = this.getFacilitySkillPhaseEffects(logicEffects, iskillEffectListUnit, 0);
        foreach (BL.ExecuteSkillEffectResult skillEffectResult in this.executeFacilitySkill(logicEffects, up, useSkillEffects: skillPhaseEffects))
          yield return skillEffectResult;
      }
    }

    public List<BL.UnitPosition> executeTurnSkillEffects(
      out List<List<BL.ExecuteSkillEffectResult>> result)
    {
      result = new List<List<BL.ExecuteSkillEffectResult>>();
      List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
      BL.FacilitySkillLogicEffect[] logicEffects = new BL.FacilitySkillLogicEffect[13]
      {
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectRatioHeal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectFixHeal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectRatioAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectFixAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemyInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemyRemoveSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerRemoveSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectInvestLandTag(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemySteal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerSteal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectEnemyProvide(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectRangeEffectPlayerProvide()
      };
      BL.ISkillEffectListUnit[] array1 = BattleFuncs.getAllUnitsOrdered(false, true, true).ToArray<BL.ISkillEffectListUnit>();
      BL.ISkillEffectListUnit[] array2 = ((IEnumerable<BL.ISkillEffectListUnit>) array1).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => !x.originalUnit.isFacility || !x.originalUnit.facility.isSkillUnit)).Concat<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) ((IEnumerable<BL.ISkillEffectListUnit>) array1).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => x.originalUnit.isFacility && x.originalUnit.facility.isSkillUnit)).OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.originalUnit.facilitySpawnOrder))).ToArray<BL.ISkillEffectListUnit>();
      List<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>> source1 = new List<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>>();
      foreach (BL.ISkillEffectListUnit unit in array2)
      {
        if (!unit.IsJumping)
        {
          foreach (BL.SkillEffect skillPhaseEffect in this.getFacilitySkillPhaseEffects(logicEffects, unit, 1))
            source1.Add(Tuple.Create<BL.ISkillEffectListUnit, BL.SkillEffect>(unit, skillPhaseEffect));
        }
      }
      foreach (IGrouping<Tuple<BL.ISkillEffectListUnit, int>, Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>> source2 in (IEnumerable<IGrouping<Tuple<BL.ISkillEffectListUnit, int>, Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>>>) source1.OrderBy<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>, int>((Func<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>, int>) (x => x.Item2.effectId)).GroupBy<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>, Tuple<BL.ISkillEffectListUnit, int>>((Func<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>, Tuple<BL.ISkillEffectListUnit, int>>) (x => Tuple.Create<BL.ISkillEffectListUnit, int>(x.Item1, x.Item2.baseSkillId))).OrderByDescending<IGrouping<Tuple<BL.ISkillEffectListUnit, int>, Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>>, int>((Func<IGrouping<Tuple<BL.ISkillEffectListUnit, int>, Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>>, int>) (x => MasterData.BattleskillSkill[x.Key.Item2].weight)).ThenBy<IGrouping<Tuple<BL.ISkillEffectListUnit, int>, Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>>, int>((Func<IGrouping<Tuple<BL.ISkillEffectListUnit, int>, Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>>, int>) (x => x.Key.Item2)))
      {
        BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(source2.Key.Item1);
        List<BL.ExecuteSkillEffectResult> list = this.executeFacilitySkill(logicEffects, unitPosition, useSkillEffects: source2.Select<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>, BL.SkillEffect>((Func<Tuple<BL.ISkillEffectListUnit, BL.SkillEffect>, BL.SkillEffect>) (x => x.Item2))).ToList<BL.ExecuteSkillEffectResult>();
        unitPositionList.Add(unitPosition);
        result.Add(list);
      }
      return unitPositionList;
    }

    public List<BL.ExecuteSkillEffectResult> completedExecuteSkillEffects(
      BL.UnitPosition up,
      HashSet<BL.ISkillEffectListUnit> deads = null)
    {
      bool isAI = up is BL.AIUnit;
      List<BL.ExecuteSkillEffectResult> skillEffectResultList = isAI ? (List<BL.ExecuteSkillEffectResult>) null : new List<BL.ExecuteSkillEffectResult>();
      BL.ISkillEffectListUnit unit = isAI ? up as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) up.unit;
      foreach (IGrouping<int, BL.SkillEffect> grouping in (IEnumerable<IGrouping<int, BL.SkillEffect>>) unit.skillEffects.Where(BattleskillEffectLogicEnum.fix_poison).Concat<BL.SkillEffect>(unit.skillEffects.Where(BattleskillEffectLogicEnum.ratio_poison)).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)).GroupBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).OrderByDescending<IGrouping<int, BL.SkillEffect>, int>((Func<IGrouping<int, BL.SkillEffect>, int>) (x => MasterData.BattleskillSkill[x.Key].weight)).ThenBy<IGrouping<int, BL.SkillEffect>, int>((Func<IGrouping<int, BL.SkillEffect>, int>) (x => x.Key)))
      {
        bool flag = false;
        int hp1 = unit.hp;
        Dictionary<BL.ISkillEffectListUnit, int> dictionary = new Dictionary<BL.ISkillEffectListUnit, int>();
        foreach (BL.SkillEffect headerEffect in (IEnumerable<BL.SkillEffect>) grouping)
        {
          if (BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(headerEffect), unit))
          {
            int num1 = 0;
            int? nullable = headerEffect.moveDistance;
            int num2 = nullable ?? 0;
            switch (headerEffect.effect.EffectLogic.Enum)
            {
              case BattleskillEffectLogicEnum.fix_poison:
                num1 = headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + num2 * headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.distance_ratio);
                break;
              case BattleskillEffectLogicEnum.ratio_poison:
                num1 = Mathf.CeilToInt((float) ((Decimal) unit.originalUnit.parameter.Hp * (Decimal) (headerEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) num2 * headerEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.distance_ratio))));
                break;
            }
            int damage1 = num1;
            BL.ISkillEffectListUnit beUnit = unit;
            nullable = new int?();
            int? colosseumTurn = nullable;
            BL.Panel panel1 = BattleFuncs.getPanel(up);
            int num3 = BattleFuncs.applyDamageCut(2, damage1, beUnit, colosseumTurn: colosseumTurn, invokePanel: panel1);
            int hp2 = unit.hp;
            unit.hp -= num3;
            if (!isAI && hp2 > unit.hp)
            {
              int damage2 = hp2 - unit.hp;
              headerEffect.investUnit.originalUnit.addAttackSubDamage(damage2);
              up.unit.addReceivedSubDamage(damage2);
            }
            if (hp2 >= 1 && headerEffect.investUnit != (BL.Unit) null && num3 >= 0)
            {
              int num4 = headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.heal_target);
              if (num4 != 0)
              {
                int healValue = Mathf.CeilToInt((float) ((Decimal) (hp2 - unit.hp) * (Decimal) headerEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.heal_percentage)));
                if (num4 == 1)
                {
                  if (!headerEffect.investUnit.isFacility)
                  {
                    BL.ISkillEffectListUnit skillEffectListUnit = isAI ? (BL.ISkillEffectListUnit) this.getAIUnit(headerEffect.investUnit) : (BL.ISkillEffectListUnit) headerEffect.investUnit;
                    if (skillEffectListUnit != null && skillEffectListUnit.hp > 0)
                    {
                      if (!dictionary.ContainsKey(skillEffectListUnit))
                        dictionary[skillEffectListUnit] = 0;
                      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(skillEffectListUnit);
                      BL.Panel panel2 = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
                      dictionary[skillEffectListUnit] += BattleFuncs.getHealValue(skillEffectListUnit, panel2, healValue, headerEffect.baseSkill.skill_type);
                    }
                  }
                }
                else if (num4 == 2)
                {
                  foreach (BL.ISkillEffectListUnit forceUnit in BattleFuncs.getForceUnits(this.getForceID(headerEffect.investUnit), isAI, true))
                  {
                    if (forceUnit.hp > 0)
                    {
                      if (!dictionary.ContainsKey(forceUnit))
                        dictionary[forceUnit] = 0;
                      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(forceUnit);
                      BL.Panel panel3 = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
                      dictionary[forceUnit] += BattleFuncs.getHealValue(forceUnit, panel3, healValue, headerEffect.baseSkill.skill_type);
                    }
                  }
                }
              }
            }
            flag = true;
          }
        }
        BL.ExecuteSkillEffectResult skillEffectResult1;
        if (!(skillEffectResultList != null & flag))
        {
          skillEffectResult1 = (BL.ExecuteSkillEffectResult) null;
        }
        else
        {
          skillEffectResult1 = new BL.ExecuteSkillEffectResult();
          skillEffectResult1.skill = MasterData.BattleskillSkill[grouping.Key];
        }
        BL.ExecuteSkillEffectResult skillEffectResult2 = skillEffectResult1;
        if (skillEffectResult2 != null)
        {
          skillEffectResult2.targets.Add(up);
          skillEffectResult2.target_prev_hps.Add(hp1);
          skillEffectResult2.target_hps.Add(unit.hp);
          skillEffectResultList.Add(skillEffectResult2);
        }
        foreach (BL.ISkillEffectListUnit key in dictionary.Keys)
        {
          if (key.hp > 0)
          {
            int hp3 = key.hp;
            key.hp += dictionary[key];
            if (key.hp <= 0 && deads != null)
              deads.Add(key);
            if (skillEffectResult2 != null)
            {
              skillEffectResult2.second_targets.Add(key.originalUnit);
              skillEffectResult2.second_target_prev_hps.Add(hp3);
              skillEffectResult2.second_target_hps.Add(key.hp);
            }
          }
        }
      }
      if (unit.skillEffects.AilmentExecuted(this, unit) && !isAI)
        up.unit.commit();
      if (!isAI)
        up.unit.mIsExecCompletedSkillEffect = true;
      return skillEffectResultList;
    }

    private Tuple<int, bool, bool> setSkillEffectSub(
      HashSet<BL.Unit> effectTargets,
      HashSet<BL.Unit> displayNumberTargets,
      int investSkillId,
      BattleskillSkill skill,
      int level,
      List<BL.ISkillEffectListUnit> targets,
      List<BL.Panel> panels,
      BL.BattleSkillResult bsr,
      BL env,
      BL.ISkillEffectListUnit useUnit,
      BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects,
      XorShift random,
      int nowUseCount,
      bool? callIsPlayer,
      HashSet<BL.Panel> effectPanelTargets,
      List<BL.Unit> createFacilities,
      List<BL.Unit> destructFacilities)
    {
      bool isAI = useUnit != null && useUnit is BL.AIUnit;
      int val1_1 = 0;
      bool flag1 = false;
      bool flag2 = false;
      BL.ISkillEffectListUnit target1 = targets.Count > 0 ? targets[0] : (BL.ISkillEffectListUnit) null;
      IEnumerable<BL.ISkillEffectListUnit> skillEffectListUnits;
      if (useUnit != null && (((IEnumerable<BattleskillEffect>) skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.shift_break || x.EffectLogic.Enum == BattleskillEffectLogicEnum.map_shift)) || skill.target_type == BattleskillTargetType.panel_single))
        skillEffectListUnits = (IEnumerable<BL.ISkillEffectListUnit>) new BL.ISkillEffectListUnit[1]
        {
          useUnit
        };
      else
        skillEffectListUnits = (IEnumerable<BL.ISkillEffectListUnit>) targets;
      List<int> intList = new List<int>();
      Judgement.BattleParameter battleParameter = (Judgement.BattleParameter) null;
      foreach (BL.ISkillEffectListUnit skillEffectListUnit in skillEffectListUnits)
      {
        BL.ISkillEffectListUnit target = skillEffectListUnit;
        BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(target);
        applyChangeSkillEffects.add(unitPosition1, target, useUnit == target);
        IEnumerable<BattleskillEffect> source1 = ((IEnumerable<BattleskillEffect>) skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum != BattleskillEffectLogicEnum.hp_consume && x.EffectLogic.Enum != BattleskillEffectLogicEnum.skill_chain && x.EffectLogic.Enum != BattleskillEffectLogicEnum.change_skill_range && x.EffectLogic.Enum != BattleskillEffectLogicEnum.change_skill_use_count && x.EffectLogic.Enum != BattleskillEffectLogicEnum.random_choice && x.EffectLogic.Enum != BattleskillEffectLogicEnum.again_use_skill && x.EffectLogic.Enum != BattleskillEffectLogicEnum.use_skill_count_range_effect && x.EffectLogic.Enum != BattleskillEffectLogicEnum.percentage_hp_consume_magic && !x.EffectLogic.HasTag(BattleskillEffectTag.ext_arg) && x.checkLevel(level) && x.checkUseSkillCount(nowUseCount)));
        bool? isTargetEnemy = new bool?();
        if (useUnit != null && (skill.target_type == BattleskillTargetType.complex_range || skill.target_type == BattleskillTargetType.complex_single))
        {
          isTargetEnemy = new bool?(env.getForceID(useUnit.originalUnit) != env.getForceID(target.originalUnit));
          source1 = source1.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x =>
          {
            int num1 = x.is_targer_enemy ? 1 : 0;
            bool? nullable = isTargetEnemy;
            int num2 = nullable.GetValueOrDefault() ? 1 : 0;
            return num1 == num2 & nullable.HasValue;
          }));
        }
        else if (skill.skill_type == BattleskillSkillType.call && skill.IsCallTargetComplex && callIsPlayer.HasValue)
        {
          isTargetEnemy = new bool?(!((IEnumerable<BL.ForceID>) (callIsPlayer.Value ? BattleFuncs.ForceIDArrayPlayer : BattleFuncs.ForceIDArrayPlayerTarget)).Contains<BL.ForceID>(env.getForceID(target.originalUnit)));
          source1 = source1.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x =>
          {
            int num3 = x.is_targer_enemy ? 1 : 0;
            bool? nullable = isTargetEnemy;
            int num4 = nullable.GetValueOrDefault() ? 1 : 0;
            return num3 == num4 & nullable.HasValue;
          }));
        }
        foreach (BattleskillEffect battleskillEffect in source1)
        {
          BattleskillEffect effect = battleskillEffect;
          if (effect.EffectLogic.HasTag(BattleskillEffectTag.immediately))
          {
            if (BattleFuncs.canUseImmediateSkillEffect(effect, target, out bool _, useUnit, target1, callIsPlayer, battleParameter) && (panels.Count != 1 || BattleFuncs.canUseImmediatePanelSkillEffect(effect, panels[0], useUnit)))
            {
              if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_lv_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_lv_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.parameter_reference_heal)
              {
                NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
                if (useUnit == null || !instance.isRaidBoss(target.originalUnit) || env.getForceID(useUnit.originalUnit) == env.getForceID(target.originalUnit))
                {
                  if (skill.skill_type == BattleskillSkillType.call)
                  {
                    if (!skill.IsCallTargetEnemy)
                    {
                      if (skill.IsCallTargetComplex)
                      {
                        bool? nullable = isTargetEnemy;
                        bool flag3 = true;
                        if (!(nullable.GetValueOrDefault() == flag3 & nullable.HasValue))
                          goto label_20;
                      }
                      else
                        goto label_20;
                    }
                    if (instance.isRaidBoss(target.originalUnit))
                      continue;
                  }
                }
                else
                  continue;
              }
label_20:
              if (effect.EffectLogic.Enum != BattleskillEffectLogicEnum.reduct_release_skill_turn && effect.EffectLogic.Enum != BattleskillEffectLogicEnum.recovery_command_skill_use && effect.EffectLogic.Enum != BattleskillEffectLogicEnum.fix_immediate_damage && effect.EffectLogic.Enum != BattleskillEffectLogicEnum.ratio_immediate_damage && effect.EffectLogic.Enum != BattleskillEffectLogicEnum.map_shift && skill.target_type != BattleskillTargetType.panel_single)
              {
                effectTargets?.Add(target.originalUnit);
                if ((effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_lv_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_lv_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.parameter_reference_heal || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_rebirth || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_rebirth) && displayNumberTargets != null)
                  displayNumberTargets.Add(target.originalUnit);
              }
              int targetPrevHp = target.hp;
              Action<int> action1 = (Action<int>) (healValue =>
              {
                int num5 = targetPrevHp + healValue;
                if (num5 <= target.originalUnit.parameter.Hp)
                  return;
                Decimal num6 = effect.HasKey(BattleskillEffectLogicArgumentEnum.over_heal_shield_percentage) ? (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.over_heal_shield_percentage) : 0M;
                if (num6 <= 0M)
                  return;
                int key = effect.GetInt(BattleskillEffectLogicArgumentEnum.over_heal_shield_skill_id);
                if (!MasterData.BattleskillSkill.ContainsKey(key))
                  return;
                BattleskillSkill skill1 = MasterData.BattleskillSkill[key];
                if (skill1.Effects.Length == 0)
                  return;
                BattleskillEffect effect1 = skill1.Effects[0];
                if (effect1.effect_logic.Enum != BattleskillEffectLogicEnum.damage_shield || BattleFuncs.checkPassiveEffectEnable(effect1, target) == 0)
                  return;
                BL.SkillEffect effect2 = BL.SkillEffect.FromMasterData(effect1, skill1, 1, investUnit: useUnit != null ? useUnit.originalUnit : (BL.Unit) null, investSkillId: investSkillId, investTurn: this.phaseState.absoluteTurnCount);
                target.skillEffects.Add(effect2);
                int num7 = (int) Math.Ceiling((Decimal) (num5 - target.originalUnit.parameter.Hp) * num6);
                int num8 = effect1.GetInt(BattleskillEffectLogicArgumentEnum.value);
                if (num7 > num8)
                  num7 = num8;
                int num9 = num8 - num7;
                BL.SkillEffect skillEffect = effect2;
                float[] numArray;
                if (num9 != 0)
                  numArray = new float[1]{ (float) num9 };
                else
                  numArray = (float[]) null;
                skillEffect.work = numArray;
              });
              Action<BL.ISkillEffectListUnit> action2 = (Action<BL.ISkillEffectListUnit>) (u =>
              {
                if (isAI)
                  return;
                NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
                if (!instance.useGameEngine)
                  return;
                instance.gameEngine.applyDeadUnit(u.originalUnit, (BL.Unit) null);
              });
              switch (effect.EffectLogic.Enum)
              {
                case BattleskillEffectLogicEnum.fix_heal:
                  if (target.hp >= 1)
                  {
                    int healValue = BattleFuncs.getHealValue(target, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), effect.GetInt(BattleskillEffectLogicArgumentEnum.value), skill.skill_type);
                    target.hp += healValue;
                    action2(target);
                    action1(healValue);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.ratio_heal:
                  if (target.hp >= 1)
                  {
                    int num10 = BattleFuncs.getHealValue(target, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), (int) ((Decimal) target.originalUnit.parameter.Hp * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage)), skill.skill_type);
                    int num11 = effect.HasKey(BattleskillEffectLogicArgumentEnum.limit) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.limit) : 0;
                    if (num11 != 0)
                    {
                      if (num10 > 0 && num10 > num11)
                        num10 = num11;
                      else if (num10 < 0 && num10 < -num11)
                        num10 = -num11;
                    }
                    target.hp += num10;
                    action2(target);
                    action1(num10);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.power_heal:
                  continue;
                case BattleskillEffectLogicEnum.remove_skilleffect:
                  BattleFuncs.removeSkillEffect(effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_id) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) : 0, effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_type) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type) : 0, effect.HasKey(BattleskillEffectLogicArgumentEnum.invest_type) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type) : 0, effect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0, effect.HasKey(BattleskillEffectLogicArgumentEnum.range_effect_remove_flag) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.range_effect_remove_flag) : 0, target, applyChangeSkillEffects, useUnit, effect, useUnit);
                  continue;
                case BattleskillEffectLogicEnum.fix_lv_heal:
                  if (target.hp >= 1)
                  {
                    int healValue = BattleFuncs.getHealValue(target, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + level * effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio), skill.skill_type);
                    target.hp += healValue;
                    action2(target);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.ratio_lv_heal:
                  if (target.hp >= 1)
                  {
                    int healValue = BattleFuncs.getHealValue(target, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), (int) ((Decimal) target.originalUnit.parameter.Hp * ((Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) level * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio))), skill.skill_type);
                    target.hp += healValue;
                    action2(target);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.invest_skilleffect_im:
                  int num12 = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                  if (num12 != 0 && MasterData.BattleskillSkill.ContainsKey(num12) && MasterData.BattleskillSkill[num12].skill_type == BattleskillSkillType.ailment)
                  {
                    Decimal lottery = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest) + (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest_levelup) * (Decimal) level;
                    BL.SkillEffect[] array = BattleFuncs.getAilmentTriggerSkillEffects(num12, target, useUnit, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column)).ToArray<BL.SkillEffect>();
                    BL.SkillEffect perfectAilmentResist = BattleFuncs.getPerfectAilmentResist(BattleFuncs.getAilmentResistEffects(num12, target, useUnit));
                    if (perfectAilmentResist != null)
                    {
                      if (perfectAilmentResist.useRemain.HasValue)
                      {
                        BL.SkillEffect skillEffect = perfectAilmentResist;
                        int? useRemain1 = skillEffect.useRemain;
                        skillEffect.useRemain = useRemain1.HasValue ? new int?(useRemain1.GetValueOrDefault() - 1) : new int?();
                        if (!isAI)
                        {
                          int? useRemain2 = perfectAilmentResist.useRemain;
                          int num13 = 0;
                          if (useRemain2.GetValueOrDefault() == num13 & useRemain2.HasValue)
                            target.originalUnit.commit();
                        }
                      }
                    }
                    else
                    {
                      List<BL.SkillEffect> useResistEffects;
                      if (BattleFuncs.isAilmentInvest(lottery, num12, target, useUnit, random, new int?(), out useResistEffects, (TurnHp) null, new int?(), new int?(), false))
                      {
                        BL.Skill[] skillArray = BattleFuncs.ailmentInvest(num12, target);
                        if (skillArray != null)
                        {
                          foreach (BL.Skill skill2 in skillArray)
                          {
                            foreach (BattleskillEffect effect3 in skill2.skill.Effects)
                              target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect3, skill2.skill, 1, investUnit: useUnit != null ? useUnit.originalUnit : (BL.Unit) null, investSkillId: investSkillId, investTurn: this.phaseState.absoluteTurnCount));
                          }
                          if (!isAI)
                            target.originalUnit.commit();
                        }
                      }
                      foreach (BL.SkillEffect skillEffect1 in useResistEffects)
                      {
                        if (skillEffect1.useRemain.HasValue)
                        {
                          BL.SkillEffect skillEffect2 = skillEffect1;
                          int? useRemain3 = skillEffect2.useRemain;
                          skillEffect2.useRemain = useRemain3.HasValue ? new int?(useRemain3.GetValueOrDefault() - 1) : new int?();
                          if (!isAI)
                          {
                            int? useRemain4 = skillEffect1.useRemain;
                            int num14 = 0;
                            if (useRemain4.GetValueOrDefault() == num14 & useRemain4.HasValue)
                              target.originalUnit.commit();
                          }
                        }
                      }
                    }
                    if (array != null)
                    {
                      foreach (BL.SkillEffect skillEffect in array)
                      {
                        int key = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                        if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
                        {
                          BattleskillSkill skill3 = MasterData.BattleskillSkill[key];
                          foreach (BattleskillEffect effect4 in skill3.Effects)
                            target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect4, skill3, 1, investUnit: target.originalUnit, investSkillId: skillEffect.baseSkillId, investTurn: this.phaseState.absoluteTurnCount), checkEnableUnit: target);
                          if (!isAI)
                            target.originalUnit.commit();
                        }
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.reduct_release_skill_turn:
                  using (List<BL.UnitPosition>.Enumerator enumerator = this.getExecuteSkillEffectsTargets(unitPosition1, new int[2]
                  {
                    effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
                    effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
                  }, this.getForceID(unitPosition1.unit), (isAI ? 1 : 0) != 0).GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(enumerator.Current);
                      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == iskillEffectListUnit.originalUnit.playerUnit.GetElement())
                      {
                        if (iskillEffectListUnit.hasOugi)
                        {
                          int num15 = effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
                          if (num15 > 0)
                            iskillEffectListUnit.ougi.useTurn -= num15;
                          else if (num15 < 0)
                          {
                            if (iskillEffectListUnit.ougi.useTurn < this.phaseState.absoluteTurnCount)
                              iskillEffectListUnit.ougi.useTurn = this.phaseState.absoluteTurnCount;
                            iskillEffectListUnit.ougi.useTurn -= num15;
                          }
                        }
                        effectTargets?.Add(iskillEffectListUnit.originalUnit);
                      }
                    }
                    continue;
                  }
                case BattleskillEffectLogicEnum.recovery_command_skill_use:
                  using (List<BL.UnitPosition>.Enumerator enumerator = this.getExecuteSkillEffectsTargets(unitPosition1, new int[2]
                  {
                    effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
                    effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
                  }, this.getForceID(unitPosition1.unit), (isAI ? 1 : 0) != 0).GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(enumerator.Current);
                      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == iskillEffectListUnit.originalUnit.playerUnit.GetElement())
                      {
                        int num16 = effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
                        foreach (BL.Skill skill4 in iskillEffectListUnit.skills)
                        {
                          BL.Skill s = skill4;
                          if (s.remain.HasValue && (num16 <= 0 || !((IEnumerable<BattleskillEffect>) s.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.cant_recovery_command_skill_use && x.checkLevel(s.level)))))
                          {
                            BL.Skill skill5 = s;
                            int? remain = skill5.remain;
                            int num17 = num16;
                            skill5.remain = remain.HasValue ? new int?(remain.GetValueOrDefault() + num17) : new int?();
                            int num18 = s.useCount + (s.level - 1);
                            if (s.maxUseCount != 0 && num18 > s.maxUseCount)
                              num18 = s.maxUseCount;
                            remain = s.remain;
                            int num19 = num18;
                            if (remain.GetValueOrDefault() > num19 & remain.HasValue)
                            {
                              s.remain = new int?(num18);
                            }
                            else
                            {
                              remain = s.remain;
                              int num20 = 0;
                              if (remain.GetValueOrDefault() < num20 & remain.HasValue)
                                s.remain = new int?(0);
                            }
                          }
                        }
                        effectTargets?.Add(iskillEffectListUnit.originalUnit);
                      }
                    }
                    continue;
                  }
                case BattleskillEffectLogicEnum.fix_rebirth:
                  target.hp += effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + level * effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
                  if (target.originalUnit.isDead)
                  {
                    target.originalUnit.rebirth(env, false, effect.GetInt(BattleskillEffectLogicArgumentEnum.is_reset_completed) != 0);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.ratio_rebirth:
                  target.hp += (int) ((Decimal) target.originalUnit.parameter.Hp * ((Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) level * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
                  if (target.originalUnit.isDead)
                  {
                    target.originalUnit.rebirth(env, false, effect.GetInt(BattleskillEffectLogicArgumentEnum.is_reset_completed) != 0);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.rescue:
                  if (useUnit != null)
                  {
                    BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                    int move = unitPosition1.unit.parameter.Move;
                    int row1 = unitPosition2.row;
                    int column1 = unitPosition2.column;
                    int num21 = unitPosition1.row - unitPosition2.row;
                    int num22 = unitPosition1.column - unitPosition2.column;
                    if (num21 > 0 && num22 >= 0)
                    {
                      if (Mathf.Abs(num21) >= Mathf.Abs(num22))
                        ++row1;
                      else
                        ++column1;
                    }
                    else if (num21 <= 0 && num22 > 0)
                    {
                      if (Mathf.Abs(num22) >= Mathf.Abs(num21))
                        ++column1;
                      else
                        --row1;
                    }
                    else if (num21 < 0 && num22 <= 0)
                    {
                      if (Mathf.Abs(num21) >= Mathf.Abs(num22))
                        --row1;
                      else
                        --column1;
                    }
                    else if (Mathf.Abs(num22) >= Mathf.Abs(num21))
                      --column1;
                    else
                      ++row1;
                    if (row1 >= env.getFieldHeight())
                      row1 = env.getFieldHeight() - 1;
                    if (column1 >= env.getFieldWidth())
                      column1 = env.getFieldWidth() - 1;
                    if (row1 < 0)
                      row1 = 0;
                    if (column1 < 0)
                      column1 = 0;
                    BL.Unit[] ignoreUnits = new BL.Unit[1]
                    {
                      target.originalUnit
                    };
                    if (BattleFuncs.isResetPositionOK(target.originalUnit, row1, column1, move, (IEnumerable<BL.Unit>) ignoreUnits))
                    {
                      RecoveryUtility.resetPosition(unitPosition1, row1, column1, env, true);
                      applyChangeSkillEffects.clearMovePanelCacheAll = true;
                      applyChangeSkillEffects.setMoveUnitDistance = true;
                      continue;
                    }
                    bool flag4 = false;
                    for (int index1 = 1; index1 <= 50; ++index1)
                    {
                      int row2 = row1 + index1;
                      int column2 = column1;
                      for (int index2 = 0; index2 < index1; ++index2)
                      {
                        if (BattleFuncs.isResetPositionOK(target.originalUnit, row2, column2, move, (IEnumerable<BL.Unit>) ignoreUnits))
                        {
                          RecoveryUtility.resetPosition(unitPosition1, row2, column2, env, true);
                          applyChangeSkillEffects.clearMovePanelCacheAll = true;
                          applyChangeSkillEffects.setMoveUnitDistance = true;
                          flag4 = true;
                          break;
                        }
                        --row2;
                        ++column2;
                      }
                      if (!flag4)
                      {
                        for (int index3 = 0; index3 < index1; ++index3)
                        {
                          if (BattleFuncs.isResetPositionOK(target.originalUnit, row2, column2, move, (IEnumerable<BL.Unit>) ignoreUnits))
                          {
                            RecoveryUtility.resetPosition(unitPosition1, row2, column2, env, true);
                            applyChangeSkillEffects.clearMovePanelCacheAll = true;
                            applyChangeSkillEffects.setMoveUnitDistance = true;
                            flag4 = true;
                            break;
                          }
                          --row2;
                          --column2;
                        }
                        if (!flag4)
                        {
                          for (int index4 = 0; index4 < index1; ++index4)
                          {
                            if (BattleFuncs.isResetPositionOK(target.originalUnit, row2, column2, move, (IEnumerable<BL.Unit>) ignoreUnits))
                            {
                              RecoveryUtility.resetPosition(unitPosition1, row2, column2, env, true);
                              applyChangeSkillEffects.clearMovePanelCacheAll = true;
                              applyChangeSkillEffects.setMoveUnitDistance = true;
                              flag4 = true;
                              break;
                            }
                            ++row2;
                            --column2;
                          }
                          if (!flag4)
                          {
                            for (int index5 = 0; index5 < index1; ++index5)
                            {
                              if (BattleFuncs.isResetPositionOK(target.originalUnit, row2, column2, move, (IEnumerable<BL.Unit>) ignoreUnits))
                              {
                                RecoveryUtility.resetPosition(unitPosition1, row2, column2, env, true);
                                applyChangeSkillEffects.clearMovePanelCacheAll = true;
                                applyChangeSkillEffects.setMoveUnitDistance = true;
                                flag4 = true;
                                break;
                              }
                              ++row2;
                              ++column2;
                            }
                            if (flag4)
                              break;
                          }
                          else
                            break;
                        }
                        else
                          break;
                      }
                      else
                        break;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.fix_immediate_damage:
                case BattleskillEffectLogicEnum.ratio_immediate_damage:
                  List<BL.UnitPosition> skillEffectsTargets = this.getExecuteSkillEffectsTargets(unitPosition1, new int[2]
                  {
                    effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
                    effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
                  }, this.getForceID(unitPosition1.unit), (isAI ? 1 : 0) != 0, (skill.max_range == 1000 ? 1 : 0) != 0);
                  int num23 = effect.GetInt(BattleskillEffectLogicArgumentEnum.min_hp);
                  using (List<BL.UnitPosition>.Enumerator enumerator = skillEffectsTargets.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      BL.UnitPosition current = enumerator.Current;
                      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(current);
                      if (iskillEffectListUnit.hp > 0)
                      {
                        int damage1 = effect.effect_logic.Enum != BattleskillEffectLogicEnum.fix_immediate_damage ? Mathf.CeilToInt((float) ((Decimal) iskillEffectListUnit.originalUnit.parameter.Hp * (Decimal) (effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) level * effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)))) : effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + level * effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
                        if (skill.skill_type == BattleskillSkillType.command || skill.skill_type == BattleskillSkillType.release)
                          damage1 = BattleFuncs.applyDamageCut(skill.skill_type == BattleskillSkillType.command ? 4 : 5, damage1, iskillEffectListUnit, useUnit, invokePanel: BattleFuncs.getPanel(current.row, current.column));
                        if (damage1 >= 1)
                        {
                          int hp = iskillEffectListUnit.hp;
                          if (iskillEffectListUnit.hp > num23)
                          {
                            iskillEffectListUnit.hp -= damage1;
                            if (iskillEffectListUnit.hp < num23)
                              iskillEffectListUnit.hp = num23;
                          }
                          if (hp > iskillEffectListUnit.hp)
                            iskillEffectListUnit.skillEffects.RemoveEffect(1000418, env, iskillEffectListUnit);
                          if (iskillEffectListUnit.hp <= 0)
                          {
                            if (useUnit != null)
                            {
                              if (!isAI)
                              {
                                ++useUnit.originalUnit.killCount;
                                iskillEffectListUnit.originalUnit.killedBy = useUnit.originalUnit;
                              }
                              useUnit.skillEffects.AddKillCount(1);
                            }
                            if (!isAI)
                            {
                              if (env.getForceID(iskillEffectListUnit.originalUnit) == BL.ForceID.player)
                                env.updateIntimateByDefense(iskillEffectListUnit.originalUnit);
                              NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
                              if (instance.useGameEngine)
                                instance.gameEngine.applyDeadUnit(iskillEffectListUnit.originalUnit, (BL.Unit) null);
                            }
                          }
                          if (!isAI && useUnit != null)
                          {
                            int damage2 = hp - iskillEffectListUnit.hp;
                            if (damage2 > 0)
                            {
                              if (!useUnit.originalUnit.isFacility && !iskillEffectListUnit.originalUnit.isFacility)
                                useUnit.originalUnit.attackDamage += damage2;
                              else
                                useUnit.originalUnit.addAttackSubDamage(damage2);
                              current.unit.addReceivedSubDamage(damage2);
                            }
                          }
                        }
                        effectTargets?.Add(iskillEffectListUnit.originalUnit);
                        displayNumberTargets?.Add(iskillEffectListUnit.originalUnit);
                        flag1 = true;
                      }
                    }
                    continue;
                  }
                case BattleskillEffectLogicEnum.attract:
                  if (useUnit != null)
                  {
                    BL.UnitPosition unitPosition3 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                    int[] attractDelta = BattleFuncs.getAttractDelta(unitPosition3, unitPosition1);
                    BL.Unit[] ignoreUnits = new BL.Unit[2]
                    {
                      useUnit.originalUnit,
                      target.originalUnit
                    };
                    if (BattleFuncs.isResetPositionOK(useUnit.originalUnit, unitPosition3.row + attractDelta[0], unitPosition3.column + attractDelta[1], unitPosition3.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits) && BattleFuncs.isResetPositionOK(target.originalUnit, unitPosition1.row + attractDelta[0], unitPosition1.column + attractDelta[1], unitPosition1.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits))
                    {
                      RecoveryUtility.resetPosition(unitPosition3, unitPosition3.row + attractDelta[0], unitPosition3.column + attractDelta[1], env, true, true);
                      RecoveryUtility.resetPosition(unitPosition1, unitPosition1.row + attractDelta[0], unitPosition1.column + attractDelta[1], env, true);
                      applyChangeSkillEffects.clearMovePanelCacheAll = true;
                      if (!isAI)
                      {
                        env.setCurrentField(unitPosition3.row, unitPosition3.column);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.shift_break:
                  int posY;
                  int posX;
                  if (useUnit != null && target1 != null && BattleFuncs.getShiftBreakPosition(useUnit, target1, effect.GetInt(BattleskillEffectLogicArgumentEnum.range), out posY, out posX))
                  {
                    RecoveryUtility.resetPosition(unitPosition1, posY, posX, env, true, true);
                    applyChangeSkillEffects.clearMovePanelCacheAll = true;
                    int key1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                    if (key1 != 0 && MasterData.BattleskillSkill.ContainsKey(key1))
                    {
                      BattleskillSkill skill6 = MasterData.BattleskillSkill[key1];
                      foreach (BattleskillEffect effect5 in skill6.Effects)
                        target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect5, skill6, 1, investUnit: useUnit.originalUnit, investSkillId: skill.ID, investTurn: this.phaseState.absoluteTurnCount), checkEnableUnit: target);
                      if (!isAI)
                        target.originalUnit.commit();
                    }
                    if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_complete) == 0)
                    {
                      val1_1 = Math.Max(val1_1, 1);
                      int[] numArray = new int[3]
                      {
                        300001381,
                        300001382,
                        300001383
                      };
                      foreach (int key2 in numArray)
                      {
                        BattleskillSkill skill7 = MasterData.BattleskillSkill[key2];
                        foreach (BattleskillEffect effect6 in skill7.Effects)
                          target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect6, skill7, 1, isDontDisplay: true));
                      }
                      if (!isAI)
                        target.originalUnit.commit();
                    }
                    if (!isAI)
                    {
                      env.setCurrentField(unitPosition1.row, unitPosition1.column);
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.map_shift:
                  if (useUnit != null && panels.Count == 1)
                  {
                    BL.Panel panel = panels[0];
                    if (BattleFuncs.isResetPositionOK(useUnit.originalUnit, panel.row, panel.column, unitPosition1.unit.parameter.Move, (IEnumerable<BL.Unit>) new BL.Unit[1]
                    {
                      useUnit.originalUnit
                    }) && BattleFuncs.canWarpPanel(useUnit, panel.row, panel.column))
                    {
                      RecoveryUtility.resetPosition(unitPosition1, panel.row, panel.column, env, true, true);
                      applyChangeSkillEffects.clearMovePanelCacheAll = true;
                      flag2 = true;
                      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_complete) == 0)
                      {
                        val1_1 = Math.Max(val1_1, 1);
                        int[] numArray = new int[3]
                        {
                          300001381,
                          300001382,
                          300001383
                        };
                        foreach (int key in numArray)
                        {
                          BattleskillSkill skill8 = MasterData.BattleskillSkill[key];
                          foreach (BattleskillEffect effect7 in skill8.Effects)
                            target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect7, skill8, 1, isDontDisplay: true));
                        }
                        if (!isAI)
                          target.originalUnit.commit();
                      }
                      if (!isAI)
                      {
                        env.setCurrentField(panel);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.keep_away:
                  if (useUnit != null)
                  {
                    BL.UnitPosition unitPosition4 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                    int penetrateCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.range);
                    List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(unitPosition4.row, unitPosition4.column, unitPosition1.row, unitPosition1.column, penetrateCount);
                    BL.Unit[] ignoreUnits = new BL.Unit[0];
                    using (IEnumerator<Tuple<int, int, int>> enumerator = penetratePosition.OrderByDescending<Tuple<int, int, int>, int>((Func<Tuple<int, int, int>, int>) (x => x.Item3)).GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        Tuple<int, int, int> current = enumerator.Current;
                        if (BattleFuncs.isResetPositionOK(target.originalUnit, current.Item1, current.Item2, unitPosition1.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits))
                        {
                          RecoveryUtility.resetPosition(unitPosition1, current.Item1, current.Item2, env, true);
                          applyChangeSkillEffects.clearMovePanelCacheAll = true;
                          applyChangeSkillEffects.setMoveUnitDistance = true;
                          break;
                        }
                      }
                      continue;
                    }
                  }
                  else
                    continue;
                case BattleskillEffectLogicEnum.call_reinforcements:
                  BL.BattleSkillResultExtendEffect resultExtendEffect = bsr as BL.BattleSkillResultExtendEffect;
                  BL.BattleSkillResultEffectCallReinforcement callReinforcement = (BL.BattleSkillResultEffectCallReinforcement) null;
                  if (resultExtendEffect != null)
                    callReinforcement = resultExtendEffect._effects.Find((Predicate<BL.BattleSkillResultEffect>) (x => x._battleskill_effect.ID == effect.ID)) as BL.BattleSkillResultEffectCallReinforcement;
                  if (callReinforcement != null)
                  {
                    int num24 = callReinforcement._battleskill_effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_group_id);
                    int num25 = callReinforcement._battleskill_effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_max);
                    List<BL.UnitPosition> unitPositionList1 = new List<BL.UnitPosition>();
                    List<BL.UnitPosition> unitPositionList2 = new List<BL.UnitPosition>();
                    foreach (BL.UnitPosition unitPosition5 in this.unitPositions.value)
                    {
                      bool flag5 = false;
                      if (unitPosition5.unit.playerUnit.reinforcement != null)
                      {
                        if (num24 > 0)
                        {
                          int? groupId = unitPosition5.unit.playerUnit.group_id;
                          int num26 = num24;
                          if (!(groupId.GetValueOrDefault() == num26 & groupId.HasValue))
                            continue;
                        }
                        if (!unitPosition5.unit.isEnable)
                          flag5 = true;
                        else if (unitPosition5.unit.isDead)
                          flag5 = true;
                        if (flag5)
                          unitPositionList1.Add(unitPosition5);
                      }
                    }
                    if (num25 <= 0 || unitPositionList1.Count <= num25)
                    {
                      unitPositionList2 = unitPositionList1;
                    }
                    else
                    {
                      for (int index = 0; index < num25; ++index)
                      {
                        uint randomValue = callReinforcement._random_values[index];
                        BL.UnitPosition unitPosition6 = unitPositionList1[(int) ((long) randomValue % (long) unitPositionList1.Count)];
                        unitPositionList1.Remove(unitPosition6);
                        unitPositionList2.Add(unitPosition6);
                      }
                    }
                    List<BL.SkillResult> skillResultList = new List<BL.SkillResult>();
                    foreach (BL.UnitPosition up in unitPositionList2)
                    {
                      BL.SkillResultUnit skillResultUnit = env.createSkillResultUnit(up);
                      skillResultUnit._is_reinforcement = true;
                      skillResultList.Add((BL.SkillResult) skillResultUnit);
                    }
                    if (skillResultList.Count > 0)
                    {
                      foreach (BL.SkillResult skillResult in skillResultList)
                      {
                        BL.SkillResultUnit skillResultUnit = skillResult as BL.SkillResultUnit;
                        skillResultUnit.run(isAI);
                        effectTargets?.Add(skillResultUnit._up.unit);
                      }
                      applyChangeSkillEffects.clearMovePanelCacheAll = true;
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.immediate_attack:
                  if (useUnit != null && target.hp > 0)
                  {
                    int damage = BattleFuncs.calcAttackDamage(useUnit, target, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack), effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease), effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage));
                    if (skill.skill_type == BattleskillSkillType.command || skill.skill_type == BattleskillSkillType.release)
                      damage = BattleFuncs.applyDamageCut(skill.skill_type == BattleskillSkillType.command ? 4 : 5, damage, target, useUnit, invokePanel: BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column));
                    BattleFuncs.applyDamage(target, damage, useUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.min_hp));
                    displayNumberTargets?.Add(target.originalUnit);
                    flag1 = true;
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.dance:
                  unitPosition1.resetOriginalPosition(env);
                  if (!isAI)
                  {
                    BL.ClassValue<List<BL.UnitPosition>> actionUnits = env.getActionUnits(env.getForceID(target.originalUnit));
                    if (!actionUnits.value.Contains(unitPosition1))
                    {
                      actionUnits.value.Add(unitPosition1);
                      actionUnits.commit();
                    }
                    if (env.completedActionUnits.value.Contains(unitPosition1))
                    {
                      this.completedActionUnits.value.Remove(unitPosition1);
                      this.completedActionUnits.commit();
                    }
                  }
                  else
                  {
                    BL.AIUnit org = target as BL.AIUnit;
                    if (!env.aiActionUnits.value.Contains(org))
                    {
                      BL.AIUnit[] array = env.aiActionOrder.value.ToArray();
                      env.aiActionOrder.value.Clear();
                      foreach (BL.AIUnit aiUnit in array)
                      {
                        if (aiUnit == target)
                          env.aiActionOrder.value.Enqueue(new BL.AIUnit(org));
                        else
                          env.aiActionOrder.value.Enqueue(aiUnit);
                      }
                      env.aiActionOrder.commit();
                      org.actionResults = (List<ActionResult>) null;
                      env.aiActionUnits.value.Add(org);
                      env.aiActionUnits.commit();
                    }
                  }
                  applyChangeSkillEffects.clearMovePanelCacheAll = true;
                  continue;
                case BattleskillEffectLogicEnum.changing:
                  if (useUnit != null)
                  {
                    BL.UnitPosition unitPosition7 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                    int row = unitPosition7.row;
                    int column = unitPosition7.column;
                    applyChangeSkillEffects.clearMovePanelCacheAll = true;
                    RecoveryUtility.resetPosition(unitPosition7, unitPosition1.row, unitPosition1.column, env, true, true);
                    if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_complete) == 0)
                    {
                      val1_1 = Math.Max(val1_1, 1);
                      int[] numArray = new int[3]
                      {
                        300001381,
                        300001382,
                        300001383
                      };
                      foreach (int key in numArray)
                      {
                        BattleskillSkill skill9 = MasterData.BattleskillSkill[key];
                        foreach (BattleskillEffect effect8 in skill9.Effects)
                          useUnit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect8, skill9, 1, isDontDisplay: true));
                      }
                      if (!isAI)
                        useUnit.originalUnit.commit();
                    }
                    if (!isAI)
                      env.setCurrentField(unitPosition7.row, unitPosition7.column);
                    effectTargets?.Add(useUnit.originalUnit);
                    RecoveryUtility.resetPosition(unitPosition1, row, column, env, true);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.steal:
                  if (useUnit != null)
                  {
                    BattleFuncs.executeSteal(useUnit, target, effect, BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit), unitPosition1, isAI);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.transformation:
                  target.skillEffects.RemoveEffect(1001677, env, target);
                  target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, level, investUnit: useUnit != null ? useUnit.originalUnit : (BL.Unit) null, investSkillId: investSkillId, investTurn: this.phaseState.absoluteTurnCount), isTargetEnemy);
                  target.skillEffects.ResetTransformationSkillEffects(target.transformationGroupId);
                  int num27 = effect.HasKey(BattleskillEffectLogicArgumentEnum.is_complete) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.is_complete) : 0;
                  if (num27 != 0)
                  {
                    RecoveryUtility.resetPosition(unitPosition1, unitPosition1.row, unitPosition1.column, env, true, true);
                    val1_1 = Math.Max(val1_1, 1);
                    int[] numArray;
                    if (num27 != 1)
                      numArray = new int[1]{ 300004267 };
                    else
                      numArray = new int[3]
                      {
                        300004267,
                        300001382,
                        300001383
                      };
                    foreach (int key in numArray)
                    {
                      BattleskillSkill skill10 = MasterData.BattleskillSkill[key];
                      foreach (BattleskillEffect effect9 in skill10.Effects)
                        target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect9, skill10, 1, isDontDisplay: true));
                    }
                  }
                  if (!isAI)
                  {
                    target.originalUnit.commit();
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.jump:
                  if (useUnit != null)
                  {
                    int key3 = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                    if (MasterData.BattleskillSkill.ContainsKey(key3))
                    {
                      int key4 = effect.GetInt(BattleskillEffectLogicArgumentEnum.aiming_skill_id);
                      if (MasterData.BattleskillSkill.ContainsKey(key4))
                      {
                        BattleskillSkill skill11 = MasterData.BattleskillSkill[key4];
                        if (skill11.Effects.Length != 0)
                        {
                          BattleskillEffect effect10 = skill11.Effects[0];
                          if (effect10.effect_logic.Enum == BattleskillEffectLogicEnum.aiming)
                          {
                            int num28 = BattleFuncs.getAllUnits(isAI, true, includeJumping: true).Max<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, int>) (x =>
                            {
                              IEnumerable<BL.SkillEffect> source2 = x.skillEffects.Where(BattleskillEffectLogicEnum.jump);
                              return !source2.Any<BL.SkillEffect>() ? 0 : source2.Max<BL.SkillEffect>((Func<BL.SkillEffect, int>) (y => (int) y.work[1])) + 1;
                            }));
                            bool flag6 = (effect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) : 0) == 0 || useUnit == target;
                            BL.ISkillEffectListUnit unit = flag6 ? useUnit : target;
                            BL.SkillEffect effect11 = BL.SkillEffect.FromMasterData(effect, skill, level, investUnit: useUnit.originalUnit, investSkillId: investSkillId, isDontDisplay: true, investTurn: this.phaseState.absoluteTurnCount);
                            effect11.work = new float[3]
                            {
                              (float) unitPosition1.id,
                              (float) num28,
                              (float) env.getForceID(useUnit.originalUnit)
                            };
                            unit.skillEffects.Add(effect11);
                            BattleskillSkill skill12 = MasterData.BattleskillSkill[key3];
                            foreach (BattleskillEffect effect12 in skill12.Effects)
                              unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect12, skill12, 1, investUnit: useUnit.originalUnit, investSkillId: investSkillId, isDontDisplay: true, investTurn: this.phaseState.absoluteTurnCount));
                            BL.SkillEffect effect13 = BL.SkillEffect.FromMasterData(effect10, skill11, 1, investUnit: useUnit.originalUnit, investSkillId: investSkillId, investTurn: this.phaseState.absoluteTurnCount);
                            target.skillEffects.Add(effect13);
                            BattleFuncs.iSkillEffectListUnitToUnitPosition(unit).commitPanelSkillEffects(BattleFuncs.getEnabledCharismaEffects(unit));
                            if (!isAI)
                            {
                              useUnit.originalUnit.commit();
                              target.originalUnit.commit();
                            }
                            if (!flag6)
                            {
                              BL.UnitPosition unitPosition8 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
                              if (!isAI)
                              {
                                BL.ClassValue<List<BL.UnitPosition>> actionUnits = env.getActionUnits(env.getForceID(unit.originalUnit));
                                if (actionUnits.value.Contains(unitPosition8))
                                {
                                  actionUnits.value.Remove(unitPosition8);
                                  actionUnits.commit();
                                }
                              }
                              else
                              {
                                BL.AIUnit aiUnit = unit as BL.AIUnit;
                                if (env.aiActionUnits.value.Contains(aiUnit))
                                {
                                  env.aiActionUnits.value.Remove(aiUnit);
                                  env.aiActionUnits.commit();
                                }
                              }
                              unitPosition8.recoveryCompleteUnit();
                              applyChangeSkillEffects.clearMovePanelCacheAll = true;
                            }
                            if (flag6)
                            {
                              val1_1 = Math.Max(val1_1, 2);
                              continue;
                            }
                            continue;
                          }
                          continue;
                        }
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.invest_land_tag:
                  if (useUnit != null && panels.Count == 1)
                  {
                    int num29 = effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
                    int num30 = effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
                    using (List<BL.Panel>.Enumerator enumerator = BattleFuncs.getRangePanels(panels[0].row, panels[0].column, new int[2]
                    {
                      num29,
                      num30
                    }).GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        BL.Panel current = enumerator.Current;
                        BL.ClassValue<List<BL.SkillEffect>> skillEffects = current.getSkillEffects(isAI);
                        if (skillEffects.value.RemoveAll((Predicate<BL.SkillEffect>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag)) >= 1)
                          skillEffects.commit();
                        current.addSkillEffect(BL.SkillEffect.FromMasterData(effect, skill, level, investUnit: useUnit.originalUnit, investSkillId: investSkillId, investTurn: this.phaseState.absoluteTurnCount), useUnit);
                        if (!isAI)
                        {
                          BL.UnitPosition[] fieldUnits = env.getFieldUnits(current.row, current.column);
                          if (fieldUnits != null)
                          {
                            foreach (BL.UnitPosition unitPosition9 in fieldUnits)
                              unitPosition9.unit.skillEffects.LandTagModified.commit();
                          }
                          if (env.fieldCurrent.value == current)
                            env.fieldCurrent.commit();
                        }
                        effectPanelTargets?.Add(current);
                      }
                      continue;
                    }
                  }
                  else
                    continue;
                case BattleskillEffectLogicEnum.inhale:
                  if (useUnit != null && panels.Count == 1)
                  {
                    BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
                    BL.ForceID[] forceIds = effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == 0 ? BattleFuncs.getForceIDArray(this.getForceID(useUnit.originalUnit)) : this.getTargetForce(useUnit.originalUnit, useUnit.IsCharm);
                    List<BL.UnitPosition> targets1 = BattleFuncs.getTargets(panels[0].row, panels[0].column, new int[2]
                    {
                      0,
                      effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
                    }, forceIds, BL.Unit.TargetAttribute.all, (isAI ? 1 : 0) != 0, nonFacility: true);
                    BL.UnitPosition unitPosition10 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                    BL.Panel panel = BattleFuncs.getPanel(unitPosition10.row, unitPosition10.column);
                    using (List<BL.UnitPosition>.Enumerator enumerator = targets1.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        BL.UnitPosition current = enumerator.Current;
                        BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(current);
                        if (!BattleFuncs.getAnchorGroundEffects(iskillEffectListUnit, useUnit, BattleFuncs.getPanel(current.row, current.column), panel).Any<BL.SkillEffect>() && BattleFuncs.checkInvokeSkillEffect(packedSkillEffect, useUnit, iskillEffectListUnit))
                        {
                          int move = current.unit.parameter.Move;
                          BL.Unit[] ignoreUnits = new BL.Unit[1]
                          {
                            iskillEffectListUnit.originalUnit
                          };
                          int row3 = panels[0].row;
                          int column3 = panels[0].column;
                          bool countMoveDistance = iskillEffectListUnit == useUnit;
                          if (BattleFuncs.isResetPositionOK(iskillEffectListUnit.originalUnit, row3, column3, move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
                          {
                            RecoveryUtility.resetPosition(current, row3, column3, env, true, countMoveDistance);
                            applyChangeSkillEffects.clearMovePanelCacheAll = true;
                            applyChangeSkillEffects.setMoveUnitDistance = !countMoveDistance;
                          }
                          else
                          {
                            int num31 = current.row - row3;
                            int num32 = current.column - column3;
                            if (num31 > 0 && num32 >= 0)
                            {
                              if (Mathf.Abs(num31) >= Mathf.Abs(num32))
                                ++row3;
                              else
                                ++column3;
                            }
                            else if (num31 <= 0 && num32 > 0)
                            {
                              if (Mathf.Abs(num32) >= Mathf.Abs(num31))
                                ++column3;
                              else
                                --row3;
                            }
                            else if (num31 < 0 && num32 <= 0)
                            {
                              if (Mathf.Abs(num31) >= Mathf.Abs(num32))
                                --row3;
                              else
                                --column3;
                            }
                            else if (Mathf.Abs(num32) >= Mathf.Abs(num31))
                              --column3;
                            else
                              ++row3;
                            if (row3 >= env.getFieldHeight())
                              row3 = env.getFieldHeight() - 1;
                            if (column3 >= env.getFieldWidth())
                              column3 = env.getFieldWidth() - 1;
                            if (row3 < 0)
                              row3 = 0;
                            if (column3 < 0)
                              column3 = 0;
                            if (BattleFuncs.isResetPositionOK(iskillEffectListUnit.originalUnit, row3, column3, move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
                            {
                              RecoveryUtility.resetPosition(current, row3, column3, env, true, countMoveDistance);
                              applyChangeSkillEffects.clearMovePanelCacheAll = true;
                              applyChangeSkillEffects.setMoveUnitDistance = !countMoveDistance;
                            }
                            else
                            {
                              bool flag7 = false;
                              for (int index6 = 1; index6 <= 50; ++index6)
                              {
                                int row4 = row3 + index6;
                                int column4 = column3;
                                for (int index7 = 0; index7 < index6; ++index7)
                                {
                                  if (BattleFuncs.isResetPositionOK(iskillEffectListUnit.originalUnit, row4, column4, move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
                                  {
                                    RecoveryUtility.resetPosition(current, row4, column4, env, true, countMoveDistance);
                                    applyChangeSkillEffects.clearMovePanelCacheAll = true;
                                    applyChangeSkillEffects.setMoveUnitDistance = !countMoveDistance;
                                    flag7 = true;
                                    break;
                                  }
                                  --row4;
                                  ++column4;
                                }
                                if (!flag7)
                                {
                                  for (int index8 = 0; index8 < index6; ++index8)
                                  {
                                    if (BattleFuncs.isResetPositionOK(iskillEffectListUnit.originalUnit, row4, column4, move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
                                    {
                                      RecoveryUtility.resetPosition(current, row4, column4, env, true, countMoveDistance);
                                      applyChangeSkillEffects.clearMovePanelCacheAll = true;
                                      applyChangeSkillEffects.setMoveUnitDistance = !countMoveDistance;
                                      flag7 = true;
                                      break;
                                    }
                                    --row4;
                                    --column4;
                                  }
                                  if (!flag7)
                                  {
                                    for (int index9 = 0; index9 < index6; ++index9)
                                    {
                                      if (BattleFuncs.isResetPositionOK(iskillEffectListUnit.originalUnit, row4, column4, move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
                                      {
                                        RecoveryUtility.resetPosition(current, row4, column4, env, true, countMoveDistance);
                                        applyChangeSkillEffects.clearMovePanelCacheAll = true;
                                        applyChangeSkillEffects.setMoveUnitDistance = !countMoveDistance;
                                        flag7 = true;
                                        break;
                                      }
                                      ++row4;
                                      --column4;
                                    }
                                    if (!flag7)
                                    {
                                      for (int index10 = 0; index10 < index6; ++index10)
                                      {
                                        if (BattleFuncs.isResetPositionOK(iskillEffectListUnit.originalUnit, row4, column4, move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
                                        {
                                          RecoveryUtility.resetPosition(current, row4, column4, env, true, countMoveDistance);
                                          applyChangeSkillEffects.clearMovePanelCacheAll = true;
                                          applyChangeSkillEffects.setMoveUnitDistance = !countMoveDistance;
                                          flag7 = true;
                                          break;
                                        }
                                        ++row4;
                                        ++column4;
                                      }
                                      if (flag7)
                                        break;
                                    }
                                    else
                                      break;
                                  }
                                  else
                                    break;
                                }
                                else
                                  break;
                              }
                            }
                          }
                          if (!isAI && iskillEffectListUnit == useUnit)
                            env.setCurrentField(current.row, current.column);
                        }
                      }
                      continue;
                    }
                  }
                  else
                    continue;
                case BattleskillEffectLogicEnum.facility_creation:
                  if (useUnit != null && panels.Count == 1)
                  {
                    BL.UnitPosition unitPosition11 = (BL.UnitPosition) null;
                    BL.UnitPosition unitPosition12 = (BL.UnitPosition) null;
                    int num33 = int.MaxValue;
                    int val1_2 = -1;
                    int num34 = effect.GetInt(BattleskillEffectLogicArgumentEnum.facility_unit_id);
                    if (!isAI)
                    {
                      foreach (BL.UnitPosition unitPosition13 in this.unitPositions.value)
                      {
                        BL.Unit unit = unitPosition13.unit;
                        if (unit.isFacility && unit.facility.isSkillUnit)
                        {
                          int? skillUnitIndex = unit.facility.skillUnitIndex;
                          int index = useUnit.originalUnit.index;
                          bool flag8 = skillUnitIndex.GetValueOrDefault() == index & skillUnitIndex.HasValue && unit.facility.thisForce == env.getForceID(useUnit.originalUnit) && unit.playerUnit._unit == num34;
                          if (unit.isEnable && !unit.isDead)
                          {
                            val1_2 = Math.Max(val1_2, unit.facilitySpawnOrder);
                            if (flag8 && unit.facilitySpawnOrder < num33)
                            {
                              num33 = unit.facilitySpawnOrder;
                              unitPosition12 = unitPosition13;
                            }
                          }
                          else if (flag8 && unitPosition11 == null)
                            unitPosition11 = unitPosition13;
                        }
                      }
                      if (unitPosition11 == null && unitPosition12 != null)
                      {
                        unitPosition11 = unitPosition12;
                        destructFacilities.Add(unitPosition11.unit);
                      }
                    }
                    if (unitPosition11 != null)
                    {
                      BL.UnitPosition unitPosition14 = unitPosition11;
                      unitPosition14.row = panels[0].row;
                      unitPosition14.column = panels[0].column;
                      unitPosition14.direction = 0.0f;
                      NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
                      if (instance.isPvp && instance.order == 1)
                        unitPosition14.direction = 180f;
                      if (!isAI)
                      {
                        BL.Unit unit = unitPosition14.unit;
                        unit.hp = unit.parameter.Hp;
                        if (unit.isDead)
                          unit.setIsDead(false, env);
                        if (!unit.isEnable)
                        {
                          unit.isSpawned = true;
                          unit.isEnable = true;
                        }
                        foreach (BL.SkillEffect skillEffect in unit.skillEffects.All())
                        {
                          skillEffect.dontCleanUseRemain = true;
                          skillEffect.useRemain = skillEffect.effect.use_count;
                        }
                        unitPosition14.resetOriginalPosition(env, resetOriginalToOuterPosition: true);
                        unit.facilitySpawnOrder = val1_2 + 1;
                        if (this.getForceID(useUnit.originalUnit) != BL.ForceID.player)
                          env.createDangerAria();
                        unit.initReinforcement();
                        unit.playerUnit.spawn_turn = env.phaseState.turnCount;
                        applyChangeSkillEffects.clearMovePanelCacheAll = true;
                        createFacilities.Add(unit);
                        if (effectPanelTargets != null)
                        {
                          effectPanelTargets.Add(panels[0]);
                          continue;
                        }
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.parameter_reference_heal:
                  if (target.hp >= 1)
                  {
                    int healValue = BattleFuncs.getHealValue(target, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), BattleFuncs.getParameterReferenceHealValue(useUnit, target, effect), skill.skill_type);
                    target.hp += healValue;
                    action2(target);
                    continue;
                  }
                  continue;
                case BattleskillEffectLogicEnum.reduct_command_skill_use:
                  int targetSkillId = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                  int targetLogicId = effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id);
                  int num35 = effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
                  if (targetSkillId == 0 && targetLogicId == 0 && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.not_random) || effect.GetInt(BattleskillEffectLogicArgumentEnum.not_random) == 0))
                  {
                    for (int index11 = 0; index11 < num35; ++index11)
                    {
                      BL.Skill[] array = BattleFuncs.getReductCommandSkillUseTargetSkills(target, targetSkillId, targetLogicId, useUnit, effect.skill_BattleskillSkill).ToArray<BL.Skill>();
                      if (array.Length != 0)
                      {
                        int index12 = (int) random.NextFixed((uint) array.Length);
                        BL.Skill skill13 = ((IEnumerable<BL.Skill>) array).ElementAt<BL.Skill>(index12);
                        int? remain = skill13.remain;
                        skill13.remain = remain.HasValue ? new int?(remain.GetValueOrDefault() - 1) : new int?();
                      }
                      else
                        break;
                    }
                    continue;
                  }
                  using (IEnumerator<BL.Skill> enumerator = BattleFuncs.getReductCommandSkillUseTargetSkills(target, targetSkillId, targetLogicId, useUnit, effect.skill_BattleskillSkill).GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      BL.Skill current = enumerator.Current;
                      BL.Skill skill14 = current;
                      int? remain1 = skill14.remain;
                      int num36 = num35;
                      skill14.remain = remain1.HasValue ? new int?(remain1.GetValueOrDefault() - num36) : new int?();
                      int? remain2 = current.remain;
                      int num37 = 0;
                      if (remain2.GetValueOrDefault() < num37 & remain2.HasValue)
                        current.remain = new int?(0);
                    }
                    continue;
                  }
                case BattleskillEffectLogicEnum.provide:
                  if (useUnit != null)
                  {
                    if (battleParameter == null)
                      battleParameter = useUnit.parameter;
                    bool isFirst = !intList.Contains(effect.ID);
                    if (isFirst)
                      intList.Add(effect.ID);
                    BattleFuncs.executeProvide(useUnit, target, effect, BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit), unitPosition1, isAI, targets.Count, battleParameter, isFirst);
                    continue;
                  }
                  continue;
                default:
                  Debug.LogError((object) ("unexpected EffectLogic: " + (object) effect.EffectLogic.ID));
                  continue;
              }
            }
          }
          else if (BattleFuncs.checkPassiveEffectEnable(effect, target) != 0)
          {
            effectTargets?.Add(target.originalUnit);
            if (!isAI)
              target.originalUnit.commit();
            target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, level, investUnit: useUnit != null ? useUnit.originalUnit : (BL.Unit) null, investSkillId: investSkillId, investTurn: this.phaseState.absoluteTurnCount), isTargetEnemy, target);
          }
        }
      }
      return Tuple.Create<int, bool, bool>(val1_1, flag1, flag2);
    }

    public Tuple<List<BL.Unit>, int, List<BL.Unit>, bool, bool, List<BL.Panel>, Tuple<List<BL.Unit>, List<BL.Unit>>> setSkillEffect(
      BattleskillSkill skill,
      int level,
      List<BL.ISkillEffectListUnit> targets,
      List<BL.Panel> panels,
      BL.BattleSkillResult bsr,
      BL env,
      BL.ISkillEffectListUnit useUnit = null,
      bool needEffectTargets = false,
      bool needDisplayNumberTargets = false,
      XorShift random = null,
      int nowUseCount = 0,
      bool? callIsPlayer = null)
    {
      HashSet<BL.Unit> unitSet1 = needEffectTargets ? new HashSet<BL.Unit>() : (HashSet<BL.Unit>) null;
      HashSet<BL.Panel> panelSet = needEffectTargets ? new HashSet<BL.Panel>() : (HashSet<BL.Panel>) null;
      HashSet<BL.Unit> unitSet2 = needDisplayNumberTargets ? new HashSet<BL.Unit>() : (HashSet<BL.Unit>) null;
      int val1_1 = 0;
      bool flag1 = false;
      bool flag2 = false;
      int id = skill.ID;
      BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects = new BattleFuncs.ApplyChangeSkillEffects(useUnit != null && useUnit is BL.AIUnit);
      Tuple<int, int> usePosition = (Tuple<int, int>) null;
      List<BL.Unit> createFacilities = needEffectTargets ? new List<BL.Unit>() : (List<BL.Unit>) null;
      List<BL.Unit> destructFacilities = needEffectTargets ? new List<BL.Unit>() : (List<BL.Unit>) null;
      if (useUnit != null)
      {
        BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
        usePosition = Tuple.Create<int, int>(unitPosition.row, unitPosition.column);
      }
      random = random != null ? new XorShift(random) : env.random;
      targets = new List<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) targets);
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.random_choice && x.checkLevel(level)));
      if (battleskillEffect != null)
      {
        int num = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.lot_count);
        if (num < targets.Count)
        {
          List<BL.ISkillEffectListUnit> skillEffectListUnitList = targets;
          targets = new List<BL.ISkillEffectListUnit>();
          for (int index1 = 0; index1 < num; ++index1)
          {
            int index2 = (int) random.NextFixed((uint) skillEffectListUnitList.Count);
            targets.Add(skillEffectListUnitList[index2]);
            skillEffectListUnitList.RemoveAt(index2);
          }
        }
      }
      Tuple<int, bool, bool> tuple1 = this.setSkillEffectSub(unitSet1, unitSet2, id, skill, level, targets, panels, bsr, env, useUnit, applyChangeSkillEffects, random, nowUseCount, callIsPlayer, panelSet, createFacilities, destructFacilities);
      int val1_2 = Math.Max(val1_1, tuple1.Item1);
      bool flag3 = flag1 | tuple1.Item2;
      bool flag4 = flag2 | tuple1.Item3;
      if (useUnit != null)
      {
        foreach (Tuple<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>> chainSkillTarget in BattleFuncs.getChainSkillTargets(skill, level, targets.Count > 0 ? targets[0] : (BL.ISkillEffectListUnit) null, panels.Count > 0 ? panels[0] : (BL.Panel) null, useUnit, nowUseCount, usePosition))
        {
          targets = chainSkillTarget.Item2.ToList<BL.ISkillEffectListUnit>();
          panels = new List<BL.Panel>();
          Tuple<int, bool, bool> tuple2 = this.setSkillEffectSub(unitSet1, unitSet2, id, chainSkillTarget.Item1, level, targets, panels, bsr, env, useUnit, applyChangeSkillEffects, random, nowUseCount, callIsPlayer, panelSet, createFacilities, destructFacilities);
          val1_2 = Math.Max(val1_2, tuple2.Item1);
          flag3 |= tuple2.Item2;
          flag4 |= tuple2.Item3;
        }
      }
      if (applyChangeSkillEffects.setMoveUnitDistance)
      {
        switch (useUnit)
        {
          case null:
          case BL.AIUnit _:
            break;
          default:
            applyChangeSkillEffects.add(BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit), useUnit, true);
            break;
        }
      }
      applyChangeSkillEffects.execute();
      return Tuple.Create<List<BL.Unit>, int, List<BL.Unit>, bool, bool, List<BL.Panel>, Tuple<List<BL.Unit>, List<BL.Unit>>>(needEffectTargets ? unitSet1.ToList<BL.Unit>() : (List<BL.Unit>) null, val1_2, needDisplayNumberTargets ? unitSet2.ToList<BL.Unit>() : (List<BL.Unit>) null, flag3, flag4, needEffectTargets ? panelSet.ToList<BL.Panel>() : (List<BL.Panel>) null, Tuple.Create<List<BL.Unit>, List<BL.Unit>>(createFacilities, destructFacilities));
    }

    public Tuple<int, List<BL.ISkillEffectListUnit>> useSkillCore(
      BL.ISkillEffectListUnit unit,
      BL.Skill skill,
      List<BL.ISkillEffectListUnit> targets,
      List<BL.Panel> panels,
      BL.BattleSkillResult bsr,
      BL env,
      BL.UseSkillWithResult usr = null,
      XorShift random = null)
    {
      if (!(unit is BL.AIUnit))
        unit.originalUnit.commit();
      BL.SkillEffect[] array = unit.skillEffects.All().ToArray();
      skill.useTurn = this.phaseState.absoluteTurnCount + skill.skill.charge_turn - (skill.level - 1);
      if (skill.maxUseCount != 0 && skill.useTurn < this.phaseState.absoluteTurnCount + skill.maxUseCount)
        skill.useTurn = this.phaseState.absoluteTurnCount + skill.maxUseCount;
      bool flag = usr != null;
      Tuple<List<BL.Unit>, int, List<BL.Unit>, bool, bool, List<BL.Panel>, Tuple<List<BL.Unit>, List<BL.Unit>>> tuple = this.setSkillEffect(skill.skill, skill.level, targets, panels, bsr, env, unit, flag, flag, random, skill.nowUseCount);
      if (flag)
      {
        usr.effectTargets = tuple.Item1;
        usr.displayNumberTargets = tuple.Item3;
        usr.isDamage = tuple.Item4;
        usr.lateDispHp = tuple.Item5;
        usr.effectPanelTargets = tuple.Item6;
        usr.createFacilities = tuple.Item7.Item1;
        usr.destructFacilities = tuple.Item7.Item2;
      }
      if (((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.hp_consume)))
      {
        Tuple<int, int> hpCost = skill.getHpCost(unit.originalUnit);
        if (hpCost.Item1 != 0)
        {
          int hp = unit.hp;
          unit.hp -= hpCost.Item1;
          if (flag)
          {
            usr.dispHpUnit = unit.originalUnit;
            usr.prevHp = hp;
          }
        }
      }
      int? nullable1 = skill.remain;
      if (nullable1.HasValue)
      {
        nullable1 = skill.remain;
        int num = 1;
        if (nullable1.GetValueOrDefault() >= num & nullable1.HasValue)
        {
          BL.Skill skill1 = skill;
          nullable1 = skill1.remain;
          int? nullable2 = nullable1;
          skill1.remain = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() - 1) : new int?();
        }
      }
      ++skill.nowUseCount;
      if (!(unit is BL.AIUnit) && skill.skill.skill_type == BattleskillSkillType.release && !BattleFuncs.checkUseOugiSkillMaxCountInDeck(unit, skill))
      {
        foreach (BL.Unit unit1 in BattleFuncs.forceUnits(env.getForceID(unit.originalUnit)).value)
        {
          if (unit1.hasOugi && unit1.ougi.id == skill.id)
            unit1.commit();
        }
      }
      int val1 = tuple.Item2;
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.again_use_skill && x.checkLevel(skill.level)));
      if (battleskillEffect != null)
      {
        val1 = Math.Max(val1, 1);
        BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
        RecoveryUtility.resetPosition(unitPosition, unitPosition.row, unitPosition.column, env, true, true);
        List<int> intList = new List<int>() { 390003074 };
        int num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.again_type);
        if (num1 != 0)
        {
          int[] numArray = new int[4]
          {
            300001382,
            300001383,
            390003075,
            390003076
          };
          int num2 = 1;
          for (int index = 0; index < numArray.Length; ++index)
          {
            if ((num1 & num2) != 0)
              intList.Add(numArray[index]);
            num2 <<= 1;
          }
        }
        foreach (int key in intList)
        {
          BattleskillSkill skill2 = MasterData.BattleskillSkill[key];
          foreach (BattleskillEffect effect in skill2.Effects)
            unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill2, 1, isDontDisplay: true));
        }
      }
      IEnumerable<BL.SkillEffect> skillEffects = unit.skillEffects.All().Except<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array);
      if (val1 != 1)
      {
        foreach (BL.SkillEffect skillEffect in skillEffects)
        {
          nullable1 = new int?();
          int? nullable3 = nullable1;
          skillEffect.moveDistance = nullable3;
        }
      }
      else
      {
        foreach (BL.SkillEffect skillEffect in skillEffects)
          skillEffect.moveDistance = new int?(0);
      }
      return new Tuple<int, List<BL.ISkillEffectListUnit>>(val1, (List<BL.ISkillEffectListUnit>) null);
    }

    public void useSkillWith(
      BL.Unit unit,
      BL.Skill skill,
      List<BL.Unit> targets,
      List<BL.Panel> panels,
      BL.BattleSkillResult bsr,
      Action<BL.UseSkillWithResult> f,
      BL env,
      XorShift random = null)
    {
      BL.UseSkillWithResult usr = new BL.UseSkillWithResult();
      int num = this.useSkillCore((BL.ISkillEffectListUnit) unit, skill, targets.Select<BL.Unit, BL.ISkillEffectListUnit>((Func<BL.Unit, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) x)).ToList<BL.ISkillEffectListUnit>(), panels, bsr, env, usr, random).Item1;
      f(usr);
      if (num == 0)
      {
        this.getUnitPosition(unit).actionActionUnit(this);
      }
      else
      {
        if (num != 2)
          return;
        this.getUnitPosition(unit).completeActionUnit(this, true);
      }
    }

    public void useCallSkillWith(
      BL.Skill skill,
      List<BL.Unit> targets,
      Action<BL.UseSkillWithResult> f,
      BL env,
      XorShift random = null,
      bool isPlayer = true)
    {
      BL.UseSkillWithResult useSkillWithResult = new BL.UseSkillWithResult();
      Tuple<List<BL.Unit>, int, List<BL.Unit>, bool, bool, List<BL.Panel>, Tuple<List<BL.Unit>, List<BL.Unit>>> tuple = this.setSkillEffect(skill.skill, skill.level, targets.Select<BL.Unit, BL.ISkillEffectListUnit>((Func<BL.Unit, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) x)).ToList<BL.ISkillEffectListUnit>(), new List<BL.Panel>(), (BL.BattleSkillResult) null, env, needEffectTargets: true, needDisplayNumberTargets: true, random: random, callIsPlayer: new bool?(isPlayer));
      useSkillWithResult.effectTargets = tuple.Item1;
      useSkillWithResult.displayNumberTargets = tuple.Item3;
      useSkillWithResult.isDamage = tuple.Item4;
      useSkillWithResult.lateDispHp = tuple.Item5;
      if (skill.remain.HasValue)
      {
        BL.Skill skill1 = skill;
        int? remain = skill1.remain;
        skill1.remain = remain.HasValue ? new int?(remain.GetValueOrDefault() - 1) : new int?();
      }
      f(useSkillWithResult);
    }

    public List<BL.Skill> getFieldSkills(BL.Unit unit)
    {
      List<BL.Skill> fieldSkills = new List<BL.Skill>();
      if (unit == (BL.Unit) null)
        return fieldSkills;
      foreach (BL.Skill skill in unit.skills)
      {
        if (skill.isCommand && skill.skill.checkEnableUnit((BL.ISkillEffectListUnit) unit))
          fieldSkills.Add(skill);
      }
      return fieldSkills;
    }

    public List<BL.UnitPosition> getSkillTargetUnits(
      BL.Unit unit,
      int row,
      int column,
      BL.Skill skill,
      bool isAI = false)
    {
      if (skill.targetType != BattleskillTargetType.myself)
        return BattleFuncs.getTargets(row, column, skill.range, skill.getTargetForceIDs(this, (BL.ISkillEffectListUnit) unit), skill.targetAttribute, isAI, isDead: skill.isDeadTargetOnly, nonFacility: skill.nonFacility, includeJumping: skill.skill.max_range == 1000);
      return new List<BL.UnitPosition>()
      {
        isAI ? (BL.UnitPosition) this.getAIUnit(unit) : this.getUnitPosition(unit)
      };
    }

    public List<BL.UnitPosition> getSkillTargetUnits(BL.UnitPosition up, BL.Skill skill)
    {
      return this.getSkillTargetUnits(up.unit, up.row, up.column, skill, up is BL.AIUnit);
    }

    public List<BL.UnitPosition> getCallSkillTargetUnits(BL.Skill skill, bool isPlayer = true)
    {
      BL.ForceID[] forceIds;
      if (skill.skill.IsCallTargetPlayer)
        forceIds = isPlayer ? BattleFuncs.ForceIDArrayPlayer : BattleFuncs.ForceIDArrayPlayerTarget;
      else if (skill.skill.IsCallTargetEnemy)
      {
        forceIds = isPlayer ? BattleFuncs.ForceIDArrayPlayerTarget : BattleFuncs.ForceIDArrayPlayer;
      }
      else
      {
        if (!skill.skill.IsCallTargetComplex)
          return new List<BL.UnitPosition>();
        forceIds = ((IEnumerable<BL.ForceID>) BattleFuncs.ForceIDArrayPlayer).Concat<BL.ForceID>((IEnumerable<BL.ForceID>) BattleFuncs.ForceIDArrayPlayerTarget).ToArray<BL.ForceID>();
      }
      return BattleFuncs.getTargets(0, 0, new int[2]
      {
        0,
        int.MaxValue
      }, forceIds, skill.targetAttribute, nonFacility: true, includeJumping: true);
    }

    public List<BL.UnitPosition> completedPositionExecuteSkillEffects(
      BL.UnitPosition up,
      out List<List<BL.ExecuteSkillEffectResult>> result,
      HashSet<BL.ISkillEffectListUnit> deads = null)
    {
      bool isAI = up is BL.AIUnit;
      List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
      result = isAI ? (List<List<BL.ExecuteSkillEffectResult>>) null : new List<List<BL.ExecuteSkillEffectResult>>();
      if (BattleFuncs.unitPositionToISkillEffectListUnit(up).IsJumping)
        return unitPositionList;
      BL.FacilitySkillLogicEffect[] logicEffects = new BL.FacilitySkillLogicEffect[26]
      {
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapRatioHeal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapFixHeal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapRatioAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapFixAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapEnemyInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapPlayerInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapEnemyRemoveSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapPlayerRemoveSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapInvestLandTag(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapEnemySteal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapPlayerSteal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapEnemyProvide(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrapPlayerProvide(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2RatioHeal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2FixHeal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2RatioAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2FixAttack(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2EnemyInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2PlayerInvestSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2EnemyRemoveSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2PlayerRemoveSkilleffect(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2InvestLandTag(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2EnemySteal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2PlayerSteal(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2EnemyProvide(),
        (BL.FacilitySkillLogicEffect) new BL.FacilitySkillLogicEffectTrap2PlayerProvide()
      };
      BL.ISkillEffectListUnit[] array1 = BattleFuncs.getAllUnitsOrdered(isAI, true, true).ToArray<BL.ISkillEffectListUnit>();
      BL.ForceID forceId = this.getForceID(up.unit);
      BL.ISkillEffectListUnit[] array2 = ((IEnumerable<BL.ISkillEffectListUnit>) array1).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => !x.originalUnit.isFacility || !x.originalUnit.facility.isSkillUnit)).ToArray<BL.ISkillEffectListUnit>();
      foreach (BL.ISkillEffectListUnit unit in ((IEnumerable<BL.ISkillEffectListUnit>) array2).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => this.getForceID(x.originalUnit) == forceId)).Concat<BL.ISkillEffectListUnit>(((IEnumerable<BL.ISkillEffectListUnit>) array2).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => this.getForceID(x.originalUnit) != forceId))).Concat<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) ((IEnumerable<BL.ISkillEffectListUnit>) array1).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => x.originalUnit.isFacility && x.originalUnit.facility.isSkillUnit)).OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.facilitySpawnOrder))))
      {
        if (!unit.IsJumping)
        {
          BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
          List<BL.ExecuteSkillEffectResult> list = this.executeFacilitySkill(logicEffects, unitPosition, up, isAI, deads).ToList<BL.ExecuteSkillEffectResult>();
          if (!isAI && list.Count > 0)
          {
            unitPositionList.Add(unitPosition);
            result.Add(list);
          }
        }
      }
      return unitPositionList;
    }

    private IEnumerable<BL.ExecuteSkillEffectResult> executeFacilitySkill(
      BL.FacilitySkillLogicEffect[] logicEffects,
      BL.UnitPosition up,
      BL.UnitPosition upMove = null,
      bool isAI = false,
      HashSet<BL.ISkillEffectListUnit> deads = null,
      object extData = null,
      IEnumerable<BL.SkillEffect> useSkillEffects = null)
    {
      BL env = this;
      BL.ISkillEffectListUnit unit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      if (unit.hp > 0 && env.phaseState.absoluteTurnCount > 0)
      {
        if (useSkillEffects == null)
          useSkillEffects = ((IEnumerable<BL.FacilitySkillLogicEffect>) logicEffects).SelectMany<BL.FacilitySkillLogicEffect, BL.SkillEffect>((Func<BL.FacilitySkillLogicEffect, IEnumerable<BL.SkillEffect>>) (le => unit.skillEffects.Where(le.logicEnum())));
        IOrderedEnumerable<IGrouping<int, BL.SkillEffect>> orderedEnumerable = useSkillEffects.OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)).GroupBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).OrderByDescending<IGrouping<int, BL.SkillEffect>, int>((Func<IGrouping<int, BL.SkillEffect>, int>) (x => MasterData.BattleskillSkill[x.Key].weight)).ThenBy<IGrouping<int, BL.SkillEffect>, int>((Func<IGrouping<int, BL.SkillEffect>, int>) (x => x.Key));
        BL.ForceID[] forceIdArray = BattleFuncs.getForceIDArray(env.getForceID(unit.originalUnit));
        BL.ForceID[] targetForce = env.getTargetForce(unit.originalUnit, false);
        List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
        List<List<BL.SkillEffect>> skillEffectListList = new List<List<BL.SkillEffect>>();
        int length = 0;
        BL.Panel panel = BattleFuncs.getPanel(up.row, up.column);
        foreach (IGrouping<int, BL.SkillEffect> grouping in (IEnumerable<IGrouping<int, BL.SkillEffect>>) orderedEnumerable)
        {
          List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
          foreach (BL.SkillEffect skillEffect in (IEnumerable<BL.SkillEffect>) grouping)
          {
            BL.SkillEffect effect = skillEffect;
            skillEffectList.Add(effect);
            int? nullable;
            if (effect.useRemain.HasValue)
            {
              nullable = effect.useRemain;
              int num = 0;
              if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
                continue;
            }
            BattleFuncs.PackedSkillEffect pse1 = BattleFuncs.PackedSkillEffect.Create(effect);
            BL.FacilitySkillLogicEffect skillLogicEffect = ((IEnumerable<BL.FacilitySkillLogicEffect>) logicEffects).First<BL.FacilitySkillLogicEffect>((Func<BL.FacilitySkillLogicEffect, bool>) (x => x.logicEnum() == effect.effect.EffectLogic.Enum));
            skillLogicEffect.init(env, pse1, forceIdArray, targetForce, up, unit, isAI, extData);
            if ((!skillLogicEffect.isCheckSeal() || !BattleFuncs.isSealedSkillEffect(unit, effect)) && (upMove == null || skillLogicEffect.checkMoveUnitInvoke(upMove)))
            {
              if (!skillLogicEffect.isPanelTarget())
              {
                BattleFuncs.PackedSkillEffect pse2 = pse1;
                nullable = new int?();
                int? colosseumTurn = nullable;
                if (BattleFuncs.checkInvokeSkillEffectCommon(pse2, colosseumTurn))
                {
                  BattleFuncs.PackedSkillEffect pse3 = pse1;
                  BL.ISkillEffectListUnit unit1 = unit;
                  nullable = new int?();
                  int? unitHp1 = nullable;
                  if (BattleFuncs.checkInvokeSkillEffectSelf(pse3, unit1, unitHp: unitHp1) && pse1.CheckLandTag(panel, isAI))
                  {
                    List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
                    foreach (BL.UnitPosition target1 in skillLogicEffect.getTargets())
                    {
                      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(target1);
                      if (iskillEffectListUnit.hp >= 1)
                      {
                        BattleFuncs.PackedSkillEffect pse4 = pse1;
                        BL.ISkillEffectListUnit target2 = iskillEffectListUnit;
                        nullable = new int?();
                        int? targetHp1 = nullable;
                        if (BattleFuncs.checkInvokeSkillEffectTarget(pse4, target2, targetHp: targetHp1))
                        {
                          BattleFuncs.PackedSkillEffect pse5 = pse1;
                          BL.ISkillEffectListUnit unit2 = unit;
                          BL.ISkillEffectListUnit target3 = iskillEffectListUnit;
                          nullable = new int?();
                          int? unitHp2 = nullable;
                          nullable = new int?();
                          int? targetHp2 = nullable;
                          if (BattleFuncs.checkInvokeSkillEffectBoth(pse5, unit2, target3, unitHp: unitHp2, targetHp: targetHp2))
                            unitPositionList.Add(target1);
                        }
                      }
                    }
                    if (unitPositionList.Count > 0)
                      skillParams.Add(BattleFuncs.SkillParam.CreateParam(unit.originalUnit, effect, (object) Tuple.Create<List<BL.UnitPosition>, int, Vector2?>(unitPositionList, length, skillLogicEffect.invokedEffectVector), (int) skillLogicEffect.category()));
                  }
                }
              }
              else
              {
                BattleFuncs.PackedSkillEffect pse6 = pse1;
                nullable = new int?();
                int? colosseumTurn = nullable;
                if (BattleFuncs.checkInvokeSkillEffectCommon(pse6, colosseumTurn))
                {
                  BattleFuncs.PackedSkillEffect pse7 = pse1;
                  BL.ISkillEffectListUnit unit3 = unit;
                  nullable = new int?();
                  int? unitHp = nullable;
                  if (BattleFuncs.checkInvokeSkillEffectSelf(pse7, unit3, unitHp: unitHp) && pse1.CheckLandTag(panel, isAI))
                  {
                    BL.Panel[] array = skillLogicEffect.getPanelTargets().ToArray<BL.Panel>();
                    if (array.Length != 0)
                      skillParams.Add(BattleFuncs.SkillParam.CreateParam(unit.originalUnit, effect, (object) Tuple.Create<BL.Panel[], int>(array, length), (int) skillLogicEffect.category()));
                  }
                }
              }
            }
          }
          skillEffectListList.Add(skillEffectList);
          ++length;
        }
        if (length > 0 && skillParams.Count > 0)
        {
          BL.ExecuteSkillEffectResult[] skillEffectResultArray1 = new BL.ExecuteSkillEffectResult[length];
          BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects = new BattleFuncs.ApplyChangeSkillEffects(isAI);
          BL.ISkillEffectListUnit iskillEffectListUnit1 = upMove != null ? BattleFuncs.unitPositionToISkillEffectListUnit(upMove) : (BL.ISkillEffectListUnit) null;
          Judgement.BattleParameter parameter = unit.parameter;
          foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamCategoryFilter(skillParams))
          {
            BL.SkillEffect effect = skillParam.effect;
            bool flag = false;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
            BL.FacilitySkillLogicEffect skillLogicEffect = ((IEnumerable<BL.FacilitySkillLogicEffect>) logicEffects).First<BL.FacilitySkillLogicEffect>((Func<BL.FacilitySkillLogicEffect, bool>) (x => x.logicEnum() == effect.effect.EffectLogic.Enum));
            int index1;
            if (!skillLogicEffect.isPanelTarget())
            {
              Tuple<List<BL.UnitPosition>, int, Vector2?> tuple = (Tuple<List<BL.UnitPosition>, int, Vector2?>) skillParam.param;
              List<BL.UnitPosition> targets = tuple.Item1;
              index1 = tuple.Item2;
              Vector2? nullable = tuple.Item3;
              if (skillEffectResultArray1[index1] == null)
                skillEffectResultArray1[index1] = new BL.ExecuteSkillEffectResult()
                {
                  skill = effect.baseSkill,
                  invokedEffectVector = nullable
                };
              foreach (BL.UnitPosition unitPosition in targets)
              {
                BL.ISkillEffectListUnit iskillEffectListUnit2 = BattleFuncs.unitPositionToISkillEffectListUnit(unitPosition);
                if (iskillEffectListUnit2.hp > 0)
                {
                  flag = true;
                  int hp = iskillEffectListUnit2.hp;
                  applyChangeSkillEffects.add(unitPosition, iskillEffectListUnit2, unitPosition == upMove);
                  skillLogicEffect.init(env, pse, forceIdArray, targetForce, up, unit, isAI, extData);
                  skillLogicEffect.execute(unitPosition, iskillEffectListUnit2, applyChangeSkillEffects, (BL.Panel) null, iskillEffectListUnit1, parameter, targets);
                  if (iskillEffectListUnit2.hp < hp)
                  {
                    if (skillLogicEffect.damageBySwapHeal)
                    {
                      if (iskillEffectListUnit2.hp <= 0 && deads != null)
                        deads.Add(iskillEffectListUnit2);
                    }
                    else
                    {
                      iskillEffectListUnit2.skillEffects.RemoveEffect(1000418, env, iskillEffectListUnit2);
                      BL.ISkillEffectListUnit skillEffectListUnit = !(effect.investUnit != (BL.Unit) null) || env.getForceID(effect.investUnit) == env.getForceID(unit.originalUnit) ? unit : (isAI ? (BL.ISkillEffectListUnit) env.getAIUnit(effect.investUnit) : (BL.ISkillEffectListUnit) effect.investUnit);
                      if (iskillEffectListUnit2.hp <= 0)
                      {
                        if (!isAI)
                        {
                          if (skillEffectListUnit != null)
                          {
                            ++skillEffectListUnit.originalUnit.killCount;
                            iskillEffectListUnit2.originalUnit.killedBy = skillEffectListUnit.originalUnit;
                          }
                          if (env.getForceID(iskillEffectListUnit2.originalUnit) == BL.ForceID.player)
                            env.updateIntimateByDefense(iskillEffectListUnit2.originalUnit);
                        }
                        deads?.Add(iskillEffectListUnit2);
                        skillEffectListUnit?.skillEffects.AddKillCount(1);
                      }
                      if (!isAI && skillEffectListUnit != null && !iskillEffectListUnit2.originalUnit.isFacility)
                      {
                        if (!skillEffectListUnit.originalUnit.isFacility)
                          skillEffectListUnit.originalUnit.attackDamage += hp - iskillEffectListUnit2.hp;
                        else
                          iskillEffectListUnit2.originalUnit.addReceivedLandformDamage(hp - iskillEffectListUnit2.hp);
                      }
                    }
                  }
                  BL.ExecuteSkillEffectResult skillEffectResult = skillEffectResultArray1[index1];
                  int index2 = skillEffectResult.targets.IndexOf(unitPosition);
                  if (index2 == -1)
                  {
                    skillEffectResult.targets.Add(unitPosition);
                    skillEffectResult.target_prev_hps.Add(hp);
                    skillEffectResult.target_hps.Add(iskillEffectListUnit2.hp);
                    skillEffectResult.disp_target_hps.Add(skillLogicEffect.isDispHpNumber());
                  }
                  else
                  {
                    skillEffectResult.target_hps[index2] = iskillEffectListUnit2.hp;
                    skillEffectResult.disp_target_hps[index2] |= skillLogicEffect.isDispHpNumber();
                  }
                }
              }
            }
            else
            {
              Tuple<BL.Panel[], int> tuple = (Tuple<BL.Panel[], int>) skillParam.param;
              BL.Panel[] panelArray = tuple.Item1;
              index1 = tuple.Item2;
              if (skillEffectResultArray1[index1] == null)
                skillEffectResultArray1[index1] = new BL.ExecuteSkillEffectResult()
                {
                  skill = effect.baseSkill
                };
              foreach (BL.Panel targetPanel in panelArray)
              {
                flag = true;
                skillLogicEffect.init(env, pse, forceIdArray, targetForce, up, unit, isAI, extData);
                skillLogicEffect.execute((BL.UnitPosition) null, (BL.ISkillEffectListUnit) null, applyChangeSkillEffects, targetPanel, iskillEffectListUnit1, (Judgement.BattleParameter) null, (List<BL.UnitPosition>) null);
                BL.ExecuteSkillEffectResult skillEffectResult = skillEffectResultArray1[index1];
                if (!skillEffectResult.targetPanels.Contains(targetPanel))
                  skillEffectResult.targetPanels.Add(targetPanel);
              }
            }
            if (flag && skillEffectListList[index1] != null)
            {
              foreach (BL.SkillEffect skillEffect1 in skillEffectListList[index1])
              {
                if (skillEffect1.useRemain.HasValue)
                {
                  int? useRemain = skillEffect1.useRemain;
                  int num1 = 1;
                  if (useRemain.GetValueOrDefault() >= num1 & useRemain.HasValue)
                  {
                    BL.SkillEffect skillEffect2 = skillEffect1;
                    useRemain = skillEffect2.useRemain;
                    int? nullable = useRemain;
                    skillEffect2.useRemain = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
                    useRemain = skillEffect1.useRemain;
                    int num2 = 0;
                    if (useRemain.GetValueOrDefault() == num2 & useRemain.HasValue && !isAI && skillEffect1.isLandTagEffect)
                      unit.skillEffects.LandTagModified.commit();
                  }
                }
              }
              skillEffectListList[index1] = (List<BL.SkillEffect>) null;
            }
          }
          applyChangeSkillEffects.execute();
          BL.ExecuteSkillEffectResult[] skillEffectResultArray = skillEffectResultArray1;
          for (int index = 0; index < skillEffectResultArray.Length; ++index)
          {
            BL.ExecuteSkillEffectResult skillEffectResult = skillEffectResultArray[index];
            if (skillEffectResult != null)
              yield return skillEffectResult;
          }
          skillEffectResultArray = (BL.ExecuteSkillEffectResult[]) null;
        }
      }
    }

    public ActionResult createBattleSkillResult(
      BL.Skill skill,
      BL.AIUnit unit,
      List<BL.AIUnit> ai_targets)
    {
      BL.BattleSkillResult battleSkillResult = BL.BattleSkillResult.createBattleSkillResult(skill.id, (BL.ISkillEffectListUnit) unit, ai_targets.Select<BL.AIUnit, BL.Unit>((Func<BL.AIUnit, BL.Unit>) (x => x.unit)).ToList<BL.Unit>(), this.phaseState.turnCount);
      battleSkillResult.isMove = true;
      battleSkillResult.row = unit.row;
      battleSkillResult.column = unit.column;
      if (!(battleSkillResult is BL.BattleSkillResultExtendEffect resultExtendEffect))
        return (ActionResult) battleSkillResult;
      resultExtendEffect.init(this);
      return (ActionResult) battleSkillResult;
    }

    public void setSomeAction()
    {
      if (this.phaseState.state == BL.Phase.enemy)
      {
        this.enemyCallSkillState.isSomeAction = true;
      }
      else
      {
        if (this.phaseState.state == BL.Phase.neutral)
          return;
        this.playerCallSkillState.isSomeAction = true;
      }
    }

    public BL.ClassValue<List<BL.UnitPosition>> getActionUnitsList(BL.Phase state)
    {
      switch (state)
      {
        case BL.Phase.player:
          return this.playerActionUnits;
        case BL.Phase.neutral:
          return this.neutralActionUnits;
        case BL.Phase.enemy:
          return this.enemyActionUnits;
        default:
          return (BL.ClassValue<List<BL.UnitPosition>>) null;
      }
    }

    public BL.ClassValue<List<BL.UnitPosition>> currentActionUnitsList()
    {
      return this.getActionUnitsList(this.phaseState.state);
    }

    public void resetActionList(BL.ForceID forceId)
    {
      BL.ClassValue<List<BL.Unit>> classValue1;
      BL.ClassValue<List<BL.UnitPosition>> classValue2;
      switch (forceId)
      {
        case BL.ForceID.player:
          classValue1 = this.playerUnits;
          classValue2 = this.playerActionUnits;
          break;
        case BL.ForceID.neutral:
          classValue1 = this.neutralUnits;
          classValue2 = this.neutralActionUnits;
          break;
        case BL.ForceID.enemy:
          classValue1 = this.enemyUnits;
          classValue2 = this.enemyActionUnits;
          break;
        default:
          return;
      }
      List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
      foreach (BL.Unit unit in classValue1.value)
      {
        if (unit.isEnable && !unit.isDead)
        {
          BL.UnitPosition unitPosition = this.getUnitPosition(unit);
          unitPosition.resetOriginalPosition(this);
          unitPositionList.Add(unitPosition);
        }
      }
      classValue2.value = unitPositionList;
      this.completedActionUnits.value = new List<BL.UnitPosition>();
    }

    public BL.ClassValue<List<BL.UnitPosition>> getActionUnits(BL.ForceID forceId)
    {
      switch (forceId)
      {
        case BL.ForceID.player:
          return this.playerActionUnits;
        case BL.ForceID.neutral:
          return this.neutralActionUnits;
        case BL.ForceID.enemy:
          return this.enemyActionUnits;
        default:
          return (BL.ClassValue<List<BL.UnitPosition>>) null;
      }
    }

    public bool isCompleted(BL.Unit unit) => unit.isDead || this.getUnitPosition(unit).isCompleted;

    public BL.ClassValue<List<BL.UnitPosition>> getActionUnits(BL.UnitPosition up)
    {
      if (this.playerActionUnits.value.Contains(up))
        return this.playerActionUnits;
      if (this.neutralActionUnits.value.Contains(up))
        return this.neutralActionUnits;
      return this.enemyActionUnits.value.Contains(up) ? this.enemyActionUnits : (BL.ClassValue<List<BL.UnitPosition>>) null;
    }

    public bool currentPhaseUnitp(BL.Unit unit)
    {
      BL.ClassValue<List<BL.Unit>> classValue = (BL.ClassValue<List<BL.Unit>>) null;
      switch (this.phaseState.state)
      {
        case BL.Phase.player:
        case BL.Phase.pvp_move_unit_waiting:
        case BL.Phase.pvp_player_start:
        case BL.Phase.pvp_disposition:
          classValue = this.playerUnits;
          break;
        case BL.Phase.neutral:
          classValue = this.neutralUnits;
          break;
        case BL.Phase.enemy:
        case BL.Phase.pvp_enemy_start:
          classValue = this.enemyUnits;
          break;
      }
      return classValue != null && classValue.value.Contains(unit);
    }

    public bool currentPhaseUnitp(BL.UnitPosition up) => this.currentPhaseUnitp(up.unit);

    public int getActionUnitsIndexOf(BL.UnitPosition up, ref BL.ForceID forceId)
    {
      if (up.unit == (BL.Unit) null)
        return -1;
      if (forceId == BL.ForceID.none)
        forceId = this.getForceID(up.unit);
      BL.ClassValue<List<BL.UnitPosition>> classValue;
      switch (forceId)
      {
        case BL.ForceID.player:
          classValue = this.playerActionUnits;
          break;
        case BL.ForceID.neutral:
          classValue = this.neutralActionUnits;
          break;
        case BL.ForceID.enemy:
          classValue = this.enemyActionUnits;
          break;
        default:
          return -1;
      }
      return classValue.value.IndexOf(up);
    }

    public int getActionUnitsIndexOf(BL.Unit unit, ref BL.ForceID forceId)
    {
      return unit == (BL.Unit) null ? -1 : this.getActionUnitsIndexOf(this.getUnitPosition(unit), ref forceId);
    }

    public bool allDeadUnitsp(BL.ForceID forceId)
    {
      BL.ClassValue<List<BL.Unit>> classValue;
      switch (forceId)
      {
        case BL.ForceID.player:
          classValue = this.playerUnits;
          break;
        case BL.ForceID.neutral:
          classValue = this.neutralUnits;
          break;
        case BL.ForceID.enemy:
          classValue = this.enemyUnits;
          break;
        default:
          return false;
      }
      foreach (BL.Unit unit in classValue.value)
      {
        if (unit.isSpawned && !unit.isDead)
          return false;
      }
      return true;
    }

    public BL.ForceID[] getTargetForce(BL.Unit unit, bool isCharm)
    {
      if (isCharm)
      {
        switch (this.getForceID(unit))
        {
          case BL.ForceID.player:
            return BattleFuncs.ForceIDArrayEnemyTarget;
          case BL.ForceID.enemy:
            return BattleFuncs.ForceIDArrayPlayerTarget;
          default:
            return (BL.ForceID[]) null;
        }
      }
      else
      {
        if (unit.targetForce != null)
          return unit.targetForce;
        switch (this.getForceID(unit))
        {
          case BL.ForceID.player:
            unit.targetForce = BattleFuncs.ForceIDArrayPlayerTarget;
            break;
          case BL.ForceID.neutral:
            unit.targetForce = BattleFuncs.ForceIDArrayNeutralTarget;
            break;
          case BL.ForceID.enemy:
            unit.targetForce = BattleFuncs.ForceIDArrayEnemyTarget;
            break;
          default:
            return (BL.ForceID[]) null;
        }
        return unit.targetForce;
      }
    }

    public bool CheckStageFinished()
    {
      if (this.condition.type == BL.ConditionType.bossdown && this.getBossUnit().isDead)
        return true;
      BL.GameoverType gamevoerType = this.getGamevoerType();
      if (gamevoerType == BL.GameoverType.guestdown && this.playerUnits.value.Find((Predicate<BL.Unit>) (x => x.playerUnit.is_guest && !x.isDead)) == (BL.Unit) null || gamevoerType == BL.GameoverType.playerdown && this.playerUnits.value.Find((Predicate<BL.Unit>) (x => !x.playerUnit.is_guest && !x.isDead)) == (BL.Unit) null || this.getLoseUnitList().Find((Predicate<BL.Unit>) (x => x.isDead)) != (BL.Unit) null)
        return true;
      if (this.battleInfo.isEarthMode)
      {
        if (this.battleInfo.isExtraEarthQuest)
        {
          BL.Unit unit = this.playerUnits.value.Find((Predicate<BL.Unit>) (x => x.unit.same_character_id == 11002));
          if (unit != (BL.Unit) null && unit.isDead)
            return true;
        }
        else
        {
          BL.Unit unit = this.playerUnits.value.Find((Predicate<BL.Unit>) (x => x.is_leader));
          if (unit != (BL.Unit) null && unit.isDead)
            return true;
        }
      }
      return this.allDeadUnitsp(BL.ForceID.enemy) || this.allDeadUnitsp(BL.ForceID.player);
    }

    public BL.Story getStory(BL.StoryType type)
    {
      foreach (BL.Story story in this.storyList.value)
      {
        if (story.type == type)
          return story;
      }
      return (BL.Story) null;
    }

    public BL.Story getStoryStart() => this.getStory(BL.StoryType.battle_start);

    public BL.Story getFirstTurnStart() => this.getStory(BL.StoryType.first_turn_start);

    public BL.Story getStoryWin() => this.getStory(BL.StoryType.battle_win);

    public BL.Story getStorySpawn(BL.Unit unit)
    {
      foreach (BL.Story storySpawn in this.storyList.value)
      {
        if (storySpawn.type == BL.StoryType.spawn_unit && storySpawn.datas.Length != 0 && storySpawn.datas[0] is int && (int) storySpawn.datas[0] == unit.unit.ID)
          return storySpawn;
      }
      return (BL.Story) null;
    }

    public BL.Story getStoryUnitForUnit(BL.Unit from, BL.Unit to)
    {
      foreach (BL.Story storyUnitForUnit in this.storyList.value)
      {
        if (storyUnitForUnit.type == BL.StoryType.unit_for_unit && storyUnitForUnit.datas.Length != 0 && storyUnitForUnit.datas[0] is int)
        {
          int data = (int) storyUnitForUnit.datas[0];
          if (data == from.unit.ID && data == to.unit.ID)
            return storyUnitForUnit;
        }
      }
      return (BL.Story) null;
    }

    public List<BL.Story> getStoryUnitForAll(BL.Unit from)
    {
      List<BL.Story> storyUnitForAll = new List<BL.Story>();
      foreach (BL.Story story in this.storyList.value)
      {
        if (story.type == BL.StoryType.unit_for_unit && story.datas.Length != 0 && story.datas[0] is int && (int) story.datas[0] == from.unit.ID)
          storyUnitForAll.Add(story);
      }
      return storyUnitForAll;
    }

    public List<BL.Story> getDuelStorys(BL.Unit attack, BL.Unit defense)
    {
      List<BL.Story> duelStorys = new List<BL.Story>();
      foreach (BL.Story story in this.storyList.value)
      {
        if ((story.type == BL.StoryType.duel_start || story.type == BL.StoryType.duel_unit_dead) && !story.isRead && story.datas.Length > 1 && story.datas[0] is int && story.datas[1] is int)
        {
          int data1 = (int) story.datas[0];
          int data2 = (int) story.datas[1];
          bool flag = false;
          switch (data1)
          {
            case 0:
              if (data2 == attack.playerUnit.id && attack.playerUnit.is_enemy || data2 == defense.playerUnit.id && defense.playerUnit.is_enemy)
              {
                flag = true;
                break;
              }
              break;
            case 1:
              if (data2 == attack.playerUnit.id && attack.playerUnit.is_enemy)
              {
                flag = true;
                break;
              }
              break;
            case 2:
              if (data2 == defense.playerUnit.id && defense.playerUnit.is_enemy)
              {
                flag = true;
                break;
              }
              break;
          }
          if (flag)
            duelStorys.Add(story);
        }
      }
      return duelStorys;
    }

    public List<BL.Story> getStoryWaveOffense(int turn, int wave)
    {
      return this.findStoryTurnStart(BL.StoryPhase.offense, turn, wave);
    }

    public List<BL.Story> getStoryWaveDefense(int turn, int wave)
    {
      return this.findStoryTurnStart(BL.StoryPhase.defense, turn, wave);
    }

    public List<BL.Story> getStoryOffense(int turn)
    {
      return this.findStoryTurnStart(BL.StoryPhase.offense, turn);
    }

    public List<BL.Story> getStoryDefense(int turn)
    {
      return this.findStoryTurnStart(BL.StoryPhase.defense, turn);
    }

    private List<BL.Story> findStoryTurnStart(BL.StoryPhase sphase, int turn, int wave = 0)
    {
      List<BL.Story> storyTurnStart = new List<BL.Story>();
      foreach (BL.Story story in this.storyList.value)
      {
        if (!story.isRead && story.type == BL.StoryType.turn_start && (BL.StoryPhase) story.datas[0] == sphase && (int) story.datas[1] == turn && (wave == 0 || (int) story.datas[2] == wave))
          storyTurnStart.Add(story);
      }
      return storyTurnStart;
    }

    public List<BL.Story> getStoryWaveClear(int wave)
    {
      List<BL.Story> storyWaveClear = new List<BL.Story>();
      foreach (BL.Story story in this.storyList.value)
      {
        if (!story.isRead && story.type == BL.StoryType.wave_clear && (int) story.datas[0] == wave)
          storyWaveClear.Add(story);
      }
      return storyWaveClear;
    }

    public List<BL.Story> getStoryOffenseInArea(int row, int column)
    {
      return this.findStoryInArea(BL.StoryPhase.offense, row, column);
    }

    public List<BL.Story> getStoryDefenseInArea(int row, int column)
    {
      return this.findStoryInArea(BL.StoryPhase.defense, row, column);
    }

    private List<BL.Story> findStoryInArea(BL.StoryPhase sphase, int row, int column)
    {
      List<BL.Story> storyInArea = new List<BL.Story>();
      foreach (BL.Story story in this.storyList.value)
      {
        if (!story.isRead && story.type == BL.StoryType.unit_in_area && (BL.StoryPhase) story.datas[0] == sphase)
        {
          int num1 = row - (int) story.datas[1];
          int num2 = column - (int) story.datas[2];
          if (0 <= num1 && num1 < (int) story.datas[3] && 0 <= num2 && num2 < (int) story.datas[4])
            storyInArea.Add(story);
        }
      }
      return storyInArea;
    }

    public List<BL.Story> getStoryDefeat(int unitId, bool swRead = true)
    {
      List<BL.Story> sl = new List<BL.Story>();
      this.storyList.value.ForEach((Action<BL.Story>) (s =>
      {
        if (s.isRead || s.type != BL.StoryType.defeat_player || (int) s.datas[0] != unitId)
          return;
        if (swRead)
          s.isRead = true;
        sl.Add(s);
      }));
      return sl;
    }

    public void setCurrentUnitByPlayerInput(BL.Unit unit, Action<BL.UnitPosition> f)
    {
      this.unitCurrent.setCurrentByPlayerInput(unit, this, f);
    }

    public void setCurrentUnitWith(BL.Unit unit, Action<BL.UnitPosition> f)
    {
      this.unitCurrent.setCurrentWith(unit, this, f);
    }

    public void setCurrentUnitWithSetOnly(BL.Unit unit)
    {
      this.unitCurrent.setCurrentWithSetOnly(unit);
    }

    public BL.ForceID getForceID(BL.Unit unit)
    {
      if (unit == (BL.Unit) null)
        return BL.ForceID.none;
      if (this.playerUnits.value.Contains(unit))
        return BL.ForceID.player;
      if (this.enemyUnits.value.Contains(unit))
        return BL.ForceID.enemy;
      if (this.neutralUnits.value.Contains(unit))
        return BL.ForceID.neutral;
      return this.facilityUnits.value.Contains(unit) ? unit.facility.thisForce : BL.ForceID.none;
    }

    public BL.ClassValue<List<BL.Unit>> forceUnits(BL.ForceID forceId)
    {
      switch (forceId)
      {
        case BL.ForceID.player:
          return this.playerUnits;
        case BL.ForceID.neutral:
          return this.neutralUnits;
        case BL.ForceID.enemy:
          return this.enemyUnits;
        default:
          return (BL.ClassValue<List<BL.Unit>>) null;
      }
    }

    public void updateUnitBattleStatus(DuelResult duelResult, BL.Unit attack, BL.Unit defense)
    {
      if (duelResult.isPlayerAttack)
        this.updateIntimateByAttack(this.getUnitPosition(attack));
      if (attack.hp <= 0)
      {
        ++defense.killCount;
        defense.skillEffects.AddKillCount(1);
        attack.killedBy = defense;
        int oDamage = 0;
        ((IEnumerable<BL.DuelTurn>) duelResult.turns).ForEach<BL.DuelTurn>((Action<BL.DuelTurn>) (x =>
        {
          if (x.isAtackker)
            return;
          oDamage += x.realDamage - x.damage;
        }));
        attack.overkillDamage = oDamage;
        if (Object.op_Inequality((Object) Singleton<NGBattleManager>.GetInstance(), (Object) null) && Singleton<NGBattleManager>.GetInstance().isGvg && !attack.isFacility && !defense.isFacility)
          defense.attackOverkillDamage += attack.overkillDamage;
        if (attack.playerUnit.is_guest)
          attack.overkillDamage = 0;
      }
      if (defense.hp <= 0)
      {
        ++attack.killCount;
        attack.skillEffects.AddKillCount(1);
        defense.killedBy = attack;
        int oDamage = 0;
        ((IEnumerable<BL.DuelTurn>) duelResult.turns).ForEach<BL.DuelTurn>((Action<BL.DuelTurn>) (x =>
        {
          if (!x.isAtackker)
            return;
          oDamage += x.realDamage - x.damage;
        }));
        defense.overkillDamage = oDamage;
        if (Object.op_Inequality((Object) Singleton<NGBattleManager>.GetInstance(), (Object) null) && Singleton<NGBattleManager>.GetInstance().isGvg && !attack.isFacility && !defense.isFacility)
          attack.attackOverkillDamage += defense.overkillDamage;
        if (defense.playerUnit.is_guest)
          defense.overkillDamage = 0;
      }
      if (duelResult.attackFromDamage > 0)
      {
        ++defense.attackCount;
        if (!attack.isFacility && !defense.isFacility)
        {
          defense.attackDamage += duelResult.attackFromDamage;
          attack.receivedDamage += duelResult.attackFromDamage;
        }
      }
      if (duelResult.defenseFromDamage > 0)
      {
        ++attack.attackCount;
        if (!attack.isFacility && !defense.isFacility)
        {
          attack.attackDamage += duelResult.defenseFromDamage;
          defense.receivedDamage += duelResult.defenseFromDamage;
        }
      }
      ++attack.duelCount;
      ++defense.duelCount;
      this.updateIntimateByDefense(duelResult.isPlayerAttack ? attack : defense);
    }

    public bool checkDeadCount(int id, int count, bool isAI)
    {
      return count == 0 ? this.enemyUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
      {
        if (id == 0)
          return true;
        return id != 0 && x.playerUnit.group_id.HasValue && x.playerUnit.group_id.Value == id;
      })).All<BL.Unit>((Func<BL.Unit, bool>) (x =>
      {
        if (x.isDead || x.hp == 0)
          return true;
        return isAI && this.getAIUnit(x) != null && this.getAIUnit(x).isDead;
      })) : count <= this.enemyUnits.value.Count<BL.Unit>((Func<BL.Unit, bool>) (x =>
      {
        if (id != 0 && (id == 0 || !x.playerUnit.group_id.HasValue || x.playerUnit.group_id.Value != id))
          return false;
        if (x.isDead || x.hp == 0)
          return true;
        return isAI && this.getAIUnit(x) != null && this.getAIUnit(x).isDead;
      }));
    }

    public bool isReinforceUnitForSmash(PlayerUnit pu, bool isAI = false)
    {
      return pu.reinforcement != null && pu.reinforcement.reinforcement_logic.Enum == BattleReinforcementLogicEnum.smash && this.checkDeadCount(pu.reinforcement.arg1_value, pu.reinforcement.arg2_value, isAI);
    }

    public List<BL.Panel> dangerAria => this.mDangerAria;

    public void hideDangerAria()
    {
      if (this.mDangerAria == null)
        return;
      BattleFuncs.setAttributePanels((IEnumerable<BL.Panel>) this.mDangerAria, BL.PanelAttribute.danger, true);
    }

    public void viewDangerAria()
    {
      if (this.mDangerAria == null)
        this.createDangerAria();
      else
        BattleFuncs.setAttributePanels((IEnumerable<BL.Panel>) this.mDangerAria, BL.PanelAttribute.danger, false);
    }

    public void createDangerAria()
    {
      this.hideDangerAria();
      if (!this.isViewDengerArea.value)
        return;
      this.mDangerAria = BattleFuncs.createDangerPanels(this.playerTarget).ToList<BL.Panel>();
      BattleFuncs.setAttributePanels((IEnumerable<BL.Panel>) this.mDangerAria, BL.PanelAttribute.danger, false);
    }

    public static BL.BattleModified<T> Observe<T>(T v) where T : BL.ModelBase
    {
      return new BL.BattleModified<T>(v);
    }

    public enum AIType
    {
      normal,
    }

    public class AIUnitNetwork
    {
      public int? unitPosition;
      public int hp;
      public int row;
      public int column;
      public BL.AIType type;
      public List<ActionResultNetwork> actionResults;
    }

    [Serializable]
    public class AIUnit : BL.UnitPosition, BL.ISkillEffectListUnit
    {
      [SerializeField]
      private BL.UnitPosition mUnitPosition;
      [SerializeField]
      private int mHp;
      [SerializeField]
      private BL.AIType mType;
      [SerializeField]
      private List<ActionResult> mActionResults;
      [SerializeField]
      private BL.SkillEffectList mSkillEffectList;
      [SerializeField]
      private bool mIsDead;
      [SerializeField]
      private BL.Skill mOugi;
      [SerializeField]
      private Dictionary<int, BL.Skill> mCommandSkills;
      [SerializeField]
      private bool checkActionRangeBySetHp;
      [SerializeField]
      private bool mIsCharmStart;
      [SerializeField]
      private int mDeadCount;
      [SerializeField]
      private int mFacilitySpawnOrder;
      [NonSerialized]
      private BL.ForceID mForceId = BL.ForceID.none;
      public Action action;
      private KeyValuePair<int, int>? mCombat;
      [NonSerialized]
      private int mSkillLevel;

      private AIUnit()
      {
      }

      public AIUnit(BL.UnitPosition up, BL.AIType type)
      {
        bool isNotUseDeepCopy = PerformanceConfig.GetInstance().IsNotUseDeepCopy;
        this.mHp = up.unit.hp;
        this.mUnitPosition = up;
        this.mType = type;
        this.mActionResults = (List<ActionResult>) null;
        this.action = (Action) null;
        this.mSkillEffectList = !isNotUseDeepCopy ? CopyUtil.DeepCopy<BL.SkillEffectList>(up.unit.skillEffects) : up.unit.skillEffects.Clone();
        this.mId = up.id;
        this.mUnit = up.unit;
        this.mOriginalRow = this.mRow = up.originalRow;
        this.mOriginalColumn = this.mColumn = up.originalColumn;
        this.mUsedMoveCost = up.usedMoveCost;
        this.mCompletedCount = up.completedCount;
        this.mActionCount = up.actionCount;
        this.mMaxCompletedCount = up.maxCompletedCount;
        this.mMaxActionCount = up.maxActionCount;
        this.mCantChangeCurrentActionCount = up.cantChangeCurrentActionCount;
        this.mDontUseSkillAgain = up.dontUseSkillAgain;
        this.mMoveDistance = up.moveDistance;
        this.mIsDead = this.mUnit.isDead || this.mUnit.hp <= 0;
        this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mMovePanels = this.mCompletePanels = (HashSet<BL.Panel>) null;
        this.asterNodeCache = up.asterNodeCache;
        this.mCommandSkills = new Dictionary<int, BL.Skill>();
        if (this.mUnit.hasOugi)
          this.mOugi = !isNotUseDeepCopy ? CopyUtil.DeepCopy<BL.Skill>(this.mUnit.ougi) : new BL.Skill(this.mUnit.ougi);
        foreach (BL.Skill skill in this.mUnit.skills)
        {
          if (skill.isCommand)
            this.mCommandSkills[skill.id] = !isNotUseDeepCopy ? CopyUtil.DeepCopy<BL.Skill>(skill) : new BL.Skill(skill);
        }
        this.mActionMovePanels = new HashSet<BL.Panel>();
        this.mHealMovePanels = new HashSet<BL.Panel>();
        this.mSkillMovePanelsDic = new Dictionary<int, HashSet<BL.Panel>>();
        this.checkActionRangeBySetHp = true;
        this.mIsCharmStart = this.mUnit.IsCharm;
        this.mDeadCount = 0;
        this.mFacilitySpawnOrder = this.mUnit.facilitySpawnOrder;
      }

      public AIUnit(BL.AIUnit org)
      {
        this.mHp = org.mHp;
        this.mUnitPosition = org.mUnitPosition;
        this.mType = org.mType;
        this.mActionResults = org.mActionResults;
        this.action = org.action;
        this.mSkillEffectList = org.mSkillEffectList;
        this.mId = org.mId;
        this.mUnit = org.mUnit;
        this.mOriginalRow = org.mOriginalRow;
        this.mRow = org.mRow;
        this.mOriginalColumn = org.mOriginalColumn;
        this.mColumn = org.mColumn;
        this.mUsedMoveCost = org.mUsedMoveCost;
        this.mCompletedCount = org.mCompletedCount;
        this.mActionCount = org.mActionCount;
        this.mMaxCompletedCount = org.mMaxCompletedCount;
        this.mMaxActionCount = org.mMaxActionCount;
        this.mCantChangeCurrentActionCount = org.mCantChangeCurrentActionCount;
        this.mDontUseSkillAgain = org.mDontUseSkillAgain;
        this.mMoveDistance = org.mMoveDistance;
        this.mIsDead = org.mIsDead;
        this.mAllMoveHealRangePanels = org.mAllMoveHealRangePanels;
        this.mAllMoveActionRangePanels = org.mAllMoveActionRangePanels;
        this.mMovePanels = org.mMovePanels;
        this.mCompletePanels = org.mCompletePanels;
        this.asterNodeCache = org.asterNodeCache;
        this.mCommandSkills = org.mCommandSkills;
        this.mOugi = org.mOugi;
        this.mActionMovePanels = org.mActionMovePanels;
        this.mHealMovePanels = org.mHealMovePanels;
        this.mSkillMovePanelsDic = org.mSkillMovePanelsDic;
        this.checkActionRangeBySetHp = org.checkActionRangeBySetHp;
        this.mIsCharmStart = org.mIsCharmStart;
        this.mDeadCount = org.mDeadCount;
        this.mFacilitySpawnOrder = org.mFacilitySpawnOrder;
        this.mForceId = org.mForceId;
      }

      public HashSet<BL.Panel> actionMovePanels
      {
        get
        {
          HashSet<BL.Panel> actionRangePanels = this.getAllMoveActionRangePanels();
          return actionRangePanels.Count != 0 ? this.mActionMovePanels : actionRangePanels;
        }
      }

      public HashSet<BL.Panel> healMovePanels
      {
        get
        {
          HashSet<BL.Panel> moveHealRangePanels = this.getAllMoveHealRangePanels();
          return moveHealRangePanels.Count != 0 ? this.mHealMovePanels : moveHealRangePanels;
        }
      }

      public HashSet<BL.Panel> getSkillMovePanels(BL.Skill skill)
      {
        HashSet<BL.Panel> skillRangePanels = this.getAllMoveSkillRangePanels(skill);
        return skillRangePanels.Count != 0 ? this.mSkillMovePanelsDic[skill.id] : skillRangePanels;
      }

      public int hp
      {
        get => this.mHp;
        set
        {
          int mHp = this.mHp;
          int num1 = Mathf.Max(Mathf.Min(value, this.unitPosition.unit.parameter.Hp), 0);
          int num2 = num1;
          if (mHp != num2)
          {
            if (this.checkActionRangeBySetHp && (this.originalUnit.magicBullets.Length != 0 || this.enabledSkillEffect(BattleskillEffectLogicEnum.fix_range).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
            {
              if (x.effect.HasKey(BattleskillEffectLogicArgumentEnum.min_hp_percentage) && (double) x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_hp_percentage) != 0.0)
                return true;
              return x.effect.HasKey(BattleskillEffectLogicArgumentEnum.max_hp_percentage) && (double) x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_hp_percentage) != 0.0;
            }))))
            {
              int[] attackRange1 = this.attackRange;
              int[] healRange1 = this.healRange;
              this.mHp = num1;
              int[] attackRange2 = this.attackRange;
              int[] healRange2 = this.healRange;
              if (attackRange1[0] != attackRange2[0] || attackRange1[1] != attackRange2[1])
                this.clearMoveActionRangePanelCache();
              if (healRange1.Length != healRange2.Length || healRange1.Length == 2 && (healRange1[0] != healRange2[0] || healRange1[1] != healRange2[1]))
                this.clearMoveHealRangePanelCache();
            }
            else
              this.mHp = num1;
          }
          ++this.revision;
        }
      }

      public bool isDead
      {
        get => this.mIsDead;
        set
        {
          this.mIsDead = value;
          ++this.revision;
        }
      }

      public BL.UnitPosition unitPosition
      {
        get => this.mUnitPosition;
        set
        {
          if (this.mUnitPosition == value)
            return;
          this.mUnitPosition = value;
          ++this.revision;
        }
      }

      public Judgement.BattleParameter parameter
      {
        get => Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) this);
      }

      public BL.AIType type
      {
        get => this.mType;
        set
        {
          this.mType = value;
          ++this.revision;
        }
      }

      public bool isHealer => this.healRange.Length != 0;

      public bool hasOugi => this.mOugi != null;

      public BL.Skill ougi => this.mOugi;

      public BL.Skill[] skills => this.mCommandSkills.Values.ToArray<BL.Skill>();

      public IEnumerable<BL.Skill> commandSkills
      {
        get => (IEnumerable<BL.Skill>) this.mCommandSkills.Values;
      }

      public BL.Skill getSkill(int skillId)
      {
        BL.Skill skill = (BL.Skill) null;
        if (this.hasOugi && this.mOugi.id == skillId)
          skill = this.mOugi;
        else
          this.mCommandSkills.TryGetValue(skillId, out skill);
        return skill;
      }

      public BL.ForceID getForceID(BL env)
      {
        if (this.mForceId == BL.ForceID.none)
          this.mForceId = env.getForceID(this.mUnit);
        return this.mForceId;
      }

      public List<ActionResult> actionResults
      {
        get => this.mActionResults;
        set
        {
          this.mActionResults = value;
          ++this.revision;
        }
      }

      public int deadCount
      {
        get => this.mDeadCount;
        set
        {
          this.mDeadCount = value;
          ++this.revision;
        }
      }

      public int facilitySpawnOrder
      {
        get => this.mFacilitySpawnOrder;
        set
        {
          this.mFacilitySpawnOrder = value;
          ++this.revision;
        }
      }

      public bool isAction => this.mActionCount != 0 && !this.IsDontAction;

      public BL.Panel GetTargetPanelOrNull(BL env) => this.mUnit.GetTargetPanel(env);

      public BL.Unit originalUnit => this.mUnit;

      public BL.SkillEffectList skillEffects => this.mSkillEffectList;

      public bool HasAilment => this.mSkillEffectList.HasAilment;

      public bool IsDontAction
      {
        get
        {
          if (this.mUnit.isFacility)
            return true;
          return this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.do_not_act, BattleskillEffectLogicEnum.sleep);
        }
      }

      public bool IsDontMove
      {
        get
        {
          if (this.mUnit.isFacility)
            return true;
          return this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.do_not_move, BattleskillEffectLogicEnum.sleep);
        }
      }

      public bool IsDontEvasion
      {
        get
        {
          return this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.sleep);
        }
      }

      public bool IsCharm
      {
        get => this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.charm);
      }

      public bool IsJumping
      {
        get => this.mSkillEffectList.Where(BattleskillEffectLogicEnum.jump).Any<BL.SkillEffect>();
      }

      public bool IsDontUseCommand(BL.Skill skill)
      {
        return BattleFuncs.isDontUseCommand((BL.ISkillEffectListUnit) this, skill);
      }

      public bool IsDontUseOugi(BL.Skill skill)
      {
        return BattleFuncs.isDontUseOugi((BL.ISkillEffectListUnit) this, skill);
      }

      public bool IsDontUseSkill(int skill_id)
      {
        return this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.seal) && this.skillEffects.IsSealedSkill(skill_id);
      }

      public bool IsDontUseSkillEffect(BL.SkillEffect effect)
      {
        return this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.seal) && this.skillEffects.IsSealedSkillEffect(effect);
      }

      public bool CantChangeCurrent
      {
        get
        {
          return this.mSkillEffectList.HasAilmentEffectLogic(BattleskillEffectLogicEnum.can_not_change_current);
        }
      }

      public bool CanHeal(BattleskillSkillType skillType = (BattleskillSkillType) 0)
      {
        return BattleFuncs.canHeal((BL.ISkillEffectListUnit) this, skillType);
      }

      public bool HasEnabledSkillEffect(BattleskillEffectLogicEnum logic)
      {
        return this.enabledSkillEffect(logic).Any<BL.SkillEffect>();
      }

      public IEnumerable<BL.SkillEffect> enabledSkillEffect(BattleskillEffectLogicEnum logic)
      {
        return this.mSkillEffectList.Where(logic).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect((BL.ISkillEffectListUnit) this, x)));
      }

      public void setSkillEffects(BL.SkillEffectList value) => this.mSkillEffectList = value;

      public IEnumerable<BL.SkillEffect> getSkillEffects(BattleskillEffectLogicEnum logic)
      {
        return this.mSkillEffectList.Where(logic);
      }

      public bool checkTargetAttribute(BL.Unit.TargetAttribute ta)
      {
        return this.mUnit.checkTargetAttribute(ta);
      }

      public int transformationGroupId
      {
        get => BattleFuncs.getTransformationGroupId((BL.ISkillEffectListUnit) this);
      }

      public bool checkEnableSkill(BattleskillSkill skill)
      {
        return BattleFuncs.checkEnableUnitSkill((BL.ISkillEffectListUnit) this, skill);
      }

      public int[] attackRange => BattleFuncs.getAttackRange((BL.ISkillEffectListUnit) this);

      public int[] healRange => BattleFuncs.getHealRange((BL.ISkillEffectListUnit) this);

      public BL.Unit.GearRange gearRange()
      {
        return BattleFuncs.getGearRange((BL.ISkillEffectListUnit) this);
      }

      public BL.Unit.MagicRange magicRange(BL.MagicBullet mb)
      {
        return BattleFuncs.getMagicRange((BL.ISkillEffectListUnit) this, mb);
      }

      public int Combat
      {
        get
        {
          if (!this.mCombat.HasValue || this.mCombat.Value.Key != this.skillEffects.revision)
          {
            Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) this.unit, true, false);
            this.mCombat = new KeyValuePair<int, int>?(new KeyValuePair<int, int>(this.skillEffects.revision, battleParameter.PhysicalAttack + battleParameter.PhysicalDefense + battleParameter.MagicAttack + battleParameter.MagicDefense + (battleParameter.Evasion + battleParameter.Hit + battleParameter.Critical) / 2));
          }
          return this.mCombat.Value.Value;
        }
      }

      public BL.AIUnitNetwork ToNetwork(BL env)
      {
        return new BL.AIUnitNetwork()
        {
          unitPosition = this.mUnitPosition == null ? new int?() : this.mUnitPosition.ToNetwork(env),
          hp = this.hp,
          row = this.row,
          column = this.column,
          type = this.type,
          actionResults = this.actionResults == null ? (List<ActionResultNetwork>) null : this.actionResults.Select<ActionResult, ActionResultNetwork>((Func<ActionResult, ActionResultNetwork>) (x => x.ToNetwork(env))).ToList<ActionResultNetwork>()
        };
      }

      public static BL.AIUnit FromNetwork(BL.AIUnitNetwork nw, BL env)
      {
        BL.AIUnit aiUnit = nw == null ? (BL.AIUnit) null : new BL.AIUnit(BL.UnitPosition.FromNetwork(nw.unitPosition, env), nw.type);
        aiUnit.hp = nw.hp;
        aiUnit.row = nw.row;
        aiUnit.column = nw.column;
        aiUnit.actionResults = nw.actionResults == null ? (List<ActionResult>) null : nw.actionResults.Select<ActionResultNetwork, ActionResult>((Func<ActionResultNetwork, ActionResult>) (x => ActionResult.FromNetworkCommon((DuelResult.FromNetwork(x, env) ?? BL.BattleSkillResult.FromNetwork(x, env)) ?? (ActionResult) new MoveCompleteResult(nw.row, nw.column), x))).ToList<ActionResult>();
        return aiUnit;
      }

      private void completedPositionExecuteSkillEffects(BL env, HashSet<BL.AIUnit> charmActionUnits)
      {
        HashSet<BL.ISkillEffectListUnit> deads = new HashSet<BL.ISkillEffectListUnit>();
        env.completedPositionExecuteSkillEffects((BL.UnitPosition) this, out List<List<BL.ExecuteSkillEffectResult>> _, deads);
        foreach (BL.ISkillEffectListUnit u in deads)
          this.doDead(u as BL.AIUnit, env);
        if (env.aiActionUnits.value.RemoveAll((Predicate<BL.AIUnit>) (au => au != this && au.IsCharm && !charmActionUnits.Contains(au))) < 1)
          return;
        env.aiActionUnits.commit();
      }

      private void doImmediateRebirth(BL env)
      {
        foreach (BL.AIUnit aiUnit in env.aiUnitPositions.value)
        {
          if (aiUnit.isDead)
          {
            BL.SkillEffect immediateRebirthEffect = BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) aiUnit).FirstOrDefault<BL.SkillEffect>();
            if (immediateRebirthEffect != null)
            {
              BattleFuncs.useImmediateRebirthEffect((BL.ISkillEffectListUnit) aiUnit, immediateRebirthEffect);
              bool resetCompleted = immediateRebirthEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_reset_completed) != 0;
              this.doRebirth(aiUnit, env, false, resetCompleted);
              int key = immediateRebirthEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
              if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
              {
                BattleFuncs.ApplyChangeSkillEffectsOne changeSkillEffectsOne = new BattleFuncs.ApplyChangeSkillEffectsOne((BL.UnitPosition) aiUnit, (BL.ISkillEffectListUnit) aiUnit, true);
                changeSkillEffectsOne.doBefore();
                BattleskillSkill skill = MasterData.BattleskillSkill[key];
                foreach (BattleskillEffect effect in skill.Effects)
                  aiUnit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, 1, investUnit: aiUnit.originalUnit, investTurn: env.phaseState.absoluteTurnCount), checkEnableUnit: (BL.ISkillEffectListUnit) aiUnit);
                changeSkillEffectsOne.doAfter();
              }
            }
          }
        }
      }

      private void _completeAIUnit(BL env, HashSet<BL.AIUnit> charmActionUnits)
      {
        this.mUsedMoveCost = this.mActionCount = this.mCompletedCount = 0;
        if (this.mActionResults == null)
        {
          this.mActionResults = new List<ActionResult>();
          this.mActionResults.Add((ActionResult) new MoveCompleteResult(this.mRow, this.mColumn));
        }
        else
        {
          ActionResult mActionResult = this.mActionResults[this.mActionResults.Count - 1];
          if (mActionResult.row != this.mRow || mActionResult.column != this.mColumn)
            this.mActionResults.Add((ActionResult) new MoveCompleteResult(this.mRow, this.mColumn));
          else
            mActionResult.terminate = true;
        }
        env.aiActionUnits.value.Remove(this);
        env.aiActionOrder.value.Enqueue(this);
        if (env.aiActionOrder.value.Count == env.aiActionMax)
          env.aiActionUnits.value.Clear();
        if (this.isDead)
          return;
        HashSet<BL.ISkillEffectListUnit> deads = new HashSet<BL.ISkillEffectListUnit>();
        env.completedExecuteSkillEffects((BL.UnitPosition) this, deads);
        if (this.hp <= 0)
          deads.Add((BL.ISkillEffectListUnit) this);
        foreach (BL.ISkillEffectListUnit u in deads)
          this.doDead(u as BL.AIUnit, env);
        this.completedPositionExecuteSkillEffects(env, charmActionUnits);
      }

      private void reinforcementIDsToPanel(BL env)
      {
        int[] ids = env.getFieldPanel((BL.UnitPosition) this).getReinforcementIDsToPanel(this.getForceID(env));
        if (ids == null)
          return;
        this.spawnAIUnits(env.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.canSpawnp(x.unit, env) && x.unit.playerUnit.reinforcement != null && ((IEnumerable<int>) ids).Contains<int>(x.unit.playerUnit.reinforcement.ID))), env);
      }

      public bool actionAIUnit(BL env, bool useCost = true)
      {
        if (this.mActionCount == 0 && this.mCompletedCount == 0)
          return false;
        int mOriginalRow = this.mOriginalRow;
        int mOriginalColumn = this.mOriginalColumn;
        BL.Panel fieldPanel1 = env.getFieldPanel(this.mOriginalRow, this.mOriginalColumn);
        BL.Panel fieldPanel2 = env.getFieldPanel(this.mRow, this.mColumn);
        int routeCostNonCache = !useCost || this.mCompletedCount <= 1 ? 0 : env.getRouteCostNonCache((BL.UnitPosition) this, fieldPanel2, fieldPanel1, this.movePanels, this.completePanels);
        int num1 = BL.fieldDistance(fieldPanel1, fieldPanel2);
        this.mMoveDistance += num1;
        foreach (BL.SkillEffect skillEffect1 in this.skillEffects.All())
        {
          if (skillEffect1.moveDistance.HasValue)
          {
            BL.SkillEffect skillEffect2 = skillEffect1;
            int? moveDistance = skillEffect2.moveDistance;
            int num2 = num1;
            skillEffect2.moveDistance = moveDistance.HasValue ? new int?(moveDistance.GetValueOrDefault() + num2) : new int?();
          }
          else
            skillEffect1.moveDistance = new int?(num1);
        }
        this.mOriginalRow = this.mRow;
        this.mOriginalColumn = this.mColumn;
        if (this.mOriginalRow != mOriginalRow || this.mOriginalColumn != mOriginalColumn)
        {
          foreach (BL.AIUnit aiUnit in env.aiUnitPositions.value)
          {
            if (aiUnit != this && aiUnit.hasPanelsCache && (aiUnit.movePanels.Contains(fieldPanel1) || aiUnit.movePanels.Contains(fieldPanel2) || aiUnit.allMoveActionRangePanels.Contains(fieldPanel1) || aiUnit.allMoveActionRangePanels.Contains(fieldPanel2)))
              aiUnit.clearMovePanelCache();
          }
          this.resetPanelSkillEffects(mOriginalRow, mOriginalColumn, this.mOriginalRow, this.mOriginalColumn);
          foreach (BL.ISkillEffectListUnit charismaTargetUnit in env.getCharismaTargetUnits((BL.ISkillEffectListUnit) this))
            charismaTargetUnit.skillEffects.commit();
          foreach (BL.ISkillEffectListUnit onemanChargeUnit in env.getOnemanChargeUnits((BL.ISkillEffectListUnit) this))
            onemanChargeUnit.skillEffects.commit();
          env.resetZocPanels((BL.ISkillEffectListUnit) this, mOriginalRow, mOriginalColumn, this.mOriginalRow, this.mOriginalColumn, true);
        }
        DuelResult duelResult = this.mActionResults != null ? this.mActionResults.Find((Predicate<ActionResult>) (x => !x.isCompleted && x is DuelResult)) as DuelResult : (DuelResult) null;
        int num3 = 0;
        int num4 = 0;
        if (duelResult != null)
        {
          BL.AIUnit aiUnit1 = env.getAIUnit(duelResult.attack);
          BL.AIUnit aiUnit2 = env.getAIUnit(duelResult.defense);
          num3 = aiUnit1.hp - duelResult.attackDamage;
          num4 = aiUnit2.hp - duelResult.defenseDamage;
        }
        HashSet<BL.AIUnit> charmActionUnits = new HashSet<BL.AIUnit>();
        foreach (BL.AIUnit aiUnit in env.aiActionUnits.value)
        {
          if (aiUnit.IsCharm)
            charmActionUnits.Add(aiUnit);
        }
        List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
        int num5 = 0;
        if (this.mActionResults != null)
        {
          foreach (ActionResult mActionResult in this.mActionResults)
          {
            if (!mActionResult.isCompleted)
            {
              try
              {
                num5 = this.applyActionResult(mActionResult, env, unitPositionList);
              }
              catch (Exception ex)
              {
                Debug.LogException(ex);
              }
            }
          }
        }
        foreach (BL.SkillEffect skillEffect in this.skillEffects.All())
        {
          if (!skillEffect.moveDistance.HasValue)
            skillEffect.moveDistance = new int?(0);
        }
        BL.AIUnit attack = (BL.AIUnit) null;
        BL.AIUnit defense = (BL.AIUnit) null;
        int defenseHp = 0;
        int mCompletedCount1 = this.mCompletedCount;
        int mActionCount1 = this.mActionCount;
        if (num5 == 1)
        {
          useCost = false;
          this.mUsedMoveCost += routeCostNonCache;
        }
        else
        {
          if (duelResult != null)
          {
            bool flag = duelResult.moveUnit != duelResult.attack.originalUnit;
            attack = env.getAIUnit(!flag ? duelResult.attack : duelResult.defense);
            defense = env.getAIUnit(!flag ? duelResult.defense : duelResult.attack);
            defenseHp = !flag ? num4 : num3;
          }
          if (this.mHp > 0)
          {
            Tuple<int, int> completeActionCount = BattleFuncs.getNextCompleteActionCount((BL.ISkillEffectListUnit) this, (BL.UnitPosition) this, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, defenseHp);
            this.mCompletedCount = completeActionCount.Item1;
            this.mActionCount = completeActionCount.Item2;
            if (num5 == 2)
              this.mCompletedCount = 0;
          }
        }
        if (this.mCompletedCount == 0 || this.mHp <= 0)
        {
          this._completeAIUnit(env, charmActionUnits);
          this.doImmediateRebirth(env);
          this.spawnAIUnits(unitPositionList.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.canSpawnp(x.unit, env))), env);
          this.reinforcementIDsToPanel(env);
          return true;
        }
        if (useCost)
        {
          this.mUsedMoveCost += routeCostNonCache - BattleFuncs.getRunAwayValue((BL.ISkillEffectListUnit) this);
          if (this.skillEffects.All().Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkillId == 390003074)))
          {
            this.skillEffects.RemoveEffect(0, 390003074, 0, env, (BL.ISkillEffectListUnit) this);
            this.skillEffects.RemoveEffect(0, 300001382, 0, env, (BL.ISkillEffectListUnit) this);
            this.skillEffects.RemoveEffect(0, 300001383, 0, env, (BL.ISkillEffectListUnit) this);
            this.skillEffects.RemoveEffect(0, 390003075, 0, env, (BL.ISkillEffectListUnit) this);
            this.skillEffects.RemoveEffect(0, 390003076, 0, env, (BL.ISkillEffectListUnit) this);
          }
          this.movePanels = (HashSet<BL.Panel>) null;
          this.completedPositionExecuteSkillEffects(env, charmActionUnits);
          this.doImmediateRebirth(env);
          if (this.movePanels.Count == 1 && !this.isAction || duelResult != null && !duelResult.moveUnitIsCharm && this.IsCharm)
          {
            this._completeAIUnit(env, charmActionUnits);
            this.spawnAIUnits(unitPositionList.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.canSpawnp(x.unit, env))), env);
            this.doImmediateRebirth(env);
            this.reinforcementIDsToPanel(env);
            return true;
          }
          int mCompletedCount2 = this.mCompletedCount;
          int mActionCount2 = this.mActionCount;
          this.mCompletedCount = mCompletedCount1;
          this.mActionCount = mActionCount1;
          Tuple<int, int> completeActionCount = BattleFuncs.getNextCompleteActionCount((BL.ISkillEffectListUnit) this, (BL.UnitPosition) this, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, defenseHp, true);
          this.mCompletedCount = mCompletedCount2;
          this.mActionCount = mActionCount2;
          if (completeActionCount.Item1 == 0)
          {
            this.mActionCount = this.mCompletedCount = 0;
            if (num5 == 0)
            {
              this._completeAIUnit(env, charmActionUnits);
              this.spawnAIUnits(unitPositionList.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.canSpawnp(x.unit, env))), env);
              this.doImmediateRebirth(env);
              this.reinforcementIDsToPanel(env);
              return false;
            }
          }
          this.spawnAIUnits(unitPositionList.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => this.canSpawnp(x.unit, env))), env);
        }
        else if (this.mOriginalRow != mOriginalRow || this.mOriginalColumn != mOriginalColumn)
          this.clearMovePanelCache();
        if (this.mCompletedCount > 0 && !this.cantChangeCurrent)
        {
          env.aiActionOrder.value.Enqueue(new BL.AIUnit(this));
          env.aiActionOrder.commit();
          this.mActionResults = (List<ActionResult>) null;
          if (env.aiActionOrder.value.Count == env.aiActionMax)
            env.aiActionUnits.value.Clear();
        }
        return false;
      }

      public void completeAIUnit(BL env, bool isAllComplete = false)
      {
        if (env.aiActionUnits.value.Count == 0 || this.mCompletedCount == 0)
          return;
        if (isAllComplete)
          this.mActionCount = this.mCompletedCount = 1;
        this.actionAIUnit(env);
      }

      private int applyActionResult(ActionResult ar, BL env, List<BL.UnitPosition> spawnUnits)
      {
        if (ar.isCompleted)
          return 0;
        int num = 0;
        DuelResult duelResult = ar as DuelResult;
        BL.BattleSkillResult bsr = ar as BL.BattleSkillResult;
        bool flag1 = false;
        if (duelResult != null)
        {
          BL.AIUnit aiUnit1 = env.getAIUnit(duelResult.attack);
          BL.AIUnit aiUnit2 = env.getAIUnit(duelResult.defense);
          aiUnit1.hp -= duelResult.attackDamage;
          if (!duelResult.isHeal)
            aiUnit2.hp -= duelResult.defenseDamage;
          bool flag2 = false;
          BL.Panel fieldPanel = env.getFieldPanel((BL.UnitPosition) aiUnit2);
          if (duelResult.isHeal)
          {
            Dictionary<BL.AIUnit, int> dictionary = new Dictionary<BL.AIUnit, int>();
            foreach (BL.ISkillEffectListUnit allUnit in BattleFuncs.getAllUnits(true, true, includeJumping: true))
              dictionary.Add(allUnit as BL.AIUnit, allUnit.hp);
            BL.MagicBullet magicBullet = duelResult.attackAttackStatus.magicBullet;
            BL bl = env;
            BattleskillSkill skill = magicBullet.skill;
            List<BL.ISkillEffectListUnit> targets = new List<BL.ISkillEffectListUnit>();
            targets.Add((BL.ISkillEffectListUnit) aiUnit2);
            List<BL.Panel> panels = new List<BL.Panel>();
            BL env1 = env;
            BL.AIUnit useUnit = aiUnit1;
            bool? callIsPlayer = new bool?();
            IEnumerable<BL.AIUnit> second = bl.setSkillEffect(skill, 1, targets, panels, (BL.BattleSkillResult) null, env1, (BL.ISkillEffectListUnit) useUnit, true, callIsPlayer: callIsPlayer).Item1.Select<BL.Unit, BL.AIUnit>((Func<BL.Unit, BL.AIUnit>) (x => env.getAIUnit(x)));
            if (((IEnumerable<BattleskillEffect>) magicBullet.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.power_heal)) && aiUnit2.hp >= 1)
              aiUnit2.hp += BattleFuncs.getHealValue((BL.ISkillEffectListUnit) aiUnit2, fieldPanel, -duelResult.defenseDamage, magicBullet.skill.skill_type);
            List<BL.AIUnit> list = new List<BL.AIUnit>()
            {
              aiUnit2
            }.Concat<BL.AIUnit>(second).Distinct<BL.AIUnit>().ToList<BL.AIUnit>();
            int healHpTotal = 0;
            foreach (BL.AIUnit aiUnit3 in list)
            {
              if (dictionary.ContainsKey(aiUnit3) && aiUnit3.hp > dictionary[aiUnit3])
                healHpTotal += aiUnit3.hp - dictionary[aiUnit3];
              if (aiUnit3.hp <= 0 && aiUnit3 != aiUnit1 && aiUnit3 != aiUnit2)
              {
                this.doDead(aiUnit3, env);
                flag2 = true;
              }
            }
            if (aiUnit1.hp >= 1)
              BattleFuncs.applyServantsJoy((BL.ISkillEffectListUnit) aiUnit1, healHpTotal);
          }
          BattleFuncs.applyDuelResultEffects(duelResult, (BL.ISkillEffectListUnit) aiUnit1, (BL.ISkillEffectListUnit) aiUnit2, env);
          Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects = new Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>>();
          foreach (BL.ISkillEffectListUnit applyDuelSkillEffect in BattleFuncs.applyDuelSkillEffects(duelResult, (BL.ISkillEffectListUnit) aiUnit1, (BL.ISkillEffectListUnit) aiUnit2, env, removeSkillEffects))
          {
            this.doDead(applyDuelSkillEffect as BL.AIUnit, env);
            flag2 = true;
          }
          BattleFuncs.applyDuelResultEffectsLate(duelResult, (BL.ISkillEffectListUnit) aiUnit1, (BL.ISkillEffectListUnit) aiUnit2, env, removeSkillEffects);
          if (aiUnit1.hp <= 0)
          {
            this.doDead(aiUnit1, env);
            flag2 = true;
            if (!duelResult.isHeal)
              aiUnit2.skillEffects.AddKillCount(1);
          }
          if (aiUnit2.hp <= 0)
          {
            this.doDead(aiUnit2, env);
            flag2 = true;
            if (!duelResult.isHeal)
              aiUnit1.skillEffects.AddKillCount(1);
          }
          flag1 = fieldPanel.id != env.getFieldPanel((BL.UnitPosition) aiUnit2).id;
          spawnUnits.AddRange(env.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.playerUnit.reinforcement != null && x.unit.playerUnit.reinforcement.isSpawnForBattle(duelResult.attack, duelResult.moveUnit == duelResult.attack ? duelResult.defense : duelResult.attack))));
          if (flag2)
          {
            foreach (BL.AIUnit aiUnit4 in env.aiUnitPositions.value)
            {
              if (aiUnit4.hasPanelsCache)
                aiUnit4.clearMovePanelCache();
            }
            spawnUnits.AddRange(env.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => env.isReinforceUnitForSmash(x.unit.playerUnit, true))));
          }
        }
        else if (bsr != null)
        {
          BL.AIUnit aiUnit = env.getAIUnit(bsr.invocation);
          BL.Skill skill = aiUnit.getSkill(bsr.skill.id);
          List<BL.ISkillEffectListUnit> list = bsr.targets.Select<BL.Unit, BL.ISkillEffectListUnit>((Func<BL.Unit, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) env.getAIUnit(x))).ToList<BL.ISkillEffectListUnit>();
          Tuple<int, List<BL.ISkillEffectListUnit>> tuple = env.useSkillCore((BL.ISkillEffectListUnit) aiUnit, skill, list, bsr.panels, bsr, env, random: bsr.random);
          num = tuple.Item1;
          if (tuple.Item2 != null)
          {
            foreach (BL.ISkillEffectListUnit up in tuple.Item2)
            {
              HashSet<BL.ISkillEffectListUnit> deads = new HashSet<BL.ISkillEffectListUnit>();
              env.completedPositionExecuteSkillEffects(up as BL.UnitPosition, out List<List<BL.ExecuteSkillEffectResult>> _, deads);
              foreach (BL.ISkillEffectListUnit u in deads)
                this.doDead(u as BL.AIUnit, env);
            }
          }
        }
        ar.isCompleted = true;
        if (flag1)
        {
          foreach (BL.UnitPosition unitPosition in env.aiUnitPositions.value)
            unitPosition.clearMovePanelCache();
        }
        return num;
      }

      private void doDead(BL.AIUnit u, BL env)
      {
        if (u.isDead)
          return;
        u.isDead = true;
        if (env.aiActionUnits.value.Contains(u))
        {
          env.aiActionUnits.value.Remove(u);
          env.aiActionUnits.commit();
        }
        BattleFuncs.removeStealEffects((BL.ISkillEffectListUnit) u);
        BattleFuncs.removeJumpEffects((BL.ISkillEffectListUnit) u);
        ++u.deadCount;
        foreach (BL.AIUnit unit in env.aiUnitPositions.value)
        {
          if (BattleFuncs.hasEnabledDeadCountEffects((BL.ISkillEffectListUnit) unit, (BL.ISkillEffectListUnit) u))
            unit.skillEffects.commit();
        }
        env.removeZocPanels((BL.ISkillEffectListUnit) u, u.originalRow, u.originalColumn, true);
        u.removePanelSkillEffects();
      }

      private void doRebirth(BL.AIUnit u, BL env, bool resetHp = true, bool resetCompleted = true)
      {
        u.resetSpawnPosition(isAI: true);
        if (resetHp && this.hp != this.unit.parameter.Hp)
          this.hp = this.unit.parameter.Hp;
        u.isDead = false;
        if (!env.aiUnitPositions.value.Contains(u))
        {
          env.aiUnitPositions.value.Add(u);
          env.aiUnitPositions.commit();
        }
        u.skillEffects.RemoveAilmentEffect(env, (BL.ISkillEffectListUnit) u);
        u.skillEffects.SetKillCount(0);
        u.skillEffects.RecoveryRemovedSkillEffects((BL.ISkillEffectListUnit) u);
        env.addZocPanels((BL.ISkillEffectListUnit) u, u.originalRow, u.originalColumn, true);
        u.addPanelSkillEffects();
        foreach (BL.AIUnit aiUnit in env.aiUnitPositions.value)
        {
          if (aiUnit.hasPanelsCache)
            aiUnit.clearMovePanelCache();
        }
        if (!resetCompleted || env.aiUnits.value.Count < 1 || env.aiUnits.value[0].getForceID(env) != u.getForceID(env) || env.aiUnits.value[0].mIsCharmStart)
          return;
        if (!env.aiUnits.value.Contains(u))
        {
          env.aiUnits.value.Add(u);
          env.aiUnits.commit();
        }
        u.resetOriginalPosition(env);
        if (env.aiActionUnits.value.Contains(u))
          return;
        BL.AIUnit[] array = env.aiActionOrder.value.ToArray();
        env.aiActionOrder.value.Clear();
        foreach (BL.AIUnit aiUnit1 in array)
        {
          if (aiUnit1 == u)
          {
            BL.AIUnit aiUnit2 = new BL.AIUnit(u);
            List<ActionResult> actionResults = aiUnit2.actionResults;
            if (actionResults != null && actionResults.Count >= 1 && aiUnit1 == array[array.Length - 1])
              actionResults[actionResults.Count - 1].terminate = false;
            env.aiActionOrder.value.Enqueue(aiUnit2);
          }
          else
            env.aiActionOrder.value.Enqueue(aiUnit1);
        }
        env.aiActionOrder.commit();
        u.actionResults = (List<ActionResult>) null;
        if (env.aiActionOrder.value.Count == env.aiActionMax)
          return;
        env.aiActionUnits.value.Add(u);
        env.aiActionUnits.commit();
      }

      private bool canSpawnp(BL.Unit unit, BL env)
      {
        if (env.aiUnits.value.Any<BL.AIUnit>((Func<BL.AIUnit, bool>) (u => u.unit == unit)))
          return false;
        BL.AIUnit aiUnit = env.getAIUnit(unit);
        if (unit.isDead || unit.isSpawned)
          return false;
        return aiUnit == null || !aiUnit.isDead;
      }

      private void spawnAIUnits(IEnumerable<BL.UnitPosition> units, BL env)
      {
        bool flag = false;
        foreach (BL.UnitPosition unit in units)
        {
          BL.AIUnit u = new BL.AIUnit(unit, BL.AIType.normal);
          u.resetSpawnPosition(isAI: true);
          env.aiUnitPositions.value.Add(u);
          env.aiUnitPositions.commit();
          env.aiUnits.value.Add(u);
          env.aiUnits.commit();
          flag = true;
          env.addZocPanels((BL.ISkillEffectListUnit) u, u.originalRow, u.originalColumn, true);
          u.addPanelSkillEffects();
        }
        if (!flag)
          return;
        foreach (BL.AIUnit aiUnit in env.aiUnitPositions.value)
        {
          if (aiUnit.hasPanelsCache)
            aiUnit.clearMovePanelCache();
        }
      }

      public int skillLevel
      {
        get
        {
          if (this.mSkillLevel == 0)
          {
            foreach (BL.Skill duelSkill in this.mUnit.duelSkills)
            {
              if (this.mSkillLevel < duelSkill.level)
                this.mSkillLevel = duelSkill.level;
            }
          }
          return this.mSkillLevel;
        }
      }

      public int rarity => this.mUnit.unit.rarity.index + 1;

      public override void respawnReinforcement(BL bl) => this.isDead = false;
    }

    public class BattleCallSkillResultNetwork
    {
      public int? mSkillID;
      public List<int> mTargets;
    }

    [Serializable]
    public class BattleCallSkillResult
    {
      [SerializeField]
      private BL.Skill mSkill;
      [SerializeField]
      private List<BL.Unit> mTargets;

      public BL.BattleCallSkillResultNetwork ToNetwork(BL env)
      {
        List<int> intList = new List<int>();
        if (this.mTargets != null)
        {
          foreach (BL.Unit mTarget in this.mTargets)
          {
            int? network = mTarget.ToNetwork(env);
            if (network.HasValue)
              intList.Add(network.Value);
          }
        }
        return new BL.BattleCallSkillResultNetwork()
        {
          mSkillID = new int?(this.mSkill.id),
          mTargets = intList
        };
      }

      public static BL.BattleCallSkillResult FromNetwork(
        BL.BattleCallSkillResultNetwork nnw,
        BL env,
        bool isPlayer)
      {
        BL.BattleCallSkillResultNetwork skillResultNetwork = nnw;
        if (skillResultNetwork == null)
          return (BL.BattleCallSkillResult) null;
        int num = (isPlayer ? (env.playerCallSkillState.isUsedCallSkill ? 1 : 0) : (env.enemyCallSkillState.isUsedCallSkill ? 1 : 0)) != 0 ? 0 : 1;
        BL.Skill skill = new BL.Skill()
        {
          id = skillResultNetwork.mSkillID.Value,
          remain = new int?(num)
        };
        return new BL.BattleCallSkillResult()
        {
          mSkill = skill,
          mTargets = skillResultNetwork.mTargets.Select<int, BL.Unit>((Func<int, BL.Unit>) (t => BL.Unit.FromNetwork(new int?(t), env))).ToList<BL.Unit>()
        };
      }

      public BattleCallSkillResult()
      {
        this.mSkill = (BL.Skill) null;
        this.mTargets = (List<BL.Unit>) null;
      }

      public static BL.BattleCallSkillResult createBattleCallSkillResult(
        int skill_id,
        List<BL.Unit> targets,
        BL env,
        bool isPlayer)
      {
        BL.BattleCallSkillResult battleCallSkillResult = (BL.BattleCallSkillResult) null;
        int num1 = isPlayer ? env.playerCallSkillState.skillId : env.enemyCallSkillState.skillId;
        if (skill_id != 0 && skill_id == num1)
        {
          int num2 = (isPlayer ? (env.playerCallSkillState.isUsedCallSkill ? 1 : 0) : (env.enemyCallSkillState.isUsedCallSkill ? 1 : 0)) != 0 ? 0 : 1;
          if (num2 >= 1)
            battleCallSkillResult = new BL.BattleCallSkillResult()
            {
              mSkill = new BL.Skill()
              {
                id = num1,
                remain = new int?(num2)
              },
              mTargets = targets
            };
        }
        return battleCallSkillResult;
      }

      public BL.Skill skill => this.mSkill;

      public List<BL.Unit> targets => this.mTargets;
    }

    public enum ConditionType
    {
      alldown,
      bossdown,
      area,
    }

    public enum GameoverType
    {
      alldown,
      guestdown,
      playerdown,
    }

    [Serializable]
    public class Condition : BL.ModelBase
    {
      [SerializeField]
      private int mId;
      [NonSerialized]
      private BattleVictoryCondition mCondition;

      public int id
      {
        get => this.mId;
        set
        {
          this.mId = value;
          this.mCondition = (BattleVictoryCondition) null;
          ++this.revision;
        }
      }

      public BattleVictoryCondition condition
      {
        get
        {
          if (this.mCondition == null)
            this.mCondition = MasterData.BattleVictoryCondition[this.id];
          return this.mCondition;
        }
      }

      public BL.ConditionType type
      {
        get
        {
          if (this.isExistWinAreaCondition)
            return BL.ConditionType.area;
          return this.condition.enemy != null ? BL.ConditionType.bossdown : BL.ConditionType.alldown;
        }
      }

      public bool isTurn => this.condition.turn.HasValue;

      public bool isElapsedTurn => this.condition.elapsed_turn.HasValue;

      public int turn => this.condition.turn.Value;

      public int elapsedTurn => this.condition.elapsed_turn.Value;

      public int bossId => this.condition.enemy.ID;

      public bool isExistWinAreaCondition => this.condition.win_area_confition_group_id.HasValue;

      public BattleVictoryAreaCondition[] winAreaConditoin
      {
        get
        {
          return this.condition.win_area_confition_group_id.HasValue ? ((IEnumerable<BattleVictoryAreaCondition>) MasterData.BattleVictoryAreaConditionList).Where<BattleVictoryAreaCondition>((Func<BattleVictoryAreaCondition, bool>) (x =>
          {
            int groupId = x.group_id;
            int? confitionGroupId = this.condition.win_area_confition_group_id;
            int valueOrDefault = confitionGroupId.GetValueOrDefault();
            return groupId == valueOrDefault & confitionGroupId.HasValue;
          })).ToArray<BattleVictoryAreaCondition>() : new BattleVictoryAreaCondition[0];
        }
      }

      public bool isExistLoseAreaCondition => this.condition.lose_area_confition_group_id.HasValue;

      public BattleVictoryAreaCondition[] loseAreaConditoin
      {
        get
        {
          return this.condition.lose_area_confition_group_id.HasValue ? ((IEnumerable<BattleVictoryAreaCondition>) MasterData.BattleVictoryAreaConditionList).Where<BattleVictoryAreaCondition>((Func<BattleVictoryAreaCondition, bool>) (x =>
          {
            int groupId = x.group_id;
            int? confitionGroupId = this.condition.lose_area_confition_group_id;
            int valueOrDefault = confitionGroupId.GetValueOrDefault();
            return groupId == valueOrDefault & confitionGroupId.HasValue;
          })).ToArray<BattleVictoryAreaCondition>() : new BattleVictoryAreaCondition[0];
        }
      }
    }

    [Serializable]
    public class DropData : BL.ModelBase
    {
      [SerializeField]
      private Reward mReward;
      [SerializeField]
      private bool mIsCompleted;
      [SerializeField]
      private int mExecuteUnitId;
      [NonSerialized]
      private int mRarity = -1;

      public Reward reward
      {
        get => this.mReward;
        set
        {
          this.mReward = value;
          ++this.revision;
        }
      }

      public bool isCompleted => this.mIsCompleted;

      public int executeUnitId => this.mExecuteUnitId;

      public bool isDropBox
      {
        get
        {
          switch (this.mReward.Type)
          {
            case MasterDataTable.CommonRewardType.unit:
            case MasterDataTable.CommonRewardType.supply:
            case MasterDataTable.CommonRewardType.gear:
            case MasterDataTable.CommonRewardType.money:
            case MasterDataTable.CommonRewardType.quest_key:
            case MasterDataTable.CommonRewardType.gacha_ticket:
            case MasterDataTable.CommonRewardType.material_unit:
            case MasterDataTable.CommonRewardType.material_gear:
            case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
            case MasterDataTable.CommonRewardType.gear_body:
              return true;
            default:
              return false;
          }
        }
      }

      public int rarity
      {
        get
        {
          if (this.mRarity != -1)
            return this.mRarity;
          switch (this.mReward.Type)
          {
            case MasterDataTable.CommonRewardType.unit:
            case MasterDataTable.CommonRewardType.material_unit:
              this.mRarity = MasterData.UnitUnit[this.mReward.Id].rarity.index;
              break;
            case MasterDataTable.CommonRewardType.supply:
              this.mRarity = MasterData.SupplySupply[this.mReward.Id].rarity.index;
              break;
            case MasterDataTable.CommonRewardType.gear:
            case MasterDataTable.CommonRewardType.material_gear:
            case MasterDataTable.CommonRewardType.gear_body:
              this.mRarity = MasterData.GearGear[this.mReward.Id].rarity.index;
              break;
            case MasterDataTable.CommonRewardType.quest_key:
              this.mRarity = 3;
              break;
            case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
              this.mRarity = 4;
              break;
            default:
              this.mRarity = 0;
              break;
          }
          return this.mRarity;
        }
      }

      public void execute(BL.Unit unit, BL env)
      {
        if (this.mIsCompleted)
          return;
        switch (this.reward.Type)
        {
          case MasterDataTable.CommonRewardType.unit:
          case MasterDataTable.CommonRewardType.material_unit:
            ++env.dropUnit.value;
            break;
          case MasterDataTable.CommonRewardType.supply:
            ++env.dropItem.value;
            break;
          case MasterDataTable.CommonRewardType.gear:
          case MasterDataTable.CommonRewardType.material_gear:
          case MasterDataTable.CommonRewardType.gear_body:
            ++env.dropItem.value;
            break;
          case MasterDataTable.CommonRewardType.money:
            env.dropMoney.value += (long) this.reward.Quantity;
            break;
          case MasterDataTable.CommonRewardType.quest_key:
          case MasterDataTable.CommonRewardType.gacha_ticket:
          case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
            ++env.dropItem.value;
            break;
        }
        if (unit != (BL.Unit) null)
          this.mExecuteUnitId = unit.playerUnit.id;
        this.mIsCompleted = true;
        ++this.revision;
      }
    }

    [Serializable]
    public class Duel : BL.ModelBase
    {
      public DuelResult result;
      public BL.MagicBullet attackBullet;
      public BL.MagicBullet defenseBullet;
    }

    [Serializable]
    public class SuiseiResult
    {
      public int damage;
      public int dispDamage;
      public int realDamage;
      public int drainDamage;
      public int dispDrainDamage;
      public int defenderDispDrainDamage;
      public int defenderDrainDamage;
      public bool isHit;
      public bool isCritical;
      public BL.Skill[] invokeDuelSkills;
      public BL.Skill[] invokeDefenderDuelSkills;
      public List<string> invokeSkillExtraInfo;
      public int attackerSwapHealDamage;
      public int defenderSwapHealDamage;
    }

    [Serializable]
    public class UseSkillEffect
    {
      public int effectEffectId;
      public int effectBaseSkillLevel;
      public int effectTurnRemain;
      public int effectUnitIndex;
      public bool effectUnitIsPlayerControl;
      public float effectWork;
      public int effectUniqueId;
      public BL.UseSkillEffect.Type type;
      public float work;

      public static BL.UseSkillEffect Create(
        BL.SkillEffect effect,
        BL.UseSkillEffect.Type type = BL.UseSkillEffect.Type.None,
        float work = 0.0f)
      {
        return new BL.UseSkillEffect()
        {
          effectEffectId = effect.effectId,
          effectBaseSkillLevel = effect.baseSkillLevel,
          effectTurnRemain = !effect.turnRemain.HasValue ? -1 : effect.turnRemain.Value,
          effectUnitIndex = effect.unit == (BL.Unit) null ? -1 : effect.unit.index,
          effectUnitIsPlayerControl = !(effect.unit == (BL.Unit) null) && effect.unit.isPlayerForce,
          effectWork = effect.work == null ? 0.0f : effect.work[0],
          effectUniqueId = effect.uniqueId,
          type = type,
          work = work
        };
      }

      public bool GetEffectUnitIsPlayerControl(bool isColosseum)
      {
        if (!isColosseum)
        {
          NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
          if (instance.isPvp && instance.order == 1)
            return !this.effectUnitIsPlayerControl;
        }
        return this.effectUnitIsPlayerControl;
      }

      public enum Type
      {
        None,
        Remove,
        SetWork,
        Decrement,
      }
    }

    [Serializable]
    public class DuelTurnNetwork
    {
      public int damage;
      public int dispDamage;
      public int realDamage;
      public int drainDamage;
      public int dispDrainDamage;
      public int counterDamage;
      public int defenderDispDrainDamage;
      public int defenderDrainDamage;
      public int attackerRestHp;
      public int defenderRestHp;
      public AttackStatus attackStatus;
      public AttackStatus attackerStatus;
      public AttackStatus defenderStatus;
      public bool isAtackker;
      public bool isHit;
      public bool isCritical;
      public int[] skillIds;
      public BL.Skill[] invokeDuelSkills;
      public BL.Skill[] invokeDefenderDuelSkills;
      public BL.Skill[] invokeAilmentSkills;
      public BL.Skill[] invokeGiveSkills;
      public int[] investUnit;
      public int[] investSkillIds;
      public int[] investFrom;
      public int[] investFromSkillIds;
      public List<BL.SuiseiResult> suiseiResults;
      public BattleskillAilmentEffect[] ailmentEffects;
      public BattleskillAilmentEffect[] attackerAilmentEffects;
      public List<int> invokeAttackerDuelSkillEffectIds;
      public List<int> invokeDefenderDuelSkillEffectIds;
      public List<string> invokeSkillExtraInfo;
      public int[] damageShareUnit;
      public List<int> damageShareDamage;
      public List<BL.UseSkillEffect> damageShareSkillEffect;
      public List<BL.UseSkillEffect> attackerUseSkillEffects;
      public List<BL.UseSkillEffect> defenderUseSkillEffects;
      public int[] attackerCombiUnit;
      public int attackCount;
      public bool isDualSingleAttack;
      public int[] useSkillUnit;
      public List<BL.UseSkillEffect> useSkillEffect;
      public int attackerSwapHealDamage;
      public int defenderSwapHealDamage;
    }

    [Serializable]
    public class DuelTurn
    {
      public int damage;
      public int dispDamage;
      public int realDamage;
      public int drainDamage;
      public int dispDrainDamage;
      public int counterDamage;
      public int defenderDispDrainDamage;
      public int defenderDrainDamage;
      public int attackerRestHp;
      public int defenderRestHp;
      public AttackStatus attackStatus;
      public AttackStatus attackerStatus;
      public AttackStatus defenderStatus;
      public bool isAtackker;
      public bool isHit;
      public bool isCritical;
      public int[] skillIds;
      public BL.Skill[] invokeDuelSkills;
      public BL.Skill[] invokeDefenderDuelSkills;
      public BL.Skill[] invokeAilmentSkills;
      public BL.Skill[] invokeGiveSkills;
      public BL.ISkillEffectListUnit[] investUnit;
      public int[] investSkillIds;
      public BL.ISkillEffectListUnit[] investFrom;
      public int[] investFromSkillIds;
      public List<BL.SuiseiResult> suiseiResults;
      public BattleskillAilmentEffect[] ailmentEffects;
      public BattleskillAilmentEffect[] attackerAilmentEffects;
      public List<int> invokeAttackerDuelSkillEffectIds;
      public List<int> invokeDefenderDuelSkillEffectIds;
      public List<string> invokeSkillExtraInfo;
      public List<BL.ISkillEffectListUnit> damageShareUnit;
      public List<int> damageShareDamage;
      public List<BL.UseSkillEffect> damageShareSkillEffect;
      public List<BL.UseSkillEffect> attackerUseSkillEffects;
      public List<BL.UseSkillEffect> defenderUseSkillEffects;
      public BL.ISkillEffectListUnit[] attackerCombiUnit;
      public int attackCount;
      public bool isDualSingleAttack;
      public List<BL.ISkillEffectListUnit> useSkillUnit;
      public List<BL.UseSkillEffect> useSkillEffect;
      public int attackerSwapHealDamage;
      public int defenderSwapHealDamage;

      public BL.DuelTurnNetwork ToNetwork(BL env)
      {
        return new BL.DuelTurnNetwork()
        {
          damage = this.damage,
          dispDamage = this.dispDamage,
          realDamage = this.realDamage,
          drainDamage = this.drainDamage,
          dispDrainDamage = this.dispDrainDamage,
          counterDamage = this.counterDamage,
          defenderDispDrainDamage = this.defenderDispDrainDamage,
          defenderDrainDamage = this.defenderDrainDamage,
          attackerRestHp = this.attackerRestHp,
          defenderRestHp = this.defenderRestHp,
          attackStatus = this.attackStatus,
          attackerStatus = this.attackerStatus,
          defenderStatus = this.defenderStatus,
          isAtackker = this.isAtackker,
          isHit = this.isHit,
          isCritical = this.isCritical,
          skillIds = this.skillIds,
          invokeDuelSkills = this.invokeDuelSkills,
          invokeDefenderDuelSkills = this.invokeDefenderDuelSkills,
          invokeAilmentSkills = this.invokeAilmentSkills,
          invokeGiveSkills = this.invokeGiveSkills,
          investUnit = this.investUnit == null ? (int[]) null : ((IEnumerable<BL.ISkillEffectListUnit>) this.investUnit).Select<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.originalUnit.ToNetwork(env) ?? -1)).ToArray<int>(),
          investSkillIds = this.investSkillIds,
          investFrom = this.investFrom == null ? (int[]) null : ((IEnumerable<BL.ISkillEffectListUnit>) this.investFrom).Select<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.originalUnit.ToNetwork(env) ?? -1)).ToArray<int>(),
          investFromSkillIds = this.investFromSkillIds,
          suiseiResults = this.suiseiResults,
          ailmentEffects = this.ailmentEffects,
          attackerAilmentEffects = this.attackerAilmentEffects,
          invokeAttackerDuelSkillEffectIds = this.invokeAttackerDuelSkillEffectIds,
          invokeDefenderDuelSkillEffectIds = this.invokeDefenderDuelSkillEffectIds,
          invokeSkillExtraInfo = this.invokeSkillExtraInfo,
          damageShareUnit = this.damageShareUnit == null ? (int[]) null : this.damageShareUnit.Select<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.originalUnit.ToNetwork(env) ?? -1)).ToArray<int>(),
          damageShareDamage = this.damageShareDamage,
          damageShareSkillEffect = this.damageShareSkillEffect,
          attackerUseSkillEffects = this.attackerUseSkillEffects,
          defenderUseSkillEffects = this.defenderUseSkillEffects,
          attackerCombiUnit = this.attackerCombiUnit == null ? (int[]) null : ((IEnumerable<BL.ISkillEffectListUnit>) this.attackerCombiUnit).Select<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.originalUnit.ToNetwork(env) ?? -1)).ToArray<int>(),
          attackCount = this.attackCount,
          isDualSingleAttack = this.isDualSingleAttack,
          useSkillUnit = this.useSkillUnit == null ? (int[]) null : this.useSkillUnit.Select<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x => x.originalUnit.ToNetwork(env) ?? -1)).ToArray<int>(),
          useSkillEffect = this.useSkillEffect,
          attackerSwapHealDamage = this.attackerSwapHealDamage,
          defenderSwapHealDamage = this.defenderSwapHealDamage
        };
      }

      public static BL.DuelTurn FromNetwork(BL.DuelTurnNetwork nw, BL env)
      {
        if (nw == null)
          return (BL.DuelTurn) null;
        return new BL.DuelTurn()
        {
          damage = nw.damage,
          dispDamage = nw.dispDamage,
          realDamage = nw.realDamage,
          drainDamage = nw.drainDamage,
          dispDrainDamage = nw.dispDrainDamage,
          counterDamage = nw.counterDamage,
          defenderDispDrainDamage = nw.defenderDispDrainDamage,
          defenderDrainDamage = nw.defenderDrainDamage,
          attackerRestHp = nw.attackerRestHp,
          defenderRestHp = nw.defenderRestHp,
          attackStatus = nw.attackStatus,
          attackerStatus = nw.attackerStatus,
          defenderStatus = nw.defenderStatus,
          isAtackker = nw.isAtackker,
          isHit = nw.isHit,
          isCritical = nw.isCritical,
          skillIds = nw.skillIds,
          invokeDuelSkills = nw.invokeDuelSkills,
          invokeDefenderDuelSkills = nw.invokeDefenderDuelSkills,
          invokeAilmentSkills = nw.invokeAilmentSkills,
          invokeGiveSkills = nw.invokeGiveSkills,
          investUnit = nw.investUnit == null ? (BL.ISkillEffectListUnit[]) null : (BL.ISkillEffectListUnit[]) ((IEnumerable<int>) nw.investUnit).Select<int, BL.Unit>((Func<int, BL.Unit>) (x => BL.UnitPosition.FromNetwork(new int?(x), env).unit)).ToArray<BL.Unit>(),
          investSkillIds = nw.investSkillIds,
          investFrom = nw.investFrom == null ? (BL.ISkillEffectListUnit[]) null : (BL.ISkillEffectListUnit[]) ((IEnumerable<int>) nw.investFrom).Select<int, BL.Unit>((Func<int, BL.Unit>) (x => BL.UnitPosition.FromNetwork(new int?(x), env).unit)).ToArray<BL.Unit>(),
          investFromSkillIds = nw.investFromSkillIds,
          suiseiResults = nw.suiseiResults,
          ailmentEffects = nw.ailmentEffects,
          attackerAilmentEffects = nw.attackerAilmentEffects,
          invokeAttackerDuelSkillEffectIds = nw.invokeAttackerDuelSkillEffectIds,
          invokeDefenderDuelSkillEffectIds = nw.invokeDefenderDuelSkillEffectIds,
          invokeSkillExtraInfo = nw.invokeSkillExtraInfo,
          damageShareUnit = nw.damageShareUnit == null ? (List<BL.ISkillEffectListUnit>) null : ((IEnumerable<int>) nw.damageShareUnit).Select<int, BL.ISkillEffectListUnit>((Func<int, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) BL.UnitPosition.FromNetwork(new int?(x), env).unit)).ToList<BL.ISkillEffectListUnit>(),
          damageShareDamage = nw.damageShareDamage,
          damageShareSkillEffect = nw.damageShareSkillEffect,
          attackerUseSkillEffects = nw.attackerUseSkillEffects,
          defenderUseSkillEffects = nw.defenderUseSkillEffects,
          attackerCombiUnit = nw.attackerCombiUnit == null ? (BL.ISkillEffectListUnit[]) null : ((IEnumerable<int>) nw.attackerCombiUnit).Select<int, BL.ISkillEffectListUnit>((Func<int, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) BL.UnitPosition.FromNetwork(new int?(x), env).unit)).ToArray<BL.ISkillEffectListUnit>(),
          attackCount = nw.attackCount,
          isDualSingleAttack = nw.isDualSingleAttack,
          useSkillUnit = nw.useSkillUnit == null ? (List<BL.ISkillEffectListUnit>) null : ((IEnumerable<int>) nw.useSkillUnit).Select<int, BL.ISkillEffectListUnit>((Func<int, BL.ISkillEffectListUnit>) (x => (BL.ISkillEffectListUnit) BL.UnitPosition.FromNetwork(new int?(x), env).unit)).ToList<BL.ISkillEffectListUnit>(),
          useSkillEffect = nw.useSkillEffect,
          attackerSwapHealDamage = nw.attackerSwapHealDamage,
          defenderSwapHealDamage = nw.defenderSwapHealDamage
        };
      }

      public BattleskillSkill[] skills
      {
        get
        {
          return ((IEnumerable<int>) this.skillIds).Select<int, BattleskillSkill>((Func<int, BattleskillSkill>) (x => MasterData.BattleskillSkill[x])).ToArray<BattleskillSkill>();
        }
      }

      public bool isDieAttackerOrDefender() => this.isDieAttacker() || this.isDieDefender();

      public bool isDieAttacker() => this.attackerRestHp <= 0;

      public bool isDieDefender() => this.defenderRestHp <= 0;
    }

    public enum PanelAttribute
    {
      clear = 0,
      playermove = 1,
      neutralmove = 2,
      enemymove = 4,
      danger = 8,
      moving = 16, // 0x00000010
      target_attack = 32, // 0x00000020
      target_heal = 64, // 0x00000040
      attack_range = 128, // 0x00000080
      heal_range = 256, // 0x00000100
      reserve0 = 512, // 0x00000200
      test = 4096, // 0x00001000
    }

    public enum PanelVictoryConditionAtrribute
    {
      none,
      win_player_on_panel,
      lose_enemy_on_panel,
    }

    public enum PanelReinforcementConditionAtrribute
    {
      none,
      player_on_panel,
      enemy_on_panel,
    }

    [Serializable]
    public class Panel : BL.ModelBase
    {
      [SerializeField]
      private int mLandformId;
      [SerializeField]
      private int mRow;
      [SerializeField]
      private int mColumn;
      [SerializeField]
      private int mIndex;
      [NonSerialized]
      private BL.PanelAttribute mAttribute;
      [SerializeField]
      private BL.DropData mFieldEvent;
      [SerializeField]
      private int mFieldEventId;
      [SerializeField]
      private BL.PanelVictoryConditionAtrribute mConditionAttribute;
      [SerializeField]
      private int[] reinforcementIDs;
      [SerializeField]
      private BL.PanelReinforcementConditionAtrribute mReinforcementConditionAttribute;
      [SerializeField]
      private List<BL.Unit> zocUnits = new List<BL.Unit>();
      [SerializeField]
      private List<BL.Unit> zocAIUnits = new List<BL.Unit>();
      [SerializeField]
      private BL.ClassValue<List<BL.SkillEffect>> skillEffects = new BL.ClassValue<List<BL.SkillEffect>>(new List<BL.SkillEffect>());
      [SerializeField]
      private BL.ClassValue<List<BL.SkillEffect>> skillEffectsAI = new BL.ClassValue<List<BL.SkillEffect>>(new List<BL.SkillEffect>());
      [SerializeField]
      private bool mIsJumping;
      [NonSerialized]
      private Dictionary<BL.Unit, Tuple<int, int>> mEffectsAddRangeCache;
      [NonSerialized]
      public int workMovement;

      public Panel(
        int index,
        int row,
        int column,
        int landformId,
        int fieldEventId,
        BL.DropData fieldEvent,
        BattleVictoryAreaCondition[] winArea,
        BattleVictoryAreaCondition[] loseArea,
        BattleReinforcement[] battleReinforcements)
      {
        this.mIndex = index;
        this.mRow = row;
        this.mColumn = column;
        this.mLandformId = landformId;
        this.mFieldEventId = fieldEventId;
        this.mFieldEvent = fieldEvent;
        this.mConditionAttribute = BL.PanelVictoryConditionAtrribute.none;
        if (winArea != null && ((IEnumerable<BattleVictoryAreaCondition>) winArea).Any<BattleVictoryAreaCondition>((Func<BattleVictoryAreaCondition, bool>) (x => x.area_y - 1 == this.mRow && x.area_x - 1 == this.mColumn)))
          this.mConditionAttribute |= BL.PanelVictoryConditionAtrribute.win_player_on_panel;
        if (loseArea != null && ((IEnumerable<BattleVictoryAreaCondition>) loseArea).Any<BattleVictoryAreaCondition>((Func<BattleVictoryAreaCondition, bool>) (x => x.area_y - 1 == this.mRow && x.area_x - 1 == this.mColumn)))
          this.mConditionAttribute |= BL.PanelVictoryConditionAtrribute.lose_enemy_on_panel;
        List<int> intList = new List<int>();
        if (battleReinforcements != null)
        {
          foreach (BattleReinforcement battleReinforcement in battleReinforcements)
          {
            if (battleReinforcement.area != null && ((IEnumerable<BattleVictoryAreaCondition>) battleReinforcement.area).Any<BattleVictoryAreaCondition>((Func<BattleVictoryAreaCondition, bool>) (x => x.area_y - 1 == this.mRow && x.area_x - 1 == this.mColumn)))
            {
              if (battleReinforcement.reinforcement_logic.Enum == BattleReinforcementLogicEnum.player_area_invasion)
              {
                this.mReinforcementConditionAttribute |= BL.PanelReinforcementConditionAtrribute.player_on_panel;
                intList.Add(battleReinforcement.ID);
              }
              else if (battleReinforcement.reinforcement_logic.Enum == BattleReinforcementLogicEnum.enemy_area_invasion)
              {
                this.mReinforcementConditionAttribute |= BL.PanelReinforcementConditionAtrribute.enemy_on_panel;
                intList.Add(battleReinforcement.ID);
              }
            }
          }
          this.reinforcementIDs = intList.ToArray();
        }
        ++this.revision;
      }

      public BattleLandform landform => MasterData.BattleLandform[this.mLandformId];

      public int landformID => this.mLandformId;

      public int row
      {
        get => this.mRow;
        set
        {
          this.mRow = value;
          ++this.revision;
        }
      }

      public int column
      {
        get => this.mColumn;
        set
        {
          this.mColumn = value;
          ++this.revision;
        }
      }

      public BL.PanelAttribute attribute
      {
        get => this.mAttribute;
        set
        {
          if (this.mAttribute == value)
            return;
          this.mAttribute = value;
          ++this.revision;
        }
      }

      public void setAttribute(BL.PanelAttribute attr)
      {
        if ((this.mAttribute & attr) != BL.PanelAttribute.clear)
          return;
        this.mAttribute |= attr;
        ++this.revision;
      }

      public void unsetAttribute(BL.PanelAttribute attr)
      {
        if ((this.mAttribute & attr) == BL.PanelAttribute.clear)
          return;
        this.mAttribute &= ~attr;
        ++this.revision;
      }

      public bool checkAttribute(BL.PanelAttribute attr) => (this.mAttribute & attr) == attr;

      public void clearAttribute()
      {
        if (this.mAttribute == BL.PanelAttribute.clear)
          return;
        this.mAttribute = BL.PanelAttribute.clear;
        ++this.revision;
      }

      public bool checkVictoryConditionAtrribute(BL.PanelVictoryConditionAtrribute attr)
      {
        return (this.mConditionAttribute & attr) == attr;
      }

      public BL.Phase getChangePhaseToPanel(BL.ForceID forceID)
      {
        switch (forceID)
        {
          case BL.ForceID.player:
            if ((this.mConditionAttribute & BL.PanelVictoryConditionAtrribute.win_player_on_panel) == BL.PanelVictoryConditionAtrribute.win_player_on_panel)
              return BL.Phase.stageclear;
            break;
          case BL.ForceID.enemy:
            if ((this.mConditionAttribute & BL.PanelVictoryConditionAtrribute.lose_enemy_on_panel) == BL.PanelVictoryConditionAtrribute.lose_enemy_on_panel)
              return BL.Phase.gameover;
            break;
        }
        return BL.Phase.none;
      }

      public bool checkReinforcementConditionAtrribute(BL.PanelReinforcementConditionAtrribute attr)
      {
        return (this.mReinforcementConditionAttribute & attr) == attr;
      }

      public int[] getReinforcementIDsToPanel(BL.ForceID forceID)
      {
        int[] reinforcementIdsToPanel = (int[]) null;
        switch (forceID)
        {
          case BL.ForceID.player:
            if ((this.mReinforcementConditionAttribute & BL.PanelReinforcementConditionAtrribute.player_on_panel) == BL.PanelReinforcementConditionAtrribute.player_on_panel)
            {
              reinforcementIdsToPanel = this.reinforcementIDs;
              break;
            }
            break;
          case BL.ForceID.enemy:
            if ((this.mReinforcementConditionAttribute & BL.PanelReinforcementConditionAtrribute.enemy_on_panel) == BL.PanelReinforcementConditionAtrribute.enemy_on_panel)
            {
              reinforcementIdsToPanel = this.reinforcementIDs;
              break;
            }
            break;
        }
        return reinforcementIdsToPanel;
      }

      public int fieldEventId
      {
        get => this.mFieldEventId;
        set
        {
          this.mFieldEventId = value;
          ++this.revision;
        }
      }

      public BL.DropData fieldEvent
      {
        get => this.mFieldEvent;
        set
        {
          this.mFieldEvent = value;
          ++this.revision;
        }
      }

      public bool hasEvent => this.mFieldEvent != null && !this.mFieldEvent.isCompleted;

      public void executeEvent(BL.Unit unit, BL env)
      {
        if (!this.hasEvent)
          return;
        this.mFieldEvent.execute(unit, env);
        ++this.revision;
      }

      private Tuple<int, int> getEffectsAddRange(int gear_kind_id, UnitMoveType moveType)
      {
        BattleLandformEffect battleLandformEffect = this.landform.GetIncr(moveType).GetLandformEffects(BattleLandformEffectPhase.move).FirstOrDefault<BattleLandformEffect>((Func<BattleLandformEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.fix_range && x.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == gear_kind_id));
        return battleLandformEffect != null ? new Tuple<int, int>(battleLandformEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_add), battleLandformEffect.GetInt(BattleskillEffectLogicArgumentEnum.max_add)) : new Tuple<int, int>(0, 0);
      }

      public Tuple<int, int> getEffectsAddRange(BL.Unit unit)
      {
        Tuple<int, int> effectsAddRange;
        if (this.mEffectsAddRangeCache == null)
        {
          this.mEffectsAddRangeCache = new Dictionary<BL.Unit, Tuple<int, int>>();
          effectsAddRange = this.getEffectsAddRange(unit.unit.kind.ID, unit.job.move_type);
          this.mEffectsAddRangeCache[unit] = effectsAddRange;
        }
        else if (!this.mEffectsAddRangeCache.TryGetValue(unit, out effectsAddRange))
        {
          effectsAddRange = this.getEffectsAddRange(unit.unit.kind.ID, unit.job.move_type);
          this.mEffectsAddRangeCache[unit] = effectsAddRange;
        }
        return effectsAddRange;
      }

      public string ShowPosition() => "(" + (object) this.row + ", " + (object) this.column + ")";

      public override string ToString() => "[" + this.ShowPosition() + "]";

      public int id => this.mRow * 100000 + this.mColumn;

      public int index => this.mIndex;

      public bool zocCheckp(BL.Unit unit, bool isAI, BL env)
      {
        List<BL.Unit> unitList = isAI ? this.zocAIUnits : this.zocUnits;
        if (unitList.Count > 0)
        {
          BL.ForceID[] targetForce = env.getTargetForce(unit, false);
          foreach (BL.Unit unit1 in unitList)
          {
            if (((IEnumerable<BL.ForceID>) targetForce).Contains<BL.ForceID>(env.getForceID(unit1)))
            {
              if (isAI)
              {
                BL.AIUnit aiUnit = env.getAIUnit(unit1);
                if (aiUnit != null && !aiUnit.IsJumping)
                  return true;
              }
              else if (!unit1.IsJumping)
                return true;
            }
          }
        }
        return false;
      }

      public void addZocUnit(BL.Unit unit, bool isAI)
      {
        if (!this.zocAIUnits.Contains(unit))
          this.zocAIUnits.Add(unit);
        if (isAI || this.zocUnits.Contains(unit))
          return;
        this.zocUnits.Add(unit);
      }

      public void removeZocUnit(BL.Unit unit, bool isAI)
      {
        this.zocAIUnits.Remove(unit);
        if (isAI)
          return;
        this.zocUnits.Remove(unit);
      }

      public BL.ClassValue<List<BL.SkillEffect>> getSkillEffects(bool isAI = false)
      {
        return !isAI ? this.skillEffects : this.skillEffectsAI;
      }

      private int skillEffectCompare(BL.SkillEffect se0, BL.SkillEffect se1)
      {
        return se0 == null ? (se1 == null ? 0 : -1) : (se1 == null ? 1 : se0.effectId - se1.effectId);
      }

      public void addSkillEffect(BL.SkillEffect se, BL.ISkillEffectListUnit u, bool noCommit = false)
      {
        int num = u is BL.AIUnit ? 1 : 0;
        if (se.parentUnit == (BL.Unit) null)
          se.parentUnit = u.originalUnit;
        if (num != 0 && !this.skillEffectsAI.value.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.uniqueId == se.uniqueId && x.parentUnit == se.parentUnit)))
        {
          this.skillEffectsAI.value.Add(se);
          this.skillEffectsAI.value.Sort(new Comparison<BL.SkillEffect>(this.skillEffectCompare));
          if (!noCommit)
            this.skillEffectsAI.commit();
        }
        if (num != 0 || this.skillEffects.value.Contains(se))
          return;
        this.skillEffects.value.Add(se);
        this.skillEffects.value.Sort(new Comparison<BL.SkillEffect>(this.skillEffectCompare));
        if (noCommit)
          return;
        this.skillEffects.commit();
      }

      public void removeSkillEffect(BL.ISkillEffectListUnit u, BL.SkillEffect se, bool noCommit = false)
      {
        bool isAI = u is BL.AIUnit;
        BL.ClassValue<List<BL.SkillEffect>> skillEffects = this.getSkillEffects(isAI);
        List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect skillEffect in skillEffects.value)
        {
          if (skillEffect.uniqueId == se.uniqueId && skillEffect.parentUnit == u.originalUnit)
            skillEffectList.Add(skillEffect);
        }
        foreach (BL.SkillEffect skillEffect in skillEffectList)
        {
          if (isAI)
          {
            if (this.skillEffectsAI.value.Contains(skillEffect))
            {
              this.skillEffectsAI.value.Remove(skillEffect);
              if (!noCommit)
                this.skillEffectsAI.commit();
            }
          }
          else if (this.skillEffects.value.Contains(skillEffect))
          {
            this.skillEffects.value.Remove(skillEffect);
            if (!noCommit)
              this.skillEffects.commit();
          }
        }
      }

      public void commitSkillEffect(bool isAI = false)
      {
        if (isAI)
          this.skillEffectsAI.commit();
        else
          this.skillEffects.commit();
      }

      public bool isJumping
      {
        get => this.mIsJumping;
        set
        {
          this.mIsJumping = value;
          ++this.revision;
        }
      }

      public void turnStart()
      {
        List<BL.SkillEffect> source = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect skillEffect1 in this.skillEffects.value)
        {
          if (!BattleFuncs.isCharismaEffect(skillEffect1.effect.EffectLogic.Enum))
          {
            int? nullable1 = skillEffect1.turnRemain;
            if (nullable1.HasValue)
            {
              BL.SkillEffect skillEffect2 = skillEffect1;
              int? turnRemain = skillEffect2.turnRemain;
              int? nullable2 = turnRemain.HasValue ? new int?(turnRemain.GetValueOrDefault() - 1) : new int?();
              skillEffect2.turnRemain = nullable2;
              nullable1 = nullable2;
              int num = 0;
              if (nullable1.GetValueOrDefault() <= num & nullable1.HasValue)
                source.Add(skillEffect1);
            }
          }
        }
        if (!source.Any<BL.SkillEffect>())
          return;
        foreach (BL.SkillEffect skillEffect in source)
          this.skillEffects.value.Remove(skillEffect);
        this.skillEffects.commit();
      }

      public bool hasLandTag(bool isAI, params int[] tags)
      {
        return this.getSkillEffects(isAI).value.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          return effect.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag && ((IEnumerable<int>) tags).Contains<int>(effect.GetInt(BattleskillEffectLogicArgumentEnum.land_tag));
        }));
      }
    }

    [Serializable]
    public class UnitPosition : BL.ModelBase
    {
      [SerializeField]
      protected int mId;
      [SerializeField]
      protected BL.Unit mUnit;
      [SerializeField]
      protected int mRow;
      [SerializeField]
      protected int mColumn;
      [SerializeField]
      protected float mDirection;
      [SerializeField]
      protected int mOriginalRow = -100;
      [SerializeField]
      protected int mOriginalColumn = -100;
      [SerializeField]
      protected int mUsedMoveCost;
      [SerializeField]
      protected int mCompletedCount;
      [SerializeField]
      protected int mActionCount;
      [SerializeField]
      protected int mMaxCompletedCount;
      [SerializeField]
      protected int mMaxActionCount;
      [SerializeField]
      protected int mCantChangeCurrentActionCount;
      [SerializeField]
      protected bool mDontUseSkillAgain;
      [SerializeField]
      protected int mMoveDistance;
      [SerializeField]
      protected List<int> mScriptList;
      [NonSerialized]
      protected HashSet<BL.Panel> mMovePanels;
      [NonSerialized]
      protected HashSet<BL.Panel> mCompletePanels;
      [NonSerialized]
      protected HashSet<BL.Panel> mAllMoveActionRangePanels;
      [NonSerialized]
      protected HashSet<BL.Panel> mAllMoveHealRangePanels;
      [NonSerialized]
      protected Dictionary<int, HashSet<BL.Panel>> mAllMoveSkillRangePanelsDic;
      [NonSerialized]
      protected HashSet<BL.Panel> mActionMovePanels;
      [NonSerialized]
      protected HashSet<BL.Panel> mHealMovePanels;
      [NonSerialized]
      protected Dictionary<int, HashSet<BL.Panel>> mSkillMovePanelsDic;
      [NonSerialized]
      public BattleFuncs.AsterNode[][] asterNodeCache = new BattleFuncs.AsterNode[2][];
      private static int moveLimiter = 1000;

      public int? ToNetwork(BL env) => new int?(this.mId);

      public static BL.UnitPosition FromNetwork(int? nw, BL env)
      {
        return nw.HasValue ? env.getUnitPositionById(nw.Value) : (BL.UnitPosition) null;
      }

      public int id
      {
        get => this.mId;
        set
        {
          this.mId = value;
          ++this.revision;
        }
      }

      public BL.Unit unit
      {
        get => this.mUnit;
        set
        {
          if (!(this.mUnit != value))
            return;
          this.mUnit = value;
          this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mCompletePanels = this.mMovePanels = (HashSet<BL.Panel>) null;
          this.mAllMoveSkillRangePanelsDic = (Dictionary<int, HashSet<BL.Panel>>) null;
          ++this.revision;
        }
      }

      public int row
      {
        get => this.mRow;
        set
        {
          if (this.mRow == value)
            return;
          this.mRow = value;
          ++this.revision;
        }
      }

      public int column
      {
        get => this.mColumn;
        set
        {
          if (this.mColumn == value)
            return;
          this.mColumn = value;
          ++this.revision;
        }
      }

      public float direction
      {
        get => this.mDirection;
        set
        {
          if ((double) this.mDirection == (double) value)
            return;
          this.mDirection = value;
          ++this.revision;
        }
      }

      public override string ToString()
      {
        return "[" + this.mUnit.unit.name + " (" + (object) this.mRow + ", " + (object) this.mColumn + ") direction:" + (object) this.mDirection + "]";
      }

      public int originalRow => this.mOriginalRow;

      public int originalColumn => this.mOriginalColumn;

      public int usedMoveCost
      {
        get => this.mUsedMoveCost;
        set => this.mUsedMoveCost = value;
      }

      public int completedCount => this.mCompletedCount;

      public int actionCount => this.mActionCount;

      public int maxCompletedCount => this.mMaxCompletedCount;

      public int maxActionCount => this.mMaxActionCount;

      public int cantChangeCurrentActionCount => this.mCantChangeCurrentActionCount;

      public bool dontUseSkillAgain => this.mDontUseSkillAgain;

      public int moveDistance
      {
        get => this.mMoveDistance;
        set => this.mMoveDistance = value;
      }

      public bool cantChangeCurrent
      {
        get
        {
          if (this.mUnit == (BL.Unit) null)
            return false;
          BL.ISkillEffectListUnit skillEffectListUnit = this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit;
          if (skillEffectListUnit.hp >= 1 && skillEffectListUnit.CantChangeCurrent)
            return true;
          return this.cantChangeCurrentActionCount == 0 && this.mCompletedCount != 0 && this.mCompletedCount != this.mMaxCompletedCount && this.mActionCount != this.mMaxActionCount;
        }
      }

      public void resetOriginalPosition(
        BL env,
        bool noCountReset = false,
        bool resetOriginalToOuterPosition = false)
      {
        if (resetOriginalToOuterPosition)
        {
          this.mOriginalRow = -100;
          this.mOriginalColumn = -100;
        }
        int r0 = -100;
        int c0 = -100;
        if (this.mOriginalRow != this.mRow || this.mOriginalColumn != this.mColumn)
        {
          r0 = this.mOriginalRow;
          c0 = this.mOriginalColumn;
          this.mOriginalRow = this.mRow;
          this.mOriginalColumn = this.mColumn;
        }
        this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mCompletePanels = this.mMovePanels = (HashSet<BL.Panel>) null;
        this.mAllMoveSkillRangePanelsDic = (Dictionary<int, HashSet<BL.Panel>>) null;
        if (!noCountReset)
        {
          this.mUsedMoveCost = 0;
          this.mCompletedCount = this.unit.skillEffects.GetCompleteCount();
          this.mActionCount = this.unit.skillEffects.GetActionCount();
          this.mMaxCompletedCount = this.mCompletedCount;
          this.mMaxActionCount = this.mActionCount;
          this.mCantChangeCurrentActionCount = this.unit.skillEffects.GetCantChageCurrentActionCount();
          this.mDontUseSkillAgain = this.unit.skillEffects.GetCantUseSkillAgain() == 1;
          BL.ISkillEffectListUnit skillEffectListUnit = this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit;
          foreach (BL.SkillEffect againEffect in skillEffectListUnit.skillEffects.GetAgainEffects())
            againEffect.againInvoked = false;
          this.mMoveDistance = 0;
          foreach (BL.SkillEffect skillEffect in skillEffectListUnit.skillEffects.All())
            skillEffect.moveDistance = new int?(0);
        }
        if (this.mUnit.isEnable && !this.mUnit.isDead)
        {
          env.resetZocPanels((BL.ISkillEffectListUnit) this.mUnit, r0, c0, this.mOriginalRow, this.mOriginalColumn, this is BL.AIUnit);
          this.resetPanelSkillEffects(r0, c0, this.mOriginalRow, this.mOriginalColumn);
        }
        ++this.revision;
      }

      public bool isLocalMoved
      {
        get => this.mOriginalRow != this.mRow || this.mOriginalColumn != this.mColumn;
      }

      public HashSet<BL.Panel> movePanels
      {
        get
        {
          if (this.mMovePanels == null)
          {
            BL.ISkillEffectListUnit skillEffectListUnit = this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit;
            this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mCompletePanels = (HashSet<BL.Panel>) null;
            this.mAllMoveSkillRangePanelsDic = (Dictionary<int, HashSet<BL.Panel>>) null;
            if (skillEffectListUnit == null)
            {
              this.mMovePanels = new HashSet<BL.Panel>();
            }
            else
            {
              if (skillEffectListUnit.IsDontMove)
                return new HashSet<BL.Panel>()
                {
                  BattleFuncs.getPanel(this.mOriginalRow, this.mOriginalColumn)
                };
              this.mMovePanels = BattleFuncs.createMovePanels(this.mOriginalRow, this.mOriginalColumn, this.moveCost, this.mUnit, isAI: this is BL.AIUnit);
            }
          }
          return this.mMovePanels;
        }
        set
        {
          this.mMovePanels = value;
          this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mCompletePanels = (HashSet<BL.Panel>) null;
          this.mAllMoveSkillRangePanelsDic = (Dictionary<int, HashSet<BL.Panel>>) null;
          ++this.revision;
        }
      }

      public bool hasPanelsCache => this.mMovePanels != null;

      public HashSet<BL.Panel> completePanels
      {
        get
        {
          if (this.mCompletePanels == null || this.mMovePanels == null)
            this.mCompletePanels = BattleFuncs.moveCompletePanels_(this.movePanels, this.mUnit, this is BL.AIUnit);
          return this.mCompletePanels;
        }
      }

      protected HashSet<BL.Panel> getAllMoveActionRangePanels()
      {
        if ((this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit).IsDontAction)
          return new HashSet<BL.Panel>();
        if (this.mAllMoveActionRangePanels == null)
          this.mAllMoveActionRangePanels = BattleFuncs.allMoveActionRangePanels_(this, isAI: this is BL.AIUnit, positionPanels: this.mActionMovePanels);
        return this.mAllMoveActionRangePanels;
      }

      public HashSet<BL.Panel> allMoveActionRangePanels => this.getAllMoveActionRangePanels();

      protected HashSet<BL.Panel> getAllMoveHealRangePanels()
      {
        if ((this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit).IsDontAction)
          return new HashSet<BL.Panel>();
        if (this.mAllMoveHealRangePanels == null)
          this.mAllMoveHealRangePanels = BattleFuncs.allMoveActionRangePanels_(this, isAI: this is BL.AIUnit, isHeal: true, positionPanels: this.mHealMovePanels);
        return this.mAllMoveHealRangePanels;
      }

      public HashSet<BL.Panel> allMoveHealRangePanels => this.getAllMoveHealRangePanels();

      public HashSet<BL.Panel> getAllMoveSkillRangePanels(BL.Skill skill)
      {
        if ((this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit).IsDontAction)
          return new HashSet<BL.Panel>();
        if (this.mAllMoveSkillRangePanelsDic == null)
          this.mAllMoveSkillRangePanelsDic = new Dictionary<int, HashSet<BL.Panel>>();
        HashSet<BL.Panel> skillRangePanels;
        if (!this.mAllMoveSkillRangePanelsDic.TryGetValue(skill.id, out skillRangePanels))
        {
          HashSet<BL.Panel> panelSet1 = (HashSet<BL.Panel>) null;
          if (this.mSkillMovePanelsDic != null)
            this.mSkillMovePanelsDic[skill.id] = panelSet1 = new HashSet<BL.Panel>();
          Dictionary<int, HashSet<BL.Panel>> skillRangePanelsDic = this.mAllMoveSkillRangePanelsDic;
          int id = skill.id;
          BL.Skill skill1 = skill;
          int num = this is BL.AIUnit ? 1 : 0;
          BL.Skill skill2 = skill1;
          HashSet<BL.Panel> positionPanels = panelSet1;
          HashSet<BL.Panel> panelSet2;
          skillRangePanels = panelSet2 = BattleFuncs.allMoveActionRangePanels_(this, isAI: num != 0, skill: skill2, positionPanels: positionPanels);
          skillRangePanelsDic[id] = panelSet2;
        }
        return skillRangePanels;
      }

      public void clearMovePanelCache()
      {
        this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mCompletePanels = this.mMovePanels = (HashSet<BL.Panel>) null;
        this.mAllMoveSkillRangePanelsDic = (Dictionary<int, HashSet<BL.Panel>>) null;
        ++this.revision;
      }

      public void clearMoveActionRangePanelCache()
      {
        this.mAllMoveActionRangePanels = (HashSet<BL.Panel>) null;
        ++this.revision;
      }

      public void clearMoveHealRangePanelCache()
      {
        this.mAllMoveHealRangePanels = (HashSet<BL.Panel>) null;
        ++this.revision;
      }

      public bool isCompleted => this.mCompletedCount == 0;

      public bool isActionComleted => this.mActionCount == 0;

      public int moveCost
      {
        get
        {
          return BattleFuncs.unitPositionToISkillEffectListUnit(this).parameter.Move - this.mUsedMoveCost;
        }
      }

      private void _completeActionUnit(BL env)
      {
        BL.ClassValue<List<BL.UnitPosition>> actionUnits = env.getActionUnits(this);
        if (actionUnits != null)
        {
          actionUnits.value.Remove(this);
          actionUnits.commit();
        }
        env.completedActionUnits.value.Add(this);
        env.completedActionUnits.commit();
        this.mUnit.mIsExecCompletedSkillEffect = false;
        this.mUsedMoveCost = this.mActionCount = this.mCompletedCount = 0;
      }

      public void recoveryCompleteUnit()
      {
        this.mUsedMoveCost = this.mActionCount = this.mCompletedCount = 0;
      }

      public void actionActionUnit(
        BL env,
        bool useCost = true,
        BL.Unit attack = null,
        BL.Unit defense = null,
        int defenseHp = 0,
        bool isKilledByPanelLandformEffect = false)
      {
        if (this.mActionCount == 0 && this.mCompletedCount == 0)
          return;
        Tuple<int, int> completeActionCount = BattleFuncs.getNextCompleteActionCount((BL.ISkillEffectListUnit) this.mUnit, this, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, defenseHp);
        this.mCompletedCount = completeActionCount.Item1;
        this.mActionCount = completeActionCount.Item2;
        BL.Panel fieldPanel1 = env.getFieldPanel(this.mOriginalRow, this.mOriginalColumn);
        BL.Panel fieldPanel2 = env.getFieldPanel(this.mRow, this.mColumn);
        int routeCostNonCache = !useCost || this.mCompletedCount <= 0 ? 0 : env.getRouteCostNonCache(this, fieldPanel2, fieldPanel1, this.movePanels, this.completePanels);
        int num1 = BL.fieldDistance(fieldPanel1, fieldPanel2);
        this.mMoveDistance += num1;
        foreach (BL.SkillEffect skillEffect1 in this.mUnit.skillEffects.All())
        {
          int? moveDistance = skillEffect1.moveDistance;
          if (moveDistance.HasValue)
          {
            BL.SkillEffect skillEffect2 = skillEffect1;
            moveDistance = skillEffect2.moveDistance;
            int num2 = num1;
            skillEffect2.moveDistance = moveDistance.HasValue ? new int?(moveDistance.GetValueOrDefault() + num2) : new int?();
          }
          else
            skillEffect1.moveDistance = new int?(0);
        }
        int mOriginalRow = this.mOriginalRow;
        int mOriginalColumn = this.mOriginalColumn;
        this.mOriginalRow = this.mRow;
        this.mOriginalColumn = this.mColumn;
        if (this.mOriginalRow != mOriginalRow || this.mOriginalColumn != mOriginalColumn)
        {
          foreach (BL.UnitPosition unitPosition in env.unitPositions.value)
          {
            if (unitPosition != this && unitPosition.hasPanelsCache && (unitPosition.movePanels.Contains(fieldPanel1) || unitPosition.movePanels.Contains(fieldPanel2) || unitPosition.allMoveActionRangePanels.Contains(fieldPanel1) || unitPosition.allMoveActionRangePanels.Contains(fieldPanel2)))
              unitPosition.clearMovePanelCache();
          }
          if (this.mUnit.isEnable && !this.mUnit.isDead)
            this.resetPanelSkillEffects(mOriginalRow, mOriginalColumn, this.mOriginalRow, this.mOriginalColumn);
          foreach (BL.ISkillEffectListUnit charismaTargetUnit in env.getCharismaTargetUnits((BL.ISkillEffectListUnit) this.mUnit))
            charismaTargetUnit.skillEffects.commit();
          foreach (BL.ISkillEffectListUnit onemanChargeUnit in env.getOnemanChargeUnits((BL.ISkillEffectListUnit) this.mUnit))
            onemanChargeUnit.skillEffects.commit();
          if (this.mUnit.isEnable && !this.mUnit.isDead)
            env.resetZocPanels((BL.ISkillEffectListUnit) this.mUnit, mOriginalRow, mOriginalColumn, this.mOriginalRow, this.mOriginalColumn);
        }
        if (this.mCompletedCount == 0)
          this._completeActionUnit(env);
        else if (useCost)
        {
          this.mUsedMoveCost += routeCostNonCache - BattleFuncs.getRunAwayValue((BL.ISkillEffectListUnit) this.mUnit);
          if (this.mUnit.skillEffects.All().Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkillId == 300001381)))
          {
            this.mUnit.skillEffects.RemoveEffect(0, 300001381, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 300001382, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 300001383, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
          }
          if (this.mUnit.skillEffects.All().Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkillId == 300004267)))
          {
            this.mUnit.skillEffects.RemoveEffect(0, 300004267, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 300001382, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 300001383, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
          }
          if (this.mUnit.skillEffects.All().Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkillId == 390003074)))
          {
            this.mUnit.skillEffects.RemoveEffect(0, 390003074, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 300001382, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 300001383, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 390003075, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
            this.mUnit.skillEffects.RemoveEffect(0, 390003076, 0, env, (BL.ISkillEffectListUnit) this.mUnit);
          }
          this.movePanels = (HashSet<BL.Panel>) null;
        }
        else if (this.mOriginalRow != mOriginalRow || this.mOriginalColumn != mOriginalColumn)
          this.clearMovePanelCache();
        if (!isKilledByPanelLandformEffect && this.mUnit != (BL.Unit) null && env.unitCurrent.unit == this.mUnit)
          env.firstCompleted.value = true;
        ++this.revision;
        env.createDangerAria();
      }

      public void completeActionUnit(
        BL env,
        bool isAllComplete = false,
        bool isKilledByPanelLandformEffect = false)
      {
        if (this.mCompletedCount == 0)
          return;
        if (isAllComplete)
          this.mActionCount = this.mCompletedCount = 1;
        this.actionActionUnit(env, isKilledByPanelLandformEffect: isKilledByPanelLandformEffect);
        env.battleLogger.Wait(this);
      }

      private List<BL.Panel> getCharismaPanels(
        int row,
        int column,
        int[] range,
        BL.SkillEffect se)
      {
        List<BL.Panel> rangePanels = BattleFuncs.getRangePanels(row, column, range);
        return !se.effect.HasKey(BattleskillEffectLogicArgumentEnum.excluding_slanting) || se.effect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) == 0 ? rangePanels : rangePanels.Where<BL.Panel>((Func<BL.Panel, bool>) (x => x.row == row || x.column == column)).ToList<BL.Panel>();
      }

      protected void resetPanelSkillEffects(int r0, int c0, int r1, int c1)
      {
        BL.ISkillEffectListUnit skillEffectListUnit = this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit;
        foreach (BL.SkillEffect enabledCharismaEffect in BattleFuncs.getEnabledCharismaEffects(skillEffectListUnit))
        {
          int[] range = new int[2]
          {
            enabledCharismaEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
            enabledCharismaEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
          };
          List<BL.Panel> charismaPanels1 = this.getCharismaPanels(r0, c0, range, enabledCharismaEffect);
          List<BL.Panel> charismaPanels2 = this.getCharismaPanels(r1, c1, range, enabledCharismaEffect);
          foreach (BL.Panel panel in charismaPanels1.Except<BL.Panel>((IEnumerable<BL.Panel>) charismaPanels2))
            panel.removeSkillEffect(skillEffectListUnit, enabledCharismaEffect);
          foreach (BL.Panel panel in charismaPanels2.Except<BL.Panel>((IEnumerable<BL.Panel>) charismaPanels1))
            panel.addSkillEffect(enabledCharismaEffect, skillEffectListUnit);
        }
      }

      public void removePanelSkillEffects(bool noCommit = false)
      {
        BL.ISkillEffectListUnit skillEffectListUnit = this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit;
        foreach (BL.SkillEffect enabledCharismaEffect in BattleFuncs.getEnabledCharismaEffects(skillEffectListUnit))
        {
          foreach (BL.Panel charismaPanel in this.getCharismaPanels(this.mOriginalRow, this.mOriginalColumn, new int[2]
          {
            enabledCharismaEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
            enabledCharismaEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
          }, enabledCharismaEffect))
            charismaPanel.removeSkillEffect(skillEffectListUnit, enabledCharismaEffect, noCommit);
        }
      }

      public void addPanelSkillEffects(bool noCommit = false)
      {
        BL.ISkillEffectListUnit skillEffectListUnit = this is BL.AIUnit ? this as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) this.mUnit;
        if (skillEffectListUnit.hp <= 0)
          return;
        foreach (BL.SkillEffect enabledCharismaEffect in BattleFuncs.getEnabledCharismaEffects(skillEffectListUnit))
        {
          foreach (BL.Panel charismaPanel in this.getCharismaPanels(this.mOriginalRow, this.mOriginalColumn, new int[2]
          {
            enabledCharismaEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
            enabledCharismaEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
          }, enabledCharismaEffect))
            charismaPanel.addSkillEffect(enabledCharismaEffect, skillEffectListUnit, noCommit);
        }
      }

      public void commitPanelSkillEffects(IEnumerable<BL.SkillEffect> effects)
      {
        if (!(this is BL.AIUnit))
        {
          BL.Unit mUnit = this.mUnit;
        }
        foreach (BL.SkillEffect effect in effects)
        {
          foreach (BL.Panel charismaPanel in this.getCharismaPanels(this.mOriginalRow, this.mOriginalColumn, new int[2]
          {
            effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
            effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
          }, effect))
            charismaPanel.commitSkillEffect(this is BL.AIUnit);
        }
      }

      public void resetSpawnPosition(BL env = null, bool isAI = false, bool resetDangerAria = false)
      {
        if (this.mUnit.isEnable && !this.mUnit.isDead)
          return;
        int moveCost = this.moveCost;
        List<BL.Panel> list;
        do
        {
          list = BattleFuncs.moveCompletePanels_(BattleFuncs.createMovePanels(this.mOriginalRow, this.mOriginalColumn, moveCost, this.mUnit, isAI: isAI, isRebirth: true), this.mUnit, isAI, false).ToList<BL.Panel>();
          moveCost += 5;
        }
        while (moveCost < BL.UnitPosition.moveLimiter && list.Count == 0);
        if (list.Count == 0)
          return;
        list.Sort((Comparison<BL.Panel>) ((a, b) => BL.fieldDistance(a, this) - BL.fieldDistance(b, this)));
        this.mOriginalRow = this.mRow = list[0].row;
        this.mOriginalColumn = this.mColumn = list[0].column;
        this.mAllMoveHealRangePanels = this.mAllMoveActionRangePanels = this.mCompletePanels = this.mMovePanels = (HashSet<BL.Panel>) null;
        this.mAllMoveSkillRangePanelsDic = (Dictionary<int, HashSet<BL.Panel>>) null;
        if (resetDangerAria)
          env.createDangerAria();
        ++this.revision;
      }

      public void setScript(int id)
      {
        if (id == 0)
          return;
        if (this.mScriptList == null)
          this.mScriptList = new List<int>();
        this.mScriptList.Add(id);
      }

      public List<int> getScripts() => this.mScriptList;

      public void resetScript() => this.mScriptList = (List<int>) null;

      public virtual void respawnReinforcement(BL bl)
      {
        if (this.unit.isDead)
          this.unit.rebirth(bl);
        if (!this.unit.isEnable)
        {
          this.unit.isSpawned = true;
          this.unit.isEnable = this.unit.isSpawned;
        }
        this.unit.initReinforcement();
        this.unit.playerUnit.spawn_turn = bl.phaseState.turnCount;
        this._completeActionUnit(bl);
      }
    }

    [Serializable]
    public class Stage : BL.ModelBase
    {
      [SerializeField]
      private int mId;

      public Stage(int id) => this.mId = id;

      public int id => this.mId;

      public BattleStage stage => MasterData.BattleStage[this.mId];

      public bool IsStage() => MasterData.BattleStage.ContainsKey(this.mId);

      public int mapId => this.stage.map.ID;

      public int mapOffsetRow => this.stage.map_offset_y;

      public int mapOffsetColumn => this.stage.map_offset_x;
    }

    public enum FieldEffectType
    {
      battle_start,
      first_turn_start,
      turn_start,
      player_start,
      neutral_start,
      enemy_start,
      stageclear,
      pvp_change_player,
      pvp_change_enemy,
      waveclear,
    }

    [Serializable]
    public class FieldEffect : BL.ModelBase
    {
      [SerializeField]
      private int mEffectId;
      [SerializeField]
      private BL.FieldEffectType mType;

      public FieldEffect(int id, BL.FieldEffectType type)
      {
        this.mEffectId = id;
        this.mType = type;
        ++this.revision;
      }

      public BL.FieldEffectType type => this.mType;

      public BattleFieldEffect fieldEffect => MasterData.BattleFieldEffect[this.mEffectId];
    }

    [Serializable]
    public class Intimate : BL.ModelBase
    {
      public Dictionary<Tuple<BL.ForceID, int, int>, int> intimateDic = new Dictionary<Tuple<BL.ForceID, int, int>, int>();

      public void add(BL.ForceID force, BL.Unit self, BL.Unit[] targetUnits, int value)
      {
        if (self.is_helper || self.playerUnit.is_guest)
          return;
        BL.Unit[] array = ((IEnumerable<BL.Unit>) targetUnits).Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.is_helper && !x.playerUnit.is_guest)).ToArray<BL.Unit>();
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        foreach (BL.Unit unit in array)
        {
          int id1 = self.unit.character.ID;
          int id2 = unit.unit.character.ID;
          if (id1 != id2)
          {
            if (!dictionary.ContainsKey(id2))
              dictionary.Add(id2, 0);
            value -= dictionary[id2];
            ++dictionary[id2];
            if (value < 0)
              value = 0;
            if (self.hp <= 0)
              value *= -1;
            Tuple<BL.ForceID, int, int> key = id1 < id2 ? Tuple.Create<BL.ForceID, int, int>(force, id1, id2) : Tuple.Create<BL.ForceID, int, int>(force, id2, id1);
            if (!this.intimateDic.ContainsKey(key))
              this.intimateDic.Add(key, 0);
            this.intimateDic[key] += value;
          }
        }
      }
    }

    [Serializable]
    public class Item : BL.ModelBase
    {
      [SerializeField]
      private int mPlayerItemId;
      [SerializeField]
      private int mItemId;
      [SerializeField]
      private int mAmount;
      [SerializeField]
      private int mInitialAmount;

      public int playerItemId
      {
        get => this.mPlayerItemId;
        set
        {
          this.mPlayerItemId = value;
          ++this.revision;
        }
      }

      public int itemId
      {
        get => this.mItemId;
        set
        {
          this.mItemId = value;
          ++this.revision;
        }
      }

      public SupplySupply item => MasterData.SupplySupply[this.mItemId];

      public int amount
      {
        get => this.mAmount;
        set
        {
          this.mAmount = value;
          ++this.revision;
        }
      }

      public int initialAmount
      {
        get => this.mInitialAmount;
        set
        {
          this.mInitialAmount = value;
          ++this.revision;
        }
      }
    }

    [Serializable]
    public class BattleLogger
    {
      private StringBuilder mBuilder = new StringBuilder();
      private BL mEnv;

      public BattleLogger(BL env) => this.mEnv = env;

      public void Random()
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        this.mBuilder.AppendFormat("[R,{0},{1},{2},{3}", (object) this.mEnv.random.x, (object) this.mEnv.random.y, (object) this.mEnv.random.z, (object) this.mEnv.random.w);
      }

      public void TurnStart()
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        this.mBuilder.AppendFormat("[T,{0}", (object) this.mEnv.phaseState.turnCount);
      }

      public void PlayerPhaseStart()
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        this.mBuilder.Append("[TP");
      }

      public void EnemyPhaseStart()
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        this.mBuilder.Append("[TE");
      }

      public void Wait(BL.UnitPosition pos)
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        if (!pos.unit.isDead)
          this.mBuilder.AppendFormat("[W,{0},{1},{2},{3}", (object) pos.unit.specificId, (object) pos.unit.unitId, (object) pos.row, (object) pos.column);
        else
          this.mBuilder.AppendFormat("[DE,{0},{1},{2},{3}", (object) pos.unit.specificId, (object) pos.unit.unitId, (object) pos.row, (object) pos.column);
      }

      public void Duel(DuelResult duelResult)
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid || duelResult.isHeal)
          return;
        BL.UnitPosition unitPosition1 = this.mEnv.getUnitPosition(duelResult.attack);
        BL.UnitPosition unitPosition2 = this.mEnv.getUnitPosition(duelResult.defense);
        int num1 = 0;
        int num2 = 0;
        foreach (BL.DuelTurn turn in duelResult.turns)
        {
          if (turn.isAtackker)
            num1 += turn.realDamage;
          else
            num2 += turn.realDamage;
        }
        this.mBuilder.AppendFormat("[D,{0},{1},{2},{3},{4},{5},{6},{7}", (object) unitPosition1.unit.specificId, (object) unitPosition1.unit.unitId, (object) unitPosition1.row, (object) unitPosition1.column, (object) unitPosition2.row, (object) unitPosition2.column, (object) num1, (object) num2);
      }

      public void SkillUse(
        BL.Unit unit,
        BL.Skill skill,
        List<BL.Unit> targets,
        List<BL.Panel> panels)
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        BL.UnitPosition unitPosition1 = this.mEnv.getUnitPosition(unit);
        this.mBuilder.AppendFormat("[S,{0},{1},{2},{3},{4}", (object) unitPosition1.unit.specificId, (object) unitPosition1.unit.unitId, (object) unitPosition1.row, (object) unitPosition1.column, (object) skill.id);
        foreach (BL.Unit target in targets)
        {
          BL.UnitPosition unitPosition2 = this.mEnv.getUnitPosition(target);
          this.mBuilder.AppendFormat(",{0},{1}", (object) unitPosition2.row, (object) unitPosition2.column);
        }
        foreach (BL.Panel panel in panels)
          this.mBuilder.AppendFormat(",{0},{1}", (object) panel.row, (object) panel.column);
      }

      public void CallSkillUse(BL.Skill skill, List<BL.Unit> targets)
      {
        if (this.mEnv.battleInfo.quest_type != CommonQuestType.GuildRaid)
          return;
        this.mBuilder.AppendFormat("[CS,{0}", (object) skill.id);
        foreach (BL.Unit target in targets)
        {
          BL.UnitPosition unitPosition = this.mEnv.getUnitPosition(target);
          this.mBuilder.AppendFormat(",{0},{1}", (object) unitPosition.row, (object) unitPosition.column);
        }
      }

      public string Dump() => this.mBuilder.ToString();

      public void Clear() => this.mBuilder.Clear();
    }

    private class AttackStatusCacheContainer
    {
      private bool isAttack;
      private bool isHeal;
      private BL.Unit[] attackNeighbors;
      private BL.Unit[] defenseNeighbors;
      private int attackHp;
      private int orgAttackHp;
      private int orgDefenseHp;
      private BL.BattleModified<BL.SkillEffectList> attackSkillEffects;
      private BL.BattleModified<BL.SkillEffectList> defenseSkillEffects;
      private BL.BattleModified<BL.ClassValue<List<BL.SkillEffect>>> attackPanelSkillEffects;
      private BL.BattleModified<BL.ClassValue<List<BL.SkillEffect>>> defensePanelSkillEffects;
      private bool isFirst;
      private int mReadCount;
      public AttackStatus[] data;

      public int readCount => this.mReadCount;

      public AttackStatusCacheContainer(
        BL.ISkillEffectListUnit attack,
        BL.ISkillEffectListUnit defense,
        BL.Unit[] attackNeighbors,
        BL.Unit[] defenseNeighbors,
        int attackHp,
        bool isAttack,
        bool isHeal,
        BL.ClassValue<List<BL.SkillEffect>> apse,
        BL.ClassValue<List<BL.SkillEffect>> dpse,
        AttackStatus[] data)
      {
        this.isAttack = isAttack;
        this.isHeal = isHeal;
        this.setData(attack, defense, attackNeighbors, defenseNeighbors, attackHp, apse, dpse, data);
      }

      public void setData(
        BL.ISkillEffectListUnit attack,
        BL.ISkillEffectListUnit defense,
        BL.Unit[] attackNeighbors,
        BL.Unit[] defenseNeighbors,
        int attackHp,
        BL.ClassValue<List<BL.SkillEffect>> apse,
        BL.ClassValue<List<BL.SkillEffect>> dpse,
        AttackStatus[] data)
      {
        this.mReadCount = 0;
        this.attackSkillEffects = new BL.BattleModified<BL.SkillEffectList>(attack.skillEffects);
        this.defenseSkillEffects = new BL.BattleModified<BL.SkillEffectList>(defense.skillEffects);
        this.attackSkillEffects.isChangedOnce();
        this.defenseSkillEffects.isChangedOnce();
        this.attackPanelSkillEffects = new BL.BattleModified<BL.ClassValue<List<BL.SkillEffect>>>(apse);
        this.defensePanelSkillEffects = new BL.BattleModified<BL.ClassValue<List<BL.SkillEffect>>>(dpse);
        this.attackPanelSkillEffects.isChangedOnce();
        this.defensePanelSkillEffects.isChangedOnce();
        this.attackNeighbors = attackNeighbors;
        this.defenseNeighbors = defenseNeighbors;
        this.attackHp = attackHp;
        this.orgAttackHp = attack.hp;
        this.orgDefenseHp = defense.hp;
        this.data = data;
        this.isFirst = true;
      }

      public bool checkBaseValues(
        bool isAttack,
        bool isHeal,
        BL.ClassValue<List<BL.SkillEffect>> apse,
        BL.ClassValue<List<BL.SkillEffect>> dpse,
        BL.Unit[] attackNeighbors,
        BL.Unit[] defenseNeighbors)
      {
        return this.isAttack == isAttack && this.isHeal == isHeal && BL.equalPanelSkillEffectList(apse.value, this.attackPanelSkillEffects.value.value) && BL.equalPanelSkillEffectList(dpse.value, this.defensePanelSkillEffects.value.value) && this.checkNeighbors(attackNeighbors, defenseNeighbors);
      }

      private bool checkNeighbors(BL.Unit[] attackNeighbors, BL.Unit[] defenseNeighbors)
      {
        if (this.attackNeighbors.Length != attackNeighbors.Length || this.defenseNeighbors.Length != defenseNeighbors.Length)
          return false;
        for (int index = 0; index < this.attackNeighbors.Length; ++index)
        {
          if (this.attackNeighbors[index] != attackNeighbors[index])
            return false;
        }
        for (int index = 0; index < this.defenseNeighbors.Length; ++index)
        {
          if (this.defenseNeighbors[index] != defenseNeighbors[index])
            return false;
        }
        return true;
      }

      public bool checkValues(
        BL.ISkillEffectListUnit attack,
        BL.ISkillEffectListUnit defense,
        int attackHp)
      {
        ++this.mReadCount;
        return this.attackHp == attackHp && this.orgAttackHp == attack.hp && this.orgDefenseHp == defense.hp && !this.attackSkillEffects.isChanged && !this.defenseSkillEffects.isChanged && !this.attackPanelSkillEffects.isChanged && !this.defensePanelSkillEffects.isChanged;
      }

      public bool checkReadCount(int n)
      {
        if (!this.isFirst)
          return this.mReadCount > n;
        this.isFirst = false;
        return true;
      }

      public void resetReadCount() => this.mReadCount = 0;
    }

    [Serializable]
    public class MagicBullet : BL.ModelBase
    {
      [SerializeField]
      private int mSkillId;
      private int mAdditionalCost;
      public BL.MagicBullet.SAttackMethod sAttackMethod;
      [NonSerialized]
      private IAttackMethod attackMethod_;
      private string mPrefabName;

      public IAttackMethod attackMethod
      {
        get
        {
          return this.sAttackMethod.ID == 0 ? (IAttackMethod) null : this.attackMethod_ ?? (this.attackMethod_ = this.sAttackMethod.from == BL.MagicBullet.From.Normal ? MasterData.AttackMethod[this.sAttackMethod.ID].CreateInterface() : (this.sAttackMethod.from == BL.MagicBullet.From.Guest ? MasterData.BattleStageGuestAttackMethod[this.sAttackMethod.ID].CreateInterface() : MasterData.BattleStageEnemyAttackMethod[this.sAttackMethod.ID].CreateInterface()));
        }
        set
        {
          this.attackMethod_ = value;
          if (value == null)
            this.sAttackMethod.ID = 0;
          else if (value.original is AttackMethod)
          {
            this.sAttackMethod.ID = ((AttackMethod) value.original).ID;
            this.sAttackMethod.from = BL.MagicBullet.From.Normal;
          }
          else if (value.original is BattleStageEnemyAttackMethod)
          {
            this.sAttackMethod.ID = ((BattleStageEnemyAttackMethod) value.original).ID;
            this.sAttackMethod.from = BL.MagicBullet.From.Enemy;
          }
          else
          {
            this.sAttackMethod.ID = ((BattleStageGuestAttackMethod) value.original).ID;
            this.sAttackMethod.from = BL.MagicBullet.From.Guest;
          }
        }
      }

      public int skillId
      {
        get => this.mSkillId;
        set
        {
          this.mSkillId = value;
          ++this.revision;
        }
      }

      public BattleskillSkill skill => MasterData.BattleskillSkill[this.skillId];

      public string name => this.skill.name;

      public void setPrefabName(MasterDataTable.UnitJob job)
      {
        if (!this.skill.variable_magic_bullet_flag)
          return;
        this.mPrefabName = job.variable_magic_bullet_name;
      }

      public string prefabName
      {
        get
        {
          if (string.IsNullOrEmpty(this.mPrefabName))
            this.mPrefabName = this.skill.duel_magic_bullet_name;
          return this.mPrefabName;
        }
      }

      public CommonElement element => this.skill.element;

      public int power => this.skill.power;

      public int weight => this.skill.weight;

      public int cost => this.skill.consume_hp + this.mAdditionalCost;

      public int additionalCost
      {
        get => this.mAdditionalCost;
        set
        {
          this.mAdditionalCost = value;
          ++this.revision;
        }
      }

      public int maxRange => this.skill.max_range;

      public int minRange => this.skill.min_range;

      public string description => this.skill.description;

      public bool isAttack
      {
        get
        {
          switch (this.skill.target_type)
          {
            case BattleskillTargetType.enemy_single:
            case BattleskillTargetType.enemy_range:
              return true;
            default:
              return false;
          }
        }
      }

      public bool isHeal => !this.isAttack;

      public bool isDrain
      {
        get
        {
          return this.skill.genre1.HasValue && this.skill.genre1.Value == BattleskillGenre.attack && this.skill.genre2.HasValue && this.skill.genre2.Value == BattleskillGenre.heal;
        }
      }

      public float drainRate
      {
        get
        {
          if (!this.isDrain)
            return 0.0f;
          foreach (BattleskillEffect effect in this.skill.Effects)
          {
            if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.drain)
              return effect.GetFloat(BattleskillEffectLogicArgumentEnum.drain);
          }
          return 0.0f;
        }
      }

      public BattleskillEffect percentageDamage
      {
        get
        {
          foreach (BattleskillEffect effect in this.skill.Effects)
          {
            if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.percentage_damage)
              return effect;
          }
          return (BattleskillEffect) null;
        }
      }

      public IEnumerable<BattleskillEffect> investSkillEffect
      {
        get
        {
          BattleskillEffect[] battleskillEffectArray = this.skill.Effects;
          for (int index = 0; index < battleskillEffectArray.Length; ++index)
          {
            BattleskillEffect battleskillEffect = battleskillEffectArray[index];
            if (battleskillEffect.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_skilleffect)
              yield return battleskillEffect;
          }
          battleskillEffectArray = (BattleskillEffect[]) null;
        }
      }

      public void setAdditionalCost(int maxHp)
      {
        int num = 0;
        foreach (BattleskillEffect effect in this.skill.Effects)
        {
          if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.percentage_hp_consume_magic)
            num += Mathf.CeilToInt((float) ((Decimal) maxHp * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.cost_percentage)));
        }
        this.additionalCost = num;
      }

      public enum From
      {
        Normal,
        Guest,
        Enemy,
      }

      [Serializable]
      public struct SAttackMethod
      {
        public int ID;
        public BL.MagicBullet.From from;
      }
    }

    [Serializable]
    public class Skill : BL.ModelBase
    {
      [SerializeField]
      private int mId = 100000001;
      [SerializeField]
      private int? mRemain;
      [SerializeField]
      private int mUseTurn;
      [SerializeField]
      private int mLevel;
      [SerializeField]
      private int mNowUseCount;
      [NonSerialized]
      private int? useCountCache;
      [NonSerialized]
      private int? maxUseCountCache;
      [NonSerialized]
      private int[] mRange;

      public Skill() => this.mNowUseCount = 0;

      public Skill(BL.Skill target)
      {
        this.mId = target.id;
        this.mRemain = target.remain;
        this.mUseTurn = target.useTurn;
        this.mLevel = target.level;
        this.mNowUseCount = target.nowUseCount;
      }

      public int id
      {
        get => this.mId;
        set
        {
          this.mId = value;
          ++this.revision;
        }
      }

      public BattleskillSkill skill => MasterData.BattleskillSkill[this.mId];

      public string name => this.skill.name;

      public string description => this.skill.description;

      public BattleskillGenre? genre1 => this.skill.genre1;

      public BattleskillGenre? genre2 => this.skill.genre2;

      public int level
      {
        get => this.mLevel;
        set
        {
          this.mLevel = value;
          ++this.revision;
        }
      }

      public int? remain
      {
        get => this.mRemain;
        set
        {
          this.mRemain = value;
          ++this.revision;
        }
      }

      public int useTurn
      {
        get => this.mUseTurn;
        set
        {
          this.mUseTurn = value;
          ++this.revision;
        }
      }

      public int nowUseCount
      {
        get => this.mNowUseCount;
        set
        {
          this.mNowUseCount = value;
          ++this.revision;
        }
      }

      public BattleskillTargetType targetType => this.skill.target_type;

      public bool isOwn
      {
        get
        {
          switch (this.skill.target_type)
          {
            case BattleskillTargetType.myself:
            case BattleskillTargetType.player_range:
            case BattleskillTargetType.player_single:
            case BattleskillTargetType.dead_player_single:
            case BattleskillTargetType.dead_player_range:
              return true;
            default:
              return false;
          }
        }
      }

      public bool isNonSelect
      {
        get
        {
          switch (this.skill.target_type)
          {
            case BattleskillTargetType.myself:
            case BattleskillTargetType.player_range:
            case BattleskillTargetType.enemy_range:
            case BattleskillTargetType.complex_range:
            case BattleskillTargetType.dead_player_range:
              return true;
            default:
              return false;
          }
        }
      }

      public bool isJobAbility => this.skill.IsJobAbility;

      public bool isOugi => this.skill.skill_type == BattleskillSkillType.release;

      public bool isSEA => this.skill.skill_type == BattleskillSkillType.SEA;

      public bool isCommand => this.skill.skill_type == BattleskillSkillType.command;

      public static bool HasEffect(
        BL.Skill[] skills,
        params BattleskillEffectLogicEnum[] effectLogic)
      {
        return ((IEnumerable<BL.Skill>) skills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect => ((IEnumerable<BattleskillEffectLogicEnum>) effectLogic).Contains<BattleskillEffectLogicEnum>(effect.EffectLogic.Enum)))));
      }

      public static bool HasDontActionEffect(BL.Skill[] skills)
      {
        return BL.Skill.HasEffect(skills, BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.do_not_act, BattleskillEffectLogicEnum.sleep);
      }

      public static bool HasDontMoveEffect(BL.Skill[] skills)
      {
        return BL.Skill.HasEffect(skills, BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.do_not_move, BattleskillEffectLogicEnum.sleep);
      }

      public static bool HasDontEvasionEffect(BL.Skill[] skills)
      {
        return BL.Skill.HasEffect(skills, BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.sleep);
      }

      public Tuple<int, int> getHpCost(BL.Unit unit)
      {
        int num1 = 0;
        int num2 = 0;
        foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) this.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.hp_consume && x.checkLevel(this.level))))
        {
          num1 += battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.cost_value) + this.level * battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.cost_value_skill_ratio);
          num1 += Mathf.CeilToInt((float) ((Decimal) unit.parameter.Hp * (Decimal) (battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.cost_percentage) + (float) this.level * battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.cost_percentage_skill_ratio))));
          num2 += battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.must_hp_value) + this.level * battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.must_hp_value_skill_ratio);
          num2 += Mathf.CeilToInt((float) ((Decimal) unit.parameter.Hp * (Decimal) (battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.must_hp_percentage) + (float) this.level * battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.must_hp_percentage_skill_ratio))));
        }
        return Tuple.Create<int, int>(num1, num2);
      }

      public void initSkillCounts()
      {
        if (((IEnumerable<BattleskillEffect>) this.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.charged_use_turn && x.checkLevel(this.level))))
        {
          this.useTurn = 1;
        }
        else
        {
          this.useTurn = this.skill.charge_turn - (this.level - 1);
          if (this.maxUseCount != 0 && this.useTurn < this.maxUseCount)
            this.useTurn = this.maxUseCount;
        }
        this.remain = this.useCount == 0 ? new int?() : new int?(this.useCount + (this.level - 1));
        if (this.skill.skill_type == BattleskillSkillType.release)
          return;
        int? remain = this.remain;
        if (!remain.HasValue || this.maxUseCount == 0)
          return;
        remain = this.remain;
        int maxUseCount = this.maxUseCount;
        if (!(remain.GetValueOrDefault() > maxUseCount & remain.HasValue))
          return;
        this.remain = new int?(this.maxUseCount);
      }

      private void setSkillCountsCache()
      {
        IEnumerable<BattleskillEffect> battleskillEffects = ((IEnumerable<BattleskillEffect>) this.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.change_skill_use_count && x.checkLevel(this.level)));
        this.useCountCache = new int?(this.skill.use_count);
        this.maxUseCountCache = new int?(this.skill.max_use_count);
        foreach (BattleskillEffect battleskillEffect in battleskillEffects)
        {
          int? nullable = this.useCountCache;
          int num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.add_value);
          this.useCountCache = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + num1) : new int?();
          nullable = this.maxUseCountCache;
          int num2 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.max_add);
          this.maxUseCountCache = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + num2) : new int?();
        }
      }

      public int useCount
      {
        get
        {
          if (!this.useCountCache.HasValue)
            this.setSkillCountsCache();
          return this.useCountCache.Value;
        }
      }

      public int maxUseCount
      {
        get
        {
          if (!this.maxUseCountCache.HasValue)
            this.setSkillCountsCache();
          return this.maxUseCountCache.Value;
        }
      }

      public bool canUseTurn(int absoluteTurn)
      {
        BattleskillEffect battleskillEffect = Array.Find<BattleskillEffect>(this.skill.Effects, (Predicate<BattleskillEffect>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.release_skill_turn_check && x.checkLevel(this.level)));
        if (battleskillEffect == null)
          return true;
        int num1 = 0;
        if (battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.start_turn))
        {
          num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
          if (num1 != 0 && absoluteTurn < num1)
            return false;
        }
        if (battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.end_turn))
        {
          int num2 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.end_turn);
          if (num2 != 0 && absoluteTurn >= num2)
            return false;
        }
        if (battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.turn_cycle))
        {
          int num3 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle);
          if (num3 != 0 && (absoluteTurn - num1) % num3 != 0)
            return false;
        }
        return true;
      }

      public BL.Unit.TargetAttribute targetAttribute => BL.Unit.TargetAttribute.all;

      public bool isDeadTargetOnly
      {
        get
        {
          return this.targetType == BattleskillTargetType.dead_player_single || this.targetType == BattleskillTargetType.dead_player_range;
        }
      }

      public bool nonFacility => true;

      public int[] range
      {
        get
        {
          if (this.mRange == null)
          {
            this.mRange = new int[2]
            {
              this.skill.min_range,
              this.skill.max_range
            };
            foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) this.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.change_skill_range && x.checkLevel(this.level))))
            {
              this.mRange[0] += battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_add);
              if (this.mRange[0] < 0)
                this.mRange[0] = 0;
              this.mRange[1] += battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.max_add);
              if (this.mRange[1] < 0)
                this.mRange[1] = 0;
            }
          }
          return this.mRange;
        }
      }

      public BL.ForceID[] getTargetForceIDs(BL env, BL.ISkillEffectListUnit unit)
      {
        if (this.targetType == BattleskillTargetType.complex_range || this.targetType == BattleskillTargetType.complex_single)
          return ((IEnumerable<BL.ForceID>) BattleFuncs.getForceIDArray(env.getForceID(unit.originalUnit))).Concat<BL.ForceID>((IEnumerable<BL.ForceID>) env.getTargetForce(unit.originalUnit, false)).ToArray<BL.ForceID>();
        return this.isOwn ? BattleFuncs.getForceIDArray(env.getForceID(unit.originalUnit)) : env.getTargetForce(unit.originalUnit, unit.IsCharm);
      }

      public override string ToString() => "- スキル:" + this.name;
    }

    [Serializable]
    public class SkillEffect : BL.ModelBase
    {
      [SerializeField]
      private int mEffectId;
      [SerializeField]
      private int mBaseSkillId;
      [SerializeField]
      private int mBaseSkillLevel;
      [SerializeField]
      private int? mTurnRemain;
      [SerializeField]
      private int? mUseRemain;
      [SerializeField]
      private int? mExecuteRemain;
      [SerializeField]
      private bool mTimeOfDeathDisable;
      [SerializeField]
      private BL.Unit mUnit;
      [SerializeField]
      private int mKillCount;
      [SerializeField]
      private bool mIsBaseSkill;
      [SerializeField]
      private int mGearIndex;
      [SerializeField]
      private BL.Unit mParentUnit;
      [SerializeField]
      private BL.Unit mInvestUnit;
      [SerializeField]
      private int mInvestSkillId;
      [SerializeField]
      private float[] mWork;
      [SerializeField]
      private bool mIsDontDisplay;
      [SerializeField]
      private int mTurnCount;
      [SerializeField]
      private bool mAgainInvoked;
      [SerializeField]
      private int? mMoveDistance;
      [SerializeField]
      private bool mIsAttackMethod;
      [SerializeField]
      private int mInvestTurn;
      [SerializeField]
      private bool mDontCleanUseRemain;
      [SerializeField]
      private int mUniqueId;
      [NonSerialized]
      private BattleskillEffect _effect;
      [NonSerialized]
      private BattleskillSkill _baseSkill;
      [NonSerialized]
      private BattleFuncs.PackedSkillEffect packedSkillEffect_;
      [NonSerialized]
      private BattleFuncs.CheckInvokeGeneric checkInvokeGeneric_;

      public SkillEffect() => this.mUniqueId = this.GetHashCode();

      public SkillEffect(BL.SkillEffect target)
      {
        this.mEffectId = target.effectId;
        this.mBaseSkillId = target.baseSkillId;
        this.mBaseSkillLevel = target.baseSkillLevel;
        this.mTurnRemain = target.turnRemain;
        this.mUseRemain = target.useRemain;
        this.mExecuteRemain = target.executeRemain;
        this.mTimeOfDeathDisable = target.timeOfDeathDisable;
        this.mUnit = target.unit;
        this.mKillCount = target.killCount;
        this.mIsBaseSkill = target.isBaseSkill;
        this.mGearIndex = target.gearIndex;
        this.mParentUnit = target.parentUnit;
        this.mInvestUnit = target.investUnit;
        this.mInvestSkillId = target.investSkillId;
        if (target.work != null)
        {
          this.mWork = new float[target.work.Length];
          for (int index = 0; index < target.work.Length; ++index)
            this.mWork[index] = target.work[index];
        }
        this.mIsDontDisplay = target.isDontDisplay;
        this.mTurnCount = target.turnCount;
        this.mAgainInvoked = target.againInvoked;
        this.mMoveDistance = target.moveDistance;
        this.mIsAttackMethod = target.isAttackMethod;
        this.mInvestTurn = target.investTurn;
        this.mDontCleanUseRemain = target.dontCleanUseRemain;
        this.mUniqueId = target.uniqueId;
      }

      public int uniqueId => this.mUniqueId;

      public int effectId
      {
        get => this.mEffectId;
        set
        {
          this.mEffectId = value;
          ++this.revision;
        }
      }

      public int baseSkillId
      {
        get => this.mBaseSkillId;
        set
        {
          this.mBaseSkillId = value;
          ++this.revision;
        }
      }

      public int baseSkillLevel
      {
        get => this.mBaseSkillLevel;
        set
        {
          this.mBaseSkillLevel = value;
          ++this.revision;
        }
      }

      public int? turnRemain
      {
        get => this.mTurnRemain;
        set
        {
          this.mTurnRemain = value;
          ++this.revision;
        }
      }

      public int? useRemain
      {
        get => this.mUseRemain;
        set
        {
          this.mUseRemain = value;
          ++this.revision;
        }
      }

      public int? executeRemain
      {
        get => this.mExecuteRemain;
        set
        {
          this.mExecuteRemain = value;
          ++this.revision;
        }
      }

      public bool timeOfDeathDisable
      {
        get => this.mTimeOfDeathDisable;
        set
        {
          this.mTimeOfDeathDisable = value;
          ++this.revision;
        }
      }

      public BL.Unit unit
      {
        get => this.mUnit;
        set
        {
          this.mUnit = value;
          ++this.revision;
        }
      }

      public int killCount
      {
        get => this.mKillCount;
        set
        {
          this.mKillCount = value;
          ++this.revision;
        }
      }

      public int gearIndex
      {
        get => this.mGearIndex;
        set
        {
          this.mGearIndex = value;
          ++this.revision;
        }
      }

      public bool isBaseSkill
      {
        get => this.mIsBaseSkill;
        set
        {
          this.mIsBaseSkill = value;
          ++this.revision;
        }
      }

      public BL.Unit parentUnit
      {
        get => this.mParentUnit;
        set
        {
          this.mParentUnit = value;
          ++this.revision;
        }
      }

      public BL.Unit investUnit
      {
        get => this.mInvestUnit;
        set
        {
          this.mInvestUnit = value;
          ++this.revision;
        }
      }

      public int investSkillId
      {
        get => this.mInvestSkillId;
        set
        {
          this.mInvestSkillId = value;
          ++this.revision;
        }
      }

      public float[] work
      {
        get => this.mWork;
        set
        {
          this.mWork = value;
          ++this.revision;
        }
      }

      public bool isDontDisplay
      {
        get => this.mIsDontDisplay;
        set
        {
          this.mIsDontDisplay = value;
          ++this.revision;
        }
      }

      public bool isAttackMethod
      {
        get => this.mIsAttackMethod;
        set
        {
          this.mIsAttackMethod = value;
          ++this.revision;
        }
      }

      public int turnCount
      {
        get => this.mTurnCount;
        set
        {
          this.mTurnCount = value;
          ++this.revision;
        }
      }

      public bool againInvoked
      {
        get => this.mAgainInvoked;
        set
        {
          this.mAgainInvoked = value;
          ++this.revision;
        }
      }

      public int? moveDistance
      {
        get => this.mMoveDistance;
        set
        {
          this.mMoveDistance = value;
          ++this.revision;
        }
      }

      public int investTurn
      {
        get => this.mInvestTurn;
        set
        {
          this.mInvestTurn = value;
          ++this.revision;
        }
      }

      public bool dontCleanUseRemain
      {
        get => this.mDontCleanUseRemain;
        set
        {
          this.mDontCleanUseRemain = value;
          ++this.revision;
        }
      }

      public BattleskillEffect effect
      {
        get
        {
          this._effect = this._effect ?? MasterData.BattleskillEffect[this.effectId];
          return this._effect;
        }
      }

      public BattleskillSkill baseSkill
      {
        get
        {
          this._baseSkill = this._baseSkill ?? MasterData.BattleskillSkill[this.baseSkillId];
          return this._baseSkill;
        }
      }

      public bool effectEnded
      {
        get
        {
          if (this.useRemain.HasValue && this.useRemain.Value == 0 && !this.dontCleanUseRemain || this.turnRemain.HasValue && this.turnRemain.Value == 0 || this.executeRemain.HasValue && this.executeRemain.Value == 0)
            return true;
          return this.timeOfDeathDisable && this.unit != (BL.Unit) null && this.unit.isDead;
        }
      }

      public bool deathDisable
      {
        get => !(this.unit == (BL.Unit) null) && this.timeOfDeathDisable && this.unit.isDead;
      }

      public bool isLandTagEffect
      {
        get
        {
          return this.effect.GetPackedSkillEffect().HasKey(BattleskillEffectLogicArgumentEnum.land_tag1);
        }
      }

      public static BL.SkillEffect FromMasterData(
        BL.Unit unit,
        BattleskillEffect effect,
        BattleskillSkill skill,
        int level,
        bool isBaseSkill = false,
        int gearIndex = 0)
      {
        return new BL.SkillEffect()
        {
          effectId = effect.ID,
          baseSkillId = skill.ID,
          baseSkillLevel = level,
          turnRemain = effect.use_turn,
          useRemain = effect.use_count,
          unit = unit,
          timeOfDeathDisable = skill.time_of_death_skill_disable,
          executeRemain = new int?(),
          killCount = 0,
          isBaseSkill = isBaseSkill,
          gearIndex = gearIndex,
          investUnit = unit,
          investSkillId = 0,
          work = (float[]) null,
          isDontDisplay = false,
          turnCount = 0,
          againInvoked = false,
          moveDistance = new int?(0),
          isAttackMethod = false,
          investTurn = 0,
          dontCleanUseRemain = false
        };
      }

      public static BL.SkillEffect FromMasterData(
        BattleskillEffect effect,
        BattleskillSkill skill,
        int level,
        bool isBaseSkill = false,
        int gearIndex = 0,
        BL.Unit investUnit = null,
        int investSkillId = 0,
        bool isDontDisplay = false,
        bool isAttackMethod = false,
        int investTurn = 0)
      {
        BL.SkillEffect skillEffect = new BL.SkillEffect();
        skillEffect.effectId = effect.ID;
        skillEffect.baseSkillId = skill.ID;
        skillEffect.baseSkillLevel = level;
        skillEffect.useRemain = effect.use_count;
        skillEffect.timeOfDeathDisable = skill.time_of_death_skill_disable;
        skillEffect.unit = (BL.Unit) null;
        skillEffect.killCount = 0;
        skillEffect.isBaseSkill = isBaseSkill;
        skillEffect.gearIndex = gearIndex;
        skillEffect.investUnit = investUnit;
        skillEffect.investSkillId = investSkillId;
        skillEffect.work = (float[]) null;
        skillEffect.isDontDisplay = isDontDisplay;
        skillEffect.turnCount = 0;
        skillEffect.againInvoked = false;
        skillEffect.moveDistance = new int?(0);
        skillEffect.isAttackMethod = isAttackMethod;
        skillEffect.investTurn = investTurn;
        skillEffect.dontCleanUseRemain = false;
        if (skill.skill_type == BattleskillSkillType.ailment)
        {
          skillEffect.turnRemain = new int?();
          skillEffect.executeRemain = effect.use_turn;
        }
        else
        {
          skillEffect.turnRemain = effect.use_turn;
          skillEffect.executeRemain = new int?();
        }
        return skillEffect;
      }

      public static BL.SkillEffect FromRecovery(RecoverySkillEffect rse, BL env)
      {
        return new BL.SkillEffect()
        {
          effectId = rse.effectId,
          baseSkillId = rse.skillId,
          baseSkillLevel = rse.level,
          turnRemain = rse.turnRemain,
          useRemain = rse.useRemain,
          executeRemain = rse.executeRemain,
          killCount = rse.killCount,
          isBaseSkill = rse.isBaseSkill,
          gearIndex = rse.gearIndex,
          timeOfDeathDisable = MasterData.BattleskillSkill[rse.skillId].time_of_death_skill_disable,
          unit = rse.unitNetworkId == -1 ? (BL.Unit) null : BL.Unit.FromNetwork(new int?(rse.unitNetworkId), env),
          investUnit = rse.investUnitNetworkId == -1 ? (BL.Unit) null : BL.Unit.FromNetwork(new int?(rse.investUnitNetworkId), env),
          investSkillId = rse.investSkillId,
          work = rse.work,
          isDontDisplay = rse.isDontDisplay,
          turnCount = rse.turnCount,
          againInvoked = rse.againInvoked,
          moveDistance = rse.moveDistance,
          isAttackMethod = rse.isAttackMethod,
          investTurn = rse.investTurn,
          dontCleanUseRemain = rse.dontCleanUseRemain,
          parentUnit = rse.parentUnitNetworkId == -1 ? (BL.Unit) null : BL.Unit.FromNetwork(new int?(rse.parentUnitNetworkId), env)
        };
      }

      public BattleFuncs.PackedSkillEffect GetPackedSkillEffect()
      {
        if (this.packedSkillEffect_ == null)
          this.packedSkillEffect_ = BattleFuncs.PackedSkillEffect.Create(this);
        return this.packedSkillEffect_;
      }

      public BattleFuncs.CheckInvokeGeneric GetCheckInvokeGeneric(
        Func<BattleskillEffectLogicArgumentEnum, BattleFuncs.CheckInvokeGeneric.CheckPattern> extraFunc = null)
      {
        if (this.checkInvokeGeneric_ == null)
          this.checkInvokeGeneric_ = new BattleFuncs.CheckInvokeGeneric(this.GetPackedSkillEffect(), extraFunc);
        return this.checkInvokeGeneric_;
      }
    }

    [Serializable]
    public class SkillEffectList : BL.ModelBase
    {
      [SerializeField]
      private List<BL.SkillEffect> effects = new List<BL.SkillEffect>();
      [NonSerialized]
      private AssocList<int, List<BL.SkillEffect>> effectDic;
      [NonSerialized]
      private List<object>[] processDic;
      [NonSerialized]
      private Dictionary<BL.Unit, List<BL.SkillEffect>> rangeFromDic;
      private List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>> fixEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>();
      private List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> ratioEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>();
      private List<BL.SkillEffect> removedBaseSkillEffects = new List<BL.SkillEffect>();
      private List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>> removedFixEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>();
      private List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> removedRatioEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>();
      private Dictionary<int, int> duelSkillEffectIdInvokeCount = new Dictionary<int, int>();
      private Dictionary<int, int> duelSkillIdInvokeCount = new Dictionary<int, int>();
      private Dictionary<int, int> duelSkillIdInvokeCount2 = new Dictionary<int, int>();
      private List<BL.SkillEffect> removedOverwriteSkillEffects = new List<BL.SkillEffect>();
      private List<BL.SkillEffect> waitingTransformationSkillEffects = new List<BL.SkillEffect>();
      private BL.ModelBase landTagModified = new BL.ModelBase();
      [NonSerialized]
      private bool? hasEnhancedElement;

      public List<BL.SkillEffect> Effects => this.effects;

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>> FixEffectParams
      {
        get => this.fixEffectParams;
      }

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> RatioEffectParams
      {
        get => this.ratioEffectParams;
      }

      public List<BL.SkillEffect> RemovedBaseSkillEffects => this.removedBaseSkillEffects;

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>> RemovedFixEffectParams
      {
        get => this.removedFixEffectParams;
      }

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> RemovedRatioEffectParams
      {
        get => this.removedRatioEffectParams;
      }

      public Dictionary<int, int> DuelSkillEffectIdInvokeCount => this.duelSkillEffectIdInvokeCount;

      public Dictionary<int, int> DuelSkillIdInvokeCount => this.duelSkillIdInvokeCount;

      public Dictionary<int, int> DuelSkillIdInvokeCount2 => this.duelSkillIdInvokeCount2;

      public List<BL.SkillEffect> RemovedOverwriteSkillEffects => this.removedOverwriteSkillEffects;

      public List<BL.SkillEffect> WaitingTransformationSkillEffects
      {
        get => this.waitingTransformationSkillEffects;
      }

      public BL.ModelBase LandTagModified => this.landTagModified;

      public bool HasAilment
      {
        get
        {
          foreach (BL.SkillEffect effect in this.effects)
          {
            if (effect.baseSkill.skill_type == BattleskillSkillType.ailment)
              return true;
          }
          return false;
        }
      }

      public BL.SkillEffectList Clone()
      {
        BL.SkillEffectList skillEffectList = new BL.SkillEffectList();
        for (int index = 0; index < this.effects.Count; ++index)
        {
          BL.SkillEffect skillEffect = new BL.SkillEffect(this.effects[index]);
          skillEffectList.Effects.Add(skillEffect);
        }
        skillEffectList.CopyPrivateParameter(this);
        return skillEffectList;
      }

      public void CopyPrivateParameter(BL.SkillEffectList target)
      {
        this.fixEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>((IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>) target.FixEffectParams);
        this.ratioEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) target.RatioEffectParams);
        this.removedBaseSkillEffects = new List<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) target.RemovedBaseSkillEffects);
        this.removedFixEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>((IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>) target.RemovedFixEffectParams);
        this.removedRatioEffectParams = new List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) target.RemovedRatioEffectParams);
        this.duelSkillEffectIdInvokeCount = new Dictionary<int, int>((IDictionary<int, int>) target.DuelSkillEffectIdInvokeCount);
        this.duelSkillIdInvokeCount = new Dictionary<int, int>((IDictionary<int, int>) target.DuelSkillIdInvokeCount);
        this.duelSkillIdInvokeCount2 = new Dictionary<int, int>((IDictionary<int, int>) target.DuelSkillIdInvokeCount2);
        this.removedOverwriteSkillEffects = new List<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) target.RemovedOverwriteSkillEffects);
        this.waitingTransformationSkillEffects = new List<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) target.WaitingTransformationSkillEffects);
      }

      private void InitEffects()
      {
        if (this.effectDic != null)
          return;
        this.effectDic = new AssocList<int, List<BL.SkillEffect>>();
        this.processDic = new List<object>[4];
        for (int index = 0; index < this.processDic.Length; ++index)
          this.processDic[index] = new List<object>();
        this.rangeFromDic = new Dictionary<BL.Unit, List<BL.SkillEffect>>();
        foreach (BL.SkillEffect effect in this.effects)
        {
          int id = effect.effect.EffectLogic.ID;
          if (!this.effectDic.ContainsKey(id))
            this.effectDic.Add(id, new List<BL.SkillEffect>());
          this.effectDic[id].Add(effect);
          this.AddProcess(effect);
          this.AddRangeFrom(effect);
        }
      }

      private List<BL.SkillEffect> GetEffects(BattleskillEffectLogicEnum e)
      {
        this.InitEffects();
        List<BL.SkillEffect> skillEffectList;
        return !this.effectDic.TryGetValue((int) e, out skillEffectList) ? new List<BL.SkillEffect>() : skillEffectList;
      }

      private void AddEffect(BL.SkillEffect effect)
      {
        int id = effect.effect.EffectLogic.ID;
        List<BL.SkillEffect> skillEffectList;
        if (!this.effectDic.TryGetValue(id, out skillEffectList))
        {
          skillEffectList = new List<BL.SkillEffect>();
          this.effectDic.Add(id, skillEffectList);
        }
        skillEffectList.Add(effect);
      }

      private void AddProcess(BL.SkillEffect effect)
      {
        int index;
        switch (effect.effect.EffectLogic.opt_test1)
        {
          case -1:
            index = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == 0 ? 2 : 3;
            break;
          case 0:
            return;
          case 1:
            index = effect.effect.EffectLogic.opt_test4 != 0 ? (effect.effect.EffectLogic.opt_test4 != 8 ? 2 : 1) : 0;
            break;
          case 2:
            index = 3;
            break;
          default:
            return;
        }
        List<object> objectList = this.processDic[index];
        if (effect.effect.EffectLogic.opt_test2 == 1)
        {
          foreach (object obj in objectList)
          {
            if (obj is List<BL.SkillEffect> skillEffectList && skillEffectList[0].baseSkillId == effect.baseSkillId && skillEffectList[0].effect.EffectLogic.ID == effect.effect.EffectLogic.ID)
            {
              skillEffectList.Add(effect);
              return;
            }
          }
          objectList.Add((object) new List<BL.SkillEffect>()
          {
            effect
          });
        }
        else
          objectList.Add((object) effect);
      }

      public List<object> GetProcessEffects(int process)
      {
        this.InitEffects();
        return this.processDic[process];
      }

      private void AddRangeFrom(BL.SkillEffect effect)
      {
        if (!(effect.unit != (BL.Unit) null) || effect.baseSkill.skill_type != BattleskillSkillType.leader && (effect.baseSkill.skill_type != BattleskillSkillType.passive || !effect.baseSkill.range_effect_passive_skill))
          return;
        List<BL.SkillEffect> skillEffectList;
        if (!this.rangeFromDic.ContainsKey(effect.unit))
        {
          skillEffectList = new List<BL.SkillEffect>();
          this.rangeFromDic[effect.unit] = skillEffectList;
        }
        else
          skillEffectList = this.rangeFromDic[effect.unit];
        skillEffectList.Add(effect);
      }

      public List<BL.SkillEffect> GetRangeFromEffects(BL.Unit unit)
      {
        this.InitEffects();
        return this.rangeFromDic.ContainsKey(unit) ? this.rangeFromDic[unit] : (List<BL.SkillEffect>) null;
      }

      public bool HasEffect(BL.SkillEffect effect, bool? isTargetEnemy = null)
      {
        if (this.HasEffect(effect.baseSkill, isTargetEnemy))
        {
          foreach (BL.SkillEffect effect1 in this.effects)
          {
            if (effect1.effect.ID == effect.effect.ID)
              return true;
          }
        }
        return false;
      }

      public bool HasEffect(BattleskillSkill skill, bool? isTargetEnemy = null)
      {
        if (skill.skill_type == BattleskillSkillType.command || skill.skill_type == BattleskillSkillType.release || skill.skill_type == BattleskillSkillType.ailment || skill.skill_type == BattleskillSkillType.item || skill.skill_type == BattleskillSkillType.SEA)
        {
          foreach (BL.SkillEffect effect in this.effects)
          {
            if (effect.baseSkill.ID == skill.ID)
            {
              if (isTargetEnemy.HasValue && (skill.target_type == BattleskillTargetType.complex_range || skill.target_type == BattleskillTargetType.complex_single))
              {
                int num1 = effect.effect.is_targer_enemy ? 1 : 0;
                bool? nullable = isTargetEnemy;
                int num2 = nullable.GetValueOrDefault() ? 1 : 0;
                if (!(num1 == num2 & nullable.HasValue))
                  continue;
              }
              int? useRemain = effect.useRemain;
              if (useRemain.HasValue)
              {
                useRemain = effect.useRemain;
                if (useRemain.Value == 0)
                  continue;
              }
              return true;
            }
          }
        }
        return false;
      }

      public void Add(
        BL.SkillEffect effect,
        bool? isTargetEnemy = null,
        BL.ISkillEffectListUnit checkEnableUnit = null)
      {
        if (effect.effect.EffectLogic.HasTag(BattleskillEffectTag.ext_arg) || !effect.effect.checkLevel(effect.baseSkillLevel))
          return;
        if (effect.baseSkill.skill_type == BattleskillSkillType.ailment)
        {
          this.AddAilment(effect);
        }
        else
        {
          if (this.HasEffect(effect, isTargetEnemy) || this.CheckAddOverwriteEffects(effect) || checkEnableUnit != null && BattleFuncs.checkPassiveEffectEnable(effect.effect, checkEnableUnit) == 0)
            return;
          this.InitEffects();
          this.effects.Add(effect);
          this.AddEffect(effect);
          this.AddProcess(effect);
          this.AddRangeFrom(effect);
          ++this.revision;
        }
      }

      private BL.SkillEffect GetEffect(BL.SkillEffect effect)
      {
        if (this.HasEffect(effect.baseSkill))
        {
          foreach (BL.SkillEffect effect1 in this.effects)
          {
            if (effect1.effect.ID == effect.effect.ID)
              return effect1;
          }
        }
        return (BL.SkillEffect) null;
      }

      private void AddAilment(BL.SkillEffect effect)
      {
        Func<bool> func = (Func<bool>) (() =>
        {
          BattleskillEffectLogicEnum battleskillEffectLogicEnum = effect.effect.EffectLogic.Enum;
          if ((battleskillEffectLogicEnum == BattleskillEffectLogicEnum.fix_poison || battleskillEffectLogicEnum == BattleskillEffectLogicEnum.ratio_poison) && effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.force_add))
            return effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.force_add) == 1;
          return battleskillEffectLogicEnum == BattleskillEffectLogicEnum.provoke;
        });
        BL.SkillEffect effect1 = this.GetEffect(effect);
        if (effect1 == null || func())
        {
          this.InitEffects();
          this.effects.Add(effect);
          this.AddEffect(effect);
          this.AddProcess(effect);
          this.AddRangeFrom(effect);
          ++this.revision;
        }
        else
        {
          int? executeRemain = effect.executeRemain;
          if (!executeRemain.HasValue)
            return;
          executeRemain = effect1.executeRemain;
          if (!executeRemain.HasValue)
            return;
          executeRemain = effect.executeRemain;
          int num1 = executeRemain.Value;
          executeRemain = effect1.executeRemain;
          int num2 = executeRemain.Value;
          if (num1 <= num2)
            return;
          BL.SkillEffect skillEffect = effect1;
          executeRemain = effect.executeRemain;
          int? nullable = new int?(executeRemain.Value);
          skillEffect.executeRemain = nullable;
          ++this.revision;
        }
      }

      public bool HasAilmentEffectLogic(params BattleskillEffectLogicEnum[] effectLogic)
      {
        if (!PerformanceConfig.GetInstance().IsNotUseDeepCopy)
          return this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => ((IEnumerable<BattleskillEffectLogicEnum>) effectLogic).Any<BattleskillEffectLogicEnum>((Func<BattleskillEffectLogicEnum, bool>) (y => y == x.effect.EffectLogic.Enum))));
        for (int index1 = 0; index1 < this.effects.Count; ++index1)
        {
          for (int index2 = 0; index2 < effectLogic.Length; ++index2)
          {
            if (this.effects[index1].effect.EffectLogic.Enum == effectLogic[index2])
              return true;
          }
        }
        return false;
      }

      public bool IsSealedSkill(int skill_id, BL.SkillEffect effect = null)
      {
        return this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.seal)).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillSkill battleskillSkill;
          if (MasterData.BattleskillSkill.TryGetValue(skill_id, out battleskillSkill) && !BattleFuncs.isBonusSkillId(skill_id))
          {
            if (effect != null)
            {
              int num1 = x.effect.HasKey(BattleskillEffectLogicArgumentEnum.invest_type) ? x.effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type) : 0;
              if (num1 == 1 && effect.isBaseSkill || num1 == 2 && !effect.isBaseSkill)
                return false;
              int num2 = x.effect.HasKey(BattleskillEffectLogicArgumentEnum.logic_id) ? x.effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id) : 0;
              if (num2 != 0 && num2 != effect.effect.EffectLogic.ID)
                return false;
            }
            int num3 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
            if (num3 > 0 && num3 == skill_id)
              return true;
            int num4 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type);
            if (num4 > 0 && (BattleskillSkillType) num4 == battleskillSkill.skill_type || num3 == 0 && num4 == 0)
              return true;
          }
          return false;
        }));
      }

      public bool IsSealedSkillEffect(BL.SkillEffect effect)
      {
        return this.IsSealedSkill(effect.baseSkillId, effect);
      }

      private void removeZocEffect(BL env, BL.ISkillEffectListUnit unit)
      {
        int originalRow;
        int originalColumn;
        if ((object) (unit as BL.Unit) != null)
        {
          BL.UnitPosition unitPosition = env.getUnitPosition(unit as BL.Unit);
          originalRow = unitPosition.originalRow;
          originalColumn = unitPosition.originalColumn;
        }
        else
        {
          if (!(unit is BL.AIUnit))
            return;
          BL.AIUnit aiUnit = unit as BL.AIUnit;
          originalRow = aiUnit.originalRow;
          originalColumn = aiUnit.originalColumn;
        }
        env.removeZocPanels(unit, originalRow, originalColumn, unit is BL.AIUnit);
      }

      private void resetZocEffect(BL env, BL.ISkillEffectListUnit unit)
      {
        if (!this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.zoc)))
          return;
        int originalRow;
        int originalColumn;
        if ((object) (unit as BL.Unit) != null)
        {
          BL.UnitPosition unitPosition = env.getUnitPosition(unit as BL.Unit);
          originalRow = unitPosition.originalRow;
          originalColumn = unitPosition.originalColumn;
        }
        else
        {
          if (!(unit is BL.AIUnit))
            return;
          BL.AIUnit aiUnit = unit as BL.AIUnit;
          originalRow = aiUnit.originalRow;
          originalColumn = aiUnit.originalColumn;
        }
        env.addZocPanels(unit, originalRow, originalColumn, unit is BL.AIUnit);
      }

      public void RemoveEffect(int logic, BL env, BL.ISkillEffectListUnit unit)
      {
        this.Clean(logic, env, unit);
      }

      public BL.SkillEffect[] RemoveEffect(
        int logic,
        int skillId,
        int skillType,
        BL env,
        BL.ISkillEffectListUnit unit,
        Func<BL.SkillEffect, bool> funcExtraCheck = null)
      {
        return this.Clean(logic, skillId, skillType, env, unit, funcExtraCheck);
      }

      public void RemoveEffect(BL.Unit invocationUnit, BL env, BL.ISkillEffectListUnit unit)
      {
        List<BL.SkillEffect> list = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.unit != (BL.Unit) null && x.unit == invocationUnit)).ToList<BL.SkillEffect>();
        foreach (BL.SkillEffect skillEffect in list)
          this.effects.Remove(skillEffect);
        this.effects.AddRange(this.RecoveryOverwriteEffects());
        if (list.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.zoc)))
        {
          this.removeZocEffect(env, unit);
          this.resetZocEffect(env, unit);
          env.unitPositions.value.ForEach((Action<BL.UnitPosition>) (x => x.clearMovePanelCache()));
        }
        if (!list.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => BattleFuncs.isCharismaEffect(x.effect.EffectLogic.Enum))))
          return;
        (unit is BL.AIUnit ? unit as BL.UnitPosition : env.getUnitPosition(unit.originalUnit)).removePanelSkillEffects();
      }

      public bool RemoveEffect(IEnumerable<BL.SkillEffect> removeEffects)
      {
        bool flag = false;
        foreach (BL.SkillEffect removeEffect in removeEffects)
          flag |= this.effects.Remove(removeEffect);
        return flag;
      }

      public void ClearCache()
      {
        this.effectDic = (AssocList<int, List<BL.SkillEffect>>) null;
        this.processDic = (List<object>[]) null;
        this.rangeFromDic = (Dictionary<BL.Unit, List<BL.SkillEffect>>) null;
      }

      public void RemoveAilmentEffect(BL env, BL.ISkillEffectListUnit unit)
      {
        this.RemoveEffect(0, 0, 9, env, unit);
      }

      public void TurnStart(int turn, BL env, BL.ISkillEffectListUnit unit)
      {
        foreach (BL.SkillEffect effect in this.effects)
        {
          int? turnRemain = effect.turnRemain;
          if (turnRemain.HasValue)
          {
            BL.SkillEffect skillEffect = effect;
            turnRemain = effect.turnRemain;
            int? nullable = new int?(turnRemain.Value - 1);
            skillEffect.turnRemain = nullable;
          }
          effect.turnCount = 0;
          effect.againInvoked = false;
          effect.moveDistance = new int?(0);
        }
        foreach (BL.SkillEffect transformationSkillEffect in this.waitingTransformationSkillEffects)
        {
          int? turnRemain = transformationSkillEffect.turnRemain;
          if (turnRemain.HasValue)
          {
            BL.SkillEffect skillEffect = transformationSkillEffect;
            turnRemain = transformationSkillEffect.turnRemain;
            int? nullable = new int?(turnRemain.Value - 1);
            skillEffect.turnRemain = nullable;
          }
        }
        this.waitingTransformationSkillEffects.RemoveAll((Predicate<BL.SkillEffect>) (x => x.turnRemain.HasValue && x.turnRemain.Value == 0));
        this.Clean(env, unit);
        foreach (BL.SkillEffect removedBaseSkillEffect in this.removedBaseSkillEffects)
        {
          int? turnRemain = removedBaseSkillEffect.turnRemain;
          if (turnRemain.HasValue)
          {
            BL.SkillEffect skillEffect = removedBaseSkillEffect;
            turnRemain = removedBaseSkillEffect.turnRemain;
            int? nullable = new int?(turnRemain.Value - 1);
            skillEffect.turnRemain = nullable;
          }
        }
        this.removedBaseSkillEffects.RemoveAll((Predicate<BL.SkillEffect>) (x => x.turnRemain.HasValue && x.turnRemain.Value == 0));
      }

      public void PhaseStart(BL env, BL.ISkillEffectListUnit unit) => this.Clean(env, unit);

      public bool AilmentExecuted(BL env, BL.ISkillEffectListUnit unit)
      {
        bool flag = false;
        foreach (BL.SkillEffect skillEffect in this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.executeRemain.HasValue)))
        {
          skillEffect.executeRemain = new int?(skillEffect.executeRemain.Value - 1);
          flag = true;
        }
        BL.SkillEffect[] array1 = BattleFuncs.getEnabledCharismaEffects(unit).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(unit, x))).ToArray<BL.SkillEffect>();
        this.Clean(env, unit);
        BL.SkillEffect[] array2 = BattleFuncs.getEnabledCharismaEffects(unit).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(unit, x))).ToArray<BL.SkillEffect>();
        BattleFuncs.iSkillEffectListUnitToUnitPosition(unit).commitPanelSkillEffects(((IEnumerable<BL.SkillEffect>) array1).Union<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array2).Except<BL.SkillEffect>(((IEnumerable<BL.SkillEffect>) array1).Intersect<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array2)));
        return flag;
      }

      public void ColosseumTurnStart(bool excludeCommand)
      {
        foreach (BL.SkillEffect effect in this.effects)
        {
          int? turnRemain = effect.turnRemain;
          if (turnRemain.HasValue && (!excludeCommand || effect.baseSkill.skill_type != BattleskillSkillType.command && effect.baseSkill.skill_type != BattleskillSkillType.release))
          {
            BL.SkillEffect skillEffect = effect;
            turnRemain = effect.turnRemain;
            int? nullable = new int?(turnRemain.Value - 1);
            skillEffect.turnRemain = nullable;
          }
        }
        this.Clean();
      }

      private void Clean(BL env, BL.ISkillEffectListUnit unit)
      {
        bool flag1 = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effectEnded || effect.deathDisable)).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.zoc));
        bool flag2 = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effectEnded || effect.deathDisable)).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => BattleFuncs.isCharismaEffect(effect.effect.EffectLogic.Enum)));
        bool flag3 = this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => (effect.effectEnded || effect.deathDisable) && effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.seal));
        bool flag4 = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effectEnded || effect.deathDisable)).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.transformation));
        bool flag5 = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effectEnded || effect.deathDisable)).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effect.GetPackedSkillEffect().HasKey(BattleskillEffectLogicArgumentEnum.land_tag1)));
        BL.UnitPosition unitPosition = unit is BL.AIUnit ? unit as BL.UnitPosition : env.getUnitPosition(unit.originalUnit);
        if (flag1)
          this.removeZocEffect(env, unit);
        if (flag2)
          unitPosition.removePanelSkillEffects();
        Dictionary<BL.ISkillEffectListUnit, int> dictionary = (Dictionary<BL.ISkillEffectListUnit, int>) null;
        BL.ISkillEffectListUnit[] skillEffectListUnitArray = (BL.ISkillEffectListUnit[]) null;
        if (flag3)
        {
          dictionary = new Dictionary<BL.ISkillEffectListUnit, int>();
          skillEffectListUnitArray = ((IEnumerable<BL.ForceID>) new BL.ForceID[3]
          {
            BL.ForceID.player,
            BL.ForceID.enemy,
            BL.ForceID.neutral
          }).SelectMany<BL.ForceID, BL.ISkillEffectListUnit>((Func<BL.ForceID, IEnumerable<BL.ISkillEffectListUnit>>) (x => BattleFuncs.getForceUnits(x, unit is BL.AIUnit, true, includeJumping: true))).ToArray<BL.ISkillEffectListUnit>();
          foreach (BL.ISkillEffectListUnit skillEffectListUnit in skillEffectListUnitArray)
          {
            BL.ISkillEffectListUnit u = skillEffectListUnit;
            dictionary[u] = u.skillEffects.All().Count<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(u, x)));
          }
        }
        this.effects = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => !effect.effectEnded && !effect.deathDisable)).ToList<BL.SkillEffect>();
        this.effects.AddRange(this.RecoveryOverwriteEffects());
        this.ClearCache();
        this.InitEffects();
        if (flag1)
        {
          this.resetZocEffect(env, unit);
          env.unitPositions.value.ForEach((Action<BL.UnitPosition>) (x => x.clearMovePanelCache()));
        }
        if (flag2)
          unitPosition.addPanelSkillEffects();
        if (flag4)
          this.ResetTransformationSkillEffects(unit.transformationGroupId);
        if (flag5)
          this.LandTagModified.commit();
        this.ResetHasEnhancedElement();
        if (flag3)
        {
          foreach (BL.ISkillEffectListUnit skillEffectListUnit in skillEffectListUnitArray)
          {
            BL.ISkillEffectListUnit u = skillEffectListUnit;
            if (u.skillEffects.All().Count<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(u, x))) != dictionary[u])
              u.skillEffects.commit();
          }
        }
        ++this.revision;
      }

      private void Clean(int logic, BL env, BL.ISkillEffectListUnit unit)
      {
        int num = 1001079 != logic ? 0 : (this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == (BattleskillEffectLogicEnum) logic)) ? 1 : 0);
        bool flag = BattleFuncs.isCharismaEffect((BattleskillEffectLogicEnum) logic) && this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == (BattleskillEffectLogicEnum) logic));
        BL.UnitPosition unitPosition = unit is BL.AIUnit ? unit as BL.UnitPosition : env.getUnitPosition(unit.originalUnit);
        if (num != 0)
          this.removeZocEffect(env, unit);
        if (flag)
          unitPosition.removePanelSkillEffects();
        this.effects = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effect.EffectLogic.Enum != (BattleskillEffectLogicEnum) logic)).ToList<BL.SkillEffect>();
        this.effects.AddRange(this.RecoveryOverwriteEffects());
        this.ClearCache();
        this.InitEffects();
        if (num != 0)
        {
          this.resetZocEffect(env, unit);
          env.unitPositions.value.ForEach((Action<BL.UnitPosition>) (x => x.clearMovePanelCache()));
        }
        if (flag)
          unitPosition.addPanelSkillEffects();
        ++this.revision;
      }

      private BL.SkillEffect[] Clean(
        int logic,
        int skillId,
        int skillType,
        BL env,
        BL.ISkillEffectListUnit unit,
        Func<BL.SkillEffect, bool> funcExtraCheck = null)
      {
        Func<BL.SkillEffect, bool> funcCheck = (Func<BL.SkillEffect, bool>) (effect =>
        {
          if (logic != 0 && (BattleskillEffectLogicEnum) logic != effect.effect.EffectLogic.Enum || skillId != 0 && skillId != effect.baseSkill.ID || skillType != 0 && (BattleskillSkillType) skillType != effect.baseSkill.skill_type)
            return true;
          return funcExtraCheck != null && funcExtraCheck(effect);
        });
        foreach (BL.SkillEffect effect in this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !funcCheck(x))))
        {
          if (effect.isBaseSkill && (effect.baseSkill.skill_type != BattleskillSkillType.passive || !effect.baseSkill.range_effect_passive_skill))
            this.AddRemovedBaseSkillEffect(effect);
        }
        List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect effect in this.waitingTransformationSkillEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !funcCheck(x))))
        {
          if (effect.isBaseSkill && (effect.baseSkill.skill_type != BattleskillSkillType.passive || !effect.baseSkill.range_effect_passive_skill))
            this.AddRemovedBaseSkillEffect(effect);
          skillEffectList.Add(effect);
        }
        foreach (BL.SkillEffect skillEffect in skillEffectList)
          this.waitingTransformationSkillEffects.Remove(skillEffect);
        List<BL.SkillEffect> remainEffects = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => funcCheck(x))).ToList<BL.SkillEffect>();
        BL.SkillEffect[] array = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !remainEffects.Contains(x))).ToArray<BL.SkillEffect>();
        int num = this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.zoc && !remainEffects.Contains(x))) ? 1 : 0;
        bool flag = this.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => BattleFuncs.isCharismaEffect(x.effect.EffectLogic.Enum) && !remainEffects.Contains(x)));
        BL.UnitPosition unitPosition = unit is BL.AIUnit ? unit as BL.UnitPosition : env.getUnitPosition(unit.originalUnit);
        if (num != 0)
          this.removeZocEffect(env, unit);
        if (flag)
          unitPosition.removePanelSkillEffects();
        this.effects = remainEffects;
        this.effects.AddRange(this.RecoveryOverwriteEffects());
        this.ClearCache();
        this.InitEffects();
        if (num != 0)
        {
          this.resetZocEffect(env, unit);
          env.unitPositions.value.ForEach((Action<BL.UnitPosition>) (x => x.clearMovePanelCache()));
        }
        if (flag)
          unitPosition.addPanelSkillEffects();
        this.ResetHasEnhancedElement();
        foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int> tuple in this.fixEffectParams.Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, bool>) (x => !funcCheck(x.Item2))))
        {
          if (tuple.Item2.isBaseSkill && (tuple.Item2.baseSkill.skill_type != BattleskillSkillType.passive || !tuple.Item2.baseSkill.range_effect_passive_skill))
            this.AddRemovedFixEffectParam(tuple);
        }
        foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> tuple in this.ratioEffectParams.Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, bool>) (x => !funcCheck(x.Item2))))
        {
          if (tuple.Item2.isBaseSkill && (tuple.Item2.baseSkill.skill_type != BattleskillSkillType.passive || !tuple.Item2.baseSkill.range_effect_passive_skill))
            this.AddRemovedRatioEffectParam(tuple);
        }
        this.fixEffectParams = this.fixEffectParams.Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, bool>) (x => funcCheck(x.Item2))).ToList<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>();
        this.ratioEffectParams = this.ratioEffectParams.Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, bool>) (x => funcCheck(x.Item2))).ToList<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>();
        ++this.revision;
        return array;
      }

      private void Clean()
      {
        this.effects = this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => !effect.turnRemain.HasValue || effect.turnRemain.Value != 0)).ToList<BL.SkillEffect>();
        this.effects.AddRange(this.RecoveryOverwriteEffects());
        this.ClearCache();
        this.InitEffects();
        ++this.revision;
      }

      public IEnumerable<BL.SkillEffect> Where(BattleskillEffectTag tag1)
      {
        return this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.HasTag(tag1)));
      }

      public IEnumerable<BL.SkillEffect> Where(BattleskillEffectTag tag1, BattleskillEffectTag tag2)
      {
        return this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.HasTag(tag1) && x.effect.EffectLogic.HasTag(tag2)));
      }

      public IEnumerable<BL.SkillEffect> Where(
        BattleskillEffectTag tag1,
        BattleskillEffectTag tag2,
        BattleskillEffectTag tag3)
      {
        return this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.HasTag(tag1) && x.effect.EffectLogic.HasTag(tag2) && x.effect.EffectLogic.HasTag(tag3)));
      }

      public IEnumerable<BL.SkillEffect> Where(BattleskillEffectLogicEnum e)
      {
        return (IEnumerable<BL.SkillEffect>) this.GetEffects(e);
      }

      public IEnumerable<BL.SkillEffect> Where(
        BattleskillEffectLogicEnum e,
        Func<BL.SkillEffect, bool> f)
      {
        List<BL.SkillEffect> effects = this.GetEffects(e);
        int count = effects.Count;
        for (int i = 0; i < count; ++i)
        {
          BL.SkillEffect skillEffect = effects[i];
          if (f(effects[i]))
            yield return skillEffect;
        }
      }

      public IEnumerable<BL.SkillEffect> WhereAndGroupBy(
        BattleskillEffectLogicEnum e,
        Func<IEnumerable<BL.SkillEffect>, BL.SkillEffect> f,
        Func<IGrouping<int, BL.SkillEffect>, IEnumerable<BL.SkillEffect>> enabled)
      {
        foreach (IGrouping<int, BL.SkillEffect> grouping in this.GetEffects(e).GroupBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)))
        {
          IEnumerable<BL.SkillEffect> source = enabled(grouping);
          if (source.Any<BL.SkillEffect>())
            yield return f(source);
        }
      }

      public IEnumerable<BattleskillSkill> Where(
        BattleskillSkillType skillType,
        bool includeDontDisplay = false,
        bool baseSkillOnly = false,
        bool remainOnly = false)
      {
        HashSet<BattleskillSkill> battleskillSkillSet = new HashSet<BattleskillSkill>();
        foreach (BL.SkillEffect effect in this.effects)
        {
          if (effect.baseSkill.skill_type == skillType && !effect.isDontDisplay | includeDontDisplay && (!baseSkillOnly || effect.isBaseSkill))
          {
            if (remainOnly)
            {
              int? useRemain = effect.useRemain;
              if (useRemain.HasValue)
              {
                useRemain = effect.useRemain;
                int num = 1;
                if (!(useRemain.GetValueOrDefault() >= num & useRemain.HasValue))
                  continue;
              }
            }
            battleskillSkillSet.Add(effect.baseSkill);
          }
        }
        return (IEnumerable<BattleskillSkill>) battleskillSkillSet;
      }

      public List<Tuple<BattleskillSkill, int?>> GetAilmentData()
      {
        Dictionary<BattleskillSkill, int?> source = new Dictionary<BattleskillSkill, int?>();
        foreach (BL.SkillEffect effect in this.effects)
        {
          if (effect.baseSkill.skill_type == BattleskillSkillType.ailment)
          {
            if (!source.ContainsKey(effect.baseSkill))
            {
              source.Add(effect.baseSkill, effect.executeRemain);
            }
            else
            {
              int? nullable = source[effect.baseSkill];
              int? executeRemain = effect.executeRemain;
              if (nullable.GetValueOrDefault() < executeRemain.GetValueOrDefault() & nullable.HasValue & executeRemain.HasValue)
                source[effect.baseSkill] = effect.executeRemain;
            }
          }
        }
        return source.Select<KeyValuePair<BattleskillSkill, int?>, Tuple<BattleskillSkill, int?>>((Func<KeyValuePair<BattleskillSkill, int?>, Tuple<BattleskillSkill, int?>>) (x => new Tuple<BattleskillSkill, int?>(x.Key, x.Value))).ToList<Tuple<BattleskillSkill, int?>>();
      }

      public List<Tuple<BattleskillSkill, int?>> GetBuffDebuffData()
      {
        Dictionary<BattleskillSkill, int?> source = new Dictionary<BattleskillSkill, int?>();
        foreach (BL.SkillEffect effect in this.effects)
        {
          int? battleskillGenre;
          if (effect.baseSkill.genre1_BattleskillGenre.HasValue)
          {
            battleskillGenre = effect.baseSkill.genre1_BattleskillGenre;
            int num = 3;
            if (battleskillGenre.GetValueOrDefault() == num & battleskillGenre.HasValue)
              goto label_8;
          }
          if (effect.baseSkill.genre1_BattleskillGenre.HasValue)
          {
            battleskillGenre = effect.baseSkill.genre1_BattleskillGenre;
            int num = 4;
            if (battleskillGenre.GetValueOrDefault() == num & battleskillGenre.HasValue)
              goto label_8;
          }
          if (effect.baseSkill.genre1_BattleskillGenre.HasValue)
          {
            battleskillGenre = effect.baseSkill.genre1_BattleskillGenre;
            int num = 5;
            if (!(battleskillGenre.GetValueOrDefault() == num & battleskillGenre.HasValue))
              continue;
          }
          else
            continue;
label_8:
          if (!source.ContainsKey(effect.baseSkill))
          {
            source.Add(effect.baseSkill, effect.turnRemain);
          }
          else
          {
            battleskillGenre = source[effect.baseSkill];
            int? turnRemain = effect.turnRemain;
            if (battleskillGenre.GetValueOrDefault() < turnRemain.GetValueOrDefault() & battleskillGenre.HasValue & turnRemain.HasValue)
              source[effect.baseSkill] = effect.turnRemain;
          }
        }
        return source.Select<KeyValuePair<BattleskillSkill, int?>, Tuple<BattleskillSkill, int?>>((Func<KeyValuePair<BattleskillSkill, int?>, Tuple<BattleskillSkill, int?>>) (x => new Tuple<BattleskillSkill, int?>(x.Key, x.Value))).ToList<Tuple<BattleskillSkill, int?>>();
      }

      public override string ToString()
      {
        string str = "";
        foreach (BL.SkillEffect effect in this.effects)
          str += string.Format("- エフェクト{0}({1}) -- スキル{2}({3})より\n", (object) effect.effect.EffectLogic.ID, (object) effect.effect.EffectLogic.name, (object) effect.baseSkill.ID, (object) effect.baseSkill.name);
        return str;
      }

      public List<BL.SkillEffect> All() => this.effects;

      public void AddKillCount(int addCount)
      {
        foreach (BL.SkillEffect effect in this.effects)
          effect.killCount += addCount;
      }

      public void SetKillCount(int setCount)
      {
        foreach (BL.SkillEffect effect in this.effects)
          effect.killCount = setCount;
      }

      public int GetCompleteCount()
      {
        IEnumerable<BL.SkillEffect> againEffects = this.GetAgainEffects();
        return againEffects.Any<BL.SkillEffect>() ? Mathf.Max(againEffects.Max<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.complete_count))), 1) : 1;
      }

      public int GetActionCount()
      {
        IEnumerable<BL.SkillEffect> againEffects = this.GetAgainEffects();
        return againEffects.Any<BL.SkillEffect>() ? Mathf.Max(againEffects.Max<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.action_count))), 1) : 1;
      }

      public int GetCantChageCurrentActionCount()
      {
        IEnumerable<BL.SkillEffect> againEffects = this.GetAgainEffects();
        return againEffects.Any<BL.SkillEffect>() ? Mathf.Max(againEffects.Max<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_chage_action_count))), 0) : 0;
      }

      public int GetCantUseSkillAgain()
      {
        IEnumerable<BL.SkillEffect> againEffects = this.GetAgainEffects();
        return againEffects.Any<BL.SkillEffect>() ? Mathf.Min(againEffects.Min<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_use_skill))), 1) : 1;
      }

      public IEnumerable<BL.SkillEffect> GetAgainEffects()
      {
        return this.effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.again));
      }

      public int CanUseSkillOne(
        BattleskillSkill skill,
        int level,
        BL.ISkillEffectListUnit unit,
        BL env,
        BL.ISkillEffectListUnit useUnit,
        int nowUseCount,
        bool? callIsPlayer = null)
      {
        IEnumerable<BattleskillEffect> source = ((IEnumerable<BattleskillEffect>) skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum != BattleskillEffectLogicEnum.hp_consume && x.EffectLogic.Enum != BattleskillEffectLogicEnum.skill_chain && x.EffectLogic.Enum != BattleskillEffectLogicEnum.change_skill_range && x.EffectLogic.Enum != BattleskillEffectLogicEnum.change_skill_use_count && x.EffectLogic.Enum != BattleskillEffectLogicEnum.random_choice && x.EffectLogic.Enum != BattleskillEffectLogicEnum.again_use_skill && x.EffectLogic.Enum != BattleskillEffectLogicEnum.use_skill_count_range_effect && !x.EffectLogic.HasTag(BattleskillEffectTag.ext_arg) && x.checkLevel(level) && x.checkUseSkillCount(nowUseCount)));
        bool? isTargetEnemy = new bool?();
        if (useUnit != null && env != null && (skill.target_type == BattleskillTargetType.complex_range || skill.target_type == BattleskillTargetType.complex_single))
        {
          isTargetEnemy = new bool?(env.getForceID(useUnit.originalUnit) != env.getForceID(unit.originalUnit));
          source = source.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x =>
          {
            int num1 = x.is_targer_enemy ? 1 : 0;
            bool? nullable = isTargetEnemy;
            int num2 = nullable.GetValueOrDefault() ? 1 : 0;
            return num1 == num2 & nullable.HasValue;
          }));
        }
        else if (env != null && skill.skill_type == BattleskillSkillType.call && skill.IsCallTargetComplex && callIsPlayer.HasValue)
        {
          isTargetEnemy = new bool?(!((IEnumerable<BL.ForceID>) (callIsPlayer.Value ? BattleFuncs.ForceIDArrayPlayer : BattleFuncs.ForceIDArrayPlayerTarget)).Contains<BL.ForceID>(env.getForceID(unit.originalUnit)));
          source = source.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x =>
          {
            int num3 = x.is_targer_enemy ? 1 : 0;
            bool? nullable = isTargetEnemy;
            int num4 = nullable.GetValueOrDefault() ? 1 : 0;
            return num3 == num4 & nullable.HasValue;
          }));
        }
        bool flag1 = source.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.EffectLogic.HasTag(BattleskillEffectTag.immediately)));
        bool flag2 = source.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.HasTag(BattleskillEffectTag.immediately)));
        bool flag3 = false;
        bool flag4 = false;
        if (flag1)
        {
          flag1 = false;
          foreach (BattleskillEffect effect in source.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.EffectLogic.HasTag(BattleskillEffectTag.immediately))))
          {
            if (BattleFuncs.checkPassiveEffectEnable(effect, unit) != 0)
            {
              flag1 = true;
              break;
            }
          }
        }
        if (flag1)
          flag3 = !this.HasEffect(skill, isTargetEnemy);
        if (flag2)
        {
          foreach (BattleskillEffect effect in source.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.HasTag(BattleskillEffectTag.immediately))))
          {
            if (((IEnumerable<BattleskillEffectLogicEnum>) new BattleskillEffectLogicEnum[3]
            {
              BattleskillEffectLogicEnum.rescue,
              BattleskillEffectLogicEnum.keep_away,
              BattleskillEffectLogicEnum.attract
            }).Contains<BattleskillEffectLogicEnum>(effect.EffectLogic.Enum))
            {
              bool enableAnchorGround;
              if (BattleFuncs.canUseImmediateSkillEffect(effect, unit, out enableAnchorGround, useUnit, callIsPlayer: callIsPlayer))
                flag4 = true;
              else if (enableAnchorGround)
              {
                flag4 = false;
                flag1 = false;
                break;
              }
            }
            else if (!flag4 && BattleFuncs.canUseImmediateSkillEffect(effect, unit, out bool _, useUnit, callIsPlayer: callIsPlayer))
              flag4 = true;
          }
          if (!flag4 && !flag1 && source.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.transformation)))
            return 1;
        }
        return flag1 & flag2 ? (!flag3 && !flag4 ? 1 : 0) : (flag1 ? (flag3 ? 0 : 1) : (flag2 && flag4 ? 0 : 2));
      }

      public int CanUseSkill(
        BattleskillSkill skill,
        int level,
        BL.ISkillEffectListUnit unit,
        BL env,
        BL.ISkillEffectListUnit useUnit,
        int nowUseCount,
        bool? callIsPlayer = null)
      {
        int num1 = this.CanUseSkillOne(skill, level, unit, env, useUnit, nowUseCount, callIsPlayer);
        if (num1 == 0)
          return 0;
        if (((IEnumerable<BattleskillEffect>) skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.rescue || x.EffectLogic.Enum == BattleskillEffectLogicEnum.attract || x.EffectLogic.Enum == BattleskillEffectLogicEnum.shift_break || x.EffectLogic.Enum == BattleskillEffectLogicEnum.keep_away || x.EffectLogic.Enum == BattleskillEffectLogicEnum.changing || x.EffectLogic.Enum == BattleskillEffectLogicEnum.reduct_command_skill_use)))
          return num1;
        foreach (Tuple<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>> chainSkillTarget in BattleFuncs.getChainSkillTargets(skill, level, unit, (BL.Panel) null, useUnit, nowUseCount))
        {
          foreach (BL.ISkillEffectListUnit unit1 in chainSkillTarget.Item2)
          {
            int num2 = unit1.skillEffects.CanUseSkillOne(chainSkillTarget.Item1, level, unit1, env, useUnit, nowUseCount, callIsPlayer);
            if (num2 == 0)
              return 0;
            if (num2 < num1)
              num1 = num2;
          }
        }
        return num1;
      }

      public IEnumerable<Tuple<BL.SkillEffect, int>> GetFixEffectParams(
        BattleskillEffectLogicEnum logic)
      {
        return this.fixEffectParams.Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, bool>) (x => x.Item1 == logic)).Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, Tuple<BL.SkillEffect, int>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, Tuple<BL.SkillEffect, int>>) (x => new Tuple<BL.SkillEffect, int>(x.Item2, x.Item3)));
      }

      public IEnumerable<Tuple<BL.SkillEffect, float>> GetRatioEffectParams(
        BattleskillEffectLogicEnum logic)
      {
        return this.ratioEffectParams.Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, bool>) (x => x.Item1 == logic)).Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, Tuple<BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, Tuple<BL.SkillEffect, float>>) (x => new Tuple<BL.SkillEffect, float>(x.Item2, x.Item3)));
      }

      public void AddFixEffectParam(
        BattleskillEffectLogicEnum logic,
        BL.SkillEffect effect,
        int value)
      {
        this.fixEffectParams.Add(new Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>(logic, effect, value));
      }

      public void AddRatioEffectParam(
        BattleskillEffectLogicEnum logic,
        BL.SkillEffect effect,
        float value)
      {
        this.ratioEffectParams.Add(new Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>(logic, effect, value));
      }

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>> GetAllFixEffectParams()
      {
        return this.fixEffectParams;
      }

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> GetAllRatioEffectParams()
      {
        return this.ratioEffectParams;
      }

      public IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> GetAllEffectParams()
      {
        return this.fixEffectParams.Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) (x => Tuple.Create<BattleskillEffectLogicEnum, BL.SkillEffect, float>(x.Item1, x.Item2, (float) x.Item3))).Concat<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) this.ratioEffectParams);
      }

      public void RecoveryRemovedSkillEffects(BL.ISkillEffectListUnit unit)
      {
        if (this.removedBaseSkillEffects.Any<BL.SkillEffect>())
        {
          foreach (BL.SkillEffect removedBaseSkillEffect in this.removedBaseSkillEffects)
            this.effects.Add(removedBaseSkillEffect);
          this.removedBaseSkillEffects.Clear();
          this.ClearCache();
          ++this.revision;
        }
        this.ResetTransformationSkillEffects(unit.transformationGroupId);
        foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int> removedFixEffectParam in this.removedFixEffectParams)
          this.fixEffectParams.Add(removedFixEffectParam);
        this.removedFixEffectParams.Clear();
        foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> ratioEffectParam in this.removedRatioEffectParams)
          this.ratioEffectParams.Add(ratioEffectParam);
        this.removedRatioEffectParams.Clear();
      }

      public void AddRemovedBaseSkillEffect(BL.SkillEffect effect)
      {
        this.removedBaseSkillEffects.Add(effect);
      }

      public void AddRemovedFixEffectParam(
        Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int> param)
      {
        this.removedFixEffectParams.Add(param);
      }

      public void AddRemovedRatioEffectParam(
        Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> param)
      {
        this.removedRatioEffectParams.Add(param);
      }

      public List<BL.SkillEffect> GetAllRemovedBaseSkillEffects() => this.removedBaseSkillEffects;

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>> GetAllRemovedFixEffectParams()
      {
        return this.removedFixEffectParams;
      }

      public List<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> GetAllRemovedRatioEffectParams()
      {
        return this.removedRatioEffectParams;
      }

      public int GetDuelSkillEffectIdInvokeCount(int skillEffectId)
      {
        return !this.duelSkillEffectIdInvokeCount.ContainsKey(skillEffectId) ? 0 : this.duelSkillEffectIdInvokeCount[skillEffectId];
      }

      public int GetDuelSkillIdInvokeCount(int skillId)
      {
        return !this.duelSkillIdInvokeCount.ContainsKey(skillId) ? 0 : this.duelSkillIdInvokeCount[skillId];
      }

      public int GetDuelSkillIdInvokeCount2(int skillId)
      {
        return !this.duelSkillIdInvokeCount2.ContainsKey(skillId) ? 0 : this.duelSkillIdInvokeCount2[skillId];
      }

      public void AddDuelSkillEffectIdInvokeCount(int skillEffectId, int addCount)
      {
        if (!this.duelSkillEffectIdInvokeCount.ContainsKey(skillEffectId))
          this.duelSkillEffectIdInvokeCount[skillEffectId] = 0;
        this.duelSkillEffectIdInvokeCount[skillEffectId] += addCount;
        if (!MasterData.BattleskillEffect.ContainsKey(skillEffectId))
          return;
        BattleskillEffect battleskillEffect = MasterData.BattleskillEffect[skillEffectId];
        BattleskillSkill skill = battleskillEffect.skill;
        int id = skill.ID;
        if (!this.duelSkillIdInvokeCount.ContainsKey(id))
          this.duelSkillIdInvokeCount[id] = 0;
        this.duelSkillIdInvokeCount[id] += addCount;
        if (!battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gda_percentage_invocation) && !battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation))
          return;
        bool flag = false;
        for (int index = 0; index < skill.Effects.Length; ++index)
        {
          BattleskillEffect effect = skill.Effects[index];
          if (effect.HasKey(BattleskillEffectLogicArgumentEnum.gda_percentage_invocation) || effect.HasKey(BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation))
          {
            if (flag)
              break;
            if (effect.ID == skillEffectId)
              flag = true;
          }
          else if (flag && effect.HasKey(BattleskillEffectLogicArgumentEnum.start_total_count2))
          {
            if (!this.duelSkillIdInvokeCount2.ContainsKey(id))
              this.duelSkillIdInvokeCount2[id] = 0;
            this.duelSkillIdInvokeCount2[id] += addCount;
            break;
          }
        }
      }

      public void SetDuelSkillEffectIdInvokeCount(int skillEffectId, int count)
      {
        this.duelSkillEffectIdInvokeCount[skillEffectId] = count;
      }

      public void SetDuelSkillIdInvokeCount(int skillId, int count)
      {
        this.duelSkillIdInvokeCount[skillId] = count;
      }

      public void SetDuelSkillIdInvokeCount2(int skillId, int count)
      {
        this.duelSkillIdInvokeCount2[skillId] = count;
      }

      public IEnumerable<KeyValuePair<int, int>> GetAllDuelSkillEffectIdInvokeCount()
      {
        foreach (KeyValuePair<int, int> keyValuePair in this.duelSkillEffectIdInvokeCount)
          yield return keyValuePair;
      }

      public IEnumerable<KeyValuePair<int, int>> GetAllDuelSkillIdInvokeCount()
      {
        foreach (KeyValuePair<int, int> keyValuePair in this.duelSkillIdInvokeCount)
          yield return keyValuePair;
      }

      public IEnumerable<KeyValuePair<int, int>> GetAllDuelSkillIdInvokeCount2()
      {
        foreach (KeyValuePair<int, int> keyValuePair in this.duelSkillIdInvokeCount2)
          yield return keyValuePair;
      }

      private int GetOverwriteEffectPriority(BL.SkillEffect effect)
      {
        return effect.isBaseSkill ? (effect.gearIndex != 1 ? (effect.gearIndex < 2 ? 0 : 1) : 2) : 3;
      }

      private bool CheckAddOverwriteEffects(BL.SkillEffect addEffect)
      {
        bool flag = false;
        if (addEffect.effect.enchant_type.HasValue)
        {
          int overwriteEffectPriority1 = this.GetOverwriteEffectPriority(addEffect);
          List<BL.SkillEffect> skillEffectList = (List<BL.SkillEffect>) null;
          foreach (BL.SkillEffect effect in this.effects)
          {
            if (effect.effect.enchant_type.HasValue)
            {
              if (addEffect.isBaseSkill)
              {
                int overwriteEffectPriority2 = this.GetOverwriteEffectPriority(effect);
                if (overwriteEffectPriority2 < overwriteEffectPriority1)
                {
                  if (skillEffectList == null)
                    skillEffectList = new List<BL.SkillEffect>();
                  skillEffectList.Add(effect);
                }
                else if (overwriteEffectPriority2 > overwriteEffectPriority1)
                  flag = true;
              }
              else if (addEffect.baseSkill.ID != effect.baseSkill.ID)
              {
                if (skillEffectList == null)
                  skillEffectList = new List<BL.SkillEffect>();
                skillEffectList.Add(effect);
              }
            }
          }
          if (flag && addEffect.isBaseSkill)
            this.removedOverwriteSkillEffects.Add(addEffect);
          if (skillEffectList != null)
          {
            foreach (BL.SkillEffect skillEffect in skillEffectList)
            {
              if (skillEffect.isBaseSkill)
                this.removedOverwriteSkillEffects.Add(skillEffect);
              this.effects.Remove(skillEffect);
            }
            this.ClearCache();
          }
        }
        return flag;
      }

      private IEnumerable<BL.SkillEffect> RecoveryOverwriteEffects()
      {
        BL.SkillEffectList skillEffectList1 = this;
        if (skillEffectList1.removedOverwriteSkillEffects.Count > 0 && !skillEffectList1.effects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.enchant_type.HasValue)))
        {
          BL.SkillEffectList skillEffectList = skillEffectList1;
          // ISSUE: reference to a compiler-generated method
          int maxPrio = skillEffectList1.removedOverwriteSkillEffects.Max<BL.SkillEffect>(new Func<BL.SkillEffect, int>(skillEffectList1.\u003CRecoveryOverwriteEffects\u003Eb__120_1));
          BL.SkillEffect[] skillEffectArray = skillEffectList1.removedOverwriteSkillEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => skillEffectList.GetOverwriteEffectPriority(x) == maxPrio)).ToArray<BL.SkillEffect>();
          for (int index = 0; index < skillEffectArray.Length; ++index)
          {
            BL.SkillEffect skillEffect = skillEffectArray[index];
            skillEffectList1.removedOverwriteSkillEffects.Remove(skillEffect);
            yield return skillEffect;
          }
          skillEffectArray = (BL.SkillEffect[]) null;
        }
      }

      public void AddRemovedOverwriteSkillEffect(BL.SkillEffect effect)
      {
        this.removedOverwriteSkillEffects.Add(effect);
      }

      public List<BL.SkillEffect> GetAllRemovedOverwriteSkillEffects()
      {
        return this.removedOverwriteSkillEffects;
      }

      public void AddWaitingTransformationSkillEffect(BL.SkillEffect effect)
      {
        this.waitingTransformationSkillEffects.Add(effect);
      }

      public List<BL.SkillEffect> GetAllWaitingTransformationSkillEffects()
      {
        return this.waitingTransformationSkillEffects;
      }

      public void ResetTransformationSkillEffects(int transformationGroupId)
      {
        List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
        bool flag = false;
        foreach (BL.SkillEffect effect in this.effects)
        {
          if (effect.baseSkill.skill_type == BattleskillSkillType.passive && effect.isBaseSkill && effect.baseSkill.transformationGroupId.HasValue)
            skillEffectList.Add(effect);
        }
        foreach (BL.SkillEffect skillEffect in skillEffectList)
        {
          int? transformationGroupId1 = skillEffect.baseSkill.transformationGroupId;
          int num = transformationGroupId;
          if (!(transformationGroupId1.GetValueOrDefault() == num & transformationGroupId1.HasValue))
          {
            this.effects.Remove(skillEffect);
            this.waitingTransformationSkillEffects.Add(skillEffect);
            flag = true;
          }
        }
        skillEffectList.Clear();
        foreach (BL.SkillEffect transformationSkillEffect in this.waitingTransformationSkillEffects)
        {
          int? transformationGroupId2 = transformationSkillEffect.baseSkill.transformationGroupId;
          int num = transformationGroupId;
          if (transformationGroupId2.GetValueOrDefault() == num & transformationGroupId2.HasValue)
            skillEffectList.Add(transformationSkillEffect);
        }
        foreach (BL.SkillEffect skillEffect in skillEffectList)
        {
          this.effects.Add(skillEffect);
          this.waitingTransformationSkillEffects.Remove(skillEffect);
          flag = true;
        }
        if (!flag)
          return;
        this.ClearCache();
        ++this.revision;
      }

      public bool IsMoveSkillActionWaiting()
      {
        return this.Where(BattleskillEffectLogicEnum.do_not_move, (Func<BL.SkillEffect, bool>) (x => x.baseSkillId == 300001381)).Any<BL.SkillEffect>();
      }

      public bool HasEnhancedElement()
      {
        if (!this.hasEnhancedElement.HasValue)
          this.ResetHasEnhancedElement();
        return this.hasEnhancedElement.Value;
      }

      public void ResetHasEnhancedElement()
      {
        this.InitEffects();
        this.hasEnhancedElement = new bool?(this.effectDic.ContainsKey(1002017));
      }
    }

    public class ExecuteSkillEffectResult
    {
      public BattleskillSkill skill;
      public List<BL.UnitPosition> targets = new List<BL.UnitPosition>();
      public List<int> target_hps = new List<int>();
      public List<int> target_prev_hps = new List<int>();
      public List<bool> disp_target_hps = new List<bool>();
      public List<BL.Unit> second_targets = new List<BL.Unit>();
      public List<int> second_target_hps = new List<int>();
      public List<int> second_target_prev_hps = new List<int>();
      public Vector2? invokedEffectVector;
      public List<BL.Panel> targetPanels = new List<BL.Panel>();
    }

    public class SkillResult
    {
      public BL _bl;
      public bool _target_ai;

      public SkillResult(BL bl) => this._bl = bl;

      public virtual void run(bool target_ai)
      {
      }
    }

    public class SkillResultUnit : BL.SkillResult
    {
      public BL.UnitPosition _up;
      public BL.UnitPosition _up_target;
      public bool _is_reinforcement;

      public SkillResultUnit(BL.UnitPosition up, BL bl)
        : base(bl)
      {
        this._up = up;
      }

      public virtual void respawnReinforcement() => this._up_target.respawnReinforcement(this._bl);

      private BL.UnitPosition getTarget()
      {
        if (!this._target_ai)
          return this._up;
        if (this._up.unit.isEnable)
          return (BL.UnitPosition) this._bl.aiUnitPositions.value.Find((Predicate<BL.AIUnit>) (x => x.unit == this._up.unit));
        BL.UnitPosition target = (BL.UnitPosition) new BL.AIUnit(this._up, BL.AIType.normal);
        this._bl.aiUnitPositions.value.Add(target as BL.AIUnit);
        return target;
      }

      public override void run(bool target_ai)
      {
        this._target_ai = target_ai;
        this._up_target = this.getTarget();
        if (this._is_reinforcement)
        {
          BattleStageEnemy battleStageEnemy = Array.Find<BattleStageEnemy>(this._bl.battleInfo.Enemies, (Predicate<BattleStageEnemy>) (x => x.ID == this._up_target.unit.specificId));
          this._bl.resetUnitStatus(this._up_target, battleStageEnemy.initial_coordinate_y - 1, battleStageEnemy.initial_coordinate_x - 1, battleStageEnemy.initial_direction);
        }
        if (!this._is_reinforcement)
          return;
        this.respawnReinforcement();
      }
    }

    public class UseSkillWithResult
    {
      public List<BL.Unit> effectTargets;
      public List<BL.Unit> displayNumberTargets;
      public bool isDamage;
      public BL.Unit dispHpUnit;
      public int prevHp;
      public bool lateDispHp;
      public List<BL.Panel> effectPanelTargets;
      public List<BL.Unit> createFacilities;
      public List<BL.Unit> destructFacilities;
    }

    public abstract class FacilitySkillLogicEffect
    {
      protected BL env;
      protected BattleFuncs.PackedSkillEffect pse;
      protected BL.ForceID[] myForce;
      protected BL.ForceID[] targetForce;
      protected BL.UnitPosition up;
      protected BL.ISkillEffectListUnit unit;
      protected bool isAI;
      protected object extData;
      protected BL.UnitPosition tup;
      protected BL.ISkillEffectListUnit target;
      protected BL.Panel targetPanel;
      protected BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects;
      protected BL.ISkillEffectListUnit moveUnit;
      protected Judgement.BattleParameter battleParameter;
      protected List<BL.UnitPosition> targets;

      public Vector2? invokedEffectVector { get; protected set; }

      public void init(
        BL env,
        BattleFuncs.PackedSkillEffect pse,
        BL.ForceID[] myForce,
        BL.ForceID[] targetForce,
        BL.UnitPosition up,
        BL.ISkillEffectListUnit unit,
        bool isAI,
        object extData)
      {
        this.env = env;
        this.pse = pse;
        this.myForce = myForce;
        this.targetForce = targetForce;
        this.up = up;
        this.unit = unit;
        this.isAI = isAI;
        this.extData = extData;
        this.invokedEffectVector = new Vector2?();
      }

      public bool damageBySwapHeal { get; protected set; }

      public void execute(
        BL.UnitPosition tup,
        BL.ISkillEffectListUnit target,
        BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects,
        BL.Panel targetPanel,
        BL.ISkillEffectListUnit moveUnit,
        Judgement.BattleParameter battleParameter,
        List<BL.UnitPosition> targets)
      {
        this.tup = tup;
        this.target = target;
        this.targetPanel = targetPanel;
        this.applyChangeSkillEffects = applyChangeSkillEffects;
        this.moveUnit = moveUnit;
        this.battleParameter = battleParameter;
        this.targets = targets;
        this.damageBySwapHeal = false;
        this.executeEffect();
      }

      public abstract BattleskillEffectLogicEnum logicEnum();

      public virtual IEnumerable<BL.UnitPosition> getTargets()
      {
        yield break;
      }

      public virtual IEnumerable<BL.Panel> getPanelTargets()
      {
        yield break;
      }

      public abstract BL.FacilitySkillLogicEffect.Category category();

      protected abstract void executeEffect();

      public abstract bool isDispHpNumber();

      public virtual bool checkMoveUnitInvoke(BL.UnitPosition upMove) => true;

      public virtual bool isCheckSeal() => true;

      public virtual bool isPanelTarget() => false;

      protected bool checkMoveUnitInvokeDistance(BL.UnitPosition upMove, BL.ForceID[] invokeForceId)
      {
        if (!((IEnumerable<BL.ForceID>) invokeForceId).Contains<BL.ForceID>(this.env.getForceID(upMove.unit)))
          return false;
        int num = BL.fieldDistance(this.up, upMove);
        return num >= this.pse.GetInt(BattleskillEffectLogicArgumentEnum.invoke_min_range) && num <= this.pse.GetInt(BattleskillEffectLogicArgumentEnum.invoke_max_range);
      }

      protected IEnumerable<BL.UnitPosition> getRangeTargets(
        BL.ForceID[] forceId,
        BL.UnitPosition fromUp)
      {
        int range_form = !this.pse.HasKey(BattleskillEffectLogicArgumentEnum.range_form) ? 0 : this.pse.GetInt(BattleskillEffectLogicArgumentEnum.range_form);
        if (range_form == 3)
        {
          if (this.pse.HasKey(BattleskillEffectLogicArgumentEnum.penetrate_is_range_from_target))
          {
            int num1 = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_vector_x);
            int num2 = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_vector_y);
            int minRange = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
            int maxRange = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
            int radius = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_radius);
            bool isRangeFromTarget = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_is_range_from_target) == 1;
            List<BL.UnitPosition> unitPositionList = (List<BL.UnitPosition>) null;
            int num3 = int.MinValue;
            int num4 = 0;
            for (int index = 0; index < 4; ++index)
            {
              int num5;
              int num6;
              switch (index)
              {
                case 0:
                  num5 = num1;
                  num6 = num2;
                  break;
                case 1:
                  num5 = num2;
                  num6 = -num1;
                  break;
                case 2:
                  num5 = -num1;
                  num6 = -num2;
                  break;
                default:
                  num5 = -num2;
                  num6 = num1;
                  break;
              }
              HashSet<Tuple<int, int>> laserPosition = BattleFuncs.getLaserPosition(fromUp.row, fromUp.column, fromUp.row + num6, fromUp.column + num5, minRange, maxRange, radius, isRangeFromTarget);
              List<BL.UnitPosition> source = new List<BL.UnitPosition>();
              foreach (Tuple<int, int> tuple in laserPosition)
              {
                BL.UnitPosition[] unitPositionArray = this.isAI ? BattleFuncs.getFieldUnitsAI(tuple.Item1, tuple.Item2) : this.env.getFieldUnits(tuple.Item1, tuple.Item2);
                if (unitPositionArray != null)
                {
                  foreach (BL.UnitPosition up in unitPositionArray)
                  {
                    if (BattleFuncs.checkTargetp(BattleFuncs.unitPositionToISkillEffectListUnit(up), forceId, BL.Unit.TargetAttribute.all, true))
                      source.Add(up);
                  }
                }
              }
              if (source.Count != 0 && source.Count >= num3)
              {
                int num7 = source.Sum<BL.UnitPosition>((Func<BL.UnitPosition, int>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x).hp));
                if (source.Count != num3 || num7 < num4)
                {
                  unitPositionList = source;
                  num3 = source.Count;
                  num4 = num7;
                  if (!this.isAI)
                    this.invokedEffectVector = new Vector2?(new Vector2((float) num5, (float) num6));
                }
              }
            }
            if (unitPositionList != null)
            {
              foreach (BL.UnitPosition rangeTarget in unitPositionList)
                yield return rangeTarget;
            }
          }
        }
        else
        {
          int[] range = new int[2]
          {
            this.pse.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
            this.pse.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
          };
          foreach (BL.UnitPosition rangeTarget in ((IEnumerable<BL.ForceID>) forceId).SelectMany<BL.ForceID, BL.UnitPosition>((Func<BL.ForceID, IEnumerable<BL.UnitPosition>>) (x => (IEnumerable<BL.UnitPosition>) this.env.getExecuteSkillEffectsTargets(fromUp, range, x, this.isAI))))
          {
            switch (range_form)
            {
              case 1:
                if (fromUp.row == rangeTarget.row || fromUp.column == rangeTarget.column)
                  break;
                continue;
              case 2:
                if (Mathf.Abs(fromUp.row - rangeTarget.row) != Mathf.Abs(fromUp.column - rangeTarget.column))
                  continue;
                break;
            }
            yield return rangeTarget;
          }
        }
      }

      protected IEnumerable<BL.UnitPosition> getRangeTargets(BL.ForceID[] forceId)
      {
        return this.getRangeTargets(forceId, this.up);
      }

      protected IEnumerable<BL.Panel> getRangePanelTargets()
      {
        int range_form = !this.pse.HasKey(BattleskillEffectLogicArgumentEnum.range_form) ? 0 : this.pse.GetInt(BattleskillEffectLogicArgumentEnum.range_form);
        int[] range = new int[2]
        {
          this.pse.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
          this.pse.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
        };
        foreach (BL.Panel rangePanel in BattleFuncs.getRangePanels(this.up.row, this.up.column, range))
        {
          switch (range_form)
          {
            case 1:
              if (this.up.row == rangePanel.row || this.up.column == rangePanel.column)
                break;
              continue;
            case 2:
              if (Mathf.Abs(this.up.row - rangePanel.row) != Mathf.Abs(this.up.column - rangePanel.column))
                continue;
              break;
          }
          yield return rangePanel;
        }
      }

      private int calcRatioValue()
      {
        return (int) Math.Ceiling((Decimal) ((float) this.tup.unit.parameter.Hp * (this.pse.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) this.pse.skillEffect.baseSkillLevel * this.pse.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio))));
      }

      private int calcFixValue()
      {
        return this.pse.GetInt(BattleskillEffectLogicArgumentEnum.value) + this.pse.skillEffect.baseSkillLevel * this.pse.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
      }

      private void addTargetHp(int addHp, int damageCutType = -1)
      {
        int num = this.pse.HasKey(BattleskillEffectLogicArgumentEnum.min_hp) ? this.pse.GetInt(BattleskillEffectLogicArgumentEnum.min_hp) : 0;
        if (addHp > 0)
        {
          addHp = BattleFuncs.getHealValue(this.target, BattleFuncs.getPanel(this.tup.row, this.tup.column), addHp, this.pse.skillEffect.baseSkill.skill_type);
          if (addHp < 0)
            this.damageBySwapHeal = true;
        }
        else if (damageCutType != -1)
          addHp = -BattleFuncs.applyDamageCut(damageCutType, -addHp, this.target, this.unit, invokePanel: BattleFuncs.getPanel(this.tup.row, this.tup.column));
        this.target.hp += addHp;
        if (this.target.hp >= num)
          return;
        this.target.hp = num;
      }

      protected void executeRatioHealEffect() => this.addTargetHp(this.calcRatioValue());

      protected void executeFixHealEffect() => this.addTargetHp(this.calcFixValue());

      protected void executeRatioDamageEffect(int damageCutType = -1)
      {
        this.addTargetHp(-this.calcRatioValue(), damageCutType);
      }

      protected void executeFixDamageEffect(int damageCutType = -1)
      {
        this.addTargetHp(-this.calcFixValue(), damageCutType);
      }

      protected void executeInvestSkillEffect(BL.Unit investUnit = null)
      {
        int num1 = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
        if (num1 == 0 || !MasterData.BattleskillSkill.ContainsKey(num1))
          return;
        BL.Skill[] skillArray = (BL.Skill[]) null;
        bool flag = MasterData.BattleskillSkill[num1].skill_type == BattleskillSkillType.ailment;
        if (flag)
        {
          if (this.up.unit.isFacility && !this.up.unit.facility.isSkillUnit)
          {
            skillArray = BattleFuncs.ailmentInvest(num1, this.target);
          }
          else
          {
            BL.SkillEffect perfectAilmentResist = BattleFuncs.getPerfectAilmentResist(BattleFuncs.getAilmentResistEffects(num1, this.target, this.unit));
            if (perfectAilmentResist != null)
            {
              if (perfectAilmentResist.useRemain.HasValue)
              {
                BL.SkillEffect skillEffect = perfectAilmentResist;
                int? useRemain1 = skillEffect.useRemain;
                skillEffect.useRemain = useRemain1.HasValue ? new int?(useRemain1.GetValueOrDefault() - 1) : new int?();
                if (!this.isAI)
                {
                  int? useRemain2 = perfectAilmentResist.useRemain;
                  int num2 = 0;
                  if (useRemain2.GetValueOrDefault() == num2 & useRemain2.HasValue)
                    this.target.originalUnit.commit();
                }
              }
            }
            else
              skillArray = BattleFuncs.ailmentInvest(num1, this.target);
          }
        }
        else
          skillArray = new BL.Skill[1]
          {
            new BL.Skill() { id = num1 }
          };
        BL.SkillEffect[] skillEffectArray = (BL.SkillEffect[]) null;
        if (flag && !this.up.unit.isFacility)
          skillEffectArray = BattleFuncs.getAilmentTriggerSkillEffects(num1, this.target, this.unit, BattleFuncs.getPanel(this.tup.row, this.tup.column)).ToArray<BL.SkillEffect>();
        if (skillArray != null)
        {
          foreach (BL.Skill skill in skillArray)
          {
            foreach (BattleskillEffect effect in skill.skill.Effects)
            {
              if (!this.isAI)
                this.target.originalUnit.commit();
              if (investUnit == (BL.Unit) null)
                investUnit = this.up.unit;
              this.target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1, investUnit: investUnit, investSkillId: this.pse.skillEffect.baseSkillId, investTurn: this.env.phaseState.absoluteTurnCount), checkEnableUnit: this.target);
            }
          }
        }
        if (skillEffectArray == null)
          return;
        foreach (BL.SkillEffect skillEffect in skillEffectArray)
        {
          int key = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
          if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
          {
            BattleskillSkill skill = MasterData.BattleskillSkill[key];
            foreach (BattleskillEffect effect in skill.Effects)
              this.target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, 1, investUnit: this.target.originalUnit, investSkillId: skillEffect.baseSkillId, investTurn: this.env.phaseState.absoluteTurnCount), checkEnableUnit: this.target);
            if (!this.isAI)
              this.target.originalUnit.commit();
          }
        }
      }

      protected void executeRemoveSkillEffect()
      {
        BattleFuncs.removeSkillEffect(this.pse.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), this.pse.HasKey(BattleskillEffectLogicArgumentEnum.skill_id) ? this.pse.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) : 0, this.pse.HasKey(BattleskillEffectLogicArgumentEnum.skill_type) ? this.pse.GetInt(BattleskillEffectLogicArgumentEnum.skill_type) : 0, this.pse.HasKey(BattleskillEffectLogicArgumentEnum.invest_type) ? this.pse.GetInt(BattleskillEffectLogicArgumentEnum.invest_type) : 0, this.pse.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? this.pse.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0, this.pse.HasKey(BattleskillEffectLogicArgumentEnum.range_effect_remove_flag) ? this.pse.GetInt(BattleskillEffectLogicArgumentEnum.range_effect_remove_flag) : 0, this.target, this.applyChangeSkillEffects, this.unit, this.pse.skillEffect?.effect, this.moveUnit);
      }

      protected void executeImmediateAttackEffect(int damageCutType = -1)
      {
        this.addTargetHp(-BattleFuncs.calcAttackDamage(this.unit, this.target, this.pse.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack), this.pse.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease), this.pse.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage)), damageCutType);
      }

      protected void executeAddLandTag()
      {
        int key = this.pse.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
        if (key == 0 || !MasterData.BattleskillSkill.ContainsKey(key))
          return;
        BL.Skill skill = new BL.Skill() { id = key };
        BattleskillEffect effect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag));
        if (effect == null)
          return;
        BL.ClassValue<List<BL.SkillEffect>> skillEffects = this.targetPanel.getSkillEffects(this.isAI);
        if (skillEffects.value.RemoveAll((Predicate<BL.SkillEffect>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag)) >= 1)
          skillEffects.commit();
        this.targetPanel.addSkillEffect(BL.SkillEffect.FromMasterData(effect, skill.skill, 1, investUnit: this.unit.originalUnit, investSkillId: this.pse.skillEffect.baseSkillId, investTurn: this.env.phaseState.absoluteTurnCount), this.unit);
        if (this.isAI)
          return;
        BL.UnitPosition[] fieldUnits = this.env.getFieldUnits(this.targetPanel.row, this.targetPanel.column);
        if (fieldUnits != null)
        {
          foreach (BL.UnitPosition unitPosition in fieldUnits)
            unitPosition.unit.skillEffects.LandTagModified.commit();
        }
        if (this.env.fieldCurrent.value != this.targetPanel)
          return;
        this.env.fieldCurrent.commit();
      }

      protected void executeStealEffect()
      {
        BL.SkillEffect skillEffect = this.pse.skillEffect;
        if (skillEffect == null)
          return;
        BattleFuncs.executeSteal(this.unit, this.target, skillEffect.effect, this.up, this.tup, this.isAI);
      }

      protected void executeProvideEffect()
      {
        BL.SkillEffect skillEffect = this.pse.skillEffect;
        if (skillEffect == null)
          return;
        BattleFuncs.executeProvide(this.unit, this.target, skillEffect.effect, this.up, this.tup, this.isAI, this.targets.Count, this.battleParameter, this.targets.IndexOf(this.tup) == 0);
      }

      public enum Category
      {
        heal = 1,
        attack = 2,
        investPlayer = 3,
        investEnemy = 4,
        removePlayer = 5,
        removeEnemy = 6,
        investLandTag = 7,
        stealPlayer = 8,
        stealEnemy = 9,
        providePlayer = 10, // 0x0000000A
        provideEnemy = 11, // 0x0000000B
      }
    }

    private class FacilitySkillLogicEffectRangeEffectRatioHeal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_ratio_heal;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.heal;
      }

      protected override void executeEffect() => this.executeRatioHealEffect();

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectRangeEffectFixHeal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_fix_heal;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.heal;
      }

      protected override void executeEffect() => this.executeFixHealEffect();

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectRangeEffectRatioAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_ratio_attack;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect() => this.executeRatioDamageEffect(6);

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectRangeEffectFixAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_fix_attack;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect() => this.executeFixDamageEffect(6);

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectRangeEffectEnemyInvestSkilleffect : 
      BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_enemy_invest_skilleffect;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investEnemy;
      }

      protected override void executeEffect() => this.executeInvestSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectPlayerInvestSkilleffect : 
      BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_player_invest_skilleffect;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investPlayer;
      }

      protected override void executeEffect() => this.executeInvestSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectEnemyRemoveSkilleffect : 
      BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_enemy_remove_skilleffect;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.removeEnemy;
      }

      protected override void executeEffect() => this.executeRemoveSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectPlayerRemoveSkilleffect : 
      BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_player_remove_skilleffect;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.removePlayer;
      }

      protected override void executeEffect() => this.executeRemoveSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectInvestLandTag : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_invest_land_tag;
      }

      public override IEnumerable<BL.Panel> getPanelTargets() => this.getRangePanelTargets();

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investLandTag;
      }

      protected override void executeEffect() => this.executeAddLandTag();

      public override bool isDispHpNumber() => false;

      public override bool isPanelTarget() => true;
    }

    private class FacilitySkillLogicEffectRangeEffectEnemySteal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_enemy_steal;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.stealEnemy;
      }

      protected override void executeEffect() => this.executeStealEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectPlayerSteal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_player_steal;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.stealPlayer;
      }

      protected override void executeEffect() => this.executeStealEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectEnemyProvide : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_enemy_provide;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.provideEnemy;
      }

      protected override void executeEffect() => this.executeProvideEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectRangeEffectPlayerProvide : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.range_effect_player_provide;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.providePlayer;
      }

      protected override void executeEffect() => this.executeProvideEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapRatioAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_ratio_attack;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect() => this.executeRatioDamageEffect(7);

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrapFixAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_fix_attack;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect() => this.executeFixDamageEffect(7);

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrapRatioHeal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_ratio_heal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.heal;
      }

      protected override void executeEffect() => this.executeRatioHealEffect();

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrapFixHeal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_fix_heal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.heal;
      }

      protected override void executeEffect() => this.executeFixHealEffect();

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrapEnemyInvestSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_enemy_invest_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investEnemy;
      }

      protected override void executeEffect() => this.executeInvestSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapPlayerInvestSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_player_invest_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investPlayer;
      }

      protected override void executeEffect() => this.executeInvestSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapEnemyRemoveSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_enemy_remove_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.removeEnemy;
      }

      protected override void executeEffect() => this.executeRemoveSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapPlayerRemoveSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_player_remove_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.removePlayer;
      }

      protected override void executeEffect() => this.executeRemoveSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapInvestLandTag : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_invest_land_tag;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.Panel> getPanelTargets() => this.getRangePanelTargets();

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investLandTag;
      }

      protected override void executeEffect() => this.executeAddLandTag();

      public override bool isDispHpNumber() => false;

      public override bool isPanelTarget() => true;
    }

    private class FacilitySkillLogicEffectTrapEnemySteal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_enemy_steal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.stealEnemy;
      }

      protected override void executeEffect() => this.executeStealEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapPlayerSteal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_player_steal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.stealPlayer;
      }

      protected override void executeEffect() => this.executeStealEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapEnemyProvide : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_enemy_provide;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.provideEnemy;
      }

      protected override void executeEffect() => this.executeProvideEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrapPlayerProvide : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap_player_provide;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.providePlayer;
      }

      protected override void executeEffect() => this.executeProvideEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2RatioAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_ratio_attack;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect() => this.executeRatioDamageEffect(7);

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrap2FixAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_fix_attack;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect() => this.executeFixDamageEffect(7);

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrap2RatioHeal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_ratio_heal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.heal;
      }

      protected override void executeEffect() => this.executeRatioHealEffect();

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrap2FixHeal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_fix_heal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.heal;
      }

      protected override void executeEffect() => this.executeFixHealEffect();

      public override bool isDispHpNumber() => true;
    }

    private class FacilitySkillLogicEffectTrap2EnemyInvestSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_enemy_invest_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investEnemy;
      }

      protected override void executeEffect() => this.executeInvestSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2PlayerInvestSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_player_invest_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investPlayer;
      }

      protected override void executeEffect() => this.executeInvestSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2EnemyRemoveSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_enemy_remove_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.removeEnemy;
      }

      protected override void executeEffect() => this.executeRemoveSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2PlayerRemoveSkilleffect : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_player_remove_skilleffect;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.removePlayer;
      }

      protected override void executeEffect() => this.executeRemoveSkillEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2InvestLandTag : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_invest_land_tag;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.Panel> getPanelTargets() => this.getRangePanelTargets();

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investLandTag;
      }

      protected override void executeEffect() => this.executeAddLandTag();

      public override bool isDispHpNumber() => false;

      public override bool isPanelTarget() => true;
    }

    private class FacilitySkillLogicEffectTrap2EnemySteal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_enemy_steal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.stealEnemy;
      }

      protected override void executeEffect() => this.executeStealEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2PlayerSteal : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_player_steal;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.stealPlayer;
      }

      protected override void executeEffect() => this.executeStealEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2EnemyProvide : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_enemy_provide;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.myForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.provideEnemy;
      }

      protected override void executeEffect() => this.executeProvideEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectTrap2PlayerProvide : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.trap2_player_provide;
      }

      public override bool checkMoveUnitInvoke(BL.UnitPosition upMove)
      {
        return this.checkMoveUnitInvokeDistance(upMove, this.targetForce);
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.providePlayer;
      }

      protected override void executeEffect() => this.executeProvideEffect();

      public override bool isDispHpNumber() => false;
    }

    private class FacilitySkillLogicEffectJumpEffectImmediateAttack : BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.jump_effect_immediate_attack;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce, (BL.UnitPosition) this.extData);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.attack;
      }

      protected override void executeEffect()
      {
        int damageCutType = -1;
        BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[this.pse.skillEffect.investSkillId];
        if (battleskillSkill.skill_type == BattleskillSkillType.command)
          damageCutType = 4;
        else if (battleskillSkill.skill_type == BattleskillSkillType.release)
          damageCutType = 5;
        this.executeImmediateAttackEffect(damageCutType);
      }

      public override bool isDispHpNumber() => true;

      public override bool isCheckSeal() => false;
    }

    private class FacilitySkillLogicEffectJumpEffectEnemyInvestSkilleffect : 
      BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.jump_effect_enemy_invest_skilleffect;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.targetForce, (BL.UnitPosition) this.extData);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investEnemy;
      }

      protected override void executeEffect()
      {
        this.executeInvestSkillEffect(this.pse.skillEffect != null ? this.pse.skillEffect.investUnit : (BL.Unit) null);
      }

      public override bool isDispHpNumber() => false;

      public override bool isCheckSeal() => false;
    }

    private class FacilitySkillLogicEffectJumpEffectPlayerInvestSkilleffect : 
      BL.FacilitySkillLogicEffect
    {
      public override BattleskillEffectLogicEnum logicEnum()
      {
        return BattleskillEffectLogicEnum.jump_effect_player_invest_skilleffect;
      }

      public override IEnumerable<BL.UnitPosition> getTargets()
      {
        return this.getRangeTargets(this.myForce, (BL.UnitPosition) this.extData);
      }

      public override BL.FacilitySkillLogicEffect.Category category()
      {
        return BL.FacilitySkillLogicEffect.Category.investPlayer;
      }

      protected override void executeEffect()
      {
        this.executeInvestSkillEffect(this.pse.skillEffect != null ? this.pse.skillEffect.investUnit : (BL.Unit) null);
      }

      public override bool isDispHpNumber() => false;

      public override bool isCheckSeal() => false;
    }

    public class BattleSkillResultNetwork : ActionResultNetwork
    {
      public int? mSkillID;
      public int? mInvocation;
      public List<int> mTargets;
      public List<int> mPanelsRow;
      public List<int> mPanelsColumn;
      public XorShift mRandom;
    }

    [Serializable]
    public class BattleSkillResult : ActionResult
    {
      [SerializeField]
      private BL.Skill mSkill;
      [SerializeField]
      private BL.Unit mInvocation;
      [SerializeField]
      private List<BL.Unit> mTargets;
      [SerializeField]
      private List<BL.Panel> mPanels;
      [SerializeField]
      private XorShift mRandom;

      public override ActionResultNetwork ToNetworkLocal(BL env)
      {
        List<int> intList1 = new List<int>();
        if (this.mTargets != null)
        {
          foreach (BL.Unit mTarget in this.mTargets)
          {
            int? network = mTarget.ToNetwork(env);
            if (network.HasValue)
              intList1.Add(network.Value);
          }
        }
        List<int> intList2 = new List<int>();
        List<int> intList3 = new List<int>();
        if (this.mPanels != null)
        {
          foreach (BL.Panel mPanel in this.mPanels)
          {
            intList2.Add(mPanel.row);
            intList3.Add(mPanel.column);
          }
        }
        return (ActionResultNetwork) new BL.BattleSkillResultNetwork()
        {
          mSkillID = new int?(this.mSkill.id),
          mInvocation = (this.mInvocation == (BL.Unit) null ? new int?() : this.mInvocation.ToNetwork(env)),
          mTargets = intList1,
          mPanelsRow = intList2,
          mPanelsColumn = intList3,
          mRandom = this.mRandom
        };
      }

      public static ActionResult FromNetwork(ActionResultNetwork nnw, BL env)
      {
        BL.BattleSkillResultNetwork nw = nnw as BL.BattleSkillResultNetwork;
        if (nw == null)
          return (ActionResult) null;
        BL.Unit unit = BL.Unit.FromNetwork(nw.mInvocation, env);
        BL.Skill skill;
        if (unit.hasOugi)
        {
          int id = unit.ougi.id;
          int? mSkillId = nw.mSkillID;
          int valueOrDefault = mSkillId.GetValueOrDefault();
          if (id == valueOrDefault & mSkillId.HasValue)
          {
            skill = unit.ougi;
            goto label_9;
          }
        }
        if (unit.hasSEASkill)
        {
          int id = unit.SEASkill.id;
          int? mSkillId = nw.mSkillID;
          int valueOrDefault = mSkillId.GetValueOrDefault();
          if (id == valueOrDefault & mSkillId.HasValue)
          {
            skill = unit.SEASkill;
            goto label_9;
          }
        }
        skill = ((IEnumerable<BL.Skill>) unit.skills).Where<BL.Skill>((Func<BL.Skill, bool>) (p =>
        {
          int id = p.id;
          int? mSkillId = nw.mSkillID;
          int valueOrDefault = mSkillId.GetValueOrDefault();
          return id == valueOrDefault & mSkillId.HasValue;
        })).FirstOrDefault<BL.Skill>();
label_9:
        if (skill == null)
          return (ActionResult) null;
        return (ActionResult) new BL.BattleSkillResult()
        {
          mSkill = skill,
          mInvocation = unit,
          mTargets = nw.mTargets.Select<int, BL.Unit>((Func<int, BL.Unit>) (t => BL.Unit.FromNetwork(new int?(t), env))).ToList<BL.Unit>(),
          mPanels = nw.mPanelsRow.Select<int, BL.Panel>((Func<int, int, BL.Panel>) ((p, index) => env.getFieldPanel(p, nw.mPanelsColumn[index]))).ToList<BL.Panel>(),
          mRandom = nw.mRandom
        };
      }

      public BattleSkillResult()
      {
        this.mSkill = (BL.Skill) null;
        this.mInvocation = (BL.Unit) null;
        this.mTargets = (List<BL.Unit>) null;
        this.mPanels = (List<BL.Panel>) null;
        this.mRandom = (XorShift) null;
      }

      public static BL.BattleSkillResult createBattleSkillResult(
        int skill_id,
        BL.ISkillEffectListUnit invocation,
        List<BL.Unit> targets,
        int nowTurnCount,
        List<BL.Panel> panels = null)
      {
        BL.BattleSkillResult battleSkillResult = (BL.BattleSkillResult) null;
        BL.Unit unit = invocation as BL.Unit;
        BL.Skill skill = !invocation.hasOugi || invocation.ougi.id != skill_id ? (!(unit != (BL.Unit) null) || !unit.hasSEASkill || unit.SEASkill.id != skill_id ? ((IEnumerable<BL.Skill>) invocation.skills).SingleOrDefault<BL.Skill>((Func<BL.Skill, bool>) (x => x.id == skill_id)) : unit.SEASkill) : invocation.ougi;
        if (skill != null)
        {
          int? remain;
          if (skill.isCommand)
          {
            if (skill.remain.HasValue)
            {
              if (skill.remain.HasValue)
              {
                remain = skill.remain;
                int num = 0;
                if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                  goto label_6;
              }
              else
                goto label_6;
            }
            if (!invocation.IsDontUseCommand(skill) && invocation.checkEnableSkill(skill.skill))
              goto label_16;
          }
label_6:
          if (skill.isOugi && skill.useTurn - nowTurnCount <= 0)
          {
            remain = skill.remain;
            if (remain.HasValue)
            {
              remain = skill.remain;
              if (remain.HasValue)
              {
                remain = skill.remain;
                int num = 0;
                if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                  goto label_11;
              }
              else
                goto label_11;
            }
            if (!invocation.IsDontUseOugi(skill) && skill.canUseTurn(nowTurnCount))
              goto label_16;
          }
label_11:
          if (skill.isSEA)
          {
            remain = skill.remain;
            if (remain.HasValue)
            {
              remain = skill.remain;
              if (remain.HasValue)
              {
                remain = skill.remain;
                int num = 0;
                if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                  goto label_22;
              }
              else
                goto label_22;
            }
            if (!(unit == (BL.Unit) null) && unit.IsDontUseSEA(skill) || !invocation.checkEnableSkill(skill.skill))
              goto label_22;
          }
          else
            goto label_22;
label_16:
          bool flag = false;
          foreach (BattleskillEffect effect in skill.skill.Effects)
          {
            if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.call_reinforcements)
            {
              flag = true;
              break;
            }
          }
          battleSkillResult = !flag ? new BL.BattleSkillResult() : (BL.BattleSkillResult) new BL.BattleSkillResultExtendEffect(skill);
          battleSkillResult.mSkill = !invocation.originalUnit.hasOugi || invocation.originalUnit.ougi.id != skill_id ? (!(unit != (BL.Unit) null) || !unit.hasSEASkill || unit.SEASkill.id != skill_id ? ((IEnumerable<BL.Skill>) invocation.originalUnit.skills).SingleOrDefault<BL.Skill>((Func<BL.Skill, bool>) (x => x.id == skill_id)) : unit.SEASkill) : invocation.originalUnit.ougi;
          battleSkillResult.mInvocation = invocation.originalUnit;
          battleSkillResult.mTargets = targets;
          battleSkillResult.mPanels = panels ?? new List<BL.Panel>();
          battleSkillResult.mRandom = new XorShift(DateTime.Now);
        }
label_22:
        return battleSkillResult;
      }

      public BL.Skill skill => this.mSkill;

      public BL.Unit invocation => this.mInvocation;

      public List<BL.Unit> targets => this.mTargets;

      public List<BL.Panel> panels => this.mPanels;

      public XorShift random => this.mRandom;
    }

    [Serializable]
    public class BattleSkillResultEffect
    {
      public BattleskillEffect _battleskill_effect;

      public BattleSkillResultEffect(BattleskillEffect be) => this._battleskill_effect = be;

      public virtual void init(BL bl)
      {
      }
    }

    [Serializable]
    public class BattleSkillResultEffectCallReinforcement : BL.BattleSkillResultEffect
    {
      public List<uint> _random_values;

      public BattleSkillResultEffectCallReinforcement(BattleskillEffect be)
        : base(be)
      {
        this._random_values = new List<uint>();
      }

      public override void init(BL bl)
      {
        int num = this._battleskill_effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_max);
        if (num <= 0)
          return;
        for (int index = 0; index < num; ++index)
          this._random_values.Add(bl.random.Next());
      }
    }

    [Serializable]
    public class BattleSkillResultExtendEffect : BL.BattleSkillResult
    {
      public List<BL.BattleSkillResultEffect> _effects;

      public BattleSkillResultExtendEffect(BL.Skill skill)
      {
        this._effects = new List<BL.BattleSkillResultEffect>();
        foreach (BattleskillEffect effect in skill.skill.Effects)
          this._effects.Add(effect.EffectLogic.Enum != BattleskillEffectLogicEnum.call_reinforcements ? new BL.BattleSkillResultEffect(effect) : (BL.BattleSkillResultEffect) new BL.BattleSkillResultEffectCallReinforcement(effect));
      }

      public void init(BL bl)
      {
        foreach (BL.BattleSkillResultEffect effect in this._effects)
          effect.init(bl);
      }
    }

    public enum Phase
    {
      unset = -2, // 0xFFFFFFFE
      none = -1, // 0xFFFFFFFF
      player_start = 0,
      neutral_start = 1,
      enemy_start = 2,
      player = 3,
      neutral = 4,
      enemy = 5,
      player_end = 6,
      neutral_end = 7,
      enemy_end = 8,
      turn_initialize = 10, // 0x0000000A
      win_finalize = 11, // 0x0000000B
      finalize = 12, // 0x0000000C
      suspend = 13, // 0x0000000D
      player_start_post = 15, // 0x0000000F
      neutral_start_post = 16, // 0x00000010
      enemy_start_post = 17, // 0x00000011
      all_dead_player = 20, // 0x00000014
      all_dead_neutral = 21, // 0x00000015
      all_dead_enemy = 22, // 0x00000016
      stageclear_pre = 29, // 0x0000001D
      stageclear = 30, // 0x0000001E
      gameover = 31, // 0x0000001F
      surrender = 32, // 0x00000020
      pvp_move_unit_waiting = 40, // 0x00000028
      pvp_player_start = 41, // 0x00000029
      pvp_enemy_start = 42, // 0x0000002A
      pvp_result = 43, // 0x0000002B
      pvp_disposition = 44, // 0x0000002C
      pvp_start_init = 45, // 0x0000002D
      pvp_wait_preparing = 46, // 0x0000002E
      pvp_exception = 50, // 0x00000032
      pvp_restart = 51, // 0x00000033
      wave_start = 60, // 0x0000003C
      battle_start = 100, // 0x00000064
      battle_start_init = 101, // 0x00000065
    }

    [Serializable]
    public class PhaseState : BL.ModelBase
    {
      [SerializeField]
      private BL.Phase mState;
      [SerializeField]
      private int mTurnCount;
      [SerializeField]
      private int mAbsoluteTurnCount;

      public PhaseState()
      {
        this.mState = BL.Phase.battle_start;
        this.mAbsoluteTurnCount = this.mTurnCount = 0;
      }

      public BL.Phase state => this.mState;

      public void setStateWithTurnInitialize(BL env)
      {
        ++this.mTurnCount;
        ++this.mAbsoluteTurnCount;
        env.firstCompleted.value = false;
        foreach (BL.UnitPosition unitPosition in env.unitPositions.value)
        {
          unitPosition.unit.skillEffects.TurnStart(this.mAbsoluteTurnCount, env, (BL.ISkillEffectListUnit) unitPosition.unit);
          unitPosition.unit.commit();
          unitPosition.commit();
        }
        foreach (BL.Panel panel in env.getAllPanel())
          panel.turnStart();
        env.attackStatusCacheGC();
      }

      public void setStateWith(BL.Phase state, BL env, Action f)
      {
        if (this.mState == state)
          return;
        this.mState = state;
        if (state == BL.Phase.turn_initialize)
          this.setStateWithTurnInitialize(env);
        if (state == BL.Phase.player_start || state == BL.Phase.enemy_start)
        {
          foreach (BL.UnitPosition unitPosition in env.unitPositions.value)
          {
            unitPosition.unit.skillEffects.PhaseStart(env, (BL.ISkillEffectListUnit) unitPosition.unit);
            unitPosition.unit.commit();
            unitPosition.commit();
          }
        }
        ++this.revision;
        f();
      }

      public int turnCount
      {
        get => this.mTurnCount;
        set => this.mTurnCount = value;
      }

      public int absoluteTurnCount
      {
        get => this.mAbsoluteTurnCount;
        set => this.mAbsoluteTurnCount = value;
      }

      public void turnReset() => this.mTurnCount = 0;
    }

    [Serializable]
    public class CallSkillState : BL.ModelBase
    {
      [SerializeField]
      private Decimal mCallSkillPoint;
      [SerializeField]
      private bool mIsChargedCallGauge;
      [SerializeField]
      private bool mIsUsedCallSkill;
      [SerializeField]
      private int mSameCharacterID;
      [SerializeField]
      private bool mIsSameCharacterJoin;
      [SerializeField]
      private int mIntimateRank;
      [SerializeField]
      private int mPlayerRank;
      [SerializeField]
      private int mSkillId;
      [SerializeField]
      private bool mIsSomeAction;
      private Decimal _gauge_capacity;

      public Decimal callSkillPoint
      {
        get => this.mCallSkillPoint;
        set
        {
          this.mCallSkillPoint = value;
          ++this.revision;
        }
      }

      public bool isChargedCallGauge
      {
        get => this.mIsChargedCallGauge;
        set => this.mIsChargedCallGauge = value;
      }

      public bool isUsedCallSkill
      {
        get => this.mIsUsedCallSkill;
        set => this.mIsUsedCallSkill = value;
      }

      public int sameCharacterID
      {
        get => this.mSameCharacterID;
        set => this.mSameCharacterID = value;
      }

      public bool isSameCharacterJoin
      {
        get => this.mIsSameCharacterJoin;
        set => this.mIsSameCharacterJoin = value;
      }

      public int intimateRank
      {
        get => this.mIntimateRank;
        set => this.mIntimateRank = value;
      }

      public int playerRank
      {
        get => this.mPlayerRank;
        set => this.mPlayerRank = value;
      }

      public int skillId
      {
        get => this.mSkillId;
        set => this.mSkillId = value;
      }

      public int countSkillUses => !this.mIsUsedCallSkill ? 0 : 1;

      public bool isSomeAction
      {
        get => this.mIsSomeAction;
        set
        {
          this.mIsSomeAction = value;
          ++this.revision;
        }
      }

      public Decimal callGaugeRate => this.callSkillPoint / this.gauge_capacity;

      public Decimal gauge_capacity
      {
        get
        {
          if (this._gauge_capacity > 0M)
            return this._gauge_capacity;
          foreach (CallGaugeRate callGaugeRate in MasterData.CallGaugeRateList)
          {
            if (callGaugeRate.key == nameof (gauge_capacity))
            {
              this._gauge_capacity = (Decimal) callGaugeRate.value;
              break;
            }
          }
          return this._gauge_capacity;
        }
      }

      public bool isCallGaugeMax => this.callSkillPoint >= this.gauge_capacity;

      public bool isCanUseCallSkill
      {
        get
        {
          NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
          return instance.isPvp ? this.isCallGaugeMax && !this.isUsedCallSkill && !this.isSomeAction : this.isCallGaugeMax && !this.isUsedCallSkill && !instance.environment.core.firstCompleted.value;
        }
      }
    }

    public enum StoryType
    {
      battle_start,
      first_turn_start,
      battle_win,
      spawn_unit,
      unit_for_unit,
      unit_in_panel,
      unit_dead,
      duel_start,
      duel_unit_dead,
      turn_start,
      unit_in_area,
      defeat_player,
      wave_clear,
    }

    public enum StoryPhase
    {
      neutral,
      offense,
      defense,
    }

    [Serializable]
    public class Story : BL.ModelBase
    {
      [SerializeField]
      private int mScriptId;
      [SerializeField]
      private BL.StoryType mType;
      [SerializeField]
      private object[] mDatas;
      [SerializeField]
      private bool mIsRead;

      public Story(int id, BL.StoryType type, object[] datas)
      {
        this.mScriptId = id;
        this.mType = type;
        this.mDatas = datas;
        this.isRead = false;
        ++this.revision;
      }

      public int scriptId => this.mScriptId;

      public BL.StoryType type => this.mType;

      public object[] datas => this.mDatas;

      public bool isRead
      {
        get => this.mIsRead;
        set
        {
          this.mIsRead = value;
          ++this.revision;
        }
      }
    }

    public class UnitParameterCache : IDisposable
    {
      private List<BL.UnitPosition> units;
      private BL env;

      public UnitParameterCache(BL env_)
      {
        this.env = env_;
        this.units = this.env.unitPositions.value;
        this.units.ForEach((Action<BL.UnitPosition>) (x => x.unit.enableCache()));
      }

      public void Dispose()
      {
        this.units.ForEach((Action<BL.UnitPosition>) (x => x.unit.Dispose()));
      }
    }

    [Serializable]
    public class DuelHistory
    {
      [SerializeField]
      public int inflictTotalDamage;
      [SerializeField]
      public int sufferTotalDamage;
      [SerializeField]
      public int criticalCount;
      [SerializeField]
      public int inflictMaxDamage;
      [SerializeField]
      public int weekElementAttackCount;
      [SerializeField]
      public int weekKindAttackCount;
      [SerializeField]
      public int targetUnitID;

      public DuelHistory()
      {
      }

      public DuelHistory(BL.DuelHistory target)
      {
        this.inflictTotalDamage = target.inflictTotalDamage;
        this.sufferTotalDamage = target.sufferTotalDamage;
        this.criticalCount = target.criticalCount;
        this.inflictMaxDamage = target.inflictMaxDamage;
        this.weekElementAttackCount = target.weekElementAttackCount;
        this.weekKindAttackCount = target.weekKindAttackCount;
        this.targetUnitID = target.targetUnitID;
      }
    }

    [Serializable]
    public class Facility
    {
      [SerializeField]
      public BL.ForceID thisForce;
      [SerializeField]
      public bool isTarget;
      [SerializeField]
      public bool isPutOn;
      [SerializeField]
      public bool isView;
      [SerializeField]
      public int? skillUnitIndex;

      public Facility()
      {
      }

      public Facility(BL.Facility target)
      {
        this.thisForce = target.thisForce;
        this.isTarget = target.isTarget;
        this.isPutOn = target.isPutOn;
        this.isView = target.isView;
        this.skillUnitIndex = target.skillUnitIndex;
      }

      public bool isSkillUnit => this.skillUnitIndex.HasValue;
    }

    [Serializable]
    public class Unit : BL.ModelBase, IDisposable, BL.ISkillEffectListUnit
    {
      [SerializeField]
      private BL.DuelHistory[] mDuelHistory;
      [SerializeField]
      private int mSpecificId;
      [SerializeField]
      private int mUnitId;
      [SerializeField]
      private PlayerUnit mPlayerUnit;
      [SerializeField]
      private int mIndex;
      [SerializeField]
      private bool mFriend;
      [SerializeField]
      private bool mCrippled;
      [SerializeField]
      private int mAIScorePatternID;
      [SerializeField]
      private int mAIMoveGroup;
      [SerializeField]
      private int mAIMoveGroupOrder;
      [SerializeField]
      private int mAIMoveTargetX;
      [SerializeField]
      private int mAIMoveTargetY;
      [SerializeField]
      private object mAIExtension;
      [SerializeField]
      private int mLv;
      [SerializeField]
      private int mHp;
      [SerializeField]
      private BL.Weapon mWeapon;
      [SerializeField]
      private bool mGearLeftHand;
      [SerializeField]
      private bool mGearDualWield;
      [SerializeField]
      private BL.Weapon[] mOptionWeapons;
      [SerializeField]
      private BL.Skill mOugi;
      [SerializeField]
      private BL.Skill mSEASkill;
      [SerializeField]
      private BL.Skill[] mArySkill;
      [SerializeField]
      private BL.Skill[] mDuelSkill;
      [SerializeField]
      private BL.MagicBullet[] mAryMB;
      [SerializeField]
      private int mSpawnTurn;
      [SerializeField]
      private int[] save_equiped_gears;
      [SerializeField]
      private bool mIsDead;
      [SerializeField]
      private List<int> mDeadTurn = new List<int>();
      [SerializeField]
      private int mLastDeadTurn;
      [SerializeField]
      private bool mIsSpawned;
      [SerializeField]
      private bool mIsCallEntryReserve;
      [SerializeField]
      private int mSkillfullWeapon;
      [SerializeField]
      private int mSkillfullShild;
      [SerializeField]
      private BL.SkillEffectList mSkillEffects = new BL.SkillEffectList();
      [SerializeField]
      private BL.DropData mDrop;
      [SerializeField]
      private int mDropMoney;
      [SerializeField]
      private int mPvpPoint;
      [SerializeField]
      private int mPvpRespawnCount;
      [SerializeField]
      public bool checkActionRangeBySetHp;
      [SerializeField]
      public bool deadByItem;
      [SerializeField]
      private int mAttackCount;
      [SerializeField]
      private int mAttackDamage;
      [SerializeField]
      private int mKillCount;
      [SerializeField]
      private int mDeadCount;
      [SerializeField]
      private int mDeadCountExceptImmediateRebirth;
      [SerializeField]
      private BL.Unit mKilledBy;
      [SerializeField]
      private int mOverkillDamage;
      [SerializeField]
      private int mReceivedDamage;
      [SerializeField]
      private int mAttackOverkillDamage;
      [SerializeField]
      private int mDuelCount;
      [SerializeField]
      private int mInitialHp;
      [SerializeField]
      private int mAttackSubDamage;
      [SerializeField]
      private int mReceivedSubDamage;
      [SerializeField]
      private int mReceivedLandformDamage;
      [SerializeField]
      private List<int> mSkillUsesIds;
      [SerializeField]
      private List<int> mSkillUsesCounts;
      [SerializeField]
      private int mFacilitySpawnOrder;
      [NonSerialized]
      private MasterDataTable.UnitJob mUnitJob;
      [NonSerialized]
      private int mParameterCacheCount;
      [NonSerialized]
      private Judgement.BattleParameter mParameterCache;
      [SerializeField]
      private BL.Facility mFacility;
      [SerializeField]
      private bool mIsPlayerControl;
      [NonSerialized]
      private BL.ForceID[] mTargetForce;
      [SerializeField]
      private bool? mIsPlayerForce;
      private bool? mIsCharm;
      private CommonElement? _element;
      [NonSerialized]
      private UnitGroup unitGroupCache;
      [NonSerialized]
      private bool isUnitGroupCached;
      [NonSerialized]
      private Tuple<BattleskillSkill, int, int>[] unitAndGearSkillsCache;
      [NonSerialized]
      private BattleskillEffect[] absoluteCounterAttackEffectsCache;
      [SerializeField]
      public bool mIsExecCompletedSkillEffect;

      public BL.Unit Clone()
      {
        BL.Unit unit = new BL.Unit();
        unit.Copy(this);
        return unit;
      }

      public void Copy(BL.Unit target)
      {
        if (target.duelHistory != null)
        {
          this.mDuelHistory = new BL.DuelHistory[target.duelHistory.Length];
          for (int index = 0; index < target.duelHistory.Length; ++index)
          {
            if (target.duelHistory[index] != null)
              this.mDuelHistory[index] = new BL.DuelHistory(target.duelHistory[index]);
          }
        }
        this.mSpecificId = target.specificId;
        this.mUnitId = target.unitId;
        this.mPlayerUnit = target.playerUnit;
        this.mIndex = target.index;
        this.mFriend = target.friend;
        this.mAIScorePatternID = target.aiScorePatternID;
        this.mAIMoveGroup = target.AIMoveGroup;
        this.mAIMoveGroupOrder = target.AIMoveGroupOrder;
        this.mAIMoveTargetX = target.AIMoveTargetX;
        this.mAIMoveTargetY = target.AIMoveTargetY;
        this.mAIExtension = target.aiExtension;
        this.mLv = target.lv;
        this.mHp = target.hp;
        this.mWeapon = target.weapon;
        this.mGearLeftHand = target.gearLeftHand;
        this.mGearDualWield = target.gearDualWield;
        this.mOptionWeapons = target.optionWeapons;
        if (target.ougi != null)
          this.mOugi = new BL.Skill(target.ougi);
        if (target.SEASkill != null)
          this.mSEASkill = new BL.Skill(target.SEASkill);
        if (target.skills != null)
        {
          this.mArySkill = new BL.Skill[target.skills.Length];
          for (int index = 0; index < target.skills.Length; ++index)
          {
            if (target.skills[index] != null)
              this.mArySkill[index] = new BL.Skill(target.skills[index]);
          }
        }
        if (target.duelSkills != null)
        {
          this.mDuelSkill = new BL.Skill[target.duelSkills.Length];
          for (int index = 0; index < target.duelSkills.Length; ++index)
          {
            if (target.duelSkills[index] != null)
              this.mDuelSkill[index] = new BL.Skill(target.duelSkills[index]);
          }
        }
        this.mAryMB = target.magicBullets;
        this.mSpawnTurn = target.spawnTurn;
        if (target.saveEquipedGears != null)
        {
          this.save_equiped_gears = new int[target.saveEquipedGears.Length];
          for (int index = 0; index < target.saveEquipedGears.Length; ++index)
            this.save_equiped_gears[index] = target.saveEquipedGears[index];
        }
        this.mIsDead = target.isDead;
        for (int index = 0; index < target.deadTurn.Count; ++index)
          this.mDeadTurn.Add(target.deadTurn[index]);
        this.mLastDeadTurn = target.lastDeadTurn;
        this.mIsSpawned = target.isSpawned;
        this.mIsCallEntryReserve = target.isCallEntryReserve;
        this.mSkillfullWeapon = target.skillfull_weapon;
        this.mSkillfullShild = target.skillfull_shield;
        this.mSkillEffects = target.skillEffects.Clone();
        this.mDrop = target.drop;
        this.mDropMoney = target.dropMoney;
        this.mPvpPoint = target.pvpPoint;
        this.mPvpRespawnCount = target.pvpRespawnCount;
        this.checkActionRangeBySetHp = target.checkActionRangeBySetHp;
        this.mAttackCount = target.attackCount;
        this.mAttackDamage = target.attackDamage;
        this.mKillCount = target.killCount;
        this.mDeadCount = target.deadCount;
        this.mDeadCountExceptImmediateRebirth = target.deadCountExceptImmediateRebirth;
        this.mKilledBy = target.killedBy;
        this.mOverkillDamage = target.overkillDamage;
        this.mReceivedDamage = target.receivedDamage;
        this.mAttackSubDamage = target.attackSubDamage;
        this.mReceivedSubDamage = target.receivedSubDamage;
        this.mReceivedLandformDamage = target.receivedLandformDamage;
        if (!target.mSkillUsesIds.IsNullOrEmpty<int>())
        {
          this.mSkillUsesIds = new List<int>((IEnumerable<int>) target.mSkillUsesIds);
          this.mSkillUsesCounts = new List<int>((IEnumerable<int>) target.mSkillUsesCounts);
        }
        else
        {
          this.mSkillUsesIds = (List<int>) null;
          this.mSkillUsesCounts = (List<int>) null;
        }
        this.mAttackOverkillDamage = target.attackOverkillDamage;
        this.mDuelCount = target.duelCount;
        this.mInitialHp = target.initialHp;
        if (target.facility != null)
          this.mFacility = new BL.Facility(target.facility);
        this.mFacilitySpawnOrder = target.facilitySpawnOrder;
        this.mIsPlayerControl = target.isPlayerControl;
        this.mIsExecCompletedSkillEffect = target.mIsExecCompletedSkillEffect;
        this.mIsPlayerForce = new bool?(target.isPlayerForce);
        if (!target.IsUseCharm)
          return;
        this.mIsCharm = new bool?(target.IsCharm);
      }

      public void initReinforcement()
      {
        this.mAttackCount = 0;
        this.mAttackDamage = 0;
        this.mKillCount = 0;
        this.mDeadCount = 0;
        this.mDeadCountExceptImmediateRebirth = 0;
        this.mKilledBy = (BL.Unit) null;
        this.mOverkillDamage = 0;
        this.mReceivedDamage = 0;
        this.mAttackSubDamage = 0;
        this.mReceivedSubDamage = 0;
        this.mReceivedLandformDamage = 0;
        this.mSkillUsesIds = (List<int>) null;
        this.mSkillUsesCounts = (List<int>) null;
        this.mAttackOverkillDamage = 0;
        this.mDuelCount = 0;
        ++this.revision;
      }

      public int? ToNetwork(BL env) => env.getUnitPosition(this).ToNetwork(env);

      public static BL.Unit FromNetwork(int? nw, BL env)
      {
        return nw.HasValue ? BL.UnitPosition.FromNetwork(nw, env).unit : (BL.Unit) null;
      }

      public BL.DuelHistory[] duelHistory
      {
        get => this.mDuelHistory;
        set
        {
          this.mDuelHistory = value;
          ++this.revision;
        }
      }

      public int specificId
      {
        get => this.mSpecificId;
        set
        {
          this.mSpecificId = value;
          ++this.revision;
        }
      }

      public int unitId
      {
        get => this.mUnitId;
        set
        {
          this.mUnitId = value;
          ++this.revision;
        }
      }

      public PlayerUnit playerUnit
      {
        get => this.mPlayerUnit;
        set
        {
          this.mPlayerUnit = value;
          this.clearLocalField();
          ++this.revision;
        }
      }

      public int index
      {
        get => this.mIndex;
        set
        {
          this.mIndex = value;
          ++this.revision;
        }
      }

      public bool is_leader => this.index == 0;

      public bool is_helper => this.index == 5;

      public bool friend
      {
        get => this.mFriend;
        set
        {
          this.mFriend = value;
          ++this.revision;
        }
      }

      public bool crippled => this.mCrippled;

      public void setCrippled(bool v) => this.mCrippled = v;

      public int aiScorePatternID
      {
        get => this.mAIScorePatternID;
        set
        {
          this.mAIScorePatternID = value;
          ++this.revision;
        }
      }

      public UnitUnit unit => MasterData.UnitUnit[this.mUnitId];

      private void clearLocalField() => this.mUnitJob = (MasterDataTable.UnitJob) null;

      public MasterDataTable.UnitJob job
      {
        get => this.mUnitJob ?? (this.mUnitJob = this.playerUnit.getJobData());
      }

      public SkillMetamorphosis metamorphosis
      {
        get => BattleFuncs.getMetamorphosis((BL.ISkillEffectListUnit) this);
      }

      public object aiExtension
      {
        get => this.mAIExtension;
        set
        {
          this.mAIExtension = value;
          ++this.revision;
        }
      }

      public BL.Panel GetTargetPanel(BL env)
      {
        return env.getFieldPanel(this.mAIMoveTargetY, this.mAIMoveTargetX);
      }

      public int lv
      {
        get => this.mLv;
        set
        {
          this.mLv = value;
          ++this.revision;
        }
      }

      public int hp
      {
        get => this.mHp;
        set
        {
          int mHp = this.mHp;
          int num1 = Mathf.Max(Mathf.Min(value, this.parameter.Hp), 0);
          int num2 = num1;
          if (mHp != num2)
          {
            if (this.checkActionRangeBySetHp && (this.magicBullets.Length != 0 || this.enabledSkillEffect(BattleskillEffectLogicEnum.fix_range).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
            {
              if (x.effect.HasKey(BattleskillEffectLogicArgumentEnum.min_hp_percentage) && (double) x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_hp_percentage) != 0.0)
                return true;
              return x.effect.HasKey(BattleskillEffectLogicArgumentEnum.max_hp_percentage) && (double) x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_hp_percentage) != 0.0;
            }))))
            {
              int[] attackRange1 = this.attackRange;
              int[] healRange1 = this.healRange;
              this.mHp = num1;
              int[] attackRange2 = this.attackRange;
              int[] healRange2 = this.healRange;
              if (attackRange1[0] != attackRange2[0] || attackRange1[1] != attackRange2[1])
                BattleFuncs.getUnitPosition((BL.ISkillEffectListUnit) this)?.clearMoveActionRangePanelCache();
              if (healRange1.Length != healRange2.Length || healRange1.Length == 2 && (healRange1[0] != healRange2[0] || healRange1[1] != healRange2[1]))
                BattleFuncs.getUnitPosition((BL.ISkillEffectListUnit) this)?.clearMoveHealRangePanelCache();
            }
            else
              this.mHp = num1;
          }
          ++this.revision;
        }
      }

      public int hpMax => this.parameter.Hp;

      public Judgement.BattleParameter parameter => this.mParameterCache ?? this.calcParameter();

      public Judgement.BattleParameter calcParameter()
      {
        return Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) this);
      }

      public void setParameter(Judgement.BattleParameter parameter)
      {
        this.mParameterCache = parameter;
      }

      public BL.Unit enableCache()
      {
        ++this.mParameterCacheCount;
        this.mParameterCache = this.mParameterCache ?? this.calcParameter();
        return this;
      }

      public void Dispose()
      {
        --this.mParameterCacheCount;
        if (this.mParameterCacheCount == 0)
        {
          this.mParameterCache = (Judgement.BattleParameter) null;
        }
        else
        {
          if (this.mParameterCacheCount >= 0)
            return;
          Debug.LogError((object) ("Illegal decrement mParameterCacheCount=" + (object) this.mParameterCacheCount));
        }
      }

      public int AIMoveTargetX
      {
        get => this.mAIMoveTargetX;
        set
        {
          this.mAIMoveTargetX = value;
          ++this.revision;
        }
      }

      public int AIMoveTargetY
      {
        get => this.mAIMoveTargetY;
        set
        {
          this.mAIMoveTargetY = value;
          ++this.revision;
        }
      }

      public int AIMoveGroup
      {
        get => this.mAIMoveGroup;
        set
        {
          this.mAIMoveGroup = value;
          ++this.revision;
        }
      }

      public bool IsAIMoveGroup => this.mAIMoveGroup > 0;

      public int AIMoveGroupOrder
      {
        get => this.mAIMoveGroupOrder;
        set
        {
          this.mAIMoveGroupOrder = value;
          ++this.revision;
        }
      }

      public BL.Weapon weapon
      {
        get => this.mWeapon;
        set
        {
          this.mWeapon = value;
          ++this.revision;
        }
      }

      public BL.Weapon[] optionWeapons
      {
        get => this.mOptionWeapons ?? (this.mOptionWeapons = new BL.Weapon[0]);
        set
        {
          this.mOptionWeapons = value;
          ++this.revision;
        }
      }

      public BL.Skill ougi
      {
        get => this.mOugi;
        set
        {
          this.mOugi = value;
          ++this.revision;
        }
      }

      public BL.Skill SEASkill
      {
        get => this.mSEASkill;
        set
        {
          this.mSEASkill = value;
          ++this.revision;
        }
      }

      public bool gearLeftHand
      {
        get => this.mGearLeftHand;
        set
        {
          this.mGearLeftHand = value;
          ++this.revision;
        }
      }

      public bool gearDualWield
      {
        get => this.mGearDualWield;
        set
        {
          this.mGearDualWield = value;
          ++this.revision;
        }
      }

      public int spawnTurn
      {
        get => this.mSpawnTurn;
        set
        {
          this.mSpawnTurn = value;
          ++this.revision;
        }
      }

      public int[] saveEquipedGears => this.save_equiped_gears;

      public int skillfull_weapon
      {
        get => this.mSkillfullWeapon;
        set
        {
          this.mSkillfullWeapon = value;
          ++this.revision;
        }
      }

      public int skillfull_shield
      {
        get => this.mSkillfullShild;
        set
        {
          this.mSkillfullShild = value;
          ++this.revision;
        }
      }

      public BL.Skill[] skills
      {
        get => this.mArySkill;
        set
        {
          this.mArySkill = value;
          ++this.revision;
        }
      }

      public BL.Skill[] duelSkills
      {
        get => this.mDuelSkill;
        set
        {
          this.mDuelSkill = value;
          ++this.revision;
        }
      }

      public BL.MagicBullet[] magicBullets
      {
        get => this.mAryMB;
        set
        {
          this.mAryMB = value;
          ++this.revision;
        }
      }

      public bool isDead => this.mIsDead;

      public List<int> deadTurn
      {
        get => this.mDeadTurn;
        set
        {
          this.mDeadTurn = value;
          ++this.revision;
        }
      }

      public int lastDeadTurn
      {
        get => this.mLastDeadTurn;
        set
        {
          this.mLastDeadTurn = value;
          ++this.revision;
        }
      }

      public bool isSpawned
      {
        get => this.mIsSpawned;
        set => this.mIsSpawned = value;
      }

      public bool isCallEntryReserve
      {
        get => this.mIsCallEntryReserve;
        set => this.mIsCallEntryReserve = value;
      }

      public BL.Facility facility
      {
        get => this.mFacility;
        set => this.mFacility = value;
      }

      public bool isFacility => this.mFacility != null;

      public bool isPutOn => this.isFacility && this.mFacility.isPutOn;

      public bool isView => !this.isFacility || this.mFacility.isView;

      public bool isAttackTarget => !this.isFacility || this.mFacility.isTarget;

      public bool isHealTarget => !this.isFacility;

      public bool checkTargetAttribute(BL.Unit.TargetAttribute ta)
      {
        return ta == BL.Unit.TargetAttribute.all || (ta & BL.Unit.TargetAttribute.attack) == BL.Unit.TargetAttribute.attack && this.isAttackTarget || (ta & BL.Unit.TargetAttribute.heal) == BL.Unit.TargetAttribute.heal && this.isHealTarget;
      }

      public int transformationGroupId
      {
        get => BattleFuncs.getTransformationGroupId((BL.ISkillEffectListUnit) this);
      }

      public bool checkEnableSkill(BattleskillSkill skill)
      {
        return BattleFuncs.checkEnableUnitSkill((BL.ISkillEffectListUnit) this, skill);
      }

      public void addDeadCount(int turn, bool deadToImmediateRebirth)
      {
        ++this.mDeadCount;
        if (!deadToImmediateRebirth)
          ++this.mDeadCountExceptImmediateRebirth;
        if (turn != 0)
          this.mDeadTurn.Add(turn);
        this.mDeadTurn.RemoveAll((Predicate<int>) (x => x < turn - 10));
        this.mLastDeadTurn = turn;
      }

      public bool setIsDead(
        bool v,
        BL env,
        int turn = 0,
        bool dontRemoveSteal = false,
        bool deadToImmediateRebirth = false,
        bool dontCountDead = false)
      {
        bool flag = false;
        if (this.mIsDead != v)
        {
          BL.UnitPosition unitPosition1 = env.getUnitPosition(this);
          this.mIsDead = v;
          ++this.revision;
          if (this.mIsDead)
          {
            if (dontCountDead)
              flag = true;
            else
              this.addDeadCount(turn, deadToImmediateRebirth);
            env.removeZocPanels((BL.ISkillEffectListUnit) this, unitPosition1.originalRow, unitPosition1.originalColumn);
            unitPosition1.removePanelSkillEffects();
            if (!dontRemoveSteal)
              BattleFuncs.removeStealEffects((BL.ISkillEffectListUnit) this);
            BattleFuncs.removeJumpEffects((BL.ISkillEffectListUnit) this);
          }
          else
          {
            this.mSkillEffects.RemoveAilmentEffect(env, (BL.ISkillEffectListUnit) this);
            this.mSkillEffects.SetKillCount(0);
            this.skillEffects.RecoveryRemovedSkillEffects((BL.ISkillEffectListUnit) this);
            env.addZocPanels((BL.ISkillEffectListUnit) this, unitPosition1.originalRow, unitPosition1.originalColumn);
            unitPosition1.addPanelSkillEffects();
          }
          foreach (BL.UnitPosition unitPosition2 in env.unitPositions.value)
          {
            if (unitPosition2.hasPanelsCache)
              unitPosition2.clearMovePanelCache();
          }
          if (!this.isPlayerControl)
            env.createDangerAria();
        }
        return flag;
      }

      public void rebirth(BL env, bool resetHp = true, bool resetCompleted = true, bool forceResetCompleted = false)
      {
        BL.UnitPosition unitPosition = env.getUnitPosition(this);
        unitPosition.resetSpawnPosition();
        if (resetHp && this.hp != this.parameter.Hp)
          this.hp = this.parameter.Hp;
        this.setIsDead(false, env);
        if (resetCompleted)
        {
          unitPosition.resetOriginalPosition(env);
          BL.ClassValue<List<BL.UnitPosition>> classValue = env.currentActionUnitsList();
          if (classValue != null && !classValue.value.Contains(unitPosition))
          {
            classValue.value.Add(unitPosition);
            classValue.commit();
          }
        }
        else if (forceResetCompleted)
        {
          unitPosition.resetOriginalPosition(env);
          BL.ClassValue<List<BL.UnitPosition>> actionUnits = env.getActionUnits(env.getForceID(this));
          if (actionUnits != null && !actionUnits.value.Contains(unitPosition))
          {
            actionUnits.value.Add(unitPosition);
            actionUnits.commit();
          }
        }
        if (!env.completedActionUnits.value.Contains(unitPosition))
          return;
        env.completedActionUnits.value.Remove(unitPosition);
        env.completedActionUnits.commit();
      }

      public bool isPlayerControl
      {
        get => this.mIsPlayerControl && !this.IsCharm;
        set
        {
          this.mIsPlayerControl = value;
          ++this.revision;
        }
      }

      public bool hasMapSkill
      {
        get
        {
          return ((IEnumerable<BL.Skill>) this.mArySkill).Any<BL.Skill>((Func<BL.Skill, bool>) (x => x.skill.checkEnableUnit((BL.ISkillEffectListUnit) this)));
        }
      }

      public bool hasOugi => this.mOugi != null;

      public bool hasSEASkill => this.mSEASkill != null;

      public BL.DropData drop
      {
        get => this.mDrop;
        set
        {
          this.mDrop = value;
          ++this.revision;
        }
      }

      public bool hasDrop => this.mDrop != null;

      public int dropMoney
      {
        get => this.mDropMoney;
        set
        {
          this.mDropMoney = value;
          ++this.revision;
        }
      }

      public int attackCount
      {
        get => this.mAttackCount;
        set
        {
          this.mAttackCount = value;
          ++this.revision;
        }
      }

      public int attackDamage
      {
        get => this.mAttackDamage;
        set
        {
          this.mAttackDamage = value;
          ++this.revision;
        }
      }

      public int killCount
      {
        get => this.mKillCount;
        set
        {
          this.mKillCount = value;
          ++this.revision;
        }
      }

      public BL.Unit killedBy
      {
        get => this.mKilledBy;
        set
        {
          this.mKilledBy = value;
          ++this.revision;
        }
      }

      public int overkillDamage
      {
        get => this.mOverkillDamage;
        set
        {
          this.mOverkillDamage = value;
          ++this.revision;
        }
      }

      public int attackOverkillDamage
      {
        get => this.mAttackOverkillDamage;
        set
        {
          this.mAttackOverkillDamage = value;
          ++this.revision;
        }
      }

      public int receivedDamage
      {
        get => this.mReceivedDamage;
        set
        {
          this.mReceivedDamage = value;
          ++this.revision;
        }
      }

      public int attackSubDamage => this.mAttackSubDamage;

      public void addAttackSubDamage(int damage)
      {
        if (damage <= 0)
          return;
        this.mAttackSubDamage += damage;
        ++this.revision;
      }

      public int receivedSubDamage => this.mReceivedSubDamage;

      public void addReceivedSubDamage(int damage)
      {
        if (damage <= 0)
          return;
        this.mReceivedSubDamage += damage;
        ++this.revision;
      }

      public int receivedLandformDamage => this.mReceivedLandformDamage;

      public void addReceivedLandformDamage(int damage)
      {
        if (damage <= 0)
          return;
        this.mReceivedLandformDamage += damage;
        ++this.revision;
      }

      public List<int> skillUsesIds => this.mSkillUsesIds;

      public List<int> skillUsesCounts => this.mSkillUsesCounts;

      public void countSkillUses(int skill_id)
      {
        if (this.mSkillUsesIds == null)
        {
          this.mSkillUsesIds = new List<int>() { skill_id };
          this.mSkillUsesCounts = new List<int>() { 1 };
        }
        else
        {
          int? nullable = this.mSkillUsesIds.FirstIndexOrNull<int>((Func<int, bool>) (x => x == skill_id));
          if (nullable.HasValue)
          {
            this.mSkillUsesCounts[nullable.Value]++;
          }
          else
          {
            this.mSkillUsesIds.Add(skill_id);
            this.mSkillUsesCounts.Add(1);
          }
        }
      }

      public int[] use_skill_group
      {
        get
        {
          int[] useSkillGroup = new int[4];
          if (!this.mSkillUsesIds.IsNullOrEmpty<int>())
          {
            int index = 0;
            foreach (int mSkillUsesId in this.mSkillUsesIds)
            {
              BattleskillSkill battleskillSkill;
              if (MasterData.BattleskillSkill.TryGetValue(mSkillUsesId, out battleskillSkill))
              {
                switch (battleskillSkill.skill_type)
                {
                  case BattleskillSkillType.command:
                    useSkillGroup[1] += this.mSkillUsesCounts[index];
                    break;
                  case BattleskillSkillType.release:
                    useSkillGroup[0] += this.mSkillUsesCounts[index];
                    break;
                  case BattleskillSkillType.SEA:
                    useSkillGroup[3] += this.mSkillUsesCounts[index];
                    break;
                }
              }
              ++index;
            }
          }
          return useSkillGroup;
        }
      }

      public int deadCount
      {
        get => this.mDeadCount;
        set
        {
          this.mDeadCount = value;
          ++this.revision;
        }
      }

      public int deadCountExceptImmediateRebirth
      {
        get => this.mDeadCountExceptImmediateRebirth;
        set
        {
          this.mDeadCountExceptImmediateRebirth = value;
          ++this.revision;
        }
      }

      public int pvpPoint
      {
        get => this.mPvpPoint;
        set
        {
          this.mPvpPoint = value;
          ++this.revision;
        }
      }

      public int pvpRespawnCount
      {
        get => this.mPvpRespawnCount;
        set
        {
          this.mPvpRespawnCount = value;
          ++this.revision;
        }
      }

      public int duelCount
      {
        get => this.mDuelCount;
        set
        {
          this.mDuelCount = value;
          ++this.revision;
        }
      }

      public int initialHp
      {
        get => this.mInitialHp;
        set
        {
          this.mInitialHp = value;
          ++this.revision;
        }
      }

      public int exploreHp
      {
        get => this.mHp;
        set
        {
          this.mHp = value;
          ++this.revision;
        }
      }

      public int facilitySpawnOrder
      {
        get => this.mFacilitySpawnOrder;
        set
        {
          this.mFacilitySpawnOrder = value;
          ++this.revision;
        }
      }

      public int[] attackRange => BattleFuncs.getAttackRange((BL.ISkillEffectListUnit) this);

      public BL.Unit.GearRange gearRange()
      {
        return BattleFuncs.getGearRange((BL.ISkillEffectListUnit) this);
      }

      public BL.Unit.MagicRange magicRange(BL.MagicBullet mb)
      {
        return BattleFuncs.getMagicRange((BL.ISkillEffectListUnit) this, mb);
      }

      public int[] healRange => BattleFuncs.getHealRange((BL.ISkillEffectListUnit) this);

      public BL.ForceID[] targetForce
      {
        get => this.mTargetForce;
        set
        {
          this.mTargetForce = value;
          ++this.revision;
        }
      }

      public bool isPlayerForce
      {
        get
        {
          if (!this.mIsPlayerForce.HasValue)
            this.mIsPlayerForce = new bool?(BattleFuncs.getForceID(this) == BL.ForceID.player);
          return this.mIsPlayerForce.Value;
        }
        set => this.mIsPlayerForce = new bool?(value);
      }

      public bool IsAllEquipUnit
      {
        get
        {
          return ((IEnumerable<string>) Consts.GetInstance().ALL_GEAR_EQUIP_UNIT_IDS.Split(',')).Contains<string>(this.unit.ID.ToString());
        }
      }

      public BL.SkillEffectList skillEffects
      {
        get => this.mSkillEffects;
        set
        {
          this.mSkillEffects = value;
          ++this.revision;
        }
      }

      public void setSkillEffects(BL.SkillEffectList value) => this.mSkillEffects = value;

      public IEnumerable<BL.SkillEffect> getSkillEffects(BattleskillEffectLogicEnum logic)
      {
        return this.mSkillEffects.Where(logic);
      }

      public void dumpSkillEffects()
      {
        string str = string.Format("Unit {0}({1}) の効果一覧:\n", (object) this.unit.ID, (object) this.unit.name) + this.skillEffects.ToString();
      }

      public void SaveEquipedGears()
      {
        if (this.playerUnit.equip_gear_ids == null)
          return;
        this.save_equiped_gears = new int[this.playerUnit.equip_gear_ids.Length];
        for (int index = 0; index < this.playerUnit.equip_gear_ids.Length; ++index)
        {
          int? equipGearId = this.playerUnit.equip_gear_ids[index];
          this.save_equiped_gears[index] = int.MinValue;
          if (equipGearId.HasValue)
            this.save_equiped_gears[index] = equipGearId.Value;
        }
      }

      public void LoadEquipedGears()
      {
        if (this.save_equiped_gears == null)
          return;
        for (int index = 0; index < this.save_equiped_gears.Length; ++index)
        {
          int saveEquipedGear = this.save_equiped_gears[index];
          this.playerUnit.equip_gear_ids[index] = saveEquipedGear == int.MinValue ? new int?() : new int?(saveEquipedGear);
        }
      }

      public BL.Unit originalUnit => this;

      public bool HasAilment => this.skillEffects.HasAilment;

      public bool IsDontAction
      {
        get
        {
          if (this.isFacility)
            return true;
          return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.do_not_act, BattleskillEffectLogicEnum.sleep);
        }
      }

      public bool IsDontMove
      {
        get
        {
          if (this.isFacility)
            return true;
          return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.do_not_move, BattleskillEffectLogicEnum.sleep);
        }
      }

      public bool IsDontEvasion
      {
        get
        {
          return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.paralysis, BattleskillEffectLogicEnum.sleep);
        }
      }

      public bool IsCharm
      {
        get
        {
          if (this.mIsCharm.HasValue)
            return this.mIsCharm.Value;
          return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.charm);
        }
        set => this.mIsCharm = new bool?(value);
      }

      public bool IsUseCharm => this.mIsCharm.HasValue;

      public bool IsJumping
      {
        get => this.skillEffects.Where(BattleskillEffectLogicEnum.jump).Any<BL.SkillEffect>();
      }

      public bool IsDontUseCommand(BL.Skill skill)
      {
        return BattleFuncs.isDontUseCommand((BL.ISkillEffectListUnit) this, skill);
      }

      public bool IsDontUseOugi(BL.Skill skill)
      {
        return BattleFuncs.isDontUseOugi((BL.ISkillEffectListUnit) this, skill);
      }

      public bool IsDontUseSEA(BL.Skill skill)
      {
        return BattleFuncs.isDontUseSEA((BL.ISkillEffectListUnit) this, skill);
      }

      public bool IsDontUseSkill(int skill_id)
      {
        return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.seal) && this.skillEffects.IsSealedSkill(skill_id);
      }

      public bool IsDontUseSkillEffect(BL.SkillEffect effect)
      {
        return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.seal) && this.skillEffects.IsSealedSkillEffect(effect);
      }

      public bool CantChangeCurrent
      {
        get
        {
          return this.skillEffects.HasAilmentEffectLogic(BattleskillEffectLogicEnum.can_not_change_current);
        }
      }

      public bool CanHeal(BattleskillSkillType skillType = (BattleskillSkillType) 0)
      {
        return BattleFuncs.canHeal((BL.ISkillEffectListUnit) this, skillType);
      }

      public bool HasEnabledSkillEffect(BattleskillEffectLogicEnum logic)
      {
        return this.enabledSkillEffect(logic).Any<BL.SkillEffect>();
      }

      public IEnumerable<BL.SkillEffect> enabledSkillEffect(BattleskillEffectLogicEnum logic)
      {
        return this.skillEffects.Where(logic).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect((BL.ISkillEffectListUnit) this, x)));
      }

      public IEnumerator InitAIExtention(LispAILogic ai, byte[] scriptByte = null)
      {
        if (scriptByte != null)
          this.aiExtension = ai.readLisp(scriptByte, (byte[]) null);
        else if (!string.IsNullOrEmpty(this.playerUnit.ai_attack) || !string.IsNullOrEmpty(this.playerUnit.ai_heal) || !string.IsNullOrEmpty(this.playerUnit.ai_move) || !string.IsNullOrEmpty(this.playerUnit.ai_skill) || !string.IsNullOrEmpty(this.playerUnit.ai_use))
        {
          StringBuilder stringBuilder = new StringBuilder();
          if (!string.IsNullOrEmpty(this.playerUnit.ai_attack))
          {
            Parser parser = new Parser(this.playerUnit.ai_attack);
            if (stringBuilder.Length > 1)
              stringBuilder.Append(" ");
            stringBuilder.AppendFormat("(\"attack-point\" . {0})", (object) parser.ToLisp());
          }
          if (!string.IsNullOrEmpty(this.playerUnit.ai_heal))
          {
            Parser parser = new Parser(this.playerUnit.ai_heal);
            if (stringBuilder.Length > 1)
              stringBuilder.Append(" ");
            stringBuilder.AppendFormat("(\"heal-point\" . {0})", (object) parser.ToLisp());
          }
          if (!string.IsNullOrEmpty(this.playerUnit.ai_move))
          {
            Parser parser = new Parser(this.playerUnit.ai_move);
            if (stringBuilder.Length > 1)
              stringBuilder.Append(" ");
            stringBuilder.AppendFormat("(\"move-point\" . {0})", (object) parser.ToLisp());
          }
          if (!string.IsNullOrEmpty(this.playerUnit.ai_skill))
          {
            Parser parser = new Parser(this.playerUnit.ai_skill);
            if (stringBuilder.Length > 1)
              stringBuilder.Append(" ");
            stringBuilder.AppendFormat("(\"skill-point\" . {0})", (object) parser.ToLisp());
          }
          if (!string.IsNullOrEmpty(this.playerUnit.ai_use))
          {
            Parser parser = new Parser(this.playerUnit.ai_use);
            if (stringBuilder.Length > 1)
              stringBuilder.Append(" ");
            stringBuilder.AppendFormat("(\"use\" . {0})", (object) parser.ToLisp());
          }
          this.aiExtension = ai.readLisp(stringBuilder.ToString());
          yield break;
        }
      }

      public CommonElement GetElement()
      {
        if (!this._element.HasValue)
        {
          BL.Skill skill = Array.Find<BL.Skill>(this.duelSkills, (Predicate<BL.Skill>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element))));
          this._element = skill == null ? new CommonElement?(CommonElement.none) : new CommonElement?(skill.skill.element);
        }
        return this._element.Value;
      }

      public override bool Equals(object rhs) => this.Equals(rhs as BL.Unit);

      public override int GetHashCode() => this.mIndex * 100000 + this.mSpecificId;

      public bool Equals(BL.Unit rhs)
      {
        if ((object) rhs == null)
          return false;
        if ((object) this == (object) rhs)
          return true;
        if ((object) this.mPlayerUnit != null)
          return this.mPlayerUnit.Equals(rhs.playerUnit);
        return this.mIndex == rhs.index && this.mSpecificId == rhs.specificId;
      }

      public static bool operator ==(BL.Unit lhs, BL.Unit rhs)
      {
        return (object) lhs == null ? (object) rhs == null : lhs.Equals(rhs);
      }

      public static bool operator !=(BL.Unit lhs, BL.Unit rhs) => !(lhs == rhs);

      public UnitGroup unitGroup
      {
        get
        {
          if (!this.isUnitGroupCached)
          {
            this.unitGroupCache = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == this.playerUnit.unit.ID));
            this.isUnitGroupCached = true;
          }
          return this.unitGroupCache;
        }
      }

      public Tuple<BattleskillSkill, int, int>[] unitAndGearSkills
      {
        get
        {
          if (this.unitAndGearSkillsCache == null)
            this.unitAndGearSkillsCache = BattleFuncs.getUnitAndGearSkills(this).ToArray<Tuple<BattleskillSkill, int, int>>();
          return this.unitAndGearSkillsCache;
        }
      }

      public BattleskillEffect[] absoluteCounterAttackEffects
      {
        get
        {
          if (this.absoluteCounterAttackEffectsCache == null)
            this.absoluteCounterAttackEffectsCache = ((IEnumerable<Tuple<BattleskillSkill, int, int>>) this.unitAndGearSkills).SelectMany<Tuple<BattleskillSkill, int, int>, BattleskillEffect>((Func<Tuple<BattleskillSkill, int, int>, IEnumerable<BattleskillEffect>>) (skill => ((IEnumerable<BattleskillEffect>) skill.Item1.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect => effect.EffectLogic.Enum == BattleskillEffectLogicEnum.absolute_counter_attack && (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation) >= 200.0 && effect.checkLevel(skill.Item2))))).ToArray<BattleskillEffect>();
          return this.absoluteCounterAttackEffectsCache;
        }
      }

      public bool IsRemainingCommandSkills
      {
        get
        {
          return ((IEnumerable<BL.Skill>) this.skills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
          {
            if (!x.remain.HasValue)
              return false;
            int? remain = x.remain;
            int num = 0;
            return remain.GetValueOrDefault() > num & remain.HasValue;
          }));
        }
      }

      public enum TargetAttribute
      {
        all = -1, // 0xFFFFFFFF
        attack = 1,
        heal = 2,
      }

      public class GearRange
      {
        public readonly int Min;
        public readonly int Max;

        public GearRange(int min, int max)
        {
          this.Min = min;
          this.Max = max;
        }
      }

      public class MagicRange
      {
        public readonly int Min;
        public readonly int Max;

        public MagicRange(int min, int max)
        {
          this.Min = min;
          this.Max = max;
        }
      }
    }

    public enum ForceID
    {
      none = -1, // 0xFFFFFFFF
      player = 0,
      neutral = 1,
      enemy = 2,
    }

    [Serializable]
    public class CurrentUnit : BL.ModelBase
    {
      [SerializeField]
      private BL.Unit mUnit;

      public BL.Unit unit => this.mUnit;

      public bool IsPlayerInput { get; private set; }

      public void setCurrentByPlayerInput(BL.Unit unit, BL env, Action<BL.UnitPosition> f)
      {
        this.canSetCurrent(unit, env, (Action<BL.UnitPosition>) (prevUp =>
        {
          this.setCurrent(unit, f, prevUp);
          this.IsPlayerInput = true;
        }));
      }

      public void setCurrentWith(BL.Unit unit, BL env, Action<BL.UnitPosition> f)
      {
        this.canSetCurrent(unit, env, (Action<BL.UnitPosition>) (prevUp =>
        {
          this.setCurrent(unit, f, prevUp);
          this.IsPlayerInput = false;
        }));
      }

      private void canSetCurrent(BL.Unit unit, BL env, Action<BL.UnitPosition> action)
      {
        if (!(this.mUnit != unit))
          return;
        BL.UnitPosition unitPosition = env.getUnitPosition(this.mUnit);
        if (unitPosition != null && unitPosition.cantChangeCurrent)
          return;
        action(unitPosition);
      }

      private void setCurrent(BL.Unit unit, Action<BL.UnitPosition> f, BL.UnitPosition prevUp)
      {
        f(prevUp);
        this.mUnit = unit;
        ++this.revision;
      }

      public void setCurrentWithSetOnly(BL.Unit unit)
      {
        if (!(this.mUnit != unit))
          return;
        this.mUnit = unit;
        this.IsPlayerInput = false;
      }
    }

    [Serializable]
    public class Weapon : BL.ModelBase
    {
      [SerializeField]
      private int mGearId;
      [SerializeField]
      private int mAssistId;
      [SerializeField]
      private bool mIsAwaked;
      public BL.Weapon.SAttackMethod sAttackMethod;
      [NonSerialized]
      private IAttackMethod attackMethod_;

      public IAttackMethod attackMethod
      {
        get
        {
          return this.sAttackMethod.ID == 0 ? (IAttackMethod) null : this.attackMethod_ ?? (this.attackMethod_ = this.sAttackMethod.from == BL.Weapon.From.Normal ? MasterData.AttackMethod[this.sAttackMethod.ID].CreateInterface() : (this.sAttackMethod.from == BL.Weapon.From.Guest ? MasterData.BattleStageGuestAttackMethod[this.sAttackMethod.ID].CreateInterface() : MasterData.BattleStageEnemyAttackMethod[this.sAttackMethod.ID].CreateInterface()));
        }
        private set
        {
          this.attackMethod_ = value;
          if (value == null)
            this.sAttackMethod.ID = 0;
          else if (value.original is AttackMethod)
          {
            this.sAttackMethod.ID = ((AttackMethod) value.original).ID;
            this.sAttackMethod.from = BL.Weapon.From.Normal;
          }
          else if (value.original is BattleStageEnemyAttackMethod)
          {
            this.sAttackMethod.ID = ((BattleStageEnemyAttackMethod) value.original).ID;
            this.sAttackMethod.from = BL.Weapon.From.Enemy;
          }
          else
          {
            this.sAttackMethod.ID = ((BattleStageGuestAttackMethod) value.original).ID;
            this.sAttackMethod.from = BL.Weapon.From.Guest;
          }
        }
      }

      public int gearId
      {
        get => this.mGearId;
        private set
        {
          this.mGearId = value;
          ++this.revision;
        }
      }

      public GearGear gear => MasterData.GearGear[this.mGearId];

      public int assistId
      {
        get => this.mAssistId;
        private set
        {
          this.mAssistId = value;
          ++this.revision;
        }
      }

      public GearGear assist
      {
        get => this.mAssistId == 0 ? (GearGear) null : MasterData.GearGear[this.mAssistId];
      }

      public BL.Unit.GearRange getGearRange()
      {
        GearGear gear = this.gear;
        int minRange = gear.min_range;
        int maxRange = gear.max_range;
        GearGear assist = this.assist;
        if (assist != null)
        {
          if (!this.mIsAwaked && minRange < assist.min_range)
            minRange = assist.min_range;
          if (maxRange < assist.max_range)
            maxRange = assist.max_range;
        }
        return new BL.Unit.GearRange(minRange, maxRange);
      }

      public Weapon(PlayerUnit playerUnit)
      {
        GearGear weaponGearOrInitial = playerUnit.equippedWeaponGearOrInitial;
        this.gearId = weaponGearOrInitial.ID;
        GearGear equippedAssistGear = playerUnit.equippedAssistGear;
        if (equippedAssistGear != null && weaponGearOrInitial != equippedAssistGear)
          this.assistId = equippedAssistGear.ID;
        this.mIsAwaked = playerUnit.unit.awake_unit_flag;
      }

      public Weapon(int gearID, int assistGearID = 0, bool bAwaked = false)
      {
        this.gearId = gearID;
        if (assistGearID != 0)
          this.assistId = assistGearID;
        this.mIsAwaked = bAwaked;
      }

      public Weapon(IAttackMethod iAttack) => this.attackMethod = iAttack;

      public enum From
      {
        Normal,
        Guest,
        Enemy,
      }

      [Serializable]
      public struct SAttackMethod
      {
        public int ID;
        public BL.Weapon.From from;
      }
    }

    [Serializable]
    public class ModelBase : IComparable
    {
      public int revision;
      [SerializeField]
      private bool mIsEnable = true;

      public bool isEnable
      {
        get => this.mIsEnable;
        set
        {
          this.mIsEnable = value;
          ++this.revision;
        }
      }

      public int commit() => this.revision++;

      public int CompareTo(object o) => !(o is BL.ModelBase modelBase) || modelBase != this ? 1 : 0;
    }

    [Serializable]
    public class StructValue<T> : BL.ModelBase where T : struct
    {
      [SerializeField]
      private T mValue;

      public StructValue()
      {
      }

      public StructValue(T v) => this.mValue = v;

      public T value
      {
        get => this.mValue;
        set
        {
          this.mValue = value;
          ++this.revision;
        }
      }
    }

    [Serializable]
    public class ClassValue<T> : BL.ModelBase where T : class
    {
      [SerializeField]
      private T mValue;

      public ClassValue(T v) => this.mValue = v;

      public T value
      {
        get => this.mValue;
        set
        {
          if ((object) this.mValue == (object) value)
            return;
          this.mValue = value;
          ++this.revision;
        }
      }
    }

    public class BattleModified<T> where T : BL.ModelBase
    {
      private int revision;
      public T value;

      public BattleModified(T v)
      {
        if ((object) v != null)
          this.revision = v.revision - 1;
        this.value = v;
      }

      public bool isChanged => this.value.isEnable && this.value.revision != this.revision;

      public void commit() => this.revision = this.value.commit();

      public void notifyChanged() => this.revision = this.value.revision - 1;

      public bool isChangedOnce()
      {
        int num = this.isChanged ? 1 : 0;
        if (num == 0)
          return num != 0;
        this.revision = this.value.revision;
        return num != 0;
      }
    }

    public interface ISkillEffectListUnit
    {
      int hp { get; set; }

      BL.Unit originalUnit { get; }

      BL.SkillEffectList skillEffects { get; }

      bool hasOugi { get; }

      BL.Skill ougi { get; }

      BL.Skill[] skills { get; }

      bool HasAilment { get; }

      bool IsDontAction { get; }

      bool IsDontMove { get; }

      bool IsDontEvasion { get; }

      bool IsCharm { get; }

      bool IsJumping { get; }

      bool IsDontUseCommand(BL.Skill skill);

      bool IsDontUseOugi(BL.Skill skill);

      bool IsDontUseSkill(int skill_id);

      bool IsDontUseSkillEffect(BL.SkillEffect effect);

      bool CantChangeCurrent { get; }

      bool CanHeal(BattleskillSkillType skillType);

      bool HasEnabledSkillEffect(BattleskillEffectLogicEnum logic);

      IEnumerable<BL.SkillEffect> enabledSkillEffect(BattleskillEffectLogicEnum logic);

      void setSkillEffects(BL.SkillEffectList value);

      IEnumerable<BL.SkillEffect> getSkillEffects(BattleskillEffectLogicEnum logic);

      bool checkTargetAttribute(BL.Unit.TargetAttribute ta);

      int transformationGroupId { get; }

      bool checkEnableSkill(BattleskillSkill skill);

      int[] attackRange { get; }

      int[] healRange { get; }

      BL.Unit.GearRange gearRange();

      BL.Unit.MagicRange magicRange(BL.MagicBullet mb);

      Judgement.BattleParameter parameter { get; }

      int facilitySpawnOrder { get; set; }
    }
  }
}
