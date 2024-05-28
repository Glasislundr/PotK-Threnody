// Decompiled with JetBrains decompiler
// Type: SightPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SightPattern
{
  [SerializeField]
  private GameObject sightImage;
  [SerializeField]
  private Vector3 mapScale;
  [SerializeField]
  private float scrollPower;

  public GameObject SightImage => this.sightImage;

  public Vector3 MapScale => this.mapScale;

  public float ScrollPower => this.scrollPower;
}
