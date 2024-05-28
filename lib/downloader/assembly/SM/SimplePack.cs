// Decompiled with JetBrains decompiler
// Type: SM.SimplePack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class SimplePack : KeyCompare
  {
    public string description;
    public DateTime? end_at;
    public int badge_category;
    public int coin_group_id;
    public int purchase_limit;
    public int id;
    public string icon_resource_name;
    public string name;

    public SimplePack()
    {
    }

    public SimplePack(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.description = (string) json[nameof (description)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.badge_category = (int) (long) json[nameof (badge_category)];
      this.coin_group_id = (int) (long) json[nameof (coin_group_id)];
      this.purchase_limit = (int) (long) json[nameof (purchase_limit)];
      this.id = (int) (long) json[nameof (id)];
      this.icon_resource_name = (string) json[nameof (icon_resource_name)];
      this.name = (string) json[nameof (name)];
    }
  }
}
