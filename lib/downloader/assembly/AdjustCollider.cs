// Decompiled with JetBrains decompiler
// Type: AdjustCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AdjustCollider : MonoBehaviour
{
  [SerializeField]
  private BoxCollider boxCollider;
  [SerializeField]
  private UIWidget widget;
  [SerializeField]
  private Vector2 addBoxSize;

  private void Start()
  {
    if (Object.op_Equality((Object) this.boxCollider, (Object) null) || Object.op_Equality((Object) this.widget, (Object) null))
      return;
    Vector4 drawingDimensions = this.widget.drawingDimensions;
    this.boxCollider.center = new Vector3((float) (((double) drawingDimensions.x + (double) drawingDimensions.z) * 0.5), (float) (((double) drawingDimensions.y + (double) drawingDimensions.w) * 0.5));
    this.boxCollider.size = new Vector3(drawingDimensions.z - drawingDimensions.x + this.addBoxSize.x, drawingDimensions.w - drawingDimensions.y + this.addBoxSize.y);
  }
}
