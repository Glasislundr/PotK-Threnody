// Decompiled with JetBrains decompiler
// Type: DailyMission0271BreakAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DailyMission0271BreakAnimation : MonoBehaviour
{
  public void onBreakPanel() => DailyMission0271PanelRoot.isBreakPanel = true;

  public void onEnd() => DailyMission0271PanelRoot.isAnimationEnd = true;
}
