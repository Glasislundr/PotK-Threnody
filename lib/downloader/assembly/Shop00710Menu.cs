// Decompiled with JetBrains decompiler
// Type: Shop00710Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00710Menu : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite linkItem;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtPopuptitle;
  public string itemTitle;
  public string itemQuantity;
  public int itemPrice;
  public int purchasedStoneQuentity;
  private Sprite sendSprite;
  private string sendName;

  public Player player { get; set; }

  public PlayerShopArticle shopArticle { get; set; }

  public IEnumerator Init(
    PlayerShopArticle psa,
    int purchasedStoneQuentity,
    Sprite spr,
    string title,
    string kisekiNum)
  {
    this.SetSprite(spr);
    this.SetText(title, kisekiNum);
    yield break;
  }

  public void SetSprite(Sprite item) => this.linkItem.sprite2D = item;

  public void SetText(string title, string kisekiNum)
  {
    Consts instance = Consts.GetInstance();
    this.itemTitle = title;
    this.itemQuantity = kisekiNum;
    this.TxtDescription01.SetTextLocalize(this.itemTitle + kisekiNum + Consts.GetInstance().SHOP_00710_MENU);
    this.TxtDescription02.SetText(instance.SHOP_00710_TXT_DESCRIPTION02);
    this.TxtDescription03.SetTextLocalize("");
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
