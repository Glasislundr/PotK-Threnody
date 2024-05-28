// Decompiled with JetBrains decompiler
// Type: SM.AnimationPatternMultiAfter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class AnimationPatternMultiAfter : KeyCompare
  {
    public string effect_gem_color_3rd;
    public string effect_gem_color_4th;
    public string effect_gem_color_1st;
    public string effect_type;
    public string effect_gem_color_2nd;
    public bool effect_brilliant;

    public AnimationPatternMultiAfter()
    {
    }

    public AnimationPatternMultiAfter(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.effect_gem_color_3rd = (string) json[nameof (effect_gem_color_3rd)];
      this.effect_gem_color_4th = (string) json[nameof (effect_gem_color_4th)];
      this.effect_gem_color_1st = (string) json[nameof (effect_gem_color_1st)];
      this.effect_type = (string) json[nameof (effect_type)];
      this.effect_gem_color_2nd = (string) json[nameof (effect_gem_color_2nd)];
      this.effect_brilliant = (bool) json[nameof (effect_brilliant)];
    }
  }
}
