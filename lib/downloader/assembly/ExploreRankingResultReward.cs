// Decompiled with JetBrains decompiler
// Type: ExploreRankingResultReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExploreRankingResultReward : MonoBehaviour
{
  [SerializeField]
  private NGxScrollMasonry scrollContainer;
  private GameObject marginPrefab;
  private GameObject boxPrefab;

  public IEnumerator Initialize(
    Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>> rewardsList)
  {
    IEnumerator e = this.lordResource();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.setScrollRewardBox(rewardsList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator lordResource()
  {
    Future<GameObject> boxPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.boxPrefab, (Object) null))
    {
      boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.boxPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.marginPrefab, (Object) null))
    {
      boxPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.marginPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator setScrollRewardBox(
    Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>> rewardsList)
  {
    if (rewardsList != null && rewardsList.Count<KeyValuePair<int, List<Versus02612ScrollRewardBox.RewardData>>>() > 0)
    {
      ((Component) this.scrollContainer.Scroll).transform.Clear();
      this.scrollContainer.Reset();
      foreach (KeyValuePair<int, List<Versus02612ScrollRewardBox.RewardData>> rewards in rewardsList)
      {
        GameObject box = this.boxPrefab.Clone();
        IEnumerator e = box.GetComponent<Versus02612ScrollRewardBox>().InitExploreRanking(rewards.Value, rewards.Key);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.scrollContainer.Add(box);
        this.scrollContainer.Add(this.marginPrefab.Clone());
        box = (GameObject) null;
      }
    }
  }

  public void scrollResolvePosition() => this.scrollContainer.ResolvePosition();

  public void Close() => Singleton<PopupManager>.GetInstance().dismiss();
}
