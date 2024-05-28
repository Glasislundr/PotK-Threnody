// Decompiled with JetBrains decompiler
// Type: SM.PlayerGiftHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerGiftHistory : KeyCompare
  {
    public int id;
    public int passed_days;
    public GiftBase gift;
    public int coin_id;
    public int gift_id;

    public PlayerGiftHistory()
    {
    }

    public PlayerGiftHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.id = (int) (long) json[nameof (id)];
      this.passed_days = (int) (long) json[nameof (passed_days)];
      this.gift = json[nameof (gift)] == null ? (GiftBase) null : new GiftBase((Dictionary<string, object>) json[nameof (gift)]);
      this.coin_id = (int) (long) json[nameof (coin_id)];
      this.gift_id = (int) (long) json[nameof (gift_id)];
    }
  }
}
