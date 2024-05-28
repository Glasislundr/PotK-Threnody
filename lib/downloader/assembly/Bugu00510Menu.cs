// Decompiled with JetBrains decompiler
// Type: Bugu00510Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu00510Menu : BackButtonMenuBase
{
  [SerializeField]
  private UIScrollView ScrollView;
  [SerializeField]
  private NGxScroll2 scroll;
  [SerializeField]
  private GameObject[] SortLabel;
  [SerializeField]
  private GameObject recipeRoot;
  private List<RecipeData> recipeDataList;
  private List<RecipeData> displayDataList;
  private List<ItemIcon> allItemIcon = new List<ItemIcon>();
  private float scroolStartY;
  private bool isInitialize;
  private GameObject recipePopupPrefab;
  private GameObject kakuninnPopupPrefab;
  private GameObject itemIconPrefab;
  [HideInInspector]
  public Bugu0053DirRecipePopup dirRecipe;
  private List<InventoryItem> inventoryItems = new List<InventoryItem>();
  private Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDic = new Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>>();
  private Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDicDescending = new Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>>();
  private RecipeData currentRecipe;
  private bool isIntegrated;
  private List<int> recipeGearIDList = new List<int>();
  private GameObject sortPopupPrefab;

  public IEnumerator InitAsync()
  {
    this.isInitialize = false;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemIconPrefab = prefabF.Result;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_005_recipe_composite.Load<GameObject>();
    e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.kakuninnPopupPrefab = popupPrefabF.Result;
    prefabF = Res.Prefabs.bugu005_3.dir_Recipe.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.recipePopupPrefab = prefabF.Result;
    this.dirRecipe = this.recipePopupPrefab.CloneAndGetComponent<Bugu0053DirRecipePopup>(this.recipeRoot);
    ((Component) this.dirRecipe).gameObject.GetComponent<NGTweenParts>().isActive = false;
    this.allItemIcon.Clear();
    this.recipeGearIDList.Clear();
    this.isIntegrated = false;
    this.currentRecipe = (RecipeData) null;
    e = this.LoadSortPopupPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadSortPopupPrefab()
  {
    if (!Object.op_Implicit((Object) this.sortPopupPrefab))
    {
      Future<GameObject> sortPopupPrefabF = new ResourceObject("Prefabs/bugu005_10/popup_Recipe_Sort__anim_popup01").Load<GameObject>();
      if (sortPopupPrefabF != null)
      {
        IEnumerator e = sortPopupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.sortPopupPrefab = sortPopupPrefabF.Result;
      }
    }
  }

  public IEnumerator StartAsync(bool isForceSetup = false)
  {
    Bugu00510Menu bugu00510Menu = this;
    if (Bugu00510Scene.isInit)
    {
      bugu00510Menu.currentRecipe = (RecipeData) null;
      ((Component) bugu00510Menu.dirRecipe).gameObject.GetComponent<NGTweenParts>().isActive = false;
      Bugu00510Scene.isInit = false;
    }
    if (bugu00510Menu.currentRecipe == null || isForceSetup || bugu00510Menu.isIntegrated)
    {
      if (bugu00510Menu.currentRecipe == null)
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      IEnumerator e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      List<GameCore.ItemInfo> itemInfoList = new List<GameCore.ItemInfo>();
      ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).ForEach<PlayerItem>((Action<PlayerItem>) (x => itemInfoList.Add(new GameCore.ItemInfo(x))));
      ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).ForEach<PlayerMaterialGear>((Action<PlayerMaterialGear>) (x => itemInfoList.Add(new GameCore.ItemInfo(x))));
      bugu00510Menu.playerGearDic.Clear();
      bugu00510Menu.playerGearDicDescending.Clear();
      foreach (IGrouping<int, GameCore.ItemInfo> source in itemInfoList.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.isWeapon || x.isCompse || x.isWeaponMaterial)).GroupBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.gear.group_id)))
      {
        List<GameCore.ItemInfo> list1 = source.OrderBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => this.CheckCanMaterial(x))).ThenBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => !x.isDisappearItem ? int.MaxValue : x.gearAccessoryRemainingAmount)).ThenBy<GameCore.ItemInfo, bool>((Func<GameCore.ItemInfo, bool>) (x => !x.isWeaponMaterial)).ThenBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.gearLevel)).ThenBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.gear.ID)).ToList<GameCore.ItemInfo>();
        int num1 = list1.Sum<GameCore.ItemInfo>((Func<GameCore.ItemInfo, int>) (x => x.quantity));
        bugu00510Menu.playerGearDic[source.Key] = new Tuple<List<GameCore.ItemInfo>, int>(list1, num1);
        List<GameCore.ItemInfo> list2 = source.OrderBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => this.CheckCanMaterial(x))).ThenBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => !x.isDisappearItem ? int.MaxValue : x.gearAccessoryRemainingAmount)).ThenBy<GameCore.ItemInfo, bool>((Func<GameCore.ItemInfo, bool>) (x => !x.isWeaponMaterial)).ThenByDescending<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.gearLevel)).ThenBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.gear.ID)).ThenBy<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.gearLevelUnLimit)).ToList<GameCore.ItemInfo>();
        int num2 = list2.Sum<GameCore.ItemInfo>((Func<GameCore.ItemInfo, int>) (x => x.quantity));
        bugu00510Menu.playerGearDicDescending[source.Key] = new Tuple<List<GameCore.ItemInfo>, int>(list2, num2);
      }
      bugu00510Menu.SetNewInventory();
      List<Tuple<int, GameCore.ItemInfo, int>> playerItemData = bugu00510Menu.playerGearDic.SelectMany<KeyValuePair<int, Tuple<List<GameCore.ItemInfo>, int>>, Tuple<int, GameCore.ItemInfo, int>>((Func<KeyValuePair<int, Tuple<List<GameCore.ItemInfo>, int>>, IEnumerable<Tuple<int, GameCore.ItemInfo, int>>>) (x => x.Value.Item1.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (y => !y.broken && !y.favorite && !y.ForBattle)).Select<GameCore.ItemInfo, Tuple<int, GameCore.ItemInfo, int>>((Func<GameCore.ItemInfo, Tuple<int, GameCore.ItemInfo, int>>) (y => new Tuple<int, GameCore.ItemInfo, int>(x.Key, y, x.Value.Item2))))).OrderBy<Tuple<int, GameCore.ItemInfo, int>, int>((Func<Tuple<int, GameCore.ItemInfo, int>, int>) (x => x.Item1)).ThenBy<Tuple<int, GameCore.ItemInfo, int>, int>((Func<Tuple<int, GameCore.ItemInfo, int>, int>) (x => x.Item2.gearLevel)).ToList<Tuple<int, GameCore.ItemInfo, int>>();
      bugu00510Menu.recipeDataList = new List<RecipeData>();
      ((IEnumerable<GearCombineRecipe>) MasterData.GearCombineRecipeList).Where<GearCombineRecipe>((Func<GearCombineRecipe, bool>) (x =>
      {
        if (x.start_at.HasValue)
        {
          DateTime? startAt = x.start_at;
          DateTime dateTime = ServerTime.NowAppTime();
          if ((startAt.HasValue ? (startAt.GetValueOrDefault() <= dateTime ? 1 : 0) : 0) == 0)
            goto label_5;
        }
        if (x.end_at.HasValue)
        {
          DateTime? endAt = x.end_at;
          DateTime dateTime = ServerTime.NowAppTime();
          if ((endAt.HasValue ? (endAt.GetValueOrDefault() >= dateTime ? 1 : 0) : 0) == 0)
            goto label_5;
        }
        return MasterData.GearGear.ContainsKey(x.combined_gear_id);
label_5:
        return false;
      })).GroupBy<GearCombineRecipe, int>((Func<GearCombineRecipe, int>) (x => MasterData.GearGear[x.combined_gear_id].group_id)).ForEach<IGrouping<int, GearCombineRecipe>>((Action<IGrouping<int, GearCombineRecipe>>) (x => this.recipeDataList.Add(new RecipeData((IEnumerable<GearCombineRecipe>) x, x.Any<GearCombineRecipe>((Func<GearCombineRecipe, bool>) (y => this.CheckEnableRecipe(playerItemData, y)))))));
      float timer = Time.realtimeSinceStartup;
      List<string> recipePaths = new List<string>();
      foreach (RecipeData recipeData in bugu00510Menu.recipeDataList)
      {
        RecipeData recipe = recipeData;
        if (!bugu00510Menu.recipeGearIDList.Any<int>((Func<int, bool>) (x => x == recipe.combinedGear.ID)))
        {
          recipePaths.AddRange((IEnumerable<string>) recipe.GetResouceNames());
          bugu00510Menu.recipeGearIDList.Add(recipe.combinedGear.ID);
          if ((double) Time.realtimeSinceStartup - (double) timer > 0.20000000298023224)
          {
            timer = Time.realtimeSinceStartup;
            yield return (object) null;
          }
        }
      }
      yield return (object) null;
      e = OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) recipePaths, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      for (int count = bugu00510Menu.allItemIcon.Count; count < Mathf.Min(ItemIcon.MaxValue, bugu00510Menu.recipeDataList.Count); ++count)
      {
        ItemIcon component = bugu00510Menu.itemIconPrefab.Clone().GetComponent<ItemIcon>();
        bugu00510Menu.allItemIcon.Add(component);
        ((Component) component).gameObject.SetActive(false);
      }
      bugu00510Menu.UpdateSortLabel();
      bugu00510Menu.SortFilter();
      e = bugu00510Menu.CreateAllIcon(bugu00510Menu.displayDataList);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00510Menu.SetScrollItem(bugu00510Menu.displayDataList);
      bugu00510Menu.scroolStartY = ((Component) bugu00510Menu.scroll.scrollView).transform.localPosition.y;
      if (bugu00510Menu.currentRecipe != null)
      {
        int index = bugu00510Menu.displayDataList.FindIndex((Predicate<RecipeData>) (x => x.combinedGear.ID == this.currentRecipe.combinedGear.ID));
        bugu00510Menu.scroll.ResolvePosition(index, bugu00510Menu.displayDataList.Count);
      }
      bugu00510Menu.StartCoroutine(bugu00510Menu.LoadObject());
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      bugu00510Menu.isIntegrated = false;
      ItemIcon.IsPoolCache = true;
      bugu00510Menu.isInitialize = true;
    }
  }

  public IEnumerator StartAsync(
    GearGear sGear,
    List<InventoryItem> selectedItems,
    List<GearCombineRecipe> sGearRecipes)
  {
    IEnumerator e = this.StartAsync(true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<GearGear> gears = new List<GearGear>();
    foreach (InventoryItem selectedItem in selectedItems)
      gears.Add(selectedItem.Item.gear);
    this.PushOK(sGear, selectedItems, sGearRecipes, this.SetZenie((IEnumerable<GearGear>) gears));
  }

  private bool CheckEnableRecipe(
    List<Tuple<int, GameCore.ItemInfo, int>> playerItemData,
    GearCombineRecipe recipe)
  {
    List<Tuple<int, int>> source = new List<Tuple<int, int>>();
    source.Add(new Tuple<int, int>(recipe.material1_gear_id, recipe.material1_gear_rank.HasValue ? recipe.material1_gear_rank.Value : 0));
    if (recipe.material2_gear_id.HasValue)
      source.Add(new Tuple<int, int>(recipe.material2_gear_id.Value, recipe.material2_gear_rank.HasValue ? recipe.material2_gear_rank.Value : 0));
    if (recipe.material3_gear_id.HasValue)
      source.Add(new Tuple<int, int>(recipe.material3_gear_id.Value, recipe.material3_gear_rank.HasValue ? recipe.material3_gear_rank.Value : 0));
    if (recipe.material4_gear_id.HasValue)
      source.Add(new Tuple<int, int>(recipe.material4_gear_id.Value, recipe.material4_gear_rank.HasValue ? recipe.material4_gear_rank.Value : 0));
    if (recipe.material5_gear_id.HasValue)
      source.Add(new Tuple<int, int>(recipe.material5_gear_id.Value, recipe.material5_gear_rank.HasValue ? recipe.material5_gear_rank.Value : 0));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    List<GameCore.ItemInfo> usebleItemInfoList = new List<GameCore.ItemInfo>();
    foreach (Tuple<int, int> tuple1 in (IEnumerable<Tuple<int, int>>) source.OrderBy<Tuple<int, int>, int>((Func<Tuple<int, int>, int>) (x => x.Item1)).ThenBy<Tuple<int, int>, int>((Func<Tuple<int, int>, int>) (x => x.Item2)))
    {
      Tuple<int, int> part = tuple1;
      Tuple<int, GameCore.ItemInfo, int> tuple2 = playerItemData.Find((Predicate<Tuple<int, GameCore.ItemInfo, int>>) (x => x.Item1 == part.Item1 && x.Item2.gearLevel >= part.Item2 && !usebleItemInfoList.Any<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (y => x.Item2 == y))));
      if (tuple2 == null)
        return false;
      if (MasterData.GearGear[tuple2.Item2.masterID].isMaterial() || tuple2.Item2.isWeaponMaterial)
      {
        if (!dictionary.ContainsKey(part.Item1))
          dictionary[part.Item1] = 1;
        else
          dictionary[part.Item1]++;
        if (dictionary[part.Item1] > tuple2.Item3)
          return false;
      }
      else
        usebleItemInfoList.Add(tuple2.Item2);
    }
    return SMManager.Get<Player>().CheckZeny(this.SetZenie(source.Select<Tuple<int, int>, GearGear>((Func<Tuple<int, int>, GearGear>) (x => ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (gear => gear.group_id == x.Item1))))));
  }

  public int SetZenie(IEnumerable<GearGear> gears)
  {
    int total_item_level = 0;
    int total_item_rarity = 0;
    int cnt_use_gears = 0;
    gears.ForEach<GearGear>((Action<GearGear>) (item =>
    {
      if (item == null)
        return;
      total_item_level += item.compose_level;
      total_item_rarity += item.rarity.index;
      ++cnt_use_gears;
    }));
    if (cnt_use_gears < 1)
      cnt_use_gears = 1;
    int index = total_item_rarity / cnt_use_gears - 1;
    if (index < 0)
      index = 0;
    NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
    return (int) ((boostInfo == null ? 1.0M : boostInfo.DiscountGearCombine) * (Decimal) total_item_level * 50M * (Decimal) GearRarity.ComposeRatio(index));
  }

  private int CheckCanMaterial(GameCore.ItemInfo gear)
  {
    int num = 0;
    if (gear.broken)
      ++num;
    if (gear.favorite)
      ++num;
    if (gear.ForBattle)
      ++num;
    return num;
  }

  private List<RecipeData> Filter(List<RecipeData> recipes, List<bool> filter)
  {
    return recipes.FilterBy(filter.ToArray()).ToList<RecipeData>();
  }

  private List<RecipeData> Sort(List<RecipeData> recipes, Persist.RecipeSortAndFilterInfo info)
  {
    return recipes.SortBy(info.sortType, info.order, info.isRecipeExist).ToList<RecipeData>();
  }

  private void SetScrollItem(List<RecipeData> recipes)
  {
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, recipes.Count); ++index)
    {
      ItemIcon itemIcon = this.allItemIcon[index];
      this.scroll.Add(((Component) itemIcon).gameObject, ItemIcon.Width, ItemIcon.Height);
      ((Component) itemIcon).gameObject.SetActive(true);
    }
    this.scroll.CreateScrollPoint(ItemIcon.Height, recipes.Count);
    this.scroll.ResolvePosition();
  }

  private IEnumerator CreateAllIcon(List<RecipeData> recipes)
  {
    Bugu00510Menu bugu00510Menu = this;
    for (int i = 0; i < Mathf.Min(bugu00510Menu.allItemIcon.Count, recipes.Count); ++i)
    {
      IEnumerator e = bugu00510Menu.allItemIcon[i].InitByGear(recipes[i].combinedGear, recipes[i].combinedGear.GetElement());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      recipes[i].icon = bugu00510Menu.allItemIcon[i];
      bugu00510Menu.allItemIcon[i].Gray = !recipes[i].isCombinEnable;
      bugu00510Menu.allItemIcon[i].onClick = new Action<ItemIcon>(bugu00510Menu.IconClicked);
      bugu00510Menu.allItemIcon[i].EnableLongPressEvent(true, new Action<ItemIcon>(bugu00510Menu.GearDetail));
    }
  }

  private void IconClicked(ItemIcon itemIcon)
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.OpenRecipePopup(this.recipeDataList.FirstOrDefault<RecipeData>((Func<RecipeData, bool>) (x => Object.op_Equality((Object) x.icon, (Object) itemIcon)))));
  }

  private void GearDetail(ItemIcon itemIcon)
  {
    RecipeData recipe = this.recipeDataList.FirstOrDefault<RecipeData>((Func<RecipeData, bool>) (x => Object.op_Equality((Object) x.icon, (Object) itemIcon)));
    if (recipe.combinedGear.kind.Enum != GearKindEnum.smith)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      GearGear[] array = this.displayDataList.Where<RecipeData>((Func<RecipeData, bool>) (x => x.combinedGear.kind.Enum != GearKindEnum.smith)).Select<RecipeData, GearGear>((Func<RecipeData, GearGear>) (x => x.combinedGear)).ToArray<GearGear>();
      int index = Array.FindIndex<GearGear>(array, (Predicate<GearGear>) (x => x == recipe.combinedGear));
      Guide01142Scene.changeScene(true, array, (int[]) null, (int[]) null, index);
    }
    else if (recipe.combinedGear.compose_kind.kind.Enum != GearKindEnum.smith)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      GearGear[] array = this.displayDataList.Where<RecipeData>((Func<RecipeData, bool>) (x => x.combinedGear.compose_kind.kind.Enum != GearKindEnum.smith && x.combinedGear.kind.Enum == GearKindEnum.smith)).Select<RecipeData, GearGear>((Func<RecipeData, GearGear>) (x => x.combinedGear)).ToArray<GearGear>();
      int index = Array.FindIndex<GearGear>(array, (Predicate<GearGear>) (x => x == recipe.combinedGear));
      Guide01142bScene.changeScene(true, array, index);
    }
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("guide011_4_2c", true, (object) itemIcon.ItemInfo, (object) false, (object) 0);
    this.currentRecipe = recipe;
  }

  private IEnumerator LoadObject()
  {
    if (this.recipeDataList.Count > ItemIcon.MaxValue)
    {
      for (int i = ItemIcon.MaxValue; i < this.recipeDataList.Count; ++i)
      {
        IEnumerator e = ItemIcon.LoadSprite(this.recipeDataList[i].combinedGear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private void ScrollIconUpdate(List<RecipeData> recipes, int recipeIndex, int iconCount)
  {
    if (recipes[recipeIndex] != null && Object.op_Equality((Object) recipes[recipeIndex].icon, (Object) this.allItemIcon[iconCount]))
      return;
    recipes.Where<RecipeData>((Func<RecipeData, bool>) (a => Object.op_Equality((Object) a.icon, (Object) this.allItemIcon[iconCount]))).ForEach<RecipeData>((Action<RecipeData>) (b => b.icon = (ItemIcon) null));
    recipes[recipeIndex].icon = this.allItemIcon[iconCount];
    this.StartCoroutine(this.allItemIcon[iconCount].InitByGear(recipes[recipeIndex].combinedGear, recipes[recipeIndex].combinedGear.GetElement()));
    this.allItemIcon[iconCount].Gray = !recipes[recipeIndex].isCombinEnable;
  }

  private IEnumerator OpenRecipePopup(RecipeData recipe)
  {
    Bugu00510Menu bugu00510Menu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    ((Component) bugu00510Menu.dirRecipe).gameObject.SetActive(true);
    ((Behaviour) ((Component) bugu00510Menu.dirRecipe).gameObject.GetComponent<TweenAlpha>()).enabled = false;
    ((UIRect) ((Component) bugu00510Menu.dirRecipe).gameObject.GetComponent<UIWidget>()).alpha = 0.0f;
    IEnumerator e = bugu00510Menu.dirRecipe.Init(new Action<GearGear, List<InventoryItem>, List<GearCombineRecipe>, int>(bugu00510Menu.PushOK), bugu00510Menu.itemIconPrefab, (IEnumerable<GearCombineRecipe>) recipe.recipes, bugu00510Menu.playerGearDic, bugu00510Menu.playerGearDicDescending);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) bugu00510Menu.dirRecipe).gameObject.GetComponent<NGTweenParts>().isActive = true;
    bugu00510Menu.currentRecipe = recipe;
    yield return (object) new WaitForSeconds(0.2f);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void IbtnSort()
  {
    if (this.IsPush)
      return;
    this.ShowSortAndFilterPopup();
  }

  private void ShowSortAndFilterPopup()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen || Object.op_Equality((Object) this.sortPopupPrefab, (Object) null))
      return;
    GameObject prefab = this.sortPopupPrefab.Clone();
    RecipeSortAndFilter sortAndFilter = prefab.GetComponent<RecipeSortAndFilter>();
    Action<List<bool>> onUpdateFilter = (Action<List<bool>>) (f => sortAndFilter.SetItemNum(this.recipeDataList.FilterBy(f.ToArray()).Count<RecipeData>(), this.recipeDataList.Count));
    Action<Persist.RecipeSortAndFilterInfo> onDicision = (Action<Persist.RecipeSortAndFilterInfo>) (info =>
    {
      Persist.bugu00510SortAndFilter.Data = info;
      Persist.bugu00510SortAndFilter.Flush();
      this.UpdateSortLabel();
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      this.StartCoroutine("SortIcon");
      Singleton<PopupManager>.GetInstance().onDismiss();
    });
    sortAndFilter.Initialize(Persist.bugu00510SortAndFilter.Data, onUpdateFilter, onDicision);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void IbtnToComposite()
  {
    if (this.IsPush)
      return;
    Bugu0053Scene.changeScene(true);
  }

  private IEnumerator SortIcon()
  {
    this.allItemIcon.ForEach((Action<ItemIcon>) (x => ((Component) x).gameObject.SetActive(false)));
    this.recipeDataList.ForEach((Action<RecipeData>) (x => x.icon = (ItemIcon) null));
    this.SortFilter();
    IEnumerator e = this.CreateAllIcon(this.displayDataList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetScrollItem(this.displayDataList);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  private void SortFilter()
  {
    Persist.RecipeSortAndFilterInfo data = Persist.bugu00510SortAndFilter.Data;
    this.displayDataList = this.Sort(this.Filter(this.recipeDataList, data.filter), data);
  }

  private void UpdateSortLabel()
  {
    if (this.SortLabel == null || Object.op_Equality((Object) this.SortLabel[0], (Object) null))
      return;
    UISprite component = this.SortLabel[0].GetComponent<UISprite>();
    RecipeSortAndFilter.SortSpriteLabel(Persist.bugu00510SortAndFilter.Data.sortType, component);
  }

  private void SetNewInventory()
  {
    this.inventoryItems.Clear();
    this.CreateInventoryItem(this.GetItemList(), this.GetMaterialList());
  }

  private List<PlayerItem> GetItemList()
  {
    return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !x.broken)).ToList<PlayerItem>();
  }

  private List<PlayerMaterialGear> GetMaterialList()
  {
    return ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x =>
    {
      if (x.isDilling() || x.isSpecialDilling())
        return false;
      return x.isCompse() || x.isWeaponMaterial();
    })).ToList<PlayerMaterialGear>();
  }

  private void CreateInventoryItem(
    List<PlayerItem> itemList,
    List<PlayerMaterialGear> materialItemList)
  {
    if (itemList != null)
    {
      foreach (PlayerItem playerItem in itemList)
        this.inventoryItems.Add(new InventoryItem(playerItem));
    }
    if (materialItemList == null)
      return;
    foreach (PlayerMaterialGear materialItem in materialItemList)
    {
      PlayerMaterialGear item = materialItem;
      this.inventoryItems.Add(new InventoryItem(item, this.inventoryItems.Count<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemID == item.id))));
    }
  }

  private void PushOK(
    GearGear gear,
    List<InventoryItem> items,
    List<GearCombineRecipe> gearRecipes,
    int zenyAmount)
  {
    this.StartCoroutine(this.StartComposite(gear, items.Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToList<GameCore.ItemInfo>(), gearRecipes, zenyAmount));
  }

  private IEnumerator StartComposite(
    GearGear gear,
    List<GameCore.ItemInfo> sendGears,
    List<GearCombineRecipe> gearRecipes,
    int zenyAmount)
  {
    Bugu00510Menu m = this;
    if (sendGears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.favorite)).ToList<GameCore.ItemInfo>().Count == 0)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      Future<WebAPI.Response.ItemGearCombineRecipeConfirm> futureF = WebAPI.ItemGearCombineRecipeConfirm(0, gearRecipes[0].ID, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.ItemGearCombineRecipeConfirm result = futureF.Result;
      if (result == null)
      {
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      }
      else
      {
        int maxCreateNum = result.max_create_num;
        GameObject gameObject = Singleton<PopupManager>.GetInstance().open(m.kakuninnPopupPrefab);
        gameObject.transform.localPosition = new Vector3(0.0f, -60f, 0.0f);
        e1 = gameObject.GetComponent<Bugu005RecipeCompositeMenu>().Init(m, gear, sendGears, gearRecipes, zenyAmount, maxCreateNum, m.inventoryItems);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        yield return (object) null;
        ((Component) m.dirRecipe).gameObject.SetActive(false);
        yield return (object) new WaitForSeconds(0.1f);
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        futureF = (Future<WebAPI.Response.ItemGearCombineRecipeConfirm>) null;
      }
    }
    else
    {
      string title = Consts.Format(Consts.GetInstance().POPUP_005_GEAR_WARNING_TITLE, (IDictionary) new Hashtable()
      {
        {
          (object) "type",
          (object) Consts.GetInstance().GEAR_0052_COMPOSITE
        }
      });
      string message = Consts.Format(Consts.GetInstance().POPUP_005_FAVORITE_WARNING_MESSAGE, (IDictionary) new Hashtable()
      {
        {
          (object) "type",
          (object) Consts.GetInstance().GEAR_0052_COMPOSITE
        }
      });
      m.StartCoroutine(PopupCommon.Show(title, message));
    }
  }

  public IEnumerator CompositeAPI(List<GameCore.ItemInfo> sendGears)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    int reisouJewelBefore = SMManager.Get<Player>().reisou_jewel;
    Future<WebAPI.Response.ItemGearCombine> futureF = WebAPI.ItemGearCombine(sendGears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.isWeapon)).Select<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.itemID)).ToArray<int>(), sendGears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.isCompse || x.isWeaponMaterial)).Select<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.itemID)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.ItemGearCombine result = futureF.Result;
    if (result == null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
    else
    {
      GameCore.ItemInfo revTargetData = !(result.player_item != (PlayerItem) null) ? new GameCore.ItemInfo(result.player_material_gear) : new GameCore.ItemInfo(result.player_item);
      int addReisouJewel = result.player.reisou_jewel - reisouJewelBefore;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      if (!revTargetData.gear.kind.isEquip)
        Bugu00561Scene.changeScene(true, revTargetData, revTargetData.isNew, true);
      else
        Gacha00611Scene.changeScene(true, revTargetData.isNew, 0, revTargetData, addReisouJewel);
      ItemIcon.IsPoolCache = true;
      this.isIntegrated = true;
    }
  }

  public IEnumerator CompositeMultipleAPI(int recipeID, int counter)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    int reisouJewelBefore = SMManager.Get<Player>().reisou_jewel;
    Future<WebAPI.Response.ItemGearCombineRecipe> futureF = WebAPI.ItemGearCombineRecipe(counter, recipeID, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.ItemGearCombineRecipe result = futureF.Result;
    if (result == null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
    else
    {
      GameCore.ItemInfo revTargetData = result.player_items.Length == 0 ? new GameCore.ItemInfo(result.player_material_gear) : new GameCore.ItemInfo(((IEnumerable<PlayerItem>) result.player_items).First<PlayerItem>());
      int addReisouJewel = result.player.reisou_jewel - reisouJewelBefore;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      if (!revTargetData.gear.kind.isEquip)
        Bugu00561Scene.changeScene(true, revTargetData, revTargetData.isNew, true, false, counter);
      else
        Gacha00611Scene.changeScene(true, revTargetData.isNew, counter, revTargetData, addReisouJewel);
      ItemIcon.IsPoolCache = true;
      this.isIntegrated = true;
    }
  }

  protected override void Update()
  {
    base.Update();
    if (!this.isInitialize || this.displayDataList.Count <= ItemIcon.ScreenValue)
      return;
    int checkHeight = ItemIcon.Height * 2;
    float base_y = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scroolStartY;
    float num = (float) (Mathf.Max(0, this.displayDataList.Count - ItemIcon.ScreenValue - 1) / ItemIcon.ColumnValue * ItemIcon.Height);
    float add_height = (float) (ItemIcon.Height * ItemIcon.RowValue);
    if ((double) base_y < 0.0)
      base_y = 0.0f;
    if ((double) base_y > (double) num)
      base_y = num;
    do
      ;
    while (this.UpdateScroll(checkHeight, base_y, add_height));
  }

  private bool UpdateScroll(int checkHeight, float base_y, float add_height)
  {
    bool flag = false;
    int iconCount = 0;
    foreach (GameObject gameObject in this.scroll)
    {
      GameObject item = gameObject;
      float num1 = item.transform.localPosition.y + base_y;
      int? nullable = this.displayDataList.FirstIndexOrNull<RecipeData>((Func<RecipeData, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) item)));
      if ((double) num1 > (double) checkHeight)
      {
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y - add_height, 0.0f);
        if (nullable.HasValue && nullable.Value + ItemIcon.MaxValue < (this.displayDataList.Count + 4) / 5 * 5)
        {
          if (nullable.Value + ItemIcon.MaxValue >= this.displayDataList.Count)
            item.SetActive(false);
          else
            this.ScrollIconUpdate(this.displayDataList, nullable.Value + ItemIcon.MaxValue, iconCount);
          flag = true;
        }
      }
      else if ((double) num1 < -((double) add_height - (double) checkHeight))
      {
        int num2 = ItemIcon.MaxValue;
        if (!item.activeSelf)
        {
          item.SetActive(true);
          num2 = 0;
        }
        if (nullable.HasValue && nullable.Value - num2 >= 0)
        {
          item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y + add_height, 0.0f);
          this.ScrollIconUpdate(this.displayDataList, nullable.Value - num2, iconCount);
          flag = true;
        }
      }
      else if (nullable.HasValue)
        this.ScrollIconUpdate(this.displayDataList, nullable.Value, iconCount);
      ++iconCount;
    }
    return flag;
  }

  private void OnDestroy()
  {
  }

  public override void onBackButton()
  {
    if (((Component) this.dirRecipe).gameObject.activeSelf)
      return;
    if (Singleton<PopupManager>.GetInstance().isOpen)
      Singleton<PopupManager>.GetInstance().onDismiss();
    else
      this.backScene();
  }

  protected virtual void OnEnable()
  {
    if (!this.ScrollView.isDragging)
      return;
    this.ScrollView.Press(false);
  }
}
