// Decompiled with JetBrains decompiler
// Type: Battle01CommandSkillUse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01CommandSkillUse : BattleMonoBehaviour
{
  private UIButton button;
  private BL.Unit unit;
  private BL.Skill skill;
  private List<BL.Unit> targets;
  private List<BL.Panel> panels;

  private void Awake()
  {
    this.button = ((Component) this).GetComponent<UIButton>();
    EventDelegate.Set(this.button.onClick, new EventDelegate((MonoBehaviour) this, "onClick"));
  }

  public void setData(BL.Unit unit, BL.Skill skill, List<BL.Unit> targets, List<BL.Panel> panels)
  {
    this.unit = unit;
    this.skill = skill;
    this.targets = targets;
    this.panels = panels;
  }

  public void setEnable(bool enable) => ((UIButtonColor) this.button).isEnabled = enable;

  public void onClick()
  {
    if (!this.battleManager.isBattleEnable)
      return;
    if (this.skill.skill.skill_type == BattleskillSkillType.call)
    {
      if (this.battleManager.useGameEngine)
        this.battleManager.gameEngine.useCallSkill(this.skill, this.targets, true);
      else
        this.env.useCallSkill(this.skill, this.targets, this.battleManager.getManager<BattleTimeManager>());
      this.battleManager.getController<BattleInputObserver>().cancelTargetSelect();
      this.setEnable(false);
      Battle01SkillUse inParents = NGUITools.FindInParents<Battle01SkillUse>(((Component) this).transform);
      inParents.clearModified();
      inParents.SetDetailCollider(false);
      NGUITools.FindInParents<Battle01SelectNode>(((Component) this).transform).disableBackButton();
    }
    else
    {
      if (this.battleManager.useGameEngine)
      {
        this.battleManager.gameEngine.moveUnitWithSkill(this.unit, this.skill, this.targets, this.panels);
      }
      else
      {
        this.env.core.setSomeAction();
        this.env.useSkill(this.unit, this.skill, this.targets, this.panels, (BL.BattleSkillResult) null, this.battleManager.getManager<BattleTimeManager>());
      }
      this.battleManager.getController<BattleInputObserver>().cancelTargetSelect();
      NGUITools.FindInParents<Battle01SelectNode>(((Component) this).transform).backToTop();
    }
  }
}
