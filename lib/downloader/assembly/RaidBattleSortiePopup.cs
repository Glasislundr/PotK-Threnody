// Decompiled with JetBrains decompiler
// Type: RaidBattleSortiePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class RaidBattleSortiePopup : BackButtonMenuBase
{
  private const float LINK_WIDTH = 92f;
  private const float LINK_DEFWIDTH = 114f;
  private const float scale = 0.807017565f;
  private const int FRIEND_INDEX = 5;
  [SerializeField]
  private UILabel lblLeaderSkillName;
  [SerializeField]
  private UILabel lblLeaderSkillDesc;
  [SerializeField]
  private GameObject objLeaderSkillZoom;
  [SerializeField]
  private GameObject slc_NotFriend_Skill;
  [SerializeField]
  private UILabel lblFriendSkillName;
  [SerializeField]
  private UILabel lblFriendSkillDesc;
  [SerializeField]
  private GameObject objFriendSkillZoom;
  [SerializeField]
  private UILabel lblNoFriend;
  [SerializeField]
  private UILabel lblDeckCombat;
  [SerializeField]
  private UILabel lblRecommendCombat;
  [SerializeField]
  private UIButton[] largeButton = new UIButton[0];
  [SerializeField]
  protected GameObject[] linkCharacters;
  [SerializeField]
  protected GameObject[] linkUnabaibleIcons;
  [SerializeField]
  protected GameObject[] dir_Items;
  [SerializeField]
  private Transform lnkTeamMain;
  [SerializeField]
  private SortieDeckMainPanel mainPanel;
  private List<ItemIcon> itemIconList = new List<ItemIcon>();
  private GameObject unitIconPrefab;
  private GameObject itemIconPrefab;
  private GameObject detailPopup;
  private RaidBattlePreparationPopup parent;
  private int mDeckCombat;
  private int mRecommendCombat;
  private GameObject skillDetailPrefab;
  private PlayerUnitLeader_skills leaderSkill;
  private PlayerUnitLeader_skills friendSkill;
  private bool isInitMainPanel = true;
  private SortieDeckMainPanel.DeckMode deckMode;
  private bool isCompletedOverkillersDeck = true;

  public IEnumerator InitializeAsync(
    RaidBattlePreparationPopup parent,
    DeckInfo[] decks,
    int[] gray_out_unit_ids,
    PlayerUnit friend,
    PlayerItem[] supplys,
    string recommendCombat)
  {
    RaidBattleSortiePopup battleSortiePopup = this;
    battleSortiePopup.parent = parent;
    battleSortiePopup.setButtonMode(RaidBattleSortiePopup.ButtonMode.Sortie);
    battleSortiePopup.deckMode = decks[0].isCustom ? SortieDeckMainPanel.DeckMode.Custom : SortieDeckMainPanel.DeckMode.Normal;
    Future<GameObject> ld;
    IEnumerator e;
    if (Object.op_Implicit((Object) battleSortiePopup.lnkTeamMain) && !Object.op_Implicit((Object) battleSortiePopup.mainPanel))
    {
      ld = new ResourceObject("Prefabs/quest002_8/dir_Team_Main").Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = ld.Result.Clone(battleSortiePopup.lnkTeamMain);
      battleSortiePopup.mainPanel = gameObject.GetComponent<SortieDeckMainPanel>();
      ld = (Future<GameObject>) null;
    }
    if (Object.op_Implicit((Object) battleSortiePopup.mainPanel) && battleSortiePopup.isInitMainPanel)
    {
      e = battleSortiePopup.mainPanel.doLoadResources();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleSortiePopup.resetModeSwitch(Util.checkUnlockedPlayerLevel(Player.Current.level) && ((IEnumerable<PlayerCustomDeck>) SMManager.Get<PlayerCustomDeck[]>()).Any<PlayerCustomDeck>((Func<PlayerCustomDeck, bool>) (x => ((IEnumerable<int>) x.player_unit_ids).Any<int>((Func<int, bool>) (i => i != 0)))));
      battleSortiePopup.mainPanel.eventDeckChanged = new Action<DeckInfo>(battleSortiePopup.updateDeckInfo);
      battleSortiePopup.mainPanel.eventClickedFriendUnit = new Action<PlayerUnit, string, int>(battleSortiePopup.onClickedFriendUnit);
      battleSortiePopup.mainPanel.eventClickedEditItem = new Action(battleSortiePopup.OnSupplyEquipButton);
      battleSortiePopup.mainPanel.eventClickedSupply = new Action<int>(battleSortiePopup.onClickedSupply);
      battleSortiePopup.mainPanel.eventClickedBattleSetting = new Action(battleSortiePopup.OnBattleConfigButton);
      battleSortiePopup.mainPanel.eventClickedEditDeck = new Action(battleSortiePopup.OnDeckEditButton);
      battleSortiePopup.isInitMainPanel = false;
    }
    if (Object.op_Equality((Object) battleSortiePopup.unitIconPrefab, (Object) null))
    {
      ld = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleSortiePopup.unitIconPrefab = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) battleSortiePopup.itemIconPrefab, (Object) null))
    {
      ld = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleSortiePopup.itemIconPrefab = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) battleSortiePopup.skillDetailPrefab, (Object) null))
    {
      ld = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) ld.Wait();
      battleSortiePopup.skillDetailPrefab = ld.Result;
      ld = (Future<GameObject>) null;
    }
    battleSortiePopup.setRecommendCombat(recommendCombat);
    if (Object.op_Implicit((Object) battleSortiePopup.mainPanel))
    {
      e = battleSortiePopup.doInitDecks(decks, gray_out_unit_ids, friend, supplys);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      yield return (object) battleSortiePopup.SetDeck(((IEnumerable<DeckInfo>) decks).First<DeckInfo>().player_units, gray_out_unit_ids);
      yield return (object) battleSortiePopup.SetFriendUnit(friend);
      yield return (object) battleSortiePopup.SetSupplys(supplys);
      if (!battleSortiePopup.isCompletedOverkillersDeck)
        battleSortiePopup.setErrorOverkillersDeck();
    }
  }

  public void resetModeSwitch(bool bExistCustom)
  {
    this.mainPanel.initModeSwitch(new bool[2]
    {
      true,
      bExistCustom
    }, new Action<SortieDeckMainPanel.DeckMode>(this.onChangedDeckMode));
  }

  public IEnumerator ReloadAsync(
    DeckInfo[] decks,
    int[] gray_out_unit_ids,
    PlayerUnit friend,
    PlayerItem[] supplys)
  {
    if (Object.op_Implicit((Object) this.mainPanel))
    {
      IEnumerator e = this.doInitDecks(decks, gray_out_unit_ids, friend, supplys);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.DestroyAllUnitIcons();
      this.DestroyAllSupplyIcons();
      yield return (object) this.SetDeck(((IEnumerable<DeckInfo>) decks).First<DeckInfo>().player_units, gray_out_unit_ids);
      yield return (object) this.SetFriendUnit(friend);
      yield return (object) this.SetSupplys(supplys);
      if (!this.isCompletedOverkillersDeck)
        this.setErrorOverkillersDeck();
    }
  }

  private void onChangedDeckMode(SortieDeckMainPanel.DeckMode mode)
  {
    if (this.deckMode == mode)
      return;
    this.deckMode = mode;
    this.parent.OnChangeDeckMode(mode == SortieDeckMainPanel.DeckMode.Custom);
  }

  private IEnumerator doInitDecks(
    DeckInfo[] decks,
    int[] gray_out_unit_ids,
    PlayerUnit friend,
    PlayerItem[] supplys)
  {
    IEnumerator e = this.mainPanel.doInitDecks(decks, gray_out_unit_ids);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mainPanel.doInitFriend(friend);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mainPanel.doInitSupplies(supplys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetDeck(PlayerUnit[] deck, int[] usedUnitIds)
  {
    RaidBattleSortiePopup battleSortiePopup = this;
    battleSortiePopup.leaderSkill = (PlayerUnitLeader_skills) null;
    foreach (GameObject linkUnabaibleIcon in battleSortiePopup.linkUnabaibleIcons)
      linkUnabaibleIcon.SetActive(true);
    OverkillersUtil.checkCompletedDeck(deck, out battleSortiePopup.isCompletedOverkillersDeck);
    bool isGray = false;
    bool isGearBroken = false;
    for (int i = 0; i < deck.Length; ++i)
    {
      isGray |= ((IEnumerable<int>) usedUnitIds).Contains<int>(deck[i].id);
      isGearBroken |= deck[i].IsBrokenEquippedGear;
      yield return (object) battleSortiePopup.LoadUnitPrefab(i, deck[i], false, isGray);
      if (i == 0)
        battleSortiePopup.leaderSkill = deck[i].leader_skill;
      battleSortiePopup.linkUnabaibleIcons[i].SetActive(false);
    }
    if (battleSortiePopup.leaderSkill != null)
    {
      BattleskillSkill skill = battleSortiePopup.leaderSkill.skill;
      battleSortiePopup.lblLeaderSkillName.SetTextLocalize(skill.name);
      battleSortiePopup.lblLeaderSkillDesc.SetTextLocalize(skill.description);
      battleSortiePopup.objLeaderSkillZoom.SetActive(true);
    }
    else
    {
      battleSortiePopup.lblLeaderSkillName.SetText("---");
      battleSortiePopup.lblLeaderSkillDesc.SetText("-----");
      battleSortiePopup.objLeaderSkillZoom.SetActive(false);
    }
    if (isGearBroken)
      battleSortiePopup.setButtonMode(RaidBattleSortiePopup.ButtonMode.Repair);
    else if (isGray || deck.Length < 1 || !battleSortiePopup.isCompletedOverkillersDeck)
      battleSortiePopup.setButtonMode(RaidBattleSortiePopup.ButtonMode.Edit);
    else
      battleSortiePopup.setButtonMode(RaidBattleSortiePopup.ButtonMode.Sortie);
    battleSortiePopup.mDeckCombat = 0;
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<PlayerUnit>) deck).ForEach<PlayerUnit>(new Action<PlayerUnit>(battleSortiePopup.\u003CSetDeck\u003Eb__39_0));
    if (battleSortiePopup.mRecommendCombat == 0)
      battleSortiePopup.lblDeckCombat.SetTextLocalize(battleSortiePopup.mDeckCombat);
    else if (battleSortiePopup.mRecommendCombat <= battleSortiePopup.mDeckCombat)
      battleSortiePopup.lblDeckCombat.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_0028_DECK_ENOUGH, (IDictionary) new Hashtable()
      {
        {
          (object) "combat",
          (object) battleSortiePopup.mDeckCombat.ToString()
        }
      }));
    else
      battleSortiePopup.lblDeckCombat.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_0028_DECK_SHORT, (IDictionary) new Hashtable()
      {
        {
          (object) "combat",
          (object) battleSortiePopup.mDeckCombat.ToString()
        }
      }));
  }

  private void updateDeckInfo(DeckInfo deckInfo)
  {
    this.parent.OnChangeDeck(deckInfo);
    Quest0028Indicator currentIndicator = this.mainPanel.currentIndicator;
    if (currentIndicator.isBrokenGear)
      this.setButtonMode(RaidBattleSortiePopup.ButtonMode.Repair);
    else if (currentIndicator.isAnyUsedUnit || !currentIndicator.isCompletedOverkillersDeck || !((IEnumerable<PlayerUnit>) deckInfo.player_units).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)))
      this.setButtonMode(RaidBattleSortiePopup.ButtonMode.Edit);
    else
      this.setButtonMode(RaidBattleSortiePopup.ButtonMode.Sortie);
    Consts instance = Consts.GetInstance();
    int totalCombat = deckInfo.total_combat;
    if (this.mRecommendCombat == 0)
      this.mainPanel.txtTotalCombat.SetTextLocalize(totalCombat);
    else
      this.mainPanel.txtTotalCombat.SetTextLocalize(Consts.Format(this.mRecommendCombat <= totalCombat ? instance.QUEST_0028_DECK_ENOUGH : instance.QUEST_0028_DECK_SHORT, (IDictionary) new Hashtable()
      {
        {
          (object) "combat",
          (object) totalCombat
        }
      }));
    this.mainPanel.txtDeckName.SetTextLocalize(deckInfo.name);
    int cost = deckInfo.cost;
    int maxCost = Player.Current.max_cost;
    this.mainPanel.txtTotalCost.SetTextLocalize(Consts.Format(maxCost >= cost ? instance.QUEST_0028_COST_SAFE : instance.QUEST_0028_COST_OVER, (IDictionary) new Hashtable()
    {
      {
        (object) "total",
        (object) cost
      },
      {
        (object) "max",
        (object) maxCost
      }
    }));
  }

  public IEnumerator SetFriendUnit(PlayerUnit friend)
  {
    if (Object.op_Implicit((Object) this.mainPanel))
    {
      IEnumerator e = this.mainPanel.doInitFriend(friend);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.DestroyFriendUnitIcon();
      this.friendSkill = (PlayerUnitLeader_skills) null;
      if (friend == (PlayerUnit) null)
      {
        this.slc_NotFriend_Skill.SetActive(true);
        this.lblNoFriend.SetTextLocalize(Consts.GetInstance().QUEST_0028_INDICATOR_NOT_RENTAL);
        yield return (object) this.LoadUnitPrefab(5, (PlayerUnit) null, true, false);
      }
      else
      {
        this.slc_NotFriend_Skill.SetActive(false);
        PlayerUnit unit = friend;
        if (unit.leader_skills != null)
          this.friendSkill = unit.leader_skill;
        yield return (object) this.LoadUnitPrefab(5, unit, true, false);
      }
      if (this.friendSkill != null)
      {
        BattleskillSkill skill = this.friendSkill.skill;
        this.lblFriendSkillDesc.SetText(skill.description);
        this.lblFriendSkillName.SetText(skill.name);
        this.objFriendSkillZoom.SetActive(true);
      }
      else
      {
        this.lblFriendSkillName.SetText("---");
        this.lblFriendSkillDesc.SetText("-----");
        this.objFriendSkillZoom.SetActive(false);
      }
    }
  }

  private void onClickedFriendUnit(PlayerUnit unit, string player_id, int unit_id)
  {
    this.parent.OnClickUnitIcon(unit, true);
  }

  private IEnumerator SetSupplys(PlayerItem[] supplys)
  {
    this.itemIconList = new List<ItemIcon>();
    for (int i = 0; i < supplys.Length; ++i)
    {
      GameObject gameObject = this.itemIconPrefab.Clone(this.dir_Items[i].transform);
      gameObject.transform.localScale = new Vector3(0.807017565f, 0.807017565f);
      ItemIcon itemIconScript = gameObject.GetComponent<ItemIcon>();
      this.itemIconList.Add(itemIconScript);
      int temp = i;
      IEnumerator e = itemIconScript.InitByPlayerItem(supplys[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      itemIconScript.onClick = (Action<ItemIcon>) (supplyicon => this.StartCoroutine(this.setDetailPopup(supplys[temp].supply.ID)));
      itemIconScript = (ItemIcon) null;
    }
    for (int length = supplys.Length; length < Consts.GetInstance().DECK_SUPPLY_MAX; ++length)
    {
      GameObject gameObject = this.itemIconPrefab.Clone(this.dir_Items[length].transform);
      gameObject.transform.localScale = new Vector3(0.807017565f, 0.807017565f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      component.SetModeSupply();
      component.SetEmpty(true);
    }
  }

  private void onClickedSupply(int itemId) => this.StartCoroutine(this.setDetailPopup(itemId));

  private IEnumerator setDetailPopup(int itemid)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    e = popup.GetComponent<Shop00742Menu>().Init(MasterDataTable.CommonRewardType.supply, itemid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  private void setErrorOverkillersDeck()
  {
    this.slc_NotFriend_Skill.gameObject.SetActive(true);
    this.lblNoFriend.SetTextLocalize(Consts.GetInstance().QUEST_0028_INDICATOR_LIMITED_OVERKILLERS);
  }

  private void setRecommendCombat(string combat)
  {
    int.TryParse(combat, out this.mRecommendCombat);
    this.lblRecommendCombat.SetTextRecommendCombat(combat);
  }

  private IEnumerator LoadUnitPrefab(int index, PlayerUnit unit, bool isFriend, bool isGray)
  {
    GameObject gameObject = this.unitIconPrefab.Clone(this.linkCharacters[index].transform);
    gameObject.transform.localScale = new Vector3(0.807017565f, 0.807017565f);
    UnitIcon iconScript = gameObject.GetComponent<UnitIcon>();
    IEnumerator e = iconScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    iconScript.setLevelText(unit);
    iconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (unit != (PlayerUnit) null)
    {
      iconScript.onLongPress = (Action<UnitIconBase>) (x => this.parent.OnLongPressUnitIcon(unit, isFriend));
      iconScript.BreakWeapon = !isFriend && unit.IsBrokenEquippedGear;
      iconScript.SpecialIcon = false;
    }
    else
      iconScript.SetEmpty();
    iconScript.onClick = (Action<UnitIconBase>) (x => this.parent.OnClickUnitIcon(unit, isFriend));
    iconScript.Favorite = false;
    iconScript.Gray = isGray;
    iconScript.UnitUsed = isGray;
    iconScript.SetupDeckStatusBlink();
  }

  private void DestroyAllUnitIcons()
  {
    if (this.linkCharacters == null)
      return;
    foreach (GameObject linkCharacter in this.linkCharacters)
    {
      UnitIcon componentInChildren = linkCharacter.GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
  }

  private void DestroyFriendUnitIcon()
  {
    if (this.linkCharacters == null)
      return;
    UnitIcon componentInChildren = this.linkCharacters[5].GetComponentInChildren<UnitIcon>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    Object.Destroy((Object) ((Component) componentInChildren).gameObject);
  }

  private void DestroyAllSupplyIcons()
  {
    foreach (GameObject dirItem in this.dir_Items)
    {
      foreach (Component child in dirItem.transform.GetChildren())
        Object.Destroy((Object) child.gameObject);
    }
  }

  private void setButtonMode(RaidBattleSortiePopup.ButtonMode mode)
  {
    for (int index = 0; index < this.largeButton.Length; ++index)
    {
      if ((RaidBattleSortiePopup.ButtonMode) index == mode)
        ((Component) this.largeButton[index]).gameObject.SetActive(true);
      else
        ((Component) this.largeButton[index]).gameObject.SetActive(false);
    }
  }

  public List<ItemIcon> GetItemIconList() => this.itemIconList;

  public void OnAutoDeckEditButton() => this.parent.OnAutoDeckEditButton();

  public void OnDeckEditButton() => this.parent.OnDeckEditButton();

  public void OnGearEquipButton() => this.parent.OnGearEquipButton();

  public void OnGearRepairButton() => this.parent.OnGearRepairButton();

  public void OnSupplyEquipButton() => this.parent.OnSupplyEquipButton();

  public void OnBattleConfigButton() => this.parent.OnBattleConfigButton();

  public void onSortieButton() => this.parent.OnSortieButton();

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.parent.OnBackButton();
  }

  public void onClickedLeaderSkillZoom()
  {
    if (this.leaderSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.leaderSkill), onClosed: new Action(this.onClosedSkillZoom));
  }

  public void onClickedFriendSkillZoom()
  {
    if (this.friendSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.friendSkill), onClosed: new Action(this.onClosedSkillZoom));
  }

  private void onClosedSkillZoom() => this.IsPush = false;

  public enum ButtonMode
  {
    Sortie,
    Edit,
    Repair,
  }
}
