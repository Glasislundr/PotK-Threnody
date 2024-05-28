// Decompiled with JetBrains decompiler
// Type: SM.SimplePackInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class SimplePackInfo : KeyCompare
  {
    public SimplePackReward[] rewards;
    public SimplePackDescription[] descriptions;
    public PlayerPackStatus player_pack;
    public SimplePack pack;

    public SimplePackInfo()
    {
    }

    public SimplePackInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<SimplePackReward> simplePackRewardList = new List<SimplePackReward>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        simplePackRewardList.Add(json1 == null ? (SimplePackReward) null : new SimplePackReward((Dictionary<string, object>) json1));
      this.rewards = simplePackRewardList.ToArray();
      List<SimplePackDescription> simplePackDescriptionList = new List<SimplePackDescription>();
      foreach (object json2 in (List<object>) json[nameof (descriptions)])
        simplePackDescriptionList.Add(json2 == null ? (SimplePackDescription) null : new SimplePackDescription((Dictionary<string, object>) json2));
      this.descriptions = simplePackDescriptionList.ToArray();
      this.player_pack = json[nameof (player_pack)] == null ? (PlayerPackStatus) null : new PlayerPackStatus((Dictionary<string, object>) json[nameof (player_pack)]);
      this.pack = json[nameof (pack)] == null ? (SimplePack) null : new SimplePack((Dictionary<string, object>) json[nameof (pack)]);
    }
  }
}
