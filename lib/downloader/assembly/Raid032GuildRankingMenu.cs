// Decompiled with JetBrains decompiler
// Type: Raid032GuildRankingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Raid032GuildRankingMenu : BackButtonMenuBase
{
  private const int offSet = 90;
  private const int height = 147;
  private const int gridCycleOffSet = 20;
  private const int maxShowItem = 2;
  public Transform bottom;
  public UIButton backBtn;
  public UIButton previousPageBtn;
  public UIButton nextPageBtn;
  public UIButton personDamageRankingBtn;
  public UIButton guildRankingRewardBtn;
  public UILabel eventPeriod;
  public UILabel eventName;
  public GameObject body;
  public Transform bottomEmptyObj;
  public Transform topEmptyObj;
  private GuildInfoPopup guildPopup;
  private UIScrollView scroll;
  private UIGridCycle wrap;
  private WebAPI.Response.GuildraidRankingGuild ranking;
  private Guild028114Popup guile028114;
  private DateTime serverTime;
  private Raid032GuildRankingPlayer myRanking;
  private GameObject prefabRankingRow_;
  private GameObject prefabUnitIcon_;
  private List<Raid032GuildRankingPlayer> prefabList = new List<Raid032GuildRankingPlayer>();
  private Dictionary<int, WebAPI.Response.GuildraidRankingGuild> rankingDic = new Dictionary<int, WebAPI.Response.GuildraidRankingGuild>();
  private float scrollLength;
  private int[] periodId = new int[0];
  private int currentIndex;
  private bool isInitialized_;

  private void Start()
  {
    this.backBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.onClickedClose())));
    this.previousPageBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      if (this.currentIndex <= 0)
        return;
      --this.currentIndex;
      this.StartCoroutine(this.RefreshInfo());
    })));
    this.nextPageBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      if (this.currentIndex >= this.periodId.Length - 1)
        return;
      ++this.currentIndex;
      this.StartCoroutine(this.RefreshInfo());
    })));
    this.guildRankingRewardBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => Raid032GuildRankingRewardScene.ChangeScene(this.periodId[this.currentIndex]))));
  }

  public IEnumerator Initalize(GuildRaid mMasterData)
  {
    Raid032GuildRankingMenu guildRankingMenu = this;
    guildRankingMenu.isInitialized_ = false;
    guildRankingMenu.personDamageRankingBtn.onClick.Clear();
    guildRankingMenu.personDamageRankingBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      if (mMasterData == null)
        return;
      Raid032MyRankingScene.changeScene(mMasterData);
    })));
    Future<GameObject> ld;
    IEnumerator e;
    if (Object.op_Equality((Object) guildRankingMenu.prefabRankingRow_, (Object) null))
    {
      ld = Res.Prefabs.raid032_guild_ranking.raid_vscroll_810_12.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guildRankingMenu.prefabRankingRow_ = ld.Result;
      if (Object.op_Equality((Object) guildRankingMenu.prefabRankingRow_, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) guildRankingMenu.prefabUnitIcon_, (Object) null))
    {
      ld = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guildRankingMenu.prefabUnitIcon_ = ld.Result;
      if (Object.op_Equality((Object) guildRankingMenu.prefabUnitIcon_, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    if (guildRankingMenu.periodId.Length == 0)
      guildRankingMenu.periodId = MasterData.GuildRaidPeriod.Select<KeyValuePair<int, GuildRaidPeriod>, int>((Func<KeyValuePair<int, GuildRaidPeriod>, int>) (x => x.Key)).ToArray<int>();
    guildRankingMenu.serverTime = ServerTime.NowAppTime();
    int num1 = 0;
    bool flag = false;
    int num2 = 0;
    foreach (KeyValuePair<int, GuildRaidPeriod> keyValuePair in MasterData.GuildRaidPeriod)
    {
      DateTime? endAt;
      if (guildRankingMenu.serverTime >= keyValuePair.Value.start_at.Value)
      {
        DateTime serverTime = guildRankingMenu.serverTime;
        endAt = keyValuePair.Value.end_at;
        if ((endAt.HasValue ? (serverTime < endAt.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        {
          guildRankingMenu.currentIndex = num1;
          flag = true;
          break;
        }
      }
      DateTime serverTime1 = guildRankingMenu.serverTime;
      endAt = keyValuePair.Value.end_at;
      if ((endAt.HasValue ? (serverTime1 >= endAt.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        num2 = num1;
      ++num1;
    }
    if (!flag)
      guildRankingMenu.currentIndex = num2;
    e = guildRankingMenu.SetRankInfo(true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) guildRankingMenu.myRanking, (Object) null))
    {
      GameObject gameObject = guildRankingMenu.prefabRankingRow_.Clone(((Component) guildRankingMenu.bottom).transform);
      guildRankingMenu.myRanking = gameObject.GetComponent<Raid032GuildRankingPlayer>();
      ((Component) guildRankingMenu.myRanking).gameObject.transform.localPosition = Vector2.op_Implicit(new Vector2(0.0f, -288f));
    }
    guildRankingMenu.myRanking.showGuildPopUp = (Action<GuildDirectory>) (x => this.StartCoroutine(this.ShowGuildPopUp(x)));
    guildRankingMenu.myRanking.Initialize(guildRankingMenu.prefabUnitIcon_, guildRankingMenu.GetMyRankingData());
    e = guildRankingMenu.myRanking.InitImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guildRankingMenu.StartCoroutine(guildRankingMenu.InitGuildInfoPop());
    guildRankingMenu.isInitialized_ = true;
  }

  private IEnumerator InitGuildInfoPop()
  {
    if (this.guildPopup == null)
    {
      this.guildPopup = new GuildInfoPopup();
      IEnumerator e = this.guildPopup.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator ShowGuildPopUp(GuildDirectory guild)
  {
    GameObject prefab = this.guildPopup.guildInfoPopup.Clone();
    this.guile028114 = prefab.GetComponent<Guild028114Popup>();
    prefab.SetActive(false);
    this.guile028114.Initialize(guild, this.guildPopup);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    IEnumerator e = this.guile028114.ResetScrollPosition();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private WebAPI.Response.GuildraidRankingGuildDamage_rankings GetMyRankingData()
  {
    GuildPlayerInfo currentGuildLeaderInfo = this.GetCurrentGuildLeaderInfo();
    WebAPI.Response.GuildraidRankingGuildDamage_rankings myRankingData = new WebAPI.Response.GuildraidRankingGuildDamage_rankings();
    if (currentGuildLeaderInfo != null)
    {
      myRankingData.leader_unit_id = currentGuildLeaderInfo._leader_unit_unit;
      myRankingData.leader_unit_level = currentGuildLeaderInfo.leader_unit_level;
      myRankingData.leader_unit_job_id = currentGuildLeaderInfo.leader_unit_job_id;
    }
    else
      Debug.LogError((object) "leaderInfo is null");
    myRankingData.guild_name = PlayerAffiliation.Current.guild.guild_name;
    myRankingData.guild_id = PlayerAffiliation.Current.guild.guild_id;
    myRankingData.current_emblem_id = PlayerAffiliation.Current.guild.appearance._current_emblem;
    myRankingData.membership_num = PlayerAffiliation.Current.guild.appearance.membership_num;
    myRankingData.membership_capacity = PlayerAffiliation.Current.guild.appearance.membership_capacity;
    myRankingData.score = this.rankingDic[this.periodId[this.currentIndex]].player_guild_score.damage_score;
    myRankingData.rank = this.rankingDic[this.periodId[this.currentIndex]].player_guild_score.damage_rank.HasValue ? this.rankingDic[this.periodId[this.currentIndex]].player_guild_score.damage_rank.Value : 0;
    return myRankingData;
  }

  private GuildPlayerInfo GetCurrentGuildLeaderInfo()
  {
    for (int index = 0; index < PlayerAffiliation.Current.guild.memberships.Length; ++index)
    {
      if (PlayerAffiliation.Current.guild.memberships[index].role == GuildRole.master)
        return PlayerAffiliation.Current.guild.memberships[index].player;
    }
    return (GuildPlayerInfo) null;
  }

  private IEnumerator RefreshInfo()
  {
    IEnumerator e = this.SetRankInfo(true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.myRanking.Initialize(this.prefabUnitIcon_, this.GetMyRankingData());
  }

  private IEnumerator SetRankInfo(bool isImmediate = false)
  {
    Raid032GuildRankingMenu guildRankingMenu = this;
    if (guildRankingMenu.isInitialized_)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
    }
    for (int index = 0; index < guildRankingMenu.prefabList.Count; ++index)
    {
      ((Component) guildRankingMenu.prefabList[index]).gameObject.SetActive(false);
      Object.Destroy((Object) ((Component) guildRankingMenu.prefabList[index]).gameObject);
    }
    guildRankingMenu.prefabList.Clear();
    Action<WebAPI.Response.UserError> userErrorCallback = (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    });
    Future<WebAPI.Response.GuildraidRankingGuild> future = WebAPI.GuildraidRankingGuild(guildRankingMenu.periodId[guildRankingMenu.currentIndex], userErrorCallback);
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.GuildraidRankingGuild result = future.Result;
    if (!guildRankingMenu.rankingDic.ContainsKey(guildRankingMenu.periodId[guildRankingMenu.currentIndex]))
      guildRankingMenu.rankingDic.Add(guildRankingMenu.periodId[guildRankingMenu.currentIndex], result);
    guildRankingMenu.ranking = guildRankingMenu.rankingDic[guildRankingMenu.periodId[guildRankingMenu.currentIndex]];
    if (guildRankingMenu.ranking == null || guildRankingMenu.ranking.damage_rankings == null)
    {
      ((Component) guildRankingMenu.nextPageBtn).gameObject.SetActive(false);
      ((Component) guildRankingMenu.previousPageBtn).gameObject.SetActive(false);
      guildRankingMenu.eventName.SetTextLocalize("");
      Debug.LogError((object) "ranking data is null");
    }
    else
    {
      Raid032GuildRankingPlayer rankingPlayerCatch = (Raid032GuildRankingPlayer) null;
      int num = 0;
      foreach (WebAPI.Response.GuildraidRankingGuildDamage_rankings damageRanking in guildRankingMenu.ranking.damage_rankings)
      {
        if (num < 10)
        {
          rankingPlayerCatch = guildRankingMenu.prefabRankingRow_.Clone(guildRankingMenu.body.transform).GetComponent<Raid032GuildRankingPlayer>();
          guildRankingMenu.prefabList.Add(rankingPlayerCatch);
          ++num;
        }
        else
          break;
      }
      if (guildRankingMenu.ranking.damage_rankings.Length > 2)
      {
        guildRankingMenu.wrap = guildRankingMenu.body.GetComponent<UIGridCycle>();
        ((Behaviour) guildRankingMenu.wrap).enabled = true;
        guildRankingMenu.wrap.minIndex = -guildRankingMenu.ranking.damage_rankings.Length + 1;
        guildRankingMenu.wrap.onInitializeItem = (UIGridCycle.OnInitializeItem) ((obj, idx, realIdx) =>
        {
          obj.SetActive(true);
          rankingPlayerCatch = obj.GetComponent<Raid032GuildRankingPlayer>();
          if (Mathf.Abs(realIdx) < this.ranking.damage_rankings.Length)
          {
            rankingPlayerCatch.showGuildPopUp = (Action<GuildDirectory>) (x => this.StartCoroutine(this.ShowGuildPopUp(x)));
            rankingPlayerCatch.Initialize(this.prefabUnitIcon_, this.ranking.damage_rankings[Mathf.Abs(realIdx)]);
            this.StartCoroutine(rankingPlayerCatch.InitImage());
          }
          else
            obj.SetActive(false);
        });
        guildRankingMenu.wrap.SortBasedOnScrollMovement();
        guildRankingMenu.scroll = ((Component) guildRankingMenu.body.transform.parent).GetComponent<UIScrollView>();
        if (Object.op_Inequality((Object) guildRankingMenu.scroll, (Object) null))
        {
          guildRankingMenu.scrollLength = (float) (guildRankingMenu.wrap.gridSize * Mathf.CeilToInt((float) guildRankingMenu.ranking.damage_rankings.Length / 1f));
          guildRankingMenu.bottomEmptyObj.localPosition = Vector2.op_Implicit(new Vector2(0.0f, (float) (-(double) guildRankingMenu.scrollLength + 90.0)));
          guildRankingMenu.topEmptyObj.localPosition = new Vector3(0.0f, 73f, 0.0f);
          guildRankingMenu.scroll.ResetPosition();
          ((Component) guildRankingMenu.wrap).transform.localPosition = Vector2.op_Implicit(Vector2.zero);
        }
      }
      else
      {
        guildRankingMenu.wrap = guildRankingMenu.body.GetComponent<UIGridCycle>();
        ((Behaviour) guildRankingMenu.wrap).enabled = false;
        for (int index = 0; index < guildRankingMenu.prefabList.Count; ++index)
        {
          ((Component) guildRankingMenu.prefabList[index]).gameObject.transform.localPosition = Vector2.op_Implicit(new Vector2(0.0f, (float) -(index * 90 * 2)));
          guildRankingMenu.prefabList[index].showGuildPopUp = (Action<GuildDirectory>) (x => this.StartCoroutine(this.ShowGuildPopUp(x)));
          guildRankingMenu.prefabList[index].Initialize(guildRankingMenu.prefabUnitIcon_, guildRankingMenu.ranking.damage_rankings[index]);
          guildRankingMenu.StartCoroutine(guildRankingMenu.prefabList[index].InitImage());
        }
        guildRankingMenu.scroll = ((Component) guildRankingMenu.body.transform.parent).GetComponent<UIScrollView>();
        if (Object.op_Inequality((Object) guildRankingMenu.scroll, (Object) null))
        {
          guildRankingMenu.scrollLength = (float) (180 * Mathf.CeilToInt((float) guildRankingMenu.ranking.damage_rankings.Length / 1f));
          guildRankingMenu.bottomEmptyObj.localPosition = Vector2.op_Implicit(new Vector2(0.0f, (float) (-(double) guildRankingMenu.scrollLength + 90.0)));
          guildRankingMenu.topEmptyObj.localPosition = new Vector3(0.0f, 73f, 0.0f);
          ((Component) guildRankingMenu.wrap).transform.localPosition = Vector2.op_Implicit(new Vector2(0.0f, 20f));
        }
        guildRankingMenu.scroll.ResetPosition();
      }
      guildRankingMenu.eventName.SetTextLocalize(MasterData.GuildRaidPeriod[guildRankingMenu.periodId[guildRankingMenu.currentIndex]].period_name);
      if (MasterData.GuildRaidPeriod[guildRankingMenu.periodId[guildRankingMenu.currentIndex]].start_at.HasValue && MasterData.GuildRaidPeriod[guildRankingMenu.periodId[guildRankingMenu.currentIndex]].end_at.HasValue)
      {
        DateTime dateTime1 = MasterData.GuildRaidPeriod[guildRankingMenu.periodId[guildRankingMenu.currentIndex]].start_at.Value;
        DateTime dateTime2 = MasterData.GuildRaidPeriod[guildRankingMenu.periodId[guildRankingMenu.currentIndex]].end_at.Value;
        guildRankingMenu.eventPeriod.SetText(Consts.Format(Consts.GetInstance().VERSUS_002614TERM, (IDictionary) new Hashtable()
        {
          {
            (object) "year",
            (object) dateTime1.Year.ToLocalizeNumberText()
          },
          {
            (object) "month",
            (object) dateTime1.Month.ToLocalizeNumberText()
          },
          {
            (object) "day",
            (object) dateTime1.Day.ToLocalizeNumberText()
          },
          {
            (object) "month2",
            (object) dateTime2.Month.ToLocalizeNumberText()
          },
          {
            (object) "day2",
            (object) dateTime2.Day.ToLocalizeNumberText()
          }
        }));
      }
      else
      {
        Debug.LogError((object) "start or finish during is null");
        guildRankingMenu.eventPeriod.text = "-";
      }
      ((Component) guildRankingMenu.previousPageBtn).gameObject.SetActive(guildRankingMenu.currentIndex > 0);
      ((Component) guildRankingMenu.nextPageBtn).gameObject.SetActive(guildRankingMenu.currentIndex < guildRankingMenu.periodId.Length - 1);
      if (guildRankingMenu.currentIndex < guildRankingMenu.periodId.Length - 1)
        ((UIButtonColor) guildRankingMenu.nextPageBtn).isEnabled = guildRankingMenu.serverTime >= MasterData.GuildRaidPeriod[guildRankingMenu.periodId[guildRankingMenu.currentIndex + 1]].start_at.Value;
      if (guildRankingMenu.isInitialized_)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading || this.IsPushAndSet())
      return;
    this.backScene();
  }
}
