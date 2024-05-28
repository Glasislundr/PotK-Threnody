// Decompiled with JetBrains decompiler
// Type: RaidDamageRewardPopupSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class RaidDamageRewardPopupSequence
{
  private GameObject rewardPopupPrefab;
  private List<RaidDamageReward> rewardList;

  public IEnumerator Init(RaidDamageReward[] rewardList)
  {
    this.rewardList = ((IEnumerable<RaidDamageReward>) rewardList).ToList<RaidDamageReward>();
    yield return (object) this.LoadPopupPrefab();
  }

  public IEnumerator Run()
  {
    foreach (RaidDamageReward reward in this.rewardList)
    {
      yield return (object) this.ShowDamageRewardPopup(reward);
      yield return (object) new WaitForSeconds(0.5f);
    }
  }

  private IEnumerator LoadPopupPrefab()
  {
    Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/raid0032_result/dir_RaidBoss_result_reward");
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) future.Result, (Object) null))
      Debug.LogError((object) "failed to load dir_RaidBoss_result_reward.prefab");
    else
      this.rewardPopupPrefab = future.Result;
  }

  private IEnumerator ShowDamageRewardPopup(RaidDamageReward reward)
  {
    GameObject obj = Singleton<PopupManager>.GetInstance().open(this.rewardPopupPrefab);
    IEnumerator e = obj.GetComponent<Raid032BattleResultDamageRewardPopup>().InitAsync(reward);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1034");
    bool toNext = false;
    RaidUtils.CreateTouchObject((EventDelegate.Callback) (() => toNext = true), obj.transform);
    while (!toNext)
      yield return (object) null;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
