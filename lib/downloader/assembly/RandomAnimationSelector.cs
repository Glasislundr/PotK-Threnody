// Decompiled with JetBrains decompiler
// Type: RandomAnimationSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomAnimationSelector : StateMachineBehaviour
{
  public string parameterName = "";
  public int randomMax;
  public List<string> parameterNames = new List<string>();

  public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (this.parameterNames.Count > 0)
    {
      string parameterName = this.parameterNames[Random.Range(0, this.parameterNames.Count)];
      animator.SetTrigger(parameterName);
    }
    else
    {
      if (this.parameterName == "" || this.randomMax == 0)
        return;
      int num = Random.Range(0, this.randomMax);
      animator.SetInteger(this.parameterName, num);
    }
  }
}
