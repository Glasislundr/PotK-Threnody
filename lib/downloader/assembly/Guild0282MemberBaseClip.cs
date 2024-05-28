// Decompiled with JetBrains decompiler
// Type: Guild0282MemberBaseClip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Guild0282MemberBaseClip : MonoBehaviour
{
  private Action onCountDownStar_;
  private Action<string> onSetNumberColor_;

  public void countDownStar()
  {
    if (this.onCountDownStar_ == null)
      return;
    this.onCountDownStar_();
  }

  public void setEventCountDownStar(Action onCountDown = null)
  {
    this.onCountDownStar_ = onCountDown;
  }

  public void setNumberColor(string col)
  {
    if (this.onSetNumberColor_ == null)
      return;
    this.onSetNumberColor_(col);
  }

  public void setEventSetNumberColor(Action<string> onSetNumberColor = null)
  {
    this.onSetNumberColor_ = onSetNumberColor;
  }
}
