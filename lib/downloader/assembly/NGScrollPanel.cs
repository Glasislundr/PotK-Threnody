// Decompiled with JetBrains decompiler
// Type: NGScrollPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Reflection;
using UnityEngine;

#nullable disable
public class NGScrollPanel : MonoBehaviour
{
  public UIPanel panel;
  private int count;

  private void Update()
  {
    if (++this.count % 3 == 0)
      return;
    this.panel.GetType().GetField("mUpdateFrame", BindingFlags.Static | BindingFlags.NonPublic).SetValue((object) this.panel, (object) Time.frameCount);
  }
}
