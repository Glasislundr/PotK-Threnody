// Decompiled with JetBrains decompiler
// Type: Versus02615Menu
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
public class Versus02615Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry Scroll;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDurationValue;
  [SerializeField]
  private UIButton btnNext;
  [SerializeField]
  private UIButton btnPrevious;
  [SerializeField]
  private int displayWaitTime = 300;
  private int nowIndex;
  private RankingGroup[] ranking_data;
  private WebAPI.Response.PvpBootCampaign_rewards[] campaign_rewards;
  private UIPopupList pullDownUI;
  private int nowRankID;
  private GameObject boxPrefab;
  private GameObject boxEventPrefab;
  private GameObject marginPrefab;
  private GameObject classSelectPrefab;
  private int rankIdMin;

  public IEnumerator Init(
    RankingGroup[] ranking_data,
    WebAPI.Response.PvpBootCampaign_rewards[] campaign_rewards)
  {
    this.ranking_data = ranking_data;
    this.campaign_rewards = campaign_rewards;
    this.nowIndex = 0;
    this.rankIdMin = MasterData.PvpRankingKind.OrderBy<KeyValuePair<int, PvpRankingKind>, int>((Func<KeyValuePair<int, PvpRankingKind>, int>) (x => x.Value.ID)).First<KeyValuePair<int, PvpRankingKind>>().Value.ID;
    this.setInitCurrentRankID();
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) this.CreateDisplay();
  }

  private IEnumerator CreateDisplay()
  {
    Versus02615Menu versus02615Menu = this;
    RankingGroup data = versus02615Menu.ranking_data[versus02615Menu.nowIndex];
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    PvPRankingPlayer ranking = data.my_ranking;
    versus02615Menu.txtTitle.SetText(Consts.GetInstance().VERSUS_002615TITLE);
    string textPeriod = versus02615Menu.getTextPeriod(data);
    versus02615Menu.txtDurationValue.SetText(textPeriod);
    IEnumerable<PvpClassRankingReward> list = ((IEnumerable<PvpClassRankingReward>) MasterData.PvpClassRankingRewardList).Where<PvpClassRankingReward>((Func<PvpClassRankingReward, bool>) (x => x.term_id == data.period_id && x.ranking_kind_PvpRankingKind == this.nowRankID));
    stopwatch.Stop();
    if (stopwatch.ElapsedMilliseconds <= (long) versus02615Menu.displayWaitTime)
      yield return (object) new WaitForSeconds((float) ((long) versus02615Menu.displayWaitTime - stopwatch.ElapsedMilliseconds) / 1000f);
    yield return (object) versus02615Menu.SetScroll(list.ToArray<PvpClassRankingReward>());
    versus02615Menu.pullDownUI.isOnSelectClose = true;
    if (Object.op_Inequality((Object) versus02615Menu.pullDownUI, (Object) null))
    {
      if (versus02615Menu.pullDownUI.onChange == null)
        versus02615Menu.pullDownUI.onChange = new List<EventDelegate>();
      versus02615Menu.pullDownUI.onChange.Add(new EventDelegate(new EventDelegate.Callback(versus02615Menu.onPulldownValueChanged)));
    }
    versus02615Menu.setArrowEnable();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private string getTextPeriod(RankingGroup ranking_data)
  {
    string textPeriod = Consts.GetInstance().COMMON_NOVALUE;
    DateTime? startTime = ranking_data.start_time;
    DateTime? finishTime = ranking_data.finish_time;
    if (startTime.HasValue && finishTime.HasValue)
    {
      string versus002614Term = Consts.GetInstance().VERSUS_002614TERM;
      Hashtable args = new Hashtable();
      DateTime dateTime = startTime.Value;
      args.Add((object) "year", (object) dateTime.Year.ToLocalizeNumberText());
      dateTime = startTime.Value;
      args.Add((object) "month", (object) dateTime.Month.ToLocalizeNumberText());
      dateTime = startTime.Value;
      args.Add((object) "day", (object) dateTime.Day.ToLocalizeNumberText());
      dateTime = finishTime.Value;
      args.Add((object) "month2", (object) dateTime.Month.ToLocalizeNumberText());
      dateTime = finishTime.Value;
      args.Add((object) "day2", (object) dateTime.Day.ToLocalizeNumberText());
      textPeriod = Consts.Format(versus002614Term, (IDictionary) args);
    }
    return textPeriod;
  }

  private IEnumerator SetScroll(PvpClassRankingReward[] rewards)
  {
    Versus02615Menu versus02615Menu = this;
    ((Component) versus02615Menu.Scroll.Scroll).gameObject.transform.Clear();
    versus02615Menu.Scroll.Reset();
    ((Component) versus02615Menu.Scroll.Scroll).gameObject.SetActive(false);
    Future<GameObject> boxPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) versus02615Menu.boxPrefab, (Object) null))
    {
      boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      versus02615Menu.boxPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) versus02615Menu.boxEventPrefab, (Object) null))
    {
      boxPrefabF = new ResourceObject("Prefabs/versus026_12/slc_Reward_Box_Event").Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      versus02615Menu.boxEventPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) versus02615Menu.marginPrefab, (Object) null))
    {
      boxPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      versus02615Menu.marginPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) versus02615Menu.classSelectPrefab, (Object) null))
    {
      boxPrefabF = new ResourceObject("Prefabs/versus026_12/slc_Reward_Box_Title").Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      versus02615Menu.classSelectPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    bool isPeriod = false;
    RankingGroup data = versus02615Menu.ranking_data[versus02615Menu.nowIndex];
    if (data.reward_receivable_period.HasValue && versus02615Menu.nowIndex == 0)
    {
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      DateTime dateTime = ServerTime.NowAppTime();
      DateTime? receivablePeriod = data.reward_receivable_period;
      if ((receivablePeriod.HasValue ? (dateTime <= receivablePeriod.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        isPeriod = true;
    }
    if (isPeriod && ((IEnumerable<WebAPI.Response.PvpBootCampaign_rewards>) versus02615Menu.campaign_rewards).Count<WebAPI.Response.PvpBootCampaign_rewards>() > 0)
    {
      versus02615Menu.Scroll.Add(versus02615Menu.boxEventPrefab.Clone());
      WebAPI.Response.PvpBootCampaign_rewards[] bootCampaignRewardsArray = versus02615Menu.campaign_rewards;
      for (int index = 0; index < bootCampaignRewardsArray.Length; ++index)
      {
        WebAPI.Response.PvpBootCampaign_rewards data1 = bootCampaignRewardsArray[index];
        GameObject box = versus02615Menu.boxPrefab.Clone();
        yield return (object) box.GetComponent<Versus02612ScrollRewardBox>().Init(data1);
        versus02615Menu.Scroll.Add(box);
        versus02615Menu.Scroll.Add(versus02615Menu.marginPrefab.Clone());
        box = (GameObject) null;
      }
      bootCampaignRewardsArray = (WebAPI.Response.PvpBootCampaign_rewards[]) null;
    }
    GameObject gameObject1 = versus02615Menu.classSelectPrefab.Clone();
    versus02615Menu.Scroll.Add(gameObject1);
    versus02615Menu.pullDownUI = ((Component) gameObject1.transform.GetChildInFind("classes")).GetComponent<UIPopupList>();
    versus02615Menu.pullDownUI.scrollViewPanel = versus02615Menu.Scroll.Scroll.panel;
    versus02615Menu.pullDownUI.keepDropDownWithinClipRegion = true;
    versus02615Menu.pullDownUI.items.Clear();
    List<KeyValuePair<int, PvpRankingKind>> list = MasterData.PvpRankingKind.OrderByDescending<KeyValuePair<int, PvpRankingKind>, int>((Func<KeyValuePair<int, PvpRankingKind>, int>) (x => x.Value.ID)).ToList<KeyValuePair<int, PvpRankingKind>>();
    // ISSUE: reference to a compiler-generated method
    list.ForEach(new Action<KeyValuePair<int, PvpRankingKind>>(versus02615Menu.\u003CSetScroll\u003Eb__19_3));
    // ISSUE: reference to a compiler-generated method
    versus02615Menu.pullDownUI.value = list.Find(new Predicate<KeyValuePair<int, PvpRankingKind>>(versus02615Menu.\u003CSetScroll\u003Eb__19_4)).Value.name;
    foreach (IGrouping<int, PvpClassRankingReward> grouping in (IEnumerable<IGrouping<int, PvpClassRankingReward>>) ((IEnumerable<PvpClassRankingReward>) rewards).GroupBy<PvpClassRankingReward, int>((Func<PvpClassRankingReward, int>) (x => x.ranking_category.ID)).OrderBy<IGrouping<int, PvpClassRankingReward>, int>((Func<IGrouping<int, PvpClassRankingReward>, int>) (x => MasterData.PvpRankingCondition[x.Key].priority)))
    {
      int rankingCondition = grouping.First<PvpClassRankingReward>().ranking_category_PvpRankingCondition;
      bool is_belong = versus02615Menu.isBelongRankingCondition(rankingCondition);
      if (is_belong)
      {
        versus02615Menu.Scroll.Add(versus02615Menu.marginPrefab.Clone());
        versus02615Menu.Scroll.Add(versus02615Menu.marginPrefab.Clone());
      }
      GameObject gameObject2 = versus02615Menu.boxPrefab.Clone();
      versus02615Menu.Scroll.Add(gameObject2);
      e = gameObject2.GetComponent<Versus02612ScrollRewardBox>().Init((IEnumerable<PvpClassRankingReward>) grouping, data.my_ranking.ranking, is_belong, isPeriod);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      versus02615Menu.Scroll.Add(versus02615Menu.marginPrefab.Clone());
    }
    ((Component) versus02615Menu.Scroll.Scroll).gameObject.SetActive(true);
    versus02615Menu.Scroll.ResolvePosition();
  }

  private bool isBelongRankingCondition(int condition_id)
  {
    RankingGroup rankingGroup = this.ranking_data[this.nowIndex];
    if (rankingGroup == null || rankingGroup.my_ranking == null || rankingGroup.my_ranking.ranking == 0 || rankingGroup.my_ranking.current_rank_id != this.nowRankID || condition_id == 7 || condition_id == 12)
      return false;
    PvpRankingCondition rankingCondition = MasterData.PvpRankingCondition[condition_id];
    int? rankUpper = rankingCondition.rank_upper;
    int ranking1 = rankingGroup.my_ranking.ranking;
    if (rankUpper.GetValueOrDefault() <= ranking1 & rankUpper.HasValue)
    {
      int? rankLower = rankingCondition.rank_lower;
      int ranking2 = rankingGroup.my_ranking.ranking;
      if (rankLower.GetValueOrDefault() >= ranking2 & rankLower.HasValue || !rankingCondition.rank_lower.HasValue)
        return true;
    }
    return false;
  }

  private void onPulldownValueChanged()
  {
    PvpRankingKind pvpRankingKind = ((IEnumerable<PvpRankingKind>) MasterData.PvpRankingKindList).FirstOrDefault<PvpRankingKind>((Func<PvpRankingKind, bool>) (x => x.name == this.pullDownUI.value));
    if (pvpRankingKind == null || this.nowRankID == pvpRankingKind.ID)
      return;
    this.nowRankID = pvpRankingKind.ID;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.pullDownUI.isOnSelectClose = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.StartCoroutine(this.CreateDisplay());
  }

  private void setInitCurrentRankID()
  {
    RankingGroup rankingGroup = this.ranking_data[this.nowIndex];
    if (rankingGroup.my_ranking == null || rankingGroup.my_ranking.current_rank_id == 0)
      this.nowRankID = this.rankIdMin;
    else
      this.nowRankID = rankingGroup.my_ranking.current_rank_id;
  }

  private void setArrowEnable()
  {
    ((UIButtonColor) this.btnNext).isEnabled = this.nowIndex >= 1;
    ((UIButtonColor) this.btnPrevious).isEnabled = this.nowIndex < this.ranking_data.Length - 1;
  }

  public void IbtnNext()
  {
    if (this.IsPush || this.nowIndex <= 0)
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    --this.nowIndex;
    this.setInitCurrentRankID();
    this.StartCoroutine(this.CreateDisplay());
  }

  public void IbtnPrevious()
  {
    if (this.IsPush || this.nowIndex >= this.ranking_data.Length - 1)
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ++this.nowIndex;
    this.setInitCurrentRankID();
    this.StartCoroutine(this.CreateDisplay());
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
