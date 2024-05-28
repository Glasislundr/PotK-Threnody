// Decompiled with JetBrains decompiler
// Type: DragHVScrollView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DragHVScrollView : MonoBehaviour
{
  [SerializeField]
  [Tooltip("水平移動スクロールを設定")]
  private UIScrollView horizontal_;
  [SerializeField]
  [Tooltip("垂直移動スクロールを設定")]
  private UIScrollView vertical_;
  [SerializeField]
  [Tooltip("この角度までを垂直方向移動と判断します(垂直:0～真横:90°)")]
  private float thresholdVertical_ = 50f;
  [SerializeField]
  [Tooltip("状況確認用")]
  private DragHVScrollView.MoveMode mode_;
  [SerializeField]
  [Tooltip("未ドラッグ状態でのホイールスクロール方向")]
  private DragHVScrollView.MoveMode defaultWheel_ = DragHVScrollView.MoveMode.Vertical;
  private bool isError_;
  private bool isDraggable_;
  private bool logPressed_;
  private const float ANGLE_MAX = 90f;

  public bool isError => this.isError_;

  public bool isDraggable => this.isDraggable_;

  public bool logPressed => this.logPressed_;

  public DragHVScrollView.MoveMode nowMode => this.mode_;

  public float thresholdVertical
  {
    get => this.thresholdVertical_;
    set => this.thresholdVertical_ = Mathf.Clamp(value, 0.0f, 90f);
  }

  public UIScrollView hscroll
  {
    get => this.horizontal_;
    set
    {
      if (Object.op_Equality((Object) this.horizontal_, (Object) value))
        return;
      this.horizontal_ = value;
      this.resetWork();
      this.checkError();
    }
  }

  public UIScrollView vscroll
  {
    get => this.vertical_;
    set
    {
      if (Object.op_Equality((Object) this.vertical_, (Object) value))
        return;
      this.vertical_ = value;
      this.resetWork();
      this.checkError();
    }
  }

  public DragHVScrollView.MoveMode dirWheel
  {
    get => this.defaultWheel_;
    set => this.defaultWheel_ = value;
  }

  private void Awake()
  {
    this.checkError();
    this.resetWork();
    this.isDraggable_ = false;
    this.logPressed_ = false;
  }

  private void resetWork() => this.mode_ = DragHVScrollView.MoveMode.Neutral;

  private void checkError()
  {
    this.isError_ = Object.op_Equality((Object) this.vertical_, (Object) null) || Object.op_Equality((Object) this.horizontal_, (Object) null);
    if (!this.isError_)
      return;
    this.isDraggable_ = false;
  }

  private void OnPress(bool pressed)
  {
    if (this.isError_ || !((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject))
      return;
    DragHVScrollView.MoveMode moveMode = DragHVScrollView.MoveMode.Neutral;
    if (pressed)
    {
      bool flag1 = ((Behaviour) this.vertical_).enabled && ((Component) this.vertical_).gameObject.activeSelf;
      bool flag2 = ((Behaviour) this.horizontal_).enabled && ((Component) this.horizontal_).gameObject.activeSelf;
      this.isDraggable_ = flag1 | flag2;
      if (flag1 == !flag2)
        moveMode = this.mode_ = flag1 ? DragHVScrollView.MoveMode.Vertical : DragHVScrollView.MoveMode.Horizontal;
    }
    if (!this.isDraggable_)
      return;
    this.onPress(pressed);
    this.logPressed_ = pressed;
    this.mode_ = moveMode;
  }

  private void onPress(bool pressed)
  {
    switch (this.mode_)
    {
      case DragHVScrollView.MoveMode.Horizontal:
        this.horizontal_.Press(pressed);
        break;
      case DragHVScrollView.MoveMode.Vertical:
        this.vertical_.Press(pressed);
        break;
      default:
        this.horizontal_.Press(pressed);
        this.vertical_.Press(pressed);
        break;
    }
  }

  private void OnEnable()
  {
    if (!this.logPressed_)
      return;
    this.onPress(false);
    this.resetWork();
    this.isDraggable_ = false;
    this.logPressed_ = false;
  }

  private void OnDrag(Vector2 delta)
  {
    if (this.isError_ || !this.isDraggable_ || !NGUITools.GetActive((Behaviour) this))
      return;
    if (this.mode_ == DragHVScrollView.MoveMode.Neutral)
    {
      Vector2 normalized = ((Vector2) ref delta).normalized;
      if ((double) this.thresholdVertical_ >= (double) (Mathf.Atan2(Mathf.Abs(normalized.x), Mathf.Abs(normalized.y)) * 57.2957764f))
      {
        this.mode_ = DragHVScrollView.MoveMode.Vertical;
        this.horizontal_.Press(false);
      }
      else
      {
        this.mode_ = DragHVScrollView.MoveMode.Horizontal;
        this.vertical_.Press(false);
      }
    }
    switch (this.mode_)
    {
      case DragHVScrollView.MoveMode.Horizontal:
        this.horizontal_.Drag();
        break;
      case DragHVScrollView.MoveMode.Vertical:
        this.vertical_.Drag();
        break;
    }
  }

  private void OnScroll(float delta)
  {
    if (!NGUITools.GetActive((Behaviour) this))
      return;
    UIScrollView uiScrollView = (UIScrollView) null;
    switch (this.mode_)
    {
      case DragHVScrollView.MoveMode.Horizontal:
        uiScrollView = this.horizontal_;
        break;
      case DragHVScrollView.MoveMode.Vertical:
        uiScrollView = this.vertical_;
        break;
      default:
        switch (this.defaultWheel_)
        {
          case DragHVScrollView.MoveMode.Horizontal:
            uiScrollView = this.horizontal_;
            break;
          case DragHVScrollView.MoveMode.Vertical:
            uiScrollView = this.vertical_;
            break;
        }
        break;
    }
    if (!Object.op_Inequality((Object) uiScrollView, (Object) null))
      return;
    uiScrollView.Scroll(delta);
  }

  public enum MoveMode
  {
    Neutral,
    Horizontal,
    Vertical,
  }
}
