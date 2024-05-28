// Decompiled with JetBrains decompiler
// Type: SM.PlayerTalkMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerTalkMessage : KeyCompare
  {
    public int player_message_id;
    public DateTime created_at;
    public int read_status;
    public int message_id;
    public int? condition_type;
    public int same_character_id;
    public int message_type;
    public int? condition_id;
    public DateTime? expire_at;

    public PlayerTalkMessage()
    {
    }

    public PlayerTalkMessage(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_message_id = (int) (long) json[nameof (player_message_id)];
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.read_status = (int) (long) json[nameof (read_status)];
      this.message_id = (int) (long) json[nameof (message_id)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (condition_type)] != null)
      {
        nullable1 = (long?) json[nameof (condition_type)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.condition_type = nullable2;
      this.same_character_id = (int) (long) json[nameof (same_character_id)];
      this.message_type = (int) (long) json[nameof (message_type)];
      int? nullable3;
      if (json[nameof (condition_id)] != null)
      {
        nullable1 = (long?) json[nameof (condition_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.condition_id = nullable3;
      this.expire_at = json[nameof (expire_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (expire_at)]));
    }
  }
}
