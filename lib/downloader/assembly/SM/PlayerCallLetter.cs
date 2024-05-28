// Decompiled with JetBrains decompiler
// Type: SM.PlayerCallLetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCallLetter : KeyCompare
  {
    public int mood_status;
    public DateTime? called_at;
    public int player_letter_id;
    public int call_status;
    public int same_character_id;

    public PlayerCallLetter()
    {
    }

    public PlayerCallLetter(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.mood_status = (int) (long) json[nameof (mood_status)];
      this.called_at = json[nameof (called_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (called_at)]));
      this.player_letter_id = (int) (long) json[nameof (player_letter_id)];
      this.call_status = (int) (long) json[nameof (call_status)];
      this.same_character_id = (int) (long) json[nameof (same_character_id)];
    }
  }
}
