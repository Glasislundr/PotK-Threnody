// Decompiled with JetBrains decompiler
// Type: AnimationGachaUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnimationGachaUnit : MonoBehaviour
{
  public EffectControllerGacha gachaCon;
  public MeshRenderer intervalMeshRenderer;
  public MeshRenderer finishMeshRenderer;
  public float Interval = 0.03f;
  private float nowTime;

  private void FixedUpdate()
  {
    this.nowTime += Time.fixedDeltaTime;
    if ((double) this.nowTime < (double) this.Interval)
      return;
    this.setRandomUnit();
    this.nowTime %= this.Interval;
  }

  private void setRandomUnit()
  {
  }
}
