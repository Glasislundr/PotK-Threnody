// Decompiled with JetBrains decompiler
// Type: Shop0078Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop0078Menu : BackButtonMenuBase
{
  [SerializeField]
  private Transform ThumParent;
  [SerializeField]
  private UI2DSprite PackThum;
  [SerializeField]
  private GameObject PackThumMount;
  [SerializeField]
  private UI2DSprite Decoration;
  [SerializeField]
  private GameObject PackMark;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  [SerializeField]
  protected UILabel TxtDescription;
  [SerializeField]
  protected UILabel TxtDescription01;
  [SerializeField]
  protected UILabel TxtDescription02;
  private ShopItemIconInfo info;
  private Action callBack;

  public IEnumerator Init(ShopItemIconInfo info)
  {
    this.info = info;
    if (info.playerShopArticle.icon_resource != "")
    {
      this.PackThumMount.SetActive(true);
      ((Component) this.PackThum).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopPack(this.PackThum, info.playerShopArticle.icon_resource);
    }
    else
    {
      this.PackThumMount.SetActive(false);
      ((Component) this.PackThum).gameObject.SetActive(false);
      yield return (object) ShopCommon.CreateThum(this.ThumParent, info.CommonRewardType, info.RewardId);
    }
    if (info.playerShopArticle.decoration_resource != "")
    {
      ((Component) this.Decoration).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopIcon(this.Decoration, info.playerShopArticle.decoration_resource);
    }
    else
      ((Component) this.Decoration).gameObject.SetActive(false);
    if (info.IsPack)
      this.PackMark.SetActive(true);
  }

  public void InitObj(GameObject obj)
  {
    GameObject gameObject = obj.Clone(this.ThumParent);
    UnitIcon component1 = gameObject.GetComponent<UnitIcon>();
    ItemIcon component2 = gameObject.GetComponent<ItemIcon>();
    UniqueIcons component3 = gameObject.GetComponent<UniqueIcons>();
    if (Object.op_Implicit((Object) component1))
      component1.SetIconBoxCollider(false);
    else if (Object.op_Implicit((Object) component2))
    {
      component2.isButtonActive = false;
    }
    else
    {
      if (!Object.op_Implicit((Object) component3))
        return;
      component3.DisableButton();
    }
  }

  public void InitDataSet(
    string name,
    int buyCount,
    int beforeHaveingCount,
    Action ok = null,
    bool isMaterialExchange = false,
    int exchangeCount = 1)
  {
    if (this.info == null)
    {
      this.PackThumMount.SetActive(false);
      ((Component) this.PackThum).gameObject.SetActive(false);
    }
    else if (this.info.playerShopArticle.icon_resource != "")
    {
      this.PackThumMount.SetActive(true);
      ((Component) this.PackThum).gameObject.SetActive(true);
    }
    else
    {
      this.PackThumMount.SetActive(false);
      ((Component) this.PackThum).gameObject.SetActive(false);
    }
    if (isMaterialExchange)
      this.TxtPopuptitle.SetTextLocalize(Consts.GetInstance().VERSUS_0026872POPUP_TITLE2);
    else
      this.TxtPopuptitle.SetTextLocalize(Consts.GetInstance().SHOP_00722_TXT_TITLE);
    this.TxtDescription.SetTextLocalize(name);
    this.TxtDescription01.SetTextLocalize(Consts.Format(isMaterialExchange ? Consts.GetInstance().SHOP_0078_TXT_DESCRIPTION02 : Consts.GetInstance().SHOP_0078_TXT_DESCRIPTION01, (IDictionary) new Hashtable()
    {
      {
        (object) "quantity",
        (object) (isMaterialExchange ? exchangeCount : buyCount).ToLocalizeNumberText()
      }
    }));
    if (beforeHaveingCount >= 0)
    {
      int num1 = buyCount;
      if (this.info != null)
        num1 = buyCount * this.info.playerShopArticle.article.ShopContents[0].quantity;
      int num2 = num1 * exchangeCount;
      this.TxtDescription02.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION_ADD_QUANTITY, (IDictionary) new Hashtable()
      {
        {
          (object) "quantity",
          (object) beforeHaveingCount.ToLocalizeNumberText()
        },
        {
          (object) "quantityNext",
          (object) (beforeHaveingCount + num2).ToLocalizeNumberText()
        }
      }));
    }
    if (ok == null)
      return;
    this.callBack = ok;
  }

  public virtual void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (this.callBack == null)
      return;
    this.callBack();
  }

  public override void onBackButton() => this.IbtnPopupOk();
}
