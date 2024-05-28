// Decompiled with JetBrains decompiler
// Type: LevelupSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class LevelupSkill
{
  public int beforeLevel { get; set; }

  public int afterLevel { get; set; }

  public BattleskillSkill skill { get; set; }

  public LevelupSkill(int beforeLevel, int afterLevel, BattleskillSkill skill)
  {
    this.beforeLevel = beforeLevel;
    this.afterLevel = afterLevel;
    this.skill = skill;
  }
}
