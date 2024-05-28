// Decompiled with JetBrains decompiler
// Type: Shop00720RewardScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00720RewardScroll : MonoBehaviour
{
  [SerializeField]
  private GameObject Pattern;
  [SerializeField]
  private int RewardListMargin;
  [SerializeField]
  private int RewardListMarginLastBottom;
  [SerializeField]
  private int RewardListHeight;

  public IEnumerator Init(Shop00720RewardPatternData data, Shop00720Prefabs prefabs)
  {
    this.SetReelPattern(data.ReelPattern, data.Description, prefabs);
    IEnumerator e = this.SetReward(data.RewardList, prefabs);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AdjastHeight(data.RewardList.Count<Shop00720RewardData>());
  }

  private void SetReelPattern(List<Sprite> pattern, string txt, Shop00720Prefabs prefabs)
  {
    prefabs.DirSlotPattern.Clone(this.Pattern.transform).GetComponent<Shop00720ReelPattern>().Init(pattern, txt);
  }

  private IEnumerator SetReward(List<Shop00720RewardData> data, Shop00720Prefabs prefabs)
  {
    Shop00720RewardScroll shop00720RewardScroll = this;
    GameObject prefab = prefabs.DirList;
    foreach (var n in data.Select((s, i) => new
    {
      s = s,
      i = i
    }))
    {
      GameObject reward = prefab.Clone(((Component) shop00720RewardScroll).gameObject.transform);
      IEnumerator e = reward.GetComponent<Shop00720Reward>().Init(n.s.RewardType, n.s.RewardId, n.s.Quantity, n.s.Description);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      shop00720RewardScroll.SetRewardPosition(reward, n.i);
      reward = (GameObject) null;
    }
  }

  private void SetRewardPosition(GameObject reward, int index)
  {
    int num = (this.RewardListHeight + this.RewardListMargin) * -index;
    reward.transform.localPosition = new Vector3(0.0f, (float) num);
  }

  private void AdjastHeight(int times)
  {
    UIWidget component1 = ((Component) this).gameObject.GetComponent<UIWidget>();
    BoxCollider component2 = ((Component) this).gameObject.GetComponent<BoxCollider>();
    foreach (int num in Enumerable.Range(1, times))
    {
      component1.height += num.Equals(1) ? this.RewardListHeight : this.RewardListHeight + this.RewardListMargin;
      if (num.Equals(times))
        component1.height += this.RewardListMarginLastBottom;
    }
    component2.center = new Vector3(component2.center.x, (float) component1.height / -2f, component2.center.z);
    component2.size = new Vector3(component2.size.x, (float) component1.height, component2.size.z);
  }
}
