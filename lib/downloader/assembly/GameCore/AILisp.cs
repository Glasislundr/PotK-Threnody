// Decompiled with JetBrains decompiler
// Type: GameCore.AILisp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.LispCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class AILisp : IELisp
  {
    private BL battleEnv;
    private HashSet<BL.Panel> dangerPanels;
    private HashSet<BL.Panel> mItemPanels;
    private HashSet<BL.Panel> mItemPanelsWork;
    private Dictionary<string, object> battleVariables = new Dictionary<string, object>();
    private BL.AIUnit mCurrentUnit;
    private BL.AIUnit mCurrentTarget;
    private BL.Panel mCurrentPanel;
    private AttackStatus mCurrentAttackStatus;
    private AttackStatus mCounterAttackStatus;
    private BL.Skill mCurrentSkill;
    private List<BL.AIUnit> mCurrentTargets = new List<BL.AIUnit>();

    private HashSet<BL.Panel> itemPanels
    {
      get
      {
        if (this.mItemPanels == null)
        {
          this.mItemPanels = new HashSet<BL.Panel>();
          for (int row = 0; row < this.battleEnv.getFieldHeight(); ++row)
          {
            for (int column = 0; column < this.battleEnv.getFieldWidth(); ++column)
            {
              BL.Panel fieldPanel = this.battleEnv.getFieldPanel(row, column);
              BL.DropData fieldEvent = fieldPanel.fieldEvent;
              if (fieldEvent != null && !fieldEvent.isCompleted)
                this.mItemPanels.Add(fieldPanel);
            }
          }
        }
        if (this.mItemPanelsWork == null)
        {
          HashSet<BL.Panel> panelSet = new HashSet<BL.Panel>();
          foreach (BL.Panel mItemPanel in this.mItemPanels)
          {
            if (!mItemPanel.fieldEvent.isCompleted)
              panelSet.Add(mItemPanel);
          }
          this.mItemPanelsWork = panelSet;
        }
        return this.mItemPanelsWork;
      }
    }

    public AILisp(BL e, SExpNumber n)
      : base(n)
    {
      this.init(e);
      this.timer = new Stopwatch();
      this.thresholdMS = 50L;
    }

    public void clearCache() => this.dangerPanels = (HashSet<BL.Panel>) null;

    private void init(BL e)
    {
      this.battleEnv = e;
      this.defun("init-work", (Func<Cons, object>) (args =>
      {
        this.mItemPanelsWork = (HashSet<BL.Panel>) null;
        return (object) null;
      }));
      this.defun("unit-list", (Func<Cons, object>) (args => SExp.toLispList<BL.AIUnit>((IEnumerable<BL.AIUnit>) this.battleEnv.aiUnits.value)));
      this.defun("get-value", (Func<Cons, object>) (args =>
      {
        SExpString name = this.checkType<SExpString>("get-value", args, 1);
        return this.getValue(args.car, name);
      }));
      this.defun("get-cant-change-action-unit-list", (Func<Cons, object>) (args =>
      {
        List<BL.AIUnit> aiUnitList = new List<BL.AIUnit>();
        foreach (BL.AIUnit aiUnit in this.battleEnv.aiActionUnits.value)
        {
          if (aiUnit.unitPosition.cantChangeCurrent)
          {
            aiUnitList.Add(aiUnit);
            break;
          }
        }
        return aiUnitList.Any<BL.AIUnit>() ? SExp.toLispList<BL.AIUnit>((IEnumerable<BL.AIUnit>) aiUnitList) : (object) null;
      }));
      this.defun("get-action-unit-list", (Func<Cons, object>) (args => SExp.toLispList<BL.AIUnit>((IEnumerable<BL.AIUnit>) this.battleEnv.aiActionUnits.value)));
      this.defun("get-target-unit-list", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-target-unit-list", args, 0);
        return SExp.toLispList<BL.AIUnit>((IEnumerable<BL.AIUnit>) this.battleEnv.getTargetAIUnits(aiUnit, BL.Unit.TargetAttribute.attack | BL.Unit.TargetAttribute.heal, BattleFuncs.getProvokeUnits((BL.ISkillEffectListUnit) aiUnit)));
      }));
      this.defun("cant-change-currentp", (Func<Cons, object>) (args => !this.checkType<BL.AIUnit>("change-currentp", args, 0).cantChangeCurrent ? (object) null : this.trueObject));
      this.defun("unitp", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("unitp", args, 1))
          return (object) null;
        return !(args.car is BL.AIUnit) ? (object) null : this.trueObject;
      }));
      this.defun("panelp", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("panelp", args, 1))
          return (object) null;
        return !(args.car is BL.Panel) ? (object) null : this.trueObject;
      }));
      this.defun("skillp", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("skillp", args, 1))
          return (object) null;
        return !(args.car is BL.Skill) ? (object) null : this.trueObject;
      }));
      this.defun("healerp", (Func<Cons, object>) (args => !this.checkType<BL.AIUnit>("healerp", args, 0).isHealer ? (object) null : this.trueObject));
      this.defun("actionp", (Func<Cons, object>) (args => !this.checkType<BL.AIUnit>("actionp", args, 0).isAction ? (object) null : this.trueObject));
      this.defun("item-pickup-movep", (Func<Cons, object>) (args =>
      {
        if (!this.battleEnv.isAutoItemMove.value)
          return (object) null;
        return this.checkType<BL.AIUnit>("item-pickup-movep", args, 0).getForceID(this.battleEnv) != BL.ForceID.player ? (object) null : this.trueObject;
      }));
      this.defun("unit-on-panelp", (Func<Cons, object>) (args => this.battleEnv.getFieldUnitAI(this.checkType<BL.Panel>("unit-on-panelp", args, 0)) == null ? (object) null : this.trueObject));
      this.defun("get-danger-panels", (Func<Cons, object>) (args => SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.getDangerPanels())));
      this.defun("get-item-panels", (Func<Cons, object>) (args => SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.itemPanels)));
      this.defun("get-panel-route", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-panel-route", args, 0);
        BL.Panel target = this.checkType<BL.Panel>("get-panel-route", args, 1);
        return SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.createTargetRoute((BL.UnitPosition) aiUnit, this.battleEnv.getFieldPanel((BL.UnitPosition) aiUnit), target).Item1);
      }));
      this.defun("get-target-route", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-target-route", args, 0);
        BL.Panel fieldPanel = this.battleEnv.getFieldPanel((BL.UnitPosition) aiUnit);
        return SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.createTargetRoute((BL.UnitPosition) aiUnit, fieldPanel, this.getTargetPanel(aiUnit, fieldPanel)).Item1);
      }));
      this.defun("ownp", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit1 = this.checkType<BL.AIUnit>("ownp", args, 0);
        BL.AIUnit aiUnit2 = this.checkType<BL.AIUnit>("ownp", args, 1);
        BL battleEnv = this.battleEnv;
        return aiUnit1.getForceID(battleEnv) != aiUnit2.getForceID(this.battleEnv) ? (object) null : this.trueObject;
      }));
      this.defun("action-completedp", (Func<Cons, object>) (args => !this.checkType<BL.AIUnit>("action-completedp", args, 0).isActionComleted ? (object) null : this.trueObject));
      this.defun("completedp", (Func<Cons, object>) (args => !this.checkType<BL.AIUnit>("completedp", args, 0).isCompleted ? (object) null : this.trueObject));
      this.defun("charmp", (Func<Cons, object>) (args => !this.checkType<BL.AIUnit>("charmp", args, 0).IsCharm ? (object) null : this.trueObject));
      this.defun("get-move-panels", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-move-panels", args, 0);
        return aiUnit != null ? SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) aiUnit.movePanels) : (object) null;
      }));
      this.defun("get-complete-panels", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-complete-panels", args, 0);
        return aiUnit != null ? SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) aiUnit.completePanels) : (object) null;
      }));
      this.defun("get-panel", (Func<Cons, object>) (args => (object) this.battleEnv.getFieldPanel((BL.UnitPosition) this.checkType<BL.AIUnit>("get-panel", args, 0))));
      this.defun("set-position", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("set-position", args, 0);
        BL.Panel panel = this.checkType<BL.Panel>("set-position", args, 1);
        aiUnit.row = panel.row;
        aiUnit.column = panel.column;
        return (object) aiUnit;
      }));
      this.defun("reset-position", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("set-position", args, 0);
        aiUnit.row = aiUnit.originalRow;
        aiUnit.column = aiUnit.originalColumn;
        return (object) aiUnit;
      }));
      this.defun("get-skill", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-skill", args, 0);
        if (this.nth(1, args) == null)
          return (object) null;
        int skillId = (int) this.checkType<Decimal?>("get-skill", args, 1).Value;
        if (aiUnit.commandSkills.Any<BL.Skill>((Func<BL.Skill, bool>) (x => x.id == skillId)))
        {
          BL.Skill skill = aiUnit.getSkill(skillId);
          if (aiUnit.IsDontUseCommand(skill))
            return (object) null;
          if (!aiUnit.checkEnableSkill(skill.skill))
            return (object) null;
          int? remain = skill.remain;
          if (remain.HasValue)
          {
            remain = skill.remain;
            if (remain.HasValue)
            {
              remain = skill.remain;
              int num = 0;
              if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                goto label_19;
            }
            else
              goto label_19;
          }
          return (object) skill;
        }
        if (aiUnit.hasOugi && aiUnit.ougi.id == skillId)
        {
          BL.Skill skill = aiUnit.getSkill(skillId);
          if (aiUnit.IsDontUseOugi(skill) || !aiUnit.ougi.canUseTurn(this.battleEnv.phaseState.turnCount))
            return (object) null;
          if (skill.useTurn - this.battleEnv.phaseState.turnCount <= 0)
          {
            int? remain = skill.remain;
            if (remain.HasValue)
            {
              remain = skill.remain;
              if (remain.HasValue)
              {
                remain = skill.remain;
                int num = 0;
                if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                  goto label_19;
              }
              else
                goto label_19;
            }
            return (object) skill;
          }
        }
label_19:
        return (object) null;
      }));
      this.defun("get-use-skills", (Func<Cons, object>) (args =>
      {
        BL.AIUnit u = this.checkType<BL.AIUnit>("get-use-skills", args, 0);
        List<BL.Skill> list = u.commandSkills.Where<BL.Skill>((Func<BL.Skill, bool>) (x =>
        {
          if (x.remain.HasValue)
          {
            if (x.remain.HasValue)
            {
              int? remain = x.remain;
              int num = 0;
              if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                goto label_5;
            }
            else
              goto label_5;
          }
          if (!u.IsDontUseCommand(x))
            return u.checkEnableSkill(x.skill);
label_5:
          return false;
        })).ToList<BL.Skill>();
        if (u.hasOugi && u.ougi.useTurn - this.battleEnv.phaseState.turnCount <= 0)
        {
          if (u.ougi.remain.HasValue)
          {
            if (u.ougi.remain.HasValue)
            {
              int? remain = u.ougi.remain;
              int num = 0;
              if (!(remain.GetValueOrDefault() > num & remain.HasValue))
                goto label_6;
            }
            else
              goto label_6;
          }
          if (!u.IsDontUseOugi(u.ougi) && u.ougi.canUseTurn(this.battleEnv.phaseState.turnCount))
            list.Add(u.ougi);
        }
label_6:
        return SExp.toLispList<BL.Skill>((IEnumerable<BL.Skill>) list);
      }));
      this.defun("skill-nonselectp", (Func<Cons, object>) (args => !this.checkType<BL.Skill>("skill-nonselectp", args, 0).isNonSelect ? (object) null : this.trueObject));
      this.defun("action-move-panels", (Func<Cons, object>) (args => SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.checkType<BL.AIUnit>("action-move-panels", args, 0).actionMovePanels)));
      this.defun("heal-move-panels", (Func<Cons, object>) (args => SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.checkType<BL.AIUnit>("heal-move-panels", args, 0).healMovePanels)));
      this.defun("skill-move-panels", (Func<Cons, object>) (args => SExp.toLispList<BL.Panel>((IEnumerable<BL.Panel>) this.checkType<BL.AIUnit>("skill-move-panels", args, 0).getSkillMovePanels(this.checkType<BL.Skill>("skill-move-panels", args, 1)))));
      this.defun("attack-targets", (Func<Cons, object>) (args => SExp.toLispList<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) BattleFuncs.getAttackTargets((BL.UnitPosition) this.checkType<BL.AIUnit>("attack-targets", args, 0), isAI: true))));
      this.defun("skill-targets", (Func<Cons, object>) (args => SExp.toLispList<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) this.getValidSkillTargets(this.checkType<BL.AIUnit>("skill-targets", args, 0), this.checkType<BL.Skill>("skill-targets", args, 1)))));
      this.defun("heal-targets", (Func<Cons, object>) (args => SExp.toLispList<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) BattleFuncs.getHealTargets((BL.UnitPosition) this.checkType<BL.AIUnit>("heal-targets", args, 0), isAI: true))));
      this.defun("create-attack-status-list", (Func<Cons, object>) (args =>
      {
        BL.AIUnit attack = this.checkType<BL.AIUnit>("create-attack-status-list", args, 0);
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("create-attack-status-list", args, 1);
        bool flag = this.nth(2, args) != null;
        BL.AIUnit defense = aiUnit;
        int num = flag ? 1 : 0;
        return SExp.toLispList<AttackStatus>((IEnumerable<AttackStatus>) BattleFuncs.getAttackStatusArray((BL.UnitPosition) attack, (BL.UnitPosition) defense, true, num != 0, true));
      }));
      this.defun("hit-attack-status-cache", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit3 = this.checkType<BL.AIUnit>("hit-attack-status-cache", args, 0);
        BL.AIUnit aiUnit4 = this.checkType<BL.AIUnit>("hit-attack-status-cache", args, 1);
        bool isHeal = this.nth(2, args) != null;
        BL.Unit[] attackNeighbors;
        BL.Unit[] defenseNeighbors;
        int move_distance;
        int move_range;
        BattleFuncs.makeAttackStatusArgs((BL.UnitPosition) aiUnit3, (BL.UnitPosition) aiUnit4, true, isHeal, out attackNeighbors, out defenseNeighbors, out move_distance, out move_range, isAI: true);
        return !this.battleEnv.getAttackStatusCache((BL.ISkillEffectListUnit) aiUnit3, this.battleEnv.getFieldPanel((BL.UnitPosition) aiUnit3), attackNeighbors, (BL.ISkillEffectListUnit) aiUnit4, this.battleEnv.getFieldPanel((BL.UnitPosition) aiUnit4), defenseNeighbors, aiUnit3.hp, true, isHeal, move_distance, move_range, true, out AttackStatus[] _) ? (object) null : this.trueObject;
      }));
      this.defun("create-action-result", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit5 = this.checkType<BL.AIUnit>("create-action-result", args, 0);
        AttackStatus attackStatus = this.checkType<AttackStatus>("create-action-result", args, 1);
        BL.AIUnit aiUnit6 = this.checkType<BL.AIUnit>("create-action-result", args, 2);
        BL.Panel fieldPanel1 = this.battleEnv.getFieldPanel(aiUnit5.originalRow, aiUnit5.originalColumn);
        BL.Panel fieldPanel2 = this.battleEnv.getFieldPanel(aiUnit5.row, aiUnit5.column);
        BL.Panel fieldPanel3 = this.battleEnv.getFieldPanel(aiUnit6.row, aiUnit6.column);
        int num1 = BL.fieldDistance(fieldPanel1, fieldPanel3) - 1;
        int num2 = BL.fieldDistance(fieldPanel1, fieldPanel2);
        BL.AIUnit attack = aiUnit5;
        BL.Panel attackPanel = fieldPanel2;
        BL.AIUnit defense = aiUnit6;
        BL.Panel defensePanel = fieldPanel3;
        int move_distance = num1;
        int move_range = num2;
        DuelResult duelResult = BattleFuncs.calcDuel(attackStatus, (BL.ISkillEffectListUnit) attack, attackPanel, (BL.ISkillEffectListUnit) defense, defensePanel, move_distance, move_range, true);
        duelResult.isMove = true;
        duelResult.row = aiUnit5.row;
        duelResult.column = aiUnit5.column;
        return (object) duelResult;
      }));
      this.defun("create-skill-result", (Func<Cons, object>) (args =>
      {
        BL.AIUnit unit = this.checkType<BL.AIUnit>("create-skill-result", args, 0);
        BL.Skill skill = this.checkType<BL.Skill>("create-skill-result", args, 1);
        Cons cons = this.checkTypeCons("create-skill-result", args, 2);
        return (object) this.battleEnv.createBattleSkillResult(skill, unit, SExp.toCSList<BL.AIUnit>((object) cons));
      }));
      this.defun("action-unit", (Func<Cons, object>) (args =>
      {
        BL.AIUnit u = this.checkType<BL.AIUnit>("action-unit", args, 0);
        ActionResult actionResult = this.checkType<ActionResult>("action-unit", args, 1);
        BL.BattleModified<BL.ClassValue<List<BL.AIUnit>>> battleModified = BL.Observe<BL.ClassValue<List<BL.AIUnit>>>(this.battleEnv.aiUnitPositions);
        battleModified.isChangedOnce();
        if (u.actionResults == null)
          u.actionResults = new List<ActionResult>();
        u.actionResults.Add(actionResult);
        u.actionAIUnit(this.battleEnv);
        this.actionComplitedClearWork(u);
        int num = battleModified.isChanged ? 1 : 0;
        if (num != 0)
          this.clearCache();
        return num == 0 ? (object) null : this.trueObject;
      }));
      this.defun("complete-unit", (Func<Cons, object>) (args =>
      {
        BL.AIUnit u = this.checkType<BL.AIUnit>("complete-unit", args, 0);
        u.completeAIUnit(this.battleEnv, true);
        this.actionComplitedClearWork(u);
        return (object) u;
      }));
      this.defun("unit-extdata", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("get-unit-extdata", args, 0);
        object key = this.nth(1, args);
        return aiUnit.unit.aiExtension != null ? SExp.cdr(SExp.assoc(key, aiUnit.unit.aiExtension)) : (object) null;
      }));
      this.defun("clear-variables", (Func<Cons, object>) (args =>
      {
        this.battleVariables.Clear();
        return (object) null;
      }));
      this.defun("clear-danger-panels", (Func<Cons, object>) (args => (object) (this.dangerPanels = (HashSet<BL.Panel>) null)));
      this.defun("set-current", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit7 = this.checkType<BL.AIUnit>("set-current", args, 0);
        BL.AIUnit aiUnit8 = this.checkType<BL.AIUnit>("set-current", args, 1);
        BL.Panel panel = this.checkType<BL.Panel>("set-current", args, 2);
        AttackStatus attackStatus = this.checkType<AttackStatus>("set-current", args, 3);
        this.currentUnit = aiUnit7;
        this.currentTarget = aiUnit8;
        this.currentPanel = panel;
        this.currentAttackStatus = attackStatus;
        this.counterAttackStatus = (AttackStatus) null;
        return (object) aiUnit7;
      }));
      this.defun("set-current-unit", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("set-current-unit", args, 0);
        this.currentUnit = aiUnit;
        this.counterAttackStatus = (AttackStatus) null;
        return (object) aiUnit;
      }));
      this.defun("set-current-target", (Func<Cons, object>) (args =>
      {
        BL.AIUnit aiUnit = this.checkType<BL.AIUnit>("set-current-target", args, 0);
        this.currentTarget = aiUnit;
        this.counterAttackStatus = (AttackStatus) null;
        return (object) aiUnit;
      }));
      this.defun("set-current-attack-status", (Func<Cons, object>) (args =>
      {
        AttackStatus attackStatus = this.checkType<AttackStatus>("set-current-attack-status", args, 0);
        this.currentAttackStatus = attackStatus;
        return (object) attackStatus;
      }));
      this.defun("set-current-panel", (Func<Cons, object>) (args =>
      {
        BL.Panel panel = this.checkType<BL.Panel>("set-current-panel", args, 0);
        this.currentPanel = panel;
        return (object) panel;
      }));
      this.defun("set-current-skill", (Func<Cons, object>) (args =>
      {
        BL.Skill skill = this.checkType<BL.Skill>("set-current-skill", args, 0);
        this.currentSkill = skill;
        return (object) skill;
      }));
      this.defun("set-current-targets", (Func<Cons, object>) (args =>
      {
        this.currentTargets.Clear();
        Cons cons = this.checkTypeCons("set-current-targets", args, 0);
        int num = SExp.length((object) cons);
        for (int idx = 0; idx < num; ++idx)
          this.currentTargets.Add(this.checkType<BL.AIUnit>("set-current-targets", cons, idx));
        return (object) this.currentTargets;
      }));
      this.defun("current-unit-has-mbp", (Func<Cons, object>) (args => !this.hasMagicBullet(this.currentUnit, (int) this.checkType<Decimal?>("current-unit-has-mbp", args, 0).Value) ? (object) null : this.trueObject));
      this.defun("current-target-has-mbp", (Func<Cons, object>) (args => !this.hasMagicBullet(this.currentTarget, (int) this.checkType<Decimal?>("current-target-has-mbp", args, 0).Value) ? (object) null : this.trueObject));
      this.defun("get-attack-point-after-func", (Func<Cons, object>) (args =>
      {
        this.checkType<BL.AIUnit>("get-attack-point-after-func", args, 0);
        return (object) null;
      }));
      this.defun("get-attack-point-func", (Func<Cons, object>) (args =>
      {
        this.checkType<BL.AIUnit>("get-attack-point-func", args, 0);
        return (object) null;
      }));
      this.defun("get-heal-point-func", (Func<Cons, object>) (args =>
      {
        this.checkType<BL.AIUnit>("get-heal-point-func", args, 0);
        return (object) null;
      }));
      this.defun("get-move-point-func", (Func<Cons, object>) (args =>
      {
        this.checkType<BL.AIUnit>("get-move-point-func", args, 0);
        return (object) null;
      }));
      this.defun("get-skill-point-func", (Func<Cons, object>) (args =>
      {
        string aiSkillFunction = this.checkType<BL.AIUnit>("get-skill-point-func", args, 0).unit.playerUnit.ai_skill_function;
        return !string.IsNullOrEmpty(aiSkillFunction) && this.globalEnv.ContainsKey(aiSkillFunction) ? this.globalEnv[aiSkillFunction] : (object) null;
      }));
    }

    protected override object symbolValE(string sym, Stack<Dictionary<string, object>> es)
    {
      string[] split = sym.Split('@');
      return this.isBattleVariable(split) ? this.getBattleVariable(split) : base.symbolValE(sym, es);
    }

    private BL.AIUnit currentUnit
    {
      get => this.mCurrentUnit;
      set => this.mCurrentUnit = value;
    }

    private BL.AIUnit currentTarget
    {
      get => this.mCurrentTarget;
      set => this.mCurrentTarget = value;
    }

    private BL.Panel currentPanel
    {
      get => this.mCurrentPanel;
      set => this.mCurrentPanel = value;
    }

    private AttackStatus currentAttackStatus
    {
      get => this.mCurrentAttackStatus;
      set => this.mCurrentAttackStatus = value;
    }

    private AttackStatus counterAttackStatus
    {
      get => this.mCounterAttackStatus;
      set => this.mCounterAttackStatus = value;
    }

    private BL.Skill currentSkill
    {
      get => this.mCurrentSkill;
      set => this.mCurrentSkill = value;
    }

    private List<BL.AIUnit> currentTargets
    {
      get => this.mCurrentTargets;
      set => this.mCurrentTargets = value;
    }

    private void actionComplitedClearWork(BL.AIUnit u)
    {
      this.battleVariables.Remove("is_own_team_can_attacked");
      if (this.mItemPanelsWork == null || u.cantChangeCurrent)
        return;
      BL.Panel fieldPanel = this.battleEnv.getFieldPanel(u.row, u.column);
      if (!this.mItemPanelsWork.Contains(fieldPanel))
        return;
      this.mItemPanelsWork.Remove(fieldPanel);
    }

    private bool isBattleVariable(string[] split) => split.Length == 2;

    private object getBattleVariable(string[] split)
    {
      string str = split[1];
      object battleVariable;
      switch (split[0])
      {
        case "own":
          battleVariable = this.getUnitVariable(this.currentUnit, this.currentTarget, this.currentAttackStatus, str);
          break;
        case "target":
          battleVariable = this.getUnitVariable(this.currentTarget, this.currentUnit, (AttackStatus) null, str);
          break;
        case "panel":
          battleVariable = this.getPanelVariable(this.currentPanel, this.currentUnit, this.currentTarget, str);
          break;
        case "skill":
          battleVariable = this.getSkillVariable(this.currentSkill, this.currentTargets, str);
          break;
        case "battle":
          if (this.battleVariables.ContainsKey(str))
          {
            battleVariable = this.battleVariables[str];
            break;
          }
          switch (str)
          {
            case "sum_own":
              battleVariable = (object) this.numberDic.numberObject(this.battleEnv.getTargetAIUnits(new BL.ForceID[1]
              {
                this.currentUnit.getForceID(this.battleEnv)
              }, BL.Unit.TargetAttribute.attack).Count);
              break;
            case "sum_enemy":
              battleVariable = (object) this.numberDic.numberObject(this.battleEnv.getTargetAIUnits(this.currentUnit, BL.Unit.TargetAttribute.attack).Count);
              break;
            case "turn":
              battleVariable = (object) this.numberDic.numberObject(this.battleEnv.phaseState.turnCount);
              break;
            case "is_own_team_can_attacked":
              battleVariable = (object) null;
              using (List<BL.AIUnit>.Enumerator enumerator = this.battleEnv.aiUnits.value.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  BL.AIUnit current = enumerator.Current;
                  if (this.getDangerPanels().Contains(this.battleEnv.getFieldPanel(current.originalRow, current.originalColumn)))
                  {
                    battleVariable = this.trueObject;
                    break;
                  }
                }
                break;
              }
            case "sum_enemy_range_1":
              battleVariable = (object) this.numberDic.numberObject(this.battleEnv.getAroundEnemyUnitsCount(this.currentUnit, 1));
              break;
            case "sum_enemy_range_2":
              battleVariable = (object) this.numberDic.numberObject(this.battleEnv.getAroundEnemyUnitsCount(this.currentUnit, 2));
              break;
            default:
              battleVariable = (object) null;
              break;
          }
          this.battleVariables[str] = battleVariable;
          break;
        case "current":
          switch (str)
          {
            case "unit":
              battleVariable = (object) this.currentUnit;
              break;
            case "target":
              battleVariable = (object) this.currentTarget;
              break;
            case "panel":
              battleVariable = (object) this.currentPanel;
              break;
            case "skill":
              battleVariable = (object) this.currentSkill;
              break;
            default:
              battleVariable = (object) null;
              break;
          }
          break;
        default:
          battleVariable = (object) null;
          break;
      }
      return battleVariable;
    }

    private object getPanelVariable(BL.Panel panel, BL.AIUnit unit, BL.AIUnit target, string sym)
    {
      BattleLandformIncr incr = panel.landform.GetIncr(unit.unit);
      object panelVariable = (object) null;
      switch (sym)
      {
        case "enemy_distance":
          panelVariable = (object) this.numberDic.numberObject(this.createTargetRoute((BL.UnitPosition) unit, panel, this.getTargetPanel(unit, panel)).Item1.Count);
          break;
        case "eva":
          panelVariable = (object) this.numberDic.numberObject(incr.evasion_incr);
          break;
        case "heal":
          panelVariable = (object) this.numberDic.numberObject(incr.hp_healing_ratio);
          break;
        case "is_danger":
          panelVariable = this.getDangerPanels().Contains(panel) ? this.trueObject : (object) null;
          break;
        case "magic":
          panelVariable = (object) this.numberDic.numberObject(incr.magic_defense_incr);
          break;
        case "move_distance":
          panelVariable = (object) this.numberDic.numberObject(this.battleEnv.getTargetRouteWithCache((BL.UnitPosition) unit, this.battleEnv.getFieldPanel(unit.originalRow, unit.originalColumn), panel, false).Item1.Count);
          break;
        case "phisics":
          panelVariable = (object) this.numberDic.numberObject(incr.physical_defense_incr);
          break;
      }
      return panelVariable;
    }

    private object getUnitVariable(
      BL.AIUnit unit,
      BL.AIUnit target,
      AttackStatus attackStatus,
      string sym)
    {
      switch (sym)
      {
        case "attack":
          return this.calcAttack(unit, target, attackStatus);
        case "combat":
          return (object) this.numberDic.numberObject(unit.Combat);
        case "critical":
          return this.calcCritical(unit, target, attackStatus);
        case "has_leader_skill":
          return !unit.unit.is_leader && !unit.unit.is_helper || unit.unit.playerUnit.leader_skills.Length == 0 ? (object) null : this.trueObject;
        case "hit":
          return this.calcHit(unit, target, attackStatus);
        case "hp":
          return (object) this.numberDic.numberObject((float) unit.hp / (float) unit.unit.parameter.Hp);
        case "is_buster":
          return this.calcIsBuster(unit, target, attackStatus);
        case "magic_cost":
          return this.calcMagicCost(unit, target, attackStatus);
        case "max_hp":
          return (object) this.numberDic.numberObject(unit.unit.parameter.Hp);
        case "move_type":
          return (object) this.numberDic.numberObject((int) unit.unit.job.move_type);
        case "rarity":
          return (object) this.numberDic.numberObject(unit.rarity);
        case "skill_level":
          return (object) this.numberDic.numberObject(unit.skillLevel);
        default:
          return (object) null;
      }
    }

    private object getSkillVariable(BL.Skill skill, List<BL.AIUnit> targets, string sym)
    {
      switch (sym)
      {
        case "id":
          return (object) this.numberDic.numberObject(this.currentSkill.id);
        case "genre_id":
          return (object) this.numberDic.numberObject((int) skill.genre1.Value);
        case "target_type":
          return (object) this.numberDic.numberObject((int) skill.targetType);
        case "sum_target_num":
          return (object) this.numberDic.numberObject(targets.Count);
        case "sum_target_hp":
          int num1 = 0;
          int num2 = 0;
          foreach (BL.AIUnit target in targets)
          {
            num1 += target.unit.hp;
            num2 += target.unit.parameter.Hp;
          }
          return (object) this.numberDic.numberObject((float) num1 / (float) num2);
        case "sum_target_combat":
          int i = 0;
          foreach (BL.AIUnit target in targets)
            i += target.Combat;
          return (object) this.numberDic.numberObject(i);
        default:
          return (object) null;
      }
    }

    private AttackStatus calcCounterAttack()
    {
      if (this.counterAttackStatus == null)
      {
        BL.Unit[] attackNeighbors;
        BL.Unit[] defenseNeighbors;
        int move_distance;
        int move_range;
        BattleFuncs.makeAttackStatusArgs((BL.UnitPosition) this.currentUnit, (BL.UnitPosition) this.currentTarget, true, false, out attackNeighbors, out defenseNeighbors, out move_distance, out move_range, movePanel: this.currentPanel, isAI: true);
        this.counterAttackStatus = BattleFuncs.getCounterAttack((BL.ISkillEffectListUnit) this.currentUnit, this.currentPanel, attackNeighbors, (BL.ISkillEffectListUnit) this.currentTarget, this.battleEnv.getFieldPanel((BL.UnitPosition) this.currentTarget), defenseNeighbors, false, false, move_distance, move_range, true);
      }
      return this.counterAttackStatus;
    }

    private object calcIsBuster(BL.AIUnit unit, BL.AIUnit target, AttackStatus attackStatus)
    {
      if (attackStatus == null)
      {
        attackStatus = this.calcCounterAttack();
        if (attackStatus == null)
          return (object) null;
      }
      int num = attackStatus.attack * attackStatus.attackCount;
      return (double) (target.hp - num) > 0.0 ? (object) null : this.trueObject;
    }

    private object calcAttack(BL.AIUnit unit, BL.AIUnit target, AttackStatus attackStatus)
    {
      if (attackStatus == null)
      {
        attackStatus = this.calcCounterAttack();
        if (attackStatus == null)
          return (object) this.numberDic.numberObject(0);
      }
      return (object) this.numberDic.numberObject(NC.Clampf(0.0f, 1f, (float) (attackStatus.attack * attackStatus.attackCount) / (float) target.hp));
    }

    private object calcHit(BL.AIUnit unit, BL.AIUnit target, AttackStatus attackStatus)
    {
      if (attackStatus == null)
      {
        attackStatus = this.calcCounterAttack();
        if (attackStatus == null)
          return (object) this.numberDic.numberObject(0);
      }
      return (object) this.numberDic.numberObject(attackStatus.hit);
    }

    private object calcCritical(BL.AIUnit unit, BL.AIUnit target, AttackStatus attackStatus)
    {
      if (attackStatus == null)
      {
        attackStatus = this.calcCounterAttack();
        if (attackStatus == null)
          return (object) this.numberDic.numberObject(0);
      }
      return (object) this.numberDic.numberObject(attackStatus.critical);
    }

    private object calcMagicCost(BL.AIUnit unit, BL.AIUnit target, AttackStatus attackStatus)
    {
      if (attackStatus == null)
      {
        attackStatus = this.calcCounterAttack();
        if (attackStatus == null)
          return (object) this.numberDic.numberObject(0);
      }
      return attackStatus.magicBullet != null ? (object) this.numberDic.numberObject((float) attackStatus.magicBullet.cost / (float) unit.unit.parameter.Hp) : (object) this.numberDic.numberObject(0);
    }

    private bool hasMagicBullet(BL.AIUnit unit, int id)
    {
      foreach (BL.MagicBullet magicBullet in unit.unit.magicBullets)
      {
        if (magicBullet.skillId == id)
          return true;
      }
      return false;
    }

    public Tuple<List<BL.Panel>, int> createTargetRoute(
      BL.UnitPosition u,
      BL.Panel panel,
      BL.Panel target)
    {
      bool im = (u is BL.ISkillEffectListUnit ? u as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) u.unit).HasEnabledSkillEffect(BattleskillEffectLogicEnum.ignore_move_cost);
      return this.battleEnv.getTargetRouteWithCache(u, panel, target, im);
    }

    private BL.Panel getTargetPanel(BL.AIUnit unit, BL.Panel panel)
    {
      BL.Panel targetPanel = unit.GetTargetPanelOrNull(this.battleEnv);
      if (targetPanel == null)
      {
        targetPanel = this.getAIGroupTarget(unit);
        if (targetPanel == null)
        {
          List<BL.AIUnit> targetAiUnits = this.battleEnv.getTargetAIUnits(unit, BL.Unit.TargetAttribute.attack, BattleFuncs.getProvokeUnits((BL.ISkillEffectListUnit) unit));
          if (targetAiUnits.Count == 0)
          {
            targetPanel = this.battleEnv.getFieldPanel((BL.UnitPosition) unit);
          }
          else
          {
            int num1 = 1000000000;
            foreach (BL.UnitPosition up in targetAiUnits)
            {
              BL.Panel fieldPanel = this.battleEnv.getFieldPanel(up);
              int num2 = this.createTargetRoute((BL.UnitPosition) unit, panel, fieldPanel).Item2;
              if (num2 < num1)
              {
                targetPanel = fieldPanel;
                num1 = num2;
              }
            }
          }
        }
      }
      return targetPanel;
    }

    private BL.Panel getAIGroupTarget(BL.AIUnit unit)
    {
      foreach (BL.AIUnit targetAiUnit in this.battleEnv.getTargetAIUnits(unit, BL.Unit.TargetAttribute.attack, BattleFuncs.getProvokeUnits((BL.ISkillEffectListUnit) unit)))
      {
        if (targetAiUnit.unit.IsAIMoveGroup && targetAiUnit.unit.AIMoveGroup == unit.unit.AIMoveGroup)
          return this.battleEnv.getFieldPanel((BL.UnitPosition) targetAiUnit);
      }
      return (BL.Panel) null;
    }

    private HashSet<BL.Panel> getDangerPanels()
    {
      if (this.dangerPanels == null)
        this.dangerPanels = BattleFuncs.createDangerPanels<BL.AIUnit>((IEnumerable<BL.AIUnit>) this.battleEnv.getTargetAIUnits(this.battleEnv.aiUnits.value[0], BL.Unit.TargetAttribute.attack));
      return this.dangerPanels;
    }

    private List<BL.UnitPosition> getValidSkillTargets(BL.AIUnit aiUnit, BL.Skill skill)
    {
      List<BL.UnitPosition> source = this.battleEnv.getSkillTargetUnits((BL.UnitPosition) aiUnit, skill);
      List<BL.Unit> provokeUnits = BattleFuncs.getProvokeUnits((BL.ISkillEffectListUnit) aiUnit);
      if (provokeUnits != null)
      {
        if (skill.isNonSelect)
        {
          if (!source.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
          {
            if (!provokeUnits.Contains(x.unit))
              return false;
            BL.AIUnit unit = (BL.AIUnit) x;
            return unit.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) unit, this.battleEnv, (BL.ISkillEffectListUnit) aiUnit, skill.nowUseCount) == 0;
          })))
            source = new List<BL.UnitPosition>();
        }
        else
        {
          List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
          foreach (BL.UnitPosition unitPosition in source)
          {
            if (!provokeUnits.Contains(unitPosition.unit))
              unitPositionList.Add(unitPosition);
          }
          foreach (BL.UnitPosition unitPosition in unitPositionList)
            source.Remove(unitPosition);
        }
      }
      List<BL.UnitPosition> validSkillTargets = new List<BL.UnitPosition>();
      foreach (BL.UnitPosition unitPosition in source)
      {
        BL.AIUnit unit = (BL.AIUnit) unitPosition;
        if (unit.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) unit, this.battleEnv, (BL.ISkillEffectListUnit) aiUnit, skill.nowUseCount) == 0)
          validSkillTargets.Add(unitPosition);
      }
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.random_choice && x.checkLevel(skill.level)));
      if (battleskillEffect != null && validSkillTargets.Count < battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_unit_count))
        validSkillTargets.Clear();
      return validSkillTargets;
    }

    private object getValue(object o, SExpString name)
    {
      string strValue = name.strValue;
      System.Type type = o.GetType();
      if (type != (System.Type) null)
      {
        FieldInfo field = type.GetField(strValue);
        if (field != (FieldInfo) null)
          return field.GetValue(o);
      }
      return (object) null;
    }
  }
}
