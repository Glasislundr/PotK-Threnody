// Decompiled with JetBrains decompiler
// Type: Popup02353Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02353Menu : Popup0235MenuBase
{
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription;

  public override IEnumerator Init(
    ResultMenuBase.CampaignReward reward,
    ResultMenuBase.CampaignNextReward nextReward,
    GameObject gearObject,
    GameObject unitObject,
    GameObject uniqueObject)
  {
    this.TxtTitle.SetText(nextReward.next_reward_title);
    this.TxtDescription.SetText(nextReward.next_reward_text);
    yield break;
  }
}
