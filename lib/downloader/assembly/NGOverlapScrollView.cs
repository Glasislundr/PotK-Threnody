// Decompiled with JetBrains decompiler
// Type: NGOverlapScrollView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (UIPanel))]
[AddComponentMenu("NGUI/Interaction/Overlap Scroll View")]
public class NGOverlapScrollView : UIScrollView
{
  private UIScrollView overlapScrollView;
  public UIScrollView.Movement thisMovement = (UIScrollView.Movement) 1;
  public UIScrollView.Movement? moveMode;
  public UIScrollView.OnDragFinished onOverlapDragFinished;

  public bool canThisMoveHorizontally => this.thisMovement == 0;

  public bool canThisMoveVertically => this.thisMovement == 1;

  protected virtual void Start()
  {
    base.Start();
    this.movement = (UIScrollView.Movement) 2;
    // ISSUE: method pointer
    this.onDragFinished = new UIScrollView.OnDragFinished((object) this, __methodptr(\u003CStart\u003Eb__8_0));
  }

  public void SetOverlapScrollView(UIScrollView scrollView) => this.overlapScrollView = scrollView;

  public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
  {
    if (Object.op_Equality((Object) this.mPanel, (Object) null))
      this.mPanel = ((Component) this).GetComponent<UIPanel>();
    this.DisableSpring();
    Bounds bounds = this.bounds;
    if ((double) ((Bounds) ref bounds).min.x == (double) ((Bounds) ref bounds).max.x || (double) ((Bounds) ref bounds).min.y == (double) ((Bounds) ref bounds).max.y)
      return;
    Vector4 finalClipRegion = this.mPanel.finalClipRegion;
    float num1 = finalClipRegion.z * 0.5f;
    float num2 = finalClipRegion.w * 0.5f;
    float num3 = ((Bounds) ref bounds).min.x + num1;
    float num4 = ((Bounds) ref bounds).max.x - num1;
    float num5 = ((Bounds) ref bounds).min.y + num2;
    float num6 = ((Bounds) ref bounds).max.y - num2;
    Vector3 localPosition = this.mTrans.localPosition;
    localPosition.z = 0.0f;
    this.mTrans.localRotation = Quaternion.Euler(Vector3.zero);
    if (this.canThisMoveHorizontally)
      localPosition.y = 0.0f;
    if (this.canThisMoveVertically)
      localPosition.x = 0.0f;
    if (this.mPanel.clipping == 3)
    {
      num3 -= this.mPanel.clipSoftness.x;
      num4 += this.mPanel.clipSoftness.x;
      num5 -= this.mPanel.clipSoftness.y;
      num6 += this.mPanel.clipSoftness.y;
    }
    float num7 = Mathf.Lerp(num3, num4, x);
    float num8 = Mathf.Lerp(num6, num5, y);
    if (!updateScrollbars)
    {
      if (this.canThisMoveHorizontally)
        localPosition.x += finalClipRegion.x - num7;
      if (this.canThisMoveVertically)
        localPosition.y += finalClipRegion.y - num8;
    }
    this.mTrans.localPosition = localPosition;
    if (this.canThisMoveHorizontally)
      finalClipRegion.x = num7;
    if (this.canThisMoveVertically)
      finalClipRegion.y = num8;
    Vector4 baseClipRegion = this.mPanel.baseClipRegion;
    this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
    if (!updateScrollbars)
      return;
    this.UpdateScrollbars(this.mDragID == -10);
  }

  public virtual void MoveRelative(Vector3 relative)
  {
    if (!this.moveMode.HasValue)
    {
      int num1 = Mathf.CeilToInt(Mathf.Abs(relative.x) * 1000f);
      int num2 = Mathf.CeilToInt(Mathf.Abs(relative.y) * 1000f);
      this.moveMode = num1 != num2 ? (num1 <= num2 ? new UIScrollView.Movement?((UIScrollView.Movement) 1) : new UIScrollView.Movement?((UIScrollView.Movement) 0)) : new UIScrollView.Movement?(this.thisMovement);
    }
    if (!this.moveMode.HasValue)
      return;
    relative.z = 0.0f;
    if (this.moveMode.Value == 1)
      relative.x = 0.0f;
    if (this.moveMode.Value == null)
      relative.y = 0.0f;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(relative.x, relative.y, 0.0f);
    if (this.overlapScrollView.movement == 1)
      vector3.x = 0.0f;
    if (this.overlapScrollView.movement == null)
      vector3.y = 0.0f;
    this.overlapScrollView.MoveRelative(vector3);
    if (this.thisMovement == 1)
      relative.x = 0.0f;
    else if (this.thisMovement == null)
      relative.y = 0.0f;
    if (!this.shouldMoveHorizontally)
      relative.x = 0.0f;
    if (!this.shouldMoveVertically)
      relative.y = 0.0f;
    base.MoveRelative(relative);
  }
}
