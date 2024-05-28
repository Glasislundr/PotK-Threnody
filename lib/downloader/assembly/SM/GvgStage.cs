// Decompiled with JetBrains decompiler
// Type: SM.GvgStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GvgStage : KeyCompare
  {
    public int turns;
    public int stage_id;
    public int annihilation_point;
    public int point;

    public GvgStage()
    {
    }

    public GvgStage(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.turns = (int) (long) json[nameof (turns)];
      this.stage_id = (int) (long) json[nameof (stage_id)];
      this.annihilation_point = (int) (long) json[nameof (annihilation_point)];
      this.point = (int) (long) json[nameof (point)];
    }
  }
}
