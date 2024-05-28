// Decompiled with JetBrains decompiler
// Type: SortieDeckMainPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/quest002_8/SortieDeckMainPanel")]
public class SortieDeckMainPanel : MonoBehaviour
{
  [Header("ユニット編成")]
  [SerializeField]
  private UILabel txtDeckName_;
  [SerializeField]
  private GameObject objEditDeckName_;
  [SerializeField]
  private UILabel txtTotalCombat_;
  [SerializeField]
  private UILabel txtTotalCost_;
  [SerializeField]
  private NGHorizontalScrollParts scroll_;
  [Header("フレンド")]
  [SerializeField]
  private Transform lnkFriendIcon_;
  [SerializeField]
  private Transform[] lnkFriendEquipps_;
  [SerializeField]
  private UIButton btnSelectFriend_;
  [Header("支給品")]
  [SerializeField]
  private Transform[] lnkSupplies_;
  [SerializeField]
  private UIButton btnEditSupplies_;
  [Header("その他")]
  [SerializeField]
  [Tooltip("[0:通常/1:カスタム]でボタン配置")]
  private GameObject[] objDeckModeButtons_;
  [SerializeField]
  private UIButton btnLeaderSkill_;
  [SerializeField]
  private UIButton btnBattleSetting_;
  [SerializeField]
  private UIButton btnEditDecks_;
  [SerializeField]
  private UIButton btnAutoDeck_;
  [SerializeField]
  private UIButton btnBuguChange;
  private Dictionary<SortieDeckMainPanel.DeckMode, int> dicStackNumber_ = new Dictionary<SortieDeckMainPanel.DeckMode, int>(Enum.GetValues(typeof (SortieDeckMainPanel.DeckMode)).Length);
  private Dictionary<SortieDeckMainPanel.DeckMode, GameObject> dicPrefabIndicator_ = new Dictionary<SortieDeckMainPanel.DeckMode, GameObject>(Enum.GetValues(typeof (SortieDeckMainPanel.DeckMode)).Length);
  private GameObject prefabIndicator_;
  private Action<SortieDeckMainPanel.DeckMode> eventModeChanged_;
  private bool friendWpChange;
  private Quest0028Indicator[] indicators_;
  private Dictionary<string, Dictionary<int, SortieDeckMainPanel.FriendInfo>> dicFriendInfos_ = new Dictionary<string, Dictionary<int, SortieDeckMainPanel.FriendInfo>>();
  private List<GameObject> friendObjects_ = new List<GameObject>();
  private List<GameObject> supplyObjects_ = new List<GameObject>();

  public SortieDeckMainPanel.DeckMode deckMode { get; private set; }

  public bool isInitializing { get; private set; } = true;

  public GameObject prefabIndicator => this.prefabIndicator_;

  public GameObject prefabUnitIcon { get; private set; }

  public GameObject prefabItemIcon { get; private set; }

  public GameObject prefabEditDeckName { get; private set; }

  public Action<DeckInfo> eventDeckChanged { get; set; }

  public UILabel txtDeckName => this.txtDeckName_;

  public UILabel txtTotalCombat => this.txtTotalCombat_;

  public UILabel txtTotalCost => this.txtTotalCost_;

  public int currentIndex { get; private set; } = -1;

  public Action<PlayerUnit, string, int> eventClickedFriendUnit { get; set; }

  public Action eventClickedEditItem { get; set; }

  public Action<int> eventClickedSupply { get; set; }

  public Action eventClickedBattleSetting { get; set; }

  public Action eventClickedEditDeck { get; set; }

  public DeckInfo[] decks { get; private set; }

  public DeckInfo currentDeck { get; private set; }

  public Quest0028Indicator currentIndicator
  {
    get
    {
      return this.decks == null || this.decks.Length == 0 || this.currentIndex < 0 ? (Quest0028Indicator) null : this.indicators_[this.currentIndex];
    }
  }

  public PlayerUnit leaderUnit { get; private set; }

  public PlayerUnit friendUnit { get; private set; }

  public IEnumerator doLoadResources()
  {
    Future<GameObject> ld;
    IEnumerator e;
    if (!Object.op_Implicit((Object) this.prefabUnitIcon))
    {
      ld = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabUnitIcon = ld.Result;
    }
    if (!Object.op_Implicit((Object) this.prefabItemIcon))
    {
      ld = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabItemIcon = ld.Result;
    }
    if (!Object.op_Implicit((Object) this.prefabEditDeckName))
    {
      ld = PopupEditDeckName.createPrefabLoader();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabEditDeckName = ld.Result;
    }
  }

  private IEnumerator doLoadPrefabIndicator()
  {
    if (!this.dicPrefabIndicator_.TryGetValue(this.deckMode, out this.prefabIndicator_))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/quest002_8/" + (this.deckMode == SortieDeckMainPanel.DeckMode.Custom ? "dir_party_2" : "dir_party")).Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabIndicator_ = prefabF.Result;
      this.dicPrefabIndicator_[this.deckMode] = this.prefabIndicator_;
    }
  }

  public IEnumerator doInitDecks(DeckInfo[] deckInfos, int[] usedUnitIds)
  {
    SortieDeckMainPanel sortieDeckMainPanel = this;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    SortieDeckMainPanel.\u003C\u003Ec__DisplayClass99_0 cDisplayClass990 = new SortieDeckMainPanel.\u003C\u003Ec__DisplayClass99_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass990.\u003C\u003E4__this = this;
    sortieDeckMainPanel.isInitializing = true;
    ((Behaviour) sortieDeckMainPanel.scroll_).enabled = false;
    sortieDeckMainPanel.scroll_.SeEnable = false;
    sortieDeckMainPanel.scroll_.destroyParts(false);
    sortieDeckMainPanel.decks = ((IEnumerable<DeckInfo>) deckInfos).Where<DeckInfo>((Func<DeckInfo, bool>) (x => ((IEnumerable<int>) x.player_unit_ids).FirstOrDefault<int>() != 0)).ToArray<DeckInfo>();
    if (sortieDeckMainPanel.decks.Length == 0)
      sortieDeckMainPanel.decks = new DeckInfo[1]
      {
        ((IEnumerable<DeckInfo>) deckInfos).First<DeckInfo>()
      };
    sortieDeckMainPanel.indicators_ = new Quest0028Indicator[sortieDeckMainPanel.decks.Length];
    sortieDeckMainPanel.deckMode = ((IEnumerable<DeckInfo>) sortieDeckMainPanel.decks).First<DeckInfo>().isCustom ? SortieDeckMainPanel.DeckMode.Custom : SortieDeckMainPanel.DeckMode.Normal;
    if (Object.op_Implicit((Object) sortieDeckMainPanel.btnAutoDeck_))
      ((UIButtonColor) sortieDeckMainPanel.btnAutoDeck_).isEnabled = sortieDeckMainPanel.deckMode == SortieDeckMainPanel.DeckMode.Normal;
    IEnumerator e = sortieDeckMainPanel.doLoadPrefabIndicator();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    for (int n = 0; n < sortieDeckMainPanel.decks.Length; ++n)
    {
      e = sortieDeckMainPanel.doInitDeck(n, usedUnitIds);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    // ISSUE: reference to a compiler-generated field
    cDisplayClass990.grid = sortieDeckMainPanel.scroll_.scrollView.GetComponentInChildren<UIGrid>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    cDisplayClass990.grid.onReposition = new UIGrid.OnReposition((object) cDisplayClass990, __methodptr(\u003CdoInitDecks\u003Eb__1));
    // ISSUE: reference to a compiler-generated field
    cDisplayClass990.grid.repositionNow = true;
    yield return (object) null;
    ((IEnumerable<GameObject>) sortieDeckMainPanel.objDeckModeButtons_).ToggleOnce((int) sortieDeckMainPanel.deckMode);
    sortieDeckMainPanel.objEditDeckName_.SetActive(sortieDeckMainPanel.deckMode == SortieDeckMainPanel.DeckMode.Custom);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass990.firstIndex = sortieDeckMainPanel.firstDeckNumber;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    cDisplayClass990.firstIndex = ((IEnumerable<DeckInfo>) sortieDeckMainPanel.decks).FirstIndexOrNull<DeckInfo>(new Func<DeckInfo, bool>(cDisplayClass990.\u003CdoInitDecks\u003Eb__2)) ?? 0;
    GameObject[] arrows = new GameObject[2]
    {
      sortieDeckMainPanel.scroll_.rightArrow,
      sortieDeckMainPanel.scroll_.leftArrow
    };
    ((IEnumerable<GameObject>) arrows).SetActives(false);
    ((Behaviour) sortieDeckMainPanel.scroll_).enabled = true;
    sortieDeckMainPanel.currentIndex = -1;
    sortieDeckMainPanel.leaderUnit = (PlayerUnit) null;
    // ISSUE: reference to a compiler-generated field
    sortieDeckMainPanel.updateDeck(cDisplayClass990.firstIndex);
    // ISSUE: reference to a compiler-generated field
    sortieDeckMainPanel.scroll_.resetCenterItem(cDisplayClass990.firstIndex);
    yield return (object) null;
    if (sortieDeckMainPanel.decks.Length > 1)
      sortieDeckMainPanel.scroll_.SeEnable = true;
    else
      ((IEnumerable<GameObject>) arrows).SetActives(false);
    sortieDeckMainPanel.isInitializing = false;
  }

  private int firstDeckNumber
  {
    get
    {
      Persist<Persist.GuildRaidLastSortie> guildRaidLastSortie = Persist.guildRaidLastSortie;
      int firstDeckNumber;
      if (!this.dicStackNumber_.TryGetValue(this.deckMode, out firstDeckNumber))
      {
        firstDeckNumber = this.deckMode == SortieDeckMainPanel.DeckMode.Custom ? guildRaidLastSortie.Data.customDeckNumber : guildRaidLastSortie.Data.deckNumber;
        this.dicStackNumber_[this.deckMode] = firstDeckNumber;
      }
      return firstDeckNumber;
    }
  }

  private IEnumerator doInitDeck(int index, int[] usedUnitIds)
  {
    DeckInfo deck = this.decks[index];
    GameObject gameObject = this.scroll_.instantiateParts(this.prefabIndicator, false);
    IEnumerator e = (this.indicators_[index] = gameObject.GetComponent<Quest0028Indicator>()).InitDeck(deck, usedUnitIds, this.prefabUnitIcon, this.prefabItemIcon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void updateDeck(int index)
  {
    if (this.currentIndex == index || index < 0 || this.decks.Length <= index)
      return;
    this.currentDeck = this.decks[index];
    this.currentIndex = index;
    this.dicStackNumber_[this.deckMode] = this.currentDeck.deck_number;
    this.leaderUnit = ((IEnumerable<PlayerUnit>) this.currentDeck.player_units).FirstOrDefault<PlayerUnit>();
    ((UIButtonColor) this.btnLeaderSkill_).isEnabled = this.leaderUnit?.leader_skill?.skill != null;
    this.buguButtonEnableSet();
    Action<DeckInfo> eventDeckChanged = this.eventDeckChanged;
    if (eventDeckChanged == null)
      return;
    eventDeckChanged(this.currentDeck);
  }

  private void onItemChanged(int index)
  {
    if (this.isInitializing || this.decks == null)
      return;
    this.updateDeck(index);
  }

  public void BtnWeaponChangeSlot()
  {
    this.indicators_[this.currentIndex].ChangeWpSlot();
    if (!this.friendWpChange)
      return;
    for (int index = 0; index < this.lnkFriendEquipps_.Length; ++index)
      ((Component) this.lnkFriendEquipps_[index]).gameObject.SetActive(!((Component) this.lnkFriendEquipps_[index]).gameObject.activeSelf);
  }

  private SortieDeckMainPanel.FriendInfo getFriendInfo(string player_id, int unit_id)
  {
    Dictionary<int, SortieDeckMainPanel.FriendInfo> dictionary;
    if (!this.dicFriendInfos_.TryGetValue(player_id, out dictionary))
      return (SortieDeckMainPanel.FriendInfo) null;
    SortieDeckMainPanel.FriendInfo friendInfo;
    dictionary.TryGetValue(unit_id, out friendInfo);
    return friendInfo;
  }

  private void setFriendInfo(
    string player_id,
    int unit_id,
    SortieDeckMainPanel.FriendInfo friendInfo)
  {
    if (friendInfo == null)
      return;
    Dictionary<int, SortieDeckMainPanel.FriendInfo> dictionary;
    if (!this.dicFriendInfos_.TryGetValue(player_id, out dictionary))
    {
      dictionary = new Dictionary<int, SortieDeckMainPanel.FriendInfo>(1);
      this.dicFriendInfos_[player_id] = dictionary;
    }
    dictionary[unit_id] = friendInfo;
  }

  public IEnumerator doInitFriend(
    PlayerUnit friend,
    Action<WebAPI.Response.UserError> errorCallback = null)
  {
    PlayerUnit unit = (PlayerUnit) null;
    IEnumerator e;
    if (friend != (PlayerUnit) null)
    {
      SortieDeckMainPanel.FriendInfo friendInfo = this.getFriendInfo(friend.player_id, friend.id);
      if (friendInfo == null)
      {
        Future<WebAPI.Response.FriendStatus> webApi = WebAPI.FriendStatus(friend.player_id, friend.id, errorCallback);
        e = webApi.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (webApi.Result == null)
        {
          yield break;
        }
        else
        {
          friendInfo = new SortieDeckMainPanel.FriendInfo(webApi.Result);
          this.setFriendInfo(friend.player_id, friend.id, friendInfo);
          webApi = (Future<WebAPI.Response.FriendStatus>) null;
        }
      }
      unit = friendInfo.playerUnit;
    }
    e = unit != (PlayerUnit) null ? this.doInitFriend(unit, friend.player_id, friend.id) : this.doInitFriend((PlayerUnit) null, string.Empty, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator doInitFriend(PlayerUnit unit, string player_id, int unit_id)
  {
    this.friendWpChange = false;
    this.destroyObjects(this.friendObjects_);
    GameObject gameObject1 = this.prefabUnitIcon.Clone(this.lnkFriendIcon_);
    this.friendObjects_.Add(gameObject1);
    UnitIcon uIcon = gameObject1.GetComponent<UnitIcon>();
    this.friendUnit = unit;
    IEnumerator e;
    if (unit != (PlayerUnit) null)
    {
      e = uIcon.setSimpleUnit(unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      uIcon.setLevelText(unit);
      uIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    else
    {
      uIcon.SetEmpty();
      uIcon.SelectUnit = true;
    }
    if (unit != (PlayerUnit) null)
    {
      int slotNum = unit.unit.awake_unit_flag ? 2 : 1;
      if (unit.equip_gear_ids.Length >= 3 || unit.equip_gear_ids.Length > slotNum)
        slotNum = unit.equip_gear_ids.Length;
      List<PlayerItem> equippedGears = new List<PlayerItem>();
      equippedGears.Add(unit.equippedGear);
      List<PlayerItem> equippedReisous = new List<PlayerItem>();
      equippedReisous.Add(unit.equippedReisou);
      switch (slotNum)
      {
        case 2:
          if (unit.unit.awake_unit_flag)
          {
            equippedGears.Add(unit.equippedGear2);
            equippedReisous.Add(unit.equippedReisou2);
            break;
          }
          equippedGears.Add(unit.equippedGear3);
          equippedReisous.Add(unit.equippedReisou3);
          break;
        case 3:
          equippedGears.Add(unit.equippedGear2);
          equippedReisous.Add(unit.equippedReisou2);
          equippedGears.Add(unit.equippedGear3);
          equippedReisous.Add(unit.equippedReisou3);
          this.friendWpChange = true;
          break;
      }
      ItemIcon[] itemIconArray = new ItemIcon[slotNum];
      for (int n = 0; n < slotNum; ++n)
      {
        GameObject gameObject2 = this.prefabItemIcon.Clone(this.lnkFriendEquipps_[n]);
        this.friendObjects_.Add(gameObject2);
        ItemIcon gIcon = gameObject2.GetComponent<ItemIcon>();
        e = equippedGears[n] != (PlayerItem) null ? gIcon.InitByItemInfo(new GameCore.ItemInfo(equippedGears[n], equippedReisous[n], true)) : gIcon.InitForEquipGear();
        while (e.MoveNext())
          yield return e.Current;
        if (equippedGears[n] != (PlayerItem) null)
          gIcon.resetExpireDate();
        gIcon = (ItemIcon) null;
        e = (IEnumerator) null;
      }
      equippedGears = (List<PlayerItem>) null;
      equippedReisous = (List<PlayerItem>) null;
    }
    ((Component) this.lnkFriendEquipps_[0]).gameObject.SetActive(true);
    ((Component) this.lnkFriendEquipps_[1]).gameObject.SetActive(true);
    ((Component) this.lnkFriendEquipps_[2]).gameObject.SetActive(false);
    if (unit != (PlayerUnit) null && string.IsNullOrEmpty(player_id) && unit_id == 0)
    {
      player_id = unit.player_id;
      unit_id = unit.id;
    }
    uIcon.onClick = (Action<UnitIconBase>) (_ => this.onClickedFriendUnit(unit, player_id, unit_id));
    if (unit != (PlayerUnit) null)
      uIcon.onLongPress = (Action<UnitIconBase>) (_ => this.onLongPressedFriendUnit(unit, player_id, unit_id));
    else
      uIcon.onLongPress = (Action<UnitIconBase>) (_ => this.onClickedFriendUnit());
    this.buguButtonEnableSet();
  }

  private void buguButtonEnableSet()
  {
    if (!Object.op_Inequality((Object) this.btnBuguChange, (Object) null))
      return;
    ((UIButtonColor) this.btnBuguChange).isEnabled = true;
    if (this.indicators_[this.currentIndex].ChangeWpSlotCheck() || this.friendWpChange)
      return;
    ((UIButtonColor) this.btnBuguChange).isEnabled = false;
  }

  private void onClickedFriendUnit(PlayerUnit unit = null, string player_id = "", int unit_id = 0)
  {
    Action<PlayerUnit, string, int> clickedFriendUnit = this.eventClickedFriendUnit;
    if (clickedFriendUnit == null)
      return;
    clickedFriendUnit(unit, player_id, unit_id);
  }

  private void onLongPressedFriendUnit(PlayerUnit unit, string player_id, int unit_id)
  {
    Unit0042Scene.changeSceneFriendUnit(true, player_id, unit_id);
  }

  public IEnumerator doInitSupplies(PlayerItem[] targets)
  {
    this.destroyObjects(this.supplyObjects_);
    for (int n = 0; n < targets.Length; ++n)
    {
      GameObject gameObject = this.prefabItemIcon.Clone(this.lnkSupplies_[n]);
      this.supplyObjects_.Add(gameObject);
      ItemIcon icon = gameObject.GetComponent<ItemIcon>();
      IEnumerator e = icon.InitByPlayerItem(targets[n]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.setClickSupplyIconEvents(icon, targets[n].entity_id);
      icon = (ItemIcon) null;
    }
    for (int length = targets.Length; length < this.lnkSupplies_.Length; ++length)
    {
      GameObject gameObject = this.prefabItemIcon.Clone(this.lnkSupplies_[length]);
      this.supplyObjects_.Add(gameObject);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      component.SetModeSupply();
      component.SetEmpty(true);
    }
  }

  private void setClickSupplyIconEvents(ItemIcon icon, int itemId)
  {
    icon.onClick = (Action<ItemIcon>) (_ => this.onClickedEditItem());
    icon.EnableLongPressEvent((Action<GameCore.ItemInfo>) (_ => this.onClickedItemIcon(itemId)));
  }

  public void onClickedEditItem()
  {
    Action eventClickedEditItem = this.eventClickedEditItem;
    if (eventClickedEditItem == null)
      return;
    eventClickedEditItem();
  }

  private void onClickedItemIcon(int itemId)
  {
    Action<int> eventClickedSupply = this.eventClickedSupply;
    if (eventClickedSupply == null)
      return;
    eventClickedSupply(itemId);
  }

  private void destroyObjects(List<GameObject> targets)
  {
    foreach (Object target in targets)
      Object.Destroy(target);
    targets.Clear();
  }

  public void initModeSwitch(
    bool[] bEnables,
    Action<SortieDeckMainPanel.DeckMode> callbackChanged)
  {
    this.eventModeChanged_ = callbackChanged;
    int num = (int) this.deckMode;
    bool flag = true;
    if (this.objDeckModeButtons_.Length != ((IEnumerable<bool>) bEnables).Count<bool>((Func<bool, bool>) (b => b)))
    {
      num = ((IEnumerable<bool>) bEnables).FirstIndexOrNull<bool>((Func<bool, bool>) (b => b)) ?? -1;
      flag = false;
      if (num >= 0)
        this.deckMode = (SortieDeckMainPanel.DeckMode) num;
    }
    for (int index = 0; index < this.objDeckModeButtons_.Length; ++index)
    {
      this.objDeckModeButtons_[index].SetActive(index == num);
      ((UIButtonColor) this.objDeckModeButtons_[index].GetComponent<UIButton>()).isEnabled = flag;
    }
  }

  public void onClickedChangeNormalDeck() => this.changeMode(SortieDeckMainPanel.DeckMode.Normal);

  public void onClickedChangeCustomDeck() => this.changeMode(SortieDeckMainPanel.DeckMode.Custom);

  private void changeMode(SortieDeckMainPanel.DeckMode mode)
  {
    ((IEnumerable<GameObject>) this.objDeckModeButtons_).ToggleOnce((int) mode);
    this.objEditDeckName_.SetActive(mode == SortieDeckMainPanel.DeckMode.Custom);
    if (this.deckMode == mode)
      return;
    this.deckMode = mode;
    Action<SortieDeckMainPanel.DeckMode> eventModeChanged = this.eventModeChanged_;
    if (eventModeChanged == null)
      return;
    eventModeChanged(mode);
  }

  public void onClickedLeaderSkillDetail() => this.StartCoroutine(this.doPopupLeaderSkill());

  private IEnumerator doPopupLeaderSkill()
  {
    IEnumerator e = PopupLeaderFriendSkill.show(this.prefabUnitIcon, false, this.leaderUnit, this.friendUnit, this.friendUnit?.leader_skill?.skill);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onClickedBattleSetting()
  {
    Action clickedBattleSetting = this.eventClickedBattleSetting;
    if (clickedBattleSetting == null)
      return;
    clickedBattleSetting();
  }

  public void onClickedEditDeck()
  {
    Action eventClickedEditDeck = this.eventClickedEditDeck;
    if (eventClickedEditDeck != null)
      eventClickedEditDeck();
    this.dicStackNumber_.Remove(this.deckMode);
  }

  public void onClickedEditDeckName()
  {
    if (this.currentDeck == null || !this.currentDeck.isCustom)
      return;
    PopupEditDeckName.show(this.prefabEditDeckName, this.txtDeckName_, this.currentDeck.customDeck);
  }

  public enum DeckMode
  {
    Normal,
    Custom,
  }

  private class FriendInfo
  {
    public PlayerUnit playerUnit { get; private set; }

    public FriendInfo(WebAPI.Response.FriendStatus status)
    {
      this.makePlayerUnit(status.target_leader_unit, status.target_player_items, status.target_player_reisou_items, status.target_player_awake_skills, status.target_leader_unit_over_killers);
    }

    private void makePlayerUnit(
      PlayerUnit target,
      PlayerItem[] gears,
      PlayerGearReisouSchema[] reisous,
      PlayerAwakeSkill[] awakeSkills,
      PlayerUnit[] overkillers)
    {
      target.primary_equipped_gear = target.FindEquippedGear(gears);
      target.primary_equipped_gear2 = target.FindEquippedGear2(gears);
      target.primary_equipped_gear3 = target.FindEquippedGear3(gears);
      target.primary_equipped_reisou = target.FindEquippedReisou(gears, reisous);
      target.primary_equipped_reisou2 = target.FindEquippedReisou2(gears, reisous);
      target.primary_equipped_reisou3 = target.FindEquippedReisou3(gears, reisous);
      target.usedPrimary = PlayerUnit.UsedPrimary.All;
      if (target.primary_equipped_gear != (PlayerItem) null)
        target.primary_equipped_gear.broken = false;
      if (target.primary_equipped_gear2 != (PlayerItem) null)
        target.primary_equipped_gear2.broken = false;
      if (target.primary_equipped_gear3 != (PlayerItem) null)
        target.primary_equipped_gear3.broken = false;
      this.playerUnit = target;
    }
  }
}
