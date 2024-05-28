// Decompiled with JetBrains decompiler
// Type: Unit00410Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit00410Popup : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite m_unitIconImage;
  [SerializeField]
  private UILabel m_lbl01_unitName;
  [SerializeField]
  private UILabel m_lbl02_unitCount;
  [SerializeField]
  private UILabel m_lbl03_unitPrice;
  [SerializeField]
  private UILabel m_lbl04_unitQuantity;
  [SerializeField]
  private UILabel m_lbl05_totalPrice;
  [SerializeField]
  private UILabel m_lbl06_maxSellableQuantity;
  [SerializeField]
  private UISlider m_slider;
  private UnitIconInfo m_unitIconInfo;
  private int m_unitCount;
  private int m_unitPrice;
  private List<Action> m_callbackList;
  private int m_maxSellableQuantity = 99;

  public IEnumerator Show(
    UnitIconInfo unitIconInfo,
    List<Action> callbackList,
    int maxSellableQuantity)
  {
    this.m_unitIconInfo = unitIconInfo;
    this.m_callbackList = callbackList;
    this.m_unitCount = unitIconInfo.count;
    this.m_unitPrice = unitIconInfo.icon.PlayerUnit.unit._base_sell_price;
    this.m_maxSellableQuantity = maxSellableQuantity;
    yield return (object) this.InitIcon(this.m_unitIconInfo.icon.PlayerUnit);
    this.InitDialog(this.m_unitIconInfo.icon.PlayerUnit.unit.name, this.m_unitIconInfo.icon.icon, this.m_unitCount, this.m_unitPrice);
  }

  public void OkButton_OnClick()
  {
    if (this.IsPushAndSet())
      return;
    this.m_unitIconInfo.SelectedCount = this.GetUnitSellingQuantity(((UIProgressBar) this.m_slider).value, this.m_maxSellableQuantity, this.m_unitCount);
    if (this.m_unitIconInfo.SelectedCount > 0)
    {
      this.m_unitIconInfo.select = this.m_unitIconInfo.icon.SelIndex;
      this.m_unitIconInfo.gray = true;
    }
    else
      this.m_unitIconInfo.gray = false;
    if (this.m_callbackList != null)
    {
      foreach (Action callback in this.m_callbackList)
        callback();
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void Slider_OnValueChanged()
  {
    int unitSellingQuantity = this.GetUnitSellingQuantity(((UIProgressBar) this.m_slider).value, this.m_maxSellableQuantity, this.m_unitCount);
    this.m_lbl04_unitQuantity.SetTextLocalize(unitSellingQuantity);
    this.m_lbl05_totalPrice.SetTextLocalize((long) unitSellingQuantity * (long) this.m_unitPrice);
  }

  public override void onBackButton() => this.Close();

  private IEnumerator InitIcon(PlayerUnit playerUnit)
  {
    Future<GameObject> unitIconPrefab = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitIconPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitIconObj = Object.Instantiate<GameObject>(unitIconPrefab.Result, ((Component) this.m_unitIconImage).transform.parent.Find("dir_thum"));
    UnitIcon unitIcon = unitIconObj.GetComponent<UnitIcon>();
    e = unitIcon.SetUnit(playerUnit, playerUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIcon.setBottom(playerUnit);
    unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ((Collider) unitIcon.buttonBoxCollider).enabled = false;
    unitIconObj.gameObject.SetActive(true);
  }

  private void InitDialog(string unitName, UI2DSprite iconImage, int unitCount, int unitPrice)
  {
    ((Behaviour) this.m_unitIconImage).enabled = false;
    this.m_lbl01_unitName.SetTextLocalize(unitName);
    this.m_lbl02_unitCount.SetTextLocalize(unitCount);
    this.m_lbl03_unitPrice.SetTextLocalize(unitPrice);
    this.m_lbl06_maxSellableQuantity.SetTextLocalize(Mathf.Min(this.m_maxSellableQuantity, unitCount));
    ((UIProgressBar) this.m_slider).value = 1f;
    this.Slider_OnValueChanged();
  }

  private int GetUnitSellingQuantity(float sliderValue, int maxSellableQuantity, int unitCount)
  {
    return (int) ((double) sliderValue * (double) Mathf.Min(maxSellableQuantity, unitCount));
  }

  private void Close()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
