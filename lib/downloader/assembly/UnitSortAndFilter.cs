// Decompiled with JetBrains decompiler
// Type: UnitSortAndFilter
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
public class UnitSortAndFilter : SortAndFilter
{
  private readonly Dictionary<UnitSortAndFilter.SORT_TYPES, string> SortLabelStr = new Dictionary<UnitSortAndFilter.SORT_TYPES, string>()
  {
    {
      UnitSortAndFilter.SORT_TYPES.None,
      Consts.GetInstance().SORT_POPUP_LABEL_NONE
    },
    {
      UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy,
      Consts.GetInstance().SORT_POPUP_LABEL_BRANCHOFANARMY
    },
    {
      UnitSortAndFilter.SORT_TYPES.Attribute,
      Consts.GetInstance().SORT_POPUP_LABEL_ATTRIBUTE
    },
    {
      UnitSortAndFilter.SORT_TYPES.Level,
      Consts.GetInstance().SORT_POPUP_LABEL_LEVEL
    },
    {
      UnitSortAndFilter.SORT_TYPES.Rarity,
      Consts.GetInstance().SORT_POPUP_LABEL_RARITY
    },
    {
      UnitSortAndFilter.SORT_TYPES.Cost,
      Consts.GetInstance().SORT_POPUP_LABEL_COST
    },
    {
      UnitSortAndFilter.SORT_TYPES.FightingPower,
      Consts.GetInstance().SORT_POPUP_LABEL_FIGHTINGPOWER
    },
    {
      UnitSortAndFilter.SORT_TYPES.PhysicalAttack,
      Consts.GetInstance().SORT_POPUP_LABEL_PHYSICALATTACK
    },
    {
      UnitSortAndFilter.SORT_TYPES.MagicAttack,
      Consts.GetInstance().SORT_POPUP_LABEL_MAGICATTACK
    },
    {
      UnitSortAndFilter.SORT_TYPES.Defence,
      Consts.GetInstance().SORT_POPUP_LABEL_DEFENCE
    },
    {
      UnitSortAndFilter.SORT_TYPES.MagicDefence,
      Consts.GetInstance().SORT_POPUP_LABEL_MAGICDEFENCE
    },
    {
      UnitSortAndFilter.SORT_TYPES.Hit,
      Consts.GetInstance().SORT_POPUP_LABEL_HIT
    },
    {
      UnitSortAndFilter.SORT_TYPES.Critical,
      Consts.GetInstance().SORT_POPUP_LABEL_CRITICAL
    },
    {
      UnitSortAndFilter.SORT_TYPES.Avoid,
      Consts.GetInstance().SORT_POPUP_LABEL_AVOID
    },
    {
      UnitSortAndFilter.SORT_TYPES.Speed,
      Consts.GetInstance().SORT_POPUP_LABEL_SPEED
    },
    {
      UnitSortAndFilter.SORT_TYPES.Dexterity,
      Consts.GetInstance().SORT_POPUP_LABEL_DEXTERITY
    },
    {
      UnitSortAndFilter.SORT_TYPES.Luck,
      Consts.GetInstance().SORT_POPUP_LABEL_LUCK
    },
    {
      UnitSortAndFilter.SORT_TYPES.WeaponProficiency,
      Consts.GetInstance().SORT_POPUP_LABEL_WEAPONPROFICIENCY
    },
    {
      UnitSortAndFilter.SORT_TYPES.ArmorProficiency,
      Consts.GetInstance().SORT_POPUP_LABEL_ARMORPROFICIENCY
    },
    {
      UnitSortAndFilter.SORT_TYPES.GetOrder,
      Consts.GetInstance().SORT_POPUP_LABEL_GETORDER
    },
    {
      UnitSortAndFilter.SORT_TYPES.HP,
      Consts.GetInstance().SORT_POPUP_LABEL_HP
    },
    {
      UnitSortAndFilter.SORT_TYPES.Breakthrough,
      Consts.GetInstance().SORT_POPUP_LABEL_BREAKTHROUGH
    },
    {
      UnitSortAndFilter.SORT_TYPES.UnityValue,
      Consts.GetInstance().SORT_POPUP_LABEL_UNITYVALUE
    },
    {
      UnitSortAndFilter.SORT_TYPES.TrustRate,
      Consts.GetInstance().SORT_POPUP_LABEL_TRUSTRATE
    },
    {
      UnitSortAndFilter.SORT_TYPES.AverageRisingValue,
      Consts.GetInstance().SORT_POPUP_LABEL_AVERAGERISINGVALUE
    },
    {
      UnitSortAndFilter.SORT_TYPES.UnitName,
      Consts.GetInstance().SORT_POPUP_LABEL_UNITNAME
    },
    {
      UnitSortAndFilter.SORT_TYPES.Illustrator,
      Consts.GetInstance().SORT_POPUP_LABEL_ILLUSTRATOR
    },
    {
      UnitSortAndFilter.SORT_TYPES.VoiceActor,
      Consts.GetInstance().SORT_POPUP_LABEL_VOICEACTOR
    },
    {
      UnitSortAndFilter.SORT_TYPES.HistoryGroupNumber,
      Consts.GetInstance().SORT_POPUP_LABEL_HISTORYGROUPNUMBER
    },
    {
      UnitSortAndFilter.SORT_TYPES.UnitID,
      Consts.GetInstance().SORT_POPUP_LABEL_UNITID
    },
    {
      UnitSortAndFilter.SORT_TYPES.Trust,
      "親愛度"
    },
    {
      UnitSortAndFilter.SORT_TYPES.MaxLevel,
      Consts.GetInstance().SORT_POPUP_LABEL_LEVEL
    },
    {
      UnitSortAndFilter.SORT_TYPES.PossessionNumber,
      Consts.GetInstance().SORT_POPUP_LABEL_POSSESSION_NUMBER
    }
  };
  public static readonly Dictionary<UnitSortAndFilter.FILTER_CATEGORIES, List<UnitSortAndFilter.FILTER_TYPES>> FilterCategories = new Dictionary<UnitSortAndFilter.FILTER_CATEGORIES, List<UnitSortAndFilter.FILTER_TYPES>>()
  {
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Weapon,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.WeaponSword,
        UnitSortAndFilter.FILTER_TYPES.WeaponAxe,
        UnitSortAndFilter.FILTER_TYPES.WeaponSpear,
        UnitSortAndFilter.FILTER_TYPES.WeaponBow,
        UnitSortAndFilter.FILTER_TYPES.WeaponGun,
        UnitSortAndFilter.FILTER_TYPES.WeaponRod,
        UnitSortAndFilter.FILTER_TYPES.WeaponUnique
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Element,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.ElementNone,
        UnitSortAndFilter.FILTER_TYPES.ElementFire,
        UnitSortAndFilter.FILTER_TYPES.ElementIce,
        UnitSortAndFilter.FILTER_TYPES.ElementWind,
        UnitSortAndFilter.FILTER_TYPES.ElementThunder,
        UnitSortAndFilter.FILTER_TYPES.ElementLight,
        UnitSortAndFilter.FILTER_TYPES.ElementDark
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.UnitType,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.UnitTypeOuki,
        UnitSortAndFilter.FILTER_TYPES.UnitTypeKouki,
        UnitSortAndFilter.FILTER_TYPES.UnitTypeMaki,
        UnitSortAndFilter.FILTER_TYPES.UnitTypeMeiki,
        UnitSortAndFilter.FILTER_TYPES.UnitTypeSyouki,
        UnitSortAndFilter.FILTER_TYPES.UnitTypeSyuki
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Rarity,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.Rare1,
        UnitSortAndFilter.FILTER_TYPES.Rare2,
        UnitSortAndFilter.FILTER_TYPES.Rare3,
        UnitSortAndFilter.FILTER_TYPES.Rare4,
        UnitSortAndFilter.FILTER_TYPES.Rare5,
        UnitSortAndFilter.FILTER_TYPES.Rare6
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Unit,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.Unit,
        UnitSortAndFilter.FILTER_TYPES.EvolutionUnit,
        UnitSortAndFilter.FILTER_TYPES.ComposeUnit,
        UnitSortAndFilter.FILTER_TYPES.TransmigrationUnit
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Level,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.LevelMax,
        UnitSortAndFilter.FILTER_TYPES.OtherLevelMax
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Equipment,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.Equipment,
        UnitSortAndFilter.FILTER_TYPES.NonEquipment
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Compose,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.NonCompose,
        UnitSortAndFilter.FILTER_TYPES.Compose,
        UnitSortAndFilter.FILTER_TYPES.ComposeComplete
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Favorite,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.Favorite,
        UnitSortAndFilter.FILTER_TYPES.NoFavorite
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Tower,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.Tower,
        UnitSortAndFilter.FILTER_TYPES.NoTower
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.AttackType,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.AttackTypeNone,
        UnitSortAndFilter.FILTER_TYPES.AttackTypeSlashing,
        UnitSortAndFilter.FILTER_TYPES.AttackTypeBlow,
        UnitSortAndFilter.FILTER_TYPES.AttackTypePiercing,
        UnitSortAndFilter.FILTER_TYPES.AttackTypeShooting,
        UnitSortAndFilter.FILTER_TYPES.AttackTypeMagic
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.Awake,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.AwakeNormal,
        UnitSortAndFilter.FILTER_TYPES.AwakeTarget,
        UnitSortAndFilter.FILTER_TYPES.AwakePossible,
        UnitSortAndFilter.FILTER_TYPES.AwakeComplete
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.ClassChange,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.ClassChangeNormal,
        UnitSortAndFilter.FILTER_TYPES.ClassChangeVertex1,
        UnitSortAndFilter.FILTER_TYPES.ClassChangeVertex2,
        UnitSortAndFilter.FILTER_TYPES.ClassChangeVertex3
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.UpperAttribute,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.UpperAttributeOn,
        UnitSortAndFilter.FILTER_TYPES.NoUpperAttributeOff
      }
    },
    {
      UnitSortAndFilter.FILTER_CATEGORIES.CallSelect,
      new List<UnitSortAndFilter.FILTER_TYPES>()
      {
        UnitSortAndFilter.FILTER_TYPES.CallDone,
        UnitSortAndFilter.FILTER_TYPES.NotCall
      }
    }
  };
  private SortAndFilter.SORT_TYPE_ORDER_BUY orderBuySort;
  private bool isBattleFirst;
  private bool isTowerEntry = true;
  [SerializeField]
  private GameObject[] ListObject;
  [SerializeField]
  private UnitSortAndFilterButton[] OrderBtn;
  [SerializeField]
  private UnitSortAndFilterTabButton[] ListBtns;
  [SerializeField]
  private UnitSortAndFilterButton[] SortBtns;
  [SerializeField]
  private UnitSortAndFilterButton[] FilterBtns;
  [SerializeField]
  private UIGrid groupLargeGrid;
  [SerializeField]
  private Transform groupLargeTitle;
  [SerializeField]
  private UIGrid groupSmallGrid;
  [SerializeField]
  private Transform groupSmallTitle;
  [SerializeField]
  private UIGrid groupClothingGrid;
  [SerializeField]
  private Transform groupClothingTitle;
  [SerializeField]
  private UIGrid groupGenerationGrid;
  [SerializeField]
  private Transform groupGenerationTitle;
  [SerializeField]
  private UIScrollView groupButtonScrollView;
  [SerializeField]
  private GameObject[] DisplayList;
  [SerializeField]
  private UILabel unitFilterNum;
  [SerializeField]
  private UILabel unitGroupNum;
  [SerializeField]
  private SpreadColorButton battleFirstBtn;
  [SerializeField]
  private SpreadColorButton towerEntryFirseBtn;
  private readonly float[] ListPositionY = new float[3]
  {
    -48f,
    -118f,
    -188f
  };
  private bool isNormalUnitOnly;
  private bool isFriendSupport;
  private UnitSortAndFilter.ModeTypes modeType;
  private UnitSortAndFilter.SORT_TYPES sortCategory = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
  private bool[] filter = new bool[60];
  private Dictionary<UnitGroupHead, List<int>> selectedGroupIDs;
  private CommonElement friendSupportCurrentSelectElement = CommonElement.none;
  private Persist<Persist.UnitSortAndFilterInfo> persist;
  private Action<SortInfo> SortActionExt;
  public Action<SortInfo> SortFilterUnitNum;
  private UnitMenuBase menu;

  private void SetOrderTypeBtn()
  {
    if (this.orderBuySort == SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING)
    {
      this.OrderBtn[1].SpriteColorGray(true);
      this.OrderBtn[1].TextColorGray(true);
      this.OrderBtn[0].SpriteColorGray(false);
      this.OrderBtn[0].TextColorGray(false);
    }
    else
    {
      this.OrderBtn[1].SpriteColorGray(false);
      this.OrderBtn[1].TextColorGray(false);
      this.OrderBtn[0].SpriteColorGray(true);
      this.OrderBtn[0].TextColorGray(true);
    }
  }

  private void SetListTypeBtn(UnitSortAndFilter.ModeTypes mode)
  {
    this.modeType = mode;
    ((IEnumerable<GameObject>) this.ListObject).ToggleOnce((int) mode);
    switch (mode)
    {
      case UnitSortAndFilter.ModeTypes.Sort:
        this.ListBtns[0].SpriteColorGray(true);
        this.ListBtns[0].TextColorGray(true);
        this.ListBtns[1].SpriteColorGray(false);
        this.ListBtns[1].TextColorGray(false);
        this.ListBtns[2].SpriteColorGray(false);
        this.ListBtns[2].TextColorGray(false);
        break;
      case UnitSortAndFilter.ModeTypes.Filter:
        this.ListBtns[0].SpriteColorGray(false);
        this.ListBtns[0].TextColorGray(false);
        this.ListBtns[1].SpriteColorGray(true);
        this.ListBtns[1].TextColorGray(true);
        this.ListBtns[2].SpriteColorGray(false);
        this.ListBtns[2].TextColorGray(false);
        break;
      case UnitSortAndFilter.ModeTypes.Group:
        this.ListBtns[0].SpriteColorGray(false);
        this.ListBtns[0].TextColorGray(false);
        this.ListBtns[1].SpriteColorGray(false);
        this.ListBtns[1].TextColorGray(false);
        this.ListBtns[2].SpriteColorGray(true);
        this.ListBtns[2].TextColorGray(true);
        break;
    }
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
    this.SetGroupTabLabel();
  }

  private GameObject CreateGroupBtn(
    GameObject prefab,
    UnitSortAndFilter menu,
    UnitGroupHead type,
    int id,
    string title,
    string spriteName)
  {
    GameObject groupBtn = Object.Instantiate<GameObject>(prefab);
    groupBtn.GetComponent<UnitSortAndFilterGroupButton>().Init(menu, type, id, title, spriteName);
    return groupBtn;
  }

  private IEnumerator CreateGroupList()
  {
    UnitSortAndFilter menu = this;
    Future<GameObject> ibtnPopupGroupPrefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/unit004_6/ibtn_Popup_Group");
    IEnumerator e = ibtnPopupGroupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = ibtnPopupGroupPrefabF.Result;
    IEnumerable<UnitGroupLargeCategory> groupLargeCategories = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).Where<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    if (groupLargeCategories != null)
    {
      foreach (UnitGroupLargeCategory groupLargeCategory in groupLargeCategories)
      {
        if (!string.IsNullOrEmpty(groupLargeCategory.name))
        {
          GameObject groupBtn = menu.CreateGroupBtn(result, menu, UnitGroupHead.group_large, groupLargeCategory.ID, groupLargeCategory.name, groupLargeCategory.GetSpriteName());
          groupBtn.transform.SetParent(((Component) menu.groupLargeGrid).transform);
          groupBtn.transform.localScale = Vector3.one;
          groupBtn.transform.localPosition = Vector3.zero;
        }
      }
    }
    IEnumerable<UnitGroupSmallCategory> groupSmallCategories = ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).Where<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    if (groupSmallCategories != null)
    {
      foreach (UnitGroupSmallCategory groupSmallCategory in groupSmallCategories)
      {
        if (!string.IsNullOrEmpty(groupSmallCategory.name))
        {
          GameObject groupBtn = menu.CreateGroupBtn(result, menu, UnitGroupHead.group_small, groupSmallCategory.ID, groupSmallCategory.name, groupSmallCategory.GetSpriteName());
          groupBtn.transform.SetParent(((Component) menu.groupSmallGrid).transform);
          groupBtn.transform.localScale = Vector3.one;
          groupBtn.transform.localPosition = Vector3.zero;
        }
      }
    }
    IEnumerable<UnitGroupClothingCategory> clothingCategories = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).Where<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    if (clothingCategories != null)
    {
      foreach (UnitGroupClothingCategory clothingCategory in clothingCategories)
      {
        if (!string.IsNullOrEmpty(clothingCategory.name))
        {
          GameObject groupBtn = menu.CreateGroupBtn(result, menu, UnitGroupHead.group_clothing, clothingCategory.ID, clothingCategory.name, clothingCategory.GetSpriteName());
          groupBtn.transform.SetParent(((Component) menu.groupClothingGrid).transform);
          groupBtn.transform.localScale = Vector3.one;
          groupBtn.transform.localPosition = Vector3.zero;
        }
      }
    }
    IEnumerable<UnitGroupGenerationCategory> generationCategories = ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).Where<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    }));
    if (generationCategories != null)
    {
      foreach (UnitGroupGenerationCategory generationCategory in generationCategories)
      {
        if (!string.IsNullOrEmpty(generationCategory.name))
        {
          GameObject groupBtn = menu.CreateGroupBtn(result, menu, UnitGroupHead.group_generation, generationCategory.ID, generationCategory.name, generationCategory.GetSpriteName());
          groupBtn.transform.SetParent(((Component) menu.groupGenerationGrid).transform);
          groupBtn.transform.localScale = Vector3.one;
          groupBtn.transform.localPosition = Vector3.zero;
        }
      }
    }
    menu.RemoveIllegalGroupID();
    yield return (object) menu.StartCoroutine(menu.AdjustGroupButtonPosition());
  }

  private void RemoveIllegalGroupID()
  {
    this.selectedGroupIDs[UnitGroupHead.group_large].Where<int>((Func<int, bool>) (x => !this.menu.AllGroupIDs[UnitGroupHead.group_large].Contains(x))).ForEach<int>((Action<int>) (x => this.selectedGroupIDs[UnitGroupHead.group_large].Remove(x)));
    this.selectedGroupIDs[UnitGroupHead.group_small].Where<int>((Func<int, bool>) (x => !this.menu.AllGroupIDs[UnitGroupHead.group_small].Contains(x))).ForEach<int>((Action<int>) (x => this.selectedGroupIDs[UnitGroupHead.group_small].Remove(x)));
    this.selectedGroupIDs[UnitGroupHead.group_clothing].Where<int>((Func<int, bool>) (x => !this.menu.AllGroupIDs[UnitGroupHead.group_clothing].Contains(x))).ForEach<int>((Action<int>) (x => this.selectedGroupIDs[UnitGroupHead.group_clothing].Remove(x)));
    this.selectedGroupIDs[UnitGroupHead.group_generation].Where<int>((Func<int, bool>) (x => !this.menu.AllGroupIDs[UnitGroupHead.group_generation].Contains(x))).ForEach<int>((Action<int>) (x => this.selectedGroupIDs[UnitGroupHead.group_generation].Remove(x)));
  }

  private IEnumerator AdjustGroupButtonPosition()
  {
    this.groupLargeGrid.Reposition();
    this.groupSmallGrid.Reposition();
    this.groupClothingGrid.Reposition();
    this.groupGenerationGrid.Reposition();
    yield return (object) new WaitForSeconds(0.35f);
    float num1 = 0.0f;
    float num2 = 10f;
    this.groupLargeTitle.localPosition = new Vector3(this.groupLargeTitle.localPosition.x, num1, 0.0f);
    double num3 = (double) num1;
    Bounds relativeWidgetBounds1 = NGUIMath.CalculateRelativeWidgetBounds(this.groupLargeTitle);
    double num4 = (double) ((Bounds) ref relativeWidgetBounds1).size.y + 12.0;
    float num5 = (float) (num3 - num4);
    ((Component) this.groupLargeGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupLargeGrid), num5 - num2, 0.0f);
    double num6 = (double) num5;
    Bounds relativeWidgetBounds2 = NGUIMath.CalculateRelativeWidgetBounds(((Component) this.groupLargeGrid).transform);
    double y1 = (double) ((Bounds) ref relativeWidgetBounds2).size.y;
    float num7 = (float) (num6 - y1);
    this.groupSmallTitle.localPosition = new Vector3(this.groupSmallTitle.localPosition.x, num7, 0.0f);
    double num8 = (double) num7;
    Bounds relativeWidgetBounds3 = NGUIMath.CalculateRelativeWidgetBounds(this.groupSmallTitle);
    double y2 = (double) ((Bounds) ref relativeWidgetBounds3).size.y;
    float num9 = (float) (num8 - y2);
    ((Component) this.groupSmallGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupSmallGrid), num9 - num2, 0.0f);
    double num10 = (double) num9;
    Bounds relativeWidgetBounds4 = NGUIMath.CalculateRelativeWidgetBounds(((Component) this.groupSmallGrid).transform);
    double y3 = (double) ((Bounds) ref relativeWidgetBounds4).size.y;
    float num11 = (float) (num10 - y3);
    this.groupClothingTitle.localPosition = new Vector3(this.groupClothingTitle.localPosition.x, num11, 0.0f);
    double num12 = (double) num11;
    Bounds relativeWidgetBounds5 = NGUIMath.CalculateRelativeWidgetBounds(this.groupClothingTitle);
    double y4 = (double) ((Bounds) ref relativeWidgetBounds5).size.y;
    float num13 = (float) (num12 - y4);
    ((Component) this.groupClothingGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupClothingGrid), num13 - num2, 0.0f);
    double num14 = (double) num13;
    Bounds relativeWidgetBounds6 = NGUIMath.CalculateRelativeWidgetBounds(((Component) this.groupClothingGrid).transform);
    double y5 = (double) ((Bounds) ref relativeWidgetBounds6).size.y;
    float num15 = (float) (num14 - y5);
    this.groupGenerationTitle.localPosition = new Vector3(this.groupGenerationTitle.localPosition.x, num15, 0.0f);
    double num16 = (double) num15;
    Bounds relativeWidgetBounds7 = NGUIMath.CalculateRelativeWidgetBounds(this.groupGenerationTitle);
    double y6 = (double) ((Bounds) ref relativeWidgetBounds7).size.y;
    float num17 = (float) (num16 - y6);
    ((Component) this.groupGenerationGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupGenerationGrid), num17 - num2, 0.0f);
    this.groupButtonScrollView.ResetPosition();
  }

  private float GetGridPositionX(UIGrid grid)
  {
    float gridPositionX = 0.0f;
    if (((Component) grid).transform.childCount < 4)
      gridPositionX = (float) ((double) -(4 - ((Component) grid).transform.childCount) * (double) grid.cellWidth * 0.5);
    return gridPositionX;
  }

  public IEnumerator Initialize(
    Action<SortInfo> SortAction,
    Persist<Persist.UnitSortAndFilterInfo> persistData,
    UnitMenuBase menu,
    bool normalUnitOnly = false,
    bool isFriendSupport = false)
  {
    this.menu = menu;
    this.isNormalUnitOnly = normalUnitOnly;
    this.isFriendSupport = isFriendSupport;
    this.persist = persistData;
    this.sortCategory = this.persist.Data.sortType;
    this.orderBuySort = this.persist.Data.order;
    this.isBattleFirst = this.persist.Data.isBattleFirst;
    this.isTowerEntry = this.persist.Data.isTowerEntry;
    this.selectedGroupIDs = this.persist.Data.groupIDs.ToDictionary<KeyValuePair<UnitGroupHead, List<int>>, UnitGroupHead, List<int>>((Func<KeyValuePair<UnitGroupHead, List<int>>, UnitGroupHead>) (entry => entry.Key), (Func<KeyValuePair<UnitGroupHead, List<int>>, List<int>>) (entry => entry.Value.ToList<int>()));
    this.SetOrderTypeBtn();
    UnitSortAndFilter.FILTER_TYPES[] ldtypes = new UnitSortAndFilter.FILTER_TYPES[2]
    {
      UnitSortAndFilter.FILTER_TYPES.ElementLight,
      UnitSortAndFilter.FILTER_TYPES.ElementDark
    };
    ((IEnumerable<UnitSortAndFilterButton>) this.FilterBtns).Where<UnitSortAndFilterButton>((Func<UnitSortAndFilterButton, bool>) (x => ((IEnumerable<UnitSortAndFilter.FILTER_TYPES>) ldtypes).Contains<UnitSortAndFilter.FILTER_TYPES>(x.FilterType))).Select<UnitSortAndFilterButton, GameObject>((Func<UnitSortAndFilterButton, GameObject>) (y => ((Component) y).gameObject)).SetActives(Player.Current.IsEnableDarkAndHoly());
    List<bool> filter = this.persist.Data.filter;
    int count = filter.Count;
    for (int index = 0; index < 60; ++index)
      this.filter[index] = index < count && filter[index];
    IEnumerator e = this.CreateGroupList();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SortActionExt = SortAction;
    this.SetValueWindow();
    this.SetListTypeBtn(this.persist.Data.modeType);
    if (Singleton<NGSceneManager>.GetInstance().sceneName != "unit004_8_6")
    {
      ((Component) this.battleFirstBtn).gameObject.SetActive(true);
      ((Component) this.towerEntryFirseBtn).gameObject.SetActive(true);
      this.battleFirstBtn.SetColor(this.isBattleFirst ? Color.white : Color.gray);
      this.towerEntryFirseBtn.SetColor(this.isTowerEntry ? Color.white : Color.gray);
      this.battleFirstBtn.onClick.Clear();
      this.towerEntryFirseBtn.onClick.Clear();
      this.battleFirstBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
      {
        this.isBattleFirst = !this.isBattleFirst;
        this.battleFirstBtn.SetColor(this.isBattleFirst ? Color.white : Color.gray);
      })));
      this.towerEntryFirseBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
      {
        this.isTowerEntry = !this.isTowerEntry;
        this.towerEntryFirseBtn.SetColor(this.isTowerEntry ? Color.white : Color.gray);
      })));
    }
    else
    {
      ((Component) this.battleFirstBtn).gameObject.SetActive(false);
      ((Component) this.towerEntryFirseBtn).gameObject.SetActive(false);
    }
  }

  private void SetSortTabLabel()
  {
    if (this.SortLabelStr.ContainsKey(this.sortCategory))
      this.ListBtns[0].SetText(this.SortLabelStr[this.sortCategory]);
    if (this.modeType == UnitSortAndFilter.ModeTypes.Sort)
      this.ListBtns[0].SetTextColor(new Color(1f, 1f, 0.0f));
    else
      this.ListBtns[0].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
  }

  private void SetFilterTabLabel()
  {
    bool flag = false;
    for (int index = 0; index < 60; ++index)
    {
      if (this.filter[index])
      {
        flag = true;
        break;
      }
    }
    this.ListBtns[1].SetText(flag ? Consts.GetInstance().SORT_POPUP_LABEL_FILTER_ON : Consts.GetInstance().SORT_POPUP_LABEL_FILTER_OFF);
    if (this.modeType == UnitSortAndFilter.ModeTypes.Filter)
    {
      if (flag)
        this.ListBtns[1].SetTextColor(new Color(1f, 1f, 0.0f));
      else
        this.ListBtns[1].SetTextColor(new Color(1f, 1f, 1f));
      if (this.SortFilterUnitNum == null)
        return;
      this.SortFilterUnitNum(new SortInfo(this.sortCategory, this.orderBuySort, this.filter, this.selectedGroupIDs, this.isBattleFirst, this.isTowerEntry));
    }
    else if (flag)
      this.ListBtns[1].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
    else
      this.ListBtns[1].SetTextColor(new Color(0.5f, 0.5f, 0.5f));
  }

  private void SetGroupTabLabel()
  {
    string title = "";
    List<int> selectedGroupId = this.selectedGroupIDs[UnitGroupHead.group_all];
    List<int> groupLargeIDs = this.selectedGroupIDs[UnitGroupHead.group_large];
    List<int> groupSmallIDs = this.selectedGroupIDs[UnitGroupHead.group_small];
    List<int> groupClothingIDs = this.selectedGroupIDs[UnitGroupHead.group_clothing];
    List<int> groupGenerationIDs = this.selectedGroupIDs[UnitGroupHead.group_generation];
    if (selectedGroupId.Contains(1))
      title = Consts.GetInstance().SORT_POPUP_LABEL_GROUP_ALL_TAB;
    else if (selectedGroupId.Contains(2))
      title = Consts.GetInstance().SORT_POPUP_LABEL_FILTER_OFF;
    else if (groupLargeIDs.Count == 1 && groupSmallIDs.Count == 0 && groupClothingIDs.Count == 0 && groupGenerationIDs.Count == 0)
    {
      UnitGroupLargeCategory groupLargeCategory = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).FirstOrDefault<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x => x.ID == groupLargeIDs[0]));
      if (groupLargeCategory != null)
        title = groupLargeCategory.short_label_name;
    }
    else if (groupLargeIDs.Count == 0 && groupSmallIDs.Count == 1 && groupClothingIDs.Count == 0 && groupGenerationIDs.Count == 0)
    {
      UnitGroupSmallCategory groupSmallCategory = ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).FirstOrDefault<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x => x.ID == groupSmallIDs[0]));
      if (groupSmallCategory != null)
        title = groupSmallCategory.short_label_name;
    }
    else if (groupLargeIDs.Count == 0 && groupSmallIDs.Count == 0 && groupClothingIDs.Count == 1 && groupGenerationIDs.Count == 0)
    {
      UnitGroupClothingCategory clothingCategory = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).FirstOrDefault<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x => x.ID == groupClothingIDs[0]));
      if (clothingCategory != null)
        title = clothingCategory.short_label_name;
    }
    else if (groupLargeIDs.Count == 0 && groupSmallIDs.Count == 0 && groupClothingIDs.Count == 0 && groupGenerationIDs.Count == 1)
    {
      UnitGroupGenerationCategory generationCategory = ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).FirstOrDefault<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x => x.ID == groupGenerationIDs[0]));
      if (generationCategory != null)
        title = generationCategory.short_label_name;
    }
    else
      title = Consts.GetInstance().SORT_POPUP_LABEL_GROUP_COMPLEX_TAB;
    this.ListBtns[2].SetText(title);
    if (this.modeType == UnitSortAndFilter.ModeTypes.Group)
    {
      if (selectedGroupId.Contains(2))
        this.ListBtns[2].SetTextColor(new Color(1f, 1f, 1f));
      else
        this.ListBtns[2].SetTextColor(new Color(1f, 1f, 0.0f));
      if (this.SortFilterUnitNum == null)
        return;
      this.SortFilterUnitNum(new SortInfo(this.sortCategory, this.orderBuySort, this.filter, this.selectedGroupIDs, this.isBattleFirst, this.isTowerEntry));
    }
    else if (selectedGroupId.Count > 0)
      this.ListBtns[2].SetTextColor(new Color(0.5f, 0.5f, 0.5f));
    else
      this.ListBtns[2].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
  }

  public void SetValueWindow()
  {
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
    this.SetGroupTabLabel();
    foreach (UnitSortAndFilterButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == this.sortCategory)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
    foreach (UnitSortAndFilterButton filterBtn in this.FilterBtns)
    {
      if (!Object.op_Equality((Object) filterBtn, (Object) null))
      {
        filterBtn.SpriteColorGray(false);
        filterBtn.TextColorGray(false);
        if (this.filter[(int) filterBtn.FilterType])
        {
          filterBtn.SpriteColorGray(true);
          filterBtn.TextColorGray(true);
        }
      }
    }
    List<Transform> transformList = new List<Transform>();
    transformList.AddRange(((Component) this.groupLargeGrid).transform.GetChildren());
    transformList.AddRange(((Component) this.groupSmallGrid).transform.GetChildren());
    transformList.AddRange(((Component) this.groupClothingGrid).transform.GetChildren());
    transformList.AddRange(((Component) this.groupGenerationGrid).transform.GetChildren());
    if (this.selectedGroupIDs[UnitGroupHead.group_all].Contains(1))
    {
      foreach (Component component1 in transformList)
      {
        UnitSortAndFilterGroupButton component2 = component1.GetComponent<UnitSortAndFilterGroupButton>();
        component2.isSelected = true;
        component2.SpriteColorGray(true);
        component2.TextColorGray(true);
      }
    }
    else if (this.selectedGroupIDs[UnitGroupHead.group_all].Contains(2))
    {
      foreach (Component component3 in transformList)
      {
        UnitSortAndFilterGroupButton component4 = component3.GetComponent<UnitSortAndFilterGroupButton>();
        component4.SpriteColorGray(false);
        component4.TextColorGray(false);
        component4.isSelected = false;
      }
    }
    else
    {
      foreach (Component component5 in transformList)
      {
        UnitSortAndFilterGroupButton component6 = component5.GetComponent<UnitSortAndFilterGroupButton>();
        component6.SpriteColorGray(false);
        component6.TextColorGray(false);
        component6.isSelected = false;
        if (this.selectedGroupIDs[component6.GroupType].Contains(component6.GroupID))
        {
          component6.isSelected = true;
          component6.SpriteColorGray(true);
          component6.TextColorGray(true);
        }
      }
    }
  }

  public void IbtnSortBtn() => this.SetListTypeBtn(UnitSortAndFilter.ModeTypes.Sort);

  public void IbtnFilterBtn() => this.SetListTypeBtn(UnitSortAndFilter.ModeTypes.Filter);

  public void IbtnGroupBtn() => this.SetListTypeBtn(UnitSortAndFilter.ModeTypes.Group);

  public override void IbtnAllFilterSelect()
  {
    for (int index = 0; index < 60; ++index)
    {
      if (!this.isFriendSupport)
        this.filter[index] = index != 29 && index != 30 && index != 31 && index != 32 && index != 36 && index != 37 && index != 38 && index != 39 && index != 34 && index != 35 && index != 56 && index != 57 && index != 58 && index != 59;
      else if (index != 29 && index != 30 && index != 31 && index != 32 && index != 36 && index != 37 && index != 38 && index != 39 && index != 34 && index != 35 && index != 56 && index != 57 && index != 58 && index != 59)
      {
        if (this.friendSupportCurrentSelectElement != CommonElement.none)
        {
          if (index != 7 && index != 8 && index != 9 && index != 10 && index != 11 && index != 12 && index != 13)
            this.filter[index] = true;
        }
        else
          this.filter[index] = true;
      }
      else
        this.filter[index] = false;
    }
    this.SetValueWindow();
  }

  public override void IbtnFilterClear()
  {
    for (int index = 0; index < 60; ++index)
    {
      if (!this.isFriendSupport)
        this.filter[index] = false;
      else if (this.friendSupportCurrentSelectElement != CommonElement.none)
      {
        if (index != 7 && index != 8 && index != 9 && index != 10 && index != 11 && index != 12 && index != 13)
          this.filter[index] = false;
      }
      else if (index != 7 && index != 8 && index != 9 && index != 10 && index != 11 && index != 12 && index != 13)
        this.filter[index] = false;
    }
    this.SetValueWindow();
  }

  public void IbtnSelectAllGroup() => this.AddGroupInfo(UnitGroupHead.group_all, 1);

  public void IbtnClearAllGroup() => this.AddGroupInfo(UnitGroupHead.group_all, 2);

  public void SetElementType(bool b)
  {
    this.filter[7] = b;
    this.filter[8] = b;
    this.filter[9] = b;
    this.filter[10] = b;
    this.filter[11] = b;
    this.filter[12] = b;
    this.filter[13] = b;
  }

  public override void IbtnDicision()
  {
    this.SaveData();
    if (this.SortActionExt != null)
      this.SortActionExt(new SortInfo(this.sortCategory, this.orderBuySort, this.filter, this.selectedGroupIDs, this.isBattleFirst, this.isTowerEntry));
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void IbtnOrder()
  {
    this.orderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.SetOrderTypeBtn();
  }

  public override void IbtnOrderDec()
  {
    this.orderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING;
    this.SetOrderTypeBtn();
  }

  public void SetSortCategory(UnitSortAndFilter.SORT_TYPES type)
  {
    this.sortCategory = type;
    this.SetValueWindow();
  }

  public void SetFilterType(UnitSortAndFilter.FILTER_TYPES type, bool flg)
  {
    if (this.isFriendSupport && (type == UnitSortAndFilter.FILTER_TYPES.ElementNone || type == UnitSortAndFilter.FILTER_TYPES.ElementFire || type == UnitSortAndFilter.FILTER_TYPES.ElementIce || type == UnitSortAndFilter.FILTER_TYPES.ElementWind || type == UnitSortAndFilter.FILTER_TYPES.ElementThunder || type == UnitSortAndFilter.FILTER_TYPES.ElementLight || type == UnitSortAndFilter.FILTER_TYPES.ElementDark) && this.friendSupportCurrentSelectElement != CommonElement.none)
      return;
    this.filter[(int) type] = flg;
    this.SetValueWindow();
  }

  public void AddGroupInfo(UnitGroupHead gType, int gID)
  {
    this.selectedGroupIDs[UnitGroupHead.group_all].Clear();
    List<int> selectedGroupId = this.selectedGroupIDs[gType];
    if (!selectedGroupId.Contains(gID))
      selectedGroupId.Add(gID);
    if (gType == UnitGroupHead.group_all)
    {
      switch (gID)
      {
        case 1:
          this.selectedGroupIDs[UnitGroupHead.group_large] = new List<int>((IEnumerable<int>) this.menu.AllGroupIDs[UnitGroupHead.group_large]);
          this.selectedGroupIDs[UnitGroupHead.group_small] = new List<int>((IEnumerable<int>) this.menu.AllGroupIDs[UnitGroupHead.group_small]);
          this.selectedGroupIDs[UnitGroupHead.group_clothing] = new List<int>((IEnumerable<int>) this.menu.AllGroupIDs[UnitGroupHead.group_clothing]);
          this.selectedGroupIDs[UnitGroupHead.group_generation] = new List<int>((IEnumerable<int>) this.menu.AllGroupIDs[UnitGroupHead.group_generation]);
          break;
        case 2:
          this.selectedGroupIDs[UnitGroupHead.group_large].Clear();
          this.selectedGroupIDs[UnitGroupHead.group_small].Clear();
          this.selectedGroupIDs[UnitGroupHead.group_clothing].Clear();
          this.selectedGroupIDs[UnitGroupHead.group_generation].Clear();
          break;
      }
    }
    else if (this.selectedGroupIDs[UnitGroupHead.group_large].Count == this.menu.AllGroupIDs[UnitGroupHead.group_large].Count && this.selectedGroupIDs[UnitGroupHead.group_small].Count == this.menu.AllGroupIDs[UnitGroupHead.group_small].Count && this.selectedGroupIDs[UnitGroupHead.group_clothing].Count == this.menu.AllGroupIDs[UnitGroupHead.group_clothing].Count && this.selectedGroupIDs[UnitGroupHead.group_generation].Count == this.menu.AllGroupIDs[UnitGroupHead.group_generation].Count)
    {
      this.AddGroupInfo(UnitGroupHead.group_all, 1);
      return;
    }
    this.SetValueWindow();
  }

  public void RemoveGroupInfo(UnitGroupHead gType, int gID)
  {
    this.selectedGroupIDs[UnitGroupHead.group_all].Clear();
    List<int> selectedGroupId = this.selectedGroupIDs[gType];
    if (selectedGroupId.Contains(gID))
      selectedGroupId.Remove(gID);
    if (this.selectedGroupIDs[UnitGroupHead.group_large].Count == 0 && this.selectedGroupIDs[UnitGroupHead.group_small].Count == 0 && this.selectedGroupIDs[UnitGroupHead.group_clothing].Count == 0 && this.selectedGroupIDs[UnitGroupHead.group_generation].Count == 0)
      this.AddGroupInfo(UnitGroupHead.group_all, 2);
    else
      this.SetValueWindow();
  }

  public void SetFriendSupportCurrentElement(CommonElement type, bool isFriend)
  {
    this.friendSupportCurrentSelectElement = type;
    this.isFriendSupport = isFriend;
  }

  public void SetUnitNum(List<UnitIconInfo> displayList, List<UnitIconInfo> allList)
  {
    int count1 = displayList.Count;
    int count2 = allList.Count;
    foreach (UnitIconInfo display in displayList)
    {
      if (display.removeButton)
      {
        --count1;
        --count2;
        break;
      }
    }
    string str = count1 <= 0 ? "[FF0000]" : "[FFFE27]";
    if (this.modeType == UnitSortAndFilter.ModeTypes.Filter)
    {
      this.unitFilterNum.SetText(str + (object) count1 + "[-]/" + (object) count2);
    }
    else
    {
      if (this.modeType != UnitSortAndFilter.ModeTypes.Group)
        return;
      this.unitGroupNum.SetText(str + (object) count1 + "[-]/" + (object) count2);
    }
  }

  public override void SaveData()
  {
    this.persist.Data.order = this.orderBuySort;
    this.persist.Data.sortType = this.sortCategory;
    this.persist.Data.modeType = this.modeType;
    this.persist.Data.groupIDs = this.selectedGroupIDs;
    this.persist.Data.isBattleFirst = this.isBattleFirst;
    this.persist.Data.isTowerEntry = this.isTowerEntry;
    for (int index = 0; index < 60; ++index)
    {
      if (index < this.persist.Data.filter.Count)
        this.persist.Data.filter[index] = this.filter[index];
      else
        this.persist.Data.filter.Add(this.filter[index]);
    }
    this.persist.Flush();
  }

  public enum ModeTypes
  {
    Sort,
    Filter,
    Group,
  }

  public enum SORT_TYPES
  {
    None,
    BranchOfAnArmy,
    Attribute,
    Level,
    Rarity,
    Cost,
    FightingPower,
    PhysicalAttack,
    MagicAttack,
    Defence,
    MagicDefence,
    Hit,
    Critical,
    Avoid,
    Speed,
    Dexterity,
    Luck,
    WeaponProficiency,
    ArmorProficiency,
    GetOrder,
    HP,
    Breakthrough,
    UnityValue,
    TrustRate,
    AverageRisingValue,
    UnitName,
    Illustrator,
    VoiceActor,
    HistoryGroupNumber,
    Trust,
    UnitID,
    MaxLevel,
    PossessionNumber,
  }

  public enum FILTER_TYPES
  {
    None,
    WeaponSword,
    WeaponAxe,
    WeaponSpear,
    WeaponBow,
    WeaponGun,
    WeaponRod,
    ElementNone,
    ElementFire,
    ElementIce,
    ElementWind,
    ElementThunder,
    ElementLight,
    ElementDark,
    UnitTypeOuki,
    UnitTypeMeiki,
    UnitTypeKouki,
    UnitTypeMaki,
    UnitTypeSyuki,
    UnitTypeSyouki,
    Rare1,
    Rare2,
    Rare3,
    Rare4,
    Rare5,
    Unit,
    EvolutionUnit,
    ComposeUnit,
    TransmigrationUnit,
    LevelMax,
    OtherLevelMax,
    Equipment,
    NonEquipment,
    NonCompose,
    Compose,
    ComposeComplete,
    Favorite,
    NoFavorite,
    Tower,
    NoTower,
    Rare6,
    AttackTypeNone,
    AttackTypeSlashing,
    AttackTypeBlow,
    AttackTypePiercing,
    AttackTypeShooting,
    AttackTypeMagic,
    AwakeNormal,
    AwakeTarget,
    AwakePossible,
    AwakeComplete,
    ClassChangeNormal,
    ClassChangeVertex1,
    ClassChangeVertex2,
    ClassChangeVertex3,
    WeaponUnique,
    UpperAttributeOn,
    NoUpperAttributeOff,
    CallDone,
    NotCall,
    Max,
  }

  public enum FILTER_CATEGORIES
  {
    Weapon,
    Element,
    UnitType,
    Rarity,
    Unit,
    Level,
    Equipment,
    Compose,
    Favorite,
    Tower,
    Bust,
    AttackType,
    Awake,
    ClassChange,
    UpperAttribute,
    CallSelect,
  }
}
