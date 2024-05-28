// Decompiled with JetBrains decompiler
// Type: UIScrollViewExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class UIScrollViewExtension
{
  public static void ResetHorizontalCenter(this UIScrollView self, float v)
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return;
    UIPanel component = ((Component) self).GetComponent<UIPanel>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    self.DisableSpring();
    Transform transform = ((Component) self).transform;
    Vector3 localPosition = transform.localPosition;
    Vector4 baseClipRegion = component.baseClipRegion;
    Vector2 clipOffset = component.clipOffset;
    if (self.movement == null)
    {
      localPosition.x = -v;
      transform.localPosition = localPosition;
      clipOffset.x = v - baseClipRegion.x;
    }
    component.clipOffset = clipOffset;
  }

  public static bool RestrictWithinObject(
    this UIScrollView self,
    GameObject target,
    bool horizontal = true,
    bool vertical = true)
  {
    UIPanel panel = self.panel;
    Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(((Component) panel).transform, target.transform);
    Vector3 constrainOffset = panel.CalculateConstrainOffset(Vector2.op_Implicit(((Bounds) ref relativeWidgetBounds).min), Vector2.op_Implicit(((Bounds) ref relativeWidgetBounds).max));
    if (!horizontal)
      constrainOffset.x = 0.0f;
    if (!vertical)
      constrainOffset.y = 0.0f;
    if ((double) ((Vector3) ref constrainOffset).sqrMagnitude <= 1.0)
      return false;
    self.MoveRelative(constrainOffset);
    self.currentMomentum = Vector3.zero;
    return true;
  }
}
