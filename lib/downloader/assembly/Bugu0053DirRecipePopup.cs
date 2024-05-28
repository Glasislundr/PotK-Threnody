// Decompiled with JetBrains decompiler
// Type: Bugu0053DirRecipePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu0053DirRecipePopup : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScroll2 ScrollContainer;
  public bool isBackKey = true;
  private const int beforeLoadingMaxCount = 4;
  private List<GameObject> recipeObjs = new List<GameObject>();
  private Bugu0053DirRecipeListPrefabs recipeListPopupPrefabs;

  public IEnumerator Init(
    Action<GearGear, List<InventoryItem>, List<GearCombineRecipe>, int> buttonEvent,
    GameObject itemIconPrefab,
    IEnumerable<GearCombineRecipe> gearRecipes,
    Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDic,
    Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDicDescending)
  {
    Bugu0053DirRecipePopup recipePopup = this;
    Player player_ = SMManager.Get<Player>();
    recipePopup.recipeListPopupPrefabs = new Bugu0053DirRecipeListPrefabs();
    IEnumerator e = recipePopup.recipeListPopupPrefabs.GetPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<GearCombineRecipe> allRecipes = (List<GearCombineRecipe>) gearRecipes;
    recipePopup.ScrollContainer.Clear();
    int recipeInterval = 10;
    Future<GameObject> recipePrefabF = Res.Prefabs.bugu005_3.dir_RecipeList.Load<GameObject>();
    e = recipePrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject recipePrefab = recipePrefabF.Result;
    int beforeLoadingCount = 0;
    foreach (GearCombineRecipe gearRecipe in gearRecipes)
    {
      GameObject gameObject = recipePrefab.Clone();
      Bugu0053DirRecipeList component = gameObject.GetComponent<Bugu0053DirRecipeList>();
      recipePopup.recipeObjs.Add(gameObject);
      recipePopup.ScrollContainer.AddColumn1(gameObject, component.width, component.height + recipeInterval);
      if (beforeLoadingCount < 4)
      {
        e = component.Init(recipePopup, gearRecipe, allRecipes, buttonEvent, playerGearDic, playerGearDicDescending, itemIconPrefab, recipePopup.recipeListPopupPrefabs, new Action(recipePopup.IbtnClose), player_.money, 0);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        recipePopup.StartCoroutine(component.Init(recipePopup, gearRecipe, allRecipes, buttonEvent, playerGearDic, playerGearDicDescending, itemIconPrefab, recipePopup.recipeListPopupPrefabs, new Action(recipePopup.IbtnClose), player_.money, beforeLoadingCount - 4));
      ++beforeLoadingCount;
    }
    recipePopup.ScrollContainer.ResolvePosition(0, 1);
    recipePopup.ScrollContainer.scrollView.UpdateScrollbars(true);
  }

  public void IbtnClose()
  {
    if (this.IsPush)
      return;
    ((Component) this).gameObject.GetComponent<NGTweenParts>().isActive = false;
  }

  public IEnumerator BackKeyEnable()
  {
    yield return (object) null;
    this.isBackKey = true;
  }

  public override void onBackButton() => this.IbtnClose();
}
