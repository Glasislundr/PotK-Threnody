// Decompiled with JetBrains decompiler
// Type: GuildOkPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GuildOkPopup : BackButtonMenuBase
{
  private Action okCallback;
  private const int CommonDlgSizeX = 532;
  private const int CommonDlgSizeY = 345;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel desc;
  [SerializeField]
  private UISprite slc_Popupbox;

  public void Initialize(string title, string message, Vector2? size = null, Action ok = null)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.popupTitle.SetTextLocalize(title);
    this.desc.SetTextLocalize(message);
    this.okCallback = ok;
    ((UIWidget) this.slc_Popupbox).SetDimensions(size.HasValue ? (int) size.Value.x : 532, size.HasValue ? (int) size.Value.y : 345);
  }

  public override void onBackButton()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.okCallback == null)
      return;
    this.okCallback();
  }
}
