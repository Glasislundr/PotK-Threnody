// Decompiled with JetBrains decompiler
// Type: NGGameDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using gu3;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnitTraining;
using UnityEngine;

#nullable disable
public class NGGameDataManager : Singleton<NGGameDataManager>
{
  private Modified<Player> player;
  private Modified<SeaPlayer> seaPlayer;
  private Modified<PlayerUnit[]> unit;
  private Modified<NGGameDataManager.TimeCounter> timeCounter;
  private DateTime oldTime;
  private bool started;
  public List<PlayerLoginBonus> loginBonuses;
  public OfficialInformationArticle[] officialInfos;
  public OfficialInformationPopup officialInfoPopup;
  public int[] playbackEventIds;
  public List<LevelRewardSchemaMixin> playerLevelRewards;
  public DateTime signedInAt;
  public DateTime? gachaLatestStartTime;
  public LimitShopInfo[] limitShopInfos;
  public DateTime? raidMedalShopLatestStartTime;
  public bool has_exchangeable_subcoin;
  public bool has_near_dead_subcoin;
  public DateTime lastInfoTime;
  public DateTime lastGachaTime;
  public DateTime lastSlotTime;
  public bool infoThrough;
  public bool isCallHomeUpdateAllData = true;
  public int challenge_point;
  public int challenge_point_max;
  public Dictionary<string, Texture2D> webImageCache;
  private GameObject[] otherBattleAtlas;
  private GameObject[] resideResources;
  public int lastReferenceUnitID = -1;
  public int lastReferenceUnitIndex = -1;
  public int lastReferenceItemID = -1;
  public int lastReferenceItemIndex = -1;
  public bool isReisouScene;
  public int[] clearedExtraQuestSIds;
  public int[] clearedTowerStageIds;
  public int[] clearedRaidQuestSIds;
  public string[] favoriteFriends = new string[0];
  private int receivedFriendRequestCount;
  private Dictionary<int, int> getTablePieceSameCharacterIds = new Dictionary<int, int>();
  public bool successStartSceneAsyncProxyImpl = true;
  public bool isActiveTotalPaymentBonus;
  public bool hasReceivableTotalPaymentBonus;
  public bool newbiePacks;
  public bool receivableGift;
  public bool unReadTalkMessage;
  public bool isOpenRoulette;
  public bool isCanRoulette;
  public bool hasFillableLoginbonus;
  public bool questAutoLap;
  public List<GuildChatMessageData> chatDataList = new List<GuildChatMessageData>();
  public bool IsConnectedResultQuestProgressExtra;
  private NGGameDataManager.Boost m_BoostInfo;
  private SM.HotDealInfo[] hotDealInfo = new SM.HotDealInfo[0];
  public SM.HotDealInfo PurchaseHotDeal;
  private bool isColosseum;
  private bool isEarth;
  private bool isSea;
  private CommonQuestType? questType;
  private bool isFromPopupStageList;
  private bool isOpenPvpCampaign;
  private bool isOpenColosseumCampaign;
  public static bool SeaChangeFlag = false;
  public static int UrlSchemePresentId = -1;
  public static string UrlSchemeSerial = "";
  private List<Tuple<int, int>> gachaTicketIDQList;
  public Color baseAmbientLight;
  public Sprite loadingBgSprite;
  public NGGameDataManager.TimeCounter timeInstance;
  private bool mRefreshHomeHome;
  private bool mRefreshGuildTop;
  private bool mRefreshGuildSetting;
  [SerializeField]
  private int webApiUpdateIntervalSeconds = 1800;
  private DateTime updatedTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
  private bool mIsUpdating;
  private bool mIsBeforeUpdating;
  private bool mNeedRetry;
  public bool HasReceivableGuildCheckIn;
  public Dictionary<string, WebAPI.Response.UnitPreviewInheritancePreview_inheritance> dicPreviewInheritance = new Dictionary<string, WebAPI.Response.UnitPreviewInheritancePreview_inheritance>();
  private Dictionary<int, OverkillersSlotRelease.Conditions[]> dicOverkillersSlotReleaseConditions = new Dictionary<int, OverkillersSlotRelease.Conditions[]>();
  private long? revisionUnitTraining_;
  private Dictionary<TrainingType, Ingredients> dicTrainingParams_;
  private NGGameDataManager.UnitTrainingOption unitTrainingOption_;
  private Dictionary<CommonQuestType, Dictionary<int, int>> dicRecommendStrength_ = new Dictionary<CommonQuestType, Dictionary<int, int>>();
  private List<NGSceneManager.SavedSceneLog> sceneChangeLog_;
  private CorpsUtil corpsUtil_;
  private NGGameDataManager.GachaPickupSelectStatus pickupSelectStatus_;
  private bool isEditCustomDeck_;

  public HashSet<int> opened_equip_number_player_unit_ids { get; private set; } = new HashSet<int>();

  public void Set_opened_equip_number_player_unit_ids(int[] ids)
  {
    this.opened_equip_number_player_unit_ids = ids != null ? new HashSet<int>((IEnumerable<int>) ids) : new HashSet<int>();
  }

  public void Add_opened_equip_number_player_unit_ids(int id)
  {
    this.opened_equip_number_player_unit_ids.Add(id);
    PlayerUnit[] array = SMManager.Get<PlayerUnit[]>();
    if (array == null)
      return;
    PlayerUnit playerUnit = Array.Find<PlayerUnit>(array, (Predicate<PlayerUnit>) (x => x.id == id));
    if (!(playerUnit != (PlayerUnit) null))
      return;
    playerUnit.setStatusOpenedEquippedGear3(true);
  }

  public void Remove_opened_equip_number_player_unit_ids(IEnumerable<int> ids)
  {
    foreach (int id in ids)
      this.opened_equip_number_player_unit_ids.Remove(id);
  }

  public int[] corps_period_ids { get; private set; }

  public HashSet<int> corps_player_unit_ids { get; set; } = new HashSet<int>((IEnumerable<int>) new int[0]);

  public int ReceivedFriendRequestCount
  {
    get => this.receivedFriendRequestCount;
    set => this.receivedFriendRequestCount = value;
  }

  public Dictionary<int, int> GetTablePieceSameCharacterIds
  {
    get => this.getTablePieceSameCharacterIds;
    set => this.getTablePieceSameCharacterIds = value;
  }

  public NGGameDataManager.Boost BoostInfo => this.m_BoostInfo;

  public SM.HotDealInfo[] HotDealInfo => this.hotDealInfo;

  public bool IsHotDealActive
  {
    get
    {
      return this.HotDealInfo != null && ((IEnumerable<SM.HotDealInfo>) this.HotDealInfo).Any<SM.HotDealInfo>((Func<SM.HotDealInfo, bool>) (x => x.IsActive));
    }
  }

  public bool IsColosseum
  {
    get => this.isColosseum;
    set => this.isColosseum = value;
  }

  public bool IsEarth
  {
    get => this.isEarth;
    set => this.isEarth = value;
  }

  public bool IsSea
  {
    get => this.isSea;
    set
    {
      this.isSea = value;
      ++this.revisionIsSea;
    }
  }

  public int revisionIsSea { get; private set; }

  public CommonQuestType? QuestType
  {
    get => this.questType;
    set => this.questType = value;
  }

  public bool IsFromPopupStageList
  {
    get => this.isFromPopupStageList;
    set => this.isFromPopupStageList = value;
  }

  public NGGameDataManager.FromPopup fromPopup { get; set; }

  public Action returnSceneFromQuest { get; set; }

  public Action<GameObject[], NGMenuBase> OpenPopup { private get; set; }

  public bool HasOpenPopup => this.OpenPopup != null;

  public PlayerTalkMessage playerTalkMessage { get; set; }

  public PlayerCallLetter[] callLetter { get; set; }

  public void clearScenePopupRecovery()
  {
    this.isFromPopupStageList = false;
    this.fromPopup = NGGameDataManager.FromPopup.None;
    this.returnSceneFromQuest = (Action) null;
    this.OpenPopup = (Action<GameObject[], NGMenuBase>) null;
    this.setSceneChangeLog();
  }

  public void OnceOpenPopup<T>(T[] prefabLoaders, NGMenuBase menu, Action preOpenPopup) where T : class
  {
    if (!this.HasOpenPopup)
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doOpenPopup<T>(prefabLoaders, menu, preOpenPopup, this.OpenPopup));
    this.OpenPopup = (Action<GameObject[], NGMenuBase>) null;
  }

  private IEnumerator doOpenPopup<T>(
    T[] prefabLoaders,
    NGMenuBase menu,
    Action preOpenPopup,
    Action<GameObject[], NGMenuBase> openPopup)
  {
    if (openPopup != null)
    {
      Action action = preOpenPopup;
      if (action != null)
        action();
      GameObject[] prefabs;
      if (prefabLoaders != null)
      {
        Future<GameObject>[] loaders;
        switch (prefabLoaders)
        {
          case Future<GameObject>[] futureArray:
            loaders = futureArray;
            prefabs = new GameObject[prefabLoaders.Length];
            for (int n = 0; n < prefabLoaders.Length; ++n)
            {
              Future<GameObject> loader = loaders[n];
              if (Object.op_Equality((Object) loader.Result, (Object) null))
                yield return (object) loader.Wait();
              prefabs[n] = loader.Result;
              loader = (Future<GameObject>) null;
            }
            break;
          case GameObject[] gameObjectArray:
            prefabs = gameObjectArray;
            break;
          default:
            Debug.LogError((object) string.Format("prefabLoaders({0}) は対応していない型です！", (object) prefabLoaders.GetType()));
            yield break;
        }
        loaders = (Future<GameObject>[]) null;
      }
      else
        prefabs = new GameObject[0];
      openPopup(prefabs, menu);
    }
  }

  public bool IsOpenPvpCampaign
  {
    get => this.isOpenPvpCampaign;
    set => this.isOpenPvpCampaign = value;
  }

  public bool IsOpenColosseumCampaign
  {
    get => this.isOpenColosseumCampaign;
    set => this.isOpenColosseumCampaign = value;
  }

  public bool IsResideResourceLoaded(string name)
  {
    if (this.resideResources != null)
    {
      bool flag = ((IEnumerable<GameObject>) this.resideResources).Any<GameObject>((Func<GameObject, bool>) (x => ((Object) x).name == name));
      if (flag)
        return flag;
    }
    return this.otherBattleAtlas != null && ((IEnumerable<GameObject>) this.otherBattleAtlas).Any<GameObject>((Func<GameObject, bool>) (x => ((Object) x).name == name));
  }

  public IEnumerator LoadResideResources()
  {
    if (PerformanceConfig.GetInstance().IsTuningCommonTextureLoaded)
    {
      this.UnLoadResideResources();
      ResourceObject[] resources = new ResourceObject[23]
      {
        new ResourceObject("GUI/button_text/button_text_prefab"),
        new ResourceObject("GUI/popup_base/popup_base_prefab"),
        new ResourceObject("GUI/popup_base_skill/popup_base_skill_prefab"),
        new ResourceObject("GUI/popup_base_other/popup_base_other_prefab"),
        new ResourceObject("GUI/unit_detail/unit_detail_prefab"),
        new ResourceObject("GUI/unit_sort_filter/unit_sort_filter_prefab"),
        new ResourceObject("GUI/unit_title_short/unit_title_short_prefab"),
        new ResourceObject("GUI/023-5_sozai/023-5_sozai_prefab"),
        new ResourceObject("GUI/004-8-3_sozai/004-8-3_sozai_prefab"),
        new ResourceObject("GUI/008-01_sozai/008-01_sozai_prefab"),
        new ResourceObject("GUI/004-9-9_sozai/004-9-9_sozai_prefab"),
        new ResourceObject("GUI/027-1_sozai/027-1_sozai_prefab"),
        new ResourceObject("GUI/facility_thumb/facility_thumb_prefab"),
        new ResourceObject("GUI/002-2_sozai/002-2_sozai_prefab"),
        new ResourceObject("GUI/002-8_sozai/002-8_sozai_prefab"),
        new ResourceObject("GUI/004-6_sozai/004-6_sozai_prefab"),
        new ResourceObject("GUI/002-17_sozai/002-17_sozai_prefab"),
        new ResourceObject("GUI/004-1_sozai/004-1_sozai_prefab"),
        new ResourceObject("GUI/battleUI/battleUI_prefab"),
        new ResourceObject("GUI/battleUI_common/battleUI_common_prefab"),
        new ResourceObject("GUI/battleUI_duel/battleUI_duel_prefab"),
        new ResourceObject("GUI/map_edit/map_edit_prefab"),
        new ResourceObject("GUI/sea_home/sea_home_prefab")
      };
      Singleton<ResourceManager>.GetInstance().ClearCache();
      int size = resources.Length;
      this.resideResources = new GameObject[size];
      for (int i = 0; i < size; ++i)
      {
        Future<GameObject> prefab = resources[i].Load<GameObject>();
        IEnumerator e = prefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.resideResources[i] = prefab.Result;
        prefab = (Future<GameObject>) null;
      }
    }
  }

  private void UnLoadResideResources()
  {
    if (this.resideResources == null)
      return;
    int length = this.resideResources.Length;
    for (int index = 0; index < length; ++index)
      this.resideResources[index] = (GameObject) null;
    this.resideResources = (GameObject[]) null;
  }

  public IEnumerator LoadOtherBattleAtlas()
  {
    if (this.otherBattleAtlas == null)
    {
      ResourceObject[] resources = new ResourceObject[2]
      {
        new ResourceObject("GUI/button_text/button_text_prefab"),
        new ResourceObject("GUI/popup_base/popup_base_prefab")
      };
      int size = resources.Length;
      this.otherBattleAtlas = new GameObject[size];
      for (int i = 0; i < size; ++i)
      {
        Future<GameObject> prefab = resources[i].Load<GameObject>();
        IEnumerator e = prefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.otherBattleAtlas[i] = prefab.Result;
        prefab = (Future<GameObject>) null;
      }
    }
  }

  public IEnumerator UnLoadOtherBattleAtlas()
  {
    if (this.otherBattleAtlas != null)
    {
      int length = this.otherBattleAtlas.Length;
      for (int index = 0; index < length; ++index)
        this.otherBattleAtlas[index] = (GameObject) null;
      this.otherBattleAtlas = (GameObject[]) null;
      yield break;
    }
  }

  public IEnumerator GetWebImage(string url, UI2DSprite sprite)
  {
    if (!string.IsNullOrEmpty(url))
    {
      IEnumerator e = this.LoadWebImage(url);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Texture2D texture2D = (Texture2D) null;
      Singleton<NGGameDataManager>.GetInstance().webImageCache.TryGetValue(url, out texture2D);
      if (!Object.op_Equality((Object) texture2D, (Object) null))
      {
        ((Texture) texture2D).wrapMode = (TextureWrapMode) 1;
        float width = (float) ((Texture) texture2D).width;
        float height = (float) ((Texture) texture2D).height;
        sprite.sprite2D = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, width, height), new Vector2(0.0f, 0.0f), 100f, 0U, (SpriteMeshType) 0);
        ((UIWidget) sprite).SetDimensions((int) width, (int) height);
        ((Component) sprite).gameObject.SetActive(true);
      }
    }
  }

  public IEnumerator LoadWebImage(string url)
  {
    int errorCount = 0;
    if (url != "")
    {
      if (!Singleton<NGGameDataManager>.GetInstance().webImageCache.ContainsKey(url))
      {
        Singleton<NGGameDataManager>.GetInstance().webImageCache[url] = (Texture2D) null;
        while (Object.op_Equality((Object) Singleton<NGGameDataManager>.GetInstance().webImageCache[url], (Object) null) && errorCount < 3)
        {
          Dictionary<string, object> requestResult = new Dictionary<string, object>();
          yield return (object) WWWUtil.RequestAndCache(url, (Action<Dictionary<string, object>>) (result => requestResult = result));
          if (string.IsNullOrEmpty(((WWW) requestResult["www"]).error))
          {
            Singleton<NGGameDataManager>.GetInstance().webImageCache[url] = (Texture2D) requestResult["texture"];
            break;
          }
          ++errorCount;
        }
      }
      if (Object.op_Equality((Object) Singleton<NGGameDataManager>.GetInstance().webImageCache[url], (Object) null))
        Debug.LogError((object) string.Format("missing download url: {0}", (object) url));
    }
  }

  public bool isInitialized => this.player != null && this.unit != null && this.started;

  private IEnumerator SaveUserInfo()
  {
    if (!Persist.userInfo.Exists)
    {
      Player player;
      while (true)
      {
        player = SMManager.Get<Player>();
        if (player == null || string.IsNullOrEmpty(player.short_id))
          yield return (object) null;
        else
          break;
      }
      Persist.userInfo.Data.userId = player.short_id;
      Persist.userInfo.Flush();
    }
  }

  public void CacheClear()
  {
    this.webImageCache.Clear();
    this.loadingBgSprite = (Sprite) null;
  }

  protected override void Initialize()
  {
    this.player = SMManager.Observe<Player>();
    this.seaPlayer = SMManager.Observe<SeaPlayer>();
    this.unit = SMManager.Observe<PlayerUnit[]>();
    this.timeCounter = SMManager.Observe<NGGameDataManager.TimeCounter>();
    this.oldTime = DateTime.Now;
    this.webImageCache = new Dictionary<string, Texture2D>();
    this.StartCoroutine(this.SaveUserInfo());
    Consts.Update(MasterData.ConstsConstsList);
  }

  private void Start()
  {
    this.timeInstance.Init(this.player, this.seaPlayer);
    SMManager.Change<NGGameDataManager.TimeCounter>(this.timeInstance);
    this.started = true;
  }

  private void Update()
  {
    if (!this.isInitialized)
      return;
    DateTime now = DateTime.Now;
    float totalSeconds = (float) (now - this.oldTime).TotalSeconds;
    this.oldTime = now;
    if (!this.timeCounter.Value.AddDeltaTime(totalSeconds))
      return;
    this.timeCounter.NotifyChanged();
  }

  private void OnDestroy() => this.UnLoadResideResources();

  private void OnApplicationPause(bool pause)
  {
    if (!this.isInitialized)
      return;
    if (pause)
    {
      if (Persist.notification.Data.Ap)
      {
        int num = Mathf.FloorToInt(this.timeInstance.ApFullRecoverySeconds);
        if (num > 0)
          LocalNotification.ScheduleWithTimeInterval(new LocalNotification.Notification()
          {
            category = "AP_RECOVERY",
            message = Consts.GetInstance().AP_RECOVER_PUSHNOTIFICATION_TEXT
          }, num);
      }
      if (Persist.notification.Data.Bp)
      {
        int num = Mathf.FloorToInt(this.timeInstance.BpFullRecoverySeconds);
        if (num > 0)
          LocalNotification.ScheduleWithTimeInterval(new LocalNotification.Notification()
          {
            category = "BP_RECOVERY",
            message = Consts.GetInstance().BP_RECOVER_PUSHNOTIFICATION_TEXT
          }, num);
      }
      if (!Persist.notification.Data.Explore)
        return;
      int exploreBoxSpan = Persist.notification.Data.ExploreBoxSpan;
      if (exploreBoxSpan > 0)
        LocalNotification.ScheduleWithTimeInterval(new LocalNotification.Notification()
        {
          category = "EXPLORE_BOX",
          message = Consts.GetInstance().EXPLORE_BOX_MAX_PUSHNOTIFICATION_TEXT
        }, exploreBoxSpan);
      int exploreProgSpan = Persist.notification.Data.ExploreProgSpan;
      if (exploreProgSpan <= 0)
        return;
      LocalNotification.ScheduleWithTimeInterval(new LocalNotification.Notification()
      {
        category = "EXPLORE_PROG",
        message = Consts.GetInstance().EXPLORE_PROG_MAX_PUSHNOTIFICATION_TEXT
      }, exploreProgSpan);
    }
    else
    {
      LocalNotification.CancelNotificationsWithCategory("AP_RECOVERY");
      LocalNotification.CancelNotificationsWithCategory("BP_RECOVERY");
      LocalNotification.CancelNotificationsWithCategory("EXPLORE_BOX");
      LocalNotification.CancelNotificationsWithCategory("EXPLORE_PROG");
    }
  }

  public bool refreshHomeHome
  {
    set => this.mRefreshHomeHome = value;
  }

  public bool isSeaStartUp { get; private set; } = true;

  public bool refreshGuildTop
  {
    set => this.mRefreshGuildTop = value;
  }

  public bool refreshGuildSetting
  {
    set => this.mRefreshGuildSetting = value;
  }

  public bool IsUpdating => this.mIsUpdating || this.mIsBeforeUpdating;

  private IEnumerator CallHomeStartup()
  {
    IEnumerator e;
    if (this.isCallHomeUpdateAllData)
    {
      Future<WebAPI.Response.HomeStartUp2> handler = Persist.gvgBattleEnvironment.Exists ? WebAPI.HomeStartUp2() : WebAPI.HomeStartUp2Hotdeal();
      e = handler.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.loginBonuses = ((IEnumerable<PlayerLoginBonus>) handler.Result.player_loginbonuses).ToList<PlayerLoginBonus>();
      this.officialInfos = handler.Result.articles;
      this.officialInfoPopup = handler.Result.officialinfo_popup;
      this.postStartUp2(handler.Result);
      this.SortPickupPopups();
      this.playerLevelRewards = ((IEnumerable<LevelRewardSchemaMixin>) handler.Result.player_achieve_level_rewards).ToList<LevelRewardSchemaMixin>();
      this.signedInAt = handler.Result.last_signed_in_at;
      this.gachaLatestStartTime = handler.Result.gacha_latest_start_time;
      this.limitShopInfos = handler.Result.limit_shop_infos;
      this.raidMedalShopLatestStartTime = handler.Result.raid_medal_shop_latest_start_time;
      this.challenge_point = handler.Result.challenge_point;
      this.receivedFriendRequestCount = handler.Result.received_friend_request_count;
      this.isOpenPvpCampaign = handler.Result.is_open_pvp_campaign;
      this.isOpenColosseumCampaign = handler.Result.is_open_colosseum_campaign;
      this.playbackEventIds = handler.Result.story_playback_event_ids;
      this.newbiePacks = handler.Result.has_buyable_newbie_packs;
      this.receivableGift = handler.Result.has_receivable_rewards;
      this.unReadTalkMessage = handler.Result.unread_talk_messages;
      this.isActiveTotalPaymentBonus = handler.Result.is_active_paymentbonus;
      this.hasReceivableTotalPaymentBonus = handler.Result.has_receivable_paymentbonus;
      this.isOpenRoulette = handler.Result.is_open_roulette;
      this.isCanRoulette = handler.Result.can_roulette;
      this.hasFillableLoginbonus = handler.Result.has_fillable_loginbonus;
      this.favoriteFriends = handler.Result.favorite_friend_list;
      this.has_exchangeable_subcoin = handler.Result.has_exchangeable_subcoin;
      this.has_near_dead_subcoin = handler.Result.has_near_dead_subcoin;
      this.corps_period_ids = handler.Result.corps_period_ids;
      this.corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) handler.Result.corps_player_unit_ids);
      this.InitBoostInfo(handler.Result.active_boost_period_id_list, handler.Result.boost_type_id_list);
      this.SetHomeStartUpHotdeal(handler.Result.hotdeal_info);
      try
      {
        this.lastInfoTime = Persist.lastInfoTime.Data.GetLastInfoTime();
        Persist.lastInfoTime.Data.SetLastInfoTime(this.signedInAt);
        Persist.lastInfoTime.Flush();
      }
      catch
      {
        Persist.lastInfoTime.Delete();
      }
      this.isCallHomeUpdateAllData = false;
      handler = (Future<WebAPI.Response.HomeStartUp2>) null;
    }
    else
    {
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "mypage" && MypageRootMenu.CurrentMode == MypageRootMenu.Mode.STORY)
      {
        Future<WebAPI.Response.HomeStartUpTransition> handler = WebAPI.HomeStartUpTransition();
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.SetHomeStartUpResult((WebAPI.Response.HomeStartUpIndexer) handler.Result);
        handler = (Future<WebAPI.Response.HomeStartUpTransition>) null;
      }
      else
      {
        Future<WebAPI.Response.HomeStartUp> handler = WebAPI.HomeStartUp();
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.SetHomeStartUpResult((WebAPI.Response.HomeStartUpIndexer) handler.Result);
        handler = (Future<WebAPI.Response.HomeStartUp>) null;
      }
      try
      {
        this.lastInfoTime = Persist.lastInfoTime.Data.GetLastInfoTime();
        Persist.lastInfoTime.Data.SetLastInfoTime(this.signedInAt);
        Persist.lastInfoTime.Flush();
      }
      catch
      {
        Persist.lastInfoTime.Delete();
      }
    }
  }

  private void SetHomeStartUpResult(WebAPI.Response.HomeStartUpIndexer result)
  {
    if ((result["player_loginbonuses"] as PlayerLoginBonus[]).Length != 0)
    {
      if (this.loginBonuses == null)
        this.loginBonuses = ((IEnumerable<PlayerLoginBonus>) (result["player_loginbonuses"] as PlayerLoginBonus[])).ToList<PlayerLoginBonus>();
      else
        this.loginBonuses.AddRange((IEnumerable<PlayerLoginBonus>) (result["player_loginbonuses"] as PlayerLoginBonus[]));
    }
    if ((result["player_achieve_level_rewards"] as LevelRewardSchemaMixin[]).Length != 0)
    {
      if (this.playerLevelRewards == null)
        this.playerLevelRewards = ((IEnumerable<LevelRewardSchemaMixin>) (result["player_achieve_level_rewards"] as LevelRewardSchemaMixin[])).ToList<LevelRewardSchemaMixin>();
      else
        this.playerLevelRewards.AddRange((IEnumerable<LevelRewardSchemaMixin>) (result["player_achieve_level_rewards"] as LevelRewardSchemaMixin[]));
    }
    this.officialInfos = result["articles"] as OfficialInformationArticle[];
    this.officialInfoPopup = result["officialinfo_popup"] as OfficialInformationPopup;
    this.SortPickupPopups();
    this.signedInAt = (DateTime) result["last_signed_in_at"];
    this.gachaLatestStartTime = result["gacha_latest_start_time"] as DateTime?;
    this.limitShopInfos = result["limit_shop_infos"] as LimitShopInfo[];
    this.raidMedalShopLatestStartTime = result["raid_medal_shop_latest_start_time"] as DateTime?;
    this.challenge_point = (int) result["challenge_point"];
    this.receivedFriendRequestCount = (int) result["received_friend_request_count"];
    this.isOpenPvpCampaign = (bool) result["is_open_pvp_campaign"];
    this.isOpenColosseumCampaign = (bool) result["is_open_colosseum_campaign"];
    this.playbackEventIds = result["story_playback_event_ids"] as int[];
    this.newbiePacks = (bool) result["has_buyable_newbie_packs"];
    this.receivableGift = (bool) result["has_receivable_rewards"];
    this.unReadTalkMessage = (bool) result["unread_talk_messages"];
    this.isActiveTotalPaymentBonus = (bool) result["is_active_paymentbonus"];
    this.hasReceivableTotalPaymentBonus = (bool) result["has_receivable_paymentbonus"];
    this.isOpenRoulette = (bool) result["is_open_roulette"];
    this.isCanRoulette = (bool) result["can_roulette"];
    this.hasFillableLoginbonus = (bool) result["has_fillable_loginbonus"];
    this.has_exchangeable_subcoin = (bool) result["has_exchangeable_subcoin"];
    this.has_near_dead_subcoin = (bool) result["has_near_dead_subcoin"];
    this.corps_period_ids = result["corps_period_ids"] as int[];
    this.InitBoostInfo(result["active_boost_period_id_list"] as int[], result["boost_type_id_list"] as int[]);
    this.SetHomeStartUpHotdeal(result["hotdeal_info"] as SM.HotDealInfo[]);
  }

  private void SetHomeStartUpHotdeal(SM.HotDealInfo[] hotdeal_info)
  {
    this.hotDealInfo = hotdeal_info;
    this.CalcHotDealTimeRemaining();
    this.hotDealInfo = ((IEnumerable<SM.HotDealInfo>) this.hotDealInfo).OrderBy<SM.HotDealInfo, int>((Func<SM.HotDealInfo, int>) (x => x.purchase_limit_sec)).ThenByDescending<SM.HotDealInfo, int>((Func<SM.HotDealInfo, int>) (x => x.priority)).ThenBy<SM.HotDealInfo, int>((Func<SM.HotDealInfo, int>) (x => x.pack_id)).ToArray<SM.HotDealInfo>();
  }

  private void CalcHotDealTimeRemaining()
  {
    foreach (SM.HotDealInfo hotDealInfo in this.HotDealInfo)
      hotDealInfo.EndDateTime = DateTime.Now + TimeSpan.FromSeconds((double) hotDealInfo.purchase_limit_sec);
  }

  public WebAPI.Response.GuildTop GuildTopResponse { get; private set; }

  private IEnumerator UpdateGuildTop()
  {
    if (PlayerAffiliation.Current.isGuildMember())
    {
      Future<WebAPI.Response.GuildTop> handler = WebAPI.GuildTop(Persist.guildHeaderChat.Data.latestLogId, (Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("GameManager Failed Update Guild : " + e.Code))));
      IEnumerator e1 = handler.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.GuildTopResponse = handler.Result;
      if (this.GuildTopResponse == null)
      {
        this.mRefreshGuildSetting = true;
        this.mNeedRetry = true;
      }
      else
      {
        this.HasReceivableGuildCheckIn = this.GuildTopResponse.has_receivable_guild_checkin;
        GuildUtil.rp = this.GuildTopResponse.rp;
        GuildRaidSettings guildRaidSettings = ((IEnumerable<GuildRaidSettings>) MasterData.GuildRaidSettingsList).FirstOrDefault<GuildRaidSettings>((Func<GuildRaidSettings, bool>) (x => x.key == "RP_BASE_MAX"));
        GuildUtil.rp_max = guildRaidSettings != null ? guildRaidSettings.value : 3;
        Persist.guildHeaderChat.Data.isChatNew = false;
        GuildRegistration guild = this.GuildTopResponse.player_affiliation.guild;
        if (Persist.guildTopLevel.Data.guildID != guild.guild_id)
          this.mRefreshGuildSetting = true;
        if (this.GuildTopResponse.raid_period == null)
          Persist.guildEventCheck.Data.isGuildRaidTransition = false;
        if (!this.GuildTopResponse.player_affiliation.onGvgOperation)
          Persist.guildEventCheck.Data.isGuildBattleTransition = false;
        Future<WebAPI.Response.GuildlogAutoupdate> future = WebAPI.SilentGuildlogAutoupdate("0", (Action<WebAPI.Response.UserError>) (e => { }));
        e1 = future.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (future.Result != null)
        {
          GuildLog[] guildLogs = future.Result.guild_logs;
          List<GuildChatMessageData> source = new List<GuildChatMessageData>();
          for (int index = 0; index < guildLogs.Length; ++index)
          {
            GuildChatMessageData guildChatMessageData = new GuildChatMessageData(guildLogs[index]);
            source.Add(guildChatMessageData);
          }
          if (source.Count > 0)
            Persist.guildHeaderChat.Data.latestLogId = source[0].messageID.ToString();
          this.chatDataList = source.Where<GuildChatMessageData>((Func<GuildChatMessageData, bool>) (data => data.messageType == GuildChatMessageData.GuildChatMessageType.MemberChat || data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerChat)).ToList<GuildChatMessageData>();
          if (this.chatDataList.Count > 0)
          {
            if (!Persist.guildHeaderChat.Data.chatId.Equals(this.chatDataList[0].messageID.ToString()))
              Persist.guildHeaderChat.Data.isChatNew = true;
            Persist.guildHeaderChat.Data.chatId = this.chatDataList[0].messageID.ToString();
          }
        }
        Persist.guildHeaderChat.Flush();
        Persist.guildEventCheck.Flush();
        handler = (Future<WebAPI.Response.GuildTop>) null;
        future = (Future<WebAPI.Response.GuildlogAutoupdate>) null;
      }
    }
    else
    {
      this.GuildTopResponse = (WebAPI.Response.GuildTop) null;
      GuildUtil.resetSettingPersist();
      if (Persist.guildEventCheck.Exists)
      {
        Persist.guildEventCheck.Data.reset();
        Persist.guildEventCheck.Flush();
      }
    }
  }

  public IEnumerator StartSceneAsyncProxyImpl(
    Promise<NGGameDataManager.StartSceneProxyResult> promise)
  {
    NGGameDataManager ngGameDataManager = this;
    while (ngGameDataManager.mIsUpdating)
      yield return (object) null;
    IEnumerator e1;
    while (true)
    {
      DateTime now = DateTime.UtcNow;
      TimeSpan timeSpan = now - ngGameDataManager.updatedTime;
      if (ngGameDataManager.mRefreshHomeHome || ngGameDataManager.isSea && ngGameDataManager.isSeaStartUp || timeSpan.TotalSeconds > (double) ngGameDataManager.webApiUpdateIntervalSeconds)
      {
        ngGameDataManager.mIsUpdating = true;
        if (!PerformanceConfig.GetInstance().IsTuningTitleToHome)
        {
          while (Object.op_Equality((Object) Singleton<TutorialRoot>.GetInstanceOrNull(), (Object) null))
            yield return (object) null;
        }
        TutorialRoot instanceOrNull = Singleton<TutorialRoot>.GetInstanceOrNull();
        bool flag = instanceOrNull != null && instanceOrNull.IsTutorialFinish();
        bool isStartupSequence = ngGameDataManager.isCallHomeUpdateAllData;
        if (flag || WebAPI.LastPlayerBoot.player_is_create)
        {
          if (flag)
            Singleton<TutorialRoot>.GetInstance().ReleaseResources();
          ngGameDataManager.mRefreshGuildTop = true;
          if (ngGameDataManager.isSea)
          {
            if (ngGameDataManager.isCallHomeUpdateAllData)
            {
              // ISSUE: reference to a compiler-generated method
              Future<WebAPI.Response.SeaStartUp2> handler = WebAPI.SeaStartUp2(new Action<WebAPI.Response.UserError>(ngGameDataManager.\u003CStartSceneAsyncProxyImpl\u003Eb__193_0));
              e1 = handler.Wait();
              while (e1.MoveNext())
                yield return e1.Current;
              e1 = (IEnumerator) null;
              if (handler.Result != null)
              {
                ngGameDataManager.loginBonuses = ((IEnumerable<PlayerLoginBonus>) handler.Result.player_loginbonuses).ToList<PlayerLoginBonus>();
                ngGameDataManager.officialInfos = handler.Result.articles;
                ngGameDataManager.officialInfoPopup = handler.Result.officialinfo_popup;
                ngGameDataManager.postStartUp2(handler.Result);
                ngGameDataManager.SortPickupPopups();
                ngGameDataManager.playerLevelRewards = ((IEnumerable<LevelRewardSchemaMixin>) handler.Result.player_achieve_level_rewards).ToList<LevelRewardSchemaMixin>();
                ngGameDataManager.signedInAt = handler.Result.last_signed_in_at;
                ngGameDataManager.gachaLatestStartTime = handler.Result.gacha_latest_start_time;
                ngGameDataManager.limitShopInfos = handler.Result.limit_shop_infos;
                ngGameDataManager.raidMedalShopLatestStartTime = handler.Result.raid_medal_shop_latest_start_time;
                ngGameDataManager.challenge_point = handler.Result.challenge_point;
                ngGameDataManager.receivedFriendRequestCount = handler.Result.received_friend_request_count;
                ngGameDataManager.playbackEventIds = handler.Result.story_playback_event_ids;
                ngGameDataManager.isActiveTotalPaymentBonus = handler.Result.is_active_paymentbonus;
                ngGameDataManager.hasReceivableTotalPaymentBonus = handler.Result.has_receivable_paymentbonus;
                ngGameDataManager.hasFillableLoginbonus = handler.Result.has_fillable_loginbonus;
                ngGameDataManager.favoriteFriends = handler.Result.favorite_friend_list;
                ngGameDataManager.unReadTalkMessage = handler.Result.unread_talk_messages;
                ngGameDataManager.corps_period_ids = handler.Result.corps_period_ids;
                ngGameDataManager.corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) handler.Result.corps_player_unit_ids);
                ngGameDataManager.InitBoostInfo(handler.Result.active_boost_period_id_list, handler.Result.boost_type_id_list);
                ngGameDataManager.SetTablePieceSameCharacterIds(handler.Result.gettable_piece_same_character_ids);
                if (handler.Result.latest_talk_message != null)
                  ngGameDataManager.playerTalkMessage = handler.Result.latest_talk_message;
                if (handler.Result.player_call_letters != null)
                  ngGameDataManager.callLetter = handler.Result.player_call_letters;
                try
                {
                  ngGameDataManager.lastInfoTime = Persist.lastInfoTime.Data.GetLastInfoTime();
                  Persist.lastInfoTime.Data.SetLastInfoTime(ngGameDataManager.signedInAt);
                  Persist.lastInfoTime.Flush();
                }
                catch
                {
                  Persist.lastInfoTime.Delete();
                }
                ngGameDataManager.isSeaStartUp = false;
                ngGameDataManager.isCallHomeUpdateAllData = false;
                handler = (Future<WebAPI.Response.SeaStartUp2>) null;
              }
              else
                break;
            }
            else
            {
              ngGameDataManager.successStartSceneAsyncProxyImpl = false;
              // ISSUE: reference to a compiler-generated method
              Future<WebAPI.Response.SeaStartUp> handler = WebAPI.SeaStartUp(new Action<WebAPI.Response.UserError>(ngGameDataManager.\u003CStartSceneAsyncProxyImpl\u003Eb__193_1));
              e1 = handler.Wait();
              while (e1.MoveNext())
                yield return e1.Current;
              e1 = (IEnumerator) null;
              if (handler.Result != null)
              {
                ngGameDataManager.successStartSceneAsyncProxyImpl = true;
                ngGameDataManager.isSeaStartUp = false;
                if (handler.Result.player_loginbonuses.Length != 0)
                {
                  if (ngGameDataManager.loginBonuses == null)
                    ngGameDataManager.loginBonuses = ((IEnumerable<PlayerLoginBonus>) handler.Result.player_loginbonuses).ToList<PlayerLoginBonus>();
                  else
                    ngGameDataManager.loginBonuses.AddRange((IEnumerable<PlayerLoginBonus>) handler.Result.player_loginbonuses);
                }
                if (handler.Result.player_achieve_level_rewards.Length != 0)
                {
                  if (ngGameDataManager.playerLevelRewards == null)
                    ngGameDataManager.playerLevelRewards = ((IEnumerable<LevelRewardSchemaMixin>) handler.Result.player_achieve_level_rewards).ToList<LevelRewardSchemaMixin>();
                  else
                    ngGameDataManager.playerLevelRewards.AddRange((IEnumerable<LevelRewardSchemaMixin>) handler.Result.player_achieve_level_rewards);
                }
                if (handler.Result.latest_talk_message != null)
                  ngGameDataManager.playerTalkMessage = handler.Result.latest_talk_message;
                if (handler.Result.player_call_letters != null)
                  ngGameDataManager.callLetter = handler.Result.player_call_letters;
                ngGameDataManager.signedInAt = handler.Result.last_signed_in_at;
                ngGameDataManager.SetTablePieceSameCharacterIds(handler.Result.gettable_piece_same_character_ids);
              }
              try
              {
                ngGameDataManager.lastInfoTime = Persist.lastInfoTime.Data.GetLastInfoTime();
                Persist.lastInfoTime.Data.SetLastInfoTime(ngGameDataManager.signedInAt);
                Persist.lastInfoTime.Flush();
              }
              catch
              {
                Persist.lastInfoTime.Delete();
              }
              handler = (Future<WebAPI.Response.SeaStartUp>) null;
            }
          }
          else
          {
            e1 = ngGameDataManager.CallHomeStartup();
            while (e1.MoveNext())
              yield return e1.Current;
            e1 = (IEnumerator) null;
          }
          e1 = OnDemandDownload.WaitLoadHasUnitResource(false, isStartupSequence);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        else
        {
          e1 = WebAPI.TutorialTutorialRagnarokResume().Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        ngGameDataManager.updatedTime = now;
        ngGameDataManager.mRefreshHomeHome = false;
        ngGameDataManager.mIsUpdating = false;
      }
      if (ngGameDataManager.mRefreshGuildTop)
      {
        e1 = ngGameDataManager.UpdateGuildTop();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        ngGameDataManager.mRefreshGuildTop = false;
      }
      if (ngGameDataManager.mRefreshGuildSetting)
      {
        e1 = WebAPI.PlayerHelpers(0, (Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("GameManager Failed Update PlayerHelper : " + e.Code)))).Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GuildUtil.resetSettingPersist();
        ngGameDataManager.mRefreshGuildSetting = false;
      }
      if (ngGameDataManager.mNeedRetry)
      {
        ngGameDataManager.mNeedRetry = false;
        ngGameDataManager.mRefreshHomeHome = true;
        ngGameDataManager.isCallHomeUpdateAllData = true;
      }
      else
        goto label_74;
    }
    e1 = ngGameDataManager.CallHomeStartup();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    ngGameDataManager.mIsUpdating = false;
    yield break;
label_74:
    promise.Result = new NGGameDataManager.StartSceneProxyResult();
    ngGameDataManager.mIsBeforeUpdating = false;
  }

  private void postStartUp2(WebAPI.Response.HomeStartUp2 result)
  {
    if (result == null)
      return;
    this.Set_opened_equip_number_player_unit_ids(result.opened_equip_number_player_unit_ids);
  }

  private void postStartUp2(WebAPI.Response.SeaStartUp2 result)
  {
    this.Set_opened_equip_number_player_unit_ids(result.opened_equip_number_player_unit_ids);
  }

  public void Parse(WebAPI.Response.ShopStatus response)
  {
    if (response == null)
      return;
    this.challenge_point_max = response.challenge_point_max;
    this.challenge_point = response.challenge_point;
  }

  public void Parse(WebAPI.Response.ShopBuy response)
  {
    if (response == null)
      return;
    this.challenge_point = response.challenge_point;
  }

  public void SetFriendRequestCount()
  {
    PlayerFriend[] playerFriendArray = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
    if (playerFriendArray != null)
      this.receivedFriendRequestCount = playerFriendArray.Length;
    else
      this.receivedFriendRequestCount = 0;
  }

  public void SetTablePieceSameCharacterIds(int[] characterIds)
  {
    if (characterIds == null || characterIds.Length == 0)
    {
      this.getTablePieceSameCharacterIds.Clear();
    }
    else
    {
      this.getTablePieceSameCharacterIds.Clear();
      foreach (int characterId in characterIds)
      {
        if (!this.getTablePieceSameCharacterIds.ContainsKey(characterId))
          this.getTablePieceSameCharacterIds.Add(characterId, -1);
      }
    }
  }

  private void InitBoostInfo(int[] periodIdList, int[] typeIdList)
  {
    if (this.m_BoostInfo == null)
      this.m_BoostInfo = new NGGameDataManager.Boost(periodIdList, typeIdList);
    else
      this.m_BoostInfo.Init(periodIdList, typeIdList);
  }

  private void SortPickupPopups()
  {
    List<OfficialInfoPopupSchema> officialInfoPopupSchemaList = new List<OfficialInfoPopupSchema>();
    foreach (IGrouping<int, OfficialInfoPopupSchema> source in (IEnumerable<IGrouping<int, OfficialInfoPopupSchema>>) ((IEnumerable<OfficialInfoPopupSchema>) this.officialInfoPopup.popup_pickups).GroupBy<OfficialInfoPopupSchema, int>((Func<OfficialInfoPopupSchema, int>) (x => x.popup_priority)).OrderBy<IGrouping<int, OfficialInfoPopupSchema>, int>((Func<IGrouping<int, OfficialInfoPopupSchema>, int>) (y => y.Key)))
      officialInfoPopupSchemaList.AddRange((IEnumerable<OfficialInfoPopupSchema>) source.OrderBy<OfficialInfoPopupSchema, Guid>((Func<OfficialInfoPopupSchema, Guid>) (x => Guid.NewGuid())));
    this.officialInfoPopup.popup_pickups = officialInfoPopupSchemaList.ToArray();
  }

  public void StartSceneAsyncProxy(
    Action<NGGameDataManager.StartSceneProxyResult> callback = null)
  {
    new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(this.StartSceneAsyncProxyImpl)).RunOn<NGGameDataManager.StartSceneProxyResult>((MonoBehaviour) this, callback);
  }

  public bool InfoOrLoginBonusJump()
  {
    bool flag = false;
    if (!this.infoThrough)
    {
      this.infoThrough = true;
      try
      {
        foreach (OfficialInformationArticle informationArticle in ((IEnumerable<OfficialInformationArticle>) this.officialInfos).Where<OfficialInformationArticle>((Func<OfficialInformationArticle, bool>) (x => !Persist.infoUnRead.Data.GetUnRead(x))))
        {
          if (this.lastInfoTime < informationArticle.published_at)
          {
            Singleton<NGSceneManager>.GetInstance().changeScene("startup000_12", false, (object) informationArticle);
            flag = true;
            break;
          }
        }
      }
      catch
      {
        Persist.infoUnRead.Delete();
      }
    }
    if (!flag)
    {
      foreach (PlayerLoginBonus loginBonuse in this.loginBonuses)
      {
        if (loginBonuse.loginbonus.draw_type != LoginbonusDrawType.popup)
        {
          Startup00014Scene.changeScene(false);
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public void bootFirstScene(string scene)
  {
    if (!WebAPI.LastPlayerBoot.player_is_create)
    {
      if (!Persist.integralNoaTutorial.Data.startIntegralNoaTutorial)
      {
        Persist.tutorial.Delete();
        Persist.tutorial.Clear();
        Persist.colosseumTutorial.Delete();
        Persist.colosseumTutorial.Clear();
        Persist.integralNoaTutorial.Data.startIntegralNoaTutorial = true;
        Persist.integralNoaTutorial.Flush();
        Persist.newTutorialGacha.Data.setDefault();
        Persist.newTutorialGacha.Flush();
      }
      Future<WebAPI.Response.TutorialTutorialRagnarokResume> future = WebAPI.TutorialTutorialRagnarokResume();
      future.RunOn<WebAPI.Response.TutorialTutorialRagnarokResume>((MonoBehaviour) this, (Action<WebAPI.Response.TutorialTutorialRagnarokResume>) (_ =>
      {
        SMManager.Get<Player>().name = Persist.tutorial.Data.PlayerName;
        Singleton<TutorialRoot>.GetInstance().StartTutorial(future.Result);
      }));
    }
    else
    {
      if (this.checkTutorialGachaState())
        return;
      if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
        Singleton<TutorialRoot>.GetInstance().EndTutorial();
      if (WebAPI.LastPlayerBoot.player_during_battle)
      {
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            Singleton<NGBattleManager>.GetInstance().startBattle((BattleInfo) null, WebAPI.LastPlayerBoot.continue_count);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ => Singleton<NGBattleManager>.GetInstance().startBattle((BattleInfo) null, WebAPI.LastPlayerBoot.continue_count)));
      }
      else if (WebAPI.LastPlayerBoot.player_during_sea_battle)
      {
        this.IsSea = true;
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.SeaStartUp2> handler = WebAPI.SeaStartUp2((Action<WebAPI.Response.UserError>) (e => this.StartCoroutine(PopupUtility.SeaErrorStartUp(e))));
          handler.RunOn<WebAPI.Response.SeaStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.SeaStartUp2>) (_ =>
          {
            if (handler.Result == null)
              return;
            this.postStartUp2(handler.Result);
            this.SetTablePieceSameCharacterIds(handler.Result.gettable_piece_same_character_ids);
            this.isCallHomeUpdateAllData = false;
            Singleton<NGBattleManager>.GetInstance().startBattle((BattleInfo) null, WebAPI.LastPlayerBoot.continue_count);
          }));
        }
        else
        {
          Future<WebAPI.Response.SeaStartUp> handler = WebAPI.SeaStartUp();
          handler.RunOn<WebAPI.Response.SeaStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.SeaStartUp>) (_ =>
          {
            this.SetTablePieceSameCharacterIds(handler.Result.gettable_piece_same_character_ids);
            Singleton<NGBattleManager>.GetInstance().startBattle((BattleInfo) null, WebAPI.LastPlayerBoot.continue_count);
          }));
        }
      }
      else if (WebAPI.LastPlayerBoot.player_during_tower_battle)
      {
        BattleInfo bi = new BattleInfo()
        {
          isResume = true,
          quest_type = CommonQuestType.Tower
        };
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            Singleton<NGBattleManager>.GetInstance().startBattle(bi, WebAPI.LastPlayerBoot.continue_count);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ => Singleton<NGBattleManager>.GetInstance().startBattle(bi, WebAPI.LastPlayerBoot.continue_count)));
      }
      else if (WebAPI.LastPlayerBoot.player_during_corps_battle)
      {
        BattleInfo bi = new BattleInfo()
        {
          isResume = true,
          quest_type = CommonQuestType.Corps
        };
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            Singleton<NGBattleManager>.GetInstance().startBattle(bi, WebAPI.LastPlayerBoot.continue_count);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ => Singleton<NGBattleManager>.GetInstance().startBattle(bi, WebAPI.LastPlayerBoot.continue_count)));
      }
      else if (WebAPI.LastPlayerBoot.player_during_raid_battle)
      {
        BattleInfo bi = new BattleInfo()
        {
          isResume = true,
          quest_type = CommonQuestType.GuildRaid
        };
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            Singleton<NGBattleManager>.GetInstance().startBattle(bi, WebAPI.LastPlayerBoot.continue_count);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ => Singleton<NGBattleManager>.GetInstance().startBattle(bi, WebAPI.LastPlayerBoot.continue_count)));
      }
      else if (WebAPI.LastPlayerBoot.player_during_pvp)
      {
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            BattleInfo battleInfo = new BattleInfo();
            battleInfo.pvp = true;
            battleInfo.pvp_restart = true;
            battleInfo.port = WebAPI.LastPlayerBoot.game_server_port;
            battleInfo.host = WebAPI.LastPlayerBoot.game_server_host;
            battleInfo.battleToken = WebAPI.LastPlayerBoot.pvp_token;
            if (string.IsNullOrEmpty(battleInfo.battleToken))
              battleInfo.pvp_vs_npc = true;
            Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ =>
          {
            BattleInfo battleInfo = new BattleInfo();
            battleInfo.pvp = true;
            battleInfo.pvp_restart = true;
            battleInfo.port = WebAPI.LastPlayerBoot.game_server_port;
            battleInfo.host = WebAPI.LastPlayerBoot.game_server_host;
            battleInfo.battleToken = WebAPI.LastPlayerBoot.pvp_token;
            if (string.IsNullOrEmpty(battleInfo.battleToken))
              battleInfo.pvp_vs_npc = true;
            Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
          }));
      }
      else if (WebAPI.LastPlayerBoot.player_during_pvp_result)
      {
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            Singleton<NGSceneManager>.GetInstance().changeScene("versus026_8", false);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ => Singleton<NGSceneManager>.GetInstance().changeScene("versus026_8", false)));
      }
      else if (WebAPI.LastPlayerBoot.player_during_sea_date)
      {
        Singleton<NGGameDataManager>.GetInstance().IsSea = true;
        Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (data =>
        {
          if (data == null)
            return;
          Sea030HomeScene.ChangeScene(false, true);
        }));
      }
      else if (WebAPI.LastPlayerBoot.during_retry_gacha)
      {
        Singleton<NGSoundManager>.GetInstance().CheckInitialize(true);
        if (this.isCallHomeUpdateAllData)
        {
          Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2();
          handler.RunOn<WebAPI.Response.HomeStartUp2>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp2>) (_ =>
          {
            this.isCallHomeUpdateAllData = false;
            this.postStartUp2(handler.Result);
            Gacha00613Scene.ChangeScene(false, true);
          }));
        }
        else
          WebAPI.HomeStartUp().RunOn<WebAPI.Response.HomeStartUp>((MonoBehaviour) this, (Action<WebAPI.Response.HomeStartUp>) (_ => Gacha00613Scene.ChangeScene(false, true)));
      }
      else
      {
        this.isSea = NGGameDataManager.SeaChangeFlag;
        if (PerformanceConfig.GetInstance().IsTuningTitleToHome)
        {
          if (this.isSea && scene == "mypage")
          {
            Singleton<CommonRoot>.GetInstance().isLoading = true;
            Sea030HomeScene.ChangeScene(false);
          }
          else if (scene == "mypage")
            MypageScene.ChangeScene();
          else
            Singleton<NGSceneManager>.GetInstance().changeScene(scene, false);
        }
        else
          Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (data =>
          {
            if (this.isSea && scene == "mypage")
            {
              Singleton<CommonRoot>.GetInstance().isLoading = true;
              Sea030HomeScene.ChangeScene(false);
            }
            else if (scene == "mypage")
              MypageScene.ChangeScene();
            else
              Singleton<NGSceneManager>.GetInstance().changeScene(scene, false);
          }));
      }
    }
  }

  public void bootFirstSceneBefore()
  {
    WebAPI.Response.PlayerBootRelease lastPlayerBoot = WebAPI.LastPlayerBoot;
    if (!lastPlayerBoot.player_is_create || lastPlayerBoot.player_during_battle || lastPlayerBoot.player_during_sea_battle || lastPlayerBoot.player_during_tower_battle || lastPlayerBoot.player_during_corps_battle || lastPlayerBoot.player_during_raid_battle || lastPlayerBoot.player_during_pvp || lastPlayerBoot.player_during_pvp_result || lastPlayerBoot.player_during_sea_date || lastPlayerBoot.during_retry_gacha)
      return;
    this.mIsBeforeUpdating = true;
    this.isSea = NGGameDataManager.SeaChangeFlag;
    Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy();
  }

  public bool IsTalkBadgeOn()
  {
    string sceneName = Singleton<NGSceneManager>.GetInstance().sceneName;
    return !(sceneName == "mypage") && (!(sceneName == "sea030_home") || this.seaPlayer.Value == null || !this.seaPlayer.Value.is_released_sea_call) && !(sceneName == "sea030_talk_himeList") && !(sceneName == "sea030_talk_himeTalk") && this.unReadTalkMessage && this.player.Value.IsSea();
  }

  public bool isChangeHaveGachaTiket()
  {
    if (this.gachaTicketIDQList == null)
    {
      this.gachaTicketIDQList = ((IEnumerable<PlayerGachaTicket>) this.player.Value.gacha_tickets).Select<PlayerGachaTicket, Tuple<int, int>>((Func<PlayerGachaTicket, Tuple<int, int>>) (x => Tuple.Create<int, int>(x.ticket_id, x.quantity))).ToList<Tuple<int, int>>();
      return true;
    }
    if (!((IEnumerable<PlayerGachaTicket>) this.player.Value.gacha_tickets).Any<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => !this.gachaTicketIDQList.Any<Tuple<int, int>>((Func<Tuple<int, int>, bool>) (tp => tp.Item1 == x.ticket_id && tp.Item2 == x.quantity)))))
      return false;
    this.gachaTicketIDQList = ((IEnumerable<PlayerGachaTicket>) this.player.Value.gacha_tickets).Select<PlayerGachaTicket, Tuple<int, int>>((Func<PlayerGachaTicket, Tuple<int, int>>) (x => Tuple.Create<int, int>(x.ticket_id, x.quantity))).ToList<Tuple<int, int>>();
    return true;
  }

  private bool checkTutorialGachaState()
  {
    if (!Persist.newTutorialGacha.Exists)
    {
      Persist.newTutorialGacha.Data.setDefault();
      Persist.newTutorialGacha.Flush();
    }
    WebAPI.Response.PlayerBootRelease lastPlayerBoot = WebAPI.LastPlayerBoot;
    if (lastPlayerBoot.player_during_battle || lastPlayerBoot.player_during_sea_battle || lastPlayerBoot.player_during_tower_battle || lastPlayerBoot.player_during_corps_battle || lastPlayerBoot.player_during_raid_battle || lastPlayerBoot.player_during_pvp || lastPlayerBoot.player_during_pvp_result || lastPlayerBoot.player_during_sea_date || lastPlayerBoot.during_retry_gacha || Persist.newTutorialGacha.Data.tutorialGacha)
      return false;
    Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (data =>
    {
      if (((IEnumerable<PlayerGachaTicket>) Player.Current.gacha_tickets).Where<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.quantity > 0)).Any<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket_id == 675)) || Singleton<TutorialRoot>.GetInstance().DecryptGachaData())
      {
        Singleton<TutorialRoot>.GetInstance().resumeTutorialForGachaPage();
        SMManager.Get<Player>().name = Persist.tutorial.Data.PlayerName;
        Singleton<TutorialRoot>.GetInstance().StartTutorial((WebAPI.Response.TutorialTutorialRagnarokResume) null);
      }
      else
      {
        Singleton<TutorialRoot>.GetInstance().EndTutorial();
        MypageScene.ChangeScene();
      }
    }));
    return true;
  }

  public string makeKeyPreviewInheritance(int baseUnitId, List<int> materialUnitIds)
  {
    string str = string.Format("{0:X8}-", (object) baseUnitId);
    foreach (int num in (IEnumerable<int>) materialUnitIds.OrderBy<int, int>((Func<int, int>) (i => i)))
      str += num.ToString("x8");
    return str;
  }

  public void clearPreviewInheritance(int baseUnitId = 0)
  {
    if (baseUnitId == 0)
    {
      this.dicPreviewInheritance.Clear();
    }
    else
    {
      string delKey = string.Format("{0:X8}", (object) baseUnitId);
      foreach (string key in this.dicPreviewInheritance.Keys.Where<string>((Func<string, bool>) (s => s.StartsWith(delKey))).ToList<string>())
        this.dicPreviewInheritance.Remove(key);
    }
  }

  public OverkillersSlotRelease.Conditions[] getOverkillersSlotReleaseConditions(
    int same_character_id)
  {
    OverkillersSlotRelease.Conditions[] releaseConditions1;
    if (this.dicOverkillersSlotReleaseConditions.TryGetValue(same_character_id, out releaseConditions1))
      return releaseConditions1;
    OverkillersSlotRelease.Conditions[] releaseConditions2 = OverkillersSlotRelease.find(same_character_id)?.getConditions() ?? new OverkillersSlotRelease.Conditions[0];
    this.dicOverkillersSlotReleaseConditions.Add(same_character_id, releaseConditions2);
    return releaseConditions2;
  }

  public void resetOverkillersSlotReleaseConditions()
  {
    this.dicOverkillersSlotReleaseConditions.Clear();
  }

  public bool setUnitTrainingParam(Ingredients param)
  {
    if (this.dicTrainingParams_ == null)
      this.dicTrainingParams_ = new Dictionary<TrainingType, Ingredients>(Enum.GetValues(typeof (TrainingType)).Length);
    this.currentTraining = param.type;
    int num1 = Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() ? 1 : 0;
    long num2 = num1 != 0 ? SMManager.Revision<PlayerUnit[]>() : 0L;
    int num3;
    if (num1 == 0)
      num3 = 0;
    else if (this.revisionUnitTraining_.HasValue)
    {
      long num4 = num2;
      long? revisionUnitTraining = this.revisionUnitTraining_;
      long valueOrDefault = revisionUnitTraining.GetValueOrDefault();
      num3 = !(num4 == valueOrDefault & revisionUnitTraining.HasValue) ? 1 : 0;
    }
    else
      num3 = 0;
    bool flag = num3 != 0;
    if (num1 != 0 && (flag || this.dicTrainingParams_.Any<KeyValuePair<TrainingType, Ingredients>>((Func<KeyValuePair<TrainingType, Ingredients>, bool>) (db => db.Value != null && db.Value.baseUnit.id != param.baseUnit.id))))
    {
      this.dicTrainingParams_.Clear();
      flag = true;
    }
    if (num1 != 0)
      this.revisionUnitTraining_ = new long?(num2);
    this.dicTrainingParams_[this.currentTraining] = param;
    return flag;
  }

  public TrainingType currentTraining { get; private set; }

  public Ingredients currentTrainingParam => this.getTrainingParam(this.currentTraining);

  public Ingredients getTrainingParam(TrainingType type)
  {
    if (this.dicTrainingParams_ == null)
      return (Ingredients) null;
    Ingredients ingredients;
    return this.dicTrainingParams_.TryGetValue(type, out ingredients) ? ingredients : (Ingredients) null;
  }

  public void resetTrainingParam(bool bParamsOnly = false)
  {
    if (!bParamsOnly)
      this.currentTraining = TrainingType.Combine;
    this.dicTrainingParams_ = (Dictionary<TrainingType, Ingredients>) null;
  }

  public NGGameDataManager.UnitTrainingOption unitTrainingOption
  {
    get
    {
      return this.unitTrainingOption_ ?? (this.unitTrainingOption_ = new NGGameDataManager.UnitTrainingOption());
    }
  }

  public int? getRecommendStrength(CommonQuestType type, int sID)
  {
    Dictionary<int, int> dictionary;
    int num;
    return this.dicRecommendStrength_.TryGetValue(type, out dictionary) && dictionary.TryGetValue(sID, out num) ? new int?(num) : new int?();
  }

  public void setRecommendStrength(CommonQuestType type, int sID, string RecommendStrength)
  {
    Dictionary<int, int> dictionary1;
    if (!this.dicRecommendStrength_.TryGetValue(type, out dictionary1))
    {
      Dictionary<int, int> dictionary2 = new Dictionary<int, int>()
      {
        {
          sID,
          int.Parse(RecommendStrength)
        }
      };
      this.dicRecommendStrength_.Add(type, dictionary2);
    }
    else
    {
      if (dictionary1.ContainsKey(sID))
        return;
      dictionary1.Add(sID, int.Parse(RecommendStrength));
    }
  }

  public void SetSpeedPriorityMode(bool speedMode)
  {
    if (!Persist.speedPriority.Data.IsSpeedPrioritySetup)
    {
      Persist.speedPriority.Data.IsSpeedPrioritySetup = true;
      Persist.speedPriority.Data.IsSpeedPriority = speedMode;
      Persist.speedPriority.Flush();
    }
    else if (Persist.speedPriority.Data.IsSpeedPriority != speedMode)
    {
      Persist.speedPriority.Data.IsSpeedPriority = speedMode;
      Persist.speedPriority.Flush();
    }
    PerformanceConfig.GetInstance().IsSpeedPriority = speedMode;
    ScreenUtil.RefreshPerformanceResolution();
  }

  public void setSceneChangeLog(List<NGSceneManager.SavedSceneLog> log = null)
  {
    this.sceneChangeLog_ = log;
  }

  public List<NGSceneManager.SavedSceneLog> getSceneChangeLog() => this.sceneChangeLog_;

  public void setFuncCreateParams(string sceneName, Func<object[]> createParams)
  {
    if (this.sceneChangeLog_ == null || !this.sceneChangeLog_.Any<NGSceneManager.SavedSceneLog>())
      return;
    foreach (NGSceneManager.SavedSceneLog savedSceneLog in this.sceneChangeLog_)
    {
      if (savedSceneLog.name == sceneName)
      {
        savedSceneLog.createParams = createParams;
        savedSceneLog.clearArgs();
      }
    }
  }

  public int currentGachaNumber { get; set; }

  public bool isReviewPopupCurrentGacha
  {
    get
    {
      GachaModule[] array = SMManager.Get<GachaModule[]>();
      GachaModule gachaModule = array != null ? Array.Find<GachaModule>(array, (Predicate<GachaModule>) (x => x.number == this.currentGachaNumber)) : (GachaModule) null;
      return gachaModule != null && gachaModule.is_review_popup;
    }
  }

  public CorpsUtil corpsUtil => this.corpsUtil_ ?? (this.corpsUtil_ = new CorpsUtil());

  public IEnumerator downloadGachaPickupSelect(
    int gacha_id,
    Action<WebAPI.Response.GachaG301PickupSelectPlayerPickup> callbackResult,
    Action<WebAPI.Response.UserError> actError = null)
  {
    long num1 = SMManager.Revision<Player>();
    long num2 = SMManager.Revision<PlayerUnit[]>();
    long num3 = SMManager.Revision<PlayerItem[]>();
    if (this.pickupSelectStatus_ == null)
      this.pickupSelectStatus_ = new NGGameDataManager.GachaPickupSelectStatus();
    else if (this.pickupSelectStatus_.revPlayer != num1 || this.pickupSelectStatus_.revUnits != num2 || this.pickupSelectStatus_.revItems != num3)
      this.pickupSelectStatus_.dicSelect.Clear();
    this.pickupSelectStatus_.revPlayer = num1;
    this.pickupSelectStatus_.revUnits = num2;
    this.pickupSelectStatus_.revItems = num3;
    WebAPI.Response.GachaG301PickupSelectPlayerPickup selectPlayerPickup;
    if (this.pickupSelectStatus_.dicSelect.TryGetValue(gacha_id, out selectPlayerPickup))
    {
      callbackResult(selectPlayerPickup);
    }
    else
    {
      Future<WebAPI.Response.GachaG301PickupSelectPlayerPickup> wApi = WebAPI.GachaG301PickupSelectPlayerPickup(gacha_id, actError);
      IEnumerator e = wApi.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (wApi.Result != null)
        this.pickupSelectStatus_.dicSelect[gacha_id] = wApi.Result;
      callbackResult(wApi.Result);
    }
  }

  public IEnumerator uploadGachaPickupSelect(
    int gacha_id,
    int[] deck_entity_ids,
    Action<WebAPI.Response.UserError> actError = null)
  {
    Future<WebAPI.Response.GachaG301PickupSelectSavePickup> wApi = WebAPI.GachaG301PickupSelectSavePickup(deck_entity_ids, gacha_id, actError);
    IEnumerator e = wApi.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (wApi.Result != null)
    {
      WebAPI.Response.GachaG301PickupSelectPlayerPickup selectPlayerPickup = this.pickupSelectStatus_.dicSelect[gacha_id];
      this.pickupSelectStatus_.dicSelect[gacha_id] = new WebAPI.Response.GachaG301PickupSelectPlayerPickup()
      {
        pickup_list = selectPlayerPickup.pickup_list,
        player_pickup_deck_entity_ids = wApi.Result.player_pickup_deck_entity_ids
      };
    }
  }

  public PlayerCustomDeck[] customDecks { get; private set; }

  public PlayerCustomDeck currentCustomDeck { get; set; }

  public Dictionary<int, Util.RestoreUnit> restoreUnits { get; private set; }

  public Dictionary<int, Util.RestoreGear> restoreGears { get; private set; }

  public bool isEditCustomDeck
  {
    get => this.isEditCustomDeck_;
    set
    {
      if (this.isEditCustomDeck_ == value)
        return;
      this.isEditCustomDeck_ = value;
      if (value)
      {
        PlayerCustomDeck[] playerCustomDeckArray = SMManager.Get<PlayerCustomDeck[]>();
        PlayerUnit[] units = SMManager.Get<PlayerUnit[]>();
        PlayerItem[] gears = SMManager.Get<PlayerItem[]>();
        this.customDecks = new PlayerCustomDeck[playerCustomDeckArray.Length];
        for (int index = 0; index < this.customDecks.Length; ++index)
          this.customDecks[index] = playerCustomDeckArray[index].cloneWork(units, gears);
        this.restoreUnits = new Dictionary<int, Util.RestoreUnit>();
        this.restoreGears = new Dictionary<int, Util.RestoreGear>();
      }
      else
      {
        this.customDecks = (PlayerCustomDeck[]) null;
        this.currentCustomDeck = (PlayerCustomDeck) null;
        this.restoreUnits = (Dictionary<int, Util.RestoreUnit>) null;
        this.restoreGears = (Dictionary<int, Util.RestoreGear>) null;
      }
    }
  }

  public IEnumerator doFinalizeEditCustomDeck(
    Action<WebAPI.Response.UserError> userErrorCallback = null)
  {
    if (this.isEditCustomDeck)
    {
      Util.restorePlayerUnits(this.restoreUnits);
      Util.restorePlayerGears(this.restoreGears);
      this.isEditCustomDeck = false;
      yield break;
    }
  }

  public enum FromPopup
  {
    None,
    Unit004JobChangeScene,
    Unit0042SceneUnity,
    Unit0042SceneRecommend,
    Unit004Combine,
    Quest002201Scene,
    Unit0042SceneCharacterQuest,
  }

  [Serializable]
  public class TimeCounter
  {
    private Modified<Player> player;
    private Modified<SeaPlayer> seaPlayer;
    private float apElapsedSeconds;
    private int ap_full_remain;
    private float bpElapsedSeconds;
    private int bp_full_remain;
    private float dpElapsedSeconds;
    private int dp_full_remain;

    public float ApRecoverySecondsPerPoint
    {
      get
      {
        return this.player.Value == null || this.player.Value.ap >= this.player.Value.ap_max ? 0.0f : (float) this.player.Value.ap_auto_healing_sec - this.apElapsedSeconds;
      }
    }

    public float ApFullRecoverySeconds
    {
      get
      {
        return this.player.Value == null ? 0.0f : (float) ((this.player.Value.ap_max - this.player.Value.ap) * this.player.Value.ap_auto_healing_sec) - this.apElapsedSeconds;
      }
    }

    public float BpRecoverySecondsPerPoint
    {
      get
      {
        return this.player.Value == null || this.player.Value.bp >= this.player.Value.bp_max ? 0.0f : (float) this.player.Value.bp_auto_healing_sec - this.bpElapsedSeconds;
      }
    }

    public float BpFullRecoverySeconds
    {
      get
      {
        return this.player.Value == null ? 0.0f : (float) ((this.player.Value.bp_max - this.player.Value.bp) * this.player.Value.bp_auto_healing_sec) - this.bpElapsedSeconds;
      }
    }

    public float DpRecoverySecondsPerPoint
    {
      get
      {
        return this.seaPlayer.Value == null || this.seaPlayer.Value.dp >= this.seaPlayer.Value.dp_max ? 0.0f : (float) this.seaPlayer.Value.dp_auto_healing_sec - this.dpElapsedSeconds;
      }
    }

    public float DpFullRecoverySeconds
    {
      get
      {
        return this.seaPlayer.Value == null ? 0.0f : (float) ((this.seaPlayer.Value.dp_max - this.seaPlayer.Value.dp) * this.seaPlayer.Value.dp_auto_healing_sec) - this.dpElapsedSeconds;
      }
    }

    public void Init(Modified<Player> p, Modified<SeaPlayer> s)
    {
      this.player = p;
      this.seaPlayer = s;
    }

    private bool ApDeltaTime(bool isChanged, float delta)
    {
      if (this.player.Value.ap >= this.player.Value.ap_max)
      {
        this.apElapsedSeconds = 0.0f;
        this.ap_full_remain = 0;
        return isChanged;
      }
      if (isChanged && this.ap_full_remain != this.player.Value.ap_full_remain)
      {
        this.ap_full_remain = this.player.Value.ap_full_remain;
        this.apElapsedSeconds = (float) (this.player.Value.ap_auto_healing_sec - this.player.Value.ap_full_remain % this.player.Value.ap_auto_healing_sec);
      }
      else
        isChanged = (int) this.apElapsedSeconds < (int) (this.apElapsedSeconds += delta);
      return isChanged;
    }

    private bool BpDeltaTime(bool isChanged, float delta)
    {
      if (this.player.Value.bp >= this.player.Value.bp_max)
      {
        this.bpElapsedSeconds = 0.0f;
        this.bp_full_remain = 0;
        return isChanged;
      }
      if (isChanged && this.bp_full_remain != this.player.Value.bp_full_remain)
      {
        this.bp_full_remain = this.player.Value.bp_full_remain;
        this.bpElapsedSeconds = (float) (this.player.Value.bp_auto_healing_sec - this.player.Value.bp_full_remain % this.player.Value.bp_auto_healing_sec);
      }
      else
        isChanged = (int) this.bpElapsedSeconds < (int) (this.bpElapsedSeconds += delta);
      return isChanged;
    }

    private bool DpDeltaTime(bool isChanged, float delta)
    {
      if (this.seaPlayer.Value.dp >= this.seaPlayer.Value.dp_max)
      {
        this.dpElapsedSeconds = 0.0f;
        this.dp_full_remain = 0;
        return isChanged;
      }
      if (isChanged && this.dp_full_remain != this.seaPlayer.Value.dp_full_remain)
      {
        this.dp_full_remain = this.seaPlayer.Value.dp_full_remain;
        this.dpElapsedSeconds = (float) Mathf.Max((this.seaPlayer.Value.dp_max - this.seaPlayer.Value.dp) * this.seaPlayer.Value.dp_auto_healing_sec - this.seaPlayer.Value.dp_full_remain, 0);
      }
      else
        isChanged = (int) this.dpElapsedSeconds < (int) (this.dpElapsedSeconds += delta);
      return isChanged;
    }

    private bool ApRecoveryPoint()
    {
      int num = (int) ((double) this.apElapsedSeconds / (double) this.player.Value.ap_auto_healing_sec);
      if (num <= 0)
        return false;
      this.player.Value.ap += num;
      this.apElapsedSeconds -= (float) (num * this.player.Value.ap_auto_healing_sec);
      if (this.player.Value.ap >= this.player.Value.ap_max)
      {
        this.player.Value.ap = this.player.Value.ap_max;
        this.apElapsedSeconds = 0.0f;
      }
      return true;
    }

    private bool BpRecoveryPoint()
    {
      int num = (int) ((double) this.bpElapsedSeconds / (double) this.player.Value.bp_auto_healing_sec);
      if (num <= 0)
        return false;
      this.player.Value.bp += num;
      this.bpElapsedSeconds -= (float) (num * this.player.Value.bp_auto_healing_sec);
      if (this.player.Value.bp >= this.player.Value.bp_max)
      {
        this.player.Value.bp = this.player.Value.bp_max;
        this.bpElapsedSeconds = 0.0f;
      }
      return true;
    }

    private bool DpRecoveryPoint()
    {
      int num = (int) ((double) this.dpElapsedSeconds / (double) this.seaPlayer.Value.dp_auto_healing_sec);
      if (num <= 0)
        return false;
      this.seaPlayer.Value.dp += num;
      this.dpElapsedSeconds -= (float) (num * this.seaPlayer.Value.dp_auto_healing_sec);
      if (this.seaPlayer.Value.dp >= this.seaPlayer.Value.dp_max)
      {
        this.seaPlayer.Value.dp = this.seaPlayer.Value.dp_max;
        this.dpElapsedSeconds = 0.0f;
      }
      return true;
    }

    public bool AddDeltaTime(float delta)
    {
      if (this.player.Value == null)
        return false;
      bool isChanged = this.ApDeltaTime(this.player.IsChangedOnce(), delta);
      bool flag = isChanged | this.BpDeltaTime(isChanged, delta);
      if (this.ApRecoveryPoint() || this.BpRecoveryPoint())
      {
        this.player.NotifyChanged();
        this.player.IsChangedOnce();
      }
      if (this.seaPlayer.Value != null)
      {
        flag |= this.DpDeltaTime(this.seaPlayer.IsChangedOnce(), delta);
        if (this.DpRecoveryPoint())
        {
          this.seaPlayer.NotifyChanged();
          this.seaPlayer.IsChangedOnce();
        }
      }
      return flag;
    }
  }

  public class Boost
  {
    private int[] m_TypeIdList;
    private int[] m_PeriodActiveList;

    public int[] TypeIdList => this.m_TypeIdList;

    public Decimal getDiscountGearDrilling(GearGear gear)
    {
      if (gear == null)
        return this.discountGearDrilling;
      BoostBonusGearDrilling bonusGearDrilling = this.findBonusGearDrilling(gear);
      return bonusGearDrilling == null ? 1.0M : (Decimal) bonusGearDrilling.increase_price;
    }

    private Decimal discountGearDrilling
    {
      get
      {
        BoostBonusGearDrilling bonusGearDrilling = ((IEnumerable<BoostBonusGearDrilling>) MasterData.BoostBonusGearDrillingList).FirstOrDefault<BoostBonusGearDrilling>((Func<BoostBonusGearDrilling, bool>) (fd => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(fd.period_id_BoostPeriod) && fd.isAllTargets));
        return bonusGearDrilling != null ? (Decimal) bonusGearDrilling.increase_price : 1.0M;
      }
    }

    public Decimal getBonusRateGearDrilling(GearGear gear)
    {
      if (gear == null)
        return this.bootRateGearDrilling;
      BoostBonusGearDrilling bonusGearDrilling = this.findBonusGearDrilling(gear);
      return bonusGearDrilling == null ? 1.0M : (Decimal) bonusGearDrilling.boot_rate;
    }

    private Decimal bootRateGearDrilling
    {
      get
      {
        BoostBonusGearDrilling bonusGearDrilling = ((IEnumerable<BoostBonusGearDrilling>) MasterData.BoostBonusGearDrillingList).FirstOrDefault<BoostBonusGearDrilling>((Func<BoostBonusGearDrilling, bool>) (fd => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(fd.period_id_BoostPeriod) && fd.isAllTargets));
        return bonusGearDrilling != null ? (Decimal) bonusGearDrilling.boot_rate : 1.0M;
      }
    }

    public BoostBonusGearDrilling findBonusGearDrilling(GearGear gear)
    {
      BoostBonusGearDrilling bonusGearDrilling1 = (BoostBonusGearDrilling) null;
      foreach (BoostBonusGearDrilling bonusGearDrilling2 in ((IEnumerable<BoostBonusGearDrilling>) MasterData.BoostBonusGearDrillingList).Where<BoostBonusGearDrilling>((Func<BoostBonusGearDrilling, bool>) (x => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(x.period_id_BoostPeriod))))
      {
        if (bonusGearDrilling2.isAllTargets)
        {
          bonusGearDrilling1 = bonusGearDrilling2;
          break;
        }
        if (bonusGearDrilling2.kind_GearKind != 9999)
        {
          if (bonusGearDrilling2.kind_GearKind == gear.kind_GearKind)
          {
            bonusGearDrilling1 = bonusGearDrilling2;
            break;
          }
        }
        else
        {
          int[] gearIds;
          if ((gearIds = bonusGearDrilling2.gearIds) != null && ((IEnumerable<int>) gearIds).Contains<int>(gear.ID))
          {
            bonusGearDrilling1 = bonusGearDrilling2;
            break;
          }
        }
      }
      return bonusGearDrilling1;
    }

    public Decimal DiscountGearCombine
    {
      get
      {
        BoostBonusGearCombine bonusGearCombine = ((IEnumerable<BoostBonusGearCombine>) MasterData.BoostBonusGearCombineList).FirstOrDefault<BoostBonusGearCombine>((Func<BoostBonusGearCombine, bool>) (fd => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(fd.period_id_BoostPeriod)));
        return bonusGearCombine != null ? (Decimal) bonusGearCombine.increase_price : 1.0M;
      }
    }

    public Decimal DiscountUnitBuildup
    {
      get
      {
        BoostBonusUnitBuildup bonusUnitBuildup = ((IEnumerable<BoostBonusUnitBuildup>) MasterData.BoostBonusUnitBuildupList).FirstOrDefault<BoostBonusUnitBuildup>((Func<BoostBonusUnitBuildup, bool>) (fd => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(fd.period_id_BoostPeriod)));
        return bonusUnitBuildup != null ? (Decimal) bonusUnitBuildup.increase_price : 1.0M;
      }
    }

    public Decimal DiscountUnitCompose
    {
      get
      {
        BoostBonusUnitCompose bonusUnitCompose = ((IEnumerable<BoostBonusUnitCompose>) MasterData.BoostBonusUnitComposeList).FirstOrDefault<BoostBonusUnitCompose>((Func<BoostBonusUnitCompose, bool>) (fd => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(fd.period_id_BoostPeriod)));
        return bonusUnitCompose != null ? (Decimal) bonusUnitCompose.increase_price : 1.0M;
      }
    }

    public Decimal DiscountUnitTransmigrate
    {
      get
      {
        BoostBonusUnitTransmigrate unitTransmigrate = ((IEnumerable<BoostBonusUnitTransmigrate>) MasterData.BoostBonusUnitTransmigrateList).FirstOrDefault<BoostBonusUnitTransmigrate>((Func<BoostBonusUnitTransmigrate, bool>) (fd => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(fd.period_id_BoostPeriod)));
        return unitTransmigrate != null ? (Decimal) unitTransmigrate.increase_price : 1.0M;
      }
    }

    public float? XExperience
    {
      get
      {
        return Array.Find<BoostXExperience>(MasterData.BoostXExperienceList, (Predicate<BoostXExperience>) (x => ((IEnumerable<int>) this.m_PeriodActiveList).Contains<int>(x.period_BoostPeriod)))?.scale;
      }
    }

    public Boost(int[] periodActiveList, int[] typeList) => this.Init(periodActiveList, typeList);

    public void Init(int[] periodActiveList, int[] typeList)
    {
      this.m_TypeIdList = typeList;
      this.m_PeriodActiveList = periodActiveList;
    }
  }

  public class StartSceneProxyResult
  {
    public bool IsBreak;
  }

  public class UnitTrainingOption
  {
    public bool isDisabledTab;
  }

  private class GachaPickupSelectStatus
  {
    public long revPlayer;
    public long revUnits;
    public long revItems;
    public Dictionary<int, WebAPI.Response.GachaG301PickupSelectPlayerPickup> dicSelect = new Dictionary<int, WebAPI.Response.GachaG301PickupSelectPlayerPickup>();
  }
}
