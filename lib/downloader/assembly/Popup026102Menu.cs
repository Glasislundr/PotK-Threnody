// Decompiled with JetBrains decompiler
// Type: Popup026102Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup026102Menu : NGMenuBase
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UILabel message;
  [SerializeField]
  private UILabel message2;
  [SerializeField]
  private UILabel himeMessage;
  [SerializeField]
  private UILabel himeNum;

  public void Init()
  {
    Player player = SMManager.Get<Player>();
    this.title.SetTextLocalize(Consts.GetInstance().PVP_CLASS_MATCH_POPUP_102_TITLE);
    this.message.SetTextLocalize(Consts.GetInstance().PVP_CLASS_MATCH_POPUP_102_MESSAGE);
    this.message2.SetTextLocalize(string.Format(Consts.GetInstance().PVP_CLASS_MATCH_POPUP_102_MESSAGE2, (object) player.mp, (object) player.mp_max));
    this.himeMessage.SetTextLocalize(Consts.GetInstance().PVP_CLASS_MATCH_POPUP_102_HIME_MESSAGE);
    this.himeNum.SetTextLocalize(player.coin);
  }

  public virtual void IbtnYes()
  {
    if (SMManager.Get<Player>().coin > 0)
      this.StartCoroutine(this.MpRecovery());
    else
      this.StartCoroutine(this.ShowStoneAlert());
  }

  public virtual void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  private IEnumerator MpRecovery()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(10000005, 1, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = paramF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (paramF.Result != null)
    {
      Future<GameObject> prefabf = Res.Prefabs.popup.popup_026_10_3__anim_popup01.Load<GameObject>();
      e1 = prefabf.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(prefabf.Result).GetComponent<Popup026103Menu>().Init();
    }
  }

  private IEnumerator ShowStoneAlert()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_buy_kiseki__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result).GetComponent<PopupBuyKiseki>().Init(Consts.GetInstance().PVP_CLASS_MATCH_KISEKI_BUY_MESSAGE);
  }
}
