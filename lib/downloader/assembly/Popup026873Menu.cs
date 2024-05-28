// Decompiled with JetBrains decompiler
// Type: Popup026873Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup026873Menu : Popup02610Base
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription1;
  [SerializeField]
  private UILabel txtDescription2;
  [SerializeField]
  private UILabel txtHime;
  [SerializeField]
  private UILabel txtHimeCount;

  public override void Init(WebAPI.Response.PvpBoot pvpInfo, Versus02610Menu menu)
  {
    base.Init(pvpInfo, menu);
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_0026873POPUP_TITLE);
    this.txtDescription1.SetText(instance.VERSUS_0026873POPUP_DESCRIPTION1);
    this.txtDescription2.SetText(instance.VERSUS_0026873POPUP_DESCRIPTION2);
    this.txtHimeCount.SetText(Player.Current.coin.ToLocalizeNumberText());
    this.txtHime.SetText(instance.GACHA_99931MENU_DESCRIPTION03);
  }

  public void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    if (Player.Current.CheckKiseki(1))
      Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.TicketRecovery());
    else
      this.StartCoroutine(this.ShortStoneAlert());
  }

  public override void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator TicketRecovery()
  {
    Popup026873Menu popup026873Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(10000006, 1, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = paramF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (paramF.Result != null)
    {
      Consts instance = Consts.GetInstance();
      // ISSUE: reference to a compiler-generated method
      ModalWindow.Show(instance.VERSUS_0026873POPUP_TITLE, instance.VERSUS_0026873POPUP_RECOVERY_DESCRIPTION, new Action(popup026873Menu.\u003CTicketRecovery\u003Eb__9_1));
    }
  }

  private IEnumerator ShortStoneAlert()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    IEnumerator e = PopupUtility._999_3_1(Consts.GetInstance().VERSUS_0026873POPUP_SHORTSTONE_DESCRIPTION);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
