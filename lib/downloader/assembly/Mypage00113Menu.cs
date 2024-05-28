// Decompiled with JetBrains decompiler
// Type: Mypage00113Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Mypage00113Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scroll;
  [SerializeField]
  private UILabel bodyText;

  public void Initialize()
  {
    this.bodyText.SetTextLocalize(Consts.GetInstance().COPYRIGHT);
    this.scroll.ResolvePosition();
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
