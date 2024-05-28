// Decompiled with JetBrains decompiler
// Type: Unit00422Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;

#nullable disable
public class Unit00422Scene : NGSceneBase
{
  public Unit00422Menu menu;

  public static void changeScene(bool stack, PlayerUnit playerUnit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_2_2", (stack ? 1 : 0) != 0, (object) playerUnit);
  }

  public IEnumerator onStartSceneAsync(PlayerUnit playerUnit)
  {
    IEnumerator e = this.menu.Init(playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
