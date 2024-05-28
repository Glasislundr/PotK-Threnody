// Decompiled with JetBrains decompiler
// Type: PopupHotDealSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class PopupHotDealSelect : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll ScrollView;

  public IEnumerator Initialize(HotDealInfo[] data)
  {
    PopupHotDealSelect parent = this;
    Future<GameObject> ft = new ResourceObject("Prefabs/HotDeal/dir_HotDeal_SaleRewards").Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = ft.Result;
    foreach (HotDealInfo data1 in (IEnumerable<HotDealInfo>) ((IEnumerable<HotDealInfo>) data).OrderBy<HotDealInfo, int>((Func<HotDealInfo, int>) (x => x.purchase_limit_sec)))
    {
      PopupHotDealSelectItem component = prefab.CloneAndGetComponent<PopupHotDealSelectItem>();
      parent.ScrollView.Add(((Component) component).gameObject);
      e = component.Initalize(parent, data1, new Action<HotDealInfo>(parent.OnDetailOpenButton));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    // ISSUE: method pointer
    parent.ScrollView.grid.onReposition = new UIGrid.OnReposition((object) parent, __methodptr(\u003CInitialize\u003Eb__1_0));
    parent.ScrollView.ResolvePosition(Vector2.zero);
  }

  public void OnDetailOpenButton(HotDealInfo info)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.StartOpenHotdeal(info));
  }

  private IEnumerator StartOpenHotdeal(HotDealInfo info)
  {
    PopupHotDealSelect popupHotDealSelect = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(2);
    IEnumerator e = new HotDealGenerator().DoDisplay(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popupHotDealSelect.IsPush = false;
  }

  public void OnSelectItemDeactivate() => this.ScrollView.grid.Reposition();

  public void OnCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.OnCloseButton();
}
