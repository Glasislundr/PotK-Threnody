// Decompiled with JetBrains decompiler
// Type: DailyMission0271ConfirmationCompRewardPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0271ConfirmationCompRewardPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private CreateIconObject RewardThumb;
  [SerializeField]
  private UILabel rewardName;

  public IEnumerator Init(BingoRewardGroup completeReward)
  {
    this.rewardName.SetTextLocalize(CommonRewardType.GetRewardName(completeReward.reward_type_id, completeReward.reward_id, completeReward.reward_quantity));
    IEnumerator e = this.RewardThumb.CreateThumbnail(completeReward.reward_type_id, completeReward.reward_id, completeReward.reward_quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnOK() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnOK();
}
