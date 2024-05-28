// Decompiled with JetBrains decompiler
// Type: EffectControllerPrincessEvolutionBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EffectControllerPrincessEvolutionBase : EffectController
{
  [SerializeField]
  protected PrincessEvolutionSoundEffect soundManager;
  [SerializeField]
  protected GameObject animationRoot;

  public PrincessEvolutionSoundEffect SoundManager => this.soundManager;

  public void EndSE() => this.soundManager.OnPlayResult();

  public virtual IEnumerator Initialize(PrincesEvolutionParam param, GameObject backBtn)
  {
    yield break;
  }
}
