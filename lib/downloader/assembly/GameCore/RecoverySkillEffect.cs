// Decompiled with JetBrains decompiler
// Type: GameCore.RecoverySkillEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GameCore
{
  public class RecoverySkillEffect
  {
    public int effectId;
    public int skillId;
    public int level;
    public int? turnRemain;
    public int? useRemain;
    public int? executeRemain;
    public int killCount;
    public bool isBaseSkill;
    public int gearIndex;
    public int unitNetworkId;
    public int investUnitNetworkId;
    public int investSkillId;
    public float[] work;
    public bool isDontDisplay;
    public int turnCount;
    public bool againInvoked;
    public int? moveDistance;
    public bool isAttackMethod;
    public int investTurn;
    public bool dontCleanUseRemain;
    public int parentUnitNetworkId;

    public RecoverySkillEffect(BL.SkillEffect se, BL env)
    {
      this.effectId = se.effectId;
      this.skillId = se.baseSkillId;
      this.level = se.baseSkillLevel;
      this.turnRemain = se.turnRemain;
      this.useRemain = se.useRemain;
      this.executeRemain = se.executeRemain;
      this.killCount = se.killCount;
      this.isBaseSkill = se.isBaseSkill;
      this.gearIndex = se.gearIndex;
      int? network;
      int num1;
      if (!(se.unit == (BL.Unit) null))
      {
        network = se.unit.ToNetwork(env);
        num1 = network.Value;
      }
      else
        num1 = -1;
      this.unitNetworkId = num1;
      int num2;
      if (!(se.investUnit == (BL.Unit) null))
      {
        network = se.investUnit.ToNetwork(env);
        num2 = network.Value;
      }
      else
        num2 = -1;
      this.investUnitNetworkId = num2;
      this.investSkillId = se.investSkillId;
      this.work = se.work;
      this.isDontDisplay = se.isDontDisplay;
      this.turnCount = se.turnCount;
      this.againInvoked = se.againInvoked;
      this.moveDistance = se.moveDistance;
      this.isAttackMethod = se.isAttackMethod;
      this.investTurn = se.investTurn;
      this.dontCleanUseRemain = se.dontCleanUseRemain;
      int num3;
      if (!(se.parentUnit == (BL.Unit) null))
      {
        network = se.parentUnit.ToNetwork(env);
        num3 = network.Value;
      }
      else
        num3 = -1;
      this.parentUnitNetworkId = num3;
    }
  }
}
