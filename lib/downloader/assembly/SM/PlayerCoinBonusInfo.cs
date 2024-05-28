// Decompiled with JetBrains decompiler
// Type: SM.PlayerCoinBonusInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCoinBonusInfo : KeyCompare
  {
    public int coin_group_id;
    public int purchased_count;
    public int badge_category;
    public int purchase_limit;
    public int id;

    public PlayerCoinBonusInfo()
    {
    }

    public PlayerCoinBonusInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.coin_group_id = (int) (long) json[nameof (coin_group_id)];
      this.purchased_count = (int) (long) json[nameof (purchased_count)];
      this.badge_category = (int) (long) json[nameof (badge_category)];
      this.purchase_limit = (int) (long) json[nameof (purchase_limit)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
