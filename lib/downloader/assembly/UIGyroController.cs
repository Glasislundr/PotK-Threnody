// Decompiled with JetBrains decompiler
// Type: UIGyroController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIGyroController : MonoBehaviour
{
  private static List<UIGyroController> controllerList = new List<UIGyroController>();
  [SerializeField]
  private bool isControllerStarted;
  public float leftMax;
  public float rightMax;
  public float bottomMax;
  public float topMax;
  public Animator stopAnimator;
  public List<UIGyroController.GyroControlledTransform> gyroControlledTransforms = new List<UIGyroController.GyroControlledTransform>();
  public List<GyroControlledObject> gyroControlledObjects = new List<GyroControlledObject>();
  private bool isGyroAvailable;
  private float currentRotationRateX;
  private float currentRotationRateY;
  private float? tX;
  private float? tY;

  public static void StartAllUIGyroControllers()
  {
    foreach (UIGyroController controller in UIGyroController.controllerList)
      controller.StartController();
  }

  public static void StopAllUIGyroControllers()
  {
    foreach (UIGyroController controller in UIGyroController.controllerList)
      controller.StopController();
  }

  private void Start()
  {
    this.isGyroAvailable = SystemInfo.supportsGyroscope;
    if (!this.isGyroAvailable)
      return;
    Input.gyro.enabled = true;
    UIGyroController.controllerList.Add(this);
  }

  private void Update()
  {
    if (!this.isGyroAvailable || !this.isControllerStarted)
      return;
    float num1 = Time.deltaTime * 180f;
    this.currentRotationRateX = Mathf.Clamp(this.currentRotationRateX + Input.gyro.rotationRateUnbiased.x * num1, this.bottomMax, this.topMax);
    this.currentRotationRateY = Mathf.Clamp(this.currentRotationRateY + Input.gyro.rotationRateUnbiased.y * num1, this.leftMax, this.rightMax);
    this.tX = new float?();
    if ((double) this.topMax != (double) this.bottomMax)
      this.tX = new float?((float) (((double) this.currentRotationRateX - (double) this.bottomMax) / ((double) this.topMax - (double) this.bottomMax)));
    this.tY = new float?();
    if ((double) this.rightMax != (double) this.leftMax)
      this.tY = new float?((float) (((double) this.currentRotationRateY - (double) this.leftMax) / ((double) this.rightMax - (double) this.leftMax)));
    int count1 = this.gyroControlledTransforms.Count;
    for (int index = 0; index < count1; ++index)
    {
      UIGyroController.GyroControlledTransform controlledTransform = this.gyroControlledTransforms[index];
      if (Object.op_Inequality((Object) controlledTransform.targetTransform, (Object) null))
      {
        float num2 = controlledTransform.originalLocalPosition.x;
        if (this.tY.HasValue)
          num2 = Mathf.Lerp(controlledTransform.originalLocalPosition.x + controlledTransform.xMin, controlledTransform.originalLocalPosition.x + controlledTransform.xMax, this.tY.Value);
        float num3 = controlledTransform.originalLocalPosition.y;
        if (this.tX.HasValue)
          num3 = Mathf.Lerp(controlledTransform.originalLocalPosition.y + controlledTransform.yMax, controlledTransform.originalLocalPosition.y + controlledTransform.yMin, this.tX.Value);
        controlledTransform.targetTransform.localPosition = new Vector3(num2, num3, controlledTransform.targetTransform.localPosition.z);
      }
    }
    int count2 = this.gyroControlledObjects.Count;
    for (int index = 0; index < count2; ++index)
      this.gyroControlledObjects[index].OnLerp(this.tX, this.tY);
  }

  private void OnDestroy()
  {
    this.StopController();
    UIGyroController.controllerList.Remove(this);
  }

  public void StartController()
  {
    if (Object.op_Inequality((Object) this.stopAnimator, (Object) null))
      ((Behaviour) this.stopAnimator).enabled = false;
    this.isControllerStarted = true;
    this.currentRotationRateX = 0.0f;
    this.currentRotationRateY = 0.0f;
    this.InitializeControlledTargets();
  }

  private void InitializeControlledTargets()
  {
    foreach (UIGyroController.GyroControlledTransform controlledTransform in this.gyroControlledTransforms)
      controlledTransform.originalLocalPosition = controlledTransform.targetTransform.localPosition;
    foreach (GyroControlledObject controlledObject in this.gyroControlledObjects)
      controlledObject.OnControllerStart();
  }

  public void StopController()
  {
    this.isControllerStarted = false;
    this.ResetControlledTargets();
  }

  private void ResetControlledTargets()
  {
    foreach (UIGyroController.GyroControlledTransform controlledTransform in this.gyroControlledTransforms)
      controlledTransform.targetTransform.localPosition = controlledTransform.originalLocalPosition;
    foreach (GyroControlledObject controlledObject in this.gyroControlledObjects)
      controlledObject.OnControllerStop();
  }

  [Serializable]
  public class GyroControlledTransform
  {
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public Transform targetTransform;
    public Vector3 originalLocalPosition;
  }
}
