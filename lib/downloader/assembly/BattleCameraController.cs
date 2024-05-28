// Decompiled with JetBrains decompiler
// Type: BattleCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class BattleCameraController : BattleMonoBehaviour
{
  protected Vector3 lookAtTarget;
  protected Vector3 lookAtVelocity = Vector3.zero;
  protected bool isTranslateSmooth;
  protected float translateSmoothTime = 0.3f;
  protected float translateSpeed = 60f;
  protected float translateSpeedByUnitMove = 20f;
  protected float dragMagnification = 1f;
  protected bool isDragNoSmooth;
  protected bool isDragStop = true;
  public float sightDistance;
  protected Vector3 sightVelocity = Vector3.zero;
  protected float sightSmoothTime = 0.3f;
  protected Transform cameraTransform;
  protected Transform lookAt;
  private BattleCameraController.CameraMoveType cameraMoveType;
  private float moveUnitTime;
  private float moveUnitTimeCounter;
  private Vector3 moveUnitStart;
  protected Transform positionOffset;
  protected Transform angle;
  protected Camera cCamera;
  private Vector3 screenPoint;

  public Camera Camera
  {
    get
    {
      this.SetCamera();
      return this.cCamera;
    }
  }

  public bool isCameraMove
  {
    get
    {
      return !Object.op_Equality((Object) this.lookAt, (Object) null) && Vector3.op_Inequality(this.lookAtTarget, this.lookAt.position);
    }
  }

  private void SetCamera()
  {
    if (!Object.op_Equality((Object) this.cCamera, (Object) null))
      return;
    Camera[] componentsInChildren = Singleton<NGBattleManager>.GetInstance().battleCamera.GetComponentsInChildren<Camera>(true);
    if (componentsInChildren == null || componentsInChildren.Length == 0)
      return;
    this.cCamera = componentsInChildren[0];
  }

  protected void initVariables()
  {
    this.SetCamera();
    this.cameraTransform = this.battleManager.battleCamera.transform;
    this.angle = this.cameraTransform.parent;
    this.positionOffset = this.angle.parent;
    this.lookAt = this.positionOffset.parent;
    this.positionOffset.localPosition = this.battleManager.cameraPositionOffsetValue;
    this.angle.localRotation = Quaternion.Euler(this.battleManager.cameraAngle, this.battleManager.cameraAngleYValue, 0.0f);
    this.lookAt.position = this.lookAtTarget;
    this.cameraTransform.localPosition = new Vector3(0.0f, 0.0f, -this.sightDistance);
  }

  protected override void Update_Battle()
  {
    if (Object.op_Equality((Object) this.cameraTransform, (Object) null))
    {
      if (Object.op_Equality((Object) this.battleManager.battleCamera, (Object) null))
        return;
      this.initVariables();
    }
    if (this.isCameraMove)
    {
      if (this.isTranslateSmooth)
        this.lookAt.position = Vector3.SmoothDamp(this.lookAt.position, this.lookAtTarget, ref this.lookAtVelocity, this.translateSmoothTime);
      else if (this.cameraMoveType == BattleCameraController.CameraMoveType.MoveUnit)
      {
        this.lookAt.position = Vector3.Lerp(this.moveUnitStart, this.lookAtTarget, this.moveUnitTimeCounter / this.moveUnitTime);
        this.moveUnitTimeCounter += Time.deltaTime;
      }
      else
        this.lookAt.position = this.cameraMoveType != BattleCameraController.CameraMoveType.BeforeMoveUnit ? Vector3.MoveTowards(this.lookAt.position, this.lookAtTarget, this.translateSpeed * Time.deltaTime) : Vector3.MoveTowards(this.lookAt.position, this.lookAtTarget, this.translateSpeedByUnitMove * Time.deltaTime);
      Vector3 vector3 = Vector3.op_Subtraction(this.lookAtTarget, this.lookAt.position);
      if ((double) ((Vector3) ref vector3).magnitude < 0.0099999997764825821)
      {
        this.lookAtTarget = this.lookAt.position;
        this.lookAtVelocity = Vector3.zero;
      }
    }
    if ((double) this.sightDistance != (double) this.cameraTransform.localPosition.z)
      this.cameraTransform.localPosition = Vector3.SmoothDamp(this.cameraTransform.localPosition, new Vector3(0.0f, 0.0f, -this.sightDistance), ref this.sightVelocity, this.sightSmoothTime);
    else
      this.sightVelocity = Vector3.zero;
    this.cameraTransform.LookAt(this.positionOffset);
  }

  public void setLookAtTarget(BL.Panel panel, bool noSmooth = false)
  {
    this.lookAtTarget = this.env.panelResource[panel].gameObject.transform.position;
    if (noSmooth)
      this.lookAt.position = this.lookAtTarget;
    this.cameraMoveType = BattleCameraController.CameraMoveType.Normal;
  }

  public void setLookAtTarget(Vector3 v, bool noSmooth = false)
  {
    this.lookAtTarget = this.env.limitFieldPosition(v);
    if (noSmooth)
      this.lookAt.position = this.lookAtTarget;
    this.cameraMoveType = BattleCameraController.CameraMoveType.Normal;
  }

  public void setBeforeMoveUnit(BL.Panel panel)
  {
    this.setLookAtTarget(panel);
    this.cameraMoveType = BattleCameraController.CameraMoveType.BeforeMoveUnit;
  }

  public void setMoveUnit(BL.Panel panel, float time)
  {
    this.setLookAtTarget(panel);
    this.cameraMoveType = BattleCameraController.CameraMoveType.MoveUnit;
    this.moveUnitTime = time;
    if ((double) this.moveUnitTime < 0.016000000759959221)
      this.moveUnitTime = 0.016f;
    this.moveUnitTimeCounter = 0.0f;
    this.moveUnitStart = this.lookAt.position;
  }

  public void onPress()
  {
    if (Object.op_Equality((Object) this.cCamera, (Object) null))
      return;
    this.screenPoint = this.cCamera.WorldToScreenPoint(this.lookAt.position);
  }

  public void onDrag(Vector2 delta)
  {
    if (Object.op_Equality((Object) this.cCamera, (Object) null))
      return;
    if (this.isTranslateSmooth)
    {
      this.screenPoint.x -= delta.x * this.dragMagnification;
      this.screenPoint.y -= delta.y * this.dragMagnification;
      this.setLookAtTarget(this.cCamera.ScreenToWorldPoint(new Vector3(this.screenPoint.x, this.screenPoint.y, this.cCamera.WorldToScreenPoint(this.lookAt.position).z)), this.isDragNoSmooth);
    }
    else
    {
      this.screenPoint.x -= delta.x * this.dragMagnification;
      this.screenPoint.y -= delta.y * this.dragMagnification;
      this.setLookAtTarget(this.cCamera.ScreenToWorldPoint(new Vector3(this.screenPoint.x, this.screenPoint.y, this.cCamera.WorldToScreenPoint(this.lookAt.position).z)), this.isDragNoSmooth);
    }
    if (!this.isDragStop)
      return;
    this.screenPoint = this.cCamera.WorldToScreenPoint(this.lookAt.position);
  }

  public void onRelease()
  {
  }

  public void onCancel()
  {
  }

  public enum CameraMoveType
  {
    Normal,
    BeforeMoveUnit,
    MoveUnit,
  }
}
