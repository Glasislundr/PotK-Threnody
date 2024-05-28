// Decompiled with JetBrains decompiler
// Type: Battle019_6_1_RecoveryButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Battle019_6_1_RecoveryButton : NGBattleMenuBase
{
  private AttackStatus attackStatus;
  private BL.Unit src;
  private BL.Unit dst;
  private BattleHealCharacterInfoBase player;
  public bool isComplited;

  public void setUseTarget(BL.Unit src, BL.Unit dst)
  {
    this.src = src;
    this.dst = dst;
    this.isComplited = false;
  }

  public void setAttackStatus(AttackStatus attackStatus, BattleHealCharacterInfoBase player)
  {
    this.attackStatus = attackStatus;
    this.player = player;
  }

  public void onClick()
  {
    if (this.env == null || this.isComplited)
      return;
    if (!this.battleManager.useGameEngine)
    {
      this.env.useMagicBullet(this.attackStatus.magicBullet, this.attackStatus.healAttack((BL.ISkillEffectListUnit) this.src, (BL.ISkillEffectListUnit) this.dst), this.src, ((IEnumerable<BL.Unit>) new BL.Unit[1]
      {
        this.dst
      }).ToList<BL.Unit>(), this.battleManager.getManager<BattleTimeManager>());
      this.backScene();
    }
    else
      this.battleManager.gameEngine.moveUnitWithAttack(this.src, this.dst, this.attackStatus.isHeal, this.player.getCurrentAttackIndex());
    this.isComplited = true;
  }
}
