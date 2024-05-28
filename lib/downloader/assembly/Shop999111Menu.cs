// Decompiled with JetBrains decompiler
// Type: Shop999111Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Shop999111Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription;

  public void SetText()
  {
    string text1 = Consts.Format(Consts.GetInstance().SHOP_999111_TXT_TITLE);
    string text2 = Consts.Format(Consts.GetInstance().SHOP_999111_TXT_DESCRIPTION);
    this.TxtTitle.SetText(text1);
    this.TxtDescription.SetText(text2);
  }

  public virtual void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton() => this.IbtnPopupOk();
}
