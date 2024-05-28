// Decompiled with JetBrains decompiler
// Type: RaidTopMenu
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
public class RaidTopMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject[] dynRaidBaseList;
  [SerializeField]
  private GameObject[] slcRoadOnList;
  [SerializeField]
  private GameObject[] slcRoadOffList;
  [SerializeField]
  private GameObject[] dirRoadAnimList;
  [SerializeField]
  private UIGrid dirRPgrid;
  [SerializeField]
  private GameObject[] slcRPIconBase;
  [SerializeField]
  private GameObject[] slcRPIconOnList;
  [SerializeField]
  private GameObject[] slcRPIconOffList;
  [SerializeField]
  private GameObject dirRPIconBase;
  [SerializeField]
  private GameObject dirLapBase;
  private GameObject lapPrefab;
  [SerializeField]
  private GameObject dirRankingFirstTimePlay;
  [SerializeField]
  private UILabel txtRaidRank_1st;
  [SerializeField]
  private GameObject dirRankingSecondTimePlay;
  [SerializeField]
  private UILabel txtRaidRank;
  [SerializeField]
  private UILabel txtBeforeRank;
  private bool isFirstEvent = true;
  [SerializeField]
  private UISprite sprZoom;
  [SerializeField]
  private UILabel txtDurationValue;
  [SerializeField]
  private GuildStatus guildStatus;
  [SerializeField]
  private GameObject raidShopButton;
  [SerializeField]
  private GameObject slcRaidShopNewBadge;
  [SerializeField]
  private GameObject dirRaidBossDefeat;
  [SerializeField]
  private UILabel dirRaidBossDefeatLabel;
  [SerializeField]
  private GameObject dirRaidAggregating;
  [SerializeField]
  private GameObject dirRaidClose;
  [SerializeField]
  private UI2DSprite sprRaidBanner;
  [SerializeField]
  private UIButton ibtnRaidShop;
  [SerializeField]
  private UIButton ibtnRank;
  [SerializeField]
  private UIButton ibtnHelp;
  [SerializeField]
  private UIButton ibtnHuntingInfo;
  [SerializeField]
  private UIButton ibtnBack;
  [SerializeField]
  private GameObject dynRaidCircling;
  [SerializeField]
  private TweenAlpha dirMapTweenAlpha;
  [SerializeField]
  private GameObject dirRaidHeader;
  [SerializeField]
  private UI2DSprite sprGround;
  private RaidEndlessChallenge endless;
  private bool canEndless;
  private bool endlessEffect;
  private bool playEndlessLapClearEffect;
  private GameObject ui3DModelPrefab;
  private List<UI3DModel> UIModels = new List<UI3DModel>();
  private List<Transform> UIModelTransfroms = new List<Transform>();
  private List<GuildRaid> targetInfoList = new List<GuildRaid>();
  private Color ClearGray = new Color(0.3f, 0.3f, 0.3f);
  public Vector3 UIModelPosOffset = new Vector3(0.0f, 0.3f, 0.0f);
  public Vector3 UIModelRotate = new Vector3(8.5f, -135f, 8.5f);
  public float ModelScaleMag = 1f;
  public int topFloorNum;
  private int currLoopNum = 1;
  private bool startedEndless;
  private int LastPrevRaidID;
  private const int maxBossIndex = 5;
  private GuildRegistration myGuild;
  private GameObject raidBasePrefab;
  private GameObject raidEndlessChallengeWindow;
  private int period_id;
  private int ranking = -1;
  private int rankingPre = -1;
  private int lapNow = 1;
  private int lapMax = 1;
  private int orderNow = 1;
  private int orderMax = 5;
  private bool isComplete;
  private RaidTopMenu.DefeatStateType DefeatState;
  private int defeatCurrentLap = 1;
  private int defeatCurrentOrder = 1;
  private bool isTapSkip;
  public int defeatWaitMSec = 1500;
  public int allClearWaitMSec = 6000;
  public int circlingWaitMSec = 4000;
  public int nextLapSetWaitMSec = 700;
  public int nextCurrentDispWaitMSec = 700;
  private RaidTopMenu.RaidPeriodStatus raidPeriodStatus;
  private bool isDefeatBoss;
  private int defeatEffectNum;
  private WebAPI.Response.GuildraidRaidTopDamage_rewards[] damage_rewards;
  private WebAPI.Response.GuildraidRaidTopDefeat_rewards[] defeat_rewards;
  private bool isRankingResult;
  private int rank_period_id;
  private long raid_total_damage;
  private int damage_rank;
  private WebAPI.Response.GuildraidRaidTopRaid_guild_ranking_rewards[] guild_ranking_rewards;
  private WebAPI.Response.GuildraidRaidTopRaid_guild_ranking_guild_rewards[] guild_ranking_rewardsExtra;
  private int damageRatio;
  private bool fromBattle;
  private GuildRaidUnitWanted currentWanted;
  [SerializeField]
  private GameObject cloudAnimParent;
  private GameObject cloudPrefab;
  private bool isCloud;
  private bool isExtraEffect;
  private GuildRaidPeriod periodData;
  private GuildRaidTopLapInfo lapInfo;

  public bool isFailedInit { get; private set; }

  public GuildRegistration MyGuild => this.myGuild;

  private IEnumerator CallGuildraidRaidTopAPI()
  {
    RaidTopMenu raidTopMenu = this;
    int lastPeriodId = Persist.guildRaidProgress.Data.lastPeriodId;
    int lastLap = Persist.guildRaidProgress.Data.lastLap;
    int lastOrder = Persist.guildRaidProgress.Data.lastOrder;
    Future<WebAPI.Response.GuildraidRaidTop> guildraidRaidTop = WebAPI.GuildraidRaidTop(Persist.guildRaidProgress.Data.isLastPeriodComplete, lastLap, lastOrder, lastPeriodId);
    IEnumerator e = guildraidRaidTop.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (guildraidRaidTop.Result == null)
    {
      raidTopMenu.isFailedInit = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      if (guildraidRaidTop.Result.raid_period != null)
      {
        raidTopMenu.period_id = guildraidRaidTop.Result.raid_period.id;
        if (!Persist.guildRaidProgress.Data.isStartedEndless && guildraidRaidTop.Result.is_started_endless)
          raidTopMenu.isExtraEffect = true;
        Persist.guildRaidProgress.Data.isStartedEndless = guildraidRaidTop.Result.is_started_endless;
        Persist.guildRaidProgress.Flush();
        raidTopMenu.startedEndless = guildraidRaidTop.Result.is_started_endless;
        raidTopMenu.canEndless = guildraidRaidTop.Result.raid_period.is_endless;
        raidTopMenu.raidPeriodStatus = RaidTopMenu.RaidPeriodStatus.ENABLE;
      }
      else if (guildraidRaidTop.Result.raid_aggregating)
      {
        DateTime now = ServerTime.NowAppTimeAddDelta();
        GuildRaidPeriod guildRaidPeriod = ((IEnumerable<GuildRaidPeriod>) MasterData.GuildRaidPeriodList).Where<GuildRaidPeriod>((Func<GuildRaidPeriod, bool>) (x =>
        {
          DateTime? startAt = x.start_at;
          DateTime dateTime = now;
          return startAt.HasValue && startAt.GetValueOrDefault() <= dateTime;
        })).OrderByDescending<GuildRaidPeriod, DateTime?>((Func<GuildRaidPeriod, DateTime?>) (x => x.start_at)).FirstOrDefault<GuildRaidPeriod>();
        if (guildRaidPeriod != null)
          raidTopMenu.period_id = guildRaidPeriod.ID;
        raidTopMenu.raidPeriodStatus = RaidTopMenu.RaidPeriodStatus.AGGREGATING;
      }
      else
      {
        DateTime now = ServerTime.NowAppTimeAddDelta();
        GuildRaidPeriod guildRaidPeriod = ((IEnumerable<GuildRaidPeriod>) MasterData.GuildRaidPeriodList).Where<GuildRaidPeriod>((Func<GuildRaidPeriod, bool>) (x =>
        {
          DateTime? startAt = x.start_at;
          DateTime dateTime = now;
          return startAt.HasValue && startAt.GetValueOrDefault() <= dateTime;
        })).OrderByDescending<GuildRaidPeriod, DateTime?>((Func<GuildRaidPeriod, DateTime?>) (x => x.start_at)).FirstOrDefault<GuildRaidPeriod>();
        if (guildRaidPeriod != null)
          raidTopMenu.period_id = guildRaidPeriod.ID;
        raidTopMenu.raidPeriodStatus = RaidTopMenu.RaidPeriodStatus.OVER;
      }
      if (guildraidRaidTop.Result.raid_period != null && Persist.guildRaidProgress.Data.lastPeriodId != guildraidRaidTop.Result.raid_period.id)
        Persist.guildRaidProgress.Data.endlessBossAnime.Clear();
      raidTopMenu.isRankingResult = false;
      int? nullable;
      if (guildraidRaidTop.Result.raid_rank_period_id.HasValue && guildraidRaidTop.Result.raid_damage_rank.HasValue)
      {
        nullable = guildraidRaidTop.Result.raid_damage_rank;
        int num = 1;
        if (nullable.GetValueOrDefault() >= num & nullable.HasValue)
        {
          raidTopMenu.rank_period_id = guildraidRaidTop.Result.raid_rank_period_id.Value;
          if (guildraidRaidTop.Result.raid_total_damage.HasValue)
            raidTopMenu.raid_total_damage = guildraidRaidTop.Result.raid_total_damage.Value;
          raidTopMenu.damage_rank = guildraidRaidTop.Result.raid_damage_rank.Value;
          raidTopMenu.guild_ranking_rewards = guildraidRaidTop.Result.raid_guild_ranking_rewards;
          raidTopMenu.guild_ranking_rewardsExtra = guildraidRaidTop.Result.raid_guild_ranking_guild_rewards;
          raidTopMenu.isRankingResult = true;
        }
      }
      if (guildraidRaidTop.Result.current_ranking != null && guildraidRaidTop.Result.current_ranking.damage_rank.HasValue && raidTopMenu.raidPeriodStatus != RaidTopMenu.RaidPeriodStatus.AGGREGATING)
        raidTopMenu.ranking = guildraidRaidTop.Result.current_ranking.damage_rank.Value;
      if (guildraidRaidTop.Result.previous_ranking != null && guildraidRaidTop.Result.previous_ranking.damage_rank.HasValue)
        raidTopMenu.rankingPre = guildraidRaidTop.Result.previous_ranking.damage_rank.Value;
      if (raidTopMenu.period_id != 0)
      {
        GuildRaidPeriod guildRaidPeriod = ((IEnumerable<GuildRaidPeriod>) MasterData.GuildRaidPeriodList).OrderBy<GuildRaidPeriod, DateTime?>((Func<GuildRaidPeriod, DateTime?>) (x => x.start_at)).FirstOrDefault<GuildRaidPeriod>();
        if (guildRaidPeriod != null)
          raidTopMenu.isFirstEvent = guildRaidPeriod.ID == raidTopMenu.period_id;
      }
      // ISSUE: reference to a compiler-generated method
      KeyValuePair<int, GuildRaid> keyValuePair = MasterData.GuildRaid.Where<KeyValuePair<int, GuildRaid>>(new Func<KeyValuePair<int, GuildRaid>, bool>(raidTopMenu.\u003CCallGuildraidRaidTopAPI\u003Eb__101_0)).OrderByDescending<KeyValuePair<int, GuildRaid>, int>((Func<KeyValuePair<int, GuildRaid>, int>) (x => x.Value.lap)).FirstOrDefault<KeyValuePair<int, GuildRaid>>();
      if (keyValuePair.Value != null)
        raidTopMenu.topFloorNum = keyValuePair.Value.lap;
      if (guildraidRaidTop.Result.loop_count.HasValue)
      {
        raidTopMenu.currLoopNum = guildraidRaidTop.Result.loop_count.Value;
        raidTopMenu.lapNow = raidTopMenu.currLoopNum < raidTopMenu.topFloorNum ? raidTopMenu.currLoopNum : raidTopMenu.topFloorNum;
      }
      if (guildraidRaidTop.Result.order.HasValue)
      {
        if (guildraidRaidTop.Result.order.Value == 1 && guildraidRaidTop.Result.is_started_endless)
        {
          int? loopCount = guildraidRaidTop.Result.loop_count;
          int topFloorNum = raidTopMenu.topFloorNum;
          nullable = loopCount.HasValue ? new int?(loopCount.GetValueOrDefault() - topFloorNum) : new int?();
          int num = 2;
          if (nullable.GetValueOrDefault() >= num & nullable.HasValue && Persist.guildRaidProgress.Data.orderNow != 1)
            raidTopMenu.playEndlessLapClearEffect = true;
        }
        Persist.guildRaidProgress.Data.orderNow = guildraidRaidTop.Result.order.Value;
        raidTopMenu.orderNow = guildraidRaidTop.Result.order.Value;
      }
      raidTopMenu.isComplete = !raidTopMenu.startedEndless && guildraidRaidTop.Result.is_complete;
      raidTopMenu.damageRatio = guildraidRaidTop.Result.boss_damage_ratio;
      raidTopMenu.defeatEffectNum = raidTopMenu.lapNow != 1 || raidTopMenu.orderNow != 1 ? guildraidRaidTop.Result.recent_quest_ids.Length : 0;
      raidTopMenu.damage_rewards = ((IEnumerable<WebAPI.Response.GuildraidRaidTopDamage_rewards>) guildraidRaidTop.Result.damage_rewards).Where<WebAPI.Response.GuildraidRaidTopDamage_rewards>((Func<WebAPI.Response.GuildraidRaidTopDamage_rewards, bool>) (x => x.rewards.Length != 0)).ToArray<WebAPI.Response.GuildraidRaidTopDamage_rewards>();
      raidTopMenu.defeat_rewards = guildraidRaidTop.Result.defeat_rewards;
      raidTopMenu.myGuild = guildraidRaidTop.Result.player_affiliation.guild;
      PlayerAffiliation.Current.guild = raidTopMenu.myGuild;
      GuildUtil.rp = guildraidRaidTop.Result.rp;
      GuildUtil.rp_max = ((IEnumerable<GuildRaidSettings>) MasterData.GuildRaidSettingsList).FirstOrDefault<GuildRaidSettings>((Func<GuildRaidSettings, bool>) (x => x.key == "RP_BASE_MAX")).value;
    }
  }

  public IEnumerator InitializeAsync(bool fromBattle = false)
  {
    RaidTopMenu raidTopMenu = this;
    raidTopMenu.IsPush = false;
    raidTopMenu.isFailedInit = false;
    raidTopMenu.fromBattle = fromBattle;
    if (Persist.guildRaidProgress.Data.endlessBossAnime == null)
      Persist.guildRaidProgress.Data.endlessBossAnime = new Dictionary<int, bool>();
    IEnumerator e = raidTopMenu.CallGuildraidRaidTopAPI();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!raidTopMenu.isFailedInit)
    {
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (raidTopMenu.period_id != 0)
        raidTopMenu.periodData = MasterData.GuildRaidPeriod[raidTopMenu.period_id];
      if (raidTopMenu.period_id == 0 || raidTopMenu.periodData == null)
        raidTopMenu.raidPeriodStatus = RaidTopMenu.RaidPeriodStatus.OVER;
      if (raidTopMenu.orderNow == 5 && !Persist.guildRaidProgress.Data.endlessBossAnime.ContainsKey(raidTopMenu.currLoopNum))
        Persist.guildRaidProgress.Data.endlessBossAnime.Add(raidTopMenu.currLoopNum, true);
      raidTopMenu.isDefeatBoss = false;
      if (raidTopMenu.defeatEffectNum >= 1 && raidTopMenu.raidPeriodStatus == RaidTopMenu.RaidPeriodStatus.ENABLE)
        raidTopMenu.isDefeatBoss = true;
      if (raidTopMenu.orderNow == 5 && raidTopMenu.startedEndless && Persist.guildRaidProgress.Data.lastOrder + 1 == 5 && Persist.guildRaidProgress.Data.endlessBossAnime.ContainsKey(raidTopMenu.currLoopNum) && !Persist.guildRaidProgress.Data.endlessBossAnime[raidTopMenu.currLoopNum])
        raidTopMenu.isDefeatBoss = false;
      if (Persist.guildRaidProgress.Data.endlessBossAnime.ContainsKey(raidTopMenu.currLoopNum))
        Persist.guildRaidProgress.Data.endlessBossAnime[raidTopMenu.currLoopNum] = false;
      Persist.guildRaidProgress.Flush();
      e = raidTopMenu.guildStatus.ResourceLoad(raidTopMenu.MyGuild);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      raidTopMenu.guildStatus.SetStatus(raidTopMenu.MyGuild);
      if (raidTopMenu.isFirstEvent)
      {
        raidTopMenu.dirRankingFirstTimePlay.SetActive(true);
        raidTopMenu.dirRankingSecondTimePlay.SetActive(false);
        if (raidTopMenu.ranking == -1)
          raidTopMenu.txtRaidRank_1st.SetTextLocalize("---");
        else
          raidTopMenu.txtRaidRank_1st.SetTextLocalize(raidTopMenu.ranking);
      }
      else
      {
        raidTopMenu.dirRankingFirstTimePlay.SetActive(false);
        raidTopMenu.dirRankingSecondTimePlay.SetActive(true);
        if (raidTopMenu.ranking == -1)
          raidTopMenu.txtRaidRank.SetTextLocalize("---");
        else
          raidTopMenu.txtRaidRank.SetTextLocalize(raidTopMenu.ranking);
        if (raidTopMenu.rankingPre == -1)
          raidTopMenu.txtBeforeRank.SetTextLocalize("---");
        else
          raidTopMenu.txtBeforeRank.SetTextLocalize(raidTopMenu.rankingPre);
      }
      Future<Sprite> spriteF;
      if (raidTopMenu.periodData != null && raidTopMenu.periodData.bg_path != null)
      {
        string bgPath = raidTopMenu.periodData.bg_path;
        spriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(bgPath);
        e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.sprGround.sprite2D = spriteF.Result;
        spriteF = (Future<Sprite>) null;
      }
      Future<GameObject> loader;
      if (Object.op_Equality((Object) raidTopMenu.raidBasePrefab, (Object) null))
      {
        loader = new ResourceObject("Prefabs/raid032_top/RaidBase").Load<GameObject>();
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.raidBasePrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) raidTopMenu.lapPrefab, (Object) null))
      {
        loader = new ResourceObject("Prefabs/raid032_top/dir_Raid_LAP_info").Load<GameObject>();
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.lapPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) raidTopMenu.cloudPrefab, (Object) null))
      {
        loader = Res.Prefabs.guild028_2.cloud.Load<GameObject>();
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.cloudPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) raidTopMenu.raidEndlessChallengeWindow, (Object) null))
      {
        loader = new ResourceObject("Prefabs/raid032_top/popup_ExtraChallenge").Load<GameObject>();
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.raidEndlessChallengeWindow = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (!raidTopMenu.isCloud)
      {
        raidTopMenu.CloudCreater(raidTopMenu.cloudAnimParent.transform);
        raidTopMenu.isCloud = true;
      }
      ((IEnumerable<GameObject>) raidTopMenu.slcRPIconBase).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
      for (int index = 0; index < GuildUtil.rp_max && index < raidTopMenu.slcRPIconBase.Length; ++index)
        raidTopMenu.slcRPIconBase[index].gameObject.SetActive(true);
      raidTopMenu.dirRPgrid.Reposition();
      for (int index = 0; index < raidTopMenu.slcRPIconOnList.Length; ++index)
        raidTopMenu.slcRPIconOnList[index].SetActive(index < GuildUtil.rp);
      for (int index = 0; index < raidTopMenu.slcRPIconOffList.Length; ++index)
        raidTopMenu.slcRPIconOffList[index].SetActive(index >= GuildUtil.rp);
      if (Object.op_Equality((Object) raidTopMenu.lapInfo, (Object) null))
      {
        // ISSUE: reference to a compiler-generated method
        KeyValuePair<int, GuildRaid> keyValuePair = MasterData.GuildRaid.Where<KeyValuePair<int, GuildRaid>>(new Func<KeyValuePair<int, GuildRaid>, bool>(raidTopMenu.\u003CInitializeAsync\u003Eb__102_1)).OrderByDescending<KeyValuePair<int, GuildRaid>, int>((Func<KeyValuePair<int, GuildRaid>, int>) (x => x.Value.lap)).FirstOrDefault<KeyValuePair<int, GuildRaid>>();
        if (keyValuePair.Value != null)
          raidTopMenu.lapMax = keyValuePair.Value.lap;
        raidTopMenu.dirLapBase.transform.Clear();
        raidTopMenu.lapInfo = raidTopMenu.lapPrefab.Clone(raidTopMenu.dirLapBase.transform).GetComponent<GuildRaidTopLapInfo>();
      }
      raidTopMenu.lapInfo.setLapNum(raidTopMenu.currLoopNum, raidTopMenu.lapMax);
      DateTime dateTime1 = ServerTime.NowAppTimeAddDelta();
      DateTime? nullable;
      if (Singleton<NGGameDataManager>.GetInstance().raidMedalShopLatestStartTime.HasValue)
      {
        GameObject raidShopNewBadge = raidTopMenu.slcRaidShopNewBadge;
        nullable = Singleton<NGGameDataManager>.GetInstance().raidMedalShopLatestStartTime;
        DateTime dateTime2 = dateTime1;
        int num = nullable.HasValue ? (nullable.GetValueOrDefault() <= dateTime2 ? 1 : 0) : 0;
        raidShopNewBadge.SetActive(num != 0);
      }
      else
        raidTopMenu.slcRaidShopNewBadge.SetActive(false);
      ShopShop shopShop;
      if (MasterData.ShopShop.TryGetValue(8000, out shopShop))
      {
        DateTime dateTime3 = dateTime1;
        nullable = shopShop.start_at;
        if ((nullable.HasValue ? (dateTime3 >= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        {
          raidTopMenu.raidShopButton.SetActive(true);
          goto label_82;
        }
      }
      raidTopMenu.raidShopButton.SetActive(false);
label_82:
      if (raidTopMenu.periodData != null)
      {
        string path = "Prefabs/Banners/RaidBanner/{0}/RaidHeader".F((object) raidTopMenu.periodData.banner_id);
        spriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
        e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.sprRaidBanner.sprite2D = spriteF.Result;
        if (raidTopMenu.raidPeriodStatus == RaidTopMenu.RaidPeriodStatus.OVER)
        {
          raidTopMenu.txtDurationValue.SetTextLocalize(Consts.GetInstance().GUILD_RAID_PERIOD_OVER);
        }
        else
        {
          string raidPeriodFormat = Consts.GetInstance().GUILD_RAID_PERIOD_FORMAT;
          object[] objArray = new object[4]
          {
            (object) raidTopMenu.periodData.start_at.Value.Month,
            null,
            null,
            null
          };
          DateTime dateTime4 = raidTopMenu.periodData.start_at.Value;
          objArray[1] = (object) dateTime4.Day;
          dateTime4 = raidTopMenu.periodData.end_at.Value;
          objArray[2] = (object) dateTime4.Month;
          dateTime4 = raidTopMenu.periodData.end_at.Value;
          objArray[3] = (object) dateTime4.Day;
          string text = raidPeriodFormat.F(objArray);
          raidTopMenu.txtDurationValue.SetTextLocalize(text);
        }
        raidTopMenu.dirRaidHeader.SetActive(true);
        spriteF = (Future<Sprite>) null;
      }
      else
        raidTopMenu.dirRaidHeader.SetActive(false);
      raidTopMenu.setObjectsByPeriodStatus();
      raidTopMenu.dirRPIconBase.SetActive(raidTopMenu.raidPeriodStatus != RaidTopMenu.RaidPeriodStatus.OVER);
      raidTopMenu.dirLapBase.SetActive(raidTopMenu.raidPeriodStatus != RaidTopMenu.RaidPeriodStatus.OVER);
      e = raidTopMenu.setWantedAndRoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void CloudCreater(Transform trans)
  {
    if (((Object) trans).name == "cloud_anim_pos")
      this.cloudPrefab.Clone(trans);
    foreach (Transform child in trans.GetChildren())
      this.CloudCreater(child);
  }

  public void OnDisable() => this.Clear3DModel();

  private void Clear3DModel()
  {
    foreach (UI3DModel uiModel in this.UIModels)
    {
      if (Object.op_Inequality((Object) uiModel, (Object) null))
      {
        if (Object.op_Inequality((Object) uiModel.model_creater_, (Object) null))
          uiModel.DestroyModelCamera();
        if (Object.op_Inequality((Object) ((Component) uiModel).gameObject, (Object) null) && ((Object) ((Component) uiModel).gameObject).name == "slc_3DModel(Clone)")
          Object.Destroy((Object) ((Component) uiModel).gameObject);
      }
    }
    this.UIModels.Clear();
    this.UIModelTransfroms.Clear();
  }

  public IEnumerator SetCharacterModel()
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.ui3DModelPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ui3DModelPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    int cnt = this.UIModelTransfroms.Count<Transform>();
    for (int i = 0; i < cnt; ++i)
    {
      this.UIModels.Add((UI3DModel) null);
      GuildRaid targetInfo = this.targetInfoList[i];
      if (targetInfo != null)
      {
        yield return (object) MasterData.LoadBattleStageEnemy(MasterData.BattleStage[targetInfo.stage_id]);
        int id = targetInfo.getBoss().unit.ID;
        Transform uiModelTransfrom = this.UIModelTransfroms[i];
        if (!Object.op_Equality((Object) uiModelTransfrom, (Object) null))
        {
          UI3DModel UIModel = this.ui3DModelPrefab.Clone(uiModelTransfrom).GetComponent<UI3DModel>();
          e = UIModel.HistoryUnit(MasterData.UnitUnit[id]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          UIModel.isNotLotate = true;
          Vector3 vector3;
          // ISSUE: explicit constructor call
          ((Vector3) ref vector3).\u002Ector(targetInfo.boss_model_offset_x, targetInfo.boss_model_offset_y, targetInfo.boss_model_offset_z);
          UIModel.ModelTarget.localPosition = Vector3.op_Addition(Vector3.op_Addition(UIModel.ModelTarget.localPosition, this.UIModelPosOffset), vector3);
          UIModel.ModelTarget.localEulerAngles = this.UIModelRotate;
          float num = targetInfo.boss_model_scale * this.ModelScaleMag;
          UIModel.setModelScale(new Vector3(num, num, num));
          ((Collider) ((Component) UIModel).GetComponent<BoxCollider>()).enabled = false;
          this.UIModels[i] = UIModel;
          ((Component) UIModel.ModelCamera).transform.localPosition = new Vector3(0.0f, (float) (i * 1000), 0.0f);
          UIModel.ModelCamera.fieldOfView = 35f;
          ((Component) UIModel).gameObject.SetActive(false);
          targetInfo = (GuildRaid) null;
          UIModel = (UI3DModel) null;
        }
      }
    }
    foreach (Component uiModel in this.UIModels)
      uiModel.gameObject.SetActive(true);
  }

  private int getOrderMax(int lap)
  {
    return MasterData.GuildRaid.Count<KeyValuePair<int, GuildRaid>>((Func<KeyValuePair<int, GuildRaid>, bool>) (x => x.Value.period_id == this.period_id && x.Value.lap == lap));
  }

  private IEnumerator setWantedAndRoad(bool enableBossTouch = true)
  {
    RaidTopMenu raidTopMenu = this;
    int lap = raidTopMenu.lapNow;
    int order = raidTopMenu.orderNow;
    if (raidTopMenu.isDefeatBoss)
    {
      if (raidTopMenu.orderNow == 1 && !raidTopMenu.isComplete && !raidTopMenu.startedEndless)
      {
        lap = raidTopMenu.lapNow - 1;
        order = Mathf.Max(raidTopMenu.getOrderMax(lap) - raidTopMenu.defeatEffectNum + 1, 1);
        raidTopMenu.lapInfo.setLapNum(raidTopMenu.currLoopNum, raidTopMenu.lapMax);
      }
      else
      {
        if (raidTopMenu.isComplete)
          --raidTopMenu.defeatEffectNum;
        lap = raidTopMenu.lapNow;
        order = Mathf.Max(raidTopMenu.orderNow - raidTopMenu.defeatEffectNum, 1);
      }
      if (Persist.guildRaidProgress.Data.lastPeriodId == raidTopMenu.period_id && Persist.guildRaidProgress.Data.lastLap >= lap)
      {
        lap = Persist.guildRaidProgress.Data.lastLap;
        order = Persist.guildRaidProgress.Data.lastOrder + 1;
        int orderMax = raidTopMenu.getOrderMax(lap);
        if (orderMax < order && lap <= raidTopMenu.lapMax - 1 && !raidTopMenu.isComplete)
        {
          lap++;
          order = 1;
        }
        order = !raidTopMenu.startedEndless || order < orderMax ? Mathf.Min(order, orderMax) : 1;
      }
      raidTopMenu.defeatCurrentLap = lap;
      raidTopMenu.defeatCurrentOrder = order;
      if (raidTopMenu.defeatCurrentLap == raidTopMenu.lapNow && raidTopMenu.defeatCurrentOrder == raidTopMenu.orderNow && (!raidTopMenu.isComplete || Persist.guildRaidProgress.Data.lastPeriodId == raidTopMenu.period_id && Persist.guildRaidProgress.Data.isLastPeriodComplete == raidTopMenu.isComplete))
      {
        raidTopMenu.lapInfo.setLapNum(raidTopMenu.currLoopNum, raidTopMenu.lapMax);
        raidTopMenu.defeatEffectNum = 0;
        raidTopMenu.isDefeatBoss = false;
      }
    }
    raidTopMenu.Clear3DModel();
    raidTopMenu.targetInfoList.Clear();
    ((IEnumerable<GameObject>) raidTopMenu.slcRoadOnList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    ((IEnumerable<GameObject>) raidTopMenu.slcRoadOffList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    ((IEnumerable<GameObject>) raidTopMenu.dirRoadAnimList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    ((IEnumerable<GameObject>) raidTopMenu.dynRaidBaseList).ForEach<GameObject>((Action<GameObject>) (x => x.transform.Clear()));
    ((IEnumerable<GameObject>) raidTopMenu.dynRaidBaseList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    IEnumerator e;
    if (raidTopMenu.raidPeriodStatus == RaidTopMenu.RaidPeriodStatus.ENABLE)
    {
      raidTopMenu.orderMax = raidTopMenu.getOrderMax(lap);
      int i;
      for (i = 0; i < order - 1; ++i)
        raidTopMenu.slcRoadOnList[i].SetActive(true);
      for (; i < raidTopMenu.orderMax - 1; ++i)
        raidTopMenu.slcRoadOffList[i].SetActive(true);
      for (i = 0; i <= raidTopMenu.orderMax - 1; ++i)
      {
        raidTopMenu.dynRaidBaseList[i].SetActive(true);
        GameObject dynRaidBase = raidTopMenu.dynRaidBaseList[i];
        GuildRaidUnitWanted unitWanted = raidTopMenu.raidBasePrefab.Clone(dynRaidBase.transform).GetComponentInChildren<GuildRaidUnitWanted>();
        KeyValuePair<int, GuildRaid> targetInfo = MasterData.GuildRaid.FirstOrDefault<KeyValuePair<int, GuildRaid>>((Func<KeyValuePair<int, GuildRaid>, bool>) (x => x.Value.period_id == this.period_id && x.Value.lap == lap && x.Value.order == i + 1));
        e = unitWanted.Init(targetInfo.Value, new Action(raidTopMenu.ShowTargetDetail));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        raidTopMenu.UIModelTransfroms.Add(unitWanted.DynRaidBaseBoss.transform);
        raidTopMenu.targetInfoList.Add(targetInfo.Value);
        if (i < order - 1)
          unitWanted.setClear();
        else if (i == order - 1)
        {
          raidTopMenu.currentWanted = unitWanted;
          unitWanted.setCurrent();
          if (raidTopMenu.isComplete && !raidTopMenu.isDefeatBoss)
            unitWanted.setClear();
        }
        if (raidTopMenu.isDefeatBoss || !enableBossTouch)
          unitWanted.setButtonEnabled(false);
        unitWanted.setHpGaugeEnable(false);
        unitWanted = (GuildRaidUnitWanted) null;
        targetInfo = new KeyValuePair<int, GuildRaid>();
      }
    }
    e = raidTopMenu.SetCharacterModel();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    for (int index = 0; index <= raidTopMenu.orderMax - 1; ++index)
    {
      GuildRaidUnitWanted componentInChildren = raidTopMenu.dynRaidBaseList[index].GetComponentInChildren<GuildRaidUnitWanted>();
      if (!Object.op_Equality((Object) componentInChildren, (Object) null))
      {
        componentInChildren.setUnlockCircle(true);
        if (index == order - 1)
        {
          if (raidTopMenu.isDefeatBoss)
            componentInChildren.setHpGaugeEnable(true);
          else
            componentInChildren.setHpGaugeEnable(true, raidTopMenu.damageRatio);
        }
        else
          componentInChildren.setHpGaugeEnable(false);
        if (index < order - 1 || raidTopMenu.isComplete && !raidTopMenu.isDefeatBoss)
        {
          raidTopMenu.UIModels[index].AnimRaidMapDefeat();
          componentInChildren.setHpGaugeEnable(false);
          TweenColor component = ((Component) raidTopMenu.UIModels[index]).GetComponent<TweenColor>();
          component.from = raidTopMenu.ClearGray;
          component.to = raidTopMenu.ClearGray;
          ((UITweener) component).duration = 0.0f;
          ((UITweener) component).PlayForward();
        }
        else if (index != order - 1)
        {
          componentInChildren.setUnlockCircle(false);
          TweenColor component = ((Component) raidTopMenu.UIModels[index]).GetComponent<TweenColor>();
          component.from = Color.gray;
          component.to = Color.gray;
          ((UITweener) component).duration = 0.0f;
          ((UITweener) component).PlayForward();
        }
      }
    }
  }

  public IEnumerator playSceneStartEffect()
  {
    Future<GameObject> loader2;
    IEnumerator e;
    if (!this.isDefeatBoss && !this.isRankingResult && this.damage_rewards.Length == 0)
    {
      this.setObjectsByPeriodStatus();
      if (!this.isExtraEffect)
      {
        if (!this.startedEndless || !this.playEndlessLapClearEffect)
        {
          yield break;
        }
        else
        {
          this.setButtonEnabled(false);
          this.playEndlessLapClearEffect = false;
          loader2 = new ResourceObject("Prefabs/raid032_top/dir_Raid_ExtraChallenge_LAP_anim").Load<GameObject>();
          e = loader2.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Singleton<PopupManager>.GetInstance().open(loader2.Result).GetComponent<RaidExtraChallengeLAPEffect>().SetNum(this.currLoopNum - this.lapMax);
          yield return (object) new WaitForSeconds((float) this.circlingWaitMSec / 2000f);
          Singleton<PopupManager>.GetInstance().dismiss();
          this.setButtonEnabled(true);
          loader2 = (Future<GameObject>) null;
          yield break;
        }
      }
    }
    this.setButtonEnabled(false);
    yield return (object) new WaitForSeconds(1f);
    while (Singleton<TutorialRoot>.GetInstance().IsAdviced)
      yield return (object) null;
    if (!Persist.guildRaidProgress.Data.isLastPeriodComplete && Persist.guildRaidProgress.Data.lastPeriodId != this.period_id && this.startedEndless)
    {
      loader2 = new ResourceObject("Prefabs/raid032_top/dir_Raid_Area_Clear_All_anim").Load<GameObject>();
      e = loader2.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(loader2.Result);
      yield return (object) new WaitForSeconds((float) this.allClearWaitMSec / 1000f);
      Singleton<PopupManager>.GetInstance().dismiss();
      this.dynRaidCircling.transform.Clear();
      loader2 = (Future<GameObject>) null;
    }
    if (this.startedEndless && this.isExtraEffect)
    {
      loader2 = new ResourceObject("Prefabs/raid032_top/dir_Raid_ExtraChallenge_anim").Load<GameObject>();
      e = loader2.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(loader2.Result);
      yield return (object) new WaitForSeconds((float) this.circlingWaitMSec / 1000f);
      Singleton<PopupManager>.GetInstance().dismiss();
      this.dynRaidCircling.transform.Clear();
      this.isExtraEffect = false;
      loader2 = (Future<GameObject>) null;
    }
    if (!this.isDefeatBoss && !this.startedEndless)
      yield return (object) this.startDamageRewardPopUp(false);
    if (this.raidPeriodStatus != RaidTopMenu.RaidPeriodStatus.ENABLE && this.defeat_rewards != null && this.defeat_rewards.Length != 0)
    {
      yield return (object) this.startKillRewardPopUpForPeriodOver();
      if (this.isRankingResult)
        yield return (object) this.startRankingReward();
    }
    else if (this.isDefeatBoss)
      yield return (object) this.startDefeatAnim();
    else if (this.isRankingResult)
      yield return (object) this.startRankingReward();
    if (this.isComplete)
    {
      this.dirRaidBossDefeat.SetActive(true);
      this.dirRaidBossDefeatLabel.SetTextLocalize(this.canEndless ? Consts.GetInstance().RAID_BOSS_DEFEAT_ENDLESS : Consts.GetInstance().RAID_BOSS_DEFEAT);
    }
    this.setButtonEnabled(true);
    this.setObjectsByPeriodStatus();
  }

  public IEnumerator startDefeatAnim()
  {
    this.isTapSkip = false;
    this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_BossDefeat);
    while (this.DefeatState != RaidTopMenu.DefeatStateType.eDefeat_End)
      yield return (object) null;
  }

  public IEnumerator startRankingReward()
  {
    GuildRaidRankingRewardPopupSequence rankingRewardPopupSeq = new GuildRaidRankingRewardPopupSequence();
    yield return (object) rankingRewardPopupSeq.Init(this.rank_period_id, this.damage_rank, this.myGuild.guild_name, this.raid_total_damage);
    if (this.guild_ranking_rewards != null)
    {
      foreach (WebAPI.Response.GuildraidRaidTopRaid_guild_ranking_rewards guildRankingReward in this.guild_ranking_rewards)
      {
        if (guildRankingReward.rewards != null)
        {
          foreach (WebAPI.Response.GuildraidRaidTopRaid_guild_ranking_rewardsRewards reward in guildRankingReward.rewards)
            rankingRewardPopupSeq.addRewardData(guildRankingReward.condition_id, (MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
        }
      }
    }
    if (this.guild_ranking_rewardsExtra != null)
    {
      foreach (WebAPI.Response.GuildraidRaidTopRaid_guild_ranking_guild_rewards rankingGuildRewards in this.guild_ranking_rewardsExtra)
      {
        if (rankingGuildRewards.rewards != null)
        {
          foreach (GuildRankingGuildReward reward in rankingGuildRewards.rewards)
            rankingRewardPopupSeq.addRewardData(rankingGuildRewards.condition_id, GuildUtil.getCommonRewardType(reward.reward_type), reward.reward_id, reward.reward_quantity, true);
        }
      }
    }
    yield return (object) rankingRewardPopupSeq.Run();
  }

  private void setDefeatState(RaidTopMenu.DefeatStateType state)
  {
    if (this.DefeatState == state)
      return;
    this.DefeatState = state;
    switch (this.DefeatState)
    {
      case RaidTopMenu.DefeatStateType.eDefeat_BossDefeat:
        this.StartCoroutine(this.runBossDefeat());
        break;
      case RaidTopMenu.DefeatStateType.eDefeat_RewardDialog:
        this.StartCoroutine(this.runDefeatRewardDialog());
        break;
      case RaidTopMenu.DefeatStateType.eDefeat_AllClear:
        this.StartCoroutine(this.runAllClear());
        break;
      case RaidTopMenu.DefeatStateType.eDefeat_Circling:
        this.StartCoroutine(this.runCircling());
        break;
      case RaidTopMenu.DefeatStateType.eDefeat_NextBoss:
        this.StartCoroutine(this.runNextBoss());
        break;
      case RaidTopMenu.DefeatStateType.eDefeat_AllClearAndExtraStage:
        this.StartCoroutine(this.runAllClearAndExtraState());
        break;
      case RaidTopMenu.DefeatStateType.eDefeat_End:
        this.isDefeatBoss = false;
        Persist.guildRaidProgress.Flush();
        break;
    }
  }

  public IEnumerator runBossDefeat()
  {
    GuildRaidUnitWanted unitWantedPre = this.dynRaidBaseList[this.defeatCurrentOrder - 1].GetComponentInChildren<GuildRaidUnitWanted>();
    if (!Object.op_Equality((Object) unitWantedPre, (Object) null))
    {
      unitWantedPre.playClearAnim();
      if (Object.op_Inequality((Object) this.UIModels[this.defeatCurrentOrder - 1], (Object) null))
      {
        this.UIModels[this.defeatCurrentOrder - 1].AnimDeath();
        this.UIModels[this.defeatCurrentOrder - 1].setSpeedAnim(2f);
        TweenColor component = ((Component) this.UIModels[this.defeatCurrentOrder - 1]).GetComponent<TweenColor>();
        component.from = Color.white;
        component.to = this.ClearGray;
        ((UITweener) component).duration = 0.5f;
        ((UITweener) component).PlayForward();
      }
      unitWantedPre.startBossDefeatAnim(this.defeatWaitMSec);
      while (!unitWantedPre.isDefeatAnimEnd)
        yield return (object) null;
      unitWantedPre.setHpGaugeEnable(false);
      if (!this.startedEndless)
        yield return (object) this.startDamageRewardPopUp(true);
      this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_RewardDialog);
    }
  }

  private IEnumerator runDefeatRewardDialog()
  {
    if (!this.startedEndless)
      yield return (object) this.startKillRewardPopUp();
    Persist.guildRaidProgress.Data.lastPeriodId = this.period_id;
    Persist.guildRaidProgress.Data.lastLap = this.defeatCurrentLap;
    Persist.guildRaidProgress.Data.lastOrder = this.defeatCurrentOrder;
    if (this.lapNow == this.defeatCurrentLap && this.orderNow == this.defeatCurrentOrder && this.isComplete)
    {
      if (!Persist.guildRaidProgress.Data.isLastPeriodComplete && Persist.guildRaidProgress.Data.lastPeriodId != this.period_id && this.startedEndless)
        this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_AllClearAndExtraStage);
      else
        this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_AllClear);
    }
    else if (this.defeatCurrentOrder == this.getOrderMax(this.defeatCurrentLap) && !this.isComplete)
      this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_Circling);
    else
      this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_NextBoss);
  }

  public IEnumerator runAllClear()
  {
    RaidTopMenu raidTopMenu = this;
    Persist.guildRaidProgress.Data.isLastPeriodComplete = true;
    raidTopMenu.currentWanted.setHpGaugeEnable(false);
    Future<GameObject> loader = new ResourceObject("Prefabs/raid032_top/dir_Raid_Area_Clear_All_anim").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(raidTopMenu.dynRaidCircling.transform);
    yield return (object) new WaitForSeconds((float) raidTopMenu.allClearWaitMSec / 1000f);
    raidTopMenu.dynRaidCircling.transform.Clear();
    raidTopMenu.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_End);
    if (raidTopMenu.startedEndless)
    {
      // ISSUE: reference to a compiler-generated method
      e = WebAPI.GuildraidRaidEndlessEntry(raidTopMenu.period_id, new Action<WebAPI.Response.UserError>(raidTopMenu.\u003CrunAllClear\u003Eb__115_0)).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Persist.guildRaidProgress.Data.lastPeriodId = raidTopMenu.period_id;
      Persist.guildRaidProgress.Data.lastLap = raidTopMenu.topFloorNum - 1;
      Persist.guildRaidProgress.Data.lastOrder = 1;
      yield return (object) raidTopMenu.StartCoroutine(raidTopMenu.InitializeAsync(true));
    }
  }

  public IEnumerator runAllClearAndExtraState()
  {
    Future<GameObject> loader = new ResourceObject("Prefabs/raid032_top/dir_Raid_Area_Clear_All_anim").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(this.dynRaidCircling.transform);
    yield return (object) new WaitForSeconds((float) this.allClearWaitMSec / 1000f);
    this.dynRaidCircling.transform.Clear();
    Future<GameObject> loader2 = new ResourceObject("Prefabs/raid032_top/dir_Raid_ExtraChallenge_anim").Load<GameObject>();
    e = loader2.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(loader2.Result);
    yield return (object) new WaitForSeconds((float) this.circlingWaitMSec / 1000f);
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public IEnumerator runCircling()
  {
    Future<GameObject> loader = new ResourceObject("Prefabs/raid032_top/dir_Raid_Area_Clear_anim").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(this.dynRaidCircling.transform);
    yield return (object) new WaitForSeconds((float) this.circlingWaitMSec / 1000f);
    ((IEnumerable<GameObject>) this.slcRoadOnList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    ((IEnumerable<GameObject>) this.slcRoadOffList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    ((IEnumerable<GameObject>) this.dirRoadAnimList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    ((IEnumerable<GameObject>) this.dynRaidBaseList).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    this.isDefeatBoss = false;
    e = this.setWantedAndRoad(false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.currentWanted.setButtonEnabled(false);
    this.lapInfo.setLapNum(this.currLoopNum, this.lapMax);
    ((UITweener) this.dirMapTweenAlpha).PlayForward();
    for (int index = 0; index <= this.orderMax - 1; ++index)
    {
      GuildRaidUnitWanted componentInChildren = this.dynRaidBaseList[index].GetComponentInChildren<GuildRaidUnitWanted>();
      if (!Object.op_Equality((Object) componentInChildren, (Object) null))
        componentInChildren.alphaIn();
    }
    yield return (object) new WaitForSeconds((float) this.nextLapSetWaitMSec / 1000f);
    this.dynRaidCircling.transform.Clear();
    this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_End);
  }

  public IEnumerator runNextBoss()
  {
    this.dirRoadAnimList[this.defeatCurrentOrder - 1].SetActive(true);
    ++this.defeatCurrentOrder;
    if (Object.op_Inequality((Object) this.UIModels[this.defeatCurrentOrder - 1], (Object) null))
    {
      TweenColor component = ((Component) this.UIModels[this.defeatCurrentOrder - 1]).GetComponent<TweenColor>();
      component.from = Color.gray;
      component.to = Color.white;
      ((UITweener) component).duration = 0.5f;
      ((UITweener) component).PlayForward();
    }
    this.currentWanted = this.dynRaidBaseList[this.defeatCurrentOrder - 1].GetComponentInChildren<GuildRaidUnitWanted>();
    this.currentWanted.setCurrent();
    this.currentWanted.setButtonEnabled(false);
    if (this.lapNow == this.defeatCurrentLap && this.orderNow == this.defeatCurrentOrder && !this.isComplete)
      this.currentWanted.setHpGaugeEnable(true, this.damageRatio);
    else
      this.currentWanted.setHpGaugeEnable(true);
    IEnumerator e = this.tapSkipWait((long) this.nextCurrentDispWaitMSec);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.isTapSkip)
    {
      this.dirRoadAnimList[this.defeatCurrentOrder - 2].SetActive(false);
      this.slcRoadOnList[this.defeatCurrentOrder - 2].SetActive(true);
      TweenColor component = ((Component) this.UIModels[this.defeatCurrentOrder - 1]).GetComponent<TweenColor>();
      component.from = Color.white;
      component.to = Color.white;
      ((UITweener) component).PlayForward();
      this.isTapSkip = false;
    }
    if (this.lapNow == this.defeatCurrentLap && this.orderNow == this.defeatCurrentOrder && !this.isComplete)
    {
      if (!this.startedEndless)
        yield return (object) this.startDamageRewardPopUp(false);
      this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_End);
    }
    else
      this.setDefeatState(RaidTopMenu.DefeatStateType.eDefeat_BossDefeat);
  }

  private IEnumerator tapSkipWait(long waitMSec)
  {
    Stopwatch sw = new Stopwatch();
    sw.Start();
    while (sw.ElapsedMilliseconds < waitMSec)
    {
      if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
      {
        this.isTapSkip = true;
        yield return (object) null;
        break;
      }
      yield return (object) null;
    }
    sw.Stop();
  }

  private IEnumerator startDamageRewardPopUp(bool isDefeat)
  {
    RaidTopMenu raidTopMenu = this;
    List<RaidDamageReward> list = new List<RaidDamageReward>();
    GuildRaid guildRaid = (GuildRaid) null;
    if (raidTopMenu.raidPeriodStatus == RaidTopMenu.RaidPeriodStatus.ENABLE)
      guildRaid = raidTopMenu.targetInfoList[raidTopMenu.defeatCurrentOrder - 1];
    foreach (WebAPI.Response.GuildraidRaidTopDamage_rewards damageReward in raidTopMenu.damage_rewards)
    {
      if (guildRaid == null || damageReward.quest_id == guildRaid.ID)
      {
        foreach (RaidDamageReward reward in damageReward.rewards)
          list.Add(new RaidDamageReward()
          {
            reward_id = reward.reward_id,
            reward_type_id = reward.reward_type_id,
            reward_quantity = reward.reward_quantity,
            damage_ratio = reward.damage_ratio
          });
      }
    }
    if (list.Count<RaidDamageReward>() > 0)
    {
      if (!isDefeat && raidTopMenu.raidPeriodStatus == RaidTopMenu.RaidPeriodStatus.ENABLE)
        yield return (object) raidTopMenu.StartCoroutine(raidTopMenu.UIModels[raidTopMenu.defeatCurrentOrder - 1].AnimRaidDamageReward());
      RaidDamageRewardPopupSequence rewardPopupSeq = new RaidDamageRewardPopupSequence();
      yield return (object) rewardPopupSeq.Init(list.ToArray());
      yield return (object) rewardPopupSeq.Run();
    }
  }

  private IEnumerator startKillRewardPopUpForPeriodOver()
  {
    List<RaidDefeatReward> list = new List<RaidDefeatReward>();
    WebAPI.Response.GuildraidRaidTopDefeat_rewards[] topDefeatRewardsArray = this.defeat_rewards;
    for (int index = 0; index < topDefeatRewardsArray.Length; ++index)
    {
      foreach (RaidDefeatReward reward in topDefeatRewardsArray[index].rewards)
        list.Add(new RaidDefeatReward()
        {
          reward_id = reward.reward_id,
          reward_type_id = reward.reward_type_id,
          reward_quantity = reward.reward_quantity,
          reward_message = reward.reward_title
        });
      if (list.Count<RaidDefeatReward>() > 0)
      {
        RaidKillRewardPopupSequence killRewardPopupSeq = new RaidKillRewardPopupSequence(true);
        yield return (object) killRewardPopupSeq.Init(this.LastPrevRaidID, list.ToArray());
        yield return (object) killRewardPopupSeq.Run();
        list.Clear();
        killRewardPopupSeq = (RaidKillRewardPopupSequence) null;
      }
    }
    topDefeatRewardsArray = (WebAPI.Response.GuildraidRaidTopDefeat_rewards[]) null;
  }

  private IEnumerator startKillRewardPopUp()
  {
    List<RaidDefeatReward> source = new List<RaidDefeatReward>();
    GuildRaid targetInfo = this.targetInfoList[this.defeatCurrentOrder - 1];
    foreach (WebAPI.Response.GuildraidRaidTopDefeat_rewards defeatReward in this.defeat_rewards)
    {
      if (defeatReward.quest_id == targetInfo.ID)
      {
        foreach (RaidDefeatReward reward in defeatReward.rewards)
          source.Add(new RaidDefeatReward()
          {
            reward_id = reward.reward_id,
            reward_type_id = reward.reward_type_id,
            reward_quantity = reward.reward_quantity,
            reward_message = reward.reward_title
          });
      }
    }
    if (source.Count<RaidDefeatReward>() > 0)
    {
      RaidKillRewardPopupSequence killRewardPopupSeq = new RaidKillRewardPopupSequence(true);
      yield return (object) killRewardPopupSeq.Init(targetInfo.ID, source.ToArray());
      yield return (object) killRewardPopupSeq.Run();
    }
  }

  private void setButtonEnabled(bool value)
  {
    ((Behaviour) this.ibtnRaidShop).enabled = value;
    ((Behaviour) this.ibtnRank).enabled = value;
    ((Behaviour) this.ibtnHelp).enabled = value;
    ((Behaviour) this.ibtnHuntingInfo).enabled = value;
    ((Behaviour) this.ibtnBack).enabled = value;
    if (Object.op_Inequality((Object) this.currentWanted, (Object) null))
      this.currentWanted.setButtonEnabled(value);
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(value);
    Singleton<CommonRoot>.GetInstance().guildChatManager.isOpenTapDisabled = !value;
  }

  private void setObjectsByPeriodStatus()
  {
    switch (this.raidPeriodStatus)
    {
      case RaidTopMenu.RaidPeriodStatus.ENABLE:
        this.dirRaidAggregating.SetActive(false);
        this.dirRaidClose.SetActive(false);
        if (!this.isDefeatBoss && !this.isRankingResult && this.isComplete)
        {
          int num;
          if (this.canEndless)
          {
            GuildRole? role = PlayerAffiliation.Current.role;
            GuildRole guildRole1 = GuildRole.master;
            if (!(role.GetValueOrDefault() == guildRole1 & role.HasValue))
            {
              role = PlayerAffiliation.Current.role;
              GuildRole guildRole2 = GuildRole.sub_master;
              num = role.GetValueOrDefault() == guildRole2 & role.HasValue ? 1 : 0;
            }
            else
              num = 1;
          }
          else
            num = 0;
          bool flag = num != 0;
          this.dirRaidBossDefeat.SetActive(!flag);
          this.dirRaidBossDefeatLabel.SetTextLocalize(this.canEndless ? Consts.GetInstance().RAID_BOSS_DEFEAT_ENDLESS : Consts.GetInstance().RAID_BOSS_DEFEAT);
          if (flag && Object.op_Equality((Object) this.endless, (Object) null))
          {
            this.endless = this.raidEndlessChallengeWindow.Clone().GetComponent<RaidEndlessChallenge>();
            if (Object.op_Inequality((Object) ((Component) this.endless).GetComponent<UIWidget>(), (Object) null))
              ((UIRect) ((Component) this.endless).GetComponent<UIWidget>()).alpha = 0.0f;
            this.endless.Init(this.period_id, this);
            Singleton<PopupManager>.GetInstance().open(((Component) this.endless).gameObject, isCloned: true);
          }
        }
        ((Behaviour) this.ibtnHuntingInfo).enabled = true;
        ((Component) this.ibtnHuntingInfo).GetComponent<UIWidget>().color = new Color(1f, 1f, 1f);
        foreach (Component component1 in ((Component) this.ibtnHuntingInfo).transform)
        {
          UISprite component2 = component1.GetComponent<UISprite>();
          if (Object.op_Inequality((Object) component2, (Object) null))
            ((UIWidget) component2).color = new Color(0.5f, 0.5f, 0.5f);
        }
        ((UIWidget) this.sprZoom).color = new Color(0.5f, 0.5f, 0.5f);
        ((Behaviour) this.ibtnRank).enabled = true;
        break;
      case RaidTopMenu.RaidPeriodStatus.AGGREGATING:
        this.dirRaidAggregating.SetActive(true);
        this.dirRaidClose.SetActive(false);
        this.dirRaidBossDefeat.SetActive(false);
        ((Behaviour) this.ibtnHuntingInfo).enabled = false;
        ((Component) this.ibtnHuntingInfo).GetComponent<UIWidget>().color = new Color(0.5f, 0.5f, 0.5f);
        foreach (Component component3 in ((Component) this.ibtnHuntingInfo).transform)
        {
          UISprite component4 = component3.GetComponent<UISprite>();
          if (Object.op_Inequality((Object) component4, (Object) null))
            ((UIWidget) component4).color = new Color(0.25f, 0.25f, 0.25f);
        }
        ((UIWidget) this.sprZoom).color = new Color(0.25f, 0.25f, 0.25f);
        ((Behaviour) this.ibtnRank).enabled = false;
        break;
      case RaidTopMenu.RaidPeriodStatus.OVER:
        this.dirRaidAggregating.SetActive(false);
        this.dirRaidClose.SetActive(true);
        this.dirRaidBossDefeat.SetActive(false);
        ((Behaviour) this.ibtnHuntingInfo).enabled = true;
        ((Component) this.ibtnHuntingInfo).GetComponent<UIWidget>().color = new Color(1f, 1f, 1f);
        foreach (Component component5 in ((Component) this.ibtnHuntingInfo).transform)
        {
          UISprite component6 = component5.GetComponent<UISprite>();
          if (Object.op_Inequality((Object) component6, (Object) null))
            ((UIWidget) component6).color = new Color(0.5f, 0.5f, 0.5f);
        }
        ((UIWidget) this.sprZoom).color = new Color(0.5f, 0.5f, 0.5f);
        ((Behaviour) this.ibtnRank).enabled = true;
        break;
    }
  }

  public override void onBackButton()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen || this.IsPushAndSet())
      return;
    if (Singleton<CommonRoot>.GetInstance().guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.DetailedView)
      Singleton<CommonRoot>.GetInstance().guildChatManager.OnBackButtonClicked();
    else
      MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
  }

  public virtual void IbtnBack() => this.onBackButton();

  public void IbtnRaidShop()
  {
    if (this.IsPushAndSet())
      return;
    Raid032ShopScene.ChangeScene(true);
    this.StartCoroutine(this.IsPushOff());
  }

  public void IbtnRaidRank()
  {
    if (this.IsPushAndSet())
      return;
    int id = 0;
    if (this.targetInfoList.Count <= 0)
    {
      for (int index = 0; index < MasterData.GuildRaidList.Length; ++index)
      {
        if (MasterData.GuildRaidList[index].period_id == this.period_id)
        {
          id = MasterData.GuildRaidList[index].ID;
          break;
        }
      }
    }
    else
      id = this.targetInfoList[this.orderNow - 1].ID;
    Raid032GuildRankingScene.ChangeScene(id);
    this.StartCoroutine(this.IsPushOff());
  }

  public void IbtnHelp()
  {
    if (this.IsPushAndSet())
      return;
    Help0152Scene.ChangeScene(true, ((IEnumerable<HelpCategory>) MasterData.HelpCategoryList).FirstOrDefault<HelpCategory>((Func<HelpCategory, bool>) (x => x.ID == 27)));
    this.StartCoroutine(this.IsPushOff());
  }

  public void IbtnRaidHuntingInfo()
  {
    if (this.IsPushAndSet())
      return;
    Raid032HuntingInfoScene.ChangeScene(true, this.period_id);
    this.StartCoroutine(this.IsPushOff());
  }

  public void ShowTargetDetail()
  {
    if (this.IsPushAndSet())
      return;
    Raid032BattleScene.changeScene(true, this.currLoopNum, this.targetInfoList[this.orderNow - 1].ID, false, false);
    this.StartCoroutine(this.IsPushOff());
  }

  private enum DefeatStateType
  {
    eDefeat_None,
    eDefeat_BossDefeat,
    eDefeat_RewardDialog,
    eDefeat_AllClear,
    eDefeat_Circling,
    eDefeat_NextBoss,
    eDefeat_AllClearAndExtraStage,
    eDefeat_End,
  }

  private enum RaidPeriodStatus
  {
    ENABLE,
    AGGREGATING,
    OVER,
  }
}
