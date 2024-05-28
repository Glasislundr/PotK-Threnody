// Decompiled with JetBrains decompiler
// Type: Shop99971Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop99971Menu : NGMenuBase
{
  [SerializeField]
  private UILabel TxtTitleMoney;
  [SerializeField]
  private UILabel TxtTitleMedal;
  [SerializeField]
  private UILabel TxtDescriptionMoney;
  [SerializeField]
  private UILabel TxtDescriptionMedal;

  public void SetText(CommonPayType payType)
  {
    switch (payType)
    {
      case CommonPayType.money:
        string text1 = Consts.Format(Consts.GetInstance().SHOP_99971_TXT_TITLE_MONEY);
        string text2 = Consts.Format(Consts.GetInstance().SHOP_99971_TXT_DESCRIPTION_MONEY);
        this.TxtTitleMoney.SetText(text1);
        this.TxtDescriptionMoney.SetText(text2);
        break;
      case CommonPayType.medal:
      case CommonPayType.battle_medal:
        string text3 = Consts.Format(Consts.GetInstance().SHOP_99971_TXT_TITLE_MEDAL);
        string text4 = Consts.Format(Consts.GetInstance().SHOP_99971_TXT_DESCRIPTION_MEDAL);
        this.TxtTitleMoney.SetText(text3);
        this.TxtDescriptionMedal.SetText(text4);
        break;
      case CommonPayType.tower_medal:
        Hashtable args = new Hashtable()
        {
          {
            (object) "name",
            (object) Consts.GetInstance().SHOP_TOWERMEDAL_NAME
          }
        };
        string text5 = Consts.Format(Consts.GetInstance().SHOP_99971_TXT_TITLE_xNAME, (IDictionary) args);
        string text6 = Consts.Format(Consts.GetInstance().SHOP_99971_TXT_DESCRIPTION_xNAME, (IDictionary) args);
        this.TxtTitleMoney.SetText(text5);
        this.TxtDescriptionMedal.SetText(text6);
        break;
    }
  }

  public void SetText(ShopArticle shop_article) => this.SetText(shop_article.pay_type);

  public virtual void IbtnPopupOk() => Singleton<PopupManager>.GetInstance().closeAll();
}
