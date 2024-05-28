// Decompiled with JetBrains decompiler
// Type: SM.GachaModuleDecksProbabilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GachaModuleDecksProbabilities : KeyCompare
  {
    public int rarity_id;
    public float probability;

    public GachaModuleDecksProbabilities()
    {
    }

    public GachaModuleDecksProbabilities(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.rarity_id = (int) (long) json[nameof (rarity_id)];
      this.probability = (float) (double) json[nameof (probability)];
    }
  }
}
