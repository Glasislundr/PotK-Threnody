// Decompiled with JetBrains decompiler
// Type: Popup026872Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup026872Menu : Popup02610Base
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescriptionYellow;
  [SerializeField]
  private UILabel txtDescriptionNormal;
  [SerializeField]
  private UIButton btnRareMadal;
  [SerializeField]
  private UIButton btnBattleMadal;

  public override void Init(WebAPI.Response.PvpBoot pvpInfo, Versus02610Menu menu)
  {
    base.Init(pvpInfo, menu);
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_0026872POPUP_TITLE);
    this.txtDescriptionYellow.SetText(instance.VERSUS_0026872POPUP_DESCRIPTION1);
    this.txtDescriptionNormal.SetText(instance.VERSUS_0026872POPUP_DESCRIPTION2);
    ((Collider) ((Component) this.btnRareMadal).GetComponent<BoxCollider>()).enabled = pvpInfo.medal_shop_is_available;
    ((Collider) ((Component) this.btnBattleMadal).GetComponent<BoxCollider>()).enabled = pvpInfo.battle_medal_shop_is_available && Player.Current.GetReleaseColosseum();
  }

  public override void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.Return2688Popup());
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnKiseki()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ExchangeTicketPopup());
  }

  private IEnumerator ExchangeTicketPopup()
  {
    Popup026872Menu popup026872Menu = this;
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_7_3__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Popup026873Menu>().Init(popup026872Menu.pvpInfo, popup026872Menu.menu);
  }

  public void IbtnRareMedal()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.ChangeSceneShop(3000));
  }

  public void IbtnBattleMedal()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.ChangeSceneShop(4000));
  }

  private IEnumerator ChangeSceneShop(int shopId)
  {
    ((UIRect) ((Component) this.menu).GetComponent<UIPanel>()).alpha = 0.0f;
    yield return (object) ShopCommon.Init();
    Future<GameObject> prefabF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e1 = prefabF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().setBackground(prefabF.Result);
    e1 = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    })).Then<WebAPI.Response.ShopStatus>((Func<WebAPI.Response.ShopStatus, WebAPI.Response.ShopStatus>) (result =>
    {
      Singleton<NGGameDataManager>.GetInstance().Parse(result);
      return result;
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    ShopItemListScene.ChangeScene(shopId, 0, "", "");
  }
}
