// Decompiled with JetBrains decompiler
// Type: SM.PlayerGuildTownSlotPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerGuildTownSlotPosition : KeyCompare
  {
    public int y;
    public int x;
    public int master_id;

    public PlayerGuildTownSlotPosition()
    {
    }

    public PlayerGuildTownSlotPosition(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.y = (int) (long) json[nameof (y)];
      this.x = (int) (long) json[nameof (x)];
      this.master_id = (int) (long) json[nameof (master_id)];
    }
  }
}
