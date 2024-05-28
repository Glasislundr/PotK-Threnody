// Decompiled with JetBrains decompiler
// Type: Popup02689Menu
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
public class Popup02689Menu : MonoBehaviour
{
  [SerializeField]
  private UILabel txtSubTitle;
  [SerializeField]
  private UILabel txtWin;
  [SerializeField]
  private UILabel txtLose;
  [SerializeField]
  private UILabel txtDraw;
  [SerializeField]
  private UILabel txtClassName;
  [SerializeField]
  private UILabel txtPeriod;
  [SerializeField]
  private UILabel txtRank;
  [SerializeField]
  private UILabel txtPoint;
  [SerializeField]
  private UILabel txtMyRank;
  [SerializeField]
  private NGxScrollMasonry Scroll;
  [SerializeField]
  private UIWidget afterResult;
  [SerializeField]
  private GameObject effectParent;
  [SerializeField]
  private GameObject dirClassEffectParent;
  [SerializeField]
  private UIWidget Top;
  [SerializeField]
  private UIWidget Bottom;
  private WebAPI.Response.PvpBoot pvpInfo;
  private WebAPI.Response.PvpRankingClose rank_close;
  private WebAPI.Response.PvpSeasonCloseClass_rewards[] rewards;
  private PlayerEmblem[] emblems;
  private bool isEffect;
  private bool isRank;
  private bool isEnd;
  private Versus02610Menu menu;
  private bool isDispWeekRank;

  public IEnumerator Init(
    WebAPI.Response.PvpBoot pvpInfo,
    WebAPI.Response.PvpSeasonClose season_close,
    Versus02610Menu menu)
  {
    Popup02689Menu popup02689Menu = this;
    ((UIRect) popup02689Menu.afterResult).alpha = 0.0f;
    popup02689Menu.menu = menu;
    popup02689Menu.isEffect = true;
    popup02689Menu.isRank = false;
    popup02689Menu.pvpInfo = pvpInfo;
    popup02689Menu.rewards = season_close.class_rewards;
    popup02689Menu.emblems = season_close.new_emblems;
    popup02689Menu.isEnd = false;
    PvpClassKind classData = MasterData.PvpClassKind[pvpInfo.current_class];
    PvpClassKind.Condition condition = classData.ClassCondition(pvpInfo.pvp_class_record.current_season_win_count);
    Popup02689Effect ef = ((Component) popup02689Menu).GetComponent<Popup02689Effect>();
    Future<GameObject> rf = ef.StartEffect(condition);
    IEnumerator e = rf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> namePrefab = Res.Prefabs.versus_result.dir_NewClassBox.Load<GameObject>();
    e = namePrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    rf.Result.Clone(popup02689Menu.effectParent.transform).GetComponentInChildren<PopupPvpClassEffect>().Init(new Action(popup02689Menu.EffectEndProvide), new Action(popup02689Menu.EffectEndEnable));
    namePrefab.Result.Clone(popup02689Menu.effectParent.transform).GetComponent<PopupPvpClassNameEffect>().Init(classData, condition);
    Singleton<NGSoundManager>.GetInstance().playSE(ef.GetSEName(condition));
    popup02689Menu.isDispWeekRank = false;
  }

  public IEnumerator Init(
    WebAPI.Response.PvpBoot pvpInfo,
    WebAPI.Response.PvpRankingClose rank_close,
    Versus02610Menu menu)
  {
    Popup02689Menu popup02689Menu = this;
    ((UIRect) popup02689Menu.afterResult).alpha = 0.0f;
    popup02689Menu.menu = menu;
    popup02689Menu.isEffect = true;
    popup02689Menu.isRank = true;
    popup02689Menu.pvpInfo = pvpInfo;
    popup02689Menu.rank_close = rank_close;
    popup02689Menu.emblems = rank_close.new_emblems;
    Future<GameObject> rf = Res.Prefabs.ranking.ranking_calculated.Load<GameObject>();
    IEnumerator e = rf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    rf.Result.Clone(popup02689Menu.effectParent.transform).GetComponentInChildren<PopupPvpClassEffect>().Init(new Action(popup02689Menu.EffectEndProvide), new Action(popup02689Menu.EffectEndEnable));
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0555");
    popup02689Menu.isDispWeekRank = true;
  }

  public void EffectEndProvide()
  {
    this.isEffect = false;
    this.StartCoroutine(this.EffectEnd());
  }

  public void EffectEndEnable() => this.isEffect = true;

  private IEnumerator EffectEnd()
  {
    while (!this.isEffect)
      yield return (object) null;
    this.effectParent.SetActive(false);
    IEnumerator e;
    if (this.isRank)
    {
      e = this.StartResultRanking();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.StartResultSeason();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = this.EmblemPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isEnd = true;
  }

  private IEnumerator StartResultSeason()
  {
    Consts instance = Consts.GetInstance();
    this.txtClassName.SetText(MasterData.PvpClassKind[this.pvpInfo.current_class].name);
    this.txtSubTitle.SetText(instance.VERSUS_002689POPUP_SUBTITLE);
    this.txtWin.SetText(this.pvpInfo.pvp_class_record.current_season_win_count.ToLocalizeNumberText());
    this.txtLose.SetText(this.pvpInfo.pvp_class_record.current_season_loss_count.ToLocalizeNumberText());
    this.txtDraw.SetText(this.pvpInfo.pvp_class_record.current_season_draw_count.ToLocalizeNumberText());
    this.SetTop();
    this.SetBottom();
    this.SetScroll();
    IEnumerator e = this.StartScroll();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.CreateClassChange();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    this.Scroll.ResolvePosition();
    ((UIRect) this.afterResult).alpha = 1f;
  }

  private IEnumerator StartResultRanking()
  {
    this.txtSubTitle.SetText(Consts.GetInstance().VERSUS_002689POPUP_SUBTITLE_RANKING);
    DateTime? startTime = this.rank_close.start_time;
    DateTime? finishTime = this.rank_close.finish_time;
    UILabel txtPeriod = this.txtPeriod;
    string versus0026810Period = Consts.GetInstance().VERSUS_0026810PERIOD;
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
    string text = Consts.Format(versus0026810Period, (IDictionary) args);
    txtPeriod.SetText(text);
    this.txtRank.SetText(this.rank_close.ranking.ToLocalizeNumberText());
    this.txtPoint.SetText(this.rank_close.ranking_pt.ToLocalizeNumberText());
    this.txtMyRank.SetText(MasterData.PvpRankingKind[this.rank_close.current_rank].name);
    this.SetTop();
    this.SetBottom();
    this.SetScroll();
    IEnumerator e = this.StartScroll(this.rank_close.ranking_rewards);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    this.Scroll.ResolvePosition();
    ((UIRect) this.afterResult).alpha = 1f;
  }

  private IEnumerator StartScroll()
  {
    this.Scroll.Reset();
    ((Component) this.Scroll.Scroll).gameObject.SetActive(false);
    Future<GameObject> boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = boxPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject boxPrefab = boxPrefabF.Result;
    Future<GameObject> marginPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
    e = marginPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject marginPrefab = marginPrefabF.Result;
    foreach (IGrouping<int, WebAPI.Response.PvpSeasonCloseClass_rewards> data in ((IEnumerable<WebAPI.Response.PvpSeasonCloseClass_rewards>) this.rewards).GroupBy<WebAPI.Response.PvpSeasonCloseClass_rewards, int>((Func<WebAPI.Response.PvpSeasonCloseClass_rewards, int>) (x => x.class_reward_type)))
    {
      GameObject gameObject = boxPrefab.Clone();
      this.Scroll.Add(gameObject);
      e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init((IEnumerable<WebAPI.Response.PvpSeasonCloseClass_rewards>) data);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Scroll.Add(marginPrefab.Clone());
    }
    ((Component) this.Scroll.Scroll).gameObject.SetActive(true);
    this.Scroll.ResolvePosition();
  }

  private IEnumerator StartScroll(
    WebAPI.Response.PvpRankingCloseRanking_rewards[] rank_rewards)
  {
    this.Scroll.Reset();
    ((Component) this.Scroll.Scroll).gameObject.SetActive(false);
    Future<GameObject> boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = boxPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject boxPrefab = boxPrefabF.Result;
    Future<GameObject> marginPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
    e = marginPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject marginPrefab = marginPrefabF.Result;
    foreach (IGrouping<int, WebAPI.Response.PvpRankingCloseRanking_rewards> source in (IEnumerable<IGrouping<int, WebAPI.Response.PvpRankingCloseRanking_rewards>>) ((IEnumerable<WebAPI.Response.PvpRankingCloseRanking_rewards>) rank_rewards).GroupBy<WebAPI.Response.PvpRankingCloseRanking_rewards, int>((Func<WebAPI.Response.PvpRankingCloseRanking_rewards, int>) (x => x.condition_id)).OrderBy<IGrouping<int, WebAPI.Response.PvpRankingCloseRanking_rewards>, int>((Func<IGrouping<int, WebAPI.Response.PvpRankingCloseRanking_rewards>, int>) (x => MasterData.PvpRankingCondition[x.Key].priority)))
    {
      GameObject gameObject = boxPrefab.Clone();
      this.Scroll.Add(gameObject);
      e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init((IEnumerable<WebAPI.Response.PvpRankingCloseRanking_rewards>) source.ToList<WebAPI.Response.PvpRankingCloseRanking_rewards>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Scroll.Add(marginPrefab.Clone());
    }
    ((Component) this.Scroll.Scroll).gameObject.SetActive(true);
    this.Scroll.ResolvePosition();
  }

  private IEnumerator CreateClassChange()
  {
    PvpClassKind c = MasterData.PvpClassKind[this.pvpInfo.current_class];
    PvpClassKind.Condition condition = c.ClassCondition(this.pvpInfo.pvp_class_record.current_season_win_count);
    Future<GameObject> pF = Res.Prefabs.popup.popup_Multi_Class_Change.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    pF.Result.Clone(this.dirClassEffectParent.transform).GetComponent<PopupClassChange>().ChangeSprite(condition, c.isLowestClass);
  }

  private IEnumerator EmblemPopup()
  {
    if (this.emblems.Length != 0)
    {
      PlayerEmblem[] playerEmblemArray = this.emblems;
      for (int index = 0; index < playerEmblemArray.Length; ++index)
      {
        PlayerEmblem data = playerEmblemArray[index];
        Future<GameObject> NewEmblemRewardPrefabF = Res.Prefabs.popup.popup_999_14__anim_popup01.Load<GameObject>();
        IEnumerator e = NewEmblemRewardPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject popup = Singleton<PopupManager>.GetInstance().open(NewEmblemRewardPrefabF.Result);
        popup.SetActive(false);
        Popup99914Menu o = popup.GetComponent<Popup99914Menu>();
        e = o.Init(data);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) new WaitForSeconds(0.5f);
        popup.SetActive(true);
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        NewEmblemRewardPrefabF = (Future<GameObject>) null;
        popup = (GameObject) null;
        o = (Popup99914Menu) null;
        data = (PlayerEmblem) null;
      }
      playerEmblemArray = (PlayerEmblem[]) null;
    }
  }

  private void SetTop()
  {
    this.SetAnchor(((UIRect) this.Top).topAnchor, this.menu.GetEffectParent, 1f, 0);
    this.SetAnchor(((UIRect) this.Top).bottomAnchor, this.menu.GetEffectParent, 1f, -960);
    this.SetAnchor(((UIRect) this.Top).leftAnchor, this.menu.GetEffectParent, 0.0f, 0);
    this.SetAnchor(((UIRect) this.Top).rightAnchor, this.menu.GetEffectParent, 1f, 0);
    ((UIRect) this.Top).ResetAnchors();
    ((UIRect) this.Top).UpdateAnchors();
    ((IEnumerable<UIWidget>) ((Component) this.Top).GetComponentsInChildren<UIWidget>()).ForEach<UIWidget>((Action<UIWidget>) (x =>
    {
      ((UIRect) x).ResetAnchors();
      ((UIRect) x).UpdateAnchors();
    }));
  }

  private void SetBottom()
  {
    this.SetAnchor(((UIRect) this.Bottom).topAnchor, this.menu.GetEffectParent, 0.0f, 960);
    this.SetAnchor(((UIRect) this.Bottom).bottomAnchor, this.menu.GetEffectParent, 0.0f, 0);
    this.SetAnchor(((UIRect) this.Bottom).leftAnchor, this.menu.GetEffectParent, 0.0f, 0);
    this.SetAnchor(((UIRect) this.Bottom).rightAnchor, this.menu.GetEffectParent, 1f, 0);
    ((UIRect) this.Bottom).ResetAnchors();
    ((UIRect) this.Bottom).UpdateAnchors();
    ((IEnumerable<UIWidget>) ((Component) this.Top).GetComponentsInChildren<UIWidget>()).ForEach<UIWidget>((Action<UIWidget>) (x =>
    {
      ((UIRect) x).ResetAnchors();
      ((UIRect) x).UpdateAnchors();
    }));
  }

  private void SetScroll()
  {
    UIWidget component = ((Component) this.Scroll).GetComponent<UIWidget>();
    this.SetAnchor(((UIRect) component).topAnchor, this.menu.GetEffectParent, 1f, -287);
    this.SetAnchor(((UIRect) component).bottomAnchor, this.menu.GetEffectParent, 0.0f, (int) sbyte.MaxValue);
    this.SetAnchor(((UIRect) component).leftAnchor, this.menu.GetEffectParent, 0.0f, 5);
    this.SetAnchor(((UIRect) component).rightAnchor, this.menu.GetEffectParent, 1f, -5);
    ((UIRect) component).ResetAnchors();
    ((UIRect) component).UpdateAnchors();
  }

  private void SetAnchor(UIRect.AnchorPoint ap, Transform parent, float relative, int absolute)
  {
    ap.target = parent;
    ap.relative = relative;
    ap.absolute = absolute;
  }

  public void IbtnTouch()
  {
    if (!this.isEnd)
      return;
    this.menu.SetPlayingEffect(false);
    if (this.isDispWeekRank)
    {
      this.StartCoroutine(this.menu.dispWeekRank(this));
    }
    else
    {
      this.updatePvPInfo();
      this.close();
    }
  }

  public void updatePvPInfo(bool _isUIUpdate = true)
  {
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    this.menu.StartSceneUpdate(isUIUpdate: _isUIUpdate);
  }

  public void close() => Object.Destroy((Object) ((Component) this).gameObject);
}
