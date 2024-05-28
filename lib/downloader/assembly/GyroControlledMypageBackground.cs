// Decompiled with JetBrains decompiler
// Type: GyroControlledMypageBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (RuntimeImageEffectController))]
public class GyroControlledMypageBackground : GyroControlledObject
{
  public float maxInterpolationProgressRate = 1f;
  private RuntimeImageEffectController imageEffectController;
  private float originalInterpolationProgress;

  public override void OnControllerStart()
  {
    this.imageEffectController = ((Component) this).GetComponent<RuntimeImageEffectController>();
    this.originalInterpolationProgress = this.imageEffectController.interpolationProgress;
  }

  public override void OnLerp(float? tX, float? tY)
  {
    if (!tX.HasValue && tY.HasValue)
      this.imageEffectController.interpolationProgress = (float) (-(double) Mathf.Cos((float) (((double) tY.Value * 2.0 - 1.0) * 3.1415927410125732)) * 0.5 + 0.5) * this.maxInterpolationProgressRate;
    else if (tX.HasValue && !tY.HasValue)
    {
      this.imageEffectController.interpolationProgress = (float) (-(double) Mathf.Cos((float) (((double) tX.Value * 2.0 - 1.0) * 3.1415927410125732)) * 0.5 + 0.5) * this.maxInterpolationProgressRate;
    }
    else
    {
      if (!tX.HasValue || !tY.HasValue)
        return;
      if ((double) Mathf.Abs(tX.Value - 0.5f) > (double) Mathf.Abs(tY.Value - 0.5f))
        this.imageEffectController.interpolationProgress = (float) (-(double) Mathf.Cos((float) (((double) tX.Value * 2.0 - 1.0) * 3.1415927410125732)) * 0.5 + 0.5) * this.maxInterpolationProgressRate;
      else
        this.imageEffectController.interpolationProgress = (float) (-(double) Mathf.Cos((float) (((double) tY.Value * 2.0 - 1.0) * 3.1415927410125732)) * 0.5 + 0.5) * this.maxInterpolationProgressRate;
    }
  }

  public override void OnControllerStop()
  {
    if (!Object.op_Inequality((Object) this.imageEffectController, (Object) null))
      return;
    this.imageEffectController.interpolationProgress = this.originalInterpolationProgress;
  }
}
