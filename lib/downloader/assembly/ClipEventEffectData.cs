// Decompiled with JetBrains decompiler
// Type: ClipEventEffectData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClipEventEffectData : ScriptableObject
{
  public List<ClipEventEffectData.EffectData> dataList;

  public ClipEventEffectData() => this.dataList = new List<ClipEventEffectData.EffectData>();

  [Serializable]
  public class EffectData
  {
    public string effect_name = "";
    public string parent = "";
    public bool is_local_postion;
    public bool is_add_bip;
    public Vector3 position = Vector3.zero;
    public bool is_local_rotation;
    public Vector3 rotation = Vector3.zero;
  }
}
