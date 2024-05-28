// Decompiled with JetBrains decompiler
// Type: GameCore.ColosseumBeforBonusParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GameCore
{
  public class ColosseumBeforBonusParam
  {
    public int HP;
    public int attack;
    public int dexerityDisplay;
    public int criticalDisplay;
    public int attackCount;

    public ColosseumBeforBonusParam(AttackStatus status)
    {
      this.HP = status.duelParameter.attackerUnitParameter.Hp;
      this.attack = status.attack;
      this.dexerityDisplay = status.dexerityDisplay();
      this.criticalDisplay = status.criticalDisplay();
      this.attackCount = status.attackCount;
    }
  }
}
