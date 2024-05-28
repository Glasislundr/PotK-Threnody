// Decompiled with JetBrains decompiler
// Type: Gacha99941Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Gacha99941Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtDescription01;
  [SerializeField]
  protected UILabel TxtPopupTitle;

  public virtual void IbtnPopupBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void SetText()
  {
    this.TxtDescription01.SetText(Consts.Format(Consts.GetInstance().GACHA_99941MENU_DESCRIPTION01));
    this.TxtPopupTitle.SetText(Consts.Format(Consts.GetInstance().GACHA_99941MENU_DESCRIPTION02));
  }

  public override void onBackButton() => this.IbtnPopupBack();
}
