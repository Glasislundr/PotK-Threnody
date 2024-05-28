// Decompiled with JetBrains decompiler
// Type: SM.PlayerPaymentBonusReceiveHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerPaymentBonusReceiveHistory : KeyCompare
  {
    public int bonus_item_id;
    public int require_paid_coins;
    public bool is_archived;
    public bool is_received;

    public PlayerPaymentBonusReceiveHistory()
    {
    }

    public PlayerPaymentBonusReceiveHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.bonus_item_id = (int) (long) json[nameof (bonus_item_id)];
      this.require_paid_coins = (int) (long) json[nameof (require_paid_coins)];
      this.is_archived = (bool) json[nameof (is_archived)];
      this.is_received = (bool) json[nameof (is_received)];
    }
  }
}
