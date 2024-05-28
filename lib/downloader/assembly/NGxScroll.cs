// Decompiled with JetBrains decompiler
// Type: NGxScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGxScroll : MonoBehaviour
{
  public UIScrollView scrollView;
  public UIGrid grid;
  [SerializeField]
  [Tooltip("Awake()でのscrollView,gridの設定行為を止めます")]
  private bool disabledAwakeSetting;

  private void Awake()
  {
    if (this.disabledAwakeSetting)
      return;
    this.scrollView.contentPivot = (UIWidget.Pivot) 0;
    this.scrollView.disableDragIfFits = true;
    if (!Object.op_Inequality((Object) this.grid, (Object) null))
      return;
    this.grid.animateSmoothly = false;
    this.grid.keepWithinPanel = true;
  }

  public IEnumerable<GameObject> GridChildren()
  {
    return ((Component) this.grid).transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (t => ((Component) t).gameObject));
  }

  public void Clear() => ((Component) this.grid).transform.Clear();

  public void Add(GameObject obj, bool ignoreResizeCollider = false)
  {
    Transform transform = obj.transform;
    transform.parent = ((Component) this.grid).transform;
    transform.localPosition = Vector3.zero;
    transform.localScale = Vector3.one;
    if (ignoreResizeCollider)
      return;
    BoxCollider componentInChildren = obj.GetComponentInChildren<BoxCollider>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.size = new Vector3(this.grid.cellWidth, this.grid.cellHeight);
  }

  public Vector2 GetScrollPosition()
  {
    Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.scrollView.contentPivot);
    return new Vector2(Object.op_Inequality((Object) this.scrollView.horizontalScrollBar, (Object) null) ? this.scrollView.horizontalScrollBar.value : pivotOffset.x, Object.op_Inequality((Object) this.scrollView.verticalScrollBar, (Object) null) ? this.scrollView.verticalScrollBar.value : 1f - pivotOffset.y);
  }

  public void ResolvePosition(Vector2 pos)
  {
    this.grid.Reposition();
    this.scrollView.ResetPosition();
    this.scrollView.SetDragAmount(pos.x, pos.y, false);
    this.scrollView.SetDragAmount(pos.x, pos.y, true);
  }

  public void ResolvePosition(int index)
  {
    this.grid.Reposition();
    this.scrollView.ResetPosition();
    Bounds bounds1 = this.scrollView.bounds;
    int num1 = (int) ((double) ((Bounds) ref bounds1).size.x / (double) this.grid.cellWidth);
    if (num1 < 1)
      num1 = 1;
    float num2 = (float) (((Component) this.grid).transform.childCount / num1);
    if (((Component) this.grid).transform.childCount % num1 > 0)
      ++num2;
    if ((double) num2 < 1.0)
      num2 = 1f;
    Bounds bounds2 = this.scrollView.bounds;
    float num3 = ((Bounds) ref bounds2).size.y - this.scrollView.panel.height;
    Bounds bounds3 = this.scrollView.bounds;
    double num4 = (double) ((Bounds) ref bounds3).size.y / (double) num2;
    float num5 = (float) (num4 / 2.0) / num3;
    float num6 = this.scrollView.panel.height / 2f / num3;
    float num7 = (float) (num4 * (double) (index / num1) / (double) num3 - ((double) num6 - (double) num5));
    if ((double) num3 <= 0.0)
      num7 = 0.0f;
    else if ((double) num7 < 0.0)
      num7 = 0.0f;
    else if ((double) num7 > 1.0)
      num7 = 1f;
    this.ResolvePosition(new Vector2(0.0f, num7));
  }

  public void ResolvePosition()
  {
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public void GridReposition(UIGrid.OnReposition afterGridReposition, bool oneshot = true)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    NGxScroll.\u003C\u003Ec__DisplayClass11_0 cDisplayClass110 = new NGxScroll.\u003C\u003Ec__DisplayClass11_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.afterGridReposition = afterGridReposition;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    if (cDisplayClass110.afterGridReposition != null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      this.grid.onReposition = !oneshot ? cDisplayClass110.afterGridReposition : new UIGrid.OnReposition((object) cDisplayClass110, __methodptr(\u003CGridReposition\u003Eb__0));
    }
    this.grid.repositionNow = true;
    this.grid.Reposition();
  }

  public void Reset() => ((Component) this.grid).transform.DetachChildren();
}
