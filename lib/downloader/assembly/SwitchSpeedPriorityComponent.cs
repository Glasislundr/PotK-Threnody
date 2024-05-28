// Decompiled with JetBrains decompiler
// Type: SwitchSpeedPriorityComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class SwitchSpeedPriorityComponent : MonoBehaviour
{
  public SwitchSpeedPriorityComponent.SwitchMode swtichMode;
  private bool isSpeedPriority;

  private void Awake()
  {
    this.isSpeedPriority = PerformanceConfig.GetInstance().IsSpeedPriority;
    switch (this.swtichMode)
    {
      case SwitchSpeedPriorityComponent.SwitchMode.SETENABLE:
        ((Component) this).gameObject.SetActive(!this.isSpeedPriority);
        break;
      case SwitchSpeedPriorityComponent.SwitchMode.SETDESTROY:
        if (!this.isSpeedPriority)
          break;
        Object.Destroy((Object) ((Component) this).gameObject);
        break;
    }
  }

  public enum SwitchMode
  {
    SETENABLE,
    SETDESTROY,
  }
}
