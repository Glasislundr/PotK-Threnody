// Decompiled with JetBrains decompiler
// Type: Unit00446Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;

#nullable disable
public class Unit00446Scene : NGSceneBase
{
  public Unit00446Menu menu;

  public static void changeScene(bool stack, GearGear targetgear)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4_6", (stack ? 1 : 0) != 0, (object) targetgear);
  }

  public IEnumerator onStartSceneAsync(GearGear targetgear)
  {
    IEnumerator e = this.menu.SetSprite(targetgear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
