// Decompiled with JetBrains decompiler
// Type: Bugu0053DirRecipeList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu0053DirRecipeList : MonoBehaviour
{
  [SerializeField]
  private UIWidget SlcRecipeBase;
  [SerializeField]
  private UILabel TxtRecipeGearName;
  [SerializeField]
  private GameObject IconRecipeGear;
  [SerializeField]
  private GameObject[] IconMaterial;
  [SerializeField]
  private GameObject[] labMaterialNum;
  [SerializeField]
  private UILabel[] TxtMaterialNum;
  [SerializeField]
  private UIButton IbtnRecipeYes;
  [SerializeField]
  private UISprite[] IconRankShortage;
  [SerializeField]
  private UILabel TxtZenie;
  [SerializeField]
  private GameObject dirMain;
  [SerializeField]
  private GameObject dirMaterial;
  [SerializeField]
  private GameObject dirYes;
  private List<GearGear> materialGears = new List<GearGear>();
  private List<GearCombineRecipe> firstRecipe;
  private List<GearCombineRecipe> allGearRecipes;
  private List<int> materialGearsRank = new List<int>();
  private List<InventoryItem> sendMaterialList = new List<InventoryItem>();
  private GameObject ItemIconPrefab;
  private GameObject materialInfoPopupPrefabF;
  private GearGear mainGear;
  private List<GearGear> otherMainGears = new List<GearGear>();
  private Action<GearGear, List<InventoryItem>, List<GearCombineRecipe>, int> IbtnEvent;
  private Action CloseEvent;
  public Bugu0053DirRecipePopup root;

  public int width => this.SlcRecipeBase.width;

  public int height => this.SlcRecipeBase.height;

  public IEnumerator Init(
    Bugu0053DirRecipePopup recipePopup,
    GearCombineRecipe gearRecipe,
    List<GearCombineRecipe> allRecipes,
    Action<GearGear, List<InventoryItem>, List<GearCombineRecipe>, int> ButtonEvent,
    Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDicData,
    Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDicDescendingData,
    GameObject itemIconPrefabObj,
    Bugu0053DirRecipeListPrefabs prefabs,
    Action backEvent,
    long playerMoney,
    int delayCount)
  {
    this.root = recipePopup;
    this.firstRecipe = new List<GearCombineRecipe>();
    this.firstRecipe.Add(gearRecipe);
    this.allGearRecipes = allRecipes;
    this.IbtnEvent = ButtonEvent;
    this.CloseEvent = backEvent;
    this.ItemIconPrefab = itemIconPrefabObj;
    this.materialInfoPopupPrefabF = prefabs.DirMaterialInfo;
    this.sendMaterialList.Clear();
    if (delayCount > 0)
      yield return (object) new WaitForSeconds(PerformanceConfig.LoadingBlanceTime * (float) delayCount);
    Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDic = new Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>>();
    Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>> playerGearDicDescending = new Dictionary<int, Tuple<List<GameCore.ItemInfo>, int>>();
    Dictionary<int, int> playerGearUseCnt = new Dictionary<int, int>();
    foreach (int? nullable in new List<int?>()
    {
      new int?(gearRecipe.material1_gear_id),
      gearRecipe.material2_gear_id,
      gearRecipe.material3_gear_id,
      gearRecipe.material4_gear_id,
      gearRecipe.material5_gear_id
    })
    {
      if (nullable.HasValue)
      {
        int key = nullable.Value;
        if (playerGearDicData.ContainsKey(key) && playerGearDicDescendingData.ContainsKey(key))
        {
          Tuple<List<GameCore.ItemInfo>, int> tuple1 = new Tuple<List<GameCore.ItemInfo>, int>(new List<GameCore.ItemInfo>((IEnumerable<GameCore.ItemInfo>) playerGearDicData[key].Item1), playerGearDicData[key].Item2);
          Tuple<List<GameCore.ItemInfo>, int> tuple2 = new Tuple<List<GameCore.ItemInfo>, int>(new List<GameCore.ItemInfo>((IEnumerable<GameCore.ItemInfo>) playerGearDicDescendingData[key].Item1), playerGearDicDescendingData[key].Item2);
          playerGearDic[key] = tuple1;
          playerGearDicDescending[key] = tuple2;
        }
      }
    }
    this.mainGear = MasterData.GearGear[gearRecipe.combined_gear_id];
    foreach (GearCombineRecipe allRecipe in allRecipes)
      this.otherMainGears.Add(MasterData.GearGear[allRecipe.combined_gear_id]);
    this.materialGears.Clear();
    this.materialGearsRank.Clear();
    this.materialGears.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x => x.group_id == gearRecipe.material1_gear_id)));
    this.materialGearsRank.Add(this.SetRequestGearRank(gearRecipe.material1_gear_rank));
    if (gearRecipe.material2_gear_id.HasValue)
    {
      this.materialGears.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material2GearId = gearRecipe.material2_gear_id;
        int valueOrDefault = material2GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material2GearId.HasValue;
      })));
      this.materialGearsRank.Add(this.SetRequestGearRank(gearRecipe.material2_gear_rank));
    }
    if (gearRecipe.material3_gear_id.HasValue)
    {
      this.materialGears.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material3GearId = gearRecipe.material3_gear_id;
        int valueOrDefault = material3GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material3GearId.HasValue;
      })));
      this.materialGearsRank.Add(this.SetRequestGearRank(gearRecipe.material3_gear_rank));
    }
    if (gearRecipe.material4_gear_id.HasValue)
    {
      this.materialGears.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material4GearId = gearRecipe.material4_gear_id;
        int valueOrDefault = material4GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material4GearId.HasValue;
      })));
      this.materialGearsRank.Add(this.SetRequestGearRank(gearRecipe.material4_gear_rank));
    }
    if (gearRecipe.material5_gear_id.HasValue)
    {
      this.materialGears.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material5GearId = gearRecipe.material5_gear_id;
        int valueOrDefault = material5GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material5GearId.HasValue;
      })));
      this.materialGearsRank.Add(this.SetRequestGearRank(gearRecipe.material5_gear_rank));
    }
    bool bwait = true;
    IEnumerator e;
    if (allRecipes.Count > 1)
    {
      e = this.SetGearMainIcon(this.mainGear, this.IconRecipeGear.transform, allRecipes.IndexOf(gearRecipe));
      while (e.MoveNext())
      {
        bwait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
    }
    else
    {
      e = this.SetGearMainIcon(this.mainGear, this.IconRecipeGear.transform);
      while (e.MoveNext())
      {
        bwait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
    }
    int materialLength = this.materialGears.Count;
    for (int i = 0; i < materialLength; ++i)
    {
      if (bwait)
        yield return (object) null;
      if (this.materialGears[i] != null)
      {
        bwait = true;
        bool isGray = true;
        int playerMaterialQuantity = 0;
        int gearGroupId = this.materialGears[i].group_id;
        if (playerGearDic.ContainsKey(gearGroupId))
        {
          playerMaterialQuantity = playerGearDicData[gearGroupId].Item1.Sum<GameCore.ItemInfo>((Func<GameCore.ItemInfo, int>) (x => x.quantity));
          GameCore.ItemInfo gear = playerGearDic[gearGroupId].Item1.Find((Predicate<GameCore.ItemInfo>) (x => x.gearLevel >= this.materialGearsRank[i]));
          if (gear != null)
          {
            if (this.CheckCanMaterial(gear) == 0)
            {
              isGray = false;
              this.sendMaterialList.Add(new InventoryItem(gear));
            }
          }
          else
          {
            gear = playerGearDicDescending[gearGroupId].Item1.Find((Predicate<GameCore.ItemInfo>) (x => x.gearLevel < this.materialGearsRank[i]));
            ((Component) this.IconRankShortage[i]).gameObject.SetActive(true);
          }
          int num;
          playerGearUseCnt.TryGetValue(gear.itemID, out num);
          playerGearUseCnt[gear.itemID] = num + 1;
          if (gear.quantity <= playerGearUseCnt[gear.itemID])
          {
            playerGearDic[gearGroupId].Item1.Remove(gear);
            playerGearDicDescending[gearGroupId].Item1.Remove(gear);
          }
          if (playerGearDic[this.materialGears[i].group_id].Item1.Count == 0)
          {
            playerGearDic.Remove(gearGroupId);
            playerGearDicDescending.Remove(gearGroupId);
          }
        }
        e = this.SetGearIcon(this.materialGears[i], this.IconMaterial[i].transform, this.materialGearsRank[i], isGray, playerMaterialQuantity);
        while (e.MoveNext())
        {
          bwait = false;
          yield return e.Current;
        }
        e = (IEnumerator) null;
        if (playerGearDicData.ContainsKey(gearGroupId))
          playerMaterialQuantity = playerGearDicData[gearGroupId].Item1.Sum<GameCore.ItemInfo>((Func<GameCore.ItemInfo, int>) (x => x.quantity));
        this.TxtMaterialNum[i].SetTextLocalize(playerMaterialQuantity);
      }
    }
    for (int index = materialLength; index < this.TxtMaterialNum.Length; ++index)
      ((Component) this.TxtMaterialNum[index]).gameObject.SetActive(false);
    for (int index = materialLength; index < this.labMaterialNum.Length; ++index)
      this.labMaterialNum[index].SetActive(false);
    int num1 = this.SetZenie(this.materialGears);
    if ((long) num1 > playerMoney)
      ((UIWidget) this.TxtZenie).color = Color.red;
    this.TxtZenie.SetTextLocalize(num1);
    ((UIButtonColor) this.IbtnRecipeYes).isEnabled = this.sendMaterialList.Count == this.materialGears.Count && (long) num1 <= playerMoney;
    this.TxtRecipeGearName.SetTextLocalize(this.mainGear.name);
    this.dirMain.SetActive(true);
    this.dirMaterial.SetActive(true);
    this.dirYes.SetActive(true);
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

  private IEnumerator SetGearMainIcon(GearGear gear, Transform iconTransform, int index = 0)
  {
    ItemIcon component = this.ItemIconPrefab.Clone(iconTransform).GetComponent<ItemIcon>();
    component.onClick = (Action<ItemIcon>) (_ => this.OnGearPopup(gear, index));
    IEnumerator e = component.InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetGearIcon(
    GameCore.ItemInfo playerGear,
    Transform iconTransform,
    int rank,
    bool isGray,
    int quantity)
  {
    ItemIcon gearIcon = this.ItemIconPrefab.Clone(iconTransform).GetComponent<ItemIcon>();
    IEnumerator e = gearIcon.InitByItemInfo(playerGear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gearIcon.onClick = (Action<ItemIcon>) (_ => this.OnMaterialPopup(playerGear.gear, quantity, rank));
    gearIcon.gear.rank.SetActive(false);
    if (playerGear.isWeapon)
    {
      gearIcon.gear.rank.SetActive(true);
      gearIcon.gear.rank.GetComponent<UI2DSprite>().sprite2D = gearIcon.rankSprite[rank - 1];
    }
    gearIcon.gear.unlimit.SetActive(false);
    gearIcon.Gray = isGray;
    gearIcon.ForBattle = playerGear.ForBattle;
    gearIcon.EnableQuantity(0);
  }

  private IEnumerator SetGearIcon(
    GearGear gear,
    Transform iconTransform,
    int rank,
    bool isGray,
    int quantity)
  {
    ItemIcon gearIcon = this.ItemIconPrefab.Clone(iconTransform).GetComponent<ItemIcon>();
    gearIcon.onClick = (Action<ItemIcon>) (_ => this.OnMaterialPopup(gear, quantity, rank));
    IEnumerator e = gearIcon.InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gearIcon.gear.rank.SetActive(false);
    if (!gear.isMaterial())
    {
      gearIcon.gear.rank.SetActive(true);
      gearIcon.gear.rank.GetComponent<UI2DSprite>().sprite2D = gearIcon.rankSprite[rank - 1];
    }
    gearIcon.gear.unlimit.SetActive(false);
    gearIcon.Gray = isGray;
    gearIcon.EnableQuantity(0);
  }

  private int SetRequestGearRank(int? rank) => rank.HasValue ? rank.Value : 0;

  private void OnGearPopup(GearGear gear, int index = 0)
  {
    if (this.mainGear.kind.Enum != GearKindEnum.smith)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      if (this.otherMainGears.Count <= 1)
        Guide01142Scene.changeScene(true, this.otherMainGears.Where<GearGear>((Func<GearGear, bool>) (x => x.kind.Enum != GearKindEnum.smith)).Select<GearGear, GearGear>((Func<GearGear, GearGear>) (x => gear)).ToArray<GearGear>(), (int[]) null, (int[]) null, index);
      else
        Guide01142Scene.changeScene(true, this.otherMainGears.ToArray(), (int[]) null, (int[]) null, index);
    }
    else if (this.mainGear.compose_kind.kind.Enum != GearKindEnum.smith)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      if (this.otherMainGears.Count <= 1)
        Guide01142bScene.changeScene(true, this.otherMainGears.Where<GearGear>((Func<GearGear, bool>) (x => x.compose_kind.kind.Enum != GearKindEnum.smith && x.kind.Enum == GearKindEnum.smith)).Select<GearGear, GearGear>((Func<GearGear, GearGear>) (x => gear)).ToArray<GearGear>(), index);
      else
        Guide01142bScene.changeScene(true, this.otherMainGears.ToArray(), index);
    }
    else
    {
      string sceneName = "guide011_4_2c";
      Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, true, (object) this.mainGear, (object) false, (object) index);
    }
  }

  private void OnMaterialPopup(GearGear gear, int quantity, int requestRank)
  {
    this.StartCoroutine(this.SetMaterialPopup(gear, quantity, requestRank));
  }

  public int SetZenie(List<GearGear> gears)
  {
    int total_item_level = 0;
    int total_item_rarity = 0;
    int cnt_use_gears = 0;
    gears.ForEach((Action<GearGear>) (item =>
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

  private IEnumerator SetMaterialPopup(GearGear gear, int quantity, int requestRank)
  {
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(this.materialInfoPopupPrefabF).GetComponent<Bugu0053MaterialPopup>().Init(this.root, gear, this.ItemIconPrefab, quantity, requestRank);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void IbtnYes()
  {
    if (this.IbtnEvent == null || this.CloseEvent == null)
      return;
    this.IbtnEvent(this.mainGear, this.sendMaterialList, this.firstRecipe, this.SetZenie(this.materialGears));
  }
}
