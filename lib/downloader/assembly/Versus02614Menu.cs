// Decompiled with JetBrains decompiler
// Type: Versus02614Menu
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
public class Versus02614Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtTerm;
  [SerializeField]
  private UIButton btnNext;
  [SerializeField]
  private UIButton btnPrevious;
  [SerializeField]
  private UIButton btnTop30;
  [SerializeField]
  private UISprite slcTxtTop30;
  [SerializeField]
  private UIButton btnMyRankSuperior;
  [SerializeField]
  private UISprite slcTxtMyRankSuperior;
  [SerializeField]
  private NGxScroll scrollContainer_long;
  [SerializeField]
  private NGxScroll scrollContainer_short;
  [SerializeField]
  private GameObject objMyRankParent;
  [SerializeField]
  private GameObject lineDecoBottom;
  [SerializeField]
  private GameObject dirBottomBtn;
  [SerializeField]
  private GameObject parentPulldown;
  [SerializeField]
  private int displayWaitTime = 300;
  private GameObject scrollPrefab;
  private GameObject scrollText;
  private GameObject unitIcon;
  private NGxScroll scrollContainer;
  private int nowIndex;
  private RankingGroup[] ranking_data;
  private Dictionary<int, OtherRankingGroup[]> other_ranking_dictionary = new Dictionary<int, OtherRankingGroup[]>();
  private UIPopupList pullDownUI;
  private int nowRankID;
  private bool isSelectTop30 = true;
  private int rankIdMin;

  private IEnumerator requestAPIPvpOtherRanking(int rank_kind)
  {
    if (!this.other_ranking_dictionary.ContainsKey(rank_kind))
    {
      Future<WebAPI.Response.PvpOtherRanking> f = WebAPI.PvpOtherRanking(rank_kind, (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      yield return (object) f.Wait();
      if (f.Result != null)
        this.other_ranking_dictionary[rank_kind] = f.Result.ranking_groups;
    }
  }

  public IEnumerator InitOne()
  {
    Versus02614Menu versus02614Menu = this;
    versus02614Menu.nowIndex = 0;
    versus02614Menu.txtTitle.SetText(Consts.GetInstance().VERSUS_002614TITLE);
    IEnumerator e = versus02614Menu.LoadPreafbFuture();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02614Menu.rankIdMin = MasterData.PvpRankingKind.OrderBy<KeyValuePair<int, PvpRankingKind>, int>((Func<KeyValuePair<int, PvpRankingKind>, int>) (x => x.Value.ID)).First<KeyValuePair<int, PvpRankingKind>>().Value.ID;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/versus026_14/pulldown_class").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Object.Instantiate<GameObject>(prefabF.Result, versus02614Menu.parentPulldown.transform);
    versus02614Menu.pullDownUI = gameObject.GetComponent<UIPopupList>();
    if (versus02614Menu.pullDownUI.onChange == null)
      versus02614Menu.pullDownUI.onChange = new List<EventDelegate>();
    versus02614Menu.pullDownUI.onChange.Add(new EventDelegate(new EventDelegate.Callback(versus02614Menu.onPulldownValueChanged)));
  }

  private IEnumerator LoadPreafbFuture()
  {
    Future<GameObject> scrollPrefabF = new ResourceObject("Prefabs/versus026_14/vscroll_810_12").Load<GameObject>();
    IEnumerator e = scrollPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.scrollPrefab = scrollPrefabF.Result;
    Future<GameObject> scrollTextF = Res.Prefabs.colosseum.colosseum023_7_1.vscroll_810_12_text.Load<GameObject>();
    e = scrollTextF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.scrollText = scrollTextF.Result;
    Future<GameObject> unitIconF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = unitIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon = unitIconF.Result;
  }

  public IEnumerator Init(WebAPI.Response.PvpBoot pvpInfo, RankingGroup[] ranking_data)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.ranking_data = ranking_data;
    this.setInitCurrentRankID();
    IEnumerator e = this.CreateDisplay();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CreateDisplay()
  {
    Versus02614Menu versus02614Menu = this;
    ((UIRect) ((Component) versus02614Menu.scrollContainer_long).GetComponent<UIWidget>()).alpha = 0.0001f;
    Stopwatch sw = new Stopwatch();
    sw.Start();
    yield return (object) null;
    yield return (object) null;
    RankingGroup data = versus02614Menu.ranking_data[versus02614Menu.nowIndex];
    versus02614Menu.scrollContainer_long.Clear();
    versus02614Menu.scrollContainer_short.Clear();
    PvPRankingPlayer mRank = data.my_ranking;
    bool isMyRankEnable = true;
    isMyRankEnable = mRank != null && mRank.current_rank_id == versus02614Menu.nowRankID && mRank.ranking > 0;
    ((Component) versus02614Menu.scrollContainer_long).gameObject.SetActive(!isMyRankEnable);
    versus02614Menu.lineDecoBottom.SetActive(isMyRankEnable);
    versus02614Menu.dirBottomBtn.SetActive(isMyRankEnable);
    ((Component) versus02614Menu.scrollContainer_short).gameObject.SetActive(isMyRankEnable);
    versus02614Menu.scrollContainer = isMyRankEnable ? versus02614Menu.scrollContainer_short : versus02614Menu.scrollContainer_long;
    IEnumerator e;
    if (isMyRankEnable)
    {
      foreach (Component component in versus02614Menu.objMyRankParent.transform)
        Object.Destroy((Object) component.gameObject);
      Versus02614ScrollParts component1 = versus02614Menu.scrollPrefab.Clone(versus02614Menu.objMyRankParent.transform).GetComponent<Versus02614ScrollParts>();
      e = versus02614Menu.SetScrollData(component1, mRank);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (versus02614Menu.ranking_data.Length != 0)
    {
      PvPRankingPlayer[] pvPrankingPlayerArray;
      int index;
      if (isMyRankEnable || mRank != null && mRank.current_rank_id == versus02614Menu.nowRankID)
      {
        if (versus02614Menu.isSelectTop30)
        {
          pvPrankingPlayerArray = data.top_group_ranking;
          for (index = 0; index < pvPrankingPlayerArray.Length; ++index)
          {
            PvPRankingPlayer data1 = pvPrankingPlayerArray[index];
            e = versus02614Menu.CreateScroll(data1);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          pvPrankingPlayerArray = (PvPRankingPlayer[]) null;
        }
        else
        {
          pvPrankingPlayerArray = data.high_group_ranking;
          for (index = 0; index < pvPrankingPlayerArray.Length; ++index)
          {
            PvPRankingPlayer data2 = pvPrankingPlayerArray[index];
            e = versus02614Menu.CreateScroll(data2);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          pvPrankingPlayerArray = (PvPRankingPlayer[]) null;
        }
      }
      else
      {
        yield return (object) versus02614Menu.requestAPIPvpOtherRanking(versus02614Menu.nowRankID);
        OtherRankingGroup otherRankingGroup = versus02614Menu.other_ranking_dictionary[versus02614Menu.nowRankID][versus02614Menu.nowIndex];
        if (otherRankingGroup != null)
        {
          pvPrankingPlayerArray = otherRankingGroup.group_ranking;
          for (index = 0; index < pvPrankingPlayerArray.Length; ++index)
          {
            PvPRankingPlayer data3 = pvPrankingPlayerArray[index];
            e = versus02614Menu.CreateScroll(data3);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          pvPrankingPlayerArray = (PvPRankingPlayer[]) null;
        }
      }
      ((UIButtonColor) versus02614Menu.btnNext).isEnabled = versus02614Menu.nowIndex >= 1;
      ((UIButtonColor) versus02614Menu.btnPrevious).isEnabled = versus02614Menu.nowIndex < versus02614Menu.ranking_data.Length - 1;
      if (versus02614Menu.isSelectTop30)
      {
        ((UIButtonColor) versus02614Menu.btnTop30).isEnabled = false;
        ((UIButtonColor) versus02614Menu.btnMyRankSuperior).isEnabled = true;
        ((UIWidget) versus02614Menu.slcTxtTop30).color = Color.white;
        ((UIWidget) versus02614Menu.slcTxtMyRankSuperior).color = Color.gray;
      }
      else
      {
        ((UIButtonColor) versus02614Menu.btnTop30).isEnabled = true;
        ((UIButtonColor) versus02614Menu.btnMyRankSuperior).isEnabled = false;
        ((UIWidget) versus02614Menu.slcTxtTop30).color = Color.gray;
        ((UIWidget) versus02614Menu.slcTxtMyRankSuperior).color = Color.white;
      }
      DateTime? startTime = data.start_time;
      DateTime? finishTime = data.finish_time;
      if (startTime.HasValue && finishTime.HasValue)
      {
        UILabel txtTerm = versus02614Menu.txtTerm;
        string versus002614Term = Consts.GetInstance().VERSUS_002614TERM;
        Hashtable args = new Hashtable();
        args.Add((object) "year", (object) startTime.Value.Year.ToLocalizeNumberText());
        args.Add((object) "month", (object) startTime.Value.Month.ToLocalizeNumberText());
        args.Add((object) "day", (object) startTime.Value.Day.ToLocalizeNumberText());
        DateTime dateTime = finishTime.Value;
        args.Add((object) "month2", (object) dateTime.Month.ToLocalizeNumberText());
        dateTime = finishTime.Value;
        args.Add((object) "day2", (object) dateTime.Day.ToLocalizeNumberText());
        string text = Consts.Format(versus002614Term, (IDictionary) args);
        txtTerm.SetText(text);
      }
    }
    else
    {
      e = versus02614Menu.CreateScroll((PvPRankingPlayer) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIButtonColor) versus02614Menu.btnNext).isEnabled = false;
      ((UIButtonColor) versus02614Menu.btnPrevious).isEnabled = false;
      versus02614Menu.dirBottomBtn.SetActive(false);
      versus02614Menu.txtTerm.SetText(Consts.GetInstance().COMMON_NOVALUE);
    }
    versus02614Menu.scrollContainer.ResolvePosition();
    sw.Stop();
    if ((mRank == null || mRank.current_rank_id != versus02614Menu.nowRankID) && sw.ElapsedMilliseconds <= (long) versus02614Menu.displayWaitTime)
      yield return (object) new WaitForSeconds((float) ((long) versus02614Menu.displayWaitTime - sw.ElapsedMilliseconds) / 1000f);
    versus02614Menu.pullDownUI.items.Clear();
    List<KeyValuePair<int, PvpRankingKind>> list = MasterData.PvpRankingKind.OrderByDescending<KeyValuePair<int, PvpRankingKind>, int>((Func<KeyValuePair<int, PvpRankingKind>, int>) (x => x.Value.ID)).ToList<KeyValuePair<int, PvpRankingKind>>();
    // ISSUE: reference to a compiler-generated method
    list.ForEach(new Action<KeyValuePair<int, PvpRankingKind>>(versus02614Menu.\u003CCreateDisplay\u003Eb__30_1));
    // ISSUE: reference to a compiler-generated method
    versus02614Menu.pullDownUI.value = list.Find(new Predicate<KeyValuePair<int, PvpRankingKind>>(versus02614Menu.\u003CCreateDisplay\u003Eb__30_2)).Value.name;
    versus02614Menu.pullDownUI.Close();
    versus02614Menu.pullDownUI.isOnSelectClose = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    ((UIRect) ((Component) versus02614Menu.scrollContainer_long).GetComponent<UIWidget>()).alpha = 1f;
    versus02614Menu.IsPush = false;
  }

  private IEnumerator CreateScroll(PvPRankingPlayer data)
  {
    GameObject gameObject = this.scrollPrefab.Clone();
    this.scrollContainer.Add(gameObject);
    IEnumerator e = this.SetScrollData(gameObject.GetComponent<Versus02614ScrollParts>(), data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetScrollData(Versus02614ScrollParts part, PvPRankingPlayer data)
  {
    part.Init(data);
    IEnumerator e = this.scrollText.Clone(part.GetTextDir().transform).GetComponent<Versus02614ScrollText>().Init(data, this.unitIcon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
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

  public void IbtnNext()
  {
    if (this.IsPush)
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    --this.nowIndex;
    this.setInitCurrentRankID();
    this.StartCoroutine(this.CreateDisplay());
  }

  public void IbtnPrevious()
  {
    if (this.IsPush)
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ++this.nowIndex;
    this.setInitCurrentRankID();
    this.StartCoroutine(this.CreateDisplay());
  }

  public void IbtnTop30()
  {
    if (this.IsPush)
      return;
    this.isSelectTop30 = true;
    this.StartCoroutine(this.CreateDisplay());
  }

  public void IbtnMyRankSuperior()
  {
    if (this.IsPush)
      return;
    this.isSelectTop30 = false;
    this.StartCoroutine(this.CreateDisplay());
  }

  public void IbtnReward()
  {
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
