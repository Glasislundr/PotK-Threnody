// Decompiled with JetBrains decompiler
// Type: Bugu005RecipeCompositeMaterialSelectScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu005RecipeCompositeMaterialSelectScene : NGSceneBase
{
  [SerializeField]
  private Bugu005RecipeCompositeMaterialSelectMenu menu;

  public static void ChangeScene(
    bool stack,
    GearGear gear,
    List<ItemInfo> gears,
    List<GearCombineRecipe> allGearRecipes)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_recipe_composite_material_select", (stack ? 1 : 0) != 0, (object) gear, (object) gears, (object) allGearRecipes);
  }

  public IEnumerator onStartSceneAsync(
    GearGear gear,
    List<ItemInfo> gears,
    List<GearCombineRecipe> allGearRecipes)
  {
    this.menu.SetMainGear(gear);
    this.menu.SetAllGearRecipes(allGearRecipes);
    this.menu.SetOriginalSelectedItem(gears);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    ((Component) this).GetComponentInChildren<NGxScroll2>().scrollView.Press(false);
  }
}
