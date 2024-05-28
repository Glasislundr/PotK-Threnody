// Decompiled with JetBrains decompiler
// Type: Guide01142bScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;

#nullable disable
public class Guide01142bScene : NGSceneBase
{
  public Guide01142bMenu menu;

  public static void changeScene(bool stack, GearGear[] itemInfo, int index)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_4_2b", (stack ? 1 : 0) != 0, (object) itemInfo, (object) false, (object) index);
  }

  public IEnumerator onStartSceneAsync(GearGear gear, bool isDispNumber, int index)
  {
    IEnumerator e = this.menu.onStartSceneAsync(gear, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear[] gears, bool isDispNumber, int index = 0)
  {
    IEnumerator e = this.menu.onStartSceneAsync(gears, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
