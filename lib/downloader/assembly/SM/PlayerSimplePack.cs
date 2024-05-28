// Decompiled with JetBrains decompiler
// Type: SM.PlayerSimplePack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerSimplePack : KeyCompare
  {
    public int? rest_receive_day;
    public int purchased_count;
    public int reward_days;
    public bool is_purchasable;
    public DateTime? purchased_at;
    public bool is_received;
    public int max_days;
    public int? id;

    public PlayerSimplePack()
    {
    }

    public PlayerSimplePack(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (rest_receive_day)] != null)
      {
        nullable1 = (long?) json[nameof (rest_receive_day)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.rest_receive_day = nullable2;
      this.purchased_count = (int) (long) json[nameof (purchased_count)];
      this.reward_days = (int) (long) json[nameof (reward_days)];
      this.is_purchasable = (bool) json[nameof (is_purchasable)];
      this.purchased_at = json[nameof (purchased_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (purchased_at)]));
      this.is_received = (bool) json[nameof (is_received)];
      this.max_days = (int) (long) json[nameof (max_days)];
      int? nullable3;
      if (json[nameof (id)] != null)
      {
        nullable1 = (long?) json[nameof (id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.id = nullable3;
    }
  }
}
