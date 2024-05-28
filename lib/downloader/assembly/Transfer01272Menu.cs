// Decompiled with JetBrains decompiler
// Type: Transfer01272Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Auth;
using SM;
using System;
using UnityEngine;

#nullable disable
public class Transfer01272Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtID;
  [SerializeField]
  protected UIInput InputPassword;
  [SerializeField]
  protected UILabel TxtDescription;
  [SerializeField]
  protected SpreadColorButton BtnDecide;
  [SerializeField]
  protected SpreadColorButton BtnCopy;

  public void InitTransfer()
  {
    this.TxtID.SetTextLocalize(SMManager.Get<Player>().short_id);
    this.TxtDescription.SetTextLocalize(Consts.GetInstance().TRANSFER01271_DESCRIPTION);
    ((UIButtonColor) this.BtnDecide).isEnabled = false;
  }

  public void OnChange()
  {
    if (!((UIButtonColor) this.BtnDecide).isEnabled && this.InputPassword.value.Length >= 8)
    {
      ((UIButtonColor) this.BtnDecide).isEnabled = true;
    }
    else
    {
      if (!((UIButtonColor) this.BtnDecide).isEnabled || this.InputPassword.value.Length >= 8)
        return;
      ((UIButtonColor) this.BtnDecide).isEnabled = false;
    }
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnDecide()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Consts consts = Consts.GetInstance();
    WebAPI.AuthRegisterEmailAddressAndPassword(SMManager.Get<Player>().short_id, this.InputPassword.value, (Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      ModalWindow.Show(consts.TRANSFER01271_POPUP_TITLE, consts.TRANSFER01271_POPUP_TEXT1, (Action) (() => this.IsPush = false));
    }), new Action<RegisterEmailAddressAndPasswordResult>(this.ErrorPopup));
  }

  private void ErrorPopup(RegisterEmailAddressAndPasswordResult error)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Consts instance = Consts.GetInstance();
    switch (error.ResultCode)
    {
      case RegisterEmailAddressAndPasswordResultCode.InvalidEmailAddress:
        ModalWindow.Show(instance.TRANSFER01271_POPUP_TITLE, instance.TRANSFER01271_ERROR_POPUP_TEXT2, (Action) (() => this.IsPush = false));
        break;
      case RegisterEmailAddressAndPasswordResultCode.InvalidPassword:
        ModalWindow.Show(instance.TRANSFER01271_POPUP_TITLE, instance.TRANSFER01271_ERROR_POPUP_TEXT3, (Action) (() => this.IsPush = false));
        break;
      case RegisterEmailAddressAndPasswordResultCode.DuplicatedEmailAddress:
        ModalWindow.Show(instance.TRANSFER01271_POPUP_TITLE, instance.TRANSFER01271_ERROR_POPUP_TEXT1, (Action) (() => this.IsPush = false));
        break;
      case RegisterEmailAddressAndPasswordResultCode.UnknownError:
        ModalWindow.Show(instance.TRANSFER01271_POPUP_TITLE, instance.TRANSFER01271_ERROR_POPUP_TEXT4, (Action) (() => this.IsPush = false));
        break;
    }
  }

  public void IbtnCopy()
  {
    if (this.IsPushAndSet())
      return;
    NGUITools.clipboard = SMManager.Get<Player>().short_id.ToString();
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.USERCODE_COPY_TITLE, instance.USERCODE_COPY, (Action) (() => this.IsPush = false));
  }
}
