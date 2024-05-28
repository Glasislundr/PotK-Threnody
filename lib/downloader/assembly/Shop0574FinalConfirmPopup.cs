// Decompiled with JetBrains decompiler
// Type: Shop0574FinalConfirmPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop0574FinalConfirmPopup : BackButtonMenuBase
{
  [SerializeField]
  private GameObject DirThum;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtDescription04;
  [SerializeField]
  private UILabel TxtDescription05;
  private Action<EarthShopArticle, GameObject, int> EndAction;
  private EarthShopArticle shopArticle;
  private int mBuyNum;

  public void Init(
    EarthShopArticle article,
    GameObject thum,
    int buyNum,
    Action<EarthShopArticle, GameObject, int> endAction)
  {
    long num1 = 0;
    long num2 = 0;
    long number = (long) buyNum * (long) article.price;
    EarthDataManager instanceOrNull = Singleton<EarthDataManager>.GetInstanceOrNull();
    if (Object.op_Inequality((Object) instanceOrNull, (Object) null))
    {
      num1 = instanceOrNull.GetProperty(MasterDataTable.CommonRewardType.money);
      num2 = num1 - number;
    }
    this.EndAction = endAction;
    this.shopArticle = article;
    this.mBuyNum = buyNum;
    thum.Clone(this.DirThum.transform);
    this.TxtDescription01.SetText(article.name);
    this.TxtDescription02.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION02_MONEY, (IDictionary) new Hashtable()
    {
      {
        (object) "money",
        (object) num1
      },
      {
        (object) "moneyNext",
        (object) num2
      }
    }));
    this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MONEY, (IDictionary) new Hashtable()
    {
      {
        (object) "money",
        (object) article.price
      }
    }));
    this.TxtDescription04.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION04, (IDictionary) new Hashtable()
    {
      {
        (object) "quantity",
        (object) buyNum
      }
    }));
    this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MONEY, (IDictionary) new Hashtable()
    {
      {
        (object) "money",
        (object) number.ToLocalizeNumberText()
      }
    }));
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.EndAction == null)
      return;
    this.EndAction(this.shopArticle, this.DirThum, this.mBuyNum);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
