// Decompiled with JetBrains decompiler
// Type: IllustrationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class IllustrationController : MonoBehaviour
{
  [SerializeField]
  private GameObject targetPicture;
  [SerializeField]
  private UIScrollView scrollPicture;
  [SerializeField]
  [Tooltip("イラストの横幅")]
  private int illustWidth;
  [SerializeField]
  [Tooltip("ダブルクリックのインターバル時間")]
  private float doubleClickIntervalTime;
  [SerializeField]
  [Tooltip("ピンチインアウトする重み")]
  private float pinchiInOutWeight;
  [SerializeField]
  [Tooltip("画像の表示最小サイズ")]
  private float imageMinSize;
  [SerializeField]
  [Tooltip("画像の初期のサイズ")]
  private float imageOriginalSize;
  [SerializeField]
  [Tooltip("画像の表示最大サイズ")]
  private float imageMaxSize;
  private bool canScrollIll = true;
  private Action callback;
  private IllustrationController.PhaseMode ePhase = IllustrationController.PhaseMode.None;
  private float fitScale;
  private bool isTapStart;
  private bool isPressFrame;
  private bool isReleaseFrame;
  private bool hasFirstClick;
  private float doubleClickTimer;

  public void Init(float panel_width, Action callback = null)
  {
    this.SetCameraAllowMultiTouch(true);
    this.fitScale = panel_width / (float) this.illustWidth;
    this.InitSize();
    this.callback = callback;
    this.ePhase = IllustrationController.PhaseMode.None;
  }

  private void SetCameraAllowMultiTouch(bool isAllow)
  {
    ((Component) Singleton<CommonRoot>.GetInstance().getCamera()).GetComponent<UICamera>().allowMultiTouch = isAllow;
  }

  private void Update()
  {
    float axis = Input.GetAxis("Mouse ScrollWheel");
    if ((double) axis != 0.0 && this.canScrollIll)
      this.PinchiInOut(axis);
    if (this.isPressFrame)
      this.CheckTouch();
    if (Input.touchCount >= 2)
    {
      Touch touch1 = Input.GetTouch(0);
      Touch touch2 = Input.GetTouch(1);
      Vector2 vector2 = Vector2.op_Subtraction(Vector2.op_Subtraction(((Touch) ref touch1).position, ((Touch) ref touch1).deltaPosition), Vector2.op_Subtraction(((Touch) ref touch2).position, ((Touch) ref touch2).deltaPosition));
      double magnitude1 = (double) ((Vector2) ref vector2).magnitude;
      vector2 = Vector2.op_Subtraction(((Touch) ref touch1).position, ((Touch) ref touch2).position);
      double magnitude2 = (double) ((Vector2) ref vector2).magnitude;
      this.PinchiInOut((float) (magnitude1 - magnitude2) / -100f);
    }
    if (Input.GetMouseButtonDown(0))
    {
      if (!this.hasFirstClick)
      {
        this.hasFirstClick = true;
        this.doubleClickTimer = Time.time + this.doubleClickIntervalTime;
      }
      else if ((double) this.doubleClickTimer > (double) Time.time)
      {
        this.ActiveDoubleClick();
        this.hasFirstClick = false;
      }
    }
    if ((double) this.doubleClickTimer >= (double) Time.time)
      return;
    this.hasFirstClick = false;
  }

  private void ActiveDoubleClick() => this.InitSize();

  private void CheckTouch(bool isRelease = false)
  {
    int touchCount = Input.touchCount;
    if (this.isReleaseFrame)
      --touchCount;
    if (touchCount == 0)
    {
      if (this.ePhase == IllustrationController.PhaseMode.PinchiInOut)
      {
        ((Behaviour) this.scrollPicture).enabled = true;
        this.IsCheckRestrictWithinBounds();
      }
      this.ePhase = IllustrationController.PhaseMode.None;
    }
    if (touchCount == 1)
    {
      if (this.ePhase == IllustrationController.PhaseMode.PinchiInOut)
      {
        ((Behaviour) this.scrollPicture).enabled = true;
        this.IsCheckRestrictWithinBounds();
      }
      this.ePhase = IllustrationController.PhaseMode.Drag;
    }
    if (touchCount >= 2)
    {
      ((Behaviour) this.scrollPicture).enabled = false;
      this.ePhase = IllustrationController.PhaseMode.PinchiInOut;
    }
    this.isPressFrame = false;
    this.isReleaseFrame = false;
  }

  private void IsCheckRestrictWithinBounds()
  {
    this.scrollPicture.Press(true);
    this.scrollPicture.RestrictWithinBounds(true, true, true);
    this.scrollPicture.Press(false);
  }

  private void OnDestroy() => this.SetCameraAllowMultiTouch(false);

  private void PinchiInOut(float movement)
  {
    float delta = movement * this.pinchiInOutWeight;
    Vector3 picturePosition;
    // ISSUE: explicit constructor call
    ((Vector3) ref picturePosition).\u002Ector(this.targetPicture.transform.localPosition.x, this.targetPicture.transform.localPosition.y);
    Vector3 scrollPosition;
    // ISSUE: explicit constructor call
    ((Vector3) ref scrollPosition).\u002Ector(((Component) this.scrollPicture).transform.localPosition.x, ((Component) this.scrollPicture).transform.localPosition.y);
    Vector3 currentScale;
    // ISSUE: explicit constructor call
    ((Vector3) ref currentScale).\u002Ector(this.targetPicture.transform.localScale.x, this.targetPicture.transform.localScale.y);
    Vector3 nextScale = this.SetNextScale(this.targetPicture.transform.localScale.x, delta);
    this.targetPicture.transform.localPosition = this.SetNextPosition(picturePosition, scrollPosition, currentScale, nextScale);
    this.targetPicture.transform.localScale = nextScale;
    this.IsCheckRestrictWithinBounds();
  }

  private Vector3 SetNextPosition(
    Vector3 picturePosition,
    Vector3 scrollPosition,
    Vector3 currentScale,
    Vector3 nextScale)
  {
    return new Vector3((picturePosition.x + scrollPosition.x) / currentScale.x * nextScale.x - scrollPosition.x, (picturePosition.y + scrollPosition.y) / currentScale.y * nextScale.y - scrollPosition.y);
  }

  private Vector3 SetNextScale(float scale, float delta)
  {
    double num = (double) Mathf.Min(Mathf.Max(scale + delta, this.fitScale * this.imageMinSize), this.imageMaxSize);
    return new Vector3((float) num, (float) num);
  }

  public void InitSize()
  {
    float num = this.fitScale * this.imageOriginalSize;
    this.targetPicture.transform.localScale = new Vector3(num, num, num);
    this.targetPicture.transform.localPosition = Vector3.zero;
    ((Component) this.scrollPicture).transform.localPosition = Vector3.zero;
  }

  public void CanScrollIllustration(bool b) => this.canScrollIll = b;

  private void OnDoubleClick()
  {
  }

  private void OnClick()
  {
  }

  private void OnPress(bool isDown)
  {
    this.isPressFrame = true;
    if (isDown)
      return;
    this.isReleaseFrame = true;
  }

  private enum PhaseMode
  {
    Drag,
    PinchiInOut,
    None,
  }
}
