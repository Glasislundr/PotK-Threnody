// Decompiled with JetBrains decompiler
// Type: SM.MonthlyPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class MonthlyPack : KeyCompare
  {
    public int purchase_limit;
    public DateTime? start_at;
    public string name;
    public int player_exp_rate;
    public int drop_item_rate;
    public int money_rate;
    public string drop_item_rate_text;
    public string player_exp_rate_text;
    public int coin_group_id;
    public DateTime? end_at;
    public int id;
    public string money_rate_text;

    public MonthlyPack()
    {
    }

    public MonthlyPack(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.purchase_limit = (int) (long) json[nameof (purchase_limit)];
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.name = (string) json[nameof (name)];
      this.player_exp_rate = (int) (long) json[nameof (player_exp_rate)];
      this.drop_item_rate = (int) (long) json[nameof (drop_item_rate)];
      this.money_rate = (int) (long) json[nameof (money_rate)];
      this.drop_item_rate_text = (string) json[nameof (drop_item_rate_text)];
      this.player_exp_rate_text = (string) json[nameof (player_exp_rate_text)];
      this.coin_group_id = (int) (long) json[nameof (coin_group_id)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.id = (int) (long) json[nameof (id)];
      this.money_rate_text = (string) json[nameof (money_rate_text)];
    }
  }
}
