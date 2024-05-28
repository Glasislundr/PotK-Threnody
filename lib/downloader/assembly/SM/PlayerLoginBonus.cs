// Decompiled with JetBrains decompiler
// Type: SM.PlayerLoginBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerLoginBonus : KeyCompare
  {
    public PlayerLoginBonusRewards[] rewards;
    public int max_fill_count;
    public int _loginbonus;
    public int[] received_reward_days;
    public int remain_fill_count;
    public int login_days;

    public LoginbonusLoginbonus loginbonus
    {
      get
      {
        if (MasterData.LoginbonusLoginbonus.ContainsKey(this._loginbonus))
          return MasterData.LoginbonusLoginbonus[this._loginbonus];
        Debug.LogError((object) ("Key not Found: MasterData.LoginbonusLoginbonus[" + (object) this._loginbonus + "]"));
        return (LoginbonusLoginbonus) null;
      }
    }

    public PlayerLoginBonus()
    {
    }

    public PlayerLoginBonus(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<PlayerLoginBonusRewards> loginBonusRewardsList = new List<PlayerLoginBonusRewards>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        loginBonusRewardsList.Add(json1 == null ? (PlayerLoginBonusRewards) null : new PlayerLoginBonusRewards((Dictionary<string, object>) json1));
      this.rewards = loginBonusRewardsList.ToArray();
      this.max_fill_count = (int) (long) json[nameof (max_fill_count)];
      this._loginbonus = (int) (long) json[nameof (loginbonus)];
      this.received_reward_days = ((IEnumerable<object>) json[nameof (received_reward_days)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.remain_fill_count = (int) (long) json[nameof (remain_fill_count)];
      this.login_days = (int) (long) json[nameof (login_days)];
    }
  }
}
