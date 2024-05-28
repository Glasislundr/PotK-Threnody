// Decompiled with JetBrains decompiler
// Type: ShopGiftWeek
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ShopGiftWeek : MonoBehaviour
{
  [SerializeField]
  private UIWidget pageUiWidget;
  [SerializeField]
  private UIScrollView scroll;

  public IEnumerator Init(WebAPI.Response.CoinbonusHistory coinbonusHistory)
  {
    ((UIRect) this.pageUiWidget).alpha = 0.0f;
    Future<GameObject> prefab = new ResourceObject("Prefabs/shop007_9/slc_Box_Gift_Week").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject baseItemGiftWeek = prefab.Result;
    prefab = new ResourceObject("Prefabs/common/dir_Reward_IconOnly_Item").Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject withLoupeIcon = prefab.Result;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTime();
    float itemPositonY = 0.0f;
    WeeklyPackInfo[] weeklyPackInfoArray = coinbonusHistory.weekly_packs;
    for (int index = 0; index < weeklyPackInfoArray.Length; ++index)
    {
      WeeklyPackInfo packInfo = weeklyPackInfoArray[index];
      GameObject itemGiftWeek = baseItemGiftWeek.Clone(((Component) this.scroll).transform);
      string productId = ((IEnumerable<CoinGroup>) coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetProductId();
      e = itemGiftWeek.GetComponent<ShopGiftWeekItem>().Init(withLoupeIcon, packInfo, productId, ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).First<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == productId)).LocalizedPrice, now);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Transform transform = itemGiftWeek.transform;
      transform.localPosition = new Vector3(transform.localPosition.x, itemPositonY, 0.0f);
      itemPositonY -= (float) (((UIWidget) itemGiftWeek.GetComponent<UISprite>()).height + 10);
      itemGiftWeek = (GameObject) null;
    }
    weeklyPackInfoArray = (WeeklyPackInfo[]) null;
    this.scroll.ResetPosition();
    ((UIRect) this.pageUiWidget).alpha = 1f;
  }
}
