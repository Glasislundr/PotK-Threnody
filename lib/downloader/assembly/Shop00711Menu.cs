// Decompiled with JetBrains decompiler
// Type: Shop00711Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00711Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtDescription01;
  [SerializeField]
  protected UILabel TxtDescription02;
  [SerializeField]
  protected UILabel TxtDescription03;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  [SerializeField]
  private UI2DSprite linkItem;
  public int quantity;
  private string itemName;
  private int itemQuantity;

  public PlayerShopArticle shop { get; set; }

  public IEnumerator Init(
    PlayerShopArticle psa,
    int purchasedStoneQuentityBefore,
    Sprite spr,
    string nm,
    int qua)
  {
    this.SetSprite(spr);
    yield break;
  }

  public IEnumerator Init(ProductInfo product, int before, int after)
  {
    Future<Sprite> coinF = CoinProduct.LoadSpriteThumbnail(product, false);
    IEnumerator e = coinF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetSprite(coinF.Result);
    this.SetText(before, after);
    this.TxtDescription01.SetText("[ffff00]" + product.LocalizedTitle.ToConverter() + "[-]");
  }

  public void SetSprite(Sprite spr) => this.linkItem.sprite2D = spr;

  public void SetText(int befereCoin, int afterCoin)
  {
    this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHPT_0711_MENU_BUY_KISEKI, (IDictionary) new Hashtable()
    {
      {
        (object) "before",
        (object) befereCoin
      },
      {
        (object) "after",
        (object) afterCoin
      }
    }));
  }

  public virtual void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    PurchaseBehavior.PopupDismiss();
    PurchaseBehaviorLoadingLayer.Disable();
    if (PurchaseBehavior.IsBattleNow)
      return;
    Shop0079Menu.IsBuyOrReceiveBack = true;
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupUtility.BuyKiseki(PurchaseBehavior.IsBattleNow));
  }

  public override void onBackButton() => this.IbtnPopupOk();
}
