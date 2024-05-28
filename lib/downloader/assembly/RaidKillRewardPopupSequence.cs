// Decompiled with JetBrains decompiler
// Type: RaidKillRewardPopupSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class RaidKillRewardPopupSequence
{
  private GameObject killRewardPopupPrefab;
  private List<BattleResultBonusInfo> RewardList;
  private int raid_id;
  private const int rewardMax = 5;
  private bool enableAndroidBackKey;

  public RaidKillRewardPopupSequence(bool enableAndroidBackKey = false)
  {
    this.enableAndroidBackKey = enableAndroidBackKey;
  }

  public IEnumerator Init(int raidID, RaidDefeatReward[] rewardList)
  {
    this.raid_id = raidID;
    this.RewardList = this.ToBattleResultBonusInfoList(rewardList);
    yield return (object) this.LoadPopupPrefab();
  }

  private List<BattleResultBonusInfo> ToBattleResultBonusInfoList(RaidDefeatReward[] rewardList)
  {
    return ((IEnumerable<RaidDefeatReward>) rewardList).Select<RaidDefeatReward, BattleResultBonusInfo>((Func<RaidDefeatReward, BattleResultBonusInfo>) (x => new BattleResultBonusInfo(x.reward_id, (MasterDataTable.CommonRewardType) x.reward_type_id, CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToList<BattleResultBonusInfo>();
  }

  public IEnumerator Run()
  {
    GameObject popup = this.killRewardPopupPrefab.Clone();
    if (Object.op_Inequality((Object) popup.GetComponent<UIWidget>(), (Object) null))
      ((UIRect) popup.GetComponent<UIWidget>()).alpha = 0.0f;
    popup.SetActive(false);
    BattleUI05ClearBonusSetting script = popup.GetComponent<BattleUI05ClearBonusSetting>();
    IEnumerator e = script.CreateClearBonusIcon(this.RewardList, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    script.SetClearBonusInfo(this.RewardList);
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1034");
    bool toNext = false;
    RaidUtils.CreateTouchObject((EventDelegate.Callback) (() => toNext = true), gameObject.transform);
    while (!toNext)
      yield return (object) null;
    Singleton<PopupManager>.GetInstance().onDismiss();
    yield return (object) new WaitForSeconds(0.5f);
  }

  private IEnumerator LoadPopupPrefab()
  {
    Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/raid0032_result/dir_RaidBoss_result_Defeat_reward");
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) future.Result, (Object) null))
      Debug.LogError((object) "failed to load dir_RaidBoss_result_Defeat_reward.prefab");
    else
      this.killRewardPopupPrefab = future.Result;
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
