// Decompiled with JetBrains decompiler
// Type: SM.StepupPackInfoPack_steps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class StepupPackInfoPack_steps : KeyCompare
  {
    public StepupPackReward[] rewards;
    public StepupPackDescription[] descriptions;
    public StepupPackSet pack_set;

    public StepupPackInfoPack_steps()
    {
    }

    public StepupPackInfoPack_steps(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<StepupPackReward> stepupPackRewardList = new List<StepupPackReward>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        stepupPackRewardList.Add(json1 == null ? (StepupPackReward) null : new StepupPackReward((Dictionary<string, object>) json1));
      this.rewards = stepupPackRewardList.ToArray();
      List<StepupPackDescription> stepupPackDescriptionList = new List<StepupPackDescription>();
      foreach (object json2 in (List<object>) json[nameof (descriptions)])
        stepupPackDescriptionList.Add(json2 == null ? (StepupPackDescription) null : new StepupPackDescription((Dictionary<string, object>) json2));
      this.descriptions = stepupPackDescriptionList.ToArray();
      this.pack_set = json[nameof (pack_set)] == null ? (StepupPackSet) null : new StepupPackSet((Dictionary<string, object>) json[nameof (pack_set)]);
    }
  }
}
