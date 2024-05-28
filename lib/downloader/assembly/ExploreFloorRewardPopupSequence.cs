// Decompiled with JetBrains decompiler
// Type: ExploreFloorRewardPopupSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExploreFloorRewardPopupSequence
{
  private GameObject rewardPopupPrefab;
  private List<BattleResultBonusInfo> RewardList;
  private bool enableAndroidBackKey;

  public ExploreFloorRewardPopupSequence(bool enableAndroidBackKey = false)
  {
    this.enableAndroidBackKey = enableAndroidBackKey;
  }

  public IEnumerator Init(ExploreFloor floorData)
  {
    this.RewardList = new List<BattleResultBonusInfo>();
    foreach (ExploreFloorReward exploreFloorReward in ((IEnumerable<ExploreFloorReward>) MasterData.ExploreFloorRewardList).Where<ExploreFloorReward>((Func<ExploreFloorReward, bool>) (x => x.floor == floorData.floor && x.period_id == floorData.period_id)))
      this.RewardList.Add(new BattleResultBonusInfo(exploreFloorReward.reward_id, (MasterDataTable.CommonRewardType) exploreFloorReward.reward_type_CommonRewardType, exploreFloorReward.reward_title));
    if (this.RewardList.Count > 0)
      yield return (object) this.LoadPopupPrefab();
  }

  public IEnumerator Run()
  {
    if (this.RewardList.Count > 0)
    {
      GameObject popup = this.rewardPopupPrefab.Clone();
      if (Object.op_Inequality((Object) popup.GetComponent<UIWidget>(), (Object) null))
        ((UIRect) popup.GetComponent<UIWidget>()).alpha = 0.0f;
      popup.SetActive(false);
      BattleUI05ClearBonusSetting script = popup.GetComponent<BattleUI05ClearBonusSetting>();
      IEnumerator e = script.CreateClearBonusIcon(this.RewardList, false, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      script.SetClearBonusInfo(this.RewardList, false);
      GameObject gameObject = Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1034");
      bool toNext = false;
      RaidUtils.CreateTouchObject((EventDelegate.Callback) (() => toNext = true), gameObject.transform);
      while (!toNext)
        yield return (object) null;
      Singleton<PopupManager>.GetInstance().onDismiss();
      yield return (object) new WaitForSeconds(0.5f);
    }
  }

  private IEnumerator LoadPopupPrefab()
  {
    Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/explore_FloorArrival_reward");
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) future.Result, (Object) null))
      Debug.LogError((object) "failed to load dir_RaidBoss_result_Defeat_reward.prefab");
    else
      this.rewardPopupPrefab = future.Result;
  }

  private class BackButtonProgressBehaviour : BackButtonMenuBase
  {
    private Action onBack;

    public void SetAction(Action onBack) => this.onBack = onBack;

    public override void onBackButton()
    {
      Action onBack = this.onBack;
      if (onBack == null)
        return;
      onBack();
    }
  }
}
