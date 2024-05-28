// Decompiled with JetBrains decompiler
// Type: Unit05443Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;

#nullable disable
public class Unit05443Scene : NGSceneBase
{
  public Unit05443Menu menu;
  private static bool block;

  public static void changeScene(bool stack, ItemInfo choiceGear)
  {
    Unit05443Scene.block = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_4_3", (stack ? 1 : 0) != 0, (object) choiceGear);
  }

  public static void changeSceneLimited(bool stack, ItemInfo choiceGear)
  {
    Unit05443Scene.block = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_4_3", (stack ? 1 : 0) != 0, (object) choiceGear);
  }

  public IEnumerator onStartSceneAsync(ItemInfo choiceGear)
  {
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.id == choiceGear.itemID)
      {
        choiceGear = new ItemInfo(playerItem);
        break;
      }
    }
    IEnumerator e = this.menu.Init(choiceGear, Unit05443Scene.block);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
