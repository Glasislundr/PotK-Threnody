// Decompiled with JetBrains decompiler
// Type: BattleStateExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public static class BattleStateExtension
{
  public static void setState_(this BL.PhaseState self, BL.Phase state, BL env)
  {
    self.setStateWith(state, env, (Action) (() =>
    {
      if (!Object.op_Inequality((Object) Singleton<TutorialRoot>.GetInstance(), (Object) null))
        return;
      Singleton<TutorialRoot>.GetInstance().OnBattleStateChange(env);
    }));
  }
}
