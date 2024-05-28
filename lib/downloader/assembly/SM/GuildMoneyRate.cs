// Decompiled with JetBrains decompiler
// Type: SM.GuildMoneyRate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildMoneyRate : KeyCompare
  {
    public int gain_contribution;
    public int ask_token;
    public int _token_type;
    public bool campaign_flag;
    public int rate_id;
    public int guild_money;

    public GuildMoneyToken token_type
    {
      get
      {
        if (!Enum.IsDefined(typeof (GuildMoneyToken), (object) this._token_type))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildMoneyToken[" + (object) this._token_type + "]"));
        return (GuildMoneyToken) this._token_type;
      }
    }

    public GuildMoneyRate()
    {
    }

    public GuildMoneyRate(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.gain_contribution = (int) (long) json[nameof (gain_contribution)];
      this.ask_token = (int) (long) json[nameof (ask_token)];
      this._token_type = (int) (long) json[nameof (token_type)];
      this.campaign_flag = (bool) json[nameof (campaign_flag)];
      this.rate_id = (int) (long) json[nameof (rate_id)];
      this.guild_money = (int) (long) json[nameof (guild_money)];
    }
  }
}
