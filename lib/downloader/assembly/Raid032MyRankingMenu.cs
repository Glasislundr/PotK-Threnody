// Decompiled with JetBrains decompiler
// Type: Raid032MyRankingMenu
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
public class Raid032MyRankingMenu : BackButtonMenuBase
{
  private Raid032MyRankingMenu.DisplayMode modeDisplay;
  private Raid032MyRankingStatus.StatusEnum modeStatus;
  private const int NUM_RANKING_KIND = 2;
  [SerializeField]
  private Raid032MyRankingScene scene;
  [SerializeField]
  private Transform dirRanking;
  private Raid032MyRankingPlayer mRanking;
  [SerializeField]
  private GameObject[] displayNodeTop = new GameObject[2];
  [SerializeField]
  private GameObject topNoneStatus;
  [SerializeField]
  private GameObject displayNodeBottom;
  [SerializeField]
  private GameObject[] topsRanking = new GameObject[2];
  [SerializeField]
  private UIGrid[] gridsRanking = new UIGrid[2];
  [SerializeField]
  private GameObject[] topsHierarchy = new GameObject[2];
  [SerializeField]
  private UIGrid[] gridsHierarchy = new UIGrid[2];
  [SerializeField]
  private SpreadColorButton[] statusButtons = new SpreadColorButton[2];
  [SerializeField]
  private GameObject topNewStatus;
  [SerializeField]
  private GameObject[] newStatus = new GameObject[2];
  [SerializeField]
  private int numInitializeRanking = 100;
  [SerializeField]
  private UILabel guildBossName;
  [SerializeField]
  private UIButton guildDamageRankingBtn;
  private GameObject prefabRankingRow;
  private GameObject prefabStatusRow;
  private GameObject prefabUnitIcon;
  private int[] myRanks = new int[2];
  private int?[] myPoints = new int?[2];
  private GuildRaid raid;
  private bool isInitialized;
  private int[] rankingCenters = new int[2]{ -1, -1 };
  private string coroutineRankingInit = string.Empty;
  private List<List<GameObject>> lstInitializeObjs = new List<List<GameObject>>(2);
  private Queue<GameObject> queInitialize = new Queue<GameObject>();
  private Dictionary<Raid032MyRankingStatus.StatusEnum, object> dicRanking = new Dictionary<Raid032MyRankingStatus.StatusEnum, object>();
  private const Raid032MyRankingMenu.DisplayMode FIRST_DISPLAY = Raid032MyRankingMenu.DisplayMode.Ranking;

  private bool isLoadedRanking(Raid032MyRankingStatus.StatusEnum eStatus)
  {
    return this.dicRanking.ContainsKey(eStatus);
  }

  private T getRanking<T>(Raid032MyRankingStatus.StatusEnum eStatus)
  {
    object ranking = (object) null;
    this.dicRanking.TryGetValue(eStatus, out ranking);
    return (T) ranking;
  }

  public IEnumerator coInitalize(GuildRaid raid)
  {
    this.isInitialized = false;
    this.raid = raid;
    this.dicRanking.Clear();
    ((IEnumerable<UIGrid>) this.gridsRanking).ForEach<UIGrid>((Action<UIGrid>) (g => ((Component) g).transform.Clear()));
    ((IEnumerable<UIGrid>) this.gridsHierarchy).ForEach<UIGrid>((Action<UIGrid>) (g => ((Component) g).transform.Clear()));
    ((IEnumerable<GameObject>) this.topsRanking).ToggleOnce(-1);
    this.lstInitializeObjs.Clear();
    for (int index = 0; index < 2; ++index)
      this.lstInitializeObjs.Add(new List<GameObject>());
    yield return (object) MasterData.LoadBattleStageEnemy(MasterData.BattleStage[raid.stage_id]);
    BattleStageEnemy boss = raid.getBoss();
    this.guildBossName.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_RAID_BOSS_NAME_LEVEL, (IDictionary) new Hashtable()
    {
      {
        (object) "bossName",
        (object) boss.unit.name
      },
      {
        (object) "bossLevel",
        (object) boss.level
      }
    }));
    Future<GameObject> ld;
    IEnumerator e;
    if (Object.op_Equality((Object) this.prefabRankingRow, (Object) null))
    {
      ld = Res.Prefabs.raid032_player_ranking.dir_raid_MyRankingStatus_scroll.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabRankingRow = ld.Result;
      if (Object.op_Equality((Object) this.prefabRankingRow, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.prefabUnitIcon, (Object) null))
    {
      ld = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabUnitIcon = ld.Result;
      if (Object.op_Equality((Object) this.prefabUnitIcon, (Object) null))
        yield break;
      else
        ld = (Future<GameObject>) null;
    }
    e = this.coChangeDisplayStatus(this.modeStatus, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Player player = SMManager.Get<Player>();
    PlayerAffiliation current = PlayerAffiliation.Current;
    PlayerUnit unit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerDeck[]>()[0].player_units).FirstOrDefault<PlayerUnit>();
    Raid032MyRankingPlayer.Data data = new Raid032MyRankingPlayer.Data(this.myRanks[(int) this.modeStatus], player, unit, current);
    if (Object.op_Equality((Object) this.mRanking, (Object) null))
      this.mRanking = this.prefabRankingRow.Clone(this.dirRanking).GetComponent<Raid032MyRankingPlayer>();
    e = this.mRanking.initialize(this.prefabUnitIcon, this.modeStatus, true, data, this.myPoints);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mRanking.coInitImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.changeDisplayMode(Raid032MyRankingMenu.DisplayMode.Ranking, true);
    this.changeDisplayStatus(this.modeStatus, true);
    this.guildDamageRankingBtn.onClick.Clear();
    this.guildDamageRankingBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      if (raid == null)
        return;
      Raid032GuildRankingScene.ChangeScene(raid.ID);
    })));
    this.isInitialized = true;
  }

  private IEnumerator coChangeDisplayStatus(
    Raid032MyRankingStatus.StatusEnum eStatus,
    bool isImmediate = false)
  {
    int index1 = (int) eStatus;
    switch (eStatus)
    {
      case Raid032MyRankingStatus.StatusEnum.Guild:
        this.statusButtons[0].SetColor(Color.white);
        this.statusButtons[1].SetColor(Color.gray);
        break;
      case Raid032MyRankingStatus.StatusEnum.Overall:
        this.statusButtons[1].SetColor(Color.white);
        this.statusButtons[0].SetColor(Color.gray);
        break;
    }
    if (index1 >= 0 && index1 < 2)
    {
      if (this.isLoadedRanking(eStatus))
      {
        this.changeDisplayStatus(eStatus, isImmediate);
      }
      else
      {
        if (this.isInitialized)
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 1;
          Singleton<CommonRoot>.GetInstance().isLoading = true;
        }
        IEnumerator e = this.coLoadRanking(eStatus);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        UIGrid grid = this.gridsRanking[index1];
        List<GameObject> lstinit = this.lstInitializeObjs[index1];
        int count = 0;
        int index2;
        GameObject go;
        switch (eStatus)
        {
          case Raid032MyRankingStatus.StatusEnum.Guild:
            WebAPI.Response.GuildraidRankingPlayer ranking1 = this.getRanking<WebAPI.Response.GuildraidRankingPlayer>(Raid032MyRankingStatus.StatusEnum.Guild);
            if (ranking1 != null && ranking1.damage_rankings_in_guild != null && ranking1.damage_rankings_in_guild.Length != 0)
            {
              this.myRanks[index1] = ranking1.player_score.damage_rank_in_guild.HasValue ? ranking1.player_score.damage_rank_in_guild.Value : 0;
              this.myPoints[index1] = this.myRanks[index1] > 0 ? new int?(ranking1.player_score.damage_score) : new int?();
              WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_guild[] damageRankingsInGuildArray = ranking1.damage_rankings_in_guild;
              for (index2 = 0; index2 < damageRankingsInGuildArray.Length; ++index2)
              {
                WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_guild dat = damageRankingsInGuildArray[index2];
                go = this.prefabRankingRow.Clone(((Component) grid).transform);
                e = go.GetComponent<Raid032MyRankingPlayer>().initialize(this.prefabUnitIcon, eStatus, true, new Raid032MyRankingPlayer.Data(dat));
                while (e.MoveNext())
                  yield return e.Current;
                e = (IEnumerator) null;
                if (this.numInitializeRanking > count)
                {
                  e = go.GetComponent<Raid032MyRankingPlayer>().coInitImage();
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
                else
                  lstinit.Add(go);
                ++count;
                go = (GameObject) null;
              }
              damageRankingsInGuildArray = (WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_guild[]) null;
              break;
            }
            break;
          case Raid032MyRankingStatus.StatusEnum.Overall:
            WebAPI.Response.GuildraidRankingPlayer ranking2 = this.getRanking<WebAPI.Response.GuildraidRankingPlayer>(Raid032MyRankingStatus.StatusEnum.Overall);
            if (ranking2 != null && ranking2.damage_rankings_in_all != null && ranking2.damage_rankings_in_all.Length != 0)
            {
              this.myRanks[index1] = ranking2.player_score.damage_rank_in_all.HasValue ? ranking2.player_score.damage_rank_in_all.Value : 0;
              this.myPoints[index1] = this.myRanks[index1] > 0 ? new int?(ranking2.player_score.damage_score) : new int?();
              WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_all[] damageRankingsInAllArray = ranking2.damage_rankings_in_all;
              for (index2 = 0; index2 < damageRankingsInAllArray.Length; ++index2)
              {
                WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_all dat = damageRankingsInAllArray[index2];
                go = this.prefabRankingRow.Clone(((Component) grid).transform);
                e = go.GetComponent<Raid032MyRankingPlayer>().initialize(this.prefabUnitIcon, eStatus, false, new Raid032MyRankingPlayer.Data(dat));
                while (e.MoveNext())
                  yield return e.Current;
                e = (IEnumerator) null;
                if (this.numInitializeRanking > count)
                {
                  e = go.GetComponent<Raid032MyRankingPlayer>().coInitImage();
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
                else
                  lstinit.Add(go);
                ++count;
                go = (GameObject) null;
              }
              damageRankingsInAllArray = (WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_all[]) null;
              break;
            }
            break;
        }
        if (((Component) grid).transform.childCount == 0)
        {
          e = this.prefabRankingRow.Clone(((Component) grid).transform).GetComponent<Raid032MyRankingPlayer>().initialize(this.prefabUnitIcon, eStatus, true);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        UIWidget uiw = this.topsRanking[index1].GetComponent<UIWidget>();
        if (Object.op_Inequality((Object) uiw, (Object) null))
        {
          if (this.modeDisplay != Raid032MyRankingMenu.DisplayMode.Ranking)
            this.displayNodeTop[0].SetActive(true);
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
          if (this.modeDisplay != Raid032MyRankingMenu.DisplayMode.Ranking)
            this.displayNodeTop[0].SetActive(false);
        }
        if (this.isInitialized)
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          this.mRanking.resetStatusValuse(this.myPoints);
          this.changeDisplayStatus(eStatus, isImmediate);
        }
      }
    }
  }

  private IEnumerator coLoadRanking(Raid032MyRankingStatus.StatusEnum eStatus)
  {
    if (!this.dicRanking.ContainsKey(eStatus))
    {
      Action<WebAPI.Response.UserError> userErrorCallback = (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      });
      object obj = (object) null;
      Future<WebAPI.Response.GuildraidRankingPlayer> future;
      IEnumerator e1;
      switch (eStatus)
      {
        case Raid032MyRankingStatus.StatusEnum.Guild:
          future = WebAPI.GuildraidRankingPlayer(this.raid.boss_id, this.raid.period_id, userErrorCallback);
          e1 = future.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          obj = (object) future.Result;
          future = (Future<WebAPI.Response.GuildraidRankingPlayer>) null;
          break;
        case Raid032MyRankingStatus.StatusEnum.Overall:
          future = WebAPI.GuildraidRankingPlayer(this.raid.boss_id, this.raid.period_id, userErrorCallback);
          e1 = future.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          obj = (object) future.Result;
          future = (Future<WebAPI.Response.GuildraidRankingPlayer>) null;
          break;
      }
      this.dicRanking.Add(eStatus, obj);
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

  private void changeDisplayMode(Raid032MyRankingMenu.DisplayMode dmode, bool isImmediate = false)
  {
    if (isImmediate)
    {
      this.modeDisplay = dmode;
      ((IEnumerable<GameObject>) this.displayNodeTop).ToggleOnce((int) dmode);
      if (!Object.op_Inequality((Object) this.topNewStatus, (Object) null))
        return;
      this.topNewStatus.SetActive(Raid032MyRankingMenu.DisplayMode.Status == dmode);
    }
    else
    {
      if (this.modeDisplay == dmode)
        return;
      List<GameObject> gameObjectList1 = new List<GameObject>()
      {
        this.displayNodeTop[(int) this.modeDisplay]
      };
      if (this.modeDisplay == Raid032MyRankingMenu.DisplayMode.Status && Object.op_Inequality((Object) this.topNewStatus, (Object) null))
        gameObjectList1.Add(this.topNewStatus);
      List<GameObject> gameObjectList2 = new List<GameObject>()
      {
        this.displayNodeTop[(int) dmode]
      };
      if (dmode == Raid032MyRankingMenu.DisplayMode.Status && Object.op_Inequality((Object) this.topNewStatus, (Object) null))
        gameObjectList2.Add(this.topNewStatus);
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
      this.modeDisplay = dmode;
    }
  }

  private void changeDisplayStatus(Raid032MyRankingStatus.StatusEnum eStatus, bool bInit = false)
  {
    if (!bInit && this.modeStatus == eStatus)
      return;
    this.mRanking.changeStatus(eStatus);
    if (this.myRanks != null)
      this.mRanking.changeDrawRank(this.myRanks[(int) eStatus]);
    if (bInit)
    {
      Raid032MyRankingMenu.resetActiveAnime(this.topsRanking, (int) eStatus);
      Raid032MyRankingMenu.resetActiveAnime(this.topsHierarchy, (int) eStatus);
    }
    else
    {
      GameObject gameObject1;
      GameObject gameObject2;
      if (this.modeDisplay == Raid032MyRankingMenu.DisplayMode.Ranking)
      {
        gameObject1 = this.topsRanking[(int) this.modeStatus];
        gameObject2 = this.topsRanking[(int) eStatus];
        Raid032MyRankingMenu.resetActiveAnime(this.topsHierarchy, (int) eStatus);
      }
      else
      {
        Raid032MyRankingMenu.resetActiveAnime(this.topsRanking, (int) eStatus);
        gameObject1 = this.topsHierarchy[(int) this.modeStatus];
        gameObject2 = this.topsHierarchy[(int) eStatus];
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
    this.modeStatus = eStatus;
    if (this.statusButtons == null)
      return;
    foreach (var data in ((IEnumerable<SpreadColorButton>) this.statusButtons).Select((v, i) => new
    {
      i = i,
      v = v
    }))
    {
      if (!Object.op_Equality((Object) data.v, (Object) null))
        ((UIButtonColor) data.v).isEnabled = (Raid032MyRankingStatus.StatusEnum) data.i != eStatus;
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

  public void onClickedGuild()
  {
    if (!this.isInitialized || this.IsPush || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    this.StartCoroutine(this.coChangeDisplayStatus(Raid032MyRankingStatus.StatusEnum.Guild));
  }

  public void onClickedOverall()
  {
    if (!this.isInitialized || this.IsPush || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    this.StartCoroutine(this.coChangeDisplayStatus(Raid032MyRankingStatus.StatusEnum.Overall));
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    if (this.modeDisplay == Raid032MyRankingMenu.DisplayMode.Ranking)
    {
      if (this.IsPushAndSet())
        return;
      this.backScene();
    }
    else
      this.changeDisplayMode(Raid032MyRankingMenu.DisplayMode.Ranking);
  }

  protected override void Update()
  {
    base.Update();
    if (!this.isInitialized)
      return;
    int modeStatus = (int) this.modeStatus;
    List<GameObject> lstInitializeObj = this.lstInitializeObjs.Count > modeStatus ? this.lstInitializeObjs[modeStatus] : (List<GameObject>) null;
    if (lstInitializeObj == null || lstInitializeObj.Count == 0)
      return;
    Transform transform = ((Component) this.gridsRanking[modeStatus]).transform;
    int num1 = this.centerIndex(((Component) ((Component) transform).transform.parent).GetComponent<UIScrollView>(), transform);
    if (this.rankingCenters[modeStatus] == num1)
      return;
    this.rankingCenters[modeStatus] = num1;
    int num2 = this.numInitializeRanking / 2 + this.numInitializeRanking % 2;
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
        this.queInitialize.Enqueue(lstInitializeObj[nullable.Value]);
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
    if (!string.IsNullOrEmpty(this.coroutineRankingInit))
      return;
    this.coroutineRankingInit = "coRankingInitAsync";
    this.StartCoroutine(this.coroutineRankingInit);
  }

  private void stopRankingInitAsync()
  {
    if (string.IsNullOrEmpty(this.coroutineRankingInit))
      return;
    this.StopCoroutine(this.coroutineRankingInit);
    this.coroutineRankingInit = string.Empty;
  }

  private IEnumerator coRankingInitAsync()
  {
    yield return (object) null;
    while (this.queInitialize.Count > 0)
    {
      GameObject gameObject = this.queInitialize.Dequeue();
      bool iswait = this.queInitialize.Count > 0;
      IEnumerator e = gameObject.GetComponent<Raid032MyRankingPlayer>().coInitImage();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      if (iswait)
        yield return (object) null;
    }
    this.coroutineRankingInit = string.Empty;
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

  public void OnButtonReward()
  {
    if (this.IsPushAndSet())
      return;
    Raid032RankingRewardConfScene.ChangeScene(true, this.raid);
  }

  public class Ranking
  {
    public Raid032MyRankingStatus status { get; private set; }

    public List<Raid032MyRankingPlayer.Data> players { get; private set; }
  }

  private enum DisplayMode
  {
    Ranking,
    Status,
    Num,
  }
}
