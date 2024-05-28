// Decompiled with JetBrains decompiler
// Type: Guild02833Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Guild02833Popup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel popupDesc1;
  [SerializeField]
  private UILabel popupDesc2;

  public void Initialize()
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_RESIGN_TITLE));
    this.popupDesc1.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_RESIGN_DESC4));
    this.popupDesc2.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_RESIGN_DESC5));
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
