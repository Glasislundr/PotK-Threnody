// Decompiled with JetBrains decompiler
// Type: Popup026821Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Popup026821Menu : Popup02682MenuBase
{
  [SerializeField]
  private UI2DSprite icon;
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription1;
  [SerializeField]
  private UILabel TxtDescription2;

  public override IEnumerator Init(
    Versus0268Menu.PvpParam.CampaignReward reward,
    Versus0268Menu.PvpParam.CampaignNextReward nextReward)
  {
    this.TxtTitle.SetText(reward.show_title);
    this.TxtDescription1.SetText(reward.show_text);
    this.TxtDescription2.SetText(reward.show_text2);
    IEnumerator e = ((Component) this.icon).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
