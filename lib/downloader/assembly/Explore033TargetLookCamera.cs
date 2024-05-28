// Decompiled with JetBrains decompiler
// Type: Explore033TargetLookCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
public class Explore033TargetLookCamera : MonoBehaviour
{
  public Transform mTarget;
  private Camera mCamera;

  private void Awake() => this.mCamera = ((Component) this).GetComponent<Camera>();

  private void LateUpdate() => this.LookAtTarget();

  public void LookAtTarget() => ((Component) this.mCamera).transform.LookAt(this.mTarget);

  public void MoveByPath(Vector3[] cameraPath, Vector3[] targetPath, float duration)
  {
    this.mTarget.localPosition = targetPath[0];
    iTween.MoveTo(((Component) this.mTarget).gameObject, new Hashtable()
    {
      {
        (object) "islocal",
        (object) true
      },
      {
        (object) "time",
        (object) duration
      },
      {
        (object) "path",
        (object) targetPath
      }
    });
    ((Component) this).transform.localPosition = cameraPath[0];
    iTween.MoveTo(((Component) this).gameObject, new Hashtable()
    {
      {
        (object) "islocal",
        (object) true
      },
      {
        (object) "time",
        (object) duration
      },
      {
        (object) "path",
        (object) cameraPath
      }
    });
    this.LookAtTarget();
  }
}
