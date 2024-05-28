// Decompiled with JetBrains decompiler
// Type: Unit0543Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit0543Scene : NGSceneBase
{
  [SerializeField]
  private Unit0543Menu menu;

  public static void changeScene(bool stack, PlayerUnit unit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_3", (stack ? 1 : 0) != 0, (object) unit);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit0543Scene unit0543Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_p0_60.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0543Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(PlayerUnit unit)
  {
    IEnumerator e = this.menu.Init(unit.unit, false, unit.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
