// Decompiled with JetBrains decompiler
// Type: CircularMotionBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class CircularMotionBase : MonoBehaviour
{
  [HideInInspector]
  protected float radius;
  [HideInInspector]
  protected Vector3 CenterPosition;
  [SerializeField]
  protected GameObject Director;
  [SerializeField]
  protected MypageSlidePanelDragged CenterTarget;
  protected const float Displace = -100f;

  public virtual void Init(MypageSlidePanelDragged centerObject)
  {
    Transform transform = this.Director.transform;
    Vector3 localPosition = transform.localPosition;
    Vector3 vector3 = ((IEnumerable<TweenPosition>) ((Component) transform.parent).GetComponents<TweenPosition>()).Where<TweenPosition>((Func<TweenPosition, bool>) (x => ((UITweener) x).tweenGroup == MypageMenuBase.START_TWEEN_GROUP_JOGDIAL)).Select<TweenPosition, Vector3>((Func<TweenPosition, Vector3>) (x => x.to)).First<Vector3>();
    if (Object.op_Inequality((Object) centerObject, (Object) null))
      this.CenterTarget = centerObject;
    this.CenterPosition = Vector3.op_Addition(localPosition, vector3);
    this.CenterPosition.x += -100f;
    this.radius = (float) this.Director.GetComponent<UIWidget>().width;
  }
}
