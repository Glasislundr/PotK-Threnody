// Decompiled with JetBrains decompiler
// Type: PopupHotDealSelectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable disable
public class PopupHotDealSelectItem : MonoBehaviour
{
  [SerializeField]
  private UILabel NameLbl;
  [SerializeField]
  private UILabel PriceTagNumLbl;
  [Space(8f)]
  [SerializeField]
  private UIButton OpenButton;
  [SerializeField]
  private UILabel RestTimeLbl;
  [Space(8f)]
  [SerializeField]
  private UIGrid Grid;
  [SerializeField]
  private int ScrollEnableItemCnt;
  [SerializeField]
  private Collider FrickArea;
  [SerializeField]
  private UIButton LeftArrow;
  [SerializeField]
  private UIButton RightArrow;
  private PopupHotDealSelect Parent;
  private Action<HotDealInfo> ButtonCallback;
  private HotDealInfo HotDealInfo;
  private DateTime EndDateTime;
  private bool TweenFlg;

  public IEnumerator Initalize(
    PopupHotDealSelect parent,
    HotDealInfo data,
    Action<HotDealInfo> buttonCallback)
  {
    this.Parent = parent;
    this.ButtonCallback = buttonCallback;
    this.HotDealInfo = data;
    this.EndDateTime = this.HotDealInfo.EndDateTime;
    this.NameLbl.SetTextLocalize(data.name);
    this.SetupPriceLabel();
    Future<GameObject> r = new ResourceObject("Prefabs/HotDeal/dir_HotDeal_SaleReward_IconOnly_Item").Load<GameObject>();
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject iconPrefeb = r.Result;
    int iconCnt = 0;
    int additionalPaidCoin = this.HotDealInfo.GetAdditionalPaidCoin();
    if (additionalPaidCoin > 0)
    {
      e = iconPrefeb.CloneAndGetComponent<PopupHotDealSelectItemIcon>(((Component) this.Grid).transform).Initialize(MasterDataTable.CommonRewardType.coin, -999, additionalPaidCoin);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++iconCnt;
    }
    HotDealRewardSchema[] dealRewardSchemaArray = this.HotDealInfo.rewards;
    for (int index = 0; index < dealRewardSchemaArray.Length; ++index)
    {
      HotDealRewardSchema dealRewardSchema = dealRewardSchemaArray[index];
      e = iconPrefeb.CloneAndGetComponent<PopupHotDealSelectItemIcon>(((Component) this.Grid).transform).Initialize((MasterDataTable.CommonRewardType) dealRewardSchema.reward_type_id, dealRewardSchema.reward_id, dealRewardSchema.reward_quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++iconCnt;
    }
    dealRewardSchemaArray = (HotDealRewardSchema[]) null;
    if (iconCnt < this.ScrollEnableItemCnt)
    {
      this.FrickArea.enabled = false;
      ((Component) this.RightArrow).gameObject.SetActive(false);
    }
    ((Component) this.LeftArrow).gameObject.SetActive(false);
    this.Grid.Reposition();
  }

  public void OnDetailOpenButton() => this.ButtonCallback(this.HotDealInfo);

  private void Update()
  {
    if (this.HotDealInfo.IsPurchased)
    {
      ((Component) this).gameObject.SetActive(false);
      this.Parent.OnSelectItemDeactivate();
    }
    else
      this.UpdateEndingTime();
  }

  private void UpdateEndingTime()
  {
    TimeSpan timeSpan = this.EndDateTime - DateTime.Now;
    StringBuilder stringBuilder = new StringBuilder("あと");
    if (timeSpan.Days > 0)
    {
      stringBuilder.Append(timeSpan.Days);
      stringBuilder.Append(Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_D);
    }
    else if (timeSpan.Hours > 0)
    {
      stringBuilder.Append(timeSpan.Hours);
      stringBuilder.Append(Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_H);
    }
    else if (timeSpan.Minutes > 0)
    {
      stringBuilder.Append(timeSpan.Minutes);
      stringBuilder.Append(Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_M);
    }
    else
    {
      int num = timeSpan.Seconds;
      if (num < 0)
      {
        ((Behaviour) this).enabled = false;
        ((UIButtonColor) this.OpenButton).isEnabled = false;
        ((UIWidget) this.RestTimeLbl).color = Color32.op_Implicit(new Color32((byte) 128, (byte) 128, (byte) 128, byte.MaxValue));
        num = 0;
      }
      stringBuilder.Append(num);
      stringBuilder.Append(Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_S);
    }
    this.RestTimeLbl.SetTextLocalize(stringBuilder.ToString());
  }

  private void SetupPriceLabel()
  {
    this.PriceTagNumLbl.SetTextLocalize(string.Empty);
    string productId = this.HotDealInfo.GetProductId();
    ProductInfo productInfo = PurchaseFlow.ProductList != null ? ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).FirstOrDefault<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == productId)) : (ProductInfo) null;
    if (productInfo == null)
      return;
    Dictionary<char, string> dictionary = new Dictionary<char, string>()
    {
      {
        '０',
        "0"
      },
      {
        '１',
        "1"
      },
      {
        '２',
        "2"
      },
      {
        '３',
        "3"
      },
      {
        '４',
        "4"
      },
      {
        '５',
        "5"
      },
      {
        '６',
        "6"
      },
      {
        '７',
        "7"
      },
      {
        '８',
        "8"
      },
      {
        '９',
        "9"
      }
    };
    StringBuilder stringBuilder = new StringBuilder();
    string localizedPrice = productInfo.LocalizedPrice;
    for (int index = 0; index < localizedPrice.Length; ++index)
    {
      if (dictionary.ContainsKey(localizedPrice[index]))
        stringBuilder.Append(dictionary[localizedPrice[index]]);
      else
        stringBuilder.Append(localizedPrice[index]);
    }
    this.PriceTagNumLbl.SetTextLocalize(stringBuilder.ToString());
  }

  public void OnSwipeRight()
  {
    UITweener component = ((Component) this.Grid).GetComponent<UITweener>();
    if (((Behaviour) component).enabled || !this.TweenFlg)
      return;
    ((Component) this.LeftArrow).gameObject.SetActive(false);
    ((Component) this.RightArrow).gameObject.SetActive(true);
    component.PlayReverse();
    this.TweenFlg = false;
  }

  public void OnSwipeLeft()
  {
    UITweener component = ((Component) this.Grid).GetComponent<UITweener>();
    if (((Behaviour) component).enabled || this.TweenFlg)
      return;
    ((Component) this.LeftArrow).gameObject.SetActive(true);
    ((Component) this.RightArrow).gameObject.SetActive(false);
    component.PlayForward();
    this.TweenFlg = true;
  }
}
