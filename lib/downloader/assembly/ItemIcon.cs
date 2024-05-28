// Decompiled with JetBrains decompiler
// Type: ItemIcon
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
using UnityEngine;

#nullable disable
public class ItemIcon : IconPrefabBase
{
  public static readonly int Width = 123;
  public static readonly int Height = 147;
  public static readonly int ColumnValue = 5;
  public static readonly int RowValue = 8;
  public static readonly int RowScreenValue = 5;
  public static readonly int ScreenValue = ItemIcon.ColumnValue * ItemIcon.RowScreenValue;
  public static readonly int MaxValue = ItemIcon.ColumnValue * ItemIcon.RowValue;
  private static readonly int SelectedIndexFontSize = 22;
  private static readonly int ManaSeedsDurabilityFontSize = 28;
  private static Dictionary<int, Sprite> gearCache = new Dictionary<int, Sprite>();
  private static Dictionary<int, Sprite> supplyCache = new Dictionary<int, Sprite>();
  public static bool IsPoolCache;
  private static GameObject elementIconPrefab;
  private static GameObject reisouEffect01Prefab;
  private static GameObject reisouEffect02Prefab;
  private static GameObject reisouEffect03Prefab;
  private static GameObject buguReisouEffect01Prefab;
  private static GameObject buguReisouEffect02Prefab;
  private static GameObject buguReisouEffect03Prefab;
  private static GameObject reisouPopupDualSkillPrefab;
  private static GameObject reisouPopupPrefab;
  private GameCore.ItemInfo itemInfo;
  public GameObject removeButton;
  public Sprite[] backSprite;
  public Sprite[] backSpriteSpecificationOfEquipmentUnits;
  public Sprite backSpriteWeaponMaterial;
  public Sprite backSpriteReisou;
  public Sprite nonBackSprite;
  public Sprite nonBackSpriteByJingi_;
  public Sprite[] raritySprite;
  public Sprite nonTypeSprite;
  public ItemIcon.SpriteArray rankSprite;
  public Sprite[] rankNumSprite;
  public Sprite[] selectNumSprite;
  public Sprite[] numberSprite;
  public Sprite[] gearUnlimitSprite;
  public Sprite[] limitRankSprite;
  public Sprite[] holyReisouRankNumSprite;
  public Sprite[] chaosReisouRankNumSprite;
  private ItemSortAndFilter.SORT_TYPES currSort;
  public ItemIcon.Gear gear;
  public ItemIcon.Supply supply;
  public GameObject quantity;
  [SerializeField]
  private GameObject m_crossForOneDigitCount;
  [SerializeField]
  private GameObject m_crossForTwoDigitsCount;
  [SerializeField]
  private GameObject m_crossForThreeDigitsCount;
  [SerializeField]
  private GameObject m_crossForFourDigitsCount;
  [SerializeField]
  private GameObject[] m_onesDigit;
  [SerializeField]
  private GameObject[] m_tensDigit;
  [SerializeField]
  private GameObject[] m_hundredsDigit;
  [SerializeField]
  private GameObject[] m_thousandsDigit;
  public GameObject quantity_bonus;
  [SerializeField]
  private GameObject m_crossForOneDigitCount_bonus;
  [SerializeField]
  private GameObject m_crossForTwoDigitsCount_bonus;
  [SerializeField]
  private GameObject m_crossForThreeDigitsCount_bonus;
  [SerializeField]
  private GameObject m_crossForFourDigitsCount_bonus;
  [SerializeField]
  private GameObject[] m_onesDigit_bonus;
  [SerializeField]
  private GameObject[] m_tensDigit_bonus;
  [SerializeField]
  private GameObject[] m_hundredsDigit_bonus;
  [SerializeField]
  private GameObject[] m_thousandsDigit_bonus;
  public GameObject selectQuantity;
  public GameObject[] selectedLeftNum;
  public GameObject[] selectedRightNum;
  public GameObject checkmark;
  [SerializeField]
  private GameObject maxUpMark;
  public GameObject renseiParent;
  public GameObject renseiNum;
  public GameObject renseiJyougen;
  public GameObject renseiBG;
  public GameObject renseiMark;
  public UI2DSprite[] renseiMeterialNum;
  public Sprite[] renseiNumSprites;
  private bool selected;
  private Action<ItemIcon> onClick_;
  private Action<ItemIcon> longPress_;
  [SerializeField]
  private ItemIcon.BottomMode bottomMode = ItemIcon.BottomMode.Visible;
  private bool enabledExpireDate_;
  private bool isReisouPopupOpen;
  private const int MAX_REMAINDAYS = 99;

  public GameCore.ItemInfo ItemInfo => this.itemInfo;

  public override bool Gray
  {
    get => this.gray;
    set
    {
      if (this.gray == value)
        return;
      this.gray = value;
      NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, !value);
    }
  }

  public bool EnabledGear => this.gear.root.activeSelf;

  public bool EnabledSupply => this.supply.root.activeSelf;

  public void SetModeGear()
  {
    this.gear.root.SetActive(true);
    this.supply.root.SetActive(false);
  }

  public void SetModeSupply()
  {
    this.gear.root.SetActive(false);
    this.supply.root.SetActive(true);
  }

  public bool NewItem
  {
    get
    {
      return this.gear.root.activeSelf ? this.gear.newGear.activeSelf : this.supply.newSupply.activeSelf;
    }
    set
    {
      if (this.gear.root.activeSelf)
        this.gear.newGear.SetActive(value);
      else
        this.supply.newSupply.SetActive(value);
    }
  }

  public ItemIcon.BottomMode BottomModeValue
  {
    get => this.bottomMode;
    set
    {
      this.gear.bottom.SetActive(value == ItemIcon.BottomMode.Visible);
      this.gear.bottomWIconNone.SetActive(value == ItemIcon.BottomMode.Visible_wIconNone);
      this.supply.bottom.SetActive(value == ItemIcon.BottomMode.Visible || value == ItemIcon.BottomMode.Visible_wIconNone);
      this.bottomMode = value;
    }
  }

  public bool ForBattle
  {
    get => !this.EnabledGear ? this.supply.forbattle.activeSelf : this.gear.forbattle.activeSelf;
    set
    {
      if (this.EnabledGear)
        this.gear.forbattle.SetActive(value);
      else
        this.supply.forbattle.SetActive(value);
    }
  }

  public bool FusionPossible
  {
    get => this.EnabledGear && this.gear.fusionPossible.activeSelf;
    set
    {
      if (!this.EnabledGear)
        return;
      this.gear.fusionPossible.SetActive(value);
    }
  }

  public void SetupIconsBlink()
  {
    if (this.gear == null || Object.op_Equality((Object) this.gear.blinkIcons, (Object) null))
      return;
    bool forBattle = this.ForBattle;
    bool fusionPossible = this.FusionPossible;
    this.gear.blinkIcons.resetBlinks();
    this.ForBattle = forBattle;
    this.FusionPossible = fusionPossible;
    ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
    bool[] flagArray = new bool[2]
    {
      this.ForBattle,
      this.FusionPossible
    };
    int num = 0;
    foreach (bool flag in flagArray)
      num = flag ? num + 1 : num;
    if (num >= 2)
    {
      List<GameObject> blinks = new List<GameObject>();
      if (this.ForBattle)
        blinks.Add(this.gear.forbattle);
      if (this.FusionPossible)
        blinks.Add(this.gear.fusionPossible);
      this.gear.blinkIcons.resetBlinks((IEnumerable<GameObject>) blinks);
    }
    if (!this.ForBattle && !this.FusionPossible)
      return;
    ((Component) this.gear.blinkIcons).gameObject.SetActive(true);
  }

  public bool Favorite
  {
    get => !this.EnabledGear ? this.supply.favorite.activeSelf : this.gear.favorite.activeSelf;
    set
    {
      if (this.EnabledGear)
        this.gear.favorite.SetActive(value);
      else
        this.supply.favorite.SetActive(value);
    }
  }

  public bool Broken
  {
    get => !this.EnabledGear ? this.supply.forbattle.activeSelf : this.gear.forbattle.activeSelf;
    set
    {
      if (!this.EnabledGear)
        return;
      this.gear.broken.SetActive(value);
    }
  }

  public bool QuantitySupply
  {
    get => this.quantity.activeSelf;
    set => this.quantity.SetActive(value);
  }

  public bool QuantitySupplyBonus
  {
    get => this.quantity_bonus.activeSelf;
    set => this.quantity_bonus.SetActive(value);
  }

  public bool DefaltGearText
  {
    get => this.gear.defaultGearTxt.activeSelf;
    set => this.gear.defaultGearTxt.SetActive(value);
  }

  public bool SelectQuantity
  {
    get => this.selectQuantity.activeSelf;
    set => this.selectQuantity.SetActive(value);
  }

  public bool MaxUpMark
  {
    get => !Object.op_Equality((Object) this.maxUpMark, (Object) null) && this.maxUpMark.activeSelf;
    set
    {
      if (!Object.op_Inequality((Object) this.maxUpMark, (Object) null))
        return;
      this.maxUpMark.SetActive(value);
    }
  }

  public bool EnabledExpireDate
  {
    get => this.ItemInfo == null ? this.enabledExpireDate_ : this.ItemInfo.enabledExpireDate_;
    set
    {
      if (this.ItemInfo != null)
        this.ItemInfo.enabledExpireDate_ = value;
      this.enabledExpireDate_ = value;
    }
  }

  public IEnumerator InitByItemInfo(GameCore.ItemInfo info)
  {
    this.itemInfo = info;
    this.removeButton.SetActive(false);
    this.gear.sortRankMaxInfo.SetActive(false);
    this.gear.dynReisouEffect.transform.Clear();
    ((Component) this.gear.blinkIcons).gameObject.SetActive(true);
    ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
    IEnumerator e;
    if (!info.isSupply)
    {
      e = this.InitByGear(info.gear, info.GetElement());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (info.isWeaponMaterial)
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteWeaponMaterial;
      else if (info.isReisou)
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteReisou;
      if (info.isWeapon && info.gear.disappearance_type_GearDisappearanceType == 0)
      {
        this.gear.rank.SetActive(true);
        if (info.gearLevel == info.gearLevelLimit)
          this.gear.rank.GetComponent<UI2DSprite>().sprite2D = this.limitRankSprite[info.gearLevel - 1 < 0 ? 0 : info.gearLevel - 1];
        else
          this.gear.rank.GetComponent<UI2DSprite>().sprite2D = this.rankSprite[info.gearLevel - 1 < 0 ? 0 : info.gearLevel - 1];
        if (info.gearLevelUnLimit > 0)
        {
          this.gear.unlimit.SetActive(true);
          this.gear.unlimit.GetComponent<UI2DSprite>().sprite2D = this.gearUnlimitSprite[info.gearLevelUnLimit - 1];
        }
        else
          this.gear.unlimit.SetActive(false);
        this.EnableQuantity(0);
        if (this.currSort != ItemSortAndFilter.SORT_TYPES.HistoryGroupNumber)
          this.gear.sortRankMaxRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_RANK, (IDictionary) new Hashtable()
          {
            {
              (object) "now",
              (object) info.gearLevel
            },
            {
              (object) "max",
              (object) info.gearLevelLimit
            }
          }));
        else
          this.gear.sortRankMaxRank.SetTextLocalize(this.itemInfo.gear.history_group_number.ToString().PadLeft(4, '0'));
      }
      else if (info.isWeapon)
      {
        this.gear.rank.SetActive(false);
        ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
        this.gear.unlimit.SetActive(false);
        this.EnableQuantity(0);
        this.setActiveObject((Component) this.gear.attackClass, false);
      }
      else if (info.isReisou)
      {
        this.setReisouRank(info);
      }
      else
      {
        this.gear.rank.SetActive(false);
        ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
        ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
        this.gear.unlimit.SetActive(false);
        this.EnableQuantity(info.quantity);
        if (!info.isWeaponMaterial)
          this.setActiveObject((Component) this.gear.attackClass, false);
      }
      this.gear.favorite.SetActive(info.favorite);
      this.gear.broken.SetActive(info.broken);
      this.initManaseedsInfo(info);
      if (info.isEquipReisou)
        this.setReisouEffect(info);
    }
    else
    {
      e = this.InitBySupply(info.supply);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.EnableQuantity(info.quantity);
      this.supply.favorite.SetActive(info.favorite);
    }
    this.onClick = (Action<ItemIcon>) null;
  }

  private void setReisouRank(GameCore.ItemInfo info)
  {
    this.gear.blinkReisouRanks.resetBlinks();
    this.gear.rank.SetActive(false);
    this.gear.holyReisouRank.SetActive(false);
    this.gear.chaosReisouRank.SetActive(false);
    this.gear.unlimit.SetActive(false);
    this.EnableQuantity(0);
    this.setActiveObject((Component) this.gear.attackClass, false);
    List<GameObject> blinks = new List<GameObject>();
    ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
    if (info.gear.isMythologyReisou())
    {
      PlayerMythologyGearStatus mythologyGearStatus = info.playerItem.GetPlayerMythologyGearStatus();
      this.gear.chaosReisouRank.SetActive(true);
      if (mythologyGearStatus.chaos_gear_level < 10)
      {
        this.gear.chaosReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.chaosReisouRankNumSprite[mythologyGearStatus.chaos_gear_level];
        this.gear.chaosReisouRankDigit[1].SetActive(false);
      }
      else
      {
        int index1 = mythologyGearStatus.chaos_gear_level / 10;
        int index2 = mythologyGearStatus.chaos_gear_level % 10;
        this.gear.chaosReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.chaosReisouRankNumSprite[index1];
        this.gear.chaosReisouRankDigit[1].SetActive(true);
        this.gear.chaosReisouRankDigit[1].GetComponent<UI2DSprite>().sprite2D = this.chaosReisouRankNumSprite[index2];
      }
      blinks.Add(this.gear.chaosReisouRank);
      this.gear.holyReisouRank.SetActive(true);
      if (mythologyGearStatus.holy_gear_level < 10)
      {
        this.gear.holyReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.holyReisouRankNumSprite[mythologyGearStatus.holy_gear_level];
        this.gear.holyReisouRankDigit[1].SetActive(false);
      }
      else
      {
        int index3 = mythologyGearStatus.holy_gear_level / 10;
        int index4 = mythologyGearStatus.holy_gear_level % 10;
        this.gear.holyReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.holyReisouRankNumSprite[index3];
        this.gear.holyReisouRankDigit[1].SetActive(true);
        this.gear.holyReisouRankDigit[1].GetComponent<UI2DSprite>().sprite2D = this.holyReisouRankNumSprite[index4];
      }
      blinks.Add(this.gear.holyReisouRank);
    }
    else if (info.gear.isHolyReisou())
    {
      this.gear.holyReisouRank.SetActive(true);
      if (info.gearLevel < 10)
      {
        this.gear.holyReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.holyReisouRankNumSprite[info.gearLevel];
        this.gear.holyReisouRankDigit[1].SetActive(false);
      }
      else
      {
        int index5 = info.gearLevel / 10;
        int index6 = info.gearLevel % 10;
        this.gear.holyReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.holyReisouRankNumSprite[index5];
        this.gear.holyReisouRankDigit[1].SetActive(true);
        this.gear.holyReisouRankDigit[1].GetComponent<UI2DSprite>().sprite2D = this.holyReisouRankNumSprite[index6];
      }
      blinks.Add(this.gear.holyReisouRank);
    }
    else if (info.gear.isChaosReisou())
    {
      this.gear.chaosReisouRank.SetActive(true);
      if (info.gearLevel < 10)
      {
        this.gear.chaosReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.chaosReisouRankNumSprite[info.gearLevel];
        this.gear.chaosReisouRankDigit[1].SetActive(false);
      }
      else
      {
        int index7 = info.gearLevel / 10;
        int index8 = info.gearLevel % 10;
        this.gear.chaosReisouRankDigit[0].GetComponent<UI2DSprite>().sprite2D = this.chaosReisouRankNumSprite[index7];
        this.gear.chaosReisouRankDigit[1].SetActive(true);
        this.gear.chaosReisouRankDigit[1].GetComponent<UI2DSprite>().sprite2D = this.chaosReisouRankNumSprite[index8];
      }
      blinks.Add(this.gear.chaosReisouRank);
    }
    if (blinks.Count >= 2)
      this.gear.blinkReisouRanks.resetBlinks((IEnumerable<GameObject>) blinks);
    ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(true);
  }

  public IEnumerator InitByPlayerItem(
    PlayerItem playerItem,
    PlayerItem reisouItem = null,
    bool bExpireDate = false)
  {
    GameCore.ItemInfo info = new GameCore.ItemInfo(playerItem, bExpireDate);
    if (reisouItem != (PlayerItem) null)
    {
      info.reisou = reisouItem;
      info.isEquipReisou_ = true;
    }
    IEnumerator e = this.InitByItemInfo(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitByPlayerMaterialGear(PlayerMaterialGear playerItem)
  {
    IEnumerator e = this.InitByItemInfo(new GameCore.ItemInfo(playerItem));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitBySupplyItem(SupplyItem supplyItem)
  {
    this.removeButton.SetActive(false);
    IEnumerator e = this.InitBySupply(supplyItem.Supply);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.EnableQuantity(supplyItem.ItemQuantity);
    if (this.itemInfo == null)
      this.itemInfo = new GameCore.ItemInfo(GameCore.ItemInfo.ItemType.Supply);
    this.itemInfo.masterID = supplyItem.Supply.ID;
    this.supply.name.GetComponent<UILabel>().SetText(supplyItem.Supply.name);
    this.onClick = (Action<ItemIcon>) null;
  }

  public void InitByRemoveButton()
  {
    this.gear.root.SetActive(false);
    this.supply.root.SetActive(false);
    this.gear.button.onLongPress.Clear();
    this.supply.button.onLongPress.Clear();
    this.removeButton.SetActive(true);
  }

  private void InitManaseedsInfo(GearGear gear, int accessoryRemainAmount)
  {
    if ((this.ItemInfo == null || !this.ItemInfo.isWeaponMaterial) && gear != null && gear.disappearance_num.HasValue)
    {
      this.gear.manaseedsBreakageRate.SetActive(false);
      this.gear.manaseedsDurabilityCount.SetActive(false);
      int num1 = gear.disappearance_num.Value;
      GameObject gameObject = (GameObject) null;
      int num2 = 0;
      switch (gear.disappearance_type_GearDisappearanceType)
      {
        case 1:
          this.gear.manaseedsDurabilityCount.SetActive(true);
          this.gear.manaseedsDurabilityCount_1.SetActive(false);
          this.gear.manaseedsDurabilityCount_10.SetActive(false);
          this.gear.manaseedsDurabilityCount_100.SetActive(false);
          num1 = accessoryRemainAmount;
          num2 = num1.ToString().Length;
          switch (num2)
          {
            case 1:
              gameObject = this.gear.manaseedsDurabilityCount_1;
              break;
            case 2:
              gameObject = this.gear.manaseedsDurabilityCount_10;
              break;
            case 3:
              gameObject = this.gear.manaseedsDurabilityCount_100;
              break;
          }
          break;
        case 2:
          this.gear.manaseedsBreakageRate.SetActive(true);
          num2 = num1.ToString().Length;
          switch (num2)
          {
            case 1:
              gameObject = this.gear.manaseedsBreakageRate_1;
              break;
            case 2:
              gameObject = this.gear.manaseedsBreakageRate_10;
              break;
          }
          break;
      }
      if (!Object.op_Inequality((Object) gameObject, (Object) null))
        return;
      gameObject.SetActive(true);
      string[] strArray = new string[3]
      {
        "slc_num_digit_ones",
        "slc_num_digit_tens",
        "slc_num_digit_hundreds"
      };
      for (int index = 0; index < num2; ++index)
      {
        Transform transform = gameObject.transform.Find(strArray[index]);
        int result;
        if (Object.op_Inequality((Object) transform, (Object) null) && int.TryParse(num1.ToString().Substring(num2 - 1 - index, 1), out result))
        {
          UI2DSprite component = ((Component) transform).GetComponent<UI2DSprite>();
          Rect textureRect = this.numberSprite[result].textureRect;
          ((UIWidget) component).SetDimensions((int) ((Rect) ref textureRect).width, ItemIcon.ManaSeedsDurabilityFontSize);
          component.sprite2D = this.numberSprite[result];
        }
      }
    }
    else
    {
      this.gear.manaseedsBreakageRate.SetActive(false);
      this.gear.manaseedsDurabilityCount.SetActive(false);
    }
  }

  public void initManaseedsInfo(GameCore.ItemInfo info)
  {
    this.InitManaseedsInfo(info.gear, info.gearAccessoryRemainingAmount);
  }

  public void initManaseedsInfo(PlayerItem playerItem)
  {
    this.InitManaseedsInfo(playerItem.gear, playerItem.gear_accessory_remaining_amount);
  }

  public IEnumerator InitByGear(
    GearGear gear,
    CommonElement element = CommonElement.none,
    bool isWeaponMaterial = false,
    GearGear setReisou = null,
    bool isRouletteWheelIcon = false)
  {
    this.gear.dynReisouEffect.transform.Clear();
    if (gear != null)
    {
      this.gear.root.SetActive(true);
      this.supply.root.SetActive(false);
      if (gear.rarity.index > 0)
      {
        UI2DSprite component1 = this.gear.star.GetComponent<UI2DSprite>();
        Rect textureRect = this.raritySprite[gear.rarity.index - 1].textureRect;
        int width1 = (int) ((Rect) ref textureRect).width;
        textureRect = this.raritySprite[gear.rarity.index - 1].textureRect;
        int height1 = (int) ((Rect) ref textureRect).height;
        ((UIWidget) component1).SetDimensions(width1, height1);
        this.gear.star.GetComponent<UI2DSprite>().sprite2D = this.raritySprite[gear.rarity.index - 1];
        Sprite sprite2D = this.gear.star.GetComponent<UI2DSprite>().sprite2D;
        UI2DSprite component2 = ((Component) this.gear.sortRankMaxStar).GetComponent<UI2DSprite>();
        textureRect = sprite2D.textureRect;
        int width2 = (int) ((Rect) ref textureRect).width;
        textureRect = sprite2D.textureRect;
        int height2 = (int) ((Rect) ref textureRect).height;
        ((UIWidget) component2).SetDimensions(width2, height2);
        this.gear.sortRankMaxStar.sprite2D = sprite2D;
        UI2DSprite component3 = this.gear.starWIconNone.GetComponent<UI2DSprite>();
        textureRect = this.raritySprite[gear.rarity.index - 1].textureRect;
        int width3 = (int) ((Rect) ref textureRect).width;
        textureRect = this.raritySprite[gear.rarity.index - 1].textureRect;
        int height3 = (int) ((Rect) ref textureRect).height;
        ((UIWidget) component3).SetDimensions(width3, height3);
        this.gear.starWIconNone.GetComponent<UI2DSprite>().sprite2D = this.raritySprite[gear.rarity.index - 1];
      }
      else
      {
        this.gear.star.SetActive(false);
        this.gear.defaultGearTxt.SetActive(true);
        this.gear.starWIconNone.SetActive(false);
      }
      GearKind gearKind = gear.kind;
      this.SetGearType(gearKind, element);
      this.gear.favorite.SetActive(false);
      this.gear.rank.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.unlimit.SetActive(false);
      this.gear.broken.SetActive(false);
      this.gear.unknown.SetActive(false);
      if (gear.hasSpecificationOfEquipmentUnits)
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteSpecificationOfEquipmentUnits[gear.customize_flag];
      else
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[gear.customize_flag];
      Sprite sprite;
      if (ItemIcon.gearCache.TryGetValue(gear.ID, out sprite))
      {
        this.gear.icon.sprite2D = sprite;
      }
      else
      {
        Future<Sprite> spriteF = gear.LoadSpriteThumbnail();
        IEnumerator e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.gear.icon.sprite2D = spriteF.Result;
        ItemIcon.gearCache[gear.ID] = spriteF.Result;
        spriteF = (Future<Sprite>) null;
      }
      if (gear.isReisou())
        this.setReisouEffect(true, gear.reisou_type, gear.drilling_exp_mythology_id.HasValue);
      else if (setReisou != null)
        this.setReisouEffect(true, setReisou.reisou_type, gear.drilling_exp_mythology_id.HasValue);
      if (gear.isReisou())
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteReisou;
      if (isRouletteWheelIcon)
        ((Component) this.gear.attackClass).gameObject.SetActive(false);
      else if (Object.op_Inequality((Object) this.gear.attackClass, (Object) null))
      {
        if (gearKind.is_attack && !gear.isReisou())
        {
          if (gear.hasAttackClass)
          {
            this.gear.attackClass.Initialize(gear.gearClassification.attack_classification, gear.attachedElement);
            ((Component) this.gear.attackClass).gameObject.SetActive(true);
            goto label_29;
          }
          else if (gear.attachedElement != CommonElement.none)
          {
            this.setElementIcon(((Component) this.gear.attackClass).gameObject, gear.attachedElement);
            goto label_29;
          }
        }
        ((Component) this.gear.attackClass).gameObject.SetActive(false);
      }
label_29:
      gearKind = (GearKind) null;
    }
    else
    {
      this.gear.root.SetActive(true);
      this.supply.root.SetActive(false);
      this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[0];
      this.gear.star.GetComponent<UI2DSprite>().sprite2D = this.raritySprite[0];
      this.gear.favorite.SetActive(false);
      this.gear.rank.SetActive(false);
      ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.unlimit.SetActive(false);
      this.gear.broken.SetActive(false);
      this.gear.unknown.SetActive(true);
      this.setActiveObject((Component) this.gear.attackClass, false);
    }
    if (isWeaponMaterial)
      this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteWeaponMaterial;
  }

  private void setElementIcon(GameObject go, CommonElement element)
  {
    UI2DSprite component = go.GetComponent<UI2DSprite>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      if (Object.op_Equality((Object) ItemIcon.elementIconPrefab, (Object) null))
        ItemIcon.elementIconPrefab = Resources.Load<GameObject>("Icons/CommonElementIcon");
      component.sprite2D = ItemIcon.elementIconPrefab.GetComponent<CommonElementIcon>().getIcon(element);
      go.SetActive(true);
    }
    else
      go.SetActive(false);
  }

  public static GameObject loadReisouEffect(bool isReisou, GearReisouType reisou_type)
  {
    switch (reisou_type)
    {
      case GearReisouType.holy:
        if (isReisou)
        {
          if (Object.op_Equality((Object) ItemIcon.reisouEffect01Prefab, (Object) null))
            ItemIcon.reisouEffect01Prefab = Resources.Load<GameObject>("Prefabs/ItemIcon/Reisou_Effect_01");
          return ItemIcon.reisouEffect01Prefab;
        }
        if (Object.op_Equality((Object) ItemIcon.buguReisouEffect01Prefab, (Object) null))
          ItemIcon.buguReisouEffect01Prefab = Resources.Load<GameObject>("Prefabs/ItemIcon/BuguReisou_Effect_01");
        return ItemIcon.buguReisouEffect01Prefab;
      case GearReisouType.chaos:
        if (isReisou)
        {
          if (Object.op_Equality((Object) ItemIcon.reisouEffect02Prefab, (Object) null))
            ItemIcon.reisouEffect02Prefab = Resources.Load<GameObject>("Prefabs/ItemIcon/Reisou_Effect_02");
          return ItemIcon.reisouEffect02Prefab;
        }
        if (Object.op_Equality((Object) ItemIcon.buguReisouEffect02Prefab, (Object) null))
          ItemIcon.buguReisouEffect02Prefab = Resources.Load<GameObject>("Prefabs/ItemIcon/BuguReisou_Effect_02");
        return ItemIcon.buguReisouEffect02Prefab;
      case GearReisouType.mythology:
        if (isReisou)
        {
          if (Object.op_Equality((Object) ItemIcon.reisouEffect03Prefab, (Object) null))
            ItemIcon.reisouEffect03Prefab = Resources.Load<GameObject>("Prefabs/ItemIcon/Reisou_Effect_03");
          return ItemIcon.reisouEffect03Prefab;
        }
        if (Object.op_Equality((Object) ItemIcon.buguReisouEffect03Prefab, (Object) null))
          ItemIcon.buguReisouEffect03Prefab = Resources.Load<GameObject>("Prefabs/ItemIcon/BuguReisou_Effect_03");
        return ItemIcon.buguReisouEffect03Prefab;
      default:
        return (GameObject) null;
    }
  }

  private void setReisouEffect(bool isReisou, GearReisouType reisou_type, bool isReisouItem)
  {
    if (this.gear.dynReisouEffect.transform.childCount > 0)
      this.gear.dynReisouEffect.transform.Clear();
    if (isReisouItem)
      return;
    GameObject self = ItemIcon.loadReisouEffect(isReisou, reisou_type);
    if (Object.op_Equality((Object) self, (Object) null))
      return;
    GameObject gameObject = self.Clone(this.gear.dynReisouEffect.transform);
    if (!Object.op_Inequality((Object) ((Component) this).transform.parent, (Object) null))
      return;
    float x = ((Component) ((Component) this).transform.parent).transform.localScale.x;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(x, x, x);
    ParticleSystem componentInChildren = ((Component) gameObject.transform).GetComponentInChildren<ParticleSystem>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    ((Component) componentInChildren).transform.localScale = vector3;
  }

  private void setReisouEffect(GameCore.ItemInfo info)
  {
    if (!info.isReisou && !info.isEquipReisou)
      return;
    GearReisouType reisou_type = GearReisouType.none;
    if (info.isReisou)
      reisou_type = info.gear.reisou_type;
    else if (info.reisou != (PlayerItem) null)
      reisou_type = info.reisou.gear.reisou_type;
    this.setReisouEffect(info.isReisou, reisou_type, info.gear.drilling_exp_mythology_id.HasValue);
  }

  public IEnumerator InitByQuestDrop(QuestCommonDrop questDrop)
  {
    if (questDrop.entity_type == MasterDataTable.CommonRewardType.supply)
    {
      yield return (object) this.InitBySupply(MasterData.SupplySupply[questDrop.entity_id]);
    }
    else
    {
      GearGear gear = MasterData.GearGear[questDrop.entity_id];
      yield return (object) this.InitByGear(gear, gear.GetElement(), questDrop.entity_type == MasterDataTable.CommonRewardType.gear_body);
    }
  }

  public IEnumerator InitByShopContent(ShopContent content)
  {
    IEnumerator e;
    if (content.entity_type == MasterDataTable.CommonRewardType.supply)
    {
      SupplySupply supply = (SupplySupply) null;
      if (MasterData.SupplySupply.TryGetValue(content.entity_id, out supply))
      {
        e = this.InitBySupply(supply);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.EnableQuantity(0);
        this.BottomModeValue = ItemIcon.BottomMode.Nothing;
      }
      else
        this.SetEmpty(true);
    }
    else
    {
      GearGear gear = (GearGear) null;
      if (MasterData.GearGear.TryGetValue(content.entity_id, out gear))
      {
        e = this.InitByGear(gear, gear.GetElement(), content.entity_type == MasterDataTable.CommonRewardType.gear_body);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.QuantitySupply = false;
      }
      else
        this.SetEmpty(true);
    }
  }

  public IEnumerator InitByMaterialExchange(MasterDataTable.CommonRewardType type, int rewardID)
  {
    IEnumerator e;
    if (type == MasterDataTable.CommonRewardType.supply)
    {
      SupplySupply supply = (SupplySupply) null;
      if (MasterData.SupplySupply.TryGetValue(rewardID, out supply))
      {
        e = this.InitBySupply(supply);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.EnableQuantity(0);
        this.BottomModeValue = ItemIcon.BottomMode.Nothing;
      }
      else
        this.SetEmpty(true);
    }
    else
    {
      GearGear gear = (GearGear) null;
      if (MasterData.GearGear.TryGetValue(rewardID, out gear))
      {
        e = this.InitByGear(gear, gear.GetElement(), type == MasterDataTable.CommonRewardType.gear_body);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.QuantitySupply = false;
      }
      else
        this.SetEmpty(true);
    }
  }

  public IEnumerator InitForEquipGear(bool bJingiBack = false)
  {
    this.gear.root.SetActive(true);
    this.supply.root.SetActive(false);
    this.gear.favorite.SetActive(false);
    this.gear.rank.SetActive(false);
    ((Component) this.gear.blinkIcons).gameObject.SetActive(true);
    ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
    this.gear.unlimit.SetActive(false);
    this.gear.broken.SetActive(false);
    this.gear.unknown.SetActive(false);
    this.gear.item_back.SetActive(false);
    this.gear.backGround.SetActive(true);
    this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = bJingiBack ? this.nonBackSpriteByJingi : this.nonBackSprite;
    this.gear.star.GetComponent<UI2DSprite>().sprite2D = this.raritySprite[0];
    this.gear.star.SetActive(false);
    this.gear.type.GetComponent<UI2DSprite>().sprite2D = this.nonTypeSprite;
    this.gear.type.SetActive(true);
    yield break;
  }

  public IEnumerator InitBySupply(SupplySupply supply)
  {
    this.supply.root.SetActive(true);
    this.gear.root.SetActive(false);
    Sprite sprite;
    if (ItemIcon.supplyCache.TryGetValue(supply.ID, out sprite))
    {
      this.supply.icon.sprite2D = sprite;
    }
    else
    {
      Future<Sprite> spriteF = supply.LoadSpriteThumbnail();
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.supply.icon.sprite2D = spriteF.Result;
      ItemIcon.supplyCache[supply.ID] = spriteF.Result;
      spriteF = (Future<Sprite>) null;
    }
    ((IEnumerable<GameObject>) this.supply.rarities).ToggleOnce(supply.rarity.index - 1);
    this.supply.favorite.SetActive(false);
    this.selectQuantity.SetActive(false);
    this.supply.name.GetComponent<UILabel>().SetText(supply.name);
  }

  public void InitByItemInfoCache(GameCore.ItemInfo info)
  {
    this.itemInfo = info;
    this.removeButton.SetActive(false);
    this.gear.sortRankMaxInfo.SetActive(false);
    ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
    ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
    if (!info.isSupply)
    {
      this.InitByGearCache(info);
      if (info.isWeaponMaterial)
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteWeaponMaterial;
      else if (info.isReisou)
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteReisou;
      if (info.isWeapon && info.gear.disappearance_type_GearDisappearanceType == 0)
      {
        this.gear.rank.SetActive(true);
        ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
        if (info.gearLevel == info.gearLevelLimit)
          this.gear.rank.GetComponent<UI2DSprite>().sprite2D = this.limitRankSprite[info.gearLevel - 1 < 0 ? 0 : info.gearLevel - 1];
        else
          this.gear.rank.GetComponent<UI2DSprite>().sprite2D = this.rankSprite[info.gearLevel - 1 < 0 ? 0 : info.gearLevel - 1];
        if (info.gearLevelUnLimit > 0)
        {
          this.gear.unlimit.SetActive(true);
          this.gear.unlimit.GetComponent<UI2DSprite>().sprite2D = this.gearUnlimitSprite[info.gearLevelUnLimit - 1];
        }
        else
          this.gear.unlimit.SetActive(false);
        this.EnableQuantity(0);
        this.gear.sortRankMaxRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_RANK, (IDictionary) new Hashtable()
        {
          {
            (object) "now",
            (object) info.gearLevel
          },
          {
            (object) "max",
            (object) info.gearLevelLimit
          }
        }));
      }
      else if (info.isReisou)
      {
        this.setReisouRank(info);
      }
      else
      {
        this.gear.rank.SetActive(false);
        ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
        this.gear.unlimit.SetActive(false);
        this.EnableQuantity(info.quantity);
        if (!info.isWeaponMaterial)
          this.setActiveObject((Component) this.gear.attackClass, false);
      }
      this.gear.favorite.SetActive(info.favorite);
      this.gear.broken.SetActive(info.broken);
      this.initManaseedsInfo(info);
      this.ForBattle = info.ForBattle;
      this.FusionPossible = info.FusionPossible;
      this.SetupIconsBlink();
      this.setReisouEffect(info);
    }
    else
    {
      this.InitBySupplyCache(info.supply);
      this.EnableQuantity(info.quantity);
      this.supply.favorite.SetActive(info.favorite);
    }
    this.onClick = (Action<ItemIcon>) null;
  }

  public void InitBySupplyItemCache(SupplyItem supplyItem)
  {
    this.removeButton.SetActive(false);
    this.InitBySupplyCache(supplyItem.Supply);
    this.EnableQuantity(supplyItem.ItemQuantity);
    this.itemInfo.masterID = supplyItem.Supply.ID;
    this.supply.name.GetComponent<UILabel>().SetText(supplyItem.Supply.name);
    this.onClick = (Action<ItemIcon>) null;
  }

  public void InitByGearCache(GearGear info)
  {
    this.gear.dynReisouEffect.transform.Clear();
    if (info != null)
    {
      this.gear.root.SetActive(true);
      this.supply.root.SetActive(false);
      this.gear.icon.sprite2D = ItemIcon.gearCache[info.ID];
      UI2DSprite component1 = this.gear.star.GetComponent<UI2DSprite>();
      Rect textureRect1 = this.raritySprite[info.rarity.index - 1].textureRect;
      int width1 = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = this.raritySprite[info.rarity.index - 1].textureRect;
      int height1 = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) component1).SetDimensions(width1, height1);
      this.gear.star.GetComponent<UI2DSprite>().sprite2D = this.raritySprite[info.rarity.index - 1];
      Sprite sprite2D = this.gear.star.GetComponent<UI2DSprite>().sprite2D;
      UI2DSprite component2 = ((Component) this.gear.sortRankMaxStar).GetComponent<UI2DSprite>();
      Rect textureRect3 = sprite2D.textureRect;
      int width2 = (int) ((Rect) ref textureRect3).width;
      textureRect3 = sprite2D.textureRect;
      int height2 = (int) ((Rect) ref textureRect3).height;
      ((UIWidget) component2).SetDimensions(width2, height2);
      this.gear.sortRankMaxStar.sprite2D = sprite2D;
      GearKind kind = info.kind;
      this.SetGearType(kind, info.GetElement());
      if (info.hasSpecificationOfEquipmentUnits)
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSpriteSpecificationOfEquipmentUnits[info.customize_flag];
      else
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[info.customize_flag];
      if (Object.op_Inequality((Object) this.gear.attackClass, (Object) null))
      {
        if (kind.is_attack)
        {
          if (info.hasAttackClass)
          {
            this.gear.attackClass.Initialize(info.gearClassification.attack_classification, info.attachedElement);
            ((Component) this.gear.attackClass).gameObject.SetActive(true);
            goto label_11;
          }
          else if (info.attachedElement != CommonElement.none)
          {
            this.setElementIcon(((Component) this.gear.attackClass).gameObject, info.attachedElement);
            goto label_11;
          }
        }
        ((Component) this.gear.attackClass).gameObject.SetActive(false);
      }
label_11:
      this.gear.favorite.SetActive(false);
      this.gear.rank.SetActive(false);
      this.gear.forbattle.SetActive(false);
      this.gear.fusionPossible.SetActive(false);
      ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.unlimit.SetActive(false);
      this.gear.broken.SetActive(false);
      this.gear.unknown.SetActive(false);
    }
    else
    {
      this.gear.root.SetActive(true);
      this.supply.root.SetActive(false);
      this.gear.star.GetComponent<UI2DSprite>().sprite2D = this.raritySprite[0];
      this.gear.favorite.SetActive(false);
      this.gear.rank.SetActive(false);
      this.gear.forbattle.SetActive(false);
      this.gear.fusionPossible.SetActive(false);
      ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.unlimit.SetActive(false);
      this.gear.broken.SetActive(false);
      this.gear.unknown.SetActive(true);
      this.setActiveObject((Component) this.gear.attackClass, false);
    }
  }

  public void InitByGearCache(GameCore.ItemInfo info) => this.InitByGearCache(info.gear);

  public void InitBySupplyCache(SupplySupply supply)
  {
    this.supply.root.SetActive(true);
    this.gear.root.SetActive(false);
    this.supply.icon.sprite2D = ItemIcon.supplyCache[supply.ID];
    ((IEnumerable<GameObject>) this.supply.rarities).ToggleOnce(supply.rarity.index - 1);
    this.supply.favorite.SetActive(false);
    this.selectQuantity.SetActive(false);
    this.supply.name.GetComponent<UILabel>().SetText(supply.name);
  }

  public static IEnumerator LoadSprite(GameCore.ItemInfo info)
  {
    Future<Sprite> spriteF;
    IEnumerator e;
    if (info != null && !ItemIcon.IsCache(info))
    {
      if (!info.isSupply)
      {
        GearGear gear = info.gear;
        spriteF = gear.LoadSpriteThumbnail();
        e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ItemIcon.gearCache[gear.ID] = spriteF.Result;
        gear = (GearGear) null;
        spriteF = (Future<Sprite>) null;
      }
      else
      {
        SupplySupply supply = info.supply;
        spriteF = supply.LoadSpriteThumbnail();
        e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ItemIcon.supplyCache[supply.ID] = spriteF.Result;
        supply = (SupplySupply) null;
        spriteF = (Future<Sprite>) null;
      }
    }
  }

  public static IEnumerator LoadSprite(GearGear gear)
  {
    if (!ItemIcon.IsCache(gear))
    {
      Future<Sprite> spriteF = gear.LoadSpriteThumbnail();
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ItemIcon.gearCache[gear.ID] = spriteF.Result;
    }
  }

  public static IEnumerator LoadSprite(GearGear gear, Action<Sprite> callbackLoaded)
  {
    if (gear != null)
    {
      Sprite result;
      if (!ItemIcon.gearCache.TryGetValue(gear.ID, out result))
      {
        Future<Sprite> spriteF = gear.LoadSpriteThumbnail();
        IEnumerator e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ItemIcon.gearCache[gear.ID] = result = spriteF.Result;
        spriteF = (Future<Sprite>) null;
      }
      callbackLoaded(result);
    }
  }

  public static Sprite GetSprite(GearGear gear)
  {
    if (gear == null)
      return (Sprite) null;
    Sprite sprite;
    return !ItemIcon.gearCache.TryGetValue(gear.ID, out sprite) ? (Sprite) null : sprite;
  }

  public static IEnumerator LoadSprite(SupplyItem supply)
  {
    if (supply != null && !ItemIcon.IsCache(supply) && supply != null)
    {
      Future<Sprite> spriteF = supply.Supply.LoadSpriteThumbnail();
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ItemIcon.supplyCache[supply.Supply.ID] = spriteF.Result;
      spriteF = (Future<Sprite>) null;
    }
  }

  public void DisableLongPressEvent()
  {
    ((Component) this.gear.button).gameObject.SetActive(false);
    ((Component) this.supply.button).gameObject.SetActive(false);
  }

  public void EnableLongPressEvent(bool isLimited = false)
  {
    if (this.itemInfo != null)
    {
      if (this.itemInfo.isWeapon)
      {
        ((Component) this.gear.button).gameObject.SetActive(true);
        EventDelegate.Set(this.gear.button.onLongPress, (EventDelegate.Callback) (() =>
        {
          if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
          {
            Unit05443Scene.changeSceneLimited(true, this.itemInfo);
          }
          else
          {
            if (Singleton<NGGameDataManager>.GetInstance().IsSea)
              Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
            if (isLimited)
              Unit00443Scene.changeSceneLimited(true, this.itemInfo);
            else
              Unit00443Scene.changeScene(true, this.itemInfo);
          }
        }));
      }
      else if (this.itemInfo.isSupply)
      {
        ((Component) this.supply.button).gameObject.SetActive(true);
        EventDelegate.Set(this.supply.button.onLongPress, (EventDelegate.Callback) (() =>
        {
          if (Singleton<NGGameDataManager>.GetInstance().IsSea)
            Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
          Bugu00561Scene.changeScene(true, this.itemInfo);
        }));
      }
      else
      {
        ((Component) this.gear.button).gameObject.SetActive(true);
        EventDelegate.Set(this.gear.button.onLongPress, (EventDelegate.Callback) (() =>
        {
          if (Singleton<NGGameDataManager>.GetInstance().IsSea)
            Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
          Bugu00561Scene.changeScene(true, this.itemInfo);
        }));
      }
    }
    else
      Debug.LogError((object) "playerItem NULL");
  }

  public void EnableLongPressEvent(bool isGear, Action<ItemIcon> action)
  {
    if (isGear)
    {
      ((Component) this.gear.button).gameObject.SetActive(true);
      EventDelegate.Set(this.gear.button.onLongPress, (EventDelegate.Callback) (() => action(this)));
    }
    else
    {
      ((Component) this.supply.button).gameObject.SetActive(true);
      EventDelegate.Set(this.supply.button.onLongPress, (EventDelegate.Callback) (() => action(this)));
    }
  }

  public void EnableLongPressEvent(Action<GameCore.ItemInfo> action)
  {
    if (this.itemInfo == null)
      return;
    if (this.itemInfo.isSupply)
    {
      ((Component) this.supply.button).gameObject.SetActive(true);
      EventDelegate.Set(this.supply.button.onLongPress, (EventDelegate.Callback) (() => action(this.itemInfo)));
    }
    else
    {
      ((Component) this.gear.button).gameObject.SetActive(true);
      EventDelegate.Set(this.gear.button.onLongPress, (EventDelegate.Callback) (() => action(this.itemInfo)));
    }
  }

  public void EnableLongPressEventEmptyGear(Action<int> action, int index)
  {
    ((Component) this.gear.button).gameObject.SetActive(true);
    EventDelegate.Set(this.gear.button.onLongPress, (EventDelegate.Callback) (() => action(index)));
  }

  public void EnableLongPressEventReisou(
    PlayerItem playerItemDummy = null,
    Action removeCallback = null,
    PlayerItem customGearBase = null)
  {
    if (this.itemInfo == null)
      return;
    ((Component) this.gear.button).gameObject.SetActive(true);
    EventDelegate.Set(this.gear.button.onLongPress, (EventDelegate.Callback) (() => this.OpenReisouDetailPopup(this.itemInfo, playerItemDummy, removeCallback, customGearBase)));
  }

  public void OpenReisouDetailPopup(
    GameCore.ItemInfo item,
    PlayerItem playerItemDummy = null,
    Action removeCallback = null,
    PlayerItem customGearBase = null)
  {
    if (this.isReisouPopupOpen)
      return;
    this.isReisouPopupOpen = true;
    this.StartCoroutine(this.OpenReisouDetailPopupAsync(item, playerItemDummy, removeCallback, customGearBase));
  }

  public IEnumerator OpenReisouDetailPopupAsync(
    GameCore.ItemInfo item,
    PlayerItem playerItemDummy = null,
    Action removeCallback = null,
    PlayerItem customGearBase = null)
  {
    if (item == null)
    {
      this.isReisouPopupOpen = false;
    }
    else
    {
      PlayerItem playerItemTemp = playerItemDummy;
      if (playerItemTemp == (PlayerItem) null)
        playerItemTemp = item.playerItem;
      if (playerItemTemp == (PlayerItem) null)
      {
        this.isReisouPopupOpen = false;
      }
      else
      {
        bool isDispRank = playerItemDummy == (PlayerItem) null;
        GameObject popup;
        Future<GameObject> popupPrefabF;
        IEnumerator e;
        if (item.gear.isMythologyReisou())
        {
          if (Object.op_Equality((Object) ItemIcon.reisouPopupDualSkillPrefab, (Object) null))
          {
            popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails_DualSkill").Load<GameObject>();
            e = popupPrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            ItemIcon.reisouPopupDualSkillPrefab = popupPrefabF.Result;
            popupPrefabF = (Future<GameObject>) null;
          }
          popup = ItemIcon.reisouPopupDualSkillPrefab.Clone();
          PopupReisouDetailsDualSkill script = popup.GetComponent<PopupReisouDetailsDualSkill>();
          popup.SetActive(false);
          e = script.Init(item, playerItemTemp, isDispRank, removeCallback, false, false, false, (PlayerItem) null);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          popup.SetActive(true);
          Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
          yield return (object) null;
          script.scrollResetPosition();
          popup = (GameObject) null;
          script = (PopupReisouDetailsDualSkill) null;
        }
        else
        {
          if (Object.op_Equality((Object) ItemIcon.reisouPopupPrefab, (Object) null))
          {
            popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails").Load<GameObject>();
            e = popupPrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            ItemIcon.reisouPopupPrefab = popupPrefabF.Result;
            popupPrefabF = (Future<GameObject>) null;
          }
          popup = ItemIcon.reisouPopupPrefab.Clone();
          PopupReisouDetails script = popup.GetComponent<PopupReisouDetails>();
          popup.SetActive(false);
          bool isFusionPossible = customGearBase == (PlayerItem) null;
          if (isFusionPossible && Object.op_Implicit((Object) GameObject.Find("popup_Reisou_fusion(Clone)")))
            isFusionPossible = false;
          e = script.Init(item, playerItemTemp, isDispRank, removeCallback, isFusionPossible: isFusionPossible, customGearBase: customGearBase);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          popup.SetActive(true);
          Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
          yield return (object) null;
          script.scrollResetPosition();
          popup = (GameObject) null;
          script = (PopupReisouDetails) null;
        }
        yield return (object) null;
        this.isReisouPopupOpen = false;
      }
    }
  }

  public void ReleaseClickEvent()
  {
    this.gear.button.onClick.Clear();
    this.supply.button.onClick.Clear();
  }

  public bool Selected
  {
    get
    {
      return this.EnabledGear ? this.gear.selectedBack.activeSelf : this.supply.selectedBack.activeSelf;
    }
  }

  public void Deselect()
  {
    if (this.EnabledGear)
    {
      this.gear.selectedBack.SetActive(false);
      this.gear.selectedNum.SetActive(false);
    }
    else
    {
      this.supply.selectedBack.SetActive(false);
      ((IEnumerable<GameObject>) this.supply.selectedSupply).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    }
  }

  public void DeselectByCheckIcon()
  {
    if (!this.EnabledGear)
      return;
    this.gear.selectedBack.SetActive(false);
    this.gear.selectedCheck.SetActive(false);
  }

  public void Select(int selectCount)
  {
    this.Deselect();
    if (this.EnabledGear)
    {
      this.gear.selectedBack.SetActive(true);
      this.gear.selectedNum.SetActive(true);
      UI2DSprite component = this.gear.selectedNum.GetComponent<UI2DSprite>();
      Rect textureRect = this.selectNumSprite[selectCount].textureRect;
      ((UIWidget) component).SetDimensions((int) ((Rect) ref textureRect).width, ItemIcon.SelectedIndexFontSize);
      component.sprite2D = this.selectNumSprite[selectCount];
    }
    else
    {
      this.supply.selectedBack.SetActive(true);
      this.supply.selectedSupply[selectCount].SetActive(true);
    }
  }

  public void SelectByCheckIcon()
  {
    this.DeselectByCheckIcon();
    if (!this.EnabledGear)
      return;
    this.gear.selectedBack.SetActive(true);
    this.gear.selectedCheck.SetActive(true);
  }

  public Sprite IconSprite
  {
    get => !this.EnabledGear ? this.supply.icon.sprite2D : this.gear.icon.sprite2D;
  }

  public Action<ItemIcon> onClick
  {
    get => this.onClick_;
    set
    {
      this.onClick_ = value;
      if (this.onClick_ == null)
        return;
      EventDelegate.Set(this.gear.button.onClick, (EventDelegate.Callback) (() =>
      {
        if (this.onClick_ == null)
          return;
        this.onClick_(this);
      }));
      EventDelegate.Set(this.supply.button.onClick, (EventDelegate.Callback) (() =>
      {
        if (this.onClick_ == null)
          return;
        this.onClick_(this);
      }));
      EventDelegate.Set(this.removeButton.GetComponent<UIButton>().onClick, (EventDelegate.Callback) (() =>
      {
        if (this.onClick_ == null)
          return;
        this.onClick_(this);
      }));
    }
  }

  private void SetGearType(GearKind kind, CommonElement element)
  {
    this.gear.type.SetActive(false);
    if (!this.gear.root.activeSelf)
      return;
    if (!kind.isEquip)
    {
      this.gear.star.transform.localPosition = new Vector3(-12f, 0.0f);
    }
    else
    {
      this.gear.star.transform.localPosition = Vector3.zero;
      Sprite sprite = GearKindIcon.LoadSprite(kind.Enum, element);
      this.gear.type.GetComponent<UI2DSprite>().sprite2D = sprite;
      this.gear.type.SetActive(true);
    }
  }

  private void SetCross(ItemIcon.CounterDigits digits)
  {
    ((IEnumerable<GameObject>) new GameObject[4]
    {
      this.m_crossForOneDigitCount,
      this.m_crossForTwoDigitsCount,
      this.m_crossForThreeDigitsCount,
      this.m_crossForFourDigitsCount
    }).ToggleOnceEx((int) digits);
  }

  public void HideCounter()
  {
    this.QuantitySupply = false;
    if (Object.op_Inequality((Object) this.m_crossForOneDigitCount, (Object) null))
      this.m_crossForOneDigitCount.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForTwoDigitsCount, (Object) null))
      this.m_crossForTwoDigitsCount.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForThreeDigitsCount, (Object) null))
      this.m_crossForThreeDigitsCount.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForFourDigitsCount, (Object) null))
      this.m_crossForFourDigitsCount.SetActive(false);
    ((IEnumerable<GameObject>) this.m_onesDigit).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_tensDigit).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_hundredsDigit).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_thousandsDigit).ToggleOnceEx(-1);
  }

  public void EnableQuantity(int quantity)
  {
    if (quantity <= 0)
    {
      this.HideCounter();
    }
    else
    {
      if (quantity > 9999)
        quantity = 9999;
      int num1 = quantity % 10;
      int num2 = quantity % 100 / 10;
      int num3 = quantity % 1000 / 100;
      int num4 = quantity % 10000 / 1000;
      ItemIcon.CounterDigits digits = ItemIcon.CounterDigits.OneDigit;
      if (quantity < 1)
      {
        this.HideCounter();
      }
      else
      {
        if (quantity >= 10)
          digits = quantity >= 100 ? (quantity >= 1000 ? ItemIcon.CounterDigits.FourDigits : ItemIcon.CounterDigits.ThreeDigits) : ItemIcon.CounterDigits.TwoDigits;
        this.SetCross(digits);
        this.SetOnesDigit(num1);
        this.SetTensDigit(num2, digits);
        this.SetHundredsDigit(num3, digits);
        this.SetThousandsDigit(num4, digits);
        this.QuantitySupply = true;
      }
    }
  }

  public void SetQuantityPositionY(float y)
  {
    Transform transform = this.quantity.transform;
    transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
  }

  private void SetOnesDigit(int num)
  {
    if (num > -1 && num < 10)
    {
      ((IEnumerable<GameObject>) this.m_onesDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetTensDigit(int num, ItemIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (ItemIcon.CounterDigits.TwoDigits > digits)
        ((IEnumerable<GameObject>) this.m_tensDigit).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_tensDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetHundredsDigit(int num, ItemIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (ItemIcon.CounterDigits.ThreeDigits > digits)
        ((IEnumerable<GameObject>) this.m_hundredsDigit).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_hundredsDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetThousandsDigit(int num, ItemIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (ItemIcon.CounterDigits.FourDigits > digits)
        ((IEnumerable<GameObject>) this.m_thousandsDigit).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_thousandsDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetCrossBonus(ItemIcon.CounterDigits digits)
  {
    ((IEnumerable<GameObject>) new GameObject[4]
    {
      this.m_crossForOneDigitCount_bonus,
      this.m_crossForTwoDigitsCount_bonus,
      this.m_crossForThreeDigitsCount_bonus,
      this.m_crossForFourDigitsCount_bonus
    }).ToggleOnceEx((int) digits);
  }

  private void HideCounterBonus()
  {
    this.QuantitySupplyBonus = false;
    if (Object.op_Inequality((Object) this.m_crossForOneDigitCount_bonus, (Object) null))
      this.m_crossForOneDigitCount_bonus.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForTwoDigitsCount_bonus, (Object) null))
      this.m_crossForTwoDigitsCount_bonus.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForThreeDigitsCount_bonus, (Object) null))
      this.m_crossForThreeDigitsCount_bonus.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForFourDigitsCount_bonus, (Object) null))
      this.m_crossForFourDigitsCount_bonus.SetActive(false);
    ((IEnumerable<GameObject>) this.m_onesDigit_bonus).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_tensDigit_bonus).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_hundredsDigit_bonus).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_thousandsDigit_bonus).ToggleOnceEx(-1);
  }

  public void EnableQuantityBonus(int quantity)
  {
    if (quantity > 9999)
      quantity = 9999;
    int num1 = quantity % 10;
    int num2 = quantity % 100 / 10;
    int num3 = quantity % 1000 / 100;
    int num4 = quantity % 10000 / 1000;
    ItemIcon.CounterDigits digits = ItemIcon.CounterDigits.OneDigit;
    if (quantity >= 10)
      digits = quantity >= 100 ? (quantity >= 1000 ? ItemIcon.CounterDigits.FourDigits : ItemIcon.CounterDigits.ThreeDigits) : ItemIcon.CounterDigits.TwoDigits;
    this.SetCrossBonus(digits);
    this.SetOnesDigitBonus(num1);
    this.SetTensDigitBonus(num2, digits);
    this.SetHundredsDigitBonus(num3, digits);
    this.SetThousandsDigitBonus(num4, digits);
    this.QuantitySupplyBonus = true;
  }

  private void SetOnesDigitBonus(int num)
  {
    if (num > -1 && num < 10)
    {
      ((IEnumerable<GameObject>) this.m_onesDigit_bonus).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetTensDigitBonus(int num, ItemIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (ItemIcon.CounterDigits.TwoDigits > digits)
        ((IEnumerable<GameObject>) this.m_tensDigit_bonus).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_tensDigit_bonus).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetHundredsDigitBonus(int num, ItemIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (ItemIcon.CounterDigits.ThreeDigits > digits)
        ((IEnumerable<GameObject>) this.m_hundredsDigit_bonus).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_hundredsDigit_bonus).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetThousandsDigitBonus(int num, ItemIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (ItemIcon.CounterDigits.FourDigits > digits)
        ((IEnumerable<GameObject>) this.m_thousandsDigit_bonus).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_thousandsDigit_bonus).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  public void SelectedQuantity(int quantity)
  {
    if (quantity == 0)
    {
      this.SelectQuantity = false;
    }
    else
    {
      this.SelectQuantity = true;
      if (quantity > 99)
      {
        Debug.LogWarning((object) ("set quantity over 99, count=" + (object) quantity));
        quantity = 99;
      }
      int index1 = quantity % 10;
      int index2 = Mathf.FloorToInt((float) (quantity / 10));
      for (int index3 = 0; index3 <= 9; ++index3)
      {
        this.selectedRightNum[index3].SetActive(false);
        this.selectedLeftNum[index3].SetActive(false);
      }
      this.selectedRightNum[index1].SetActive(true);
      if (index2 != 0)
        this.selectedLeftNum[index2].SetActive(true);
      else
        this.selectedLeftNum[index2].SetActive(true);
      this.checkmark.SetActive(true);
    }
  }

  private Sprite nonBackSpriteByJingi
  {
    get
    {
      return Object.op_Inequality((Object) this.nonBackSpriteByJingi_, (Object) null) ? this.nonBackSpriteByJingi_ : this.nonBackSprite;
    }
  }

  public void SetEmpty(bool empty, bool bJingiBack = false)
  {
    if (empty)
    {
      this.gear.dynReisouEffect.transform.Clear();
      if (this.EnabledGear)
      {
        this.clearGearBottomBlinks();
        this.supply.root.SetActive(false);
        this.gear.item_back.SetActive(true);
        this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = bJingiBack ? this.nonBackSpriteByJingi : this.nonBackSprite;
        this.gear.bottom.SetActive(true);
        this.gear.bottomWIconNone.SetActive(false);
        this.gear.broken.SetActive(false);
        ((Component) ((Component) this.gear.button).transform).gameObject.SetActive(false);
        this.gear.favorite.SetActive(false);
        this.gear.forbattle.SetActive(false);
        this.gear.fusionPossible.SetActive(false);
        ((Component) ((Component) this.gear.icon).transform).gameObject.SetActive(false);
        this.gear.star.SetActive(false);
        this.gear.rank.SetActive(false);
        ((Component) this.gear.blinkIcons).gameObject.SetActive(false);
        ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
        this.gear.unlimit.SetActive(false);
        this.gear.type.GetComponent<UI2DSprite>().sprite2D = this.nonTypeSprite;
        this.gear.type.SetActive(true);
        this.gear.unknown.SetActive(false);
        this.gear.manaseedsBreakageRate.SetActive(false);
        this.gear.manaseedsDurabilityCount.SetActive(false);
        this.setActiveObject((Component) this.gear.attackClass, false);
      }
      else
      {
        this.gear.root.SetActive(false);
        this.supply.back.SetActive(true);
        this.supply.bottom.SetActive(true);
        ((Component) ((Component) this.supply.button).transform).gameObject.SetActive(false);
        this.supply.equals.SetActive(false);
        this.supply.favorite.SetActive(false);
        this.supply.forbattle.SetActive(false);
        ((Component) ((Component) this.supply.icon).transform).gameObject.SetActive(false);
        this.quantity.SetActive(false);
        ((IEnumerable<GameObject>) this.supply.rarities).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
        this.supply.name.SetActive(false);
      }
      this.EnableQuantity(0);
    }
    else if (this.EnabledGear)
    {
      this.clearGearBottomBlinks();
      this.gear.item_back.SetActive(true);
      this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[0];
      this.gear.bottom.SetActive(true);
      this.gear.bottomWIconNone.SetActive(false);
      this.gear.broken.SetActive(true);
      ((Component) ((Component) this.gear.button).transform).gameObject.SetActive(true);
      this.gear.favorite.SetActive(true);
      this.gear.forbattle.SetActive(false);
      this.gear.fusionPossible.SetActive(false);
      ((Component) ((Component) this.gear.icon).transform).gameObject.SetActive(true);
      this.gear.star.SetActive(true);
      this.gear.rank.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.unlimit.SetActive(false);
      this.gear.type.GetComponent<UI2DSprite>().sprite2D = this.nonTypeSprite;
      this.gear.type.SetActive(false);
      this.setActiveObject((Component) this.gear.attackClass, false);
      this.gear.unknown.SetActive(true);
    }
    else
    {
      this.supply.back.SetActive(true);
      this.supply.bottom.SetActive(true);
      ((Component) ((Component) this.supply.button).transform).gameObject.SetActive(true);
      this.supply.equals.SetActive(true);
      this.supply.favorite.SetActive(true);
      this.supply.forbattle.SetActive(false);
      ((Component) ((Component) this.supply.icon).transform).gameObject.SetActive(true);
      this.quantity.SetActive(true);
      ((IEnumerable<GameObject>) this.supply.rarities).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(true)));
      this.supply.name.SetActive(true);
    }
  }

  private void clearGearBottomBlinks()
  {
    if (this.gear.bottomBlink == null || Object.op_Equality((Object) this.gear.bottomBlink.blink, (Object) null))
      return;
    ItemIcon.ControlBottomBlink[] controlBottomBlinkArray = new ItemIcon.ControlBottomBlink[2]
    {
      this.gear.bottomBlink,
      this.gear.bottomWIconNoneBlink
    };
    foreach (ItemIcon.ControlBottomBlink controlBottomBlink in controlBottomBlinkArray)
    {
      controlBottomBlink.remainDays.SetActive(false);
      if (((Component) controlBottomBlink.blink).gameObject.activeSelf)
      {
        ((Component) controlBottomBlink.blink).gameObject.SetActive(false);
        controlBottomBlink.blink.resetBlinks();
      }
    }
    this.gear.currentBlink = (ItemIcon.ControlBottomBlink) null;
  }

  private void setActiveObject(Component com, bool bActive)
  {
    if (!Object.op_Inequality((Object) com, (Object) null))
      return;
    com.gameObject.SetActive(bActive);
  }

  public void setEquipPlus(bool bl) => this.gear.equipPlus.SetActive(bl);

  public void setEquipJinkiPlus(bool bl) => this.gear.equipJinkiPlus.SetActive(bl);

  public static bool IsCache(GameCore.ItemInfo info)
  {
    if (info == null)
      return false;
    return !info.isSupply ? ItemIcon.gearCache.ContainsKey(info.gear.ID) : ItemIcon.supplyCache.ContainsKey(info.supply.ID);
  }

  public static bool IsCache(PlayerItem playerItem)
  {
    if (playerItem == (PlayerItem) null)
      return false;
    return playerItem.gear != null ? ItemIcon.gearCache.ContainsKey(playerItem.gear.ID) : ItemIcon.supplyCache.ContainsKey(playerItem.supply.ID);
  }

  public static bool IsCache(SupplyItem supply)
  {
    return supply != null && ItemIcon.supplyCache.ContainsKey(supply.Supply.ID);
  }

  public static bool IsCache(GearGear gear) => ItemIcon.gearCache.ContainsKey(gear.ID);

  public static bool IsCache(SupplySupply supply) => ItemIcon.supplyCache.ContainsKey(supply.ID);

  public static void ClearCache()
  {
    if (ItemIcon.IsPoolCache)
    {
      ItemIcon.IsPoolCache = false;
      if (!PerformanceConfig.GetInstance().IsLowMemory)
        return;
    }
    ItemIcon.gearCache.Clear();
    ItemIcon.supplyCache.Clear();
    ItemIcon.elementIconPrefab = (GameObject) null;
    ItemIcon.reisouEffect01Prefab = (GameObject) null;
    ItemIcon.reisouEffect02Prefab = (GameObject) null;
    ItemIcon.reisouEffect03Prefab = (GameObject) null;
    ItemIcon.buguReisouEffect01Prefab = (GameObject) null;
    ItemIcon.buguReisouEffect02Prefab = (GameObject) null;
    ItemIcon.buguReisouEffect03Prefab = (GameObject) null;
  }

  public bool isButtonActive
  {
    set
    {
      if (this.EnabledGear)
        ((Component) ((Component) this.gear.button).transform).gameObject.SetActive(value);
      else
        ((Component) ((Component) this.supply.button).transform).gameObject.SetActive(value);
    }
  }

  public bool isBackActive
  {
    set
    {
      if (this.EnabledGear)
      {
        this.gear.item_back.SetActive(value);
        if (value)
          this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.nonBackSprite;
        else
          this.gear.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[0];
      }
      else
        this.supply.back.SetActive(value);
    }
  }

  public void ShowBottomInfo(ItemSortAndFilter.SORT_TYPES sort)
  {
    if (this.itemInfo == null || this.itemInfo.gear == null)
      return;
    this.clearGearBottomBlinks();
    this.IsRankGear(this.itemInfo);
    this.currSort = sort;
    bool flag = sort == ItemSortAndFilter.SORT_TYPES.RankMax;
    if (sort == ItemSortAndFilter.SORT_TYPES.HistoryGroupNumber)
    {
      this.gear.rank.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.sortRankMaxInfo.SetActive(true);
      this.gear.star.SetActive(false);
      this.gear.sortRankMaxRank.SetTextLocalize(this.itemInfo.gear.history_group_number.ToString().PadLeft(4, '0'));
      this.resetExpireDate();
    }
    else if (this.IsRankReisou(this.itemInfo))
    {
      this.gear.rank.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(true);
      this.gear.sortRankMaxInfo.SetActive(flag);
      this.gear.star.SetActive(!flag);
    }
    else if (this.IsRankGear(this.itemInfo))
    {
      this.gear.rank.SetActive(!flag);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.sortRankMaxInfo.SetActive(flag);
      this.gear.star.SetActive(!flag);
      this.resetExpireDate();
    }
    else
    {
      this.gear.rank.SetActive(false);
      ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
      this.gear.sortRankMaxInfo.SetActive(false);
      this.gear.star.SetActive(true);
      this.resetExpireDate();
    }
  }

  public void resetExpireDate()
  {
    if (this.ItemInfo == null || this.ItemInfo.itemType != GameCore.ItemInfo.ItemType.Gear || !this.EnabledExpireDate)
      return;
    GameObject[] gameObjectArray;
    if (this.gear.bottomWIconNone.activeSelf)
    {
      this.gear.currentBlink = this.gear.bottomWIconNoneBlink;
      gameObjectArray = new GameObject[1]
      {
        this.gear.starWIconNone
      };
    }
    else
    {
      this.gear.currentBlink = this.gear.bottomBlink;
      gameObjectArray = new GameObject[2]
      {
        this.gear.star,
        ((Component) this.gear.sortRankMaxRank).gameObject
      };
    }
    List<GameObject> blinks = new List<GameObject>(2);
    for (int index = 0; index < gameObjectArray.Length; ++index)
    {
      if (gameObjectArray[index].activeSelf)
      {
        blinks.Add(gameObjectArray[index]);
        break;
      }
    }
    DateTime? endAt = this.ItemInfo.gear.expire_date?.end_at;
    if (endAt.HasValue)
    {
      DateTime dateTime1 = ServerTime.NowAppTimeAddDelta();
      DateTime? nullable = endAt;
      DateTime dateTime2 = dateTime1;
      int num;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() > dateTime2 ? 1 : 0) : 0) == 0)
      {
        num = 0;
      }
      else
      {
        nullable = endAt;
        DateTime dateTime3 = dateTime1;
        num = Mathf.Min((int) (nullable.HasValue ? new TimeSpan?(nullable.GetValueOrDefault() - dateTime3) : new TimeSpan?()).Value.TotalDays, 99);
      }
      this.gear.currentBlink.txtRemainDays.SetTextLocalize(num);
      this.gear.currentBlink.remainDays.SetActive(true);
      blinks.Add(this.gear.currentBlink.remainDays);
    }
    else
      this.gear.currentBlink.remainDays.SetActive(false);
    GameObject[] array = blinks.ToArray();
    if (array.Length != 0 && array.Length > 1)
    {
      ((Component) this.gear.currentBlink.blink).gameObject.SetActive(true);
      this.gear.currentBlink.blink.resetBlinks((IEnumerable<GameObject>) blinks);
    }
    else
      this.gear.currentBlink = (ItemIcon.ControlBottomBlink) null;
  }

  private bool IsRankGear(GameCore.ItemInfo item)
  {
    return item != null && !item.isWeaponMaterial && item.gear != null && item.gear.kind.isEquip && item.gear.disappearance_type_GearDisappearanceType == 0;
  }

  private bool IsRankReisou(GameCore.ItemInfo item) => item != null && item.isReisou;

  public void ShowInRecipe()
  {
    this.EnableQuantity(0);
    this.gear.unlimit.SetActive(false);
  }

  public void setEquipReisouDisp()
  {
    this.ShowBottomInfo(ItemSortAndFilter.SORT_TYPES.Rarity);
    this.gear.rank.SetActive(false);
    ((Component) this.gear.blinkReisouRanks).gameObject.SetActive(false);
  }

  public void SetRenseiIcon(bool bg = true, bool num = true)
  {
    this.renseiParent.gameObject.SetActive(true);
    this.renseiNum.gameObject.SetActive(num);
    this.renseiBG.gameObject.SetActive(bg);
    this.renseiJyougen.gameObject.SetActive(false);
    this.HideCounter();
  }

  public void SetRenseiMaterialCount(int count)
  {
    if (!this.renseiNum.gameObject.activeSelf)
      return;
    if (count > 0)
      this.renseiNum.gameObject.GetComponent<UILabel>().SetText(count.ToString() + "個所持");
    else
      this.renseiNum.gameObject.SetActive(false);
  }

  public void SetRenseiMaxUpMark(bool state) => this.renseiJyougen.SetActive(state);

  public void SetRenseiMaterialNum(int num)
  {
    if (num <= 0)
    {
      ((Component) this.renseiMeterialNum[0]).gameObject.SetActive(false);
      ((Component) this.renseiMeterialNum[1]).gameObject.SetActive(false);
      this.renseiMark.SetActive(false);
    }
    else
    {
      this.renseiMark.SetActive(true);
      if (num < 10)
      {
        ((Component) this.renseiMeterialNum[0]).gameObject.SetActive(true);
        this.renseiMeterialNum[0].sprite2D = this.renseiNumSprites[num];
        ((Component) this.renseiMeterialNum[1]).gameObject.SetActive(false);
        ((Component) this.renseiMeterialNum[2]).gameObject.SetActive(false);
      }
      else
      {
        for (int index = 0; index < this.renseiMeterialNum.Length; ++index)
          ((Component) this.renseiMeterialNum[index]).gameObject.SetActive(false);
        int[] array = this.intToArray(num);
        int index1 = array.Length - 1;
        for (int index2 = 0; index2 < array.Length; ++index2)
        {
          ((Component) this.renseiMeterialNum[index1]).gameObject.SetActive(true);
          this.renseiMeterialNum[index1].sprite2D = this.renseiNumSprites[array[index2]];
          --index1;
        }
      }
    }
  }

  private int[] intToArray(int num)
  {
    num = Math.Abs(num);
    int length = num.ToString().Length;
    int[] array = new int[length];
    do
    {
      array[--length] = num % 10;
      num /= 10;
    }
    while (num != 0);
    return array;
  }

  [Conditional("DEBUG_SYMBOL_JINGI")]
  private void debugSetJingi()
  {
  }

  [Conditional("DEBUG_SYMBOL_JINGI")]
  private void debugSetJingi(GearGear target)
  {
    if (this.gear == null || Object.op_Equality((Object) this.gear.root, (Object) null))
      return;
    string str = "deb_jingi";
    Transform transform = this.gear.root.transform.Find(str);
    if (Object.op_Equality((Object) transform, (Object) null))
    {
      UILabel componentInChildren = this.gear.root.GetComponentInChildren<UILabel>(true);
      GameObject self = new GameObject(str);
      self.SetParent(this.gear.root);
      transform = self.transform;
      UILabel uiLabel = self.AddComponent<UILabel>();
      if (Object.op_Inequality((Object) componentInChildren.trueTypeFont, (Object) null))
        uiLabel.trueTypeFont = componentInChildren.trueTypeFont;
      else
        uiLabel.bitmapFont = componentInChildren.bitmapFont;
      ((UIWidget) uiLabel).color = Color.white;
      ((UIRect) uiLabel).alpha = 0.6f;
      uiLabel.fontSize = 84;
      ((UIWidget) uiLabel).height = 84;
      ((UIWidget) uiLabel).width = 84;
      ((UIWidget) uiLabel).depth = (int) byte.MaxValue;
      uiLabel.text = "神";
      uiLabel.effectStyle = (UILabel.Effect) 0;
      uiLabel.applyGradient = false;
    }
    ((Component) transform).gameObject.SetActive(target != null && target.is_jingi);
  }

  [Serializable]
  public class SpriteArray
  {
    [SerializeField]
    private Sprite[] sprites_;
    private Sprite errorSprite_;

    public Sprite this[int i]
    {
      get
      {
        Sprite sprite = this.sprites_ == null || i < 0 || i >= this.sprites_.Length ? (Sprite) null : this.sprites_[i];
        if (Object.op_Inequality((Object) sprite, (Object) null))
          return sprite;
        if (this.sprites_ == null)
          Debug.LogError((object) "SpriteArray is null");
        else
          Debug.LogError((object) string.Format("SpriteArray(Length={0}) index(={1}) is out of range", (object) this.sprites_.Length, (object) i));
        return this.errorSprite();
      }
    }

    private Sprite errorSprite()
    {
      if (Object.op_Inequality((Object) this.errorSprite_, (Object) null))
        return this.errorSprite_;
      Texture2D texture2D = Resources.Load<Texture2D>("Sprites/1x1_alpha0");
      this.errorSprite_ = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      return this.errorSprite_;
    }
  }

  private enum CounterDigits
  {
    OneDigit,
    TwoDigits,
    ThreeDigits,
    FourDigits,
  }

  public enum Sort
  {
    RARITY,
    GETORDER,
    CATEGORY,
    FAVORITE,
    MAX,
  }

  [Serializable]
  public class ControlBottomBlink
  {
    public BlinkSync blink;
    public GameObject remainDays;
    public UILabel txtRemainDays;
  }

  [Serializable]
  public class Gear
  {
    public GameObject root;
    public UI2DSprite icon;
    public LongPressButton button;
    public GameObject favorite;
    public GameObject rank;
    public GameObject star;
    public GameObject sortRankMaxInfo;
    public UI2DSprite sortRankMaxStar;
    public UILabel sortRankMaxRank;
    public GameObject type;
    public GameObject broken;
    public GameObject bottom;
    public GameObject bottomWIconNone;
    public GameObject starWIconNone;
    public GameObject unknown;
    public GameObject forbattle;
    public GameObject fusionPossible;
    public GameObject item_back;
    public GameObject selectedNum;
    public GameObject selectedBack;
    public GameObject selectedCheck;
    public GameObject newGear;
    public GameObject equipPlus;
    public GameObject equipJinkiPlus;
    public GameObject defaultGearTxt;
    public GameObject backGround;
    public GameObject manaseedsDurabilityCount;
    public GameObject manaseedsBreakageRate;
    public GameObject manaseedsDurabilityCount_1;
    public GameObject manaseedsDurabilityCount_10;
    public GameObject manaseedsDurabilityCount_100;
    public GameObject manaseedsBreakageRate_1;
    public GameObject manaseedsBreakageRate_10;
    public GameObject unlimit;
    public AttackClassIcon attackClass;
    public GameObject dynReisouEffect;
    public BlinkSync blinkIcons;
    public BlinkSync blinkReisouRanks;
    public GameObject holyReisouRank;
    public GameObject[] holyReisouRankDigit;
    public GameObject chaosReisouRank;
    public GameObject[] chaosReisouRankDigit;
    public ItemIcon.ControlBottomBlink bottomBlink;
    public ItemIcon.ControlBottomBlink bottomWIconNoneBlink;

    public ItemIcon.ControlBottomBlink currentBlink { get; set; }
  }

  [Serializable]
  public class Supply
  {
    public GameObject root;
    public UI2DSprite icon;
    public LongPressButton button;
    public GameObject bottom;
    public GameObject favorite;
    public GameObject[] rarities;
    public GameObject equals;
    public Transform equalsPos;
    public GameObject forbattle;
    public GameObject back;
    public GameObject[] selectedSupply;
    public GameObject selectedBack;
    public GameObject name;
    public GameObject newSupply;
  }

  public enum BottomMode
  {
    Nothing,
    Visible,
    Visible_wIconNone,
  }
}
