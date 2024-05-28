// Decompiled with JetBrains decompiler
// Type: PopupAutoCloseOnAnyTap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PopupAutoCloseOnAnyTap : BackButtonMenuBase
{
  protected void setEventOnAnyTap(Collider col = null)
  {
    if (Object.op_Equality((Object) col, (Object) null))
      col = (Collider) ((IEnumerable<BoxCollider>) ((Component) ((Component) this).transform.parent).GetComponentsInChildren<BoxCollider>()).FirstOrDefault<BoxCollider>();
    if (!Object.op_Inequality((Object) col, (Object) null))
      return;
    EventDelegate.Set(((Component) col).gameObject.AddComponent<UIEventTrigger>().onRelease, new EventDelegate.Callback(this.onAnyTap));
  }

  public override void onBackButton() => this.onAnyTap();

  private void onAnyTap()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
