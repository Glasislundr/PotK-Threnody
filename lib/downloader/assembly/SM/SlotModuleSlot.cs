// Decompiled with JetBrains decompiler
// Type: SM.SlotModuleSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class SlotModuleSlot : KeyCompare
  {
    public int count;
    public int? payment_id;
    public int deck_id;
    public DateTime? start_at;
    public string description;
    public int roll_count;
    public DateTime? end_at;
    public int daily_count;
    public int payment_amount;
    public int? limit;
    public int? daily_limit;
    public int payment_type_id;
    public int id;
    public string name;

    public SlotModuleSlot()
    {
    }

    public SlotModuleSlot(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (payment_id)] != null)
      {
        nullable1 = (long?) json[nameof (payment_id)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.payment_id = nullable2;
      this.deck_id = (int) (long) json[nameof (deck_id)];
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.description = (string) json[nameof (description)];
      this.roll_count = (int) (long) json[nameof (roll_count)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.daily_count = (int) (long) json[nameof (daily_count)];
      this.payment_amount = (int) (long) json[nameof (payment_amount)];
      int? nullable3;
      if (json[nameof (limit)] != null)
      {
        nullable1 = (long?) json[nameof (limit)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.limit = nullable3;
      int? nullable4;
      if (json[nameof (daily_limit)] != null)
      {
        nullable1 = (long?) json[nameof (daily_limit)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.daily_limit = nullable4;
      this.payment_type_id = (int) (long) json[nameof (payment_type_id)];
      this.id = (int) (long) json[nameof (id)];
      this.name = (string) json[nameof (name)];
    }
  }
}
