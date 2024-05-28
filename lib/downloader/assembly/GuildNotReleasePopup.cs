// Decompiled with JetBrains decompiler
// Type: GuildNotReleasePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class GuildNotReleasePopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel lblTitle;
  [SerializeField]
  private UILabel lblDesc;

  public void Initialize()
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.lblTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_COMING_SOON_TITLE));
    this.lblDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_COMING_SOON_DESC));
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
