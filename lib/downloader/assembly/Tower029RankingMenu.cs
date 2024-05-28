// Decompiled with JetBrains decompiler
// Type: Tower029RankingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029RankingMenu : BackButtonMenuBase
{
  private Tower029RankingMenu.DisplayMode modeDisplay_;
  private Tower029RankingStatus.StatusEnum modeStatus_;
  private const int NUM_RANKING_KIND = 3;
  [SerializeField]
  private Tower029RankingScene scene_;
  [SerializeField]
  private Tower029RankingPlayer myRanking_;
  [SerializeField]
  private GameObject[] displayNodeTop_ = new GameObject[2];
  [SerializeField]
  private GameObject topNoneStatus_;
  [SerializeField]
  private GameObject[] displayNodeBottom_ = new GameObject[2];
  [SerializeField]
  private GameObject[] topsRanking_ = new GameObject[3];
  [SerializeField]
  private UIGrid[] gridsRanking_ = new UIGrid[3];
  [SerializeField]
  private GameObject[] topsHierarchy_ = new GameObject[3];
  [SerializeField]
  private UIGrid[] gridsHierarchy_ = new UIGrid[3];
  [SerializeField]
  private SpreadColorButton[] statusButtons_ = new SpreadColorButton[3];
  [SerializeField]
  private GameObject topNewStatus_;
  [SerializeField]
  private GameObject[] newStatus_ = new GameObject[3];
  [SerializeField]
  private int numInitializeRanking_ = 10;
  private GameObject prefabRankingRow_;
  private GameObject prefabStatusRow_;
  private GameObject prefabUnitIcon_;
  private int[] myRanks_ = new int[3];
  private int?[] myPoints_ = new int?[3];
  private int periodId_;
  private int towerId_;
  private bool isInitialized_;
  private bool isInitMyStatus_;
  private int[] rankingCenters_ = new int[3]{ -1, -1, -1 };
  private string coroutineRankingInit_ = string.Empty;
  private List<List<GameObject>> lstInitializeObjs_ = new List<List<GameObject>>(3);
  private Queue<GameObject> queInitialize_ = new Queue<GameObject>();
  private Dictionary<Tower029RankingStatus.StatusEnum, object> dicRanking_ = new Dictionary<Tower029RankingStatus.StatusEnum, object>();
  private const Tower029RankingMenu.DisplayMode FIRST_DISPLAY = Tower029RankingMenu.DisplayMode.Ranking;
  private const Tower029RankingStatus.StatusEnum FIRST_STATUS = Tower029RankingStatus.StatusEnum.Speed;

  private bool isLoadedRanking(Tower029RankingStatus.StatusEnum eStatus)
  {
    return this.dicRanking_.ContainsKey(eStatus);
  }

  private T getRanking<T>(Tower029RankingStatus.StatusEnum eStatus)
  {
    object ranking = (object) null;
    this.dicRanking_.TryGetValue(eStatus, out ranking);
    return (T) ranking;
  }

  public IEnumerator coInitalize(int tower_period, int tower_id)
  {
    this.isInitialized_ = false;
    this.isInitMyStatus_ = false;
    this.periodId_ = tower_period;
    this.towerId_ = tower_id;
    this.dicRanking_.Clear();
    ((IEnumerable<UIGrid>) this.gridsRanking_).ForEach<UIGrid>((Action<UIGrid>) (g => ((Component) g).transform.Clear()));
    ((IEnumerable<UIGrid>) this.gridsHierarchy_).ForEach<UIGrid>((Action<UIGrid>) (g => ((Component) g).transform.Clear()));
    ((IEnumerable<GameObject>) this.topsRanking_).ToggleOnce(-1);
    this.lstInitializeObjs_.Clear();
    for (int index = 0; index < 3; ++index)
      this.lstInitializeObjs_.Add(new List<GameObject>());
    Future<GameObject> ld;
    IEnumerator e;
    if (Object.op_Equality((Object) this.prefabRankingRow_, (Object) null))
    {
      ld = Res.Prefabs.tower029_ranking.tower_vscroll_810_12.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabRankingRow_ = ld.Result;
      if (Object.op_Equality((Object) this.prefabRankingRow_, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.prefabStatusRow_, (Object) null))
    {
      ld = Res.Prefabs.tower029_ranking.dir_tower_rankingStatus_scroll.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabStatusRow_ = ld.Result;
      if (Object.op_Equality((Object) this.prefabStatusRow_, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.prefabUnitIcon_, (Object) null))
    {
      ld = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabUnitIcon_ = ld.Result;
      if (Object.op_Equality((Object) this.prefabUnitIcon_, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    e = this.coChangeDisplayStatus(Tower029RankingStatus.StatusEnum.Speed, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.myRanking_.initialize(this.prefabUnitIcon_, this.modeStatus_, new Tower029RankingPlayer.Data(this.myRanks_[0], SMManager.Get<Player>(), ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerDeck[]>()[Persist.deckOrganized.Data.number].player_units).FirstOrDefault<PlayerUnit>()), this.myPoints_);
    e = this.myRanking_.coInitImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.changeDisplayMode(Tower029RankingMenu.DisplayMode.Ranking, true);
    this.changeDisplayStatus(Tower029RankingStatus.StatusEnum.Speed, true);
    this.isInitialized_ = true;
  }

  private IEnumerator coChangeDisplayStatus(
    Tower029RankingStatus.StatusEnum eStatus,
    bool isImmediate = false)
  {
    int index1 = (int) eStatus;
    if (index1 >= 0 && index1 < 3)
    {
      if (this.isLoadedRanking(eStatus))
      {
        this.changeDisplayStatus(eStatus, isImmediate);
      }
      else
      {
        if (this.isInitialized_)
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 1;
          Singleton<CommonRoot>.GetInstance().isLoading = true;
        }
        IEnumerator e = this.coLoadRanking(eStatus);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        UIGrid grid = this.gridsRanking_[index1];
        List<GameObject> lstinit = this.lstInitializeObjs_[index1];
        int count = 0;
        int index2;
        switch (eStatus)
        {
          case Tower029RankingStatus.StatusEnum.Speed:
            WebAPI.Response.TowerScoreRankingSpeed ranking1 = this.getRanking<WebAPI.Response.TowerScoreRankingSpeed>(Tower029RankingStatus.StatusEnum.Speed);
            if (ranking1 != null && ranking1.speed_rankings != null && ranking1.speed_rankings.Length != 0)
            {
              this.myRanks_[index1] = ranking1.player_score.speed_ranking_rank.HasValue ? ranking1.player_score.speed_ranking_rank.Value : 0;
              this.myPoints_[index1] = this.myRanks_[index1] > 0 ? new int?(ranking1.player_score.speed_ranking_score) : new int?();
              WebAPI.Response.TowerScoreRankingSpeedSpeed_rankings[] speedSpeedRankingsArray = ranking1.speed_rankings;
              for (index2 = 0; index2 < speedSpeedRankingsArray.Length; ++index2)
              {
                WebAPI.Response.TowerScoreRankingSpeedSpeed_rankings dat = speedSpeedRankingsArray[index2];
                GameObject gameObject = this.prefabRankingRow_.Clone(((Component) grid).transform);
                gameObject.GetComponent<Tower029RankingPlayer>().initialize(this.prefabUnitIcon_, eStatus, new Tower029RankingPlayer.Data(dat));
                if (this.numInitializeRanking_ > count)
                {
                  e = gameObject.GetComponent<Tower029RankingPlayer>().coInitImage();
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
                else
                  lstinit.Add(gameObject);
                ++count;
              }
              speedSpeedRankingsArray = (WebAPI.Response.TowerScoreRankingSpeedSpeed_rankings[]) null;
              break;
            }
            break;
          case Tower029RankingStatus.StatusEnum.Technic:
            WebAPI.Response.TowerScoreRankingTechnical ranking2 = this.getRanking<WebAPI.Response.TowerScoreRankingTechnical>(Tower029RankingStatus.StatusEnum.Technic);
            if (ranking2 != null && ranking2.technical_rankings != null && ranking2.technical_rankings.Length != 0)
            {
              this.myRanks_[index1] = ranking2.player_score.technical_ranking_rank.HasValue ? ranking2.player_score.technical_ranking_rank.Value : 0;
              this.myPoints_[index1] = this.myRanks_[index1] > 0 ? new int?(ranking2.player_score.technical_ranking_score) : new int?();
              WebAPI.Response.TowerScoreRankingTechnicalTechnical_rankings[] technicalRankingsArray = ranking2.technical_rankings;
              for (index2 = 0; index2 < technicalRankingsArray.Length; ++index2)
              {
                WebAPI.Response.TowerScoreRankingTechnicalTechnical_rankings dat = technicalRankingsArray[index2];
                GameObject gameObject = this.prefabRankingRow_.Clone(((Component) grid).transform);
                gameObject.GetComponent<Tower029RankingPlayer>().initialize(this.prefabUnitIcon_, eStatus, new Tower029RankingPlayer.Data(dat));
                if (this.numInitializeRanking_ > count)
                {
                  e = gameObject.GetComponent<Tower029RankingPlayer>().coInitImage();
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
                else
                  lstinit.Add(gameObject);
                ++count;
              }
              technicalRankingsArray = (WebAPI.Response.TowerScoreRankingTechnicalTechnical_rankings[]) null;
              break;
            }
            break;
          case Tower029RankingStatus.StatusEnum.Damage:
            WebAPI.Response.TowerScoreRankingDamage ranking3 = this.getRanking<WebAPI.Response.TowerScoreRankingDamage>(Tower029RankingStatus.StatusEnum.Damage);
            if (ranking3 != null && ranking3.damage_rankings != null && ranking3.damage_rankings.Length != 0)
            {
              this.myRanks_[index1] = ranking3.player_score.damage_ranking_rank.HasValue ? ranking3.player_score.damage_ranking_rank.Value : 0;
              this.myPoints_[index1] = this.myRanks_[index1] > 0 ? new int?(ranking3.player_score.damage_ranking_score) : new int?();
              WebAPI.Response.TowerScoreRankingDamageDamage_rankings[] damageDamageRankingsArray = ranking3.damage_rankings;
              for (index2 = 0; index2 < damageDamageRankingsArray.Length; ++index2)
              {
                WebAPI.Response.TowerScoreRankingDamageDamage_rankings dat = damageDamageRankingsArray[index2];
                GameObject gameObject = this.prefabRankingRow_.Clone(((Component) grid).transform);
                gameObject.GetComponent<Tower029RankingPlayer>().initialize(this.prefabUnitIcon_, eStatus, new Tower029RankingPlayer.Data(dat));
                if (this.numInitializeRanking_ > count)
                {
                  e = gameObject.GetComponent<Tower029RankingPlayer>().coInitImage();
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
                else
                  lstinit.Add(gameObject);
                ++count;
              }
              damageDamageRankingsArray = (WebAPI.Response.TowerScoreRankingDamageDamage_rankings[]) null;
              break;
            }
            break;
        }
        if (((Component) grid).transform.childCount == 0)
          this.prefabRankingRow_.Clone(((Component) grid).transform).GetComponent<Tower029RankingPlayer>().initialize(this.prefabUnitIcon_, eStatus);
        UIWidget uiw = this.topsRanking_[index1].GetComponent<UIWidget>();
        if (Object.op_Inequality((Object) uiw, (Object) null))
        {
          if (this.modeDisplay_ != Tower029RankingMenu.DisplayMode.Ranking)
            this.displayNodeTop_[0].SetActive(true);
          ((UIRect) uiw).alpha = 0.0f;
          ((Component) uiw).gameObject.SetActive(true);
          yield return (object) null;
        }
        this.repositionScrollView(grid);
        if (Object.op_Inequality((Object) uiw, (Object) null))
        {
          yield return (object) null;
          ((UIRect) uiw).alpha = 1f;
          ((Component) uiw).gameObject.SetActive(false);
          if (this.modeDisplay_ != Tower029RankingMenu.DisplayMode.Ranking)
            this.displayNodeTop_[0].SetActive(false);
        }
        if (this.isInitialized_)
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          this.myRanking_.resetStatusValuse(this.myPoints_);
          this.changeDisplayStatus(eStatus, isImmediate);
        }
      }
    }
  }

  private IEnumerator coLoadRanking(Tower029RankingStatus.StatusEnum eStatus)
  {
    if (!this.dicRanking_.ContainsKey(eStatus))
    {
      Action<WebAPI.Response.UserError> userErrorCallback = (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      });
      object obj = (object) null;
      IEnumerator e1;
      switch (eStatus)
      {
        case Tower029RankingStatus.StatusEnum.Speed:
          Future<WebAPI.Response.TowerScoreRankingSpeed> future1 = WebAPI.TowerScoreRankingSpeed(this.periodId_, this.towerId_, userErrorCallback);
          e1 = future1.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          obj = (object) future1.Result;
          future1 = (Future<WebAPI.Response.TowerScoreRankingSpeed>) null;
          break;
        case Tower029RankingStatus.StatusEnum.Technic:
          Future<WebAPI.Response.TowerScoreRankingTechnical> future2 = WebAPI.TowerScoreRankingTechnical(this.periodId_, this.towerId_, userErrorCallback);
          e1 = future2.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          obj = (object) future2.Result;
          future2 = (Future<WebAPI.Response.TowerScoreRankingTechnical>) null;
          break;
        case Tower029RankingStatus.StatusEnum.Damage:
          Future<WebAPI.Response.TowerScoreRankingDamage> future3 = WebAPI.TowerScoreRankingDamage(this.periodId_, this.towerId_, userErrorCallback);
          e1 = future3.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          obj = (object) future3.Result;
          future3 = (Future<WebAPI.Response.TowerScoreRankingDamage>) null;
          break;
      }
      this.dicRanking_.Add(eStatus, obj);
    }
  }

  private void repositionScrollView(UIGrid grid)
  {
    grid.Reposition();
    UIScrollView component = ((Component) ((Component) grid).transform.parent).GetComponent<UIScrollView>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.ResetPosition();
  }

  private void changeDisplayMode(Tower029RankingMenu.DisplayMode dmode, bool isImmediate = false)
  {
    if (isImmediate)
    {
      this.modeDisplay_ = dmode;
      ((IEnumerable<GameObject>) this.displayNodeTop_).ToggleOnce((int) dmode);
      ((IEnumerable<GameObject>) this.displayNodeBottom_).ToggleOnce((int) dmode);
      if (!Object.op_Inequality((Object) this.topNewStatus_, (Object) null))
        return;
      this.topNewStatus_.SetActive(Tower029RankingMenu.DisplayMode.Status == dmode);
    }
    else
    {
      if (this.modeDisplay_ == dmode)
        return;
      List<GameObject> gameObjectList1 = new List<GameObject>()
      {
        this.displayNodeTop_[(int) this.modeDisplay_],
        this.displayNodeBottom_[(int) this.modeDisplay_]
      };
      if (this.modeDisplay_ == Tower029RankingMenu.DisplayMode.Status && Object.op_Inequality((Object) this.topNewStatus_, (Object) null))
        gameObjectList1.Add(this.topNewStatus_);
      List<GameObject> gameObjectList2 = new List<GameObject>()
      {
        this.displayNodeTop_[(int) dmode],
        this.displayNodeBottom_[(int) dmode]
      };
      if (dmode == Tower029RankingMenu.DisplayMode.Status && Object.op_Inequality((Object) this.topNewStatus_, (Object) null))
        gameObjectList2.Add(this.topNewStatus_);
      foreach (GameObject gameObject in gameObjectList1)
      {
        NGTweenParts component = gameObject.GetComponent<NGTweenParts>();
        if (Object.op_Inequality((Object) component, (Object) null))
          component.forceActive(false);
        else
          gameObject.SetActive(false);
      }
      foreach (GameObject gameObject in gameObjectList2)
      {
        NGTweenParts component = gameObject.GetComponent<NGTweenParts>();
        if (Object.op_Inequality((Object) component, (Object) null))
          component.forceActive(true);
        else
          gameObject.SetActive(true);
      }
      this.modeDisplay_ = dmode;
    }
  }

  private void changeDisplayStatus(Tower029RankingStatus.StatusEnum eStatus, bool bInit = false)
  {
    if (!bInit && this.modeStatus_ == eStatus)
      return;
    this.myRanking_.changeStatus(eStatus);
    if (this.myRanks_ != null)
      this.myRanking_.changeDrawRank(this.myRanks_[(int) eStatus]);
    if (bInit)
    {
      Tower029RankingMenu.resetActiveAnime(this.topsRanking_, (int) eStatus);
      Tower029RankingMenu.resetActiveAnime(this.topsHierarchy_, (int) eStatus);
    }
    else
    {
      GameObject gameObject1;
      GameObject gameObject2;
      if (this.modeDisplay_ == Tower029RankingMenu.DisplayMode.Ranking)
      {
        gameObject1 = this.topsRanking_[(int) this.modeStatus_];
        gameObject2 = this.topsRanking_[(int) eStatus];
        Tower029RankingMenu.resetActiveAnime(this.topsHierarchy_, (int) eStatus);
      }
      else
      {
        Tower029RankingMenu.resetActiveAnime(this.topsRanking_, (int) eStatus);
        gameObject1 = this.topsHierarchy_[(int) this.modeStatus_];
        gameObject2 = this.topsHierarchy_[(int) eStatus];
      }
      NGTweenParts component1 = gameObject1.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) component1, (Object) null))
        component1.forceActive(false);
      else
        gameObject1.SetActive(false);
      NGTweenParts component2 = gameObject2.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        component2.forceActive(true);
      else
        gameObject2.SetActive(true);
    }
    this.modeStatus_ = eStatus;
    if (this.statusButtons_ == null)
      return;
    foreach (var data in ((IEnumerable<SpreadColorButton>) this.statusButtons_).Select((v, i) => new
    {
      i = i,
      v = v
    }))
    {
      if (!Object.op_Equality((Object) data.v, (Object) null))
        ((UIButtonColor) data.v).isEnabled = (Tower029RankingStatus.StatusEnum) data.i != eStatus;
    }
  }

  private static void resetActiveAnime(GameObject[] objs, int index)
  {
    ((IEnumerable<GameObject>) objs).Select((go, inx) => new
    {
      inx = inx,
      go = go
    }).ForEach(dat =>
    {
      NGTweenParts component = dat.go.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.resetActive(dat.inx == index);
      else
        dat.go.SetActive(dat.inx == index);
    });
  }

  public void onClickedSpeed()
  {
    if (!this.isInitialized_ || this.IsPush || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    this.StartCoroutine(this.coChangeDisplayStatus(Tower029RankingStatus.StatusEnum.Speed));
  }

  public void onClickedTechnic()
  {
    if (!this.isInitialized_ || this.IsPush || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    this.StartCoroutine(this.coChangeDisplayStatus(Tower029RankingStatus.StatusEnum.Technic));
  }

  public void onClickedDamage()
  {
    if (!this.isInitialized_ || this.IsPush || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    this.StartCoroutine(this.coChangeDisplayStatus(Tower029RankingStatus.StatusEnum.Damage));
  }

  public void onClickedMyStatus()
  {
    if (this.IsPush || Singleton<CommonRoot>.GetInstance().isLoading || this.modeDisplay_ != Tower029RankingMenu.DisplayMode.Ranking)
      return;
    this.StartCoroutine(this.coChangeMyStatus());
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    if (this.modeDisplay_ == Tower029RankingMenu.DisplayMode.Ranking)
    {
      if (this.IsPushAndSet())
        return;
      this.backScene();
    }
    else
      this.changeDisplayMode(Tower029RankingMenu.DisplayMode.Ranking);
  }

  private IEnumerator coChangeMyStatus()
  {
    if (this.isInitMyStatus_)
    {
      this.changeDisplayMode(Tower029RankingMenu.DisplayMode.Status);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.TowerScoreDetail> future = WebAPI.TowerScoreDetail(this.periodId_, this.towerId_, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        WebAPI.Response.TowerScoreDetail detail = future.Result;
        bool isempty = true;
        this.displayNodeTop_[1].SetActive(true);
        ((IEnumerable<GameObject>) this.topsHierarchy_).ToggleOnce(-1);
        for (int ng = 0; ng < this.gridsHierarchy_.Length; ++ng)
        {
          UIGrid grid = this.gridsHierarchy_[ng];
          Tower029RankingStatus.StatusEnum initStatus = (Tower029RankingStatus.StatusEnum) ng;
          bool isnew = false;
          switch (initStatus)
          {
            case Tower029RankingStatus.StatusEnum.Speed:
              foreach (WebAPI.Response.TowerScoreDetailTower_scores detailTowerScores in ((IEnumerable<WebAPI.Response.TowerScoreDetailTower_scores>) detail.tower_scores).Where<WebAPI.Response.TowerScoreDetailTower_scores>((Func<WebAPI.Response.TowerScoreDetailTower_scores, bool>) (s => s.speed_score > 0)).ToList<WebAPI.Response.TowerScoreDetailTower_scores>())
              {
                isnew |= detailTowerScores.is_new_speed;
                bool[] newflags = new bool[3]
                {
                  detailTowerScores.is_new_speed,
                  false,
                  false
                };
                int?[] nullableArray = new int?[3];
                nullableArray[0] = new int?(detailTowerScores.turn_count);
                int?[] statusvalues = nullableArray;
                e1 = this.prefabStatusRow_.Clone(((Component) grid).transform).GetComponent<Tower029RankingHierarchy>().coInitialize(detailTowerScores.floor, initStatus, newflags, statusvalues, detailTowerScores.speed_score);
                while (e1.MoveNext())
                  yield return e1.Current;
                e1 = (IEnumerator) null;
              }
              break;
            case Tower029RankingStatus.StatusEnum.Technic:
              foreach (WebAPI.Response.TowerScoreDetailTower_scores detailTowerScores in ((IEnumerable<WebAPI.Response.TowerScoreDetailTower_scores>) detail.tower_scores).Where<WebAPI.Response.TowerScoreDetailTower_scores>((Func<WebAPI.Response.TowerScoreDetailTower_scores, bool>) (s => s.technical_score > 0)).ToList<WebAPI.Response.TowerScoreDetailTower_scores>())
              {
                isnew |= detailTowerScores.is_new_technical;
                bool[] newflags = new bool[3]
                {
                  false,
                  detailTowerScores.is_new_technical,
                  false
                };
                int?[] nullableArray = new int?[3];
                nullableArray[1] = new int?(detailTowerScores.unit_death_count);
                int?[] statusvalues = nullableArray;
                e1 = this.prefabStatusRow_.Clone(((Component) grid).transform).GetComponent<Tower029RankingHierarchy>().coInitialize(detailTowerScores.floor, initStatus, newflags, statusvalues, detailTowerScores.technical_score);
                while (e1.MoveNext())
                  yield return e1.Current;
                e1 = (IEnumerator) null;
              }
              break;
            case Tower029RankingStatus.StatusEnum.Damage:
              foreach (WebAPI.Response.TowerScoreDetailTower_scores detailTowerScores in ((IEnumerable<WebAPI.Response.TowerScoreDetailTower_scores>) detail.tower_scores).Where<WebAPI.Response.TowerScoreDetailTower_scores>((Func<WebAPI.Response.TowerScoreDetailTower_scores, bool>) (s => s.damage_score > 0)).ToList<WebAPI.Response.TowerScoreDetailTower_scores>())
              {
                isnew |= detailTowerScores.is_new_damage;
                bool[] newflags = new bool[3]
                {
                  false,
                  false,
                  detailTowerScores.is_new_damage
                };
                int?[] nullableArray = new int?[3];
                nullableArray[2] = new int?(detailTowerScores.overkill_damage);
                int?[] statusvalues = nullableArray;
                e1 = this.prefabStatusRow_.Clone(((Component) grid).transform).GetComponent<Tower029RankingHierarchy>().coInitialize(detailTowerScores.floor, initStatus, newflags, statusvalues, detailTowerScores.damage_score);
                while (e1.MoveNext())
                  yield return e1.Current;
                e1 = (IEnumerator) null;
              }
              break;
          }
          this.topsHierarchy_[ng].SetActive(true);
          this.repositionScrollView(grid);
          yield return (object) null;
          this.topsHierarchy_[ng].SetActive(false);
          isempty &= ((Component) grid).transform.childCount == 0;
          if (this.newStatus_ != null && this.newStatus_.Length > ng && Object.op_Inequality((Object) this.newStatus_[ng], (Object) null))
            this.newStatus_[ng].SetActive(isnew);
          grid = (UIGrid) null;
        }
        if (Object.op_Inequality((Object) this.topNoneStatus_, (Object) null))
          this.topNoneStatus_.SetActive(isempty);
        this.displayNodeTop_[1].SetActive(false);
        ((IEnumerable<GameObject>) this.topsHierarchy_).ToggleOnce((int) this.modeStatus_);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        this.isInitMyStatus_ = true;
        this.changeDisplayMode(Tower029RankingMenu.DisplayMode.Status);
      }
    }
  }

  protected override void Update()
  {
    base.Update();
    if (!this.isInitialized_)
      return;
    int modeStatus = (int) this.modeStatus_;
    List<GameObject> lstInitializeObj = this.lstInitializeObjs_.Count > modeStatus ? this.lstInitializeObjs_[modeStatus] : (List<GameObject>) null;
    if (lstInitializeObj == null || lstInitializeObj.Count == 0)
      return;
    Transform transform = ((Component) this.gridsRanking_[modeStatus]).transform;
    int num1 = this.centerIndex(((Component) ((Component) transform).transform.parent).GetComponent<UIScrollView>(), transform);
    if (this.rankingCenters_[modeStatus] == num1)
      return;
    this.rankingCenters_[modeStatus] = num1;
    int num2 = this.numInitializeRanking_ / 2 + this.numInitializeRanking_ % 2;
    int num3 = num1 + num2 + 1;
    int num4 = num1 - num2;
    if (num4 < 0)
      num4 = 0;
    if (num3 > transform.childCount)
      num3 = transform.childCount;
    bool flag = false;
    for (; num4 < num3; ++num4)
    {
      GameObject c = ((Component) transform.GetChild(num4)).gameObject;
      int? nullable = lstInitializeObj.FirstIndexOrNull<GameObject>((Func<GameObject, bool>) (obj => Object.op_Equality((Object) obj, (Object) c)));
      if (nullable.HasValue)
      {
        this.queInitialize_.Enqueue(lstInitializeObj[nullable.Value]);
        lstInitializeObj.RemoveAt(nullable.Value);
        flag = true;
      }
    }
    if (!flag)
      return;
    this.startRankingInitAsync();
  }

  private void startRankingInitAsync()
  {
    if (!string.IsNullOrEmpty(this.coroutineRankingInit_))
      return;
    this.coroutineRankingInit_ = "coRankingInitAsync";
    this.StartCoroutine(this.coroutineRankingInit_);
  }

  private void stopRankingInitAsync()
  {
    if (string.IsNullOrEmpty(this.coroutineRankingInit_))
      return;
    this.StopCoroutine(this.coroutineRankingInit_);
    this.coroutineRankingInit_ = string.Empty;
  }

  private IEnumerator coRankingInitAsync()
  {
    yield return (object) null;
    while (this.queInitialize_.Count > 0)
    {
      GameObject gameObject = this.queInitialize_.Dequeue();
      bool iswait = this.queInitialize_.Count > 0;
      IEnumerator e = gameObject.GetComponent<Tower029RankingPlayer>().coInitImage();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      if (iswait)
        yield return (object) null;
    }
    this.coroutineRankingInit_ = string.Empty;
  }

  private void OnDestroy() => this.stopRankingInitAsync();

  private int centerIndex(UIScrollView scrollView, Transform itemNode)
  {
    Vector3[] worldCorners = ((UIRect) scrollView.panel).worldCorners;
    Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Multiply(Vector3.op_Addition(worldCorners[2], worldCorners[0]), 0.5f), Vector3.op_Multiply(scrollView.currentMomentum, scrollView.momentumAmount * 0.1f));
    float num1 = float.MaxValue;
    int num2 = 0;
    int num3 = 0;
    for (int childCount = itemNode.childCount; num3 < childCount; ++num3)
    {
      float num4 = Vector3.SqrMagnitude(Vector3.op_Subtraction(itemNode.GetChild(num3).position, vector3));
      if ((double) num4 < (double) num1)
      {
        num1 = num4;
        num2 = num3;
      }
    }
    return num2;
  }

  public class Ranking
  {
    public Tower029RankingStatus status_ { get; private set; }

    public List<Tower029RankingPlayer.Data> players_ { get; private set; }
  }

  private enum DisplayMode
  {
    Ranking,
    Status,
    Num,
  }
}
