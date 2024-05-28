// Decompiled with JetBrains decompiler
// Type: DebugInfoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DebugInfoScript : MonoBehaviour
{
  public int Stock = 10;
  public Dictionary<string, List<object>> debugInfo = new Dictionary<string, List<object>>();

  public static void Show(string name, object x)
  {
  }
}
