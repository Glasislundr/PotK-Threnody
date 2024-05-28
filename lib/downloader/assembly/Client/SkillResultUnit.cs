// Decompiled with JetBrains decompiler
// Type: Client.SkillResultUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;

#nullable disable
namespace Client
{
  internal class SkillResultUnit : BL.SkillResultUnit
  {
    public BE _be;

    public SkillResultUnit(BL.UnitPosition up, BE be)
      : base(up, be.core)
    {
      this._be = be;
    }

    public override void respawnReinforcement()
    {
      base.respawnReinforcement();
      if (this._up_target is BL.AIUnit)
        return;
      this._be.unitResource[this._up_target.unit].unitParts_.setActive(true);
    }
  }
}
