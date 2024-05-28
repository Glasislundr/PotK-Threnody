// Decompiled with JetBrains decompiler
// Type: Bugu0052MedalPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu0052MedalPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private UILabel txtTitle;
  private Bugu0052MedalPopup.CurrencyKind kindType = Bugu0052MedalPopup.CurrencyKind.Money;
  private int price;
  private int boostCnt;
  private Action<bool, int, int> yesAction;

  public void Init(
    Bugu0052MedalPopup.CurrencyKind kind,
    int value,
    int bCnt,
    Action<bool, int, int> yesAct)
  {
    this.kindType = kind;
    this.price = value;
    this.boostCnt = bCnt;
    this.yesAction = yesAct;
    this.txtTitle.SetText(Consts.Format(Consts.GetInstance().BUGU_0052POPUP_TITLE));
    if (kind == Bugu0052MedalPopup.CurrencyKind.RareMedal)
    {
      this.txtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0052POPUP_DESCRIPTION_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "price",
          (object) this.price
        }
      }));
    }
    else
    {
      if (kind != Bugu0052MedalPopup.CurrencyKind.Money)
        return;
      this.txtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0052POPUP_DESCRIPTION_MONEY, (IDictionary) new Hashtable()
      {
        {
          (object) "price",
          (object) this.price
        }
      }));
    }
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.yesAction(this.kindType == Bugu0052MedalPopup.CurrencyKind.RareMedal, this.price, this.boostCnt);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public enum CurrencyKind
  {
    RareMedal,
    Money,
  }
}
