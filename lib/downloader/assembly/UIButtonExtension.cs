// Decompiled with JetBrains decompiler
// Type: UIButtonExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class UIButtonExtension
{
  public static void SetPossibility(this UIButton button, bool enabled)
  {
    UISprite component = ((Component) button).GetComponent<UISprite>();
    UISprite[] componentsInChildren = ((Component) button).GetComponentsInChildren<UISprite>();
    Color color = enabled ? Color.white : Color.gray;
    Color color1 = color;
    ((UIWidget) component).color = color1;
    ((IEnumerable<UISprite>) componentsInChildren).ForEach<UISprite>((Action<UISprite>) (x => ((UIWidget) x).color = color));
    ((Behaviour) button).enabled = enabled;
  }
}
