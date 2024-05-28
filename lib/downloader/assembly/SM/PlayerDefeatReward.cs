// Decompiled with JetBrains decompiler
// Type: SM.PlayerDefeatReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerDefeatReward : KeyCompare
  {
    public int defeat_reward_id;

    public PlayerDefeatReward()
    {
    }

    public PlayerDefeatReward(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.defeat_reward_id = (int) (long) json[nameof (defeat_reward_id)];
    }
  }
}
