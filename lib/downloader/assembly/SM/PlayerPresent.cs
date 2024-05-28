// Decompiled with JetBrains decompiler
// Type: SM.PlayerPresent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerPresent : KeyCompare
  {
    public int? reward_quantity;
    public int? reward_type_id;
    public string title;
    public DateTime created_at;
    public DateTime? received_at;
    public string message;
    public int id;
    public int? reward_id;
    public bool? isReceivable;

    public PlayerPresent()
    {
    }

    public PlayerPresent(Dictionary<string, object> json)
    {
      this._hasKey = true;
      long? nullable1;
      int? nullable2;
      if (json[nameof (reward_quantity)] != null)
      {
        nullable1 = (long?) json[nameof (reward_quantity)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.reward_quantity = nullable2;
      int? nullable3;
      if (json[nameof (reward_type_id)] != null)
      {
        nullable1 = (long?) json[nameof (reward_type_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.reward_type_id = nullable3;
      this.title = (string) json[nameof (title)];
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.received_at = json[nameof (received_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (received_at)]));
      this.message = (string) json[nameof (message)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      int? nullable4;
      if (json[nameof (reward_id)] != null)
      {
        nullable1 = (long?) json[nameof (reward_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.reward_id = nullable4;
    }
  }
}
