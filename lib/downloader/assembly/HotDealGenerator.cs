// Decompiled with JetBrains decompiler
// Type: HotDealGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class HotDealGenerator
{
  private GameObject hotDealPrefab;
  private GameObject withLoupeIconPrefab;
  private HotDeal hotDeal;

  public IEnumerator DoDisplay(HotDealInfo hotdealInfo)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(2);
    IEnumerator e = this.LoadHotDealPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadIconPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObj = this.hotDealPrefab.Clone();
    gameObj.SetActive(false);
    NGTweenParts component = gameObj.GetComponent<NGTweenParts>();
    if (Object.op_Inequality((Object) component, (Object) null))
      Object.Destroy((Object) component);
    this.hotDeal = gameObj.GetOrAddComponent<HotDeal>();
    e = this.hotDeal.Init(this.withLoupeIconPrefab, hotdealInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    gameObj.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(gameObj, true, isCloned: true, isNonSe: true, isNonOpenAnime: true, openAnim: (Action) (() => ((Component) this.hotDeal).gameObject.SetActive(true)), closeAnim: (Action) (() => this.hotDeal.PlayCloseAnimation((Action) (() =>
    {
      if (Object.op_Inequality((Object) this.hotDeal, (Object) null))
        ((Component) this.hotDeal).gameObject.SetActive(false);
      else
        hotdealInfo.is_modal = false;
    }), false)));
    this.hotDeal.PlayItemIconAnimation();
  }

  private IEnumerator LoadHotDealPrefab()
  {
    Future<GameObject> r = new ResourceObject("Prefabs/HotDeal/dir_HotDeal").Load<GameObject>();
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) r.Result, (Object) null))
      this.hotDealPrefab = r.Result;
  }

  private IEnumerator LoadIconPrefab()
  {
    Future<GameObject> r = new ResourceObject("Prefabs/common/dir_Reward_IconOnly_Item").Load<GameObject>();
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) r.Result, (Object) null))
      this.withLoupeIconPrefab = r.Result;
  }
}
