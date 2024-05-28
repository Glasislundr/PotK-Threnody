// Decompiled with JetBrains decompiler
// Type: SeaTalkDestructionConfirmation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class SeaTalkDestructionConfirmation : BackButtonPopupWindow
{
  [SerializeField]
  private UIButton noButton;
  [SerializeField]
  private UIButton YesButton;
  private PlayerCallLetter playerCallLetter;
  private Action callBack;
  private bool OnYesPush;

  public void Init(PlayerCallLetter playerCallLetter, Action callBack)
  {
    this.playerCallLetter = playerCallLetter;
    this.callBack = callBack;
    this.OnYesPush = false;
  }

  public void OnNoButton() => Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();

  public void OnYesButton()
  {
    if (this.OnYesPush)
      return;
    this.OnYesPush = true;
    this.StartCoroutine(this.DoDestruction());
  }

  private IEnumerator DoDestruction()
  {
    IEnumerator e1 = WebAPI.SeaCallDivorce(this.playerCallLetter.same_character_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    SeaTalkMessageMenu.SeaTalkPartnerRefresh();
    this.callBack();
  }

  public override void onBackButton()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
  }
}
