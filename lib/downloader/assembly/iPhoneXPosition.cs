// Decompiled with JetBrains decompiler
// Type: iPhoneXPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;
using Util;

#nullable disable
public class iPhoneXPosition : MonoBehaviour
{
  [Tooltip("Is the \"localPosition\" below a RELATIVE position (offset to original position) or an ABSOLUTE position.")]
  public bool isRelativePosition;
  [Tooltip("New position or relative offset.")]
  public Vector3 localPosition;
  [Tooltip("In which method should the local position be adjusted.")]
  public iPhoneXPosition.ProcessTiming processTiming = iPhoneXPosition.ProcessTiming.Start;
  private Vector3? newLocalPosition;

  private void Awake()
  {
    if (this.processTiming != iPhoneXPosition.ProcessTiming.Awake)
      return;
    this.AdjustIPhoneXPosition();
  }

  private void OnEnable()
  {
    if (this.processTiming != iPhoneXPosition.ProcessTiming.Enable)
      return;
    this.AdjustIPhoneXPosition();
  }

  private void Start()
  {
    if (this.processTiming != iPhoneXPosition.ProcessTiming.Start)
      return;
    this.AdjustIPhoneXPosition();
  }

  private void Update()
  {
    if (this.processTiming != iPhoneXPosition.ProcessTiming.Update)
      return;
    this.AdjustIPhoneXPosition();
  }

  private void AdjustIPhoneXPosition()
  {
    if (!IOSUtil.IsDeviceGenerationiPhoneX)
      return;
    if (!this.newLocalPosition.HasValue)
      this.newLocalPosition = !this.isRelativePosition ? new Vector3?(this.localPosition) : new Vector3?(Vector3.op_Addition(((Component) this).transform.localPosition, this.localPosition));
    ((Component) this).transform.localPosition = this.newLocalPosition.Value;
  }

  public enum ProcessTiming
  {
    None,
    Awake,
    Enable,
    Start,
    Update,
  }
}
