// Decompiled with JetBrains decompiler
// Type: MypageSlidePanelOpen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MypageSlidePanelOpen : MonoBehaviour
{
  private Action<MypageSlidePanelOpen> endAction;

  public void Init(Action<MypageSlidePanelOpen> action) => this.endAction = action;

  public void StartEffect()
  {
  }

  public void EndEffect() => this.endAction(this);
}
