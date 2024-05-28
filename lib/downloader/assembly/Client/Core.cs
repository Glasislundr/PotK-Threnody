// Decompiled with JetBrains decompiler
// Type: Client.Core
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;

#nullable disable
namespace Client
{
  [Serializable]
  public class Core : BL
  {
    public BE _be;

    public Core(BE be) => this._be = be;

    public override BL.SkillResultUnit createSkillResultUnit(BL.UnitPosition up)
    {
      return (BL.SkillResultUnit) new Client.SkillResultUnit(up, this._be);
    }

    public override void resetUnitStatus(BL.UnitPosition up, int row, int column, float direction)
    {
      base.resetUnitStatus(up, row, column, direction);
      if (up is BL.AIUnit)
        return;
      this._be.unitResource[up.unit].unitParts_.resetStatus(up.row, up.column, up.direction);
    }
  }
}
