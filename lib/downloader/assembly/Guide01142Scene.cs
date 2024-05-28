// Decompiled with JetBrains decompiler
// Type: Guide01142Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide01142Scene : NGSceneBase
{
  public Guide01142Menu menu;

  public static void changeScene(bool stack, ItemInfo itemInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_4_2", (stack ? 1 : 0) != 0, (object) itemInfo.gear, (object) itemInfo.quantity, (object) 0);
  }

  public static void changeScene(
    bool stack,
    GearGear[] itemInfo,
    int[] quantitys,
    int[] itemIDs,
    int index,
    bool WeaponMaterial = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_4_2", (stack ? 1 : 0) != 0, (object) itemInfo, (object) quantitys, (object) itemIDs, (object) false, (object) index, (object) WeaponMaterial);
  }

  public IEnumerator onStartSceneAsync(GearGear gear, bool isDispNumber, int index = 0)
  {
    IEnumerator e = this.menu.onStartSceneAsync(gear, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    GearGear[] gears,
    int[] quantitys,
    int[] itemIDs,
    bool isDispNumber,
    int index,
    bool WeaponMaterial)
  {
    Guide01142Scene guide01142Scene = this;
    IEnumerator e;
    if (WeaponMaterial)
    {
      Future<GameObject> bgF = Res.Prefabs.BackGround.DefaultBackground_storage.Load<GameObject>();
      e = bgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guide01142Scene.backgroundPrefab = bgF.Result;
      bgF = (Future<GameObject>) null;
    }
    e = guide01142Scene.menu.onStartSceneAsync(gears, quantitys, itemIDs, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear gear, int quantity, int index)
  {
    Guide01142Scene guide01142Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.DefaultBackground_storage.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide01142Scene.backgroundPrefab = bgF.Result;
    e = guide01142Scene.menu.onStartSceneAsync(gear, quantity, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator onEndSceneAsync()
  {
    this.menu.EndScene();
    yield return (object) null;
  }
}
