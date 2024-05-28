// Decompiled with JetBrains decompiler
// Type: Unit05468Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit05468Scene : NGSceneBase
{
  [SerializeField]
  private Unit05468Menu menu;
  private bool isInit = true;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_6_8", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) Singleton<EarthDataManager>.GetInstance().GetPlayerUnits()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !x.unit.IsMaterialUnit)).ToArray<PlayerUnit>();
    PlayerItem[] formationSupplys = ((IEnumerable<PlayerItem>) Singleton<EarthDataManager>.GetInstance().GetPlayerItems()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.supply != null)).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 2)).ToArray<PlayerItem>();
    if (this.isInit)
    {
      e = this.menu.Initialize(((IEnumerable<PlayerUnit>) array).ToArray<PlayerUnit>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = false;
    }
    e = this.menu.DispSupplyDeck(formationSupplys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
  }

  public override void onEndScene()
  {
  }
}
