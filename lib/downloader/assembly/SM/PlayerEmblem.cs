// Decompiled with JetBrains decompiler
// Type: SM.PlayerEmblem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerEmblem : KeyCompare
  {
    public DateTime created_at;
    public int emblem_id;
    public bool is_new;

    public PlayerEmblem()
    {
    }

    public PlayerEmblem(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.emblem_id = (int) (long) json[nameof (emblem_id)];
      this.is_new = (bool) json[nameof (is_new)];
    }
  }
}
