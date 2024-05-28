// Decompiled with JetBrains decompiler
// Type: ExploreRankingPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExploreRankingPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel LabelPeriod;
  [SerializeField]
  private GameObject scrollGrid;
  [SerializeField]
  private GameObject scrollBar;
  [SerializeField]
  private Explore033RankingPlayer dirPlayerRank;
  private GameObject statusScrollPrefab;
  private List<Explore033RankingPlayer> prefabList = new List<Explore033RankingPlayer>();
  private WebAPI.Response.ExploreRankingRanking rankingData;
  private const int offSet = 30;
  private const int height = 65;
  private const int gridCycleOffSet = 20;
  private const int maxShowItem = 7;
  private UIScrollView scroll;
  private UIGridCycle wrap;
  private float scrollLength;
  public Transform bottomEmptyObj;
  public Transform topEmptyObj;

  public IEnumerator Initialize()
  {
    Future<WebAPI.Response.ExploreRankingRanking> futureF = WebAPI.ExploreRankingRanking((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result != null)
    {
      this.rankingData = futureF.Result;
      futureF = (Future<WebAPI.Response.ExploreRankingRanking>) null;
      yield return (object) this.LoadResource();
      this.LabelPeriod.SetTextLocalize(Consts.GetInstance().EXPLORE_PERIOD_FORMAT.F((object) this.rankingData.start_time.Value.Month, (object) this.rankingData.start_time.Value.Day, (object) this.rankingData.finish_time.Value.Month, (object) this.rankingData.finish_time.Value.Day));
      this.scrollBar.SetActive(false);
    }
  }

  public IEnumerator LoadResource()
  {
    if (Object.op_Equality((Object) this.statusScrollPrefab, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/explore033_Ranking/popup_ExploreRanking_Status_scroll").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.statusScrollPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public void setRankingInfo()
  {
    if (Object.op_Equality((Object) this.scrollGrid, (Object) null))
      return;
    this.scrollGrid.transform.Clear();
    this.prefabList.Clear();
    Explore033RankingPlayer rankingPlayerCatch = (Explore033RankingPlayer) null;
    int num = 0;
    foreach (WebAPI.Response.ExploreRankingRankingRankings ranking in this.rankingData.rankings)
    {
      rankingPlayerCatch = this.statusScrollPrefab.Clone(this.scrollGrid.transform).GetComponent<Explore033RankingPlayer>();
      this.prefabList.Add(rankingPlayerCatch);
      ++num;
    }
    if (this.prefabList.Count > 7)
    {
      this.scrollBar.SetActive(true);
      this.wrap = this.scrollGrid.GetComponent<UIGridCycle>();
      ((Behaviour) this.wrap).enabled = true;
      this.wrap.minIndex = -this.prefabList.Count + 1;
      this.wrap.onInitializeItem = (UIGridCycle.OnInitializeItem) ((obj, idx, realIdx) =>
      {
        obj.SetActive(true);
        rankingPlayerCatch = obj.GetComponent<Explore033RankingPlayer>();
        if (Mathf.Abs(realIdx) < this.prefabList.Count)
          rankingPlayerCatch.Initialize(this.rankingData.rankings[idx]);
        else
          obj.SetActive(false);
      });
      this.wrap.SortBasedOnScrollMovement();
      this.scroll = ((Component) this.scrollGrid.transform.parent).GetComponent<UIScrollView>();
      if (Object.op_Inequality((Object) this.scroll, (Object) null))
      {
        this.scrollLength = (float) (this.wrap.gridSize * Mathf.CeilToInt((float) this.prefabList.Count / 1f));
        this.bottomEmptyObj.localPosition = Vector2.op_Implicit(new Vector2(0.0f, (float) (-(double) this.scrollLength + 30.0)));
        this.scroll.ResetPosition();
        ((Component) this.wrap).transform.localPosition = Vector2.op_Implicit(Vector2.zero);
      }
      this.scroll.UpdatePosition();
    }
    else
    {
      this.scrollBar.SetActive(false);
      this.wrap = this.scrollGrid.GetComponent<UIGridCycle>();
      ((Behaviour) this.wrap).enabled = false;
      for (int index = 0; index < this.prefabList.Count; ++index)
      {
        ((Component) this.prefabList[index]).gameObject.transform.localPosition = Vector2.op_Implicit(new Vector2(0.0f, (float) -(index * 30 * 2)));
        this.prefabList[index].Initialize(this.rankingData.rankings[index]);
      }
      this.scroll = ((Component) this.scrollGrid.transform.parent).GetComponent<UIScrollView>();
      if (Object.op_Inequality((Object) this.scroll, (Object) null))
      {
        this.scrollLength = (float) (60 * Mathf.CeilToInt((float) this.prefabList.Count / 1f));
        this.bottomEmptyObj.localPosition = Vector2.op_Implicit(Vector2.zero);
        ((Component) this.wrap).transform.localPosition = Vector2.op_Implicit(new Vector2(0.0f, 20f));
      }
      this.scroll.ResetPosition();
    }
    this.dirPlayerRank.Initialize(new WebAPI.Response.ExploreRankingRankingRankings()
    {
      player_id = this.rankingData.my_ranking.player_id,
      ranking = this.rankingData.my_ranking.ranking,
      current_floor = this.rankingData.my_ranking.current_floor,
      name = this.rankingData.my_ranking.name,
      defeat_count = this.rankingData.my_ranking.defeat_count
    });
  }

  public void onRewardButton()
  {
    if (this.IsPushAndSet())
      return;
    Explore033RankingRewardScene.changeScene();
  }

  public void onCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.onCloseButton();
}
