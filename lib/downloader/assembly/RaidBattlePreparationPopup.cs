// Decompiled with JetBrains decompiler
// Type: RaidBattlePreparationPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RaidBattlePreparationPopup : MonoBehaviour
{
  [SerializeField]
  private RaidBattleSortiePopup sortiePopup;
  [SerializeField]
  private RaidBattleGuestSelectPopup guestSelectPopup;
  [SerializeField]
  private ToggleTweenPositionControl toggleAuto_;
  private DeckInfo[] deckInfos;
  private DeckInfo deckInfo;
  private PlayerUnit friend;
  private PlayerItem[] supply;
  private int[] usedUnitId;
  private Persist.AutoBattleSetting saveData_;
  private IBattlePreparationPopup menu;
  private RaidBattlePreparationPopup.MODE mode;

  public IEnumerator InitializeAsync(
    IBattlePreparationPopup menu,
    RaidBattlePreparationPopup.MODE mode,
    GuildRaidPlayerHelpers[] helpers,
    string[] usedHelpers,
    string recommendCombat)
  {
    RaidBattlePreparationPopup parent = this;
    parent.menu = menu;
    parent.deckInfos = menu.GetPopupDecks();
    parent.deckInfo = (DeckInfo) null;
    parent.friend = menu.GetPopupFriend();
    parent.supply = menu.GetPopupSupply();
    parent.usedUnitId = menu.GetPopupGrayUnitIds();
    try
    {
      parent.saveData_ = Persist.autoBattleSetting.Data;
    }
    catch
    {
      Persist.autoBattleSetting.Delete();
      parent.saveData_ = Persist.autoBattleSetting.Data = new Persist.AutoBattleSetting();
    }
    if (Object.op_Implicit((Object) parent.toggleAuto_))
      parent.toggleAuto_.resetSwitch(parent.saveData_.isAutoBattle);
    yield return (object) parent.sortiePopup.InitializeAsync(parent, parent.deckInfos, parent.usedUnitId, parent.friend, parent.supply, recommendCombat);
    yield return (object) parent.guestSelectPopup.InitializeAsync(parent, helpers, usedHelpers);
    switch (mode)
    {
      case RaidBattlePreparationPopup.MODE.Sortie:
        parent.showSortie();
        break;
      case RaidBattlePreparationPopup.MODE.Guest:
        parent.showGuestSelect();
        break;
    }
  }

  public void resetModeSwitch(bool bExistCustom) => this.sortiePopup.resetModeSwitch(bExistCustom);

  public IEnumerator ReloadAsync()
  {
    RaidBattlePreparationPopup preparationPopup = this;
    preparationPopup.deckInfos = preparationPopup.menu.GetPopupDecks();
    if (preparationPopup.deckInfo != null)
    {
      // ISSUE: reference to a compiler-generated method
      preparationPopup.deckInfo = Array.Find<DeckInfo>(preparationPopup.deckInfos, new Predicate<DeckInfo>(preparationPopup.\u003CReloadAsync\u003Eb__14_0));
    }
    preparationPopup.friend = preparationPopup.menu.GetPopupFriend();
    preparationPopup.supply = preparationPopup.menu.GetPopupSupply();
    preparationPopup.usedUnitId = preparationPopup.menu.GetPopupGrayUnitIds();
    if (preparationPopup.mode == RaidBattlePreparationPopup.MODE.Sortie)
      yield return (object) preparationPopup.sortiePopup.ReloadAsync(preparationPopup.deckInfos, preparationPopup.usedUnitId, preparationPopup.friend, preparationPopup.supply);
  }

  public void OnBackButton()
  {
    if (this.mode == RaidBattlePreparationPopup.MODE.Guest)
      this.showSortie();
    else
      this.menu.OnPopupClose();
  }

  public void OnGuestDecided(GvgCandidate friend)
  {
    this.friend = friend == null ? (PlayerUnit) null : friend.player_unit;
    GuildUtil.RaidFriend = friend;
    this.StartCoroutine(this.updateFriend());
  }

  private IEnumerator updateFriend()
  {
    yield return (object) this.sortiePopup.SetFriendUnit(this.friend);
    this.showSortie();
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.toggleAuto_) || this.saveData_ == null || this.saveData_.isAutoBattle == this.toggleAuto_.isSwitch)
      return;
    this.saveData_.isAutoBattle = this.toggleAuto_.isSwitch;
  }

  public void OnClickUnitIcon(PlayerUnit unit, bool isFriend)
  {
    if (isFriend)
      this.showGuestSelect();
    else
      this.menu.OnPopupUnitDetailOpen(unit, this.deckInfo.player_units, isFriend);
  }

  public void OnLongPressUnitIcon(PlayerUnit unit, bool isFriend)
  {
    if (unit == (PlayerUnit) null)
      return;
    PlayerUnit[] playerUnitArray;
    if (!isFriend)
      playerUnitArray = this.deckInfo.player_units;
    else
      playerUnitArray = new PlayerUnit[1]{ unit };
    PlayerUnit[] units = playerUnitArray;
    this.menu.OnPopupUnitDetailOpen(unit, units, isFriend);
  }

  public void OnChangeDeckMode(bool bCustom) => this.menu.OnChangeDeckMode(bCustom);

  public void OnChangeDeck(DeckInfo deck) => this.deckInfo = deck;

  public void OnAutoDeckEditButton() => this.menu.OnPopupAutoDeckEdit();

  public void OnDeckEditButton() => this.menu.OnPopupDeckEditOpen(this.deckInfo);

  public void OnGearEquipButton() => this.menu.OnPopupGearEquipOpen();

  public void OnGearRepairButton() => this.menu.OnPopupGearRepairOpen();

  public void OnSupplyEquipButton() => this.menu.OnPopupSupplyEquipOpen();

  public void OnBattleConfigButton() => this.menu.OnPopupBattleConfigOpen();

  public void OnSortieButton() => this.menu.OnPopupSortie(this.deckInfo);

  public RaidBattleSortiePopup GetSortiePopup() => this.sortiePopup;

  private void showSortie()
  {
    this.guestSelectPopup.Hide();
    ((Component) this.sortiePopup).gameObject.SetActive(true);
    this.sortiePopup.IsPush = false;
    this.mode = RaidBattlePreparationPopup.MODE.Sortie;
  }

  private void showGuestSelect()
  {
    ((Component) this.sortiePopup).gameObject.SetActive(false);
    this.StartCoroutine(this.guestSelectPopup.Show());
    this.guestSelectPopup.IsPush = false;
    this.mode = RaidBattlePreparationPopup.MODE.Guest;
  }

  public enum MODE
  {
    Sortie,
    Guest,
  }
}
