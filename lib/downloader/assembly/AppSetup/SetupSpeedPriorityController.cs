// Decompiled with JetBrains decompiler
// Type: AppSetup.SetupSpeedPriorityController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
namespace AppSetup
{
  public class SetupSpeedPriorityController : MonoBehaviour
  {
    [SerializeField]
    private UIButton speedButton;
    [SerializeField]
    private UIButton graphicButton;

    public void OnSpeedButton()
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      Persist.speedPriority.Data.IsSpeedPriority = true;
      Persist.speedPriority.Data.IsSpeedPrioritySetup = true;
      Persist.speedPriority.Flush();
      PerformanceConfig.GetInstance().IsSpeedPriority = true;
    }

    public void OnGraphicButton()
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      Persist.speedPriority.Data.IsSpeedPriority = false;
      Persist.speedPriority.Data.IsSpeedPrioritySetup = true;
      Persist.speedPriority.Flush();
      PerformanceConfig.GetInstance().IsSpeedPriority = false;
    }
  }
}
