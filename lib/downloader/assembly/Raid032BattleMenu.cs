// Decompiled with JetBrains decompiler
// Type: Raid032BattleMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using DeckOrganization;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Raid032BattleMenu : BackButtonMenuBase, IBattlePreparationPopup
{
  [SerializeField]
  private RaidBattleStatus mBattleStatus;
  [SerializeField]
  private Raid032BattleBossInfo mBossInfo;
  [SerializeField]
  private UIButton mMainBattleSelectBtn;
  [SerializeField]
  private UIButton mSimulatedBattleSelectBtn;
  [SerializeField]
  private UIButton mMainBattleChallengeBtn;
  [SerializeField]
  private UIButton mSimulatedBattleChallengeBtn;
  [SerializeField]
  private GameObject mSimulatedBattleAttentions;
  [SerializeField]
  private UILabel mRankingAllLbl;
  [SerializeField]
  private UILabel mRankingGuildLbl;
  [SerializeField]
  private Transform mPopupAnchor;
  [SerializeField]
  private SpriteRenderer mBackground;
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  private WebAPI.Response.GuildraidBattleDetail mResponse;
  private GuildRaid mMasterData;
  private GameObject mMapInfoPopupPrefab;
  private GameObject mEnemyInfoPopupPrefab;
  private GameObject mRaidPreparationPopupPrefab;
  private RaidBattlePreparationPopup mPreparationPopup;
  private GameObject detailPopup;
  private List<ItemIcon> itemIconList = new List<ItemIcon>();
  private int loop_count;
  private Raid032BattleMenu.BattleMode mMode;
  private string[] mUsedHelpers;
  private bool fromBattle;

  public bool isInitializeSucceeded { get; private set; }

  public bool isCustomDeckMode { get; private set; }

  public IEnumerator initAsync()
  {
    if (Object.op_Equality((Object) this.mRaidPreparationPopupPrefab, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/raid032_battle/dir_raid_battle_attack_target_Custum").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mRaidPreparationPopupPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public IEnumerator onStartSceneAsync(
    int loopCount,
    int raid_id,
    bool isSimulation = false,
    bool fromBattle = false)
  {
    Raid032BattleMenu raid032BattleMenu = this;
    raid032BattleMenu.fromBattle = fromBattle;
    raid032BattleMenu.loop_count = loopCount;
    Future<WebAPI.Response.GuildraidBattleDetail> ft = WebAPI.GuildraidBattleDetail(raid032BattleMenu.loop_count, raid_id, new Action<WebAPI.Response.UserError>(raid032BattleMenu.webErrorCallback));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ft.Result != null)
    {
      raid032BattleMenu.mResponse = ft.Result;
      GuildUtil.rp = raid032BattleMenu.mResponse.rp;
      GuildUtil.rp_max = ((IEnumerable<GuildRaidSettings>) MasterData.GuildRaidSettingsList).FirstOrDefault<GuildRaidSettings>((Func<GuildRaidSettings, bool>) (x => x.key == "RP_BASE_MAX")).value;
      if (!MasterData.GuildRaid.TryGetValue(raid_id, out raid032BattleMenu.mMasterData))
      {
        Debug.LogError((object) ("There is no MasterData in local [ID:" + (object) raid_id + "]"));
      }
      else
      {
        GuildUtil.RaidUsedUnitIds = raid032BattleMenu.mResponse.used_player_unit_ids;
        GuildUtil.UpdateRaidDeckInfo();
        raid032BattleMenu.updateRankingInfo();
        raid032BattleMenu.isCustomDeckMode = Persist.guildRaidLastSortie.Data.isCustom;
        raid032BattleMenu.checkEmptyCustomDeck();
        string path = "Prefabs/BackGround/101_plain_daytime";
        GuildRaidPeriod guildRaidPeriod;
        if (MasterData.GuildRaidPeriod.TryGetValue(raid032BattleMenu.mMasterData.period_id, out guildRaidPeriod) && !string.IsNullOrEmpty(guildRaidPeriod.bg_path))
          path = guildRaidPeriod.bg_path;
        Future<Sprite> prefabBgF = new ResourceObject(path).Load<Sprite>();
        e = prefabBgF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Sprite result = prefabBgF.Result;
        raid032BattleMenu.mBackground.sprite = result;
        yield return (object) raid032BattleMenu.mBossInfo.InitAsync(raid032BattleMenu.mMasterData, raid032BattleMenu.mResponse);
        yield return (object) raid032BattleMenu.mBattleStatus.InitAsync();
        if (isSimulation)
          raid032BattleMenu.changeMode(Raid032BattleMenu.BattleMode.Simulated);
        else
          raid032BattleMenu.changeMode(Raid032BattleMenu.BattleMode.Main);
        raid032BattleMenu.isInitializeSucceeded = true;
      }
    }
  }

  private void checkEmptyCustomDeck(bool bResetModeSwitch = false)
  {
    if (!this.isCustomDeckMode || Util.checkUnlockedPlayerLevel(Player.Current.level) && ((IEnumerable<PlayerCustomDeck>) SMManager.Get<PlayerCustomDeck[]>()).Any<PlayerCustomDeck>((Func<PlayerCustomDeck, bool>) (x => ((IEnumerable<int>) x.player_unit_ids).Any<int>((Func<int, bool>) (i => i != 0)))))
      return;
    this.isCustomDeckMode = false;
    Persist<Persist.GuildRaidLastSortie> guildRaidLastSortie = Persist.guildRaidLastSortie;
    guildRaidLastSortie.Data.isCustom = false;
    guildRaidLastSortie.Data.customDeckNumber = 0;
    guildRaidLastSortie.Flush();
    if (!bResetModeSwitch)
      return;
    this.mPreparationPopup.resetModeSwitch(false);
  }

  private void updateRankingInfo()
  {
    if (this.mResponse.damage_rank_in_all.HasValue)
      this.mRankingAllLbl.SetTextLocalize(this.mResponse.damage_rank_in_all.Value);
    else
      this.mRankingAllLbl.SetTextLocalize("---");
    if (this.mResponse.damage_rank_in_guild.HasValue)
      this.mRankingGuildLbl.SetTextLocalize(this.mResponse.damage_rank_in_guild.Value);
    else
      this.mRankingGuildLbl.SetTextLocalize("---");
  }

  public IEnumerator onBackSceneAsync()
  {
    this.mBossInfo.Reload(this.mMode == Raid032BattleMenu.BattleMode.Main);
    GuildUtil.UpdateRaidDeckInfo();
    if (Object.op_Inequality((Object) this.mPreparationPopup, (Object) null))
      yield return (object) this.mPreparationPopup.ReloadAsync();
  }

  public void onCharangeButton()
  {
    PlayerUnit[] canRaidUnits = this.GetCanRaidUnits();
    if (this.mMode == Raid032BattleMenu.BattleMode.Simulated || GuildUtil.rp > 0)
    {
      if (canRaidUnits == null || ((IEnumerable<PlayerUnit>) canRaidUnits).Count<PlayerUnit>() <= 0)
        ModalWindow.Show(Consts.GetInstance().GUILD_RAID_CAN_NOT_CHALLENGE, Consts.GetInstance().GUILD_RAID_NO_USED_UNIT, (Action) (() => { }));
      else
        this.StartCoroutine(this.openBattlePreparationPopup());
    }
    else
    {
      Consts instance = Consts.GetInstance();
      this.StartCoroutine(PopupCommon.Show(instance.GUILD_RAID_RP_LACK_TITLE, instance.GUILD_RAID_RP_LACK_MESSAGE));
    }
  }

  public void onCancelButton() => this.onBackButton();

  public void onMainBattleButton() => this.changeMode(Raid032BattleMenu.BattleMode.Main);

  public void onSimulatedBattleButton() => this.changeMode(Raid032BattleMenu.BattleMode.Simulated);

  public void onDamageRankingButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Raid032MyRankingScene.changeScene(this.mMasterData);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (this.fromBattle)
    {
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      RaidTopScene.ChangeSceneBattleFinish();
    }
    else
      this.backScene();
  }

  private void changeMode(Raid032BattleMenu.BattleMode mode)
  {
    if (mode != Raid032BattleMenu.BattleMode.Main)
    {
      if (mode != Raid032BattleMenu.BattleMode.Simulated)
        return;
      ((UIButtonColor) this.mMainBattleSelectBtn).isEnabled = true;
      ((Component) this.mMainBattleChallengeBtn).gameObject.SetActive(false);
      ((UIButtonColor) this.mSimulatedBattleSelectBtn).isEnabled = false;
      ((Component) this.mSimulatedBattleChallengeBtn).gameObject.SetActive(true);
      this.mSimulatedBattleAttentions.SetActive(true);
      this.mBossInfo.SetRewardsEnable(false);
      this.mBattleStatus.DisableAllLamp();
      this.mMode = Raid032BattleMenu.BattleMode.Simulated;
    }
    else
    {
      ((UIButtonColor) this.mMainBattleSelectBtn).isEnabled = false;
      ((Component) this.mMainBattleChallengeBtn).gameObject.SetActive(true);
      ((UIButtonColor) this.mSimulatedBattleSelectBtn).isEnabled = true;
      ((Component) this.mSimulatedBattleChallengeBtn).gameObject.SetActive(false);
      this.mSimulatedBattleAttentions.SetActive(false);
      this.mBossInfo.SetRewardsEnable(true);
      this.mBattleStatus.EnableAllLamp();
      this.mMode = Raid032BattleMenu.BattleMode.Main;
    }
  }

  private IEnumerator openBattlePreparationPopup()
  {
    Raid032BattleMenu menu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<WebAPI.Response.GuildraidBattleHelper> ft = WebAPI.GuildraidBattleHelper(RaidBattleGuestSelectPopup.GetSelectedCategoryId(), new Action<WebAPI.Response.UserError>(menu.webErrorCallback));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ft.Result != null)
    {
      WebAPI.Response.GuildraidBattleHelper result = ft.Result;
      menu.mUsedHelpers = result.used_helpers;
      menu.mPreparationPopup = menu.mRaidPreparationPopupPrefab.CloneAndGetComponent<RaidBattlePreparationPopup>(menu.mPopupAnchor);
      yield return (object) null;
      yield return (object) menu.mPreparationPopup.InitializeAsync((IBattlePreparationPopup) menu, RaidBattlePreparationPopup.MODE.Sortie, result.helpers, menu.mUsedHelpers, menu.mResponse.recommend_strength);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
  }

  public void OnChangeDeckMode(bool bCustom)
  {
    if (this.isCustomDeckMode != bCustom)
      this.mPreparationPopup.OnChangeDeck((DeckInfo) null);
    this.isCustomDeckMode = bCustom;
    this.StartCoroutine(this.doReloadPopupSortieDeck());
  }

  private IEnumerator doReloadPopupSortieDeck()
  {
    if (Object.op_Inequality((Object) this.mPreparationPopup, (Object) null))
    {
      CommonRoot cRoot = Singleton<CommonRoot>.GetInstance();
      cRoot.ShowLoadingLayer(0);
      DateTime wait = DateTime.Now.AddSeconds(0.3);
      IEnumerator e = this.mPreparationPopup.ReloadAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      while (wait > DateTime.Now)
        yield return (object) null;
      cRoot.HideLoadingLayer();
      cRoot = (CommonRoot) null;
    }
  }

  public DeckInfo[] GetPopupDecks()
  {
    this.checkEmptyCustomDeck(true);
    if (this.isCustomDeckMode)
      return ((IEnumerable<PlayerCustomDeck>) SMManager.Get<PlayerCustomDeck[]>()).Select<PlayerCustomDeck, DeckInfo>((Func<PlayerCustomDeck, DeckInfo>) (x => PlayerCustomDeck.createGuildRaidDeckInfo(x))).ToArray<DeckInfo>();
    return new DeckInfo[1]
    {
      PlayerDeck.createGuildRaidDeck(GuildUtil.RaidDeck)
    };
  }

  public PlayerUnit GetPopupFriend()
  {
    if (GuildUtil.RaidFriend == null)
      return (PlayerUnit) null;
    if (!((IEnumerable<string>) this.mUsedHelpers).Contains<string>(GuildUtil.RaidFriend.player_id))
      return GuildUtil.RaidFriend.player_unit;
    GuildUtil.RaidFriend = (GvgCandidate) null;
    return (PlayerUnit) null;
  }

  public PlayerItem[] GetPopupSupply() => SMManager.Get<PlayerItem[]>().AllRaidSupplies();

  public int[] GetPopupGrayUnitIds() => GuildUtil.RaidUsedUnitIds;

  public void OnPopupSortie(DeckInfo deck)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.sortieBattle(deck));
  }

  public void OnPopupClose()
  {
    if (!Object.op_Inequality((Object) this.mPreparationPopup, (Object) null))
      return;
    Object.Destroy((Object) ((Component) this.mPreparationPopup).gameObject);
    this.mPreparationPopup = (RaidBattlePreparationPopup) null;
  }

  public void OnPopupUnitDetailOpen(PlayerUnit unit, PlayerUnit[] units, bool isFriend)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (isFriend)
      Unit0042Scene.changeSceneFriendUnit(true, unit.player_id, unit.id);
    else
      Unit0042Scene.changeScene(true, unit, units);
  }

  public void OnPopupAutoDeckEdit()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.StartCoroutine(this.AutoDeckEditAsync());
  }

  private PlayerUnit[] GetCanRaidUnits()
  {
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    int[] usedUnitIds = this.GetPopupGrayUnitIds();
    Func<PlayerUnit, bool> predicate = (Func<PlayerUnit, bool>) (x => !((IEnumerable<int>) usedUnitIds).Contains<int>(x.id) && !x.IsBrokenEquippedGear);
    return ((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>(predicate).ToArray<PlayerUnit>();
  }

  private IEnumerator AutoDeckEditAsync()
  {
    Raid032BattleMenu raid032BattleMenu = this;
    bool bok = false;
    CommonElement element = CommonElement.none;
    IEnumerator waitPopup = Unit0046ConfirmAutoOrganization.doPopup((string) null, (Action<CommonElement>) (_element =>
    {
      bok = true;
      element = _element;
    }));
    while (waitPopup.MoveNext())
      yield return waitPopup.Current;
    if (bok)
    {
      PlayerUnit[] array = ((IEnumerable<PlayerUnit>) raid032BattleMenu.GetCanRaidUnits()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.cost <= 75)).ToArray<PlayerUnit>();
      if (element != CommonElement.none)
        array = ((IEnumerable<PlayerUnit>) array).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.GetElement() == element)).ToArray<PlayerUnit>();
      Creator deckCreator = new Creator((PlayerUnit[]) null, array, (List<DeckOrganization.Filter>) null, 1, 5, SMManager.Get<Player>().max_cost);
      IEnumerator e = deckCreator.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (deckCreator.isSuccess)
      {
        GuildUtil.RaidDeck = deckCreator.result_ == null ? new PlayerUnit[0] : deckCreator.result_.ToArray();
        yield return (object) raid032BattleMenu.mPreparationPopup.ReloadAsync();
      }
      else
      {
        bool bwait = true;
        ModalWindow.Show(Consts.GetInstance().UNIT_0046_AUTODECK_ERROR_TITLE, Consts.GetInstance().UNIT_0046_AUTODECK_ERROR_MESSAGE, (Action) (() => bwait = false));
        while (bwait)
          yield return (object) null;
      }
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      raid032BattleMenu.IsPush = false;
    }
  }

  public void OnPopupDeckEditOpen(DeckInfo deck)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (this.isCustomDeckMode)
      EditCustomDeckScene.changeScene(true, deck.deck_number, this.GetPopupGrayUnitIds());
    else
      Unit00468Scene.changeScene00468Raid(true, this.mMode == Raid032BattleMenu.BattleMode.Simulated);
  }

  private void saveData(DeckInfo deck)
  {
    Persist<Persist.GuildRaidLastSortie> guildRaidLastSortie = Persist.guildRaidLastSortie;
    bool flag = false;
    if (guildRaidLastSortie.Data.isCustom != deck.isCustom)
    {
      guildRaidLastSortie.Data.isCustom = deck.isCustom;
      flag = true;
    }
    if (deck.isCustom && guildRaidLastSortie.Data.customDeckNumber != deck.deck_number)
    {
      guildRaidLastSortie.Data.customDeckNumber = deck.deck_number;
      flag = true;
    }
    if (!flag)
      return;
    guildRaidLastSortie.Flush();
  }

  public void OnPopupGearEquipOpen()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Unit00468Scene.changeScene00412Raid(true);
  }

  public void OnPopupGearRepairOpen()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Bugu00524Scene.ChangeScene(true);
  }

  public void OnPopupSupplyEquipOpen()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>().ToArray(), ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllRaidSupplies()).ToArray<PlayerItem>())).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = true,
      mode = Quest00210Scene.Mode.Raid
    });
  }

  public void OnPopupBattleConfigOpen()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.openPopupBattleSetting());
  }

  private IEnumerator openPopupBattleSetting()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Raid032BattleMenu raid032BattleMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      raid032BattleMenu.IsPush = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) Quest0028PopupBattleSetting.show();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void onMapInfoButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openMapInfoPopup());
  }

  private IEnumerator openMapInfoPopup()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    if (Object.op_Equality((Object) this.mMapInfoPopupPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_029_tower_stage_status__anim_popup01").Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mMapInfoPopupPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    Tower029MapcheckPopup popup = this.mMapInfoPopupPrefab.CloneAndGetComponent<Tower029MapcheckPopup>();
    ((Component) popup).gameObject.SetActive(false);
    yield return (object) popup.InitializeAsync(this.mMasterData.stage_id);
    ((Component) popup).gameObject.SetActive(true);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<PopupManager>.GetInstance().open(((Component) popup).gameObject, isCloned: true);
  }

  public void onEnemyInfoButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openEnemyInfoPopup());
  }

  private IEnumerator openEnemyInfoPopup()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    if (Object.op_Equality((Object) this.mEnemyInfoPopupPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_032_raid_enemy_status__anim_popup01").Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mEnemyInfoPopupPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    RaidBattleEnemyInfoPopup popup = this.mEnemyInfoPopupPrefab.CloneAndGetComponent<RaidBattleEnemyInfoPopup>();
    ((Component) popup).gameObject.SetActive(false);
    yield return (object) popup.InitializeAsync(this.mMasterData, this.mResponse.boss_total_damage, this.loop_count);
    ((Component) popup).gameObject.SetActive(true);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<PopupManager>.GetInstance().open(((Component) popup).gameObject, isCloned: true);
  }

  private IEnumerator sortieBattle(DeckInfo deck)
  {
    Raid032BattleMenu raid032BattleMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    raid032BattleMenu.saveData(deck);
    bool is_simulation = raid032BattleMenu.mMode == Raid032BattleMenu.BattleMode.Simulated;
    int[] playerUnitIds = deck.player_unit_ids;
    // ISSUE: explicit non-virtual call
    // ISSUE: explicit non-virtual call
    string support_player_id = __nonvirtual (raid032BattleMenu.GetPopupFriend()) != (PlayerUnit) null ? __nonvirtual (raid032BattleMenu.GetPopupFriend()).player_id : string.Empty;
    // ISSUE: explicit non-virtual call
    // ISSUE: explicit non-virtual call
    Future<WebAPI.Response.GuildraidBattleStart> ft = WebAPI.GuildraidBattleStart(deck.deck_number, deck.deck_type_id, is_simulation, raid032BattleMenu.loop_count, playerUnitIds, raid032BattleMenu.mMasterData.ID, support_player_id, __nonvirtual (raid032BattleMenu.GetPopupFriend()) != (PlayerUnit) null ? __nonvirtual (raid032BattleMenu.GetPopupFriend()).id : 0, new Action<WebAPI.Response.UserError>(raid032BattleMenu.webErrorCallback));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ft.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 4;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      yield return (object) null;
      Persist.battleEnvironment.Delete();
      Persist.pvpSuspend.Delete();
      BattleInfo battleInfo = BattleInfo.MakeRaidBattleInfo(ft.Result);
      Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
    }
  }

  private void webErrorCallback(WebAPI.Response.UserError error)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string title;
    string message;
    if (error.Code == "GRB016")
    {
      title = Consts.GetInstance().GUILD_RAID_BOSS_WAS_ALREADY_DIED_TITLE;
      message = Consts.GetInstance().GUILD_RAID_BOSS_WAS_ALREADY_DIED_MESSAGE;
    }
    else
    {
      title = error.Code;
      message = error.Reason;
    }
    ModalWindow.Show(title, message, (Action) (() =>
    {
      this.IsPush = false;
      Singleton<NGSceneManager>.GetInstance();
      if (error.Code == "GRB016")
        this.onBackButton();
      else
        MypageScene.ChangeSceneOnError();
    }));
  }

  private enum BattleMode
  {
    Main,
    Simulated,
  }
}
