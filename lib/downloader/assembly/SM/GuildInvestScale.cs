// Decompiled with JetBrains decompiler
// Type: SM.GuildInvestScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildInvestScale : KeyCompare
  {
    public int release_level;
    public int scale;

    public GuildInvestScale()
    {
    }

    public GuildInvestScale(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.release_level = (int) (long) json[nameof (release_level)];
      this.scale = (int) (long) json[nameof (scale)];
    }
  }
}
