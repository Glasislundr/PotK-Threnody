// Decompiled with JetBrains decompiler
// Type: EffectSEExpansion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectSEExpansion : MonoBehaviour
{
  private EffectSE effectSE;

  private void Awake() => this.effectSE = ((Component) this).GetComponent<EffectSE>();

  private void OnDisable()
  {
    if (!Object.op_Implicit((Object) Singleton<NGSoundManager>.GetInstance()) || this.effectSE.UseChannel == -1)
      return;
    Singleton<NGSoundManager>.GetInstance().StopSe(this.effectSE.UseChannel);
  }
}
