// Decompiled with JetBrains decompiler
// Type: PopupPvpClassEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PopupPvpClassEffect : MonoBehaviour
{
  private Action actEnd;
  private Action actTouch;

  public void Init(Action actEnd, Action actTouch)
  {
    this.actEnd = actEnd;
    this.actTouch = actTouch;
  }

  public void End()
  {
    if (this.actEnd == null)
      return;
    this.actEnd();
  }

  public void Touch()
  {
    if (this.actTouch == null)
      return;
    this.actTouch();
  }
}
