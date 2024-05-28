// Decompiled with JetBrains decompiler
// Type: SM.PlayerLoginBonusRewards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerLoginBonusRewards : KeyCompare
  {
    public string client_reward_message;
    public int reward_quantity;
    public string client_next_reward_message;
    public int reward_id;
    public int reward_type_id;

    public PlayerLoginBonusRewards()
    {
    }

    public PlayerLoginBonusRewards(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.client_reward_message = json[nameof (client_reward_message)] == null ? (string) null : (string) json[nameof (client_reward_message)];
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this.client_next_reward_message = json[nameof (client_next_reward_message)] == null ? (string) null : (string) json[nameof (client_next_reward_message)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
      this.reward_type_id = (int) (long) json[nameof (reward_type_id)];
    }
  }
}
