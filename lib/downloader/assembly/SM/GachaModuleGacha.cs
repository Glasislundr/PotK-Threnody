// Decompiled with JetBrains decompiler
// Type: SM.GachaModuleGacha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GachaModuleGacha : KeyCompare
  {
    public bool is_one_hundred_ream;
    public int? common_ticket_quantity;
    public int id;
    public int? max_roll_count;
    public bool is_selected_pickup;
    public int deck_id;
    public DateTime? end_at;
    public int daily_count;
    public int payment_amount;
    public int payment_type_id;
    public GachaDescription details;
    public bool is_gacha_pickup;
    public int? payment_id;
    public int? pickup_select_count;
    public DateTime? start_at;
    public string description;
    public int roll_count;
    public string button_text;
    public int? common_ticket_id;
    public int? daily_limit;
    public int? remain_count_for_reward;
    public bool is_change_pickup_select;
    public int count;
    public string name;
    public bool is_pickup_select;
    public int? limit;
    public string button_url;

    public GachaModuleGacha()
    {
    }

    public GachaModuleGacha(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_one_hundred_ream = (bool) json[nameof (is_one_hundred_ream)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (common_ticket_quantity)] != null)
      {
        nullable1 = (long?) json[nameof (common_ticket_quantity)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.common_ticket_quantity = nullable2;
      this.id = (int) (long) json[nameof (id)];
      int? nullable3;
      if (json[nameof (max_roll_count)] != null)
      {
        nullable1 = (long?) json[nameof (max_roll_count)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.max_roll_count = nullable3;
      this.is_selected_pickup = (bool) json[nameof (is_selected_pickup)];
      this.deck_id = (int) (long) json[nameof (deck_id)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.daily_count = (int) (long) json[nameof (daily_count)];
      this.payment_amount = (int) (long) json[nameof (payment_amount)];
      this.payment_type_id = (int) (long) json[nameof (payment_type_id)];
      this.details = json[nameof (details)] == null ? (GachaDescription) null : new GachaDescription((Dictionary<string, object>) json[nameof (details)]);
      this.is_gacha_pickup = (bool) json[nameof (is_gacha_pickup)];
      int? nullable4;
      if (json[nameof (payment_id)] != null)
      {
        nullable1 = (long?) json[nameof (payment_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.payment_id = nullable4;
      int? nullable5;
      if (json[nameof (pickup_select_count)] != null)
      {
        nullable1 = (long?) json[nameof (pickup_select_count)];
        nullable5 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable5 = new int?();
      this.pickup_select_count = nullable5;
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.description = (string) json[nameof (description)];
      this.roll_count = (int) (long) json[nameof (roll_count)];
      this.button_text = json[nameof (button_text)] == null ? (string) null : (string) json[nameof (button_text)];
      int? nullable6;
      if (json[nameof (common_ticket_id)] != null)
      {
        nullable1 = (long?) json[nameof (common_ticket_id)];
        nullable6 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable6 = new int?();
      this.common_ticket_id = nullable6;
      int? nullable7;
      if (json[nameof (daily_limit)] != null)
      {
        nullable1 = (long?) json[nameof (daily_limit)];
        nullable7 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable7 = new int?();
      this.daily_limit = nullable7;
      int? nullable8;
      if (json[nameof (remain_count_for_reward)] != null)
      {
        nullable1 = (long?) json[nameof (remain_count_for_reward)];
        nullable8 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable8 = new int?();
      this.remain_count_for_reward = nullable8;
      this.is_change_pickup_select = (bool) json[nameof (is_change_pickup_select)];
      this.count = (int) (long) json[nameof (count)];
      this.name = (string) json[nameof (name)];
      this.is_pickup_select = (bool) json[nameof (is_pickup_select)];
      int? nullable9;
      if (json[nameof (limit)] != null)
      {
        nullable1 = (long?) json[nameof (limit)];
        nullable9 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable9 = new int?();
      this.limit = nullable9;
      this.button_url = json[nameof (button_url)] == null ? (string) null : (string) json[nameof (button_url)];
    }
  }
}
