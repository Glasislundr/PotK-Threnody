// Decompiled with JetBrains decompiler
// Type: DirectionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class DirectionPoint
{
  public Vector3 Point;
  public Vector3 Direction;

  public DirectionPoint(Vector3 direction, Vector3 point)
  {
    this.Direction = direction;
    this.Point = point;
  }

  public static DirectionPoint Zero() => new DirectionPoint(Vector3.zero, Vector3.zero);
}
