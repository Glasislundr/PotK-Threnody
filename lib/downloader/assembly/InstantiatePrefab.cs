// Decompiled with JetBrains decompiler
// Type: InstantiatePrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InstantiatePrefab : MonoBehaviour
{
  public GameObject prefab;

  private void Start()
  {
    Object.Instantiate<GameObject>(this.prefab, ((Component) this).transform.position, Quaternion.identity);
  }
}
