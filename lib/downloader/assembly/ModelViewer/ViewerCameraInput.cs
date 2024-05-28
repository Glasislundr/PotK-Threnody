// Decompiled with JetBrains decompiler
// Type: ModelViewer.ViewerCameraInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace ModelViewer
{
  public class ViewerCameraInput
  {
    private ViewerCameraInput.TouchParameter[] touchParameters = new ViewerCameraInput.TouchParameter[3];
    private int touchCount;
    private float swipeDistance;
    private float wheelDistance;
    private Vector3 cameraRotation;
    private Vector3 cameraMove;
    private float wheelBottomUpValue = 0.2f;
    public float dumpingRatio;

    public void Initialize(float dumpingRatio)
    {
      for (int index = 0; index < this.touchParameters.Length; ++index)
      {
        if (this.touchParameters[index] == null)
          this.touchParameters[index] = new ViewerCameraInput.TouchParameter();
      }
      this.ClearInputParameter();
      this.dumpingRatio = dumpingRatio;
    }

    public void UpdateParameter()
    {
      this.UpdateAlreadyInputParameter();
      this.touchCount = this.UpdateNewInputParameter();
      this.UpdateCameraMoveParameter();
      this.UpdateCameraRotateParameter();
      this.UpdateCameraSwipeParameter();
    }

    public bool IsTouchingDisplay() => this.CountTouchPoint() > 0;

    public Vector3 GetTouchPosition() => this.GetTouchPosition(0);

    public Vector3 GetCameraRotate() => this.cameraRotation;

    public float TargetDistance() => this.swipeDistance;

    public float TargetWheelDistance() => this.wheelDistance;

    public Vector3 GetCameraMove() => this.cameraMove;

    public void ClearInputParameter()
    {
      foreach (ViewerCameraInput.TouchParameter touchParameter in this.touchParameters)
        touchParameter.TouchId = -1;
      this.cameraMove = Vector3.zero;
      this.cameraRotation = Vector3.zero;
      this.swipeDistance = 0.0f;
      this.wheelDistance = 0.0f;
    }

    private void UpdateAlreadyInputParameter()
    {
      foreach (ViewerCameraInput.TouchParameter touchParameter in this.touchParameters)
      {
        if (touchParameter.TouchId >= 0)
        {
          int touchContinueIndex = this.GetTouchContinueIndex(touchParameter.TouchId);
          if (touchContinueIndex < 0)
          {
            touchParameter.TouchId = -1;
          }
          else
          {
            touchParameter.BeforeTouchPosition = touchParameter.CurrentTouchPosition;
            touchParameter.CurrentTouchPosition = this.GetTouchPosition(touchContinueIndex);
            touchParameter.IsTrigger = false;
          }
        }
      }
    }

    private int UpdateNewInputParameter()
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.touchParameters.Length; ++index2)
      {
        if (this.touchParameters[index2].TouchId >= 0)
        {
          ++index1;
        }
        else
        {
          for (int index3 = index2 + 1; index3 < this.touchParameters.Length; ++index3)
          {
            if (this.touchParameters[index3].TouchId >= 0)
            {
              this.touchParameters[index2].Copy(this.touchParameters[index3]);
              this.touchParameters[index3].TouchId = -1;
              ++index1;
              break;
            }
          }
          if (this.touchParameters[index2].TouchId < 0)
            break;
        }
      }
      int num = this.CountTouchPoint();
      for (int index4 = 0; index4 < num; ++index4)
      {
        if (index1 < this.touchParameters.Length)
        {
          int touchId = this.GetTouchId(index4);
          if (((IEnumerable<ViewerCameraInput.TouchParameter>) this.touchParameters).FirstOrDefault<ViewerCameraInput.TouchParameter>((Func<ViewerCameraInput.TouchParameter, bool>) (x => x.TouchId == touchId)) == null)
          {
            ViewerCameraInput.TouchParameter touchParameter = this.touchParameters[index1];
            touchParameter.TouchId = this.GetTouchId(index4);
            touchParameter.FirstTouchPosition = this.GetTouchPosition(index4);
            touchParameter.CurrentTouchPosition = this.GetTouchPosition(index4);
            touchParameter.BeforeTouchPosition = touchParameter.CurrentTouchPosition;
            touchParameter.IsTrigger = true;
            ++index1;
          }
        }
        else
          break;
      }
      return index1;
    }

    private void UpdateCameraMoveParameter()
    {
      this.cameraMove = Vector3.op_Multiply(this.cameraMove, Mathf.Pow(this.dumpingRatio, Time.deltaTime));
      ViewerCameraInput.TouchParameter touchParameter = (ViewerCameraInput.TouchParameter) null;
      if (this.CheckOtherThanTouchDevice())
        touchParameter = ((IEnumerable<ViewerCameraInput.TouchParameter>) this.touchParameters).FirstOrDefault<ViewerCameraInput.TouchParameter>((Func<ViewerCameraInput.TouchParameter, bool>) (x => x.TouchId == 100000));
      else if (this.touchCount >= 2 && !this.IsPinch())
        touchParameter = this.touchParameters[0];
      if (touchParameter == null)
        return;
      this.cameraMove = Vector3.op_Subtraction(UICamera.currentCamera.ScreenToWorldPoint(touchParameter.CurrentTouchPosition), UICamera.currentCamera.ScreenToWorldPoint(touchParameter.BeforeTouchPosition));
    }

    private void UpdateCameraRotateParameter()
    {
      this.cameraRotation = Vector3.op_Multiply(this.cameraRotation, Mathf.Pow(this.dumpingRatio, Time.deltaTime));
      ViewerCameraInput.TouchParameter touchParameter = (ViewerCameraInput.TouchParameter) null;
      if (this.CheckOtherThanTouchDevice())
        touchParameter = ((IEnumerable<ViewerCameraInput.TouchParameter>) this.touchParameters).FirstOrDefault<ViewerCameraInput.TouchParameter>((Func<ViewerCameraInput.TouchParameter, bool>) (x => x.TouchId == 200000));
      else if (this.touchCount == 1)
        touchParameter = this.touchParameters[0];
      if (touchParameter == null)
        return;
      this.cameraRotation.x = touchParameter.CurrentTouchPosition.y - touchParameter.BeforeTouchPosition.y;
      this.cameraRotation.y = touchParameter.CurrentTouchPosition.x - touchParameter.BeforeTouchPosition.x;
    }

    private void UpdateCameraSwipeParameter()
    {
      this.swipeDistance *= Mathf.Pow(this.dumpingRatio, Time.deltaTime);
      this.wheelDistance *= Mathf.Pow(this.dumpingRatio, Time.deltaTime);
      if (this.IsPinch())
      {
        float num = Vector3.Distance(this.touchParameters[0].BeforeTouchPosition, this.touchParameters[1].BeforeTouchPosition);
        this.swipeDistance = Vector3.Distance(this.touchParameters[0].CurrentTouchPosition, this.touchParameters[1].CurrentTouchPosition) - num;
      }
      else
      {
        float mouseWheel = this.GetMouseWheel();
        if (this.Neary0(mouseWheel))
          return;
        this.wheelDistance = mouseWheel * this.wheelBottomUpValue;
      }
    }

    private bool IsPinch()
    {
      return this.touchCount >= 2 && (double) Vector2.Dot(Vector2.op_Implicit(Vector3.op_Subtraction(this.touchParameters[0].CurrentTouchPosition, this.touchParameters[0].BeforeTouchPosition)), Vector2.op_Implicit(Vector3.op_Subtraction(this.touchParameters[1].CurrentTouchPosition, this.touchParameters[1].BeforeTouchPosition))) < 0.0;
    }

    private int GetTouchContinueIndex(int touchId)
    {
      int num = this.CountTouchPoint();
      for (int index = 0; index < num; ++index)
      {
        if (touchId == this.GetTouchId(index) && this.GetTouchState(index) != null)
          return index;
      }
      return -1;
    }

    private int CountTouchPoint()
    {
      int num = 0;
      if (this.CheckOtherThanTouchDevice())
      {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
          ++num;
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
          ++num;
      }
      else
        num = Input.touchCount;
      return num;
    }

    private void ClearInput() => Input.ResetInputAxes();

    private TouchPhase GetTouchState(int index)
    {
      if (this.CheckOtherThanTouchDevice())
      {
        if (Input.GetMouseButtonDown(0))
          return (TouchPhase) 0;
        if (Input.GetMouseButton(0))
          return (TouchPhase) 1;
        if (Input.GetMouseButtonUp(0))
          return (TouchPhase) 3;
        if (Input.GetMouseButtonDown(1))
          return (TouchPhase) 0;
        if (Input.GetMouseButton(1))
          return (TouchPhase) 1;
        return Input.GetMouseButtonUp(1) ? (TouchPhase) 3 : (TouchPhase) 4;
      }
      Touch touch = Input.GetTouch(index);
      return ((Touch) ref touch).phase;
    }

    private Vector3 GetTouchPosition(int index)
    {
      if (this.CheckOtherThanTouchDevice())
        return this.CountTouchPoint() > 0 ? Input.mousePosition : Vector3.zero;
      Touch touch = Input.GetTouch(index);
      return Vector2.op_Implicit(((Touch) ref touch).position);
    }

    private int GetTouchId(int index)
    {
      if (this.CheckOtherThanTouchDevice())
      {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
          return 200000;
        return Input.GetMouseButtonDown(1) || Input.GetMouseButton(1) || Input.GetMouseButtonUp(1) ? 100000 : -1;
      }
      Touch touch = Input.GetTouch(index);
      return ((Touch) ref touch).fingerId;
    }

    private float GetMouseWheel() => Input.GetAxis("Mouse ScrollWheel");

    private bool CheckOtherThanTouchDevice() => Input.touchCount == 0 && Input.anyKey;

    private bool Neary0(float value) => (double) Mathf.Abs(value) <= (double) Mathf.Epsilon;

    private class TouchParameter
    {
      public int TouchId;
      public Vector3 FirstTouchPosition;
      public Vector3 CurrentTouchPosition;
      public Vector3 BeforeTouchPosition;
      public bool IsTrigger;

      public void Copy(ViewerCameraInput.TouchParameter target)
      {
        this.TouchId = target.TouchId;
        this.FirstTouchPosition = target.FirstTouchPosition;
        this.CurrentTouchPosition = target.CurrentTouchPosition;
        this.BeforeTouchPosition = target.BeforeTouchPosition;
        this.IsTrigger = target.IsTrigger;
      }
    }

    private enum MouseInputAssignedTapId
    {
      MouseRight = 100000, // 0x000186A0
      MouseLeft = 200000, // 0x00030D40
    }
  }
}
