// Decompiled with JetBrains decompiler
// Type: Unit0549Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit0549Scene : NGSceneBase
{
  [SerializeField]
  private Unit0549Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_9", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.Init(((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !x.unit.IsMaterialUnit)).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.EvolutionPattern.Length != 0)).ToArray<PlayerUnit>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
  }
}
