// Decompiled with JetBrains decompiler
// Type: NGCommon.CameraAspectRatioFitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace NGCommon
{
  [RequireComponent(typeof (Camera))]
  public class CameraAspectRatioFitter : MonoBehaviour
  {
    private const float TARGET_ASPECT_RATIO = 0.5625f;
    private Camera targetCamera;

    private float? BeforeOrthographicSize { set; get; }

    private float? BeforeFieldOfView { set; get; }

    private Camera TargetCamera
    {
      get
      {
        if (Object.op_Equality((Object) this.targetCamera, (Object) null))
          this.targetCamera = ((Component) this).GetComponent<Camera>();
        return this.targetCamera;
      }
    }

    private void Awake() => this.UpdateCamera();

    private void LateUpdate()
    {
      if (this.BeforeOrthographicSize.HasValue && (double) this.BeforeOrthographicSize.Value != (double) this.TargetCamera.orthographicSize)
      {
        this.UpdateCamera();
      }
      else
      {
        if (!this.BeforeFieldOfView.HasValue || (double) this.BeforeFieldOfView.Value == (double) this.TargetCamera.fieldOfView)
          return;
        this.UpdateCamera();
      }
    }

    private void UpdateCamera()
    {
      if (9.0 / 16.0 <= (double) this.TargetCamera.aspect)
        return;
      if (this.TargetCamera.orthographic)
      {
        this.TargetCamera.orthographicSize *= 9f / 16f / this.TargetCamera.aspect;
        this.BeforeOrthographicSize = new float?(this.TargetCamera.orthographicSize);
      }
      else
      {
        this.TargetCamera.fieldOfView = (float) (2.0 * (double) Mathf.Atan(Mathf.Tan((float) ((double) this.TargetCamera.fieldOfView * (Math.PI / 180.0) * 0.5)) * (9f / 16f) / this.TargetCamera.aspect) * 57.295780181884766);
        this.BeforeFieldOfView = new float?(this.TargetCamera.fieldOfView);
      }
    }
  }
}
