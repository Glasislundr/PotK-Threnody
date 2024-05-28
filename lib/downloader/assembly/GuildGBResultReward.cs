// Decompiled with JetBrains decompiler
// Type: GuildGBResultReward
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
public class GuildGBResultReward : MonoBehaviour
{
  [SerializeField]
  private NGxScrollMasonry scrollContainer;
  private GameObject marginPrefab;
  private GameObject boxPrefab;

  public void PositionReset() => this.scrollContainer.ResolvePosition();

  public IEnumerator Initialize(WebAPI.Response.GvgResult result)
  {
    GvgWholeRewardMaster[] rewardList = result.whole_rewards;
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
    List<string> list = ((IEnumerable<GvgWholeRewardMaster>) rewardList).Select<GvgWholeRewardMaster, string>((Func<GvgWholeRewardMaster, string>) (x => x.reward_title)).Distinct<string>().ToList<string>();
    if (list != null && list.Count<string>() > 0)
    {
      List<List<GvgWholeRewardMaster>> wholeRewardMasterListList = new List<List<GvgWholeRewardMaster>>();
      foreach (string str in list)
      {
        string title = str;
        wholeRewardMasterListList.Add(((IEnumerable<GvgWholeRewardMaster>) rewardList).Where<GvgWholeRewardMaster>((Func<GvgWholeRewardMaster, bool>) (x => x.reward_title == title)).ToList<GvgWholeRewardMaster>());
      }
      ((Component) this.scrollContainer.Scroll).transform.Clear();
      this.scrollContainer.Reset();
      foreach (List<GvgWholeRewardMaster> source in wholeRewardMasterListList)
      {
        GameObject gameObject = this.boxPrefab.Clone();
        this.scrollContainer.Add(gameObject);
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(source.ToList<GvgWholeRewardMaster>());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.scrollContainer.Add(this.marginPrefab.Clone());
      }
      this.scrollContainer.ResolvePosition();
    }
  }

  public IEnumerator TestInitialize(WebAPI.Response.GvgResult result)
  {
    yield break;
  }

  public void Close() => Singleton<PopupManager>.GetInstance().dismiss();
}
