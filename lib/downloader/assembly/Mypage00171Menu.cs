// Decompiled with JetBrains decompiler
// Type: Mypage00171Menu
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
public class Mypage00171Menu : BackButtonMenuBase
{
  public NGxScroll scroll;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UILabel TxtPopuptitle;

  private PlayerPresent CreatePresentData(PlayerPresent[] presents, int rewardTypeID)
  {
    int num1 = ((IEnumerable<PlayerPresent>) presents).Where<PlayerPresent>((Func<PlayerPresent, bool>) (x =>
    {
      int? rewardTypeId = x.reward_type_id;
      int num2 = rewardTypeID;
      if (!(rewardTypeId.GetValueOrDefault() == num2 & rewardTypeId.HasValue))
        return false;
      int? rewardQuantity = x.reward_quantity;
      int num3 = 0;
      return rewardQuantity.GetValueOrDefault() > num3 & rewardQuantity.HasValue;
    })).Sum<PlayerPresent>((Func<PlayerPresent, int>) (x => x.reward_quantity.Value));
    if (num1 < 1)
      return (PlayerPresent) null;
    return new PlayerPresent()
    {
      reward_type_id = new int?(rewardTypeID),
      reward_quantity = new int?(num1)
    };
  }

  private List<PlayerPresent> CreatePresentDataByGroup(PlayerPresent[] presents, int rewardTypeID)
  {
    List<PlayerPresent> presentDataByGroup = new List<PlayerPresent>();
    foreach (IGrouping<int, PlayerPresent> source in ((IEnumerable<PlayerPresent>) presents).Where<PlayerPresent>((Func<PlayerPresent, bool>) (x =>
    {
      int? rewardTypeId = x.reward_type_id;
      int num1 = rewardTypeID;
      if (!(rewardTypeId.GetValueOrDefault() == num1 & rewardTypeId.HasValue))
        return false;
      int? rewardQuantity = x.reward_quantity;
      int num2 = 0;
      return rewardQuantity.GetValueOrDefault() > num2 & rewardQuantity.HasValue;
    })).GroupBy<PlayerPresent, int>((Func<PlayerPresent, int>) (x => x.reward_id.Value)))
    {
      int num = source.Sum<PlayerPresent>((Func<PlayerPresent, int>) (x => x.reward_quantity.Value));
      presentDataByGroup.Add(new PlayerPresent()
      {
        reward_id = new int?(source.Key),
        reward_type_id = new int?(rewardTypeID),
        reward_quantity = new int?(num)
      });
    }
    return presentDataByGroup;
  }

  private List<PlayerPresent> CreatePresentList(PlayerPresent[] presents)
  {
    List<PlayerPresent> presentList = new List<PlayerPresent>();
    presentList.AddRange((IEnumerable<PlayerPresent>) ((IEnumerable<PlayerPresent>) presents).Where<PlayerPresent>((Func<PlayerPresent, bool>) (x =>
    {
      int? rewardTypeId1 = x.reward_type_id;
      int num1 = 1;
      if (rewardTypeId1.GetValueOrDefault() == num1 & rewardTypeId1.HasValue)
        return true;
      int? rewardTypeId2 = x.reward_type_id;
      int num2 = 24;
      return rewardTypeId2.GetValueOrDefault() == num2 & rewardTypeId2.HasValue;
    })).ToArray<PlayerPresent>());
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 2));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 3));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 26));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 35));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 16));
    presentList.Add(this.CreatePresentData(presents, 4));
    presentList.Add(this.CreatePresentData(presents, 6));
    presentList.Add(this.CreatePresentData(presents, 10));
    presentList.Add(this.CreatePresentData(presents, 14));
    presentList.Add(this.CreatePresentData(presents, 15));
    presentList.Add(this.CreatePresentData(presents, 17));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 19));
    presentList.Add(this.CreatePresentData(presents, 21));
    presentList.Add(this.CreatePresentData(presents, 23));
    presentList.Add(this.CreatePresentData(presents, 20));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 29));
    presentList.Add(this.CreatePresentData(presents, 11));
    presentList.Add(this.CreatePresentData(presents, 18));
    presentList.Add(this.CreatePresentData(presents, 22));
    presentList.Add(this.CreatePresentData(presents, 30));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 33));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 32));
    presentList.Add(this.CreatePresentData(presents, 31));
    presentList.Add(this.CreatePresentData(presents, 34));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 39));
    presentList.AddRange((IEnumerable<PlayerPresent>) this.CreatePresentDataByGroup(presents, 40));
    return presentList;
  }

  public IEnumerator Init(PlayerPresent[] presents)
  {
    List<PlayerPresent> presentList = this.CreatePresentList(presents);
    Future<GameObject> prefabF = Res.Prefabs.mypage001_7_1.vscroll_576_13.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    this.scroll.Clear();
    foreach (PlayerPresent present in presentList)
    {
      if (present != null)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(prefab);
        this.scroll.Add(gameObject);
        e = gameObject.GetComponent<Mypage00171Scroll>().Init(present);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    this.scroll.scrollView.panel.baseClipRegion = Vector4.zero;
    this.scroll.ResolvePosition();
    yield return (object) null;
    ((UIRect) ((Component) this.scroll).GetComponent<UIWidget>()).alpha = 1f;
    this.TxtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().mypage_001_7_1_title_text, (IDictionary) new Hashtable()
    {
      {
        (object) "ReceiveCnt",
        (object) presents.Length
      }
    }));
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
