// Decompiled with JetBrains decompiler
// Type: Unit05499Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit05499Scene : NGSceneBase
{
  [SerializeField]
  private Unit05499Menu menu;

  public static void ChangeScene(bool stack, PlayerUnit selectUnit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_9_9", (stack ? 1 : 0) != 0, (object) selectUnit);
  }

  public IEnumerator onStartSceneAsync(PlayerUnit selectUnit)
  {
    int[] array = ((IEnumerable<UnitEvolutionPattern>) selectUnit.unit.EvolutionPattern).Select<UnitEvolutionPattern, int>((Func<UnitEvolutionPattern, int>) (x => x.ID)).ToArray<int>();
    PlayerUnit[] evolutionPattern = Singleton<EarthDataManager>.GetInstance().GetEvolutionPattern(selectUnit.id, array);
    IEnumerator e = this.menu.Initialize(selectUnit, evolutionPattern);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
