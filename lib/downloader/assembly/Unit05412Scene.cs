// Decompiled with JetBrains decompiler
// Type: Unit05412Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Unit05412Scene : NGSceneBase
{
  public Unit05412Menu menu;
  private bool isInit = true;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_12", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) Singleton<EarthDataManager>.GetInstance().GetPlayerUnits()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !x.unit.IsMaterialUnit)).ToArray<PlayerUnit>();
    Player player = SMManager.Get<Player>();
    IEnumerator e;
    if (!this.isInit)
    {
      e = this.menu.UpdateInfoAndScroll(array);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.menu.Init(player, array, true, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = false;
    }
  }
}
