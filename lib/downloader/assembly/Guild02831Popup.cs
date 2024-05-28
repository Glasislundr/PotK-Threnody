// Decompiled with JetBrains decompiler
// Type: Guild02831Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Guild02831Popup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel popupDesc1;
  [SerializeField]
  private UILabel popupDesc2;

  public void Initialize()
  {
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_DISMISS_TITLE));
    this.popupDesc1.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_DISMISS_DESC1));
    this.popupDesc2.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_DISMISS_DESC2));
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
