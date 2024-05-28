// Decompiled with JetBrains decompiler
// Type: Versus02612Menu
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
public class Versus02612Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry Scroll;
  [SerializeField]
  private UILabel txtClassName;
  [SerializeField]
  private UISprite slcRankGaugeBlue;
  [SerializeField]
  private UISprite slcRankGaugeGreen;
  [SerializeField]
  private UISprite slcRankGaugeYellow;
  [SerializeField]
  private UISprite slcRankGaugeRed;

  public IEnumerator Init(int id, int best_class)
  {
    this.Scroll.Reset();
    ((Component) this.Scroll.Scroll).gameObject.SetActive(false);
    PvpClassKind c = MasterData.PvpClassKind[id];
    this.txtClassName.SetText(c.name);
    this.SetRankGauge(c);
    Future<GameObject> boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = boxPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject boxPrefab = boxPrefabF.Result;
    Future<GameObject> marginPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
    e = marginPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject marginPrefab = marginPrefabF.Result;
    bool isClear = MasterData.PvpClassKind[best_class].weight >= c.weight;
    Dictionary<PvpClassRewardTypeEnum, List<PvpClassReward>> rewardsDict = this.SetRewardsListOrder(id);
    List<PvpClassRewardTypeEnum> classRewardTypeEnumList = new List<PvpClassRewardTypeEnum>()
    {
      PvpClassRewardTypeEnum.title,
      PvpClassRewardTypeEnum.promotion,
      PvpClassRewardTypeEnum.residual,
      PvpClassRewardTypeEnum.demotion
    };
    if (!isClear)
      classRewardTypeEnumList.Insert(0, PvpClassRewardTypeEnum.first_promotion);
    else
      classRewardTypeEnumList.Add(PvpClassRewardTypeEnum.first_promotion);
    foreach (PvpClassRewardTypeEnum key in classRewardTypeEnumList)
    {
      if (rewardsDict.ContainsKey(key))
      {
        List<PvpClassReward> data = rewardsDict[key];
        GameObject gameObject = boxPrefab.Clone();
        this.Scroll.Add(gameObject);
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(data, isClear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.Scroll.Add(marginPrefab.Clone());
      }
    }
    ((Component) this.Scroll.Scroll).gameObject.SetActive(true);
    this.Scroll.ResolvePosition();
  }

  private Dictionary<PvpClassRewardTypeEnum, List<PvpClassReward>> SetRewardsListOrder(int id)
  {
    return ((IEnumerable<PvpClassReward>) MasterData.PvpClassRewardList).Where<PvpClassReward>((Func<PvpClassReward, bool>) (x => x.class_kind.ID == id)).GroupBy<PvpClassReward, PvpClassRewardTypeEnum>((Func<PvpClassReward, PvpClassRewardTypeEnum>) (x => x.class_reward_type)).ToDictionary<IGrouping<PvpClassRewardTypeEnum, PvpClassReward>, PvpClassRewardTypeEnum, List<PvpClassReward>>((Func<IGrouping<PvpClassRewardTypeEnum, PvpClassReward>, PvpClassRewardTypeEnum>) (x => x.Key), (Func<IGrouping<PvpClassRewardTypeEnum, PvpClassReward>, List<PvpClassReward>>) (y => y.ToList<PvpClassReward>()));
  }

  private void SetRankGauge(PvpClassKind c)
  {
    int width = ((UIWidget) this.slcRankGaugeRed).width;
    int num = 10;
    ((UIWidget) this.slcRankGaugeBlue).width = (c.stay_point - 1) * width / num;
    ((UIWidget) this.slcRankGaugeGreen).width = (c.up_point - 1) * width / num;
    ((UIWidget) this.slcRankGaugeYellow).width = (c.title_point - 1) * width / num;
    ((Component) this.slcRankGaugeBlue).gameObject.SetActive(c.stay_point > 0);
    ((Component) this.slcRankGaugeGreen).gameObject.SetActive(c.up_point - c.stay_point > 0);
    ((Component) this.slcRankGaugeYellow).gameObject.SetActive(c.title_point - c.up_point > 0);
    ((Component) this.slcRankGaugeRed).gameObject.SetActive(true);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
