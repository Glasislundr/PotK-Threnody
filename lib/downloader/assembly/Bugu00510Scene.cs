// Decompiled with JetBrains decompiler
// Type: Bugu00510Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu00510Scene : NGSceneBase
{
  private Bugu00510Menu _menu;
  private static GearGear selectedGear;
  private static List<InventoryItem> selectedItems;
  private static List<GearCombineRecipe> selectedGearRecipes;
  public static bool isInit;

  private Bugu00510Menu menu
  {
    get
    {
      if (Object.op_Equality((Object) this._menu, (Object) null))
        this._menu = this.menuBase as Bugu00510Menu;
      return this._menu;
    }
  }

  public static void changeSceneRecipe(bool isStack)
  {
    Bugu00510Scene.selectedItems = (List<InventoryItem>) null;
    Singleton<NGSceneManager>.GetInstance().clearStack("bugu005_10");
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_10", isStack);
  }

  public static void changeSceneRecipeInit()
  {
    Bugu00510Scene.selectedItems = (List<InventoryItem>) null;
    Bugu00510Scene.isInit = true;
    Singleton<NGSceneManager>.GetInstance().clearStack("bugu005_10");
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_10", false);
  }

  public static void changeSceneMaterialRecipe(
    bool isStack,
    GearGear sGear,
    List<InventoryItem> sItems,
    List<GearCombineRecipe> sGearRecipes)
  {
    Bugu00510Scene.selectedGear = sGear;
    Bugu00510Scene.selectedItems = sItems;
    Bugu00510Scene.selectedGearRecipes = sGearRecipes;
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_10", isStack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    if (Object.op_Inequality((Object) this.menu, (Object) null))
    {
      IEnumerator e = this.menu.InitAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    if (Object.op_Inequality((Object) this.menu, (Object) null))
    {
      IEnumerator e;
      if (Bugu00510Scene.selectedItems != null)
      {
        e = this.menu.StartAsync(Bugu00510Scene.selectedGear, Bugu00510Scene.selectedItems, Bugu00510Scene.selectedGearRecipes);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.menu.StartAsync(true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Bugu00510Scene.selectedItems = (List<InventoryItem>) null;
    }
  }

  public void onStartScene()
  {
  }
}
