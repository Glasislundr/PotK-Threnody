// Decompiled with JetBrains decompiler
// Type: Guide01142cScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;

#nullable disable
public class Guide01142cScene : NGSceneBase
{
  public Guide01142cMenu menu;

  public IEnumerator onStartSceneAsync(ItemInfo item, bool isDispNumber, int index)
  {
    this.menu.SetNumber(item, isDispNumber);
    IEnumerator e = this.menu.onStartSceneAsync(item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(ItemInfo[] items, bool isDispNumber, int index = 0)
  {
    this.menu.SetNumber(items[0], isDispNumber);
    IEnumerator e = this.menu.onStartSceneAsync(items[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear gear, bool isDispNumber, int index)
  {
    this.menu.SetNumber(gear, isDispNumber);
    IEnumerator e = this.menu.onStartSceneAsync(gear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear[] gears, bool isDispNumber, int index = 0)
  {
    this.menu.SetNumber(gears[0], isDispNumber);
    IEnumerator e = this.menu.onStartSceneAsync(gears[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
