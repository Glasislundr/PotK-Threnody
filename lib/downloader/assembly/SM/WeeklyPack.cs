// Decompiled with JetBrains decompiler
// Type: SM.WeeklyPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class WeeklyPack : KeyCompare
  {
    public int coin_group_id;
    public int purchase_limit;
    public int id;
    public string name;
    public DateTime? end_at;

    public WeeklyPack()
    {
    }

    public WeeklyPack(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.coin_group_id = (int) (long) json[nameof (coin_group_id)];
      this.purchase_limit = (int) (long) json[nameof (purchase_limit)];
      this.id = (int) (long) json[nameof (id)];
      this.name = (string) json[nameof (name)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
    }
  }
}
