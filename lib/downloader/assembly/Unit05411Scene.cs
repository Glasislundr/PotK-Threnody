// Decompiled with JetBrains decompiler
// Type: Unit05411Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using SM;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Unit05411Scene : NGSceneBase
{
  public Unit05411Menu menu;
  private bool isInit = true;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_11", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) Singleton<EarthDataManager>.GetInstance().GetPlayerUnits()).ToArray<PlayerUnit>();
    Player player = SMManager.Get<Player>();
    IEnumerator e;
    if (!this.isInit)
    {
      e = this.menu.UpdateInfoAndScroll(array, (PlayerMaterialUnit[]) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.menu.Init(player, array, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = false;
    }
  }
}
