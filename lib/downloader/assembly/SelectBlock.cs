// Decompiled with JetBrains decompiler
// Type: SelectBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelectBlock
{
  public List<SelectBlock.Data> data = new List<SelectBlock.Data>();
  public int selected = -1;
  public GameObject obj;

  public class Data
  {
    public string msg = "";
    public string label = "";
  }
}
