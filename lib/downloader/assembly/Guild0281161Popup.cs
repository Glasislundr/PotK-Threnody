// Decompiled with JetBrains decompiler
// Type: Guild0281161Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Guild0281161Popup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel popupDesc;
  private bool changeGuild;

  public void Initialize(bool fromChangeGuild = false)
  {
    this.changeGuild = fromChangeGuild;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_TITLE));
    this.popupDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_1_DESC));
  }

  public override void onBackButton()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    if (!this.changeGuild)
      return;
    MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
  }
}
