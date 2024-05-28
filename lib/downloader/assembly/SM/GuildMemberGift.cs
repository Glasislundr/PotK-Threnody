// Decompiled with JetBrains decompiler
// Type: SM.GuildMemberGift
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildMemberGift : KeyCompare
  {
    public string send_player_id;
    public int gift_reward_id;
    public int gift_reward_type_id;
    public int gift_reward_quantity;
    public GuildPlayerInfo player;
    public DateTime limit_at;
    public DateTime send_at;
    public string player_id;
    public string id;

    public GuildMemberGift()
    {
    }

    public GuildMemberGift(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.send_player_id = (string) json[nameof (send_player_id)];
      this.gift_reward_id = (int) (long) json[nameof (gift_reward_id)];
      this.gift_reward_type_id = (int) (long) json[nameof (gift_reward_type_id)];
      this.gift_reward_quantity = (int) (long) json[nameof (gift_reward_quantity)];
      this.player = json[nameof (player)] == null ? (GuildPlayerInfo) null : new GuildPlayerInfo((Dictionary<string, object>) json[nameof (player)]);
      this.limit_at = DateTime.Parse((string) json[nameof (limit_at)]);
      this.send_at = DateTime.Parse((string) json[nameof (send_at)]);
      this.player_id = (string) json[nameof (player_id)];
      this.id = (string) json[nameof (id)];
    }
  }
}
