// Decompiled with JetBrains decompiler
// Type: Explore033EasyBillBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Explore033EasyBillBoard : MonoBehaviour
{
  public Camera mTargetCamera;

  private void LateUpdate()
  {
    ((Component) this).transform.LookAt(((Component) this.mTargetCamera).transform.position);
  }
}
