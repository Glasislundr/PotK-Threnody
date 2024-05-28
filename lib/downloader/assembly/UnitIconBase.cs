// Decompiled with JetBrains decompiler
// Type: UnitIconBase
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
public class UnitIconBase : IconPrefabBase
{
  [SerializeField]
  protected LongPressButton button;
  [SerializeField]
  private Sprite[] selectNumberSprites;
  [SerializeField]
  private Sprite[] selectByCheckAndNumberSprites;
  [SerializeField]
  private GameObject selectNumberBase;
  [SerializeField]
  private UI2DSprite selectNumber;
  [SerializeField]
  private GameObject dir_select_check;
  [SerializeField]
  private UI2DSprite slc_tensDigitForSelected;
  [SerializeField]
  private UI2DSprite slc_onesDigitForSelected;
  [SerializeField]
  private UnitIconBase.ButtonType buttonType;
  protected PlayerUnit playerUnit;
  public GameObject checkIcon;
  public GameObject for_battle;
  public GameObject tower_entry;
  public GameObject can_awake;
  public GameObject equip;
  public GameObject selectMarker;
  public GameObject unitSelected;
  public GameObject unitUsed;
  public GameObject unitRental;
  public GameObject overkillers;
  public BlinkSync blinkDeckStatus;
  [SerializeField]
  private GameObject recordNumObj;
  [SerializeField]
  private UILabel recordNumSprite;
  public UI2DSprite icon;
  protected Sprite defaultIconSprite;
  protected bool selected;
  protected bool marker;
  private int selIndex;
  protected bool deckUnit;
  private UnitIcon unitIcon;
  protected Action pressEvent;
  private Action<UnitIconBase> onClick_;
  [SerializeField]
  private Sprite[] friendlyEffectSprites;
  [SerializeField]
  private UI2DSprite friendlyEffect;
  private UnitIconBase.BottomMode bottomMode;
  [SerializeField]
  private Sprite[] bottomBaseSprites;
  [SerializeField]
  private GameObject bottomBaseObject;
  [SerializeField]
  private UI2DSprite bottomBaseSprite;
  [SerializeField]
  protected UILabel txtLabel;
  [SerializeField]
  private UIWidget bottomUnityBase;
  [SerializeField]
  private NGTweenGaugeScale unityGauge;
  [SerializeField]
  private UILabel txtUnityIntValue;
  [SerializeField]
  private UILabel txtUnityBuildupPer;
  [SerializeField]
  protected UI2DSprite rarityStar;
  [SerializeField]
  protected UI2DSprite rarityStarUnitySort;
  private readonly int MARKER_ALPHA_TWEEN = 49;
  public Action markerDisplayFinished;
  [SerializeField]
  private Sprite[] m_digitSprite;
  [SerializeField]
  private Vector3[] m_corssPosList;
  [SerializeField]
  private GameObject m_cross;
  [SerializeField]
  private UI2DSprite m_onesDigit;
  [SerializeField]
  private UI2DSprite m_tensDigit;
  [SerializeField]
  private UI2DSprite m_hundredsDigit;
  [SerializeField]
  private UI2DSprite m_thousandsDigit;
  [SerializeField]
  private Sprite[] m_checkDigitSprite;
  [SerializeField]
  private GameObject m_checkForSelected;
  [SerializeField]
  private UI2DSprite m_tensDigitForSelected;
  [SerializeField]
  private UI2DSprite m_onesDigitForSelected;
  [SerializeField]
  private EarthUnitNumIcon[] earthUnitNumIcon;
  private static readonly Color gradientBottomDefaultColor = new Color(0.698039234f, 0.698039234f, 0.698039234f);
  private static readonly Color textDefaultColor = new Color(0.305882365f, 0.305882365f, 0.305882365f);
  private static readonly Color trustCapTextColor = new Color(1f, 0.3137255f, 0.6509804f);
  private static readonly Color[,] ElementGradientColor = new Color[12, 2]
  {
    {
      Color.white,
      Color.white
    },
    {
      new Color(1f, 1f, 1f),
      new Color(0.698039234f, 0.698039234f, 0.698039234f)
    },
    {
      new Color(1f, 0.509803951f, 0.380392164f),
      new Color(0.8784314f, 0.007843138f, 0.007843138f)
    },
    {
      new Color(0.596078455f, 1f, 0.5019608f),
      new Color(0.08627451f, 0.698039234f, 0.0f)
    },
    {
      new Color(1f, 0.905882359f, 0.02745098f),
      new Color(1f, 0.5411765f, 0.0f)
    },
    {
      new Color(0.349019617f, 1.0739131f, 1f),
      new Color(0.0117647061f, 0.407843143f, 0.8980392f)
    },
    {
      new Color(1f, 1f, 1f),
      new Color(0.698039234f, 0.698039234f, 0.698039234f)
    },
    {
      new Color(1f, 1f, 1f),
      new Color(0.8901961f, 0.858823538f, 0.635294139f)
    },
    {
      new Color(0.9372549f, 0.6509804f, 1f),
      new Color(0.6039216f, 0.0f, 0.9529412f)
    },
    {
      new Color(1f, 1f, 1f),
      new Color(0.698039234f, 0.698039234f, 0.698039234f)
    },
    {
      new Color(1f, 1f, 1f),
      new Color(0.698039234f, 0.698039234f, 0.698039234f)
    },
    {
      new Color(1f, 1f, 1f),
      new Color(0.698039234f, 0.698039234f, 0.698039234f)
    }
  };

  public LongPressButton Button => this.button;

  public Sprite[] SelectNumberSprites
  {
    set => this.selectNumberSprites = value;
    get => this.selectNumberSprites;
  }

  public GameObject SelectNumberBase
  {
    set => this.selectNumberBase = value;
    get => this.selectNumberBase;
  }

  public UI2DSprite SelectNumber
  {
    set => this.selectNumber = value;
    get => this.selectNumber;
  }

  public GameObject Dir_select_check => this.dir_select_check;

  public UnitUnit unit { get; set; }

  protected bool IsRecord => Object.op_Inequality((Object) this.recordNumSprite, (Object) null);

  public int SelIndex
  {
    set => this.selIndex = value;
    get => this.selIndex;
  }

  public UnitIconBase.BottomMode BottomModeValue
  {
    get => this.bottomMode;
    set
    {
      if (!Object.op_Inequality((Object) this.bottomBaseObject, (Object) null) || this.bottomMode >= (UnitIconBase.BottomMode) this.bottomBaseSprites.Length)
        return;
      this.bottomMode = value;
      this.BottomBaseObject = value != UnitIconBase.BottomMode.Nothing;
      this.bottomBaseSprite.sprite2D = this.bottomBaseSprites[(int) this.bottomMode];
      if (this.bottomMode != UnitIconBase.BottomMode.TrustValue || this.unit.ID == 0)
        return;
      ((Component) this.bottomBaseSprite).GetComponent<SwitchUnitComponentBase>()?.SwitchMaterial(this.unit.ID);
    }
  }

  public bool BottomBaseObject
  {
    set
    {
      if (!Object.op_Inequality((Object) this.bottomBaseObject, (Object) null))
        return;
      this.bottomBaseObject.gameObject.SetActive(value);
    }
  }

  public UILabel TxtLabel => this.txtLabel;

  public void setCostText(PlayerUnit playerUnit)
  {
    if (!(playerUnit != (PlayerUnit) null))
      return;
    ((Component) this.txtLabel).transform.localPosition = new Vector3(12f, -1f, 0.0f);
    this.txtLabel.fontSize = 19;
    ((UIWidget) this.txtLabel).SetDimensions(66, 22);
    this.txtLabel.SetTextLocalize(playerUnit.cost);
  }

  public void setLevelText(PlayerUnit playerUnit)
  {
    if (!(playerUnit != (PlayerUnit) null))
      return;
    this.setLevelText(playerUnit.total_level.ToLocalizeNumberText());
  }

  public void setLevelText(string level)
  {
    ((Component) this.txtLabel).transform.localPosition = new Vector3(27f, -1f, 0.0f);
    this.txtLabel.fontSize = 19;
    ((UIWidget) this.txtLabel).SetDimensions(42, 22);
    this.txtLabel.SetTextLocalize(level);
  }

  public UI2DSprite RarityStar => this.rarityStar;

  public void SetRarities(PlayerUnit playerUnit)
  {
    RarityIcon.SetRarity(playerUnit, this.rarityStar, false);
  }

  public void SetRarities(UnitUnit unit) => RarityIcon.SetRarity(unit, this.rarityStar, false);

  public void SetRaritiesUnitySort(PlayerUnit playerUnit)
  {
    RarityIcon.SetRarity(playerUnit, this.rarityStarUnitySort, false);
  }

  public GameObject CheckForSelected => this.m_checkForSelected;

  private void Start()
  {
    if (!Object.op_Inequality((Object) this.icon, (Object) null))
      return;
    this.defaultIconSprite = this.icon.sprite2D;
  }

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

  public PlayerUnit PlayerUnit
  {
    get => this.playerUnit;
    set => this.playerUnit = value;
  }

  public UnitUnit Unit => this.unit;

  public void Select(int index, bool gray = true)
  {
    this.SelIndex = index;
    this.selected = true;
    this.DispNumber(index);
    if (this.unit.IsNormalUnit && Object.op_Inequality((Object) this.checkIcon, (Object) null))
      this.checkIcon.SetActive(false);
    this.Gray = gray;
  }

  private void DispNumber(int index)
  {
    if (this.selectNumberSprites.Length <= index)
      return;
    this.selectNumberBase.SetActive(true);
    this.selectNumber.sprite2D = this.selectNumberSprites[index];
    UI2DSprite selectNumber = this.selectNumber;
    Rect textureRect = this.selectNumberSprites[index].textureRect;
    int width = (int) ((Rect) ref textureRect).width;
    textureRect = this.selectNumberSprites[index].textureRect;
    int height = (int) ((Rect) ref textureRect).height;
    ((UIWidget) selectNumber).SetDimensions(width, height);
  }

  public void SelectByCheckIcon(bool gray = true)
  {
    this.selected = true;
    this.selectNumberBase.SetActive(true);
    if (Object.op_Inequality((Object) this.checkIcon, (Object) null))
      this.checkIcon.SetActive(true);
    this.Gray = gray;
  }

  public void SelectByCheckAndNumber(UnitIconInfo unitIconInfo)
  {
    unitIconInfo.playerUnit.UnitIconInfo = unitIconInfo;
    this.selected = true;
    this.dir_select_check.SetActive(true);
    Sprite sprite1 = (Sprite) null;
    Sprite sprite2 = (Sprite) null;
    if (unitIconInfo.isNormalUnit)
    {
      sprite1 = (Sprite) null;
      sprite2 = this.selectByCheckAndNumberSprites[1];
    }
    else
    {
      int selectedCount = unitIconInfo.SelectedCount;
      switch (selectedCount == 0 ? 1 : (int) Mathf.Log10((float) selectedCount) + 1)
      {
        case 1:
          sprite1 = (Sprite) null;
          sprite2 = this.selectByCheckAndNumberSprites[selectedCount];
          break;
        case 2:
          int num = selectedCount;
          int index = num % 10;
          sprite1 = this.selectByCheckAndNumberSprites[num / 10 % 10];
          sprite2 = this.selectByCheckAndNumberSprites[index];
          break;
        default:
          Debug.LogError((object) "SelectByCheckAndNumber: 想定していない桁数です");
          break;
      }
    }
    this.slc_tensDigitForSelected.sprite2D = sprite1;
    this.slc_onesDigitForSelected.sprite2D = sprite2;
    this.Gray = true;
  }

  public void HideCheckIcon()
  {
    this.selectNumberBase.SetActive(false);
    if (!Object.op_Inequality((Object) this.checkIcon, (Object) null))
      return;
    this.checkIcon.SetActive(false);
  }

  public void DispEarthUnitNumberIcon(int num, bool isGorgeous, bool isLeft)
  {
    if (isLeft)
    {
      ((Component) this.earthUnitNumIcon[0]).gameObject.SetActive(true);
      this.earthUnitNumIcon[0].SetNumIcon(num, isGorgeous);
    }
    else
    {
      ((Component) this.earthUnitNumIcon[1]).gameObject.SetActive(true);
      this.earthUnitNumIcon[1].SetNumIcon(num, isGorgeous);
    }
  }

  public void HiddenEarthUnitNumberIcon(bool isLeft)
  {
    ((IEnumerable<EarthUnitNumIcon>) this.earthUnitNumIcon).ForEach<EarthUnitNumIcon>((Action<EarthUnitNumIcon>) (x => ((Component) x).gameObject.SetActive(false)));
  }

  public bool DeckUnit
  {
    get => this.deckUnit;
    set => this.deckUnit = value;
  }

  public bool Selected => this.selected;

  public bool ForBattle
  {
    get => this.for_battle.activeSelf;
    set => this.for_battle.SetActive(value);
  }

  public bool UnitSelected
  {
    get
    {
      return Object.op_Inequality((Object) this.unitSelected, (Object) null) && this.unitSelected.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.unitSelected, (Object) null))
        return;
      this.unitSelected.SetActive(value);
    }
  }

  public bool TowerEntry
  {
    get
    {
      return Object.op_Inequality((Object) this.tower_entry, (Object) null) && this.tower_entry.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.tower_entry, (Object) null))
        return;
      this.tower_entry.SetActive(value);
    }
  }

  public bool CanAwake
  {
    get
    {
      return Object.op_Inequality((Object) this.can_awake, (Object) null) && this.can_awake.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.can_awake, (Object) null))
        return;
      this.can_awake.SetActive(value);
    }
  }

  public bool UnitUsed
  {
    get => Object.op_Inequality((Object) this.unitUsed, (Object) null) && this.unitUsed.activeSelf;
    set
    {
      if (!Object.op_Inequality((Object) this.unitUsed, (Object) null))
        return;
      this.unitUsed.SetActive(value);
    }
  }

  public bool UnitRental
  {
    get
    {
      return Object.op_Inequality((Object) this.unitRental, (Object) null) && this.unitRental.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.unitRental, (Object) null))
        return;
      this.unitRental.SetActive(value);
    }
  }

  public bool Overkillers
  {
    get
    {
      return Object.op_Inequality((Object) this.overkillers, (Object) null) && this.overkillers.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.overkillers, (Object) null))
        return;
      this.overkillers.SetActive(value);
    }
  }

  public void SetupDeckStatusBlink()
  {
    if (Object.op_Equality((Object) this.blinkDeckStatus, (Object) null))
      return;
    bool[] flagArray = new bool[6]
    {
      this.ForBattle,
      this.TowerEntry,
      this.CanAwake,
      this.UnitUsed,
      this.UnitRental,
      this.Overkillers
    };
    int num = 0;
    foreach (bool flag in flagArray)
      num = flag ? num + 1 : num;
    if (num >= 2)
    {
      List<GameObject> blinks = new List<GameObject>();
      if (this.ForBattle)
        blinks.Add(this.for_battle);
      if (this.TowerEntry)
        blinks.Add(this.tower_entry);
      if (this.CanAwake)
        blinks.Add(this.can_awake);
      if (this.UnitUsed)
        blinks.Add(this.unitUsed);
      if (this.UnitRental)
        blinks.Add(this.unitRental);
      if (this.Overkillers)
        blinks.Add(this.overkillers);
      this.blinkDeckStatus.resetBlinks((IEnumerable<GameObject>) blinks);
    }
    else
      this.blinkDeckStatus.resetBlinks();
    ((Component) this.blinkDeckStatus).gameObject.SetActive(num > 0);
  }

  public bool Equip
  {
    get => this.equip.activeSelf;
    set => this.equip.SetActive(value);
  }

  public UnitIcon UnitIcon
  {
    get => this.unitIcon;
    set => this.unitIcon = value;
  }

  public bool SelectMarker
  {
    get => !Object.op_Equality((Object) this.selectMarker, (Object) null) && this.marker;
    set
    {
      if (!Object.op_Inequality((Object) this.selectMarker, (Object) null))
        return;
      this.marker = value;
      this.selectMarker.SetActive(value);
      if (!value)
        return;
      UITweener[] array = ((IEnumerable<UITweener>) NGTween.findTweenersGroup(this.selectMarker, 49, false)).Where<UITweener>((Func<UITweener, bool>) (x => x.style != 1)).ToArray<UITweener>();
      if (array.Length == 0)
        return;
      array[0].SetOnFinished((EventDelegate.Callback) (() =>
      {
        if (this.markerDisplayFinished == null)
          return;
        this.markerDisplayFinished();
      }));
      NGTween.playTweens(array, this.MARKER_ALPHA_TWEEN);
    }
  }

  public void Deselect()
  {
    this.selected = false;
    this.selectNumberBase.SetActive(false);
    if (Object.op_Inequality((Object) this.dir_select_check, (Object) null))
      this.dir_select_check.SetActive(false);
    if (Object.op_Inequality((Object) this.checkIcon, (Object) null))
      this.checkIcon.SetActive(false);
    this.Gray = false;
  }

  public void SetPressEvent(Action evt) => this.pressEvent = evt;

  public void setLongPressEvent(Action long_press_event)
  {
    EventDelegate.Set(this.Button.onLongPress, new EventDelegate.Callback(((Action) (() =>
    {
      if (this.pressEvent != null)
        this.pressEvent();
      long_press_event();
    })).Invoke));
  }

  public Coroutine StartCoroutine(IEnumerator e)
  {
    return Singleton<NGSceneManager>.GetInstance().StartCoroutine(e);
  }

  public virtual IEnumerator SetUnit(PlayerUnit playerUnit, CommonElement element, bool isGray = false)
  {
    this.playerUnit = playerUnit;
    this.unit = playerUnit.unit;
    yield break;
  }

  public virtual IEnumerator SetUnit(UnitUnit unit, CommonElement element, bool isGray = false)
  {
    this.unit = unit;
    yield break;
  }

  public virtual IEnumerator SetUnitWithLongPressAction(
    UnitUnit unit,
    Action buttonEvent,
    bool isGray = false)
  {
    this.unit = unit;
    yield break;
  }

  public virtual IEnumerator SetMaterialUnit(
    PlayerUnit playerUnit,
    bool isNew,
    PlayerUnit[] playerUnits)
  {
    this.playerUnit = playerUnit;
    yield break;
  }

  public virtual IEnumerator SetMaterialUnit(
    PlayerUnit playerUnit,
    bool isNew,
    PlayerUnit[] playerUnits,
    bool isTrust)
  {
    this.playerUnit = playerUnit;
    yield break;
  }

  public virtual IEnumerator SetMaterialUnit(
    PlayerUnit playerUnit,
    PlayerUnit basePlayerUnit,
    bool isNew,
    PlayerUnit[] playerUnits,
    bool isTrust)
  {
    IEnumerator e = this.SetMaterialUnit(playerUnit, isNew, playerUnits);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator SetPlayerUnit(
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    PlayerUnit basePlayerUnit = null,
    bool isMaterial = false,
    bool isMemory = false)
  {
    this.playerUnit = playerUnit;
    yield break;
  }

  public virtual void SetRemoveButton() => this.BottomBaseObject = false;

  public Action<UnitIconBase> onClick
  {
    get => this.onClick_;
    set
    {
      this.onClick_ = value;
      if (this.onClick_ == null || !Object.op_Inequality((Object) this.Button, (Object) null))
        return;
      EventDelegate.Set(this.Button.onClick, (EventDelegate.Callback) (() => this.onClick_(this)));
    }
  }

  public Action<UnitIconBase> onLongPress
  {
    set
    {
      if (!Object.op_Inequality((Object) this.Button, (Object) null))
        return;
      EventDelegate.Set(this.Button.onLongPress, (EventDelegate.Callback) (() => value(this)));
    }
  }

  public void SetFriendlyEffect(bool bl, bool isEarthMode = false)
  {
    ((Component) this.friendlyEffect).gameObject.SetActive(bl);
    if (bl)
    {
      if (isEarthMode)
        this.friendlyEffect.sprite2D = this.friendlyEffectSprites[1];
      else
        this.friendlyEffect.sprite2D = this.friendlyEffectSprites[0];
    }
    else
      this.friendlyEffect.sprite2D = this.friendlyEffectSprites[0];
  }

  private void setNumTextGradientColor(Color top, Color bottom)
  {
    this.txtLabel.gradientTop = top;
    this.txtLabel.gradientBottom = bottom;
  }

  private void setTrustTextGradientColor(Color top, Color bottom)
  {
    this.txtLabel.gradientTop = top;
    this.txtLabel.gradientBottom = bottom;
  }

  private void SetTrustTextColor(Color color) => ((UIWidget) this.txtLabel).color = color;

  private void SetCombatText(string str)
  {
    ((Component) this.txtLabel).transform.localPosition = new Vector3(12f, -1f, 0.0f);
    this.txtLabel.fontSize = 19;
    ((UIWidget) this.txtLabel).SetDimensions(66, 22);
    this.txtLabel.SetTextLocalize(str);
  }

  public void setCombatText(int value) => this.setCombatText(value.ToString());

  public void setCombatText(string str = "")
  {
    this.setNumTextGradientColor(Color.white, UnitIconBase.gradientBottomDefaultColor);
    this.SetCombatText(str);
  }

  public void setTrustText(float value = 0.0f, float max = 0.0f)
  {
    if (Object.op_Equality((Object) this.txtLabel, (Object) null))
      return;
    this.setTrustTextGradientColor(Color.white, UnitIconBase.gradientBottomDefaultColor);
    if ((double) max > 0.0 && Mathf.Approximately(value, max))
      this.SetTrustTextColor(UnitIconBase.trustCapTextColor);
    else
      this.SetTrustTextColor(UnitIconBase.textDefaultColor);
    ((Component) this.txtLabel).transform.localPosition = new Vector3(20f, 0.0f, 0.0f);
    this.txtLabel.fontSize = 19;
    ((UIWidget) this.txtLabel).SetDimensions(54, 22);
    this.txtLabel.SetTextLocalize(string.Format("{0}{1}", (object) (Math.Round((double) value * 100.0) / 100.0), (object) Consts.GetInstance().PERCENT));
  }

  public void ShowBottomInfosLevelOnly()
  {
    ((Component) this.rarityStar).gameObject.SetActive(false);
  }

  public static UnitIconBase.BottomMode GetBottomMode(UnitUnit unit, PlayerUnit playerUnit)
  {
    if (playerUnit != (PlayerUnit) null && Player.Current != null && Player.Current.IsCalledUnit(unit) && ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Any<PlayerUnit>((Func<PlayerUnit, bool>) (p => p == playerUnit)))
      return UnitIconBase.BottomMode.Call;
    return !unit.awake_unit_flag ? UnitIconBase.BottomMode.Normal : UnitIconBase.BottomMode.AwakeUnit;
  }

  public static UnitIconBase.BottomMode GetBottomModeLevel(UnitUnit unit, PlayerUnit playerUnit)
  {
    if (playerUnit != (PlayerUnit) null && Player.Current != null && Player.Current.IsCalledUnit(unit) && ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Any<PlayerUnit>((Func<PlayerUnit, bool>) (p => p == playerUnit)))
      return UnitIconBase.BottomMode.CallLevel;
    return !unit.awake_unit_flag ? UnitIconBase.BottomMode.Level : UnitIconBase.BottomMode.AwakeUnitLevel;
  }

  public void ShowBottomInfos(UnitSortAndFilter.SORT_TYPES type)
  {
    UnitUnit unit = (UnitUnit) null;
    if (this.unit != null)
      unit = this.unit;
    else if (this.PlayerUnit != (PlayerUnit) null)
      unit = this.PlayerUnit.unit;
    if (type == UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy || type == UnitSortAndFilter.SORT_TYPES.Rarity || type == UnitSortAndFilter.SORT_TYPES.GetOrder || type == UnitSortAndFilter.SORT_TYPES.UnitID || type == UnitSortAndFilter.SORT_TYPES.Attribute || type == UnitSortAndFilter.SORT_TYPES.UnitName || type == UnitSortAndFilter.SORT_TYPES.Illustrator || type == UnitSortAndFilter.SORT_TYPES.VoiceActor || type == UnitSortAndFilter.SORT_TYPES.Level)
    {
      this.BottomModeValue = UnitIconBase.GetBottomModeLevel(unit, this.PlayerUnit);
    }
    else
    {
      if (type != UnitSortAndFilter.SORT_TYPES.Cost && type != UnitSortAndFilter.SORT_TYPES.Trust && (type == UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy || type == UnitSortAndFilter.SORT_TYPES.Level || type == UnitSortAndFilter.SORT_TYPES.Rarity || type == UnitSortAndFilter.SORT_TYPES.Cost || type == UnitSortAndFilter.SORT_TYPES.GetOrder || type == UnitSortAndFilter.SORT_TYPES.UnitID || type == UnitSortAndFilter.SORT_TYPES.Attribute || type == UnitSortAndFilter.SORT_TYPES.Trust))
        return;
      this.BottomModeValue = UnitIconBase.GetBottomMode(unit, this.PlayerUnit);
    }
  }

  public void ShowBottomInfo(UnitSortAndFilter.SORT_TYPES sort)
  {
    if (this.PlayerUnit == (PlayerUnit) null)
      return;
    this.ShowBottomInfos(sort);
    if (!this.Gray)
    {
      if (this.buttonType == UnitIconBase.ButtonType.Sea)
        ((UIWidget) this.txtLabel).color = new Color(0.3f, 0.3f, 0.3f);
      else
        ((UIWidget) this.txtLabel).color = Color.white;
    }
    if (Object.op_Implicit((Object) this.bottomBaseSprite))
      ((Component) this.bottomBaseSprite).gameObject.SetActive(true);
    if (Object.op_Implicit((Object) this.bottomUnityBase))
      ((Component) this.bottomUnityBase).gameObject.SetActive(false);
    GameObject target = (GameObject) null;
    switch (sort)
    {
      case UnitSortAndFilter.SORT_TYPES.Cost:
        this.setCostText(this.PlayerUnit);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.FightingPower:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Combat);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.PhysicalAttack:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).PhysicalAttack);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.MagicAttack:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).MagicAttack);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Defence:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).PhysicalDefense);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.MagicDefence:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).MagicDefense);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Hit:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Hit);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Critical:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Critical);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Avoid:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Evasion);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Speed:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Agility);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Dexterity:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Dexterity);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Luck:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Luck);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.WeaponProficiency:
        this.setCombatText(this.PlayerUnit.unit.kind_GearKind != 8 ? this.PlayerUnit.weaponProficiency().proficiency : "-");
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.ArmorProficiency:
        this.setCombatText(this.PlayerUnit.shildProficiency().proficiency);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.HP:
        this.setCombatText(Judgement.NonBattleParameter.FromPlayerUnit(this.PlayerUnit).Hp);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Breakthrough:
        string str1 = "[d4d4d4]";
        string str2 = Consts.GetInstance().COMMON_NOVALUE;
        if (this.PlayerUnit.breakthrough_count > 0)
        {
          str1 = "[15c8ee]";
          str2 = "";
          for (int index = 0; index < this.PlayerUnit.breakthrough_count; ++index)
            str2 += Consts.GetInstance().SORT_POPUP_BREAKTHROUGH_COUNT;
        }
        ((Component) this.txtLabel).transform.localPosition = new Vector3(12f, 0.0f, 0.0f);
        ((UIWidget) this.txtLabel).SetDimensions(66, 22);
        this.txtLabel.fontSize = 16;
        this.txtLabel.SetTextLocalize(str1 + str2 + "[-]");
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.UnityValue:
      case UnitSortAndFilter.SORT_TYPES.PossessionNumber:
        if (Object.op_Inequality((Object) this.bottomBaseObject, (Object) null))
        {
          ((Component) this.bottomBaseSprite).gameObject.SetActive(false);
          ((Component) this.bottomUnityBase).gameObject.SetActive(true);
          this.SetRaritiesUnitySort(this.PlayerUnit);
          this.txtUnityIntValue.SetTextLocalize(((double) this.PlayerUnit.unityTotal < (double) PlayerUnit.GetUnityValueMax() ? (object) "[FFFFFF]" : (object) "[FFFF00]").ToString() + (object) this.playerUnit.unityInt + "[-]");
          this.txtUnityBuildupPer.SetTextLocalize(this.playerUnit.unityDec.ToString() + "%");
          this.unityGauge.setValue(this.playerUnit.unityDec, 99, false);
          target = ((Component) this.txtUnityBuildupPer).gameObject;
          break;
        }
        break;
      case UnitSortAndFilter.SORT_TYPES.TrustRate:
        if (this.PlayerUnit.unit.trust_target_flag)
        {
          this.setCombatText((Math.Round((double) this.PlayerUnit.trust_rate * 100.0) / 100.0).ToString("f2").PadLeft(5, '0'));
          this.txtLabel.fontSize = 19;
        }
        else
          this.setCombatText(Consts.GetInstance().COMMON_NOVALUE);
        this.txtLabel.fontSize = 19;
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.AverageRisingValue:
        this.setCombatText(this.PlayerUnit.GetUnitAverageRisingValue().ToString().ToConverter());
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.HistoryGroupNumber:
        string str3 = this.PlayerUnit.unit.history_group_number.ToString();
        this.setCombatText(str3.Substring(1, str3.Length - 1));
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.Trust:
        this.setTrustText(this.PlayerUnit.trust_rate, this.PlayerUnit.trust_max_rate);
        target = ((Component) this.txtLabel).gameObject;
        break;
      case UnitSortAndFilter.SORT_TYPES.MaxLevel:
        this.setCombatText(this.playerUnit.total_level.ToLocalizeNumberText() + "/" + this.playerUnit.total_max_level.ToLocalizeNumberText());
        target = ((Component) this.txtLabel).gameObject;
        break;
      default:
        this.setLevelText(this.PlayerUnit);
        target = ((Component) this.txtLabel).gameObject;
        break;
    }
    this.showBottomInfoEx(sort, target);
  }

  protected virtual void showBottomInfoEx(UnitSortAndFilter.SORT_TYPES sort, GameObject target)
  {
  }

  public void SetCounter(int number, bool isReward = false, bool isDisplayBelowZero = false)
  {
    if (this.unit != null)
    {
      if (this.unit.IsNormalUnit && !isReward)
      {
        this.HideCounter();
      }
      else
      {
        if (number > 9999)
          number = 9999;
        int num1 = number % 10;
        int num2 = number % 100 / 10;
        int num3 = number % 1000 / 100;
        int num4 = number % 10000 / 1000;
        UnitIconBase.CounterDigits digits = UnitIconBase.CounterDigits.OneDigit;
        if (number < 1 && !isDisplayBelowZero)
        {
          this.HideCounter();
        }
        else
        {
          if (number >= 10)
            digits = number >= 100 ? (number >= 1000 ? UnitIconBase.CounterDigits.FourDigits : UnitIconBase.CounterDigits.ThreeDigits) : UnitIconBase.CounterDigits.TwoDigits;
          this.SetCross(digits);
          this.SetOnesDigit(num1);
          this.SetTensDigit(num2, digits);
          this.SetHundredsDigit(num3, digits);
          this.SetThousandsDigit(num4, digits);
        }
      }
    }
    else
      this.HideCounter();
  }

  public void SetCountPositionY(float y)
  {
    Transform transform1 = this.m_cross.transform;
    transform1.localPosition = new Vector3(transform1.localPosition.x, y, transform1.localPosition.z);
    Transform transform2 = ((Component) this.m_onesDigit).transform;
    transform2.localPosition = new Vector3(transform2.localPosition.x, y, transform2.localPosition.z);
    Transform transform3 = ((Component) this.m_tensDigit).transform;
    transform3.localPosition = new Vector3(transform3.localPosition.x, y, transform3.localPosition.z);
    Transform transform4 = ((Component) this.m_hundredsDigit).transform;
    transform4.localPosition = new Vector3(transform4.localPosition.x, y, transform4.localPosition.z);
    Transform transform5 = ((Component) this.m_thousandsDigit).transform;
    transform5.localPosition = new Vector3(transform5.localPosition.x, y, transform5.localPosition.z);
  }

  private void SetOnesDigit(int num)
  {
    if (num <= -1 || num >= 10 || !Object.op_Inequality((Object) this.m_onesDigit, (Object) null))
      return;
    ((Component) this.m_onesDigit).gameObject.SetActive(true);
    this.m_onesDigit.sprite2D = this.m_digitSprite[num];
    UI2DSprite onesDigit = this.m_onesDigit;
    Rect textureRect1 = this.m_digitSprite[num].textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = this.m_digitSprite[num].textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) onesDigit).SetDimensions(width, height);
  }

  private void SetTensDigit(int num, UnitIconBase.CounterDigits digits)
  {
    if (num <= -1 || num >= 10 || !Object.op_Inequality((Object) this.m_tensDigit, (Object) null))
      return;
    if (UnitIconBase.CounterDigits.TwoDigits > digits)
    {
      ((Component) this.m_tensDigit).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.m_tensDigit).gameObject.SetActive(true);
      this.m_tensDigit.sprite2D = this.m_digitSprite[num];
      UI2DSprite tensDigit = this.m_tensDigit;
      Rect textureRect1 = this.m_digitSprite[num].textureRect;
      int width = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = this.m_digitSprite[num].textureRect;
      int height = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) tensDigit).SetDimensions(width, height);
    }
  }

  private void SetHundredsDigit(int num, UnitIconBase.CounterDigits digits)
  {
    if (num <= -1 || num >= 10 || !Object.op_Inequality((Object) this.m_hundredsDigit, (Object) null))
      return;
    if (UnitIconBase.CounterDigits.ThreeDigits > digits)
    {
      ((Component) this.m_hundredsDigit).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.m_hundredsDigit).gameObject.SetActive(true);
      this.m_hundredsDigit.sprite2D = this.m_digitSprite[num];
      UI2DSprite hundredsDigit = this.m_hundredsDigit;
      Rect textureRect1 = this.m_digitSprite[num].textureRect;
      int width = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = this.m_digitSprite[num].textureRect;
      int height = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) hundredsDigit).SetDimensions(width, height);
    }
  }

  private void SetThousandsDigit(int num, UnitIconBase.CounterDigits digits)
  {
    if (num <= -1 || num >= 10 || !Object.op_Inequality((Object) this.m_thousandsDigit, (Object) null))
      return;
    if (UnitIconBase.CounterDigits.FourDigits > digits)
    {
      ((Component) this.m_thousandsDigit).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.m_thousandsDigit).gameObject.SetActive(true);
      this.m_thousandsDigit.sprite2D = this.m_digitSprite[num];
      UI2DSprite thousandsDigit = this.m_thousandsDigit;
      Rect textureRect1 = this.m_digitSprite[num].textureRect;
      int width = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = this.m_digitSprite[num].textureRect;
      int height = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) thousandsDigit).SetDimensions(width, height);
    }
  }

  private void SetCross(UnitIconBase.CounterDigits digits)
  {
    if (!Object.op_Inequality((Object) this.m_cross, (Object) null))
      return;
    this.m_cross.transform.localPosition = this.m_corssPosList[(int) digits];
    this.m_cross.SetActive(true);
  }

  private void HideCounter()
  {
    if (Object.op_Inequality((Object) this.m_cross, (Object) null))
      this.m_cross.SetActive(false);
    if (Object.op_Inequality((Object) this.m_onesDigit, (Object) null))
      ((Component) this.m_onesDigit).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.m_tensDigit, (Object) null))
      ((Component) this.m_tensDigit).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.m_hundredsDigit, (Object) null))
      ((Component) this.m_hundredsDigit).gameObject.SetActive(false);
    if (!Object.op_Inequality((Object) this.m_thousandsDigit, (Object) null))
      return;
    ((Component) this.m_thousandsDigit).gameObject.SetActive(false);
  }

  public void SetSelectionCounter(int selectedQuantity)
  {
    if (selectedQuantity < 0 || selectedQuantity > 99)
    {
      Debug.LogWarning((object) ("Illegal parameter specified for SetSelectionCounter(). Paramerter: " + selectedQuantity.ToString()));
    }
    else
    {
      bool flag = false;
      if (this.unit == null)
        flag = true;
      else if (this.unit.IsNormalUnit)
        flag = true;
      if (flag)
      {
        this.HideSelectedQuantityCounter();
        if (!Object.op_Inequality((Object) this.m_checkForSelected, (Object) null))
          return;
        this.m_checkForSelected.SetActive(false);
      }
      else
      {
        if (selectedQuantity > 0)
        {
          this.SetOnesDigitNumberForSelectedQuantity(selectedQuantity % 10);
          this.SetTensDigitNumberForSelectedQuantity(selectedQuantity / 10);
          this.Gray = true;
        }
        else
        {
          this.HideSelectedQuantityCounter();
          this.Gray = false;
        }
        if (!Object.op_Inequality((Object) this.m_checkForSelected, (Object) null))
          return;
        this.m_checkForSelected.SetActive(selectedQuantity > 0);
      }
    }
  }

  private void HideSelectedQuantityCounter()
  {
    if (Object.op_Inequality((Object) this.m_onesDigitForSelected, (Object) null))
      ((Component) this.m_onesDigitForSelected).gameObject.SetActive(false);
    if (!Object.op_Inequality((Object) this.m_tensDigitForSelected, (Object) null))
      return;
    ((Component) this.m_tensDigitForSelected).gameObject.SetActive(false);
  }

  private void SetTensDigitNumberForSelectedQuantity(int number)
  {
    if (!Object.op_Inequality((Object) this.m_tensDigitForSelected, (Object) null))
      return;
    this.m_tensDigitForSelected.sprite2D = this.m_checkDigitSprite[number];
    UI2DSprite digitForSelected = this.m_tensDigitForSelected;
    Rect textureRect1 = this.m_checkDigitSprite[number].textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = this.m_checkDigitSprite[number].textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) digitForSelected).SetDimensions(width, height);
    ((Component) this.m_tensDigitForSelected).gameObject.SetActive(true);
  }

  private void SetOnesDigitNumberForSelectedQuantity(int number)
  {
    if (!Object.op_Inequality((Object) this.m_onesDigitForSelected, (Object) null))
      return;
    this.m_onesDigitForSelected.sprite2D = this.m_checkDigitSprite[number];
    UI2DSprite digitForSelected = this.m_onesDigitForSelected;
    Rect textureRect1 = this.m_checkDigitSprite[number].textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = this.m_checkDigitSprite[number].textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) digitForSelected).SetDimensions(width, height);
    ((Component) this.m_onesDigitForSelected).gameObject.SetActive(true);
  }

  public void SetRecordObj(GameObject obj)
  {
    if (!Object.op_Equality((Object) this.recordNumSprite, (Object) null))
      return;
    this.recordNumSprite = obj.Clone(this.recordNumObj.transform).GetComponent<UILabel>();
  }

  public void SetRecordNum()
  {
    if (!(this.PlayerUnit != (PlayerUnit) null) || !Object.op_Inequality((Object) this.recordNumSprite, (Object) null))
      return;
    int? nullable1 = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? ((IEnumerable<int?>) PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids).FirstIndexOrNull<int?>((Func<int?, bool>) (x =>
    {
      int? nullable2 = x;
      int id = this.playerUnit.id;
      return nullable2.GetValueOrDefault() == id & nullable2.HasValue;
    })) : new int?();
    this.recordNumObj.SetActive(nullable1.HasValue);
    if (!nullable1.HasValue)
      return;
    this.recordNumSprite.SetTextLocalize(nullable1.Value + 1);
  }

  private enum ButtonType
  {
    Normal,
    Sea,
  }

  private enum FriendlyEffectSpriteID
  {
    normal,
    earth,
  }

  public enum BottomMode
  {
    Nothing,
    Normal,
    Friendly,
    FriendlyEarth,
    Level,
    AwakeUnit,
    AwakeUnitLevel,
    TrustValue,
    Call,
    CallLevel,
  }

  private enum EarthNumIndex
  {
    Left,
    Right,
  }

  private enum CounterDigits
  {
    OneDigit,
    TwoDigits,
    ThreeDigits,
    FourDigits,
  }
}
