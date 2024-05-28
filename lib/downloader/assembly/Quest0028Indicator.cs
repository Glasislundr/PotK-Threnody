// Decompiled with JetBrains decompiler
// Type: Quest0028Indicator
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
public class Quest0028Indicator : MonoBehaviour
{
  [SerializeField]
  private GameObject[] slcTextGuests;
  [SerializeField]
  protected GameObject[] linkCharacters;
  [SerializeField]
  protected List<Quest0028Indicator.GearList> linkGears = new List<Quest0028Indicator.GearList>();
  [NonSerialized]
  public bool disableUnitDetail;
  [NonSerialized]
  public int maxPlayer;
  private GameObject normalPrefab;
  private GameObject gearIconPrefab;
  private const float scale = 1f;
  private const float item_scale = 1f;
  private const float item_scale_sea = 0.65f;
  private PlayerUnit[] DeckUnitData;
  private HashSet<int> regulationUnitIds;
  private bool isLimitation;
  private bool isUserDeckStage;
  private UnitGender genderRestriction;
  private const int DECK_UNIT_MAX = 5;
  private List<List<GameObject>> changeGearList = new List<List<GameObject>>();
  private bool isDisabledAutoDestroy_;

  public PlayerUnit[] deckUnitData => this.DeckUnitData;

  public bool isCompletedOverkillersDeck { get; private set; } = true;

  public bool isCustomDeck { get; private set; }

  public bool isBrokenGear { get; private set; }

  public bool isAnyUsedUnit { get; private set; }

  public IEnumerator InitPlayerDeck(
    DeckInfo playerDeck,
    PlayerExtraQuestS extraQuest,
    PlayerStoryQuestS storyQuest,
    PlayerCharacterQuestS charQuest,
    PlayerQuestSConverter convertQuest,
    PlayerSeaQuestS seaQuest,
    DeckInfo[] regulationDeck,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    BattleStageGuest[] guest,
    int battleStageID,
    GameObject prefabUnitIcon,
    GameObject prefabItemIcon)
  {
    this.normalPrefab = prefabUnitIcon;
    this.gearIconPrefab = prefabItemIcon;
    this.isLimitation = storyQuest != null && Quest0028Menu.IsStoryLimitation(storyQuest) || extraQuest != null && Quest0028Menu.IsExtraLimitation(extraQuest) || charQuest != null && Quest0028Menu.IsCharaLimitation(charQuest) || convertQuest != null && Quest0028Menu.IsConvertLimitation(convertQuest) || seaQuest != null && Quest0028Menu.IsSeaLimitation(seaQuest);
    if (this.isLimitation)
      this.regulationUnitIds = new HashSet<int>((IEnumerable<int>) Array.Find<DeckInfo>(regulationDeck, (Predicate<DeckInfo>) (x => x.deck_number == playerDeck.deck_number)).player_unit_ids);
    this.isUserDeckStage = extraQuest != null && extraQuest.extra_quest_area == 3;
    this.genderRestriction = extraQuest != null ? extraQuest.quest_extra_s.gender_restriction : (storyQuest != null ? storyQuest.quest_story_s.gender_restriction : (charQuest != null ? charQuest.quest_character_s.gender_restriction : (convertQuest != null ? convertQuest.questS.gender_restriction : (seaQuest != null ? seaQuest.quest_sea_s.gender_restriction : UnitGender.none))));
    this.isCustomDeck = playerDeck.isCustom;
    this.isBrokenGear = false;
    this.isAnyUsedUnit = false;
    this.changeGearList.Clear();
    ((IEnumerable<GameObject>) this.slcTextGuests).ToggleOnceEx(-1);
    this.maxPlayer = ((IEnumerable<KeyValuePair<int, BattleStagePlayer>>) MasterData.BattleStagePlayer.Where<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == battleStageID)).ToArray<KeyValuePair<int, BattleStagePlayer>>()).Count<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.deck_position != Consts.GetInstance().DECK_POSITION_FRIEND));
    this.maxPlayer = Mathf.Min(this.maxPlayer, playerDeck.player_units.Length);
    this.DeckUnitData = new PlayerUnit[5];
    int guestIdx = 0;
    bool[] posGuests = Enumerable.Repeat<bool>(true, 5).ToArray<bool>();
    IEnumerator e;
    for (int i = 0; i < playerDeck.player_units.Length; ++i)
    {
      if (guest != null && guest.Length != 0 && ((IEnumerable<BattleStageGuest>) guest).Any<BattleStageGuest>((Func<BattleStageGuest, bool>) (x => x.deck_position == i + 1)))
      {
        this.DeckUnitData[i] = PlayerUnit.FromGuest(guest[guestIdx]);
        ++guestIdx;
      }
      else
      {
        this.DeckUnitData[i] = playerDeck.player_units[i];
        if (i < this.maxPlayer)
          posGuests[i] = false;
      }
      e = this.LoadUnitPrefab(i, this.DeckUnitData[i], tables, unitBonus, i >= this.maxPlayer);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    for (int i = playerDeck.player_units.Length; i < 5; ++i)
    {
      e = this.LoadUnitPrefab(i, (PlayerUnit) null, tables, unitBonus, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    bool bCompleted;
    if (this.isCustomDeck)
      OverkillersUtil.checkCompletedCustomDeck(this.DeckUnitData, out bCompleted, ignores: posGuests);
    else
      OverkillersUtil.checkCompletedDeck(this.DeckUnitData, out bCompleted, ignores: posGuests);
    this.isCompletedOverkillersDeck = bCompleted;
  }

  public IEnumerator InitDeck(
    DeckInfo deckInfo,
    int[] usedUnitIds,
    GameObject prefabUnitIcon,
    GameObject prefabItemIcon)
  {
    this.normalPrefab = prefabUnitIcon;
    this.gearIconPrefab = prefabItemIcon;
    this.isCustomDeck = deckInfo.isCustom;
    this.isBrokenGear = false;
    this.isAnyUsedUnit = false;
    this.isDisabledAutoDestroy_ = true;
    ((IEnumerable<GameObject>) this.slcTextGuests).ToggleOnceEx(-1);
    this.DeckUnitData = new PlayerUnit[deckInfo.member_limit];
    int i;
    IEnumerator e;
    for (i = 0; i < deckInfo.player_units.Length; ++i)
    {
      this.DeckUnitData[i] = deckInfo.player_units[i];
      PlayerUnit playerUnit = this.DeckUnitData[i];
      int id = (object) playerUnit != null ? playerUnit.id : 0;
      e = this.LoadUnitPrefab(i, this.DeckUnitData[i], ((IEnumerable<int>) usedUnitIds).Contains<int>(id));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    for (i = deckInfo.player_units.Length; i < this.DeckUnitData.Length; ++i)
    {
      e = this.LoadUnitPrefab(i, (PlayerUnit) null, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    bool bCompleted;
    if (this.isCustomDeck)
      OverkillersUtil.checkCompletedCustomDeck(this.DeckUnitData, out bCompleted);
    else
      OverkillersUtil.checkCompletedDeck(this.DeckUnitData, out bCompleted);
    this.isCompletedOverkillersDeck = bCompleted;
  }

  public IEnumerator LoadUnitPrefab(
    int index,
    PlayerUnit unit,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    bool sortieNot = false)
  {
    GameObject gameObject = this.normalPrefab.Clone(this.linkCharacters[index].transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    IEnumerator e = unitScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.setLevelText(unit);
    if (unit != (PlayerUnit) null && Singleton<NGGameDataManager>.GetInstance().IsSea && !unit.is_guest && !sortieNot)
      unitScript.SetSeaPiece(unit.unit.GetPiece);
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (this.isLimitation || this.genderRestriction != UnitGender.none)
      this.SetRegulation(unit, unitScript, index);
    if (unit != (PlayerUnit) null)
    {
      if (sortieNot)
      {
        unitScript.SetEmpty();
        unitScript.SetRegulation(UnitIcon.Regulation.None);
        unitScript.BackgroundModeValue = UnitIcon.BackgroundMode.Empty;
      }
      else
      {
        if (!this.disableUnitDetail)
        {
          unitScript.onClick = (Action<UnitIconBase>) (x => this.ChangeDetailScene(unit, index));
          EventDelegate.Set(unitScript.Button.onLongPress, (EventDelegate.Callback) (() => this.ChangeDetailScene(unit, index)));
        }
        unitScript.BreakWeapon = unit.IsBrokenEquippedGear;
        string color_code = unit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
        unitScript.SpecialIcon = !string.IsNullOrEmpty(color_code);
        if (!string.IsNullOrEmpty(color_code))
        {
          int? specialIconType = UnitIcon.GetSpecialIconType(color_code);
          if (specialIconType.HasValue)
            unitScript.SpecialIconType = specialIconType.Value;
        }
        if (unit.is_guest)
          this.slcTextGuests[index].SetActive(true);
      }
    }
    else
    {
      unitScript.SetEmpty();
      if (sortieNot)
      {
        unitScript.SetRegulation(UnitIcon.Regulation.None);
        unitScript.BackgroundModeValue = UnitIcon.BackgroundMode.Empty;
      }
    }
    if (unit != (PlayerUnit) null && !sortieNot)
    {
      e = this.doInitUnitGears(index, unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unitScript.Favorite = false;
    unitScript.Gray = false;
  }

  private IEnumerator doInitUnitGears(int index, PlayerUnit unit, bool bUsed = false)
  {
    int slotNum = 1;
    if (unit.isOpenedEquippedGear3)
    {
      slotNum = unit.unit.awake_unit_flag ? 3 : 2;
      if (slotNum >= 3)
        this.changeGearList.Add(this.linkGears[index].GearBaseObj);
    }
    else
      slotNum = unit.unit.awake_unit_flag ? 2 : 1;
    List<int> equippedSlotList = new List<int>();
    List<PlayerItem> equippedGears = new List<PlayerItem>(slotNum);
    equippedGears.Add(unit.equippedGear);
    equippedSlotList.Add(1);
    List<PlayerItem> equippedReisous = new List<PlayerItem>(slotNum);
    equippedReisous.Add(unit.equippedReisou);
    if (slotNum > 1)
    {
      if (unit.isOpenedEquippedGear3 && !unit.unit.awake_unit_flag)
      {
        equippedGears.Add(unit.equippedGear3);
        equippedSlotList.Add(3);
        equippedReisous.Add(unit.equippedReisou3);
      }
      else
      {
        equippedGears.Add(unit.equippedGear2);
        equippedSlotList.Add(2);
        equippedGears.Add(unit.equippedGear3);
        equippedSlotList.Add(3);
        equippedReisous.Add(unit.equippedReisou2);
        equippedReisous.Add(unit.equippedReisou3);
      }
    }
    for (int i = 0; i < slotNum; ++i)
    {
      ItemIcon gearIcon = this.gearIconPrefab.CloneAndGetComponent<ItemIcon>(this.linkGears[index].List[i].transform);
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        ((Component) gearIcon).transform.localScale = new Vector3(0.65f, 0.65f);
      else
        ((Component) gearIcon).transform.localScale = new Vector3(1f, 1f);
      int changeGearIndex = equippedSlotList[i];
      if (equippedGears[i] != (PlayerItem) null)
      {
        yield return (object) gearIcon.InitByItemInfo(new GameCore.ItemInfo(equippedGears[i], equippedReisous[i], true));
        gearIcon.resetExpireDate();
        bool broken = equippedGears[i].broken;
        gearIcon.Broken = broken;
        this.isBrokenGear |= broken;
        EventDelegate.Set(gearIcon.gear.button.onLongPress, (EventDelegate.Callback) (() => this.ChangeWpDetailsScene(unit, gearIcon.ItemInfo)));
      }
      else
      {
        yield return (object) gearIcon.InitForEquipGear();
        if (this.isCustomDeck | bUsed)
        {
          gearIcon.setEquipPlus(false);
          gearIcon.isButtonActive = false;
        }
        else if (!bUsed)
        {
          gearIcon.setEquipPlus(true);
          EventDelegate.Set(gearIcon.gear.button.onLongPress, (EventDelegate.Callback) (() => this.ChangeWpEquipScene(unit, changeGearIndex)));
        }
      }
      gearIcon.onClick = (Action<ItemIcon>) (_ => this.ChangeWpEquipScene(unit, changeGearIndex));
      gearIcon.Gray = bUsed;
    }
    this.linkGears[index].GearBaseObj[1].SetActive(false);
  }

  public IEnumerator LoadUnitPrefab(int index, PlayerUnit unit, bool bUsed)
  {
    GameObject gameObject = this.normalPrefab.Clone(this.linkCharacters[index].transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    IEnumerator e = unitScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.setLevelText(unit);
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (unit != (PlayerUnit) null)
    {
      if (!this.disableUnitDetail)
      {
        unitScript.onClick = (Action<UnitIconBase>) (x => this.ChangeDetailScene(unit, index));
        EventDelegate.Set(unitScript.Button.onLongPress, (EventDelegate.Callback) (() => this.ChangeDetailScene(unit, index)));
      }
      unitScript.BreakWeapon = unit.IsBrokenEquippedGear;
      unitScript.Favorite = false;
      this.isAnyUsedUnit |= bUsed;
      unitScript.Gray = bUsed;
      unitScript.UnitUsed = bUsed;
      unitScript.SetupDeckStatusBlink();
    }
    else
      unitScript.SetEmpty();
    if (unit != (PlayerUnit) null)
    {
      e = this.doInitUnitGears(index, unit, bUsed);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void ChangeDetailScene(PlayerUnit unit, int index)
  {
    if (!unit.is_guest)
    {
      PlayerUnit[] array = ((IEnumerable<PlayerUnit>) this.DeckUnitData).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && !x.is_guest)).ToArray<PlayerUnit>();
      if (this.isCustomDeck)
        Unit0042Scene.changeSceneCustomDeck(true, unit, array);
      else
        Unit0042Scene.changeScene(true, unit, array);
    }
    else
      Unit0042Scene.changeSceneGuestUnit(true, unit, ((IEnumerable<PlayerUnit>) this.DeckUnitData).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.is_guest)).ToArray<PlayerUnit>());
    this.DestroyObject();
  }

  public void ChangeWpEquipScene(PlayerUnit unit, int changeGearIndex)
  {
    if (this.isCustomDeck || unit.is_guest)
      return;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Unit0044Scene.ChangeScene(true, unit, changeGearIndex);
    this.DestroyObject();
  }

  public void ChangeWpDetailsScene(PlayerUnit unit, GameCore.ItemInfo choiceGear)
  {
    if (unit.is_guest || choiceGear == null)
      return;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    if (this.isCustomDeck)
    {
      PlayerItem gear = unit.equippedGear;
      PlayerItem reisou = unit.equippedReisou;
      if (gear == (PlayerItem) null || gear.id != choiceGear.itemID)
      {
        gear = unit.equippedGear2;
        reisou = unit.equippedReisou2;
        if (gear == (PlayerItem) null || gear.id != choiceGear.itemID)
        {
          gear = unit.equippedGear3;
          reisou = unit.equippedReisou3;
        }
      }
      Unit00443Scene.changeSceneCustomDeck(gear, unit, reisou);
    }
    else
      Unit00443Scene.changeScene(true, choiceGear);
    this.DestroyObject();
  }

  public void ChangeWpSlot()
  {
    if (this.changeGearList.Count == 0)
      return;
    foreach (List<GameObject> changeGear in this.changeGearList)
    {
      foreach (GameObject gameObject in changeGear)
        gameObject.SetActive(!gameObject.activeSelf);
    }
  }

  public bool ChangeWpSlotCheck() => this.changeGearList.Count != 0;

  public void DestroyObject()
  {
    if (this.isDisabledAutoDestroy_)
      return;
    foreach (GameObject linkCharacter in this.linkCharacters)
    {
      UnitIcon componentInChildren = linkCharacter.GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
    foreach (Quest0028Indicator.GearList linkGear in this.linkGears)
    {
      foreach (GameObject gameObject in linkGear.List)
      {
        ItemIcon componentInChildren = gameObject.GetComponentInChildren<ItemIcon>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          Object.Destroy((Object) ((Component) componentInChildren).gameObject);
      }
    }
  }

  private void SetRegulation(PlayerUnit unit, UnitIcon unitScript, int count)
  {
    bool flag1 = true;
    bool flag2 = true;
    if (unit != (PlayerUnit) null && !unit.is_guest)
    {
      if (this.regulationUnitIds != null)
        flag1 = this.regulationUnitIds.Contains(unit.id);
      if (this.genderRestriction != UnitGender.none)
        flag2 = unit.unit.character.gender == this.genderRestriction;
    }
    if (!flag1 || !flag2)
    {
      if (unitScript.BreakWeapon)
        unitScript.SetRegulation(UnitIcon.Regulation.WithBroken);
      else
        unitScript.SetRegulation(UnitIcon.Regulation.Default);
    }
    else
      unitScript.SetRegulation(UnitIcon.Regulation.None);
  }

  [Serializable]
  public class GearList
  {
    public List<GameObject> List = new List<GameObject>();
    public List<GameObject> GearBaseObj = new List<GameObject>();

    public GearList(List<GameObject> list) => this.List = list;
  }
}
