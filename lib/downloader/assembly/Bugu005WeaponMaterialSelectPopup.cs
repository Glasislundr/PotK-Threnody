// Decompiled with JetBrains decompiler
// Type: Bugu005WeaponMaterialSelectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu005WeaponMaterialSelectPopup : BackButtonMenuBase
{
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UILabel TxtItemName;
  [SerializeField]
  private UILabel TxtItemDescription;
  [SerializeField]
  private UILabel TxtPossessionNum;
  [SerializeField]
  private UILabel TxtConvertNum;
  [SerializeField]
  private UILabel TxtItemMaxNum;
  [SerializeField]
  private UILabel TxtSelectMax;
  [SerializeField]
  private UI2DSprite LinkItem;
  private Bugu005WeaponMaterialConversionMenu menu;
  private int itemCount;
  private int totalItem;
  private InventoryItem item;

  public void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    this.menu.SetConvertCount(this.item, this.itemCount);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public IEnumerator InitSceneAsync(InventoryItem item, Bugu005WeaponMaterialConversionMenu menu)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu005WeaponMaterialSelectPopup materialSelectPopup = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Behaviour) materialSelectPopup.LinkItem).enabled = false;
      materialSelectPopup.item = item;
      materialSelectPopup.menu = menu;
      materialSelectPopup.Set(item);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ((UIRect) ((Component) materialSelectPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) Bugu005711Menu.InitIcon(item, ((Component) materialSelectPopup.LinkItem).transform.parent.Find("dir_thum"));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Set(InventoryItem item)
  {
    int num = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Count<PlayerItem>((Func<PlayerItem, bool>) (x => !x.isSupply() && !x.isReisou()));
    this.totalItem = item.Item.quantity < this.menu.SelectMax ? item.Item.quantity : this.menu.SelectMax;
    this.TxtItemName.SetText(item.GetName());
    this.TxtItemDescription.SetText(item.GetDescription());
    this.TxtPossessionNum.SetTextLocalize(item.Item.quantity);
    this.TxtConvertNum.SetTextLocalize(this.totalItem);
    this.TxtSelectMax.SetTextLocalize(this.totalItem);
    this.TxtItemMaxNum.SetTextLocalize(num);
    ((UIProgressBar) this.slider).value = 1f;
  }

  protected override void Update()
  {
    this.itemCount = (int) ((double) ((UIProgressBar) this.slider).value * (double) this.totalItem);
    if (this.totalItem <= 1 && (double) ((UIProgressBar) this.slider).value < 1.0)
    {
      if ((double) ((UIProgressBar) this.slider).value >= 0.0099999997764825821)
        this.itemCount = 1;
    }
    else if (this.itemCount > this.totalItem)
    {
      this.itemCount = this.totalItem;
      ((UIProgressBar) this.slider).value = (float) this.itemCount / (float) this.totalItem;
    }
    this.TxtConvertNum.SetTextLocalize(this.itemCount);
  }
}
