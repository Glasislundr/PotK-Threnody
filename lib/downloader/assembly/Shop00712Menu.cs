// Decompiled with JetBrains decompiler
// Type: Shop00712Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00712Menu : BackButtonMenuBase
{
  private Modified<Player> player;
  [SerializeField]
  private GameObject DirPopup01;
  [SerializeField]
  private GameObject DirPopup02;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtPopuptitle;
  [SerializeField]
  private UILabel txtnumber;
  private const int APBP_RECOVERY_SHOP_ID = 10000001;
  private Action btnAct;
  private int before_player_ap;
  private int after_player_ap;
  private string text1 = "[fff000]姫石１個[-]を消費しＡＰを{0}回復します。よろしいですか？";
  private string strPlayerAP = "AP:{0} → [ffff00]{1}[-]/{2}";

  private IEnumerator APRecover()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Player player = this.player.Value;
      this.before_player_ap = player.ap + player.ap_overflow;
      Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(10000001, 1, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      })).Then<WebAPI.Response.ShopBuy>((Func<WebAPI.Response.ShopBuy, WebAPI.Response.ShopBuy>) (result =>
      {
        Singleton<NGGameDataManager>.GetInstance().Parse(result);
        return result;
      }));
      IEnumerator e1 = paramF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (paramF.Result != null)
      {
        WebAPI.Response.ShopBuy result = paramF.Result;
        this.after_player_ap = result.player.ap + result.player.ap_overflow;
        e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
        paramF = (Future<WebAPI.Response.ShopBuy>) null;
      }
    }
  }

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss(true);
    if (SMManager.Get<Player>().CheckKiseki(1))
      Singleton<PopupManager>.GetInstance().monitorCoroutine(this.APRecoverAsync());
    else
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_3_1(Consts.GetInstance().SHOP_99931_TXT_DESCRIPTION));
  }

  private IEnumerator popupApFull()
  {
    Future<GameObject> popupF = Res.Prefabs.popup.popup_999_11_1_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popupF.Result);
  }

  private IEnumerator APRecoverAsync()
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.APRecover();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    e = this.popup_AP_Recovery_result();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator popup_AP_Recovery_result()
  {
    Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_AP_Recovery_result").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<APRecoveryResult>().Init(this.before_player_ap, this.after_player_ap, this.btnAct);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void setUserData()
  {
    this.player = SMManager.Observe<Player>();
    Player player = SMManager.Get<Player>();
    int num1 = player.ap + player.ap_overflow;
    this.TxtDescription01.SetTextLocalize(string.Format(this.text1, (object) Consts.GetInstance().AP_RECOVERY_AMOUNT_STONE));
    int num2 = num1 + int.Parse(Consts.GetInstance().AP_RECOVERY_AMOUNT_STONE);
    this.TxtDescription02.SetTextLocalize(string.Format(this.strPlayerAP, (object) num1, (object) num2, (object) player.ap_max));
    this.txtnumber.SetTextLocalize(player.coin);
  }

  public void SetBtnAction(Action questChangeScene) => this.btnAct = questChangeScene;
}
