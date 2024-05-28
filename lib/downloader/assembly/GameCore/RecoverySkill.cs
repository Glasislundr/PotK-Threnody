// Decompiled with JetBrains decompiler
// Type: GameCore.RecoverySkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GameCore
{
  public class RecoverySkill
  {
    public int id;
    public int level;
    public int? remain;
    public int useTurn;
    public int nowUseCount;

    public RecoverySkill(BL.Skill skill)
    {
      this.id = skill.id;
      this.level = skill.level;
      this.remain = skill.remain;
      this.useTurn = skill.useTurn;
      this.nowUseCount = skill.nowUseCount;
    }
  }
}
