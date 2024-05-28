// Decompiled with JetBrains decompiler
// Type: SM.PlayerEnemyHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerEnemyHistory : KeyCompare
  {
    public DateTime created_at;
    public int unit_id;
    public int defeat;

    public PlayerEnemyHistory()
    {
    }

    public PlayerEnemyHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.unit_id = (int) (long) json[nameof (unit_id)];
      this.defeat = (int) (long) json[nameof (defeat)];
    }
  }
}
