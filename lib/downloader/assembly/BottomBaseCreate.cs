// Decompiled with JetBrains decompiler
// Type: BottomBaseCreate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BottomBaseCreate : MonoBehaviour
{
  public static GameObject BottomBase(Transform parent)
  {
    return Resources.Load<GameObject>("Prefabs/UnitIcon/bottom_base").Clone(parent);
  }
}
