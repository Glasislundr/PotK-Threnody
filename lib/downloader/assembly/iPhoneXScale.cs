// Decompiled with JetBrains decompiler
// Type: iPhoneXScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;
using Util;

#nullable disable
public class iPhoneXScale : MonoBehaviour
{
  [Tooltip("Is the \"localScale\" below RELATIVE to the original or an ABSOLUTE scale.")]
  public bool isRelativeScale;
  [Tooltip("New local scale or relative scale change.")]
  public Vector3 localScale = Vector3.one;
  [Tooltip("In which method should the local scale be adjusted.")]
  public iPhoneXScale.ProcessTiming processTiming = iPhoneXScale.ProcessTiming.Start;
  private Vector3? newLocalScale;

  private void Awake()
  {
    if (this.processTiming != iPhoneXScale.ProcessTiming.Awake)
      return;
    this.AdjustIPhoneXScale();
  }

  private void OnEnable()
  {
    if (this.processTiming != iPhoneXScale.ProcessTiming.Enable)
      return;
    this.AdjustIPhoneXScale();
  }

  private void Start()
  {
    if (this.processTiming != iPhoneXScale.ProcessTiming.Start)
      return;
    this.AdjustIPhoneXScale();
  }

  private void Update()
  {
    if (this.processTiming != iPhoneXScale.ProcessTiming.Update)
      return;
    this.AdjustIPhoneXScale();
  }

  private void AdjustIPhoneXScale()
  {
    if (!IOSUtil.IsDeviceGenerationiPhoneX)
      return;
    if (!this.newLocalScale.HasValue)
      this.newLocalScale = !this.isRelativeScale ? new Vector3?(this.localScale) : new Vector3?(new Vector3(((Component) this).transform.localScale.x * this.localScale.x, ((Component) this).transform.localScale.y * this.localScale.y, ((Component) this).transform.localScale.z * this.localScale.z));
    ((Component) this).transform.localScale = this.newLocalScale.Value;
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
