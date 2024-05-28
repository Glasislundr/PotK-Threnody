// Decompiled with JetBrains decompiler
// Type: SM.WeeklyPackInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class WeeklyPackInfo : KeyCompare
  {
    public WeeklyPackReward[] rewards;
    public WeeklyPackDescription[] descriptions;
    public PlayerPackStatus player_pack;
    public WeeklyPack pack;

    public WeeklyPackInfo()
    {
    }

    public WeeklyPackInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<WeeklyPackReward> weeklyPackRewardList = new List<WeeklyPackReward>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        weeklyPackRewardList.Add(json1 == null ? (WeeklyPackReward) null : new WeeklyPackReward((Dictionary<string, object>) json1));
      this.rewards = weeklyPackRewardList.ToArray();
      List<WeeklyPackDescription> weeklyPackDescriptionList = new List<WeeklyPackDescription>();
      foreach (object json2 in (List<object>) json[nameof (descriptions)])
        weeklyPackDescriptionList.Add(json2 == null ? (WeeklyPackDescription) null : new WeeklyPackDescription((Dictionary<string, object>) json2));
      this.descriptions = weeklyPackDescriptionList.ToArray();
      this.player_pack = json[nameof (player_pack)] == null ? (PlayerPackStatus) null : new PlayerPackStatus((Dictionary<string, object>) json[nameof (player_pack)]);
      this.pack = json[nameof (pack)] == null ? (WeeklyPack) null : new WeeklyPack((Dictionary<string, object>) json[nameof (pack)]);
    }
  }
}
