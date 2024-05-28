// Decompiled with JetBrains decompiler
// Type: UnitMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniLinq;
using UnityEngine;

#nullable disable
public class UnitMenuBase : BackButtonMenuBase
{
  protected DateTime serverTime = DateTime.MinValue;
  private int iconWidth;
  private int iconHeight;
  private int iconColumnValue;
  private int iconRowValue;
  private int iconScreenValue;
  private int iconMaxValue;
  protected bool isInitialize;
  private float scrool_start_y;
  protected UnitMenuBase.IconType iconType;
  protected GameObject unitPrefab;
  protected UnitSortAndFilter.SORT_TYPES sortType;
  protected SortAndFilter.SORT_TYPE_ORDER_BUY orderType;
  public bool isBattleFirst;
  public bool isTowerEntry = true;
  protected List<UnitIconBase> allUnitIcons = new List<UnitIconBase>();
  protected List<UnitIconInfo> allUnitInfos = new List<UnitIconInfo>();
  private List<UnitIconInfo> displayUnitInfos_ = new List<UnitIconInfo>();
  private int? displayIconSize_;
  private bool[] showFilters;
  protected List<UnitIconInfo> usedfilterIconInfoList;
  protected bool isUpdateIcon;
  [SerializeField]
  protected UILabel TxtNumber;
  [SerializeField]
  protected UILabel TxtNumberpossession;
  [SerializeField]
  protected UILabel TxtNumberselect;
  [SerializeField]
  protected UILabel TxtNumberzeny;
  [SerializeField]
  protected UILabel TxtPossessionUnit;
  [SerializeField]
  protected UILabel TxtPossessionUnitSell;
  [SerializeField]
  protected UISprite SortSprite;
  [SerializeField]
  protected NGxScroll2 scroll;
  [SerializeField]
  protected bool isDispOnlyNormalUnit;
  [SerializeField]
  protected float possessionBiasX;
  [SerializeField]
  private GameObject dir_noList;
  [SerializeField]
  [Tooltip("使用期限表示スイッチ")]
  protected bool enabledExpireDate;
  protected float initPossessionPosX;
  protected Action extendFunc;
  protected Persist<Persist.UnitSortAndFilterInfo> persist;
  private bool IsEquip;
  private bool RemoveButton;
  private bool ForBattleCheck;
  private bool PrincessType;
  private bool IsSpecialIcon;
  protected bool IsRecord;
  private int MaxDispMaterialUnit;
  private bool m_isGroupingUniqueMaterialUnit;
  protected int lastReferenceUnitID = -1;
  protected int lastReferenceUnitIndex = -1;
  public string Title = "";
  public GameObject BottomViewPossession;
  public bool isBottomViewSell;
  public bool isBottomViewFormation;
  public bool isBottomViewPossesion;
  public bool isHideSortAndFilterButton;
  public bool isStorageButton;
  public bool isMaterialButton;
  public bool isRegressionButton;
  public bool isFriendSupport;
  [NonSerialized]
  public PlayerUnit BaseUnit;
  public Action exceptionBackScene;
  public bool isMaterial;
  private Dictionary<UnitGroupHead, List<int>> allGroupIDs;

  protected int IconWidth => this.iconWidth;

  protected int IconHeight
  {
    get => this.iconHeight;
    set => this.iconHeight = value;
  }

  protected int IconMaxValue => this.iconMaxValue;

  public UnitSortAndFilter.SORT_TYPES CurrentSortType => this.sortType;

  protected List<UnitIconInfo> displayUnitInfos
  {
    get => this.displayUnitInfos_;
    set
    {
      this.displayUnitInfos_ = value;
      this.resetDisplayIconSize();
    }
  }

  protected int displayIconSize
  {
    get
    {
      return !this.displayIconSize_.HasValue ? (this.displayIconSize_ = new int?(Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count))).Value : this.displayIconSize_.Value;
    }
  }

  private void resetDisplayIconSize() => this.displayIconSize_ = new int?();

  public PlayerUnit baseUnit
  {
    get => this.BaseUnit;
    set => this.BaseUnit = value;
  }

  public bool isEditCustomDeck { get; set; }

  public PlayerCustomDeck customDeck { get; set; }

  protected List<int> firstPositionUnitIds { get; set; }

  public Dictionary<UnitGroupHead, List<int>> AllGroupIDs
  {
    get
    {
      if (this.allGroupIDs == null)
        this.allGroupIDs = UnitMenuBase.CreateAllGroupIDs();
      return this.allGroupIDs;
    }
  }

  private void Awake()
  {
    if (!Object.op_Inequality((Object) this.BottomViewPossession, (Object) null))
      return;
    this.initPossessionPosX = this.BottomViewPossession.transform.localPosition.x;
  }

  [Conditional("UNITY_EDITOR")]
  private void Debug_Log(string log)
  {
  }

  public virtual void Foreground()
  {
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.backScene();
  }

  public override void onBackButton()
  {
    if (!this.isInitialize)
      return;
    this.IbtnBack();
  }

  public virtual void IbtnClearS()
  {
  }

  public virtual void IbtnNo()
  {
  }

  public virtual void IbtnSort()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowSortAndFilterPopup());
  }

  public virtual void IbtnYes()
  {
  }

  public virtual void IbtnYesS()
  {
  }

  public virtual void VScrollBar()
  {
  }

  private bool[] CreateFilterTable()
  {
    if (this.persist == null)
      return Enumerable.Repeat<bool>(true, 60).ToArray<bool>();
    bool[] filterTable = new bool[60];
    List<bool> filter = this.persist.Data.filter;
    int count = filter.Count;
    for (int index = 0; index < 60; ++index)
      filterTable[index] = index < count && filter[index];
    return filterTable;
  }

  private Dictionary<UnitGroupHead, List<int>> GetGroupIDs()
  {
    Dictionary<UnitGroupHead, List<int>> groupIds;
    if (this.persist == null || this.persist.Data.groupIDs == null)
    {
      groupIds = new Dictionary<UnitGroupHead, List<int>>()
      {
        [UnitGroupHead.group_all] = new List<int>() { 2 },
        [UnitGroupHead.group_large] = new List<int>(),
        [UnitGroupHead.group_small] = new List<int>(),
        [UnitGroupHead.group_clothing] = new List<int>(),
        [UnitGroupHead.group_generation] = new List<int>()
      };
      if (this.persist != null)
        this.persist.Data.groupIDs = groupIds;
    }
    else
      groupIds = this.persist.Data.groupIDs;
    return groupIds;
  }

  public UnitIconInfo GetUnitInfoAll(PlayerUnit target)
  {
    if (target == (PlayerUnit) null)
      return (UnitIconInfo) null;
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (allUnitInfo.playerUnit == target)
        return allUnitInfo;
    }
    return (UnitIconInfo) null;
  }

  public int GetUnitInfoDisplayIndex(PlayerUnit target)
  {
    if (target == (PlayerUnit) null)
      return -1;
    int? nullable = this.displayUnitInfos.FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => v.playerUnit == target));
    return nullable.HasValue ? nullable.Value : -1;
  }

  public UnitIconInfo GetUnitInfoDisplay(PlayerUnit target)
  {
    if (target == (PlayerUnit) null)
      return (UnitIconInfo) null;
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (displayUnitInfo.playerUnit == target)
        return displayUnitInfo;
    }
    return (UnitIconInfo) null;
  }

  public virtual void ForBattle(Func<UnitIconInfo, PlayerUnit, bool> func)
  {
    Action<PlayerUnit[]> action = (Action<PlayerUnit[]>) (units =>
    {
      if (units == null)
        return;
      foreach (PlayerUnit unit1 in units)
      {
        PlayerUnit unit = unit1;
        if (!(unit == (PlayerUnit) null))
        {
          UnitIconInfo unitIconInfo = this.allUnitInfos.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => func(x, unit)));
          if (unitIconInfo != null)
            unitIconInfo.for_battle = true;
        }
      }
    });
    if (this.isEditCustomDeck)
    {
      if (this.customDeck == null)
        return;
      action(this.customDeck.player_units);
    }
    else
    {
      foreach (PlayerDeck playerDeck in SMManager.Get<PlayerDeck[]>())
      {
        if (playerDeck != null && playerDeck.player_units != null)
          action(playerDeck.player_units);
      }
      PlayerSeaDeck[] playerSeaDeckArray = SMManager.Get<PlayerSeaDeck[]>();
      if (playerSeaDeckArray != null)
      {
        foreach (PlayerSeaDeck playerSeaDeck in playerSeaDeckArray)
        {
          if (playerSeaDeck != null && playerSeaDeck.player_units != null)
            action(playerSeaDeck.player_units);
        }
      }
      ExploreDeck[] exploreDeckArray = SMManager.Get<ExploreDeck[]>();
      if (exploreDeckArray == null)
        return;
      foreach (ExploreDeck exploreDeck in exploreDeckArray)
      {
        if (exploreDeck != null && exploreDeck.player_units != null)
          action(exploreDeck.player_units);
      }
    }
  }

  public virtual void UpdateAllUnitTowerEntryView()
  {
    if (this.isEditCustomDeck)
      return;
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      allUnitInfo.UpdateTowerEntryViewFlag();
  }

  protected virtual void UpdateAllUnitRentalFlag()
  {
    if (this.isEditCustomDeck)
      return;
    int?[] rentalPlayerUnitIds = SMManager.Get<PlayerRentalPlayerUnitIds>()?.rental_player_unit_ids;
    if (rentalPlayerUnitIds == null)
      return;
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      allUnitInfo.is_rental = allUnitInfo.playerUnit != (PlayerUnit) null && ((IEnumerable<int?>) rentalPlayerUnitIds).Contains<int?>(new int?(allUnitInfo.playerUnit.id));
  }

  public virtual IEnumerator UpdateInfoAndScroll(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits = null)
  {
    if (this.lastReferenceUnitID != -1 && this.lastReferenceUnitIndex != -1)
    {
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = this.lastReferenceUnitID;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = this.lastReferenceUnitIndex;
      this.lastReferenceUnitID = -1;
      this.lastReferenceUnitIndex = -1;
    }
    this.CreateAllUnitInfoEx((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) playerMaterialUnits, this.IsEquip, this.RemoveButton, this.ForBattleCheck, this.PrincessType, this.m_isGroupingUniqueMaterialUnit, this.IsSpecialIcon);
    if (Object.op_Inequality((Object) this.unitPrefab, (Object) null))
    {
      for (int count = this.allUnitIcons.Count; count < Mathf.Min(this.iconMaxValue, this.allUnitInfos.Count); ++count)
      {
        UnitIconBase component = Object.Instantiate<GameObject>(this.unitPrefab).GetComponent<UnitIconBase>();
        UnitIcon unitIcon = component as UnitIcon;
        if (Object.op_Inequality((Object) unitIcon, (Object) null))
          unitIcon.EnabledExpireDate = this.enabledExpireDate;
        if (this.allUnitInfos[count].playerUnit != (PlayerUnit) null)
          component.unit = this.allUnitInfos[count].playerUnit.unit;
        this.allUnitIcons.Add(component);
      }
    }
    this.displayUnitInfos = this.FilterBy(this.CreateFilterTable(), this.GetGroupIDs()).ToList<UnitIconInfo>();
    this.SortAndSetIcons(this.sortType, this.orderType, this.isBattleFirst, this.isTowerEntry);
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++index)
      this.ResetUnitIcon(index);
    for (int i = 0; i < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++i)
    {
      IEnumerator e = this.CreateUnitIcon(i, i, this.BaseUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private bool CheckWeaponType(UnitIconInfo info, bool[] filters)
  {
    if (!filters[1] && !filters[2] && !filters[3] && !filters[4] && !filters[5] && !filters[6] && !filters[55])
      return true;
    bool flag = false;
    switch (info.playerUnit.unit.kind.Enum)
    {
      case GearKindEnum.sword:
        flag = filters[1];
        break;
      case GearKindEnum.axe:
        flag = filters[2];
        break;
      case GearKindEnum.spear:
        flag = filters[3];
        break;
      case GearKindEnum.bow:
        flag = filters[4];
        break;
      case GearKindEnum.gun:
        flag = filters[5];
        break;
      case GearKindEnum.staff:
        flag = filters[6];
        break;
      case GearKindEnum.unique_wepon:
        flag = filters[55];
        break;
    }
    return flag;
  }

  private bool CheckElementType(UnitIconInfo info, bool[] filters)
  {
    if (!filters[7] && !filters[8] && !filters[9] && !filters[10] && !filters[11] && !filters[12] && !filters[13])
      return true;
    bool flag = false;
    switch (info.playerUnit.GetElement())
    {
      case CommonElement.none:
        flag = filters[7];
        break;
      case CommonElement.fire:
        flag = filters[8];
        break;
      case CommonElement.wind:
        flag = filters[10];
        break;
      case CommonElement.thunder:
        flag = filters[11];
        break;
      case CommonElement.ice:
        flag = filters[9];
        break;
      case CommonElement.light:
        flag = filters[12];
        break;
      case CommonElement.dark:
        flag = filters[13];
        break;
    }
    return flag;
  }

  private bool CheckUnitType(UnitIconInfo info, bool[] filters)
  {
    if (!filters[14] && !filters[16] && !filters[17] && !filters[15] && !filters[19] && !filters[18])
      return true;
    bool flag = false;
    switch (info.playerUnit.unit_type.Enum)
    {
      case UnitTypeEnum.ouki:
        flag = filters[14];
        break;
      case UnitTypeEnum.meiki:
        flag = filters[15];
        break;
      case UnitTypeEnum.kouki:
        flag = filters[16];
        break;
      case UnitTypeEnum.maki:
        flag = filters[17];
        break;
      case UnitTypeEnum.syuki:
        flag = filters[18];
        break;
      case UnitTypeEnum.syouki:
        flag = filters[19];
        break;
    }
    return flag;
  }

  private bool CheckRarity(UnitIconInfo info, bool[] filters)
  {
    if (!filters[20] && !filters[21] && !filters[22] && !filters[23] && !filters[24] && !filters[40])
      return true;
    bool flag = false;
    switch (info.playerUnit.unit.rarity.index)
    {
      case 0:
        flag = filters[20];
        break;
      case 1:
        flag = filters[21];
        break;
      case 2:
        flag = filters[22];
        break;
      case 3:
        flag = filters[23];
        break;
      case 4:
        flag = filters[24];
        break;
      case 5:
        flag = filters[40];
        break;
    }
    return flag;
  }

  private bool CheckUnit(UnitIconInfo info, bool[] filters)
  {
    if (!filters[25] && !filters[26] && !filters[27] && !filters[28])
      return true;
    bool flag = false;
    if (this.CheckNormalUnit(info, filters) || this.CheckEvolutionUnit(info, filters) || this.CheckComposeUnit(info, filters) || this.CheckTransmigrationUnit(info, filters))
      flag = true;
    return flag;
  }

  private bool CheckNormalUnit(UnitIconInfo info, bool[] filters)
  {
    bool flag = false;
    if (info.playerUnit.unit.IsNormalUnit)
      flag = filters[25];
    return flag;
  }

  private bool CheckEvolutionUnit(UnitIconInfo info, bool[] filters)
  {
    bool flag = false;
    if (info.playerUnit.unit.IsSinkaUnit)
      flag = filters[26];
    return flag;
  }

  private bool CheckComposeUnit(UnitIconInfo info, bool[] filters)
  {
    bool flag = false;
    if (info.playerUnit.unit.IsTougouUnit)
      flag = filters[27];
    return flag;
  }

  private bool CheckTransmigrationUnit(UnitIconInfo info, bool[] filters)
  {
    bool flag = false;
    if (info.playerUnit.unit.IsTenseiUnit)
      flag = filters[28];
    return flag;
  }

  private bool CheckLevel(UnitIconInfo info, bool[] filters)
  {
    if (!filters[29] && !filters[30])
      return true;
    bool flag1 = false;
    bool flag2 = false;
    if (info.playerUnit.max_level == info.playerUnit.level)
      flag1 = filters[29];
    if (info.playerUnit.max_level > info.playerUnit.level)
      flag2 = filters[30];
    return flag1 | flag2;
  }

  private bool CheckEquipment(UnitIconInfo info, bool[] filters)
  {
    if (!filters[31] && !filters[32])
      return true;
    bool flag1 = false;
    bool flag2 = false;
    GearGear equippedGearOrInitial = info.playerUnit.equippedGearOrInitial;
    if (equippedGearOrInitial != info.playerUnit.initial_gear || info.playerUnit.equippedGear2 != (PlayerItem) null)
      flag1 = filters[31];
    if (equippedGearOrInitial == info.playerUnit.initial_gear && info.playerUnit.equippedGear2 == (PlayerItem) null)
      flag2 = filters[32];
    if (equippedGearOrInitial == info.playerUnit.initial_gear && info.playerUnit.equippedGear3 == (PlayerItem) null)
      flag2 = filters[32];
    return flag1 | flag2;
  }

  private bool CheckCompose(UnitIconInfo info, bool[] filters)
  {
    if (!filters[34] && !filters[35])
      return true;
    int incrementInCompose = info.playerUnit.amountIncrementInCompose;
    return info.playerUnit.isMaxParamInComposeEx ? filters[34] : filters[35];
  }

  private bool CheckFavorite(UnitIconInfo info, bool[] filters)
  {
    if (!filters[36] && !filters[37])
      return true;
    return info.playerUnit.favorite ? filters[36] : filters[37];
  }

  private bool CheckTowerEntry(UnitIconInfo info, bool[] filters)
  {
    if (!filters[38] && !filters[39])
      return true;
    return info.playerUnit.tower_is_entry || info.playerUnit.corps_is_entry ? filters[38] : filters[39];
  }

  private bool CheckAttackType(UnitIconInfo info, bool[] filters)
  {
    if (!filters[41] && !filters[42] && !filters[43] && !filters[44] && !filters[45] && !filters[46])
      return true;
    bool flag = false;
    GearAttackClassification attackClassification = info.playerUnit.unit.GetInitialGear(info.playerUnit.job_id).gearClassification.attack_classification;
    if (info.playerUnit.equippedGear != (PlayerItem) null)
      attackClassification = info.playerUnit.equippedGear.gear.gearClassification.attack_classification;
    switch (attackClassification)
    {
      case GearAttackClassification.none:
        flag = filters[41];
        break;
      case GearAttackClassification.slash:
        flag = filters[42];
        break;
      case GearAttackClassification.blow:
        flag = filters[43];
        break;
      case GearAttackClassification.pierce:
        flag = filters[44];
        break;
      case GearAttackClassification.shoot:
        flag = filters[45];
        break;
      case GearAttackClassification.magic:
        flag = filters[46];
        break;
    }
    return flag;
  }

  private bool CheckAwake(UnitIconInfo info, bool[] filters)
  {
    if (!filters[48] && !filters[49] && !filters[50] && !filters[47])
      return true;
    List<UnitUnit> list = MasterData.UnitUnit.Where<KeyValuePair<int, UnitUnit>>((Func<KeyValuePair<int, UnitUnit>, bool>) (x => x.Value.same_character_id == info.unit.same_character_id)).Select<KeyValuePair<int, UnitUnit>, UnitUnit>((Func<KeyValuePair<int, UnitUnit>, UnitUnit>) (x => x.Value)).ToList<UnitUnit>();
    bool flag = false;
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].CanAwakeUnitFlag)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return filters[47];
    if (info.unit.awake_unit_flag)
      return filters[50];
    return !filters[48] && info.unit.rarity.index == 5 ? filters[49] : filters[48];
  }

  private bool CheckClassChange(UnitIconInfo info, bool[] filters)
  {
    if (!filters[51] && !filters[52] && !filters[53] && !filters[54])
      return true;
    MasterDataTable.UnitJob unitJob;
    if (!MasterData.UnitJob.TryGetValue(info.playerUnit.job_id, out unitJob))
    {
      Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) info.playerUnit.job_id + "]"));
      return false;
    }
    switch (unitJob.job_rank)
    {
      case UnitJobRank.rank1:
      case UnitJobRank.rank2:
      case UnitJobRank.rank3:
      case UnitJobRank.vertex:
        return filters[51];
      case UnitJobRank.vertex1:
        return filters[52];
      case UnitJobRank.vertex2:
        return filters[53];
      case UnitJobRank.vertex3:
        return filters[54];
      default:
        return false;
    }
  }

  private bool CheckGroup(
    Dictionary<int, UnitGroup> groupDic,
    UnitIconInfo info,
    Dictionary<UnitGroupHead, List<int>> groupIDs)
  {
    UnitGroup groupInfo = (UnitGroup) null;
    groupDic.TryGetValue(info.unit.ID, out groupInfo);
    if (groupInfo == null)
      return false;
    int num1 = groupIDs[UnitGroupHead.group_large].Count == 0 ? 1 : (groupIDs[UnitGroupHead.group_large].Count == this.AllGroupIDs[UnitGroupHead.group_large].Count ? 1 : 0);
    UnitGroupLargeCategory groupLargeCategory = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).FirstOrDefault<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x => x.ID == groupInfo.group_large_category_id_UnitGroupLargeCategory));
    int num2 = num1 != 0 ? 1 : (groupLargeCategory == null ? 0 : (groupIDs[UnitGroupHead.group_large].Contains(groupLargeCategory.ID) ? 1 : 0));
    bool flag1 = groupIDs[UnitGroupHead.group_small].Count == 0 || groupIDs[UnitGroupHead.group_small].Count == this.AllGroupIDs[UnitGroupHead.group_small].Count;
    UnitGroupSmallCategory groupSmallCategory = ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).FirstOrDefault<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x => x.ID == groupInfo.group_small_category_id_UnitGroupSmallCategory));
    bool flag2 = flag1 || groupSmallCategory != null && groupIDs[UnitGroupHead.group_small].Contains(groupSmallCategory.ID);
    bool flag3 = groupIDs[UnitGroupHead.group_clothing].Count == 0 || groupIDs[UnitGroupHead.group_clothing].Count == this.AllGroupIDs[UnitGroupHead.group_clothing].Count;
    UnitGroupClothingCategory clothingCategory = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).FirstOrDefault<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x => x.ID == groupInfo.group_clothing_category_id_UnitGroupClothingCategory || x.ID == groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory));
    bool flag4 = flag3 || clothingCategory != null && (groupIDs[UnitGroupHead.group_clothing].Contains(groupInfo.group_clothing_category_id_UnitGroupClothingCategory) || groupIDs[UnitGroupHead.group_clothing].Contains(groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory));
    bool flag5 = groupIDs[UnitGroupHead.group_generation].Count == 0 || groupIDs[UnitGroupHead.group_generation].Count == this.AllGroupIDs[UnitGroupHead.group_generation].Count;
    UnitGroupGenerationCategory generationCategory = ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).FirstOrDefault<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x => x.ID == groupInfo.group_generation_category_id_UnitGroupGenerationCategory));
    bool flag6 = flag5 || generationCategory != null && groupIDs[UnitGroupHead.group_generation].Contains(generationCategory.ID);
    int num3 = flag2 ? 1 : 0;
    return (num2 & num3 & (flag4 ? 1 : 0) & (flag6 ? 1 : 0)) != 0;
  }

  private bool CheckUpperAttribute(UnitIconInfo info, bool[] filters)
  {
    if (!filters[56] && !filters[57] || filters[57])
      return true;
    return info.playerUnit.unit.upper_attribute_flag ? filters[56] : filters[57];
  }

  private bool CheckCallSelect(UnitIconInfo info, bool[] filters)
  {
    if (!filters[58] && !filters[59])
      return true;
    Player player = SMManager.Get<Player>();
    return ((IEnumerable<int>) player.call_skill_same_character_ids).Contains<int>(info.playerUnit.unit.same_character_id) || ((IEnumerable<PlayerCallDivorceHistory>) player.call_divorce_histories).Any<PlayerCallDivorceHistory>((Func<PlayerCallDivorceHistory, bool>) (x => x.same_character_id == info.playerUnit.unit.same_character_id)) ? filters[58] : filters[59];
  }

  private bool arrayEquals(bool[] b1, bool[] b2)
  {
    if (b1.Length != b2.Length || b1 == null || b2 == null)
      return false;
    for (int index = 0; index < b1.Length; ++index)
    {
      if (b1[index] != b2[index])
        return false;
    }
    return true;
  }

  public IEnumerable<UnitIconInfo> FilterBy(
    bool[] filters,
    Dictionary<UnitGroupHead, List<int>> groupIDs)
  {
    List<UnitIconInfo> unitIconInfoList = new List<UnitIconInfo>();
    Dictionary<int, UnitGroup> groupDic = (Dictionary<int, UnitGroup>) null;
    if (groupIDs[UnitGroupHead.group_all].Count <= 0)
      groupDic = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).ToDictionary<UnitGroup, int>((Func<UnitGroup, int>) (x => x.unit_id));
    Func<UnitSortAndFilter.FILTER_CATEGORIES, bool> func = (Func<UnitSortAndFilter.FILTER_CATEGORIES, bool>) (category =>
    {
      foreach (int index in UnitSortAndFilter.FilterCategories[category])
      {
        if (!filters[index])
          return true;
      }
      return false;
    });
    bool flag1 = func(UnitSortAndFilter.FILTER_CATEGORIES.Weapon);
    bool flag2 = func(UnitSortAndFilter.FILTER_CATEGORIES.Element);
    bool flag3 = func(UnitSortAndFilter.FILTER_CATEGORIES.UnitType);
    bool flag4 = func(UnitSortAndFilter.FILTER_CATEGORIES.Rarity);
    int num = func(UnitSortAndFilter.FILTER_CATEGORIES.Unit) ? 1 : 0;
    bool flag5 = func(UnitSortAndFilter.FILTER_CATEGORIES.Level);
    bool flag6 = func(UnitSortAndFilter.FILTER_CATEGORIES.Equipment);
    bool flag7 = func(UnitSortAndFilter.FILTER_CATEGORIES.Compose);
    bool flag8 = func(UnitSortAndFilter.FILTER_CATEGORIES.Favorite);
    bool flag9 = func(UnitSortAndFilter.FILTER_CATEGORIES.Tower);
    bool flag10 = func(UnitSortAndFilter.FILTER_CATEGORIES.AttackType);
    bool flag11 = func(UnitSortAndFilter.FILTER_CATEGORIES.Awake);
    bool flag12 = func(UnitSortAndFilter.FILTER_CATEGORIES.ClassChange);
    bool flag13 = groupIDs[UnitGroupHead.group_all].Count <= 0;
    bool flag14 = func(UnitSortAndFilter.FILTER_CATEGORIES.UpperAttribute);
    bool flag15 = func(UnitSortAndFilter.FILTER_CATEGORIES.CallSelect);
    for (int index = 0; index < this.allUnitInfos.Count; ++index)
    {
      UnitIconInfo allUnitInfo = this.allUnitInfos[index];
      if (allUnitInfo.removeButton)
        unitIconInfoList.Add(allUnitInfo);
      else if (allUnitInfo.unit != null && (!flag1 || this.CheckWeaponType(allUnitInfo, filters)) && (!flag2 || this.CheckElementType(allUnitInfo, filters)) && (!flag3 || this.CheckUnitType(allUnitInfo, filters)) && (!flag4 || this.CheckRarity(allUnitInfo, filters)) && (!flag5 || this.CheckLevel(allUnitInfo, filters)) && (!flag6 || this.CheckEquipment(allUnitInfo, filters)) && (!flag7 || this.CheckCompose(allUnitInfo, filters)) && (!flag8 || this.CheckFavorite(allUnitInfo, filters)) && (!flag9 || this.CheckTowerEntry(allUnitInfo, filters)) && (!flag10 || this.CheckAttackType(allUnitInfo, filters)) && (!flag11 || this.CheckAwake(allUnitInfo, filters)) && (!flag12 || this.CheckClassChange(allUnitInfo, filters)) && (!flag13 || this.CheckGroup(groupDic, allUnitInfo, groupIDs)) && (!flag14 || this.CheckUpperAttribute(allUnitInfo, filters)) && (!flag15 || this.CheckCallSelect(allUnitInfo, filters)) && this.CheckUnit(allUnitInfo, filters))
        unitIconInfoList.Add(allUnitInfo);
    }
    return (IEnumerable<UnitIconInfo>) unitIconInfoList;
  }

  protected virtual void Sort(SortInfo info)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.sortType = info.sortType;
    this.orderType = info.orderType;
    this.isBattleFirst = info.isBattleFirst;
    this.isTowerEntry = info.isTowerEntry;
    this.displayUnitInfos = this.FilterBy(info.filters, this.GetGroupIDs()).ToList<UnitIconInfo>();
    this.SortAndSetIcons(this.sortType, this.orderType, this.isBattleFirst, this.isTowerEntry);
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++index)
      this.ResetUnitIcon(index);
    this.StartCoroutine(this.CreateUnitIconRange(Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count)));
    foreach (UnitIcon unitIcon in this.scroll.GridChildren().Select<GameObject, UnitIcon>((Func<GameObject, UnitIcon>) (x => x.GetComponent<UnitIcon>())))
    {
      if (Object.op_Inequality((Object) unitIcon, (Object) null) && unitIcon.PlayerUnit != (PlayerUnit) null)
        unitIcon.ShowBottomInfo(this.sortType);
    }
    this.SetNoListLable();
  }

  private IEnumerator CreateUnitIconRange(int value)
  {
    for (int i = 0; i < value; ++i)
    {
      IEnumerator e = this.CreateUnitIcon(i, i, this.BaseUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  protected void UpdateSortSprite(UnitSortAndFilter.SORT_TYPES type)
  {
    string[] strArray = new string[34]
    {
      "",
      "GearKind",
      "CommonElement",
      "Lv",
      "Rare",
      "Cost",
      "Fight",
      "ATK",
      "MAT",
      "DEF",
      "MDE",
      "DEX",
      "Critical",
      "EVA",
      "AGI",
      "Technic",
      "Luck",
      "WeaponProficiency",
      "ArmorProficiency",
      "GetOrder",
      "Hp",
      "Breakthrough",
      "UnityValue",
      "TrustRate",
      "AverageRisingValue",
      "UnitName",
      "Illustrator",
      "VoiceActor",
      "HistoryGroupNumber",
      "Trust",
      "UnitID",
      "Lv",
      "PossessionNumber",
      ""
    };
    if (string.IsNullOrEmpty(strArray[(int) type]))
      return;
    string str = string.Format("slc_Label_{0}.png__GUI__unit_title_short__unit_title_short_prefab", (object) strArray[(int) type]);
    UISpriteData sprite = this.SortSprite.atlas.GetSprite(str);
    if (sprite == null)
    {
      Debug.LogError((object) (str + "    is not exist!!!"));
      sprite = this.SortSprite.atlas.GetSprite("slc_Label_GearKind.png__GUI__unit_title_short__unit_title_short_prefab");
    }
    this.SortSprite.spriteName = str;
    ((Component) this.SortSprite).GetComponent<UIWidget>().SetDimensions(sprite.width, sprite.height);
  }

  protected virtual void SortAndSetIcons(
    UnitSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isBattleFirst,
    bool isTowerEntry)
  {
    bool flag = Object.op_Inequality((Object) this.SortSprite, (Object) null);
    if (flag)
      this.UpdateSortSprite(type);
    if (this.allUnitIcons.Count < 1 || this.allUnitIcons.Any<UnitIconBase>((Func<UnitIconBase, bool>) (v => Object.op_Equality((Object) v, (Object) null))))
      return;
    if (flag)
    {
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "unit004_8_6")
      {
        isBattleFirst = false;
        isTowerEntry = false;
      }
      this.displayUnitInfos = this.displayUnitInfos.SortBy(type, order, isBattleFirst, isTowerEntry).ToList<UnitIconInfo>();
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "unit004_UnitTraining_List")
      {
        int?[] memoryIds = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : new int?[0];
        for (int i = memoryIds.Length - 1; i >= 0; i--)
        {
          if (memoryIds[i].HasValue)
            this.displayUnitInfos = this.displayUnitInfos.OrderByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x =>
            {
              int? nullable = memoryIds[i];
              int id = x.playerUnit.id;
              return nullable.GetValueOrDefault() == id & nullable.HasValue;
            })).ToList<UnitIconInfo>();
        }
      }
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "unit004_8_6")
      {
        this.displayUnitInfos = this.displayUnitInfos.OrderByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x => x.unit.same_character_id == this.baseUnit.unit.same_character_id)).ThenByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x => x.unit.IsBreakThrough)).ThenByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x => x.unit.is_unity_value_up)).ThenByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x => x.unit.IsTougouUnit)).ToList<UnitIconInfo>();
        List<UnitIconInfo> list1 = this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.for_battle)).SortBy(type, order, isBattleFirst, isTowerEntry).ToList<UnitIconInfo>();
        List<UnitIconInfo> list2 = this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => !x.for_battle && x.is_rental)).SortBy(type, order, isBattleFirst, isTowerEntry).ToList<UnitIconInfo>();
        List<UnitIconInfo> list3 = this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => !x.for_battle && !x.is_rental && x.playerUnit.favorite)).SortBy(type, order, isBattleFirst, isTowerEntry).ToList<UnitIconInfo>();
        List<UnitIconInfo> second = list2;
        List<UnitIconInfo> list4 = list1.Concat<UnitIconInfo>((IEnumerable<UnitIconInfo>) second).Concat<UnitIconInfo>((IEnumerable<UnitIconInfo>) list3).ToList<UnitIconInfo>().OrderByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (ui => ui.for_battle)).ThenByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (ui => ui.is_rental)).ThenByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (ui => ui.playerUnit.favorite)).ToList<UnitIconInfo>();
        for (int index = 0; index < list4.Count; ++index)
        {
          this.displayUnitInfos.Remove(list4[index]);
          this.displayUnitInfos.Add(list4[index]);
        }
      }
      else if (Singleton<NGSceneManager>.GetInstance().sceneName == "mypage_edit")
      {
        for (int index = 0; index < this.displayUnitInfos.Count; ++index)
        {
          UnitIconInfo displayUnitInfo = this.displayUnitInfos[index];
          if (displayUnitInfo.playerUnit != (PlayerUnit) null && displayUnitInfo.playerUnit.id == MypageUnitUtil.getUnitId())
          {
            this.displayUnitInfos.Remove(displayUnitInfo);
            this.displayUnitInfos.Insert(0, displayUnitInfo);
            break;
          }
        }
      }
      if (!this.firstPositionUnitIds.IsNullOrEmpty<int>())
      {
        int weightEnd = this.firstPositionUnitIds.Count;
        this.displayUnitInfos = this.displayUnitInfos.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x =>
        {
          PlayerUnit pu = x.playerUnit;
          if (x.removeButton || pu == (PlayerUnit) null)
            return -1;
          int? nullable = this.firstPositionUnitIds.FirstIndexOrNull<int>((Func<int, bool>) (i => i == pu.id));
          return !nullable.HasValue ? weightEnd : nullable.Value;
        })).ToList<UnitIconInfo>();
      }
    }
    this.scroll.Reset();
    foreach (UnitIconBase allUnitIcon in this.allUnitIcons)
    {
      ((Component) allUnitIcon).transform.parent = ((Component) this).transform;
      ((Component) allUnitIcon).gameObject.SetActive(false);
    }
    int displayIconSize = this.displayIconSize;
    for (int index = 0; index < displayIconSize; ++index)
    {
      this.scroll.Add(((Component) this.allUnitIcons[index]).gameObject, this.iconWidth, this.iconHeight, index);
      ((Component) this.allUnitIcons[index]).gameObject.SetActive(true);
      if (this.allUnitIcons[index].unit != null && this.allUnitIcons[index].unit.IsMaterialUnit)
      {
        this.allUnitIcons[index].SetCounter(this.displayUnitInfos[index].count);
        this.allUnitIcons[index].SetSelectionCounter(this.displayUnitInfos[index].SelectedCount);
      }
    }
    this.resolveScrollPosition(Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID, Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex);
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
  }

  protected void resolveScrollPosition(int targetId = -1, int targetIndex = -1)
  {
    int count = this.displayUnitInfos.Count;
    this.scroll.CreateScrollPoint(this.iconHeight, count);
    if (targetId != -1)
    {
      int? nullable = this.displayUnitInfos.FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => !x.removeButton && x.playerUnit.id == targetId));
      if (nullable.HasValue)
        this.scroll.ResolvePosition(nullable.Value, count);
      else if (targetIndex != -1 && targetIndex < count)
        this.scroll.ResolvePosition(targetIndex, count);
      else
        this.scroll.ResolvePosition(count - 1, count);
    }
    else
      this.scroll.ResolvePosition();
  }

  public void setLastReference(int unitId = -1, int unitIndex = -1)
  {
    this.lastReferenceUnitID = unitId;
    this.lastReferenceUnitIndex = unitIndex;
  }

  private void ScrollIconUpdate(int info_index, int unit_index)
  {
    this.ResetUnitIcon(unit_index);
    if (this.iconType == UnitMenuBase.IconType.Normal || this.iconType == UnitMenuBase.IconType.EarthNormal || this.iconType == UnitMenuBase.IconType.NormalWithHpGauge)
    {
      if (this.displayUnitInfos[info_index].removeButton)
        this.CreateUnitIconCache(info_index, unit_index);
      else if (UnitIcon.IsCache(this.displayUnitInfos[info_index].playerUnit.unit))
        this.CreateUnitIconCache(info_index, unit_index);
      else
        this.StartCoroutine(this.CreateUnitIcon(info_index, unit_index));
    }
    else if (UnitIcon.IsCache(this.displayUnitInfos[info_index].playerUnit.unit))
      this.CreateUnitIconCache(info_index, unit_index, this.BaseUnit);
    else
      this.StartCoroutine(this.CreateUnitIcon(info_index, unit_index, this.BaseUnit));
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.displayUnitInfos.Count <= this.iconScreenValue) && !this.isUpdateIcon)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.displayUnitInfos.Count - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
    float num4 = (float) (this.iconHeight * this.iconRowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    while (true)
    {
      do
      {
        bool flag = false;
        int unit_index = 0;
        foreach (GameObject gameObject in this.scroll)
        {
          GameObject unit = gameObject;
          float num5 = unit.transform.localPosition.y + num2;
          if ((double) num5 > (double) num1)
          {
            int? nullable = this.displayUnitInfos.FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) unit)));
            int info_index = nullable.HasValue ? nullable.Value + this.iconMaxValue : (this.displayUnitInfos.Count + 4) / 5 * 5;
            if (nullable.HasValue && info_index < (this.displayUnitInfos.Count + 4) / 5 * 5)
            {
              unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
              if (info_index >= this.displayUnitInfos.Count)
                unit.SetActive(false);
              else
                this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          else if ((double) num5 < -((double) num4 - (double) num1))
          {
            int num6 = this.iconMaxValue;
            if (!unit.activeSelf)
            {
              unit.SetActive(true);
              num6 = 0;
            }
            int? nullable = this.displayUnitInfos.FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) unit)));
            int info_index = nullable.HasValue ? nullable.Value - num6 : -1;
            if (nullable.HasValue && info_index >= 0)
            {
              unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
              this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          ++unit_index;
        }
        if (!flag)
          goto label_27;
      }
      while (!this.isUpdateIcon);
      this.isUpdateIcon = false;
    }
label_27:;
  }

  protected virtual void ResetUnitIcon(int index)
  {
    if (this.allUnitIcons == null || this.allUnitIcons.Count == 0 || this.allUnitIcons.Any<UnitIconBase>((Func<UnitIconBase, bool>) (v => Object.op_Equality((Object) v, (Object) null))))
      return;
    UnitIconBase unitIcon = this.allUnitIcons[index];
    if (this.iconType == UnitMenuBase.IconType.Normal || this.iconType == UnitMenuBase.IconType.EarthNormal || this.iconType == UnitMenuBase.IconType.NormalWithHpGauge)
      unitIcon.Deselect();
    else
      unitIcon.Deselect();
    this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<UnitIconInfo>((Action<UnitIconInfo>) (b => b.icon = (UnitIconBase) null));
  }

  protected virtual IEnumerator CreateUnitIconBase(GameObject prefab)
  {
    this.allUnitIcons.Clear();
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.allUnitInfos.Count); ++index)
    {
      UnitIconBase component = Object.Instantiate<GameObject>(prefab).GetComponent<UnitIconBase>();
      if (this.allUnitInfos[index].playerUnit != (PlayerUnit) null)
        component.unit = this.allUnitInfos[index].playerUnit.unit;
      this.allUnitIcons.Add(component);
      component.ForBattle = this.allUnitInfos[index].for_battle;
      component.TowerEntry = this.allUnitInfos[index].is_tower_entry;
      component.UnitRental = !Singleton<NGGameDataManager>.GetInstance().IsEarth && this.allUnitInfos[index].is_rental;
      if (this.allUnitInfos[index].unit != null)
        component.CanAwake = this.allUnitInfos[index].unit.CanAwakeUnitFlag;
      component.UnitUsed = this.allUnitInfos[index].is_used;
      component.Overkillers = this.allUnitInfos[index].is_overkillers;
      component.SetupDeckStatusBlink();
      component.Equip = this.allUnitInfos[index].equip;
      switch (component)
      {
        case UnitIcon _:
          UnitIcon unitIcon = (UnitIcon) component;
          unitIcon.EnabledExpireDate = this.enabledExpireDate;
          unitIcon.princessType.DispPrincessType(this.allUnitInfos[index].pricessType);
          unitIcon.SpecialIconType = this.allUnitInfos[index].specialIconType;
          unitIcon.SpecialIcon = this.allUnitInfos[index].isSpecialIcon;
          break;
        case UnitDetailIcon _:
          UnitDetailIcon unitDetailIcon = (UnitDetailIcon) component;
          unitDetailIcon.UnitIcon.EnabledExpireDate = this.enabledExpireDate;
          unitDetailIcon.UnitIcon.princessType.DispPrincessType(this.allUnitInfos[index].pricessType);
          break;
      }
      component.SetCounter(this.allUnitInfos[index].count);
    }
    this.SortAndSetIcons(this.sortType, this.orderType, this.isBattleFirst, this.isTowerEntry);
    IEnumerator e;
    if (this.IsRecord)
    {
      Future<GameObject> prefabF = Res.Prefabs.unit004_9_9.slc_reinforce_memory_slot_icon.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.allUnitIcons.ForEach((Action<UnitIconBase>) (x => x.SetRecordObj(prefabF.Result)));
    }
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++index)
      this.ResetUnitIcon(index);
    for (int i = 0; i < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++i)
    {
      e = this.CreateUnitIcon(i, i, this.BaseUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected virtual IEnumerator CreateUnitIcon(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    if (!Object.op_Equality((Object) unitIcon, (Object) null))
    {
      this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<UnitIconInfo>((Action<UnitIconInfo>) (b => b.icon = (UnitIconBase) null));
      unitIcon.SetCounter(this.displayUnitInfos[info_index].count);
      this.displayUnitInfos[info_index].icon = unitIcon;
      IEnumerator e;
      if (this.iconType == UnitMenuBase.IconType.Normal || this.iconType == UnitMenuBase.IconType.EarthNormal || this.iconType == UnitMenuBase.IconType.NormalWithHpGauge)
      {
        if (this.displayUnitInfos[info_index].removeButton)
        {
          unitIcon.SetRemoveButton();
          unitIcon.PlayerUnit = (PlayerUnit) null;
        }
        else if (this.displayUnitInfos[info_index].playerUnit.unit.IsMaterialUnit)
        {
          e = unitIcon.SetMaterialUnit(this.displayUnitInfos[info_index].playerUnit, false, this.getUnits());
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          e = unitIcon.SetPlayerUnit(this.displayUnitInfos[info_index].playerUnit, this.getUnits(), baseUnit, this.isMaterial);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        ((UnitIcon) unitIcon).setBottom(this.displayUnitInfos[info_index].playerUnit);
      }
      else if (this.displayUnitInfos[info_index].playerUnit.unit.IsMaterialUnit)
      {
        e = unitIcon.SetMaterialUnit(this.displayUnitInfos[info_index].playerUnit, false, this.getUnits());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = unitIcon.SetPlayerUnit(this.displayUnitInfos[info_index].playerUnit, this.getUnits(), baseUnit, this.isMaterial);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      unitIcon.ForBattle = this.displayUnitInfos[info_index].for_battle;
      unitIcon.TowerEntry = this.displayUnitInfos[info_index].is_tower_entry;
      unitIcon.UnitRental = !Singleton<NGGameDataManager>.GetInstance().IsEarth && this.displayUnitInfos[info_index].is_rental;
      if (this.displayUnitInfos[info_index].unit != null)
        unitIcon.CanAwake = this.displayUnitInfos[info_index].unit.CanAwakeUnitFlag;
      unitIcon.UnitUsed = this.displayUnitInfos[info_index].is_used;
      unitIcon.Overkillers = this.displayUnitInfos[info_index].is_overkillers;
      unitIcon.SetupDeckStatusBlink();
      unitIcon.Equip = this.displayUnitInfos[info_index].equip;
      if (this.IsRecord)
        unitIcon.SetRecordNum();
      if (unitIcon is UnitIcon)
      {
        UnitIcon unitIcon1 = (UnitIcon) unitIcon;
        unitIcon1.princessType.DispPrincessType(this.displayUnitInfos[info_index].pricessType);
        unitIcon1.SpecialIconType = this.displayUnitInfos[info_index].specialIconType;
        unitIcon1.SpecialIcon = this.displayUnitInfos[info_index].isSpecialIcon;
      }
      else if (unitIcon is UnitDetailIcon)
      {
        unitIcon.UnitIcon.princessType.DispPrincessType(this.displayUnitInfos[info_index].pricessType);
        unitIcon.UnitIcon.ShowBottomInfos(this.sortType);
      }
      unitIcon.SetCounter(this.displayUnitInfos[info_index].count);
      unitIcon.SetSelectionCounter(this.displayUnitInfos[info_index].SelectedCount);
      unitIcon.SelectMarker = this.displayUnitInfos[info_index].selectMarker;
      if (this.displayUnitInfos[info_index].alignmentSequence == 0)
      {
        if (this.displayUnitInfos[info_index].select == -1)
          unitIcon.Deselect();
        else
          unitIcon.Select(this.displayUnitInfos[info_index].select);
      }
      if (!this.displayUnitInfos[info_index].removeButton)
        unitIcon.ShowBottomInfo(this.sortType);
      if (!((Component) unitIcon).gameObject.activeSelf)
        ((Component) unitIcon).gameObject.SetActive(true);
    }
  }

  protected virtual void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<UnitIconInfo>((Action<UnitIconInfo>) (b => b.icon = (UnitIconBase) null));
    this.displayUnitInfos[info_index].icon = unitIcon;
    if (this.iconType == UnitMenuBase.IconType.Normal || this.iconType == UnitMenuBase.IconType.EarthNormal || this.iconType == UnitMenuBase.IconType.NormalWithHpGauge)
    {
      if (this.displayUnitInfos[info_index].removeButton)
      {
        unitIcon.SetRemoveButton();
        unitIcon.PlayerUnit = (PlayerUnit) null;
      }
      else if (this.displayUnitInfos[info_index].playerUnit.unit.IsMaterialUnit)
        ((UnitIcon) unitIcon).SetMaterialUnitCache(this.displayUnitInfos[info_index].playerUnit, false, this.getUnits());
      else
        ((UnitIcon) unitIcon).SetPlayerUnitCache(this.displayUnitInfos[info_index].playerUnit, this.getUnits());
      ((UnitIcon) unitIcon).setBottom(this.displayUnitInfos[info_index].playerUnit);
      this.displayUnitInfos[info_index].icon.SetCounter(this.displayUnitInfos[info_index].count);
      this.displayUnitInfos[info_index].icon.SetSelectionCounter(this.displayUnitInfos[info_index].SelectedCount);
    }
    else if (this.displayUnitInfos[info_index].playerUnit.unit.IsMaterialUnit)
      ((UnitDetailIcon) unitIcon).SetMaterialUnitCache(this.displayUnitInfos[info_index].playerUnit, false, this.getUnits(), baseUnit, this.displayUnitInfos[info_index].isTrustMaterial);
    else
      ((UnitDetailIcon) unitIcon).SetPlayerUnitCache(this.displayUnitInfos[info_index].playerUnit, this.getUnits(), baseUnit, this.isMaterial);
    unitIcon.ForBattle = this.displayUnitInfos[info_index].for_battle;
    unitIcon.TowerEntry = this.displayUnitInfos[info_index].is_tower_entry;
    unitIcon.UnitRental = !Singleton<NGGameDataManager>.GetInstance().IsEarth && this.displayUnitInfos[info_index].is_rental;
    if (this.displayUnitInfos[info_index].unit != null)
      unitIcon.CanAwake = this.displayUnitInfos[info_index].unit.CanAwakeUnitFlag;
    unitIcon.UnitUsed = this.displayUnitInfos[info_index].is_used;
    unitIcon.Overkillers = this.displayUnitInfos[info_index].is_overkillers;
    unitIcon.SetupDeckStatusBlink();
    unitIcon.Equip = this.displayUnitInfos[info_index].equip;
    if (this.IsRecord)
      unitIcon.SetRecordNum();
    if (unitIcon is UnitIcon)
    {
      UnitIcon unitIcon1 = (UnitIcon) unitIcon;
      unitIcon1.princessType.DispPrincessType(this.displayUnitInfos[info_index].pricessType);
      unitIcon1.SpecialIconType = this.displayUnitInfos[info_index].specialIconType;
      unitIcon1.SpecialIcon = this.displayUnitInfos[info_index].isSpecialIcon;
    }
    else if (unitIcon is UnitDetailIcon)
    {
      unitIcon.UnitIcon.princessType.DispPrincessType(this.displayUnitInfos[info_index].pricessType);
      unitIcon.UnitIcon.ShowBottomInfos(this.sortType);
    }
    unitIcon.SelectMarker = this.displayUnitInfos[info_index].selectMarker;
    if (!this.displayUnitInfos[info_index].removeButton)
      unitIcon.ShowBottomInfo(this.sortType);
    if (((Component) unitIcon).gameObject.activeSelf)
      return;
    ((Component) unitIcon).gameObject.SetActive(true);
  }

  private IEnumerator LoadObjectNormal()
  {
    yield return (object) null;
    for (int i = this.iconMaxValue; i < this.allUnitInfos.Count; ++i)
    {
      IEnumerator e = UnitIcon.LoadSprite(this.allUnitInfos[i].playerUnit.unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator LoadObjectDetail()
  {
    yield return (object) null;
    for (int i = this.iconMaxValue; i < this.allUnitInfos.Count; ++i)
    {
      IEnumerator e = UnitDetailIcon.LoadSprite(this.allUnitInfos[i].playerUnit.unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected IEnumerator LoadObject()
  {
    UnitMenuBase unitMenuBase = this;
    yield return (object) null;
    if (unitMenuBase.allUnitInfos.Count > unitMenuBase.iconMaxValue)
    {
      if (unitMenuBase.iconType == UnitMenuBase.IconType.Normal || unitMenuBase.iconType == UnitMenuBase.IconType.EarthNormal || unitMenuBase.iconType == UnitMenuBase.IconType.NormalWithHpGauge)
        unitMenuBase.StartCoroutine(unitMenuBase.LoadObjectNormal());
      else
        unitMenuBase.StartCoroutine(unitMenuBase.LoadObjectDetail());
    }
  }

  public IEnumerator CreateUnitIcon()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    instance.lastReferenceUnitID = -1;
    instance.lastReferenceUnitIndex = -1;
    IEnumerator e = this.CreateUnitIconBase(this.unitPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetNoListLable()
  {
    if (!Object.op_Inequality((Object) this.dir_noList, (Object) null))
      return;
    this.dir_noList.SetActive(this.getUnits().Length == 0);
  }

  public virtual IEnumerator Initialize()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitPrefab = prefabF.Result;
    this.sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
    this.orderType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.isBattleFirst = true;
    this.isTowerEntry = true;
    this.isInitialize = false;
    this.scroll.Clear();
  }

  protected void InitializeEnd()
  {
    this.StartCoroutine(this.LoadObject());
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
    if (Object.op_Inequality((Object) this.BottomViewPossession, (Object) null))
      this.BottomViewPossession.transform.localPosition = new Vector3(this.initPossessionPosX + this.possessionBiasX, this.BottomViewPossession.transform.localPosition.y, this.BottomViewPossession.transform.localPosition.z);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    this.SetNoListLable();
  }

  private void CreateAllUnitInfoEx(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnits,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool bGroupUniqueMaterialUnit,
    bool isSpecialIcon)
  {
    this.m_isGroupingUniqueMaterialUnit = bGroupUniqueMaterialUnit;
    if (!bGroupUniqueMaterialUnit)
    {
      this.CreateAllUnitInfo(playerUnits, playerMaterialUnits, isEquip, removeButton, for_battle_check, princessType, isSpecialIcon, this.MaxDispMaterialUnit);
    }
    else
    {
      this.allUnitInfos.Clear();
      if (removeButton)
        this.allUnitInfos.Add(new UnitIconInfo()
        {
          removeButton = true,
          gray = false,
          select = -1,
          for_battle = false,
          is_rental = false,
          pricessType = false,
          equip = false,
          isSpecialIcon = false,
          is_awakeUnti = false
        });
      if (playerUnits != null)
      {
        foreach (PlayerUnit playerUnit in playerUnits)
        {
          UnitIconInfo unitIconInfo = new UnitIconInfo();
          unitIconInfo.playerUnit = playerUnit;
          unitIconInfo.gray = false;
          unitIconInfo.select = -1;
          unitIconInfo.for_battle = false;
          unitIconInfo.is_rental = false;
          unitIconInfo.pricessType = princessType;
          unitIconInfo.isSpecialIcon = false;
          unitIconInfo.is_awakeUnti = false;
          if (isEquip && (unitIconInfo.playerUnit.equippedGear != (PlayerItem) null || unitIconInfo.playerUnit.equippedGear2 != (PlayerItem) null || unitIconInfo.playerUnit.equippedGear3 != (PlayerItem) null))
            unitIconInfo.equip = true;
          this.allUnitInfos.Add(unitIconInfo);
        }
      }
      if (playerMaterialUnits != null)
      {
        foreach (PlayerMaterialUnit playerMaterialUnit in playerMaterialUnits)
        {
          if (playerMaterialUnit.quantity > 0)
            this.allUnitInfos.Add(new UnitIconInfo()
            {
              playerUnit = PlayerUnit.CreateByPlayerMaterialUnit(playerMaterialUnit),
              gray = false,
              select = -1,
              for_battle = false,
              is_rental = false,
              pricessType = false,
              equip = false,
              isSpecialIcon = false,
              is_awakeUnti = false,
              count = playerMaterialUnit.quantity
            });
        }
      }
      if (for_battle_check)
      {
        this.ForBattle((Func<UnitIconInfo, PlayerUnit, bool>) ((info, unit) =>
        {
          if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
            return true;
          return info.playerUnit != (PlayerUnit) null && !info.removeButton && unit.id == info.playerUnit.id;
        }));
        this.UpdateAllUnitTowerEntryView();
        this.UpdateAllUnitRentalFlag();
      }
      if (isSpecialIcon)
      {
        QuestScoreBonusTimetable[] tables = ((IEnumerable<QuestScoreBonusTimetable>) SMManager.Get<QuestScoreBonusTimetable[]>()).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < this.serverTime && x.end_at > this.serverTime)).ToArray<QuestScoreBonusTimetable>();
        UnitBonus[] unitBonus = UnitBonus.getActiveUnitBonus(ServerTime.NowAppTime());
        if (tables.Length != 0 || unitBonus.Length != 0)
          this.allUnitInfos.ForEach((Action<UnitIconInfo>) (x =>
          {
            if (!(x.playerUnit != (PlayerUnit) null))
              return;
            string str = x.playerUnit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
            x.isSpecialIcon = !string.IsNullOrEmpty(str);
          }));
      }
      if (this.extendFunc == null)
        return;
      this.extendFunc();
    }
  }

  protected virtual void CreateAllUnitInfo(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnits,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool isSpecialIcon,
    int maxDispMaterialUnit)
  {
    this.allUnitInfos.Clear();
    if (removeButton)
      this.allUnitInfos.Add(new UnitIconInfo()
      {
        removeButton = true,
        gray = false,
        select = -1,
        for_battle = false,
        is_rental = false,
        pricessType = false,
        equip = false,
        isSpecialIcon = false
      });
    if (playerUnits != null)
    {
      foreach (PlayerUnit playerUnit in playerUnits)
      {
        UnitIconInfo unitIconInfo = new UnitIconInfo();
        unitIconInfo.playerUnit = playerUnit;
        unitIconInfo.gray = false;
        unitIconInfo.select = -1;
        unitIconInfo.for_battle = false;
        unitIconInfo.is_rental = false;
        unitIconInfo.pricessType = princessType;
        unitIconInfo.isSpecialIcon = false;
        if (isEquip && (unitIconInfo.playerUnit.equippedGear != (PlayerItem) null || unitIconInfo.playerUnit.equippedGear2 != (PlayerItem) null || unitIconInfo.playerUnit.equippedGear3 != (PlayerItem) null))
          unitIconInfo.equip = true;
        this.allUnitInfos.Add(unitIconInfo);
      }
    }
    if (playerMaterialUnits != null)
    {
      foreach (PlayerMaterialUnit playerMaterialUnit in playerMaterialUnits)
      {
        int num = maxDispMaterialUnit > 0 ? Mathf.Min(maxDispMaterialUnit, playerMaterialUnit.quantity) : playerMaterialUnit.quantity;
        for (int count = 0; count < num; ++count)
          this.allUnitInfos.Add(new UnitIconInfo()
          {
            playerUnit = PlayerUnit.CreateByPlayerMaterialUnit(playerMaterialUnit, count),
            gray = false,
            select = -1,
            for_battle = false,
            is_rental = false,
            pricessType = false,
            isSpecialIcon = false
          });
      }
    }
    if (for_battle_check)
    {
      this.ForBattle((Func<UnitIconInfo, PlayerUnit, bool>) ((info, unit) => info.playerUnit != (PlayerUnit) null && !info.removeButton && unit.id == info.playerUnit.id));
      this.UpdateAllUnitTowerEntryView();
      this.UpdateAllUnitRentalFlag();
    }
    if (isSpecialIcon)
    {
      QuestScoreBonusTimetable[] tables = ((IEnumerable<QuestScoreBonusTimetable>) SMManager.Get<QuestScoreBonusTimetable[]>()).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < this.serverTime && x.end_at > this.serverTime)).ToArray<QuestScoreBonusTimetable>();
      UnitBonus[] unitBonus = UnitBonus.getActiveUnitBonus(ServerTime.NowAppTime());
      if (tables.Length != 0 || unitBonus.Length != 0)
        this.allUnitInfos.ForEach((Action<UnitIconInfo>) (x =>
        {
          if (!(x.playerUnit != (PlayerUnit) null))
            return;
          string color_code = x.playerUnit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
          int? specialIconType = UnitIcon.GetSpecialIconType(color_code);
          x.specialIconType = specialIconType.HasValue ? specialIconType.Value : 0;
          x.isSpecialIcon = !string.IsNullOrEmpty(color_code);
        }));
    }
    if (this.extendFunc == null)
      return;
    this.extendFunc();
  }

  protected virtual void InitializeInfo(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnit,
    Persist<Persist.UnitSortAndFilterInfo> persistData,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool isSpecialIcon,
    Action createAllUnitInfoExtendFunc = null,
    int maxDispMaterialUnit = 0)
  {
    this.IsEquip = isEquip;
    this.RemoveButton = removeButton;
    this.ForBattleCheck = for_battle_check;
    this.PrincessType = princessType;
    this.IsSpecialIcon = isSpecialIcon;
    this.MaxDispMaterialUnit = maxDispMaterialUnit;
    this.extendFunc = createAllUnitInfoExtendFunc;
    this.sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
    this.orderType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.isBattleFirst = true;
    this.isTowerEntry = true;
    this.setSaveData(persistData);
    this.CreateAllUnitInfo(playerUnits, playerMaterialUnit, this.IsEquip, this.RemoveButton, this.ForBattleCheck, this.PrincessType, isSpecialIcon, maxDispMaterialUnit);
    this.displayUnitInfos = this.FilterBy(this.CreateFilterTable(), this.GetGroupIDs()).ToList<UnitIconInfo>();
  }

  private void setSaveData(Persist<Persist.UnitSortAndFilterInfo> persistData)
  {
    this.persist = persistData;
    if (this.persist == null)
      return;
    Persist.UnitSortAndFilterInfo sortAndFilterInfo = this.persist.Data;
    if (sortAndFilterInfo.filter.Count != 60)
    {
      this.persist.Delete();
      this.persist.Data = sortAndFilterInfo = new Persist.UnitSortAndFilterInfo();
    }
    this.sortType = sortAndFilterInfo.sortType;
    this.orderType = sortAndFilterInfo.order;
    this.isBattleFirst = sortAndFilterInfo.isBattleFirst;
    this.isTowerEntry = sortAndFilterInfo.isTowerEntry;
  }

  protected virtual void InitializeInfoEx(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnit,
    Persist<Persist.UnitSortAndFilterInfo> persistData,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool bGroupUniqueMaterialUnit,
    bool isSpecialIcon,
    Action createAllUnitInfoExtendFunc = null)
  {
    this.sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
    this.orderType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.setSaveData(persistData);
    this.InitializeInfoEx(playerUnits, playerMaterialUnit, this.sortType, this.orderType, isEquip, removeButton, for_battle_check, princessType, bGroupUniqueMaterialUnit, isSpecialIcon, createAllUnitInfoExtendFunc, this.isBattleFirst, this.isTowerEntry);
  }

  protected virtual void InitializeInfoEx(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnit,
    UnitSortAndFilter.SORT_TYPES sortType,
    SortAndFilter.SORT_TYPE_ORDER_BUY orderType,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool bGroupUniqueMaterialUnit,
    bool isSpecialIcon,
    Action createAllUnitInfoExtendFunc,
    bool battleFirst,
    bool towerEntry)
  {
    this.IsEquip = isEquip;
    this.RemoveButton = removeButton;
    this.ForBattleCheck = for_battle_check;
    this.PrincessType = princessType;
    this.IsSpecialIcon = isSpecialIcon;
    this.extendFunc = createAllUnitInfoExtendFunc;
    this.sortType = sortType;
    this.orderType = orderType;
    this.isBattleFirst = battleFirst;
    this.isTowerEntry = towerEntry;
    this.CreateAllUnitInfoEx(playerUnits, playerMaterialUnit, this.IsEquip, this.RemoveButton, this.ForBattleCheck, this.PrincessType, bGroupUniqueMaterialUnit, this.IsSpecialIcon);
    this.displayUnitInfos = this.FilterBy(this.CreateFilterTable(), this.GetGroupIDs()).ToList<UnitIconInfo>();
  }

  public static Dictionary<UnitGroupHead, List<int>> CreateAllGroupIDs()
  {
    Dictionary<UnitGroupHead, List<int>> allGroupIds = new Dictionary<UnitGroupHead, List<int>>();
    IEnumerable<UnitGroupLargeCategory> groupLargeCategories = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).Where<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    allGroupIds.Add(UnitGroupHead.group_large, new List<int>());
    if (groupLargeCategories != null)
    {
      foreach (UnitGroupLargeCategory groupLargeCategory in groupLargeCategories)
      {
        if (!string.IsNullOrEmpty(groupLargeCategory.name))
          allGroupIds[UnitGroupHead.group_large].Add(groupLargeCategory.ID);
      }
    }
    IEnumerable<UnitGroupSmallCategory> groupSmallCategories = ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).Where<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    allGroupIds.Add(UnitGroupHead.group_small, new List<int>());
    if (groupSmallCategories != null)
    {
      foreach (UnitGroupSmallCategory groupSmallCategory in groupSmallCategories)
      {
        if (!string.IsNullOrEmpty(groupSmallCategory.name))
          allGroupIds[UnitGroupHead.group_small].Add(groupSmallCategory.ID);
      }
    }
    IEnumerable<UnitGroupClothingCategory> clothingCategories = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).Where<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    allGroupIds.Add(UnitGroupHead.group_clothing, new List<int>());
    if (clothingCategories != null)
    {
      foreach (UnitGroupClothingCategory clothingCategory in clothingCategories)
      {
        if (!string.IsNullOrEmpty(clothingCategory.name))
          allGroupIds[UnitGroupHead.group_clothing].Add(clothingCategory.ID);
      }
    }
    IEnumerable<UnitGroupGenerationCategory> generationCategories = ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).Where<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    allGroupIds.Add(UnitGroupHead.group_generation, new List<int>());
    if (generationCategories != null)
    {
      foreach (UnitGroupGenerationCategory generationCategory in generationCategories)
      {
        if (!string.IsNullOrEmpty(generationCategory.name))
          allGroupIds[UnitGroupHead.group_generation].Add(generationCategory.ID);
      }
    }
    return allGroupIds;
  }

  public void SetIconType(UnitMenuBase.IconType type)
  {
    switch (type)
    {
      case UnitMenuBase.IconType.Normal:
        this.iconWidth = UnitIcon.Width;
        this.iconHeight = UnitIcon.Height;
        this.iconColumnValue = UnitIcon.ColumnValue;
        this.iconRowValue = UnitIcon.RowValue;
        this.iconScreenValue = UnitIcon.ScreenValue;
        this.iconMaxValue = UnitIcon.MaxValue;
        break;
      case UnitMenuBase.IconType.EarthNormal:
        this.iconWidth = UnitIcon.Width;
        this.iconHeight = UnitIcon.HeightEarth;
        this.iconColumnValue = UnitIcon.ColumnValue;
        this.iconRowValue = UnitIcon.RowValue;
        this.iconScreenValue = UnitIcon.ScreenValue;
        this.iconMaxValue = UnitIcon.MaxValue;
        break;
      case UnitMenuBase.IconType.NormalWithHpGauge:
        this.iconWidth = UnitIcon.Width;
        this.iconHeight = UnitIcon.HeightWithHpGauge;
        this.iconColumnValue = UnitIcon.ColumnValue;
        this.iconRowValue = UnitIcon.RowValue;
        this.iconScreenValue = UnitIcon.ScreenValue;
        this.iconMaxValue = UnitIcon.MaxValue;
        break;
      default:
        this.iconWidth = UnitDetailIcon.Width;
        this.iconHeight = UnitDetailIcon.Height;
        this.iconColumnValue = UnitDetailIcon.ColumnValue;
        this.iconRowValue = UnitDetailIcon.RowValue;
        this.iconScreenValue = UnitDetailIcon.ScreenValue;
        this.iconMaxValue = UnitDetailIcon.MaxValue;
        break;
    }
    this.iconType = type;
    this.resetDisplayIconSize();
  }

  protected PlayerUnit[] getUnits()
  {
    return this.displayUnitInfos.Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>();
  }

  protected virtual IEnumerator ShowSortAndFilterPopup()
  {
    UnitMenuBase menu = this;
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      UnitMenuBase unitMenuBase = menu;
      Future<GameObject> sortPopupPrefabF = Res.Prefabs.popup.popup_Unit_Sort__anim_popup01.Load<GameObject>();
      IEnumerator e = sortPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject SortPopupPrefab = sortPopupPrefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
      UnitSortAndFilter filter = SortPopupPrefab.GetComponent<UnitSortAndFilter>();
      e = filter.Initialize(new Action<SortInfo>(menu.Sort), menu.persist, menu, menu.isDispOnlyNormalUnit, menu.isFriendSupport);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      filter.SetUnitNum(menu.displayUnitInfos, menu.allUnitInfos);
      filter.SortFilterUnitNum = (Action<SortInfo>) (x => filter.SetUnitNum(unitMenuBase.FilterBy(x.filters, x.groupIDs).ToList<UnitIconInfo>(), unitMenuBase.allUnitInfos));
      SortPopupPrefab.gameObject.SetActive(false);
      Singleton<PopupManager>.GetInstance().open(SortPopupPrefab, isCloned: true);
      SortPopupPrefab.gameObject.SetActive(true);
      sortPopupPrefabF = (Future<GameObject>) null;
      SortPopupPrefab = (GameObject) null;
    }
    else
      menu.IsPush = false;
  }

  public enum IconType
  {
    Normal,
    Detail,
    EarthNormal,
    NormalWithHpGauge,
  }
}
