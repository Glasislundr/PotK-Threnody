// Decompiled with JetBrains decompiler
// Type: ExploreClipEffectPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class ExploreClipEffectPlayer : clipEffectPlayer
{
  public bool IsFootStepSoundOnly;

  public void SetUnit(UnitUnit unit) => this._mUnit = unit;

  protected override void playSound(string seName)
  {
    ExploreSceneManager instanceOrNull = Singleton<ExploreSceneManager>.GetInstanceOrNull();
    if (Object.op_Equality((Object) instanceOrNull, (Object) null) || !instanceOrNull.IsSceneActive || this.IsFootStepSoundOnly && !seName.Contains("FOOTSTEPS"))
      return;
    base.playSound(seName);
  }
}
