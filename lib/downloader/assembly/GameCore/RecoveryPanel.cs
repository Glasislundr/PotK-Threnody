// Decompiled with JetBrains decompiler
// Type: GameCore.RecoveryPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class RecoveryPanel
  {
    public int row;
    public int column;
    public RecoverySkillEffect[] skillEffectList;

    public RecoveryPanel(BL.Panel panel, BL env)
    {
      this.row = panel.row;
      this.column = panel.column;
      this.skillEffectList = panel.getSkillEffects().value.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isCharismaEffect(x.effect.EffectLogic.Enum))).Select<BL.SkillEffect, RecoverySkillEffect>((Func<BL.SkillEffect, RecoverySkillEffect>) (se => new RecoverySkillEffect(se, env))).ToArray<RecoverySkillEffect>();
    }
  }
}
