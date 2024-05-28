// Decompiled with JetBrains decompiler
// Type: TBOXVelocityController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TBOXVelocityController : MonoBehaviour
{
  public Rigidbody rgb;
  public float acc;

  private void AddVelocity() => this.rgb.velocity = new Vector3(0.0f, this.acc, 0.0f);
}
