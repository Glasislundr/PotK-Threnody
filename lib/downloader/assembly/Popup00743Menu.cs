// Decompiled with JetBrains decompiler
// Type: Popup00743Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup00743Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject baseSheet;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UILabel txtPrice;
  [SerializeField]
  private UILabel txtCaption;
  private bool bResetScrollView;
  private bool bLockScroll;

  private void Awake()
  {
    this.baseSheet.SetActive(false);
    this.bResetScrollView = false;
    this.bLockScroll = false;
  }

  public IEnumerator Init(List<ShopItemListMenu.BattleMedal> medals)
  {
    UIGrid componentInChildren;
    while (Object.op_Equality((Object) (componentInChildren = ((Component) this.scrollView).GetComponentInChildren<UIGrid>()), (Object) null))
      yield return (object) null;
    DateTime nolimit = new DateTime(0L);
    if (!medals.Any<ShopItemListMenu.BattleMedal>((Func<ShopItemListMenu.BattleMedal, bool>) (m => m.limit == nolimit)))
      medals.Add(new ShopItemListMenu.BattleMedal());
    int num = 0;
    int count = medals.Count;
    foreach (ShopItemListMenu.BattleMedal medal in medals)
    {
      GameObject gameObject = NGUITools.AddChild(((Component) componentInChildren).gameObject, this.baseSheet);
      gameObject.SetActive(true);
      Popup00743MenuSheet component = gameObject.GetComponent<Popup00743MenuSheet>();
      component.txtLimit.SetTextLocalize(medal.limit != nolimit ? string.Format(Consts.GetInstance().SHOP_00743_POPUP_DATE_LIMIT, (object) medal.limit) : Consts.GetInstance().SHOP_00743_POPUP_DATE_NOLIMIT);
      component.txtCount.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00743_POPUP_PRICE, (object) medal.count));
      num += medal.count;
      if (--count == 0)
      {
        ((UIRect) this.txtCaption).topAnchor.target = gameObject.transform;
        ((UIRect) this.txtCaption).bottomAnchor.target = gameObject.transform;
        ((UIRect) this.txtCaption).ResetAnchors();
        ((UIRect) this.txtCaption).Update();
      }
    }
    this.txtPrice.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00743_POPUP_TOTAL_PRICE, (object) num));
    this.bResetScrollView = true;
    this.bLockScroll = medals.Count <= 3;
  }

  private void resetScrollView()
  {
    this.scrollView.ResetPosition();
    this.scrollView.RestrictWithinBounds(false, false, true);
    ((Component) this.scrollView).GetComponentInChildren<UIGrid>().Reposition();
    this.scrollView.UpdateScrollbars(true);
  }

  protected override void Update()
  {
    base.Update();
    if (!this.bResetScrollView)
      return;
    this.bResetScrollView = false;
    this.resetScrollView();
    if (!this.bLockScroll)
      return;
    ((Behaviour) this.scrollView).enabled = false;
  }

  public void onClosePressed() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.onClosePressed();
}
