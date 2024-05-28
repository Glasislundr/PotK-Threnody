// Decompiled with JetBrains decompiler
// Type: SM.PlayerSeasonTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerSeasonTicket : KeyCompare
  {
    public string player_id;
    public int max_quantity;
    public int season_ticket_id;
    public int quantity;

    public PlayerSeasonTicket()
    {
    }

    public PlayerSeasonTicket(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_id = (string) json[nameof (player_id)];
      this.max_quantity = (int) (long) json[nameof (max_quantity)];
      this.season_ticket_id = (int) (long) json[nameof (season_ticket_id)];
      this.quantity = (int) (long) json[nameof (quantity)];
    }
  }
}
