// Decompiled with JetBrains decompiler
// Type: Friend008161Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Friend008161Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription;

  public IEnumerator Init(string title = "", string description = "")
  {
    if (string.IsNullOrEmpty(title))
      title = Consts.GetInstance().POPUP_0186_COPY_POPUP_TITLE;
    if (string.IsNullOrEmpty(description))
      description = Consts.GetInstance().POPUP_0186_COPY_POPUP_DESCRIPTION;
    this.txtTitle.SetText(title);
    this.txtDescription.SetText(description);
    yield break;
  }

  public virtual void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo() => this.IbtnOk();

  public override void onBackButton() => this.IbtnNo();
}
