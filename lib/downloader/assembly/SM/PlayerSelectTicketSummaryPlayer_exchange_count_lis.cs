// Decompiled with JetBrains decompiler
// Type: SM.PlayerSelectTicketSummaryPlayer_exchange_count_list
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerSelectTicketSummaryPlayer_exchange_count_list : KeyCompare
  {
    public int exchange_count;
    public int reward_id;

    public PlayerSelectTicketSummaryPlayer_exchange_count_list()
    {
    }

    public PlayerSelectTicketSummaryPlayer_exchange_count_list(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.exchange_count = (int) (long) json[nameof (exchange_count)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
    }
  }
}
