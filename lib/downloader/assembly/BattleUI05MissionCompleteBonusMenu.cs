// Decompiled with JetBrains decompiler
// Type: BattleUI05MissionCompleteBonusMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05MissionCompleteBonusMenu : ResultMenuBase
{
  public GameObject IconObject;
  public UILabel CompleteText;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    BattleEndMission_complete_rewards[] missionCompleteRewards = result.mission_complete_rewards;
    int rewardQuantity = missionCompleteRewards[0].reward_quantity;
    int rewardTypeID = missionCompleteRewards[0].reward_type_id;
    int rewardId = missionCompleteRewards[0].reward_id;
    this.CompleteText.SetText(missionCompleteRewards[0].message);
    CreateIconObject target = this.IconObject.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = target.CreateThumbnail((MasterDataTable.CommonRewardType) rewardTypeID, rewardId, rewardQuantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (rewardTypeID == 1 || rewardTypeID == 24)
    {
      UnitIcon component = target.GetIcon().GetComponent<UnitIcon>();
      component.setLevelText(1.ToLocalizeNumberText());
      component.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
  }
}
