// Decompiled with JetBrains decompiler
// Type: UIGestureSwipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (BoxCollider))]
[AddComponentMenu("Utility/Behaviour/UIGestureSwipe")]
public class UIGestureSwipe : MonoBehaviour
{
  [Tooltip("On Moved* を呼ぶ移動量")]
  public float threshold_;
  public List<EventDelegate> onMovedLeft_;
  public List<EventDelegate> onMovedRight_;
  private bool isPressed_;
  private Vector2 startPoint_;
  private BoxCollider collider_;
  private Camera uiCamera_;
  private bool isMouseControl_;

  private BoxCollider holdArea
  {
    get
    {
      return !Object.op_Implicit((Object) this.collider_) ? (this.collider_ = ((Component) this).GetComponent<BoxCollider>()) : this.collider_;
    }
  }

  private Camera uiCamera
  {
    get
    {
      return !Object.op_Inequality((Object) this.uiCamera_, (Object) null) ? (this.uiCamera_ = UICamera.mainCamera) : this.uiCamera_;
    }
  }

  private void OnPress(bool isPress)
  {
    this.isPressed_ = isPress;
    if ((double) this.threshold_ <= 0.0 || !isPress || !this.onMovedLeft_.Any<EventDelegate>() || !this.onMovedRight_.Any<EventDelegate>() || this.checkTouch() != 1)
      return;
    this.StartCoroutine(this.doWatchGesture());
  }

  private IEnumerator doWatchGesture()
  {
    this.startPoint_ = this.getTouchPosition();
    while (this.isPressed_ && this.checkTouch() == 1)
    {
      Vector2 touchPosition = this.getTouchPosition();
      Vector3 worldPoint = this.uiCamera.ScreenToWorldPoint(Vector2.op_Implicit(touchPosition));
      Bounds bounds = ((Collider) this.holdArea).bounds;
      if (!((Bounds) ref bounds).Contains(worldPoint))
        break;
      Vector2 vector2 = Vector2.op_Subtraction(touchPosition, this.startPoint_);
      if ((double) Mathf.Abs(vector2.x) >= (double) this.threshold_)
      {
        if ((double) vector2.x < 0.0)
        {
          EventDelegate.Execute(this.onMovedLeft_);
          break;
        }
        EventDelegate.Execute(this.onMovedRight_);
        break;
      }
      yield return (object) null;
    }
  }

  private int checkTouch()
  {
    int num = 0;
    this.isMouseControl_ = false;
    if (Input.touchCount == 0)
    {
      if (Input.anyKey)
      {
        int[] numArray = new int[1];
        for (int index = 0; index < numArray.Length; ++index)
        {
          if (Input.GetMouseButtonDown(numArray[index]) || Input.GetMouseButton(numArray[index]))
            ++num;
        }
      }
      if (num > 0)
        this.isMouseControl_ = true;
    }
    else
      num = Input.touchCount;
    return num;
  }

  private Vector2 getTouchPosition()
  {
    if (this.isMouseControl_)
      return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Touch touch = Input.GetTouch(0);
    return ((Touch) ref touch).position;
  }
}
