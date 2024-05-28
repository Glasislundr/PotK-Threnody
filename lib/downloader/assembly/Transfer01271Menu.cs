// Decompiled with JetBrains decompiler
// Type: Transfer01271Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Transfer01271Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scroll;

  public void Init() => ((Component) this.scroll.scrollView).transform.localPosition = Vector3.zero;

  public void IbtnPopupPublish()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Consts.GetInstance();
    Singleton<CommonRoot>.GetInstance().isLoading = true;
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
