// Decompiled with JetBrains decompiler
// Type: DailyMissionWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using MissionData;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/DailyMission/DailyMissionWindow")]
public class DailyMissionWindow : BackButtonPopupWindow
{
  [SerializeField]
  public DailyMissionList[] dirMissionListObject;
  [SerializeField]
  private GameObject dailyQuest;
  [SerializeField]
  private List<DailyMissionPointRewardItemController> dailyMissionPointRewardItems;
  [SerializeField]
  private UIButton weeelyMissionButton;
  [SerializeField]
  private GameObject weeklyMissionButtonBadge;
  [SerializeField]
  private UIButton BulkReceiveButton;
  [SerializeField]
  private UISprite BulkReceiveSprite;
  [SerializeField]
  private UILabel pointAcquiredTodayText;
  [SerializeField]
  private UIProgressBar dailyMissionPointGauge;
  [SerializeField]
  private GameObject[] dirBadge;
  [SerializeField]
  private SpreadColorButton[] tabBtn;
  [SerializeField]
  [Tooltip("ギルドミッション表示時、ギルド未所属時に表示")]
  private GameObject objGuildNotAffiliation;
  [SerializeField]
  [Tooltip("objGuildNotAffiliationを表示する際に調整する")]
  private UIWidget widgetGuildMain;
  [SerializeField]
  [Tooltip("objGuildNotAffiliationを表示する際に調整する")]
  private int objGuildNotAffiliationOffsetY = -274;
  private int? originalGuildMainAnchorAbsolute;
  private HelpCategory helpCategory;
  private List<PointRewardBox> weeklyPointRewardBoxList_;
  private int currentWeeklyPoint;
  private List<PointRewardBox> dailyPointRewardBoxList_;
  private int currentDailyPoint;
  private IMissionAchievement[] oldPlayerDailyMissions;
  private IMissionAchievement[] oldPlayerGuildMissions;
  private List<int> notReceiveList = new List<int>();
  private List<int> received_missionsList;
  private List<int> not_received_pointrewards = new List<int>();
  private List<DailyMission0272Panel.RewardViewModel> rewardModelList = new List<DailyMission0272Panel.RewardViewModel>();
  private List<PointReward> resultPointRewards = new List<PointReward>();
  private bool isProcessingMissionList;
  private Dictionary<DailyMissionWindow.ScrollType, MissionType> scrollMissionType = new Dictionary<DailyMissionWindow.ScrollType, MissionType>()
  {
    {
      DailyMissionWindow.ScrollType.Daily,
      MissionType.daily
    },
    {
      DailyMissionWindow.ScrollType.Game,
      MissionType.game
    },
    {
      DailyMissionWindow.ScrollType.Period,
      MissionType.period
    },
    {
      DailyMissionWindow.ScrollType.Guild,
      MissionType.guild
    }
  };
  private DailyMissionWindow.ScrollType scrollType;
  private bool[] isScrollInit = new bool[Enum.GetValues(typeof (DailyMissionWindow.ScrollType)).Length];
  private Dictionary<int, IMissionAchievement[]> missionDic;
  private DailyMissionController controller;

  public bool isGuildNotAffiliation { get; private set; }

  private List<PointRewardBox> weeklyPointRewardBoxList
  {
    get
    {
      if (this.weeklyPointRewardBoxList_ == null)
        this.resetPointRewardBoxes();
      return this.weeklyPointRewardBoxList_;
    }
  }

  private List<PointRewardBox> dailyPointRewardBoxList
  {
    get
    {
      if (this.dailyPointRewardBoxList_ == null)
        this.resetPointRewardBoxes();
      return this.dailyPointRewardBoxList_;
    }
  }

  public bool IsPush { get; set; }

  public bool IsPushAndSet()
  {
    if (this.IsPush)
      return true;
    this.IsPush = true;
    return false;
  }

  private IEnumerator DisplayMissionList(DailyMissionWindow.ScrollType type)
  {
    this.isProcessingMissionList = true;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    int index1 = (int) type;
    for (int index2 = 0; index2 < this.tabBtn.Length; ++index2)
      ((UIButtonColor) this.tabBtn[index2]).isEnabled = index2 != index1;
    ((IEnumerable<DailyMissionList>) this.dirMissionListObject).ForEach<DailyMissionList>((Action<DailyMissionList>) (x => x.SetVisible(false)));
    this.dailyQuest.SetActive(type == DailyMissionWindow.ScrollType.Daily);
    ((Component) this.weeelyMissionButton).gameObject.SetActive(type == DailyMissionWindow.ScrollType.Daily);
    DailyMissionList targetList = this.dirMissionListObject[index1];
    if (!targetList.IsCreated)
    {
      IEnumerator e = targetList.Create(this.controller);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    while (!targetList.IsCreated)
      yield return (object) null;
    targetList.SetVisible(true);
    targetList.scrollView.SetDragAmount(0.0f, ((UIProgressBar) targetList.scrollBar).value, true);
    if (!this.isScrollInit[(int) type])
    {
      this.isScrollInit[(int) type] = true;
      targetList.ResetPosition();
    }
    if (Object.op_Inequality((Object) this.objGuildNotAffiliation, (Object) null))
      this.objGuildNotAffiliation.SetActive(type == DailyMissionWindow.ScrollType.Guild && this.isGuildNotAffiliation);
    this.updateBulkReceiveButton(type, true);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    this.isProcessingMissionList = false;
  }

  private void updateBulkReceiveButton(DailyMissionWindow.ScrollType type, bool bInitialize)
  {
    bool flag = this.BulkReceiveFlag(type);
    if (!bInitialize && ((UIButtonColor) this.BulkReceiveButton).isEnabled == flag)
      return;
    ((UIButtonColor) this.BulkReceiveButton).isEnabled = flag;
    if (flag)
      ((UIWidget) this.BulkReceiveSprite).color = ((UIButtonColor) this.BulkReceiveButton).defaultColor;
    else
      ((UIWidget) this.BulkReceiveSprite).color = ((UIButtonColor) this.BulkReceiveButton).disabledColor;
  }

  private bool BulkReceiveFlag(DailyMissionWindow.ScrollType type)
  {
    if (type != DailyMissionWindow.ScrollType.Daily)
      return this.dirMissionListObject[(int) type].BulkReceiveFlag();
    if (this.dirMissionListObject[(int) type].BulkReceiveFlag())
      return true;
    PlayerDailyMissionPointDaily pointDaily = SMManager.Get<PlayerDailyMissionPoint>().daily;
    return this.dailyPointRewardBoxList.Any<PointRewardBox>((Func<PointRewardBox, bool>) (x =>
    {
      int point1 = x.point;
      int? point2 = pointDaily.point;
      int valueOrDefault = point2.GetValueOrDefault();
      return point1 <= valueOrDefault & point2.HasValue && !((IEnumerable<int?>) pointDaily.received_rewards).Contains<int?>(new int?(x.ID));
    }));
  }

  public IEnumerator InitMissionList(
    IMissionAchievement[] playerDailyMissions,
    int[] types,
    bool isTabBtnDisplay)
  {
    DailyMissionWindow targetWindow = this;
    if (types.Length == 1 && types[0] == 3 || types.Length >= 2)
      targetWindow.oldPlayerGuildMissions = playerDailyMissions;
    if (types.Length == 1 && types[0] != 3 || types.Length >= 2)
      targetWindow.oldPlayerDailyMissions = playerDailyMissions;
    ((IEnumerable<bool>) targetWindow.isScrollInit).ForEach<bool>((Action<bool>) (x => x = false));
    HashSet<MissionType> missionTargets = new HashSet<MissionType>(((IEnumerable<int>) types).Select<int, MissionType>((Func<int, MissionType>) (i => this.scrollMissionType[(DailyMissionWindow.ScrollType) i])));
    Dictionary<MissionType, IMissionAchievement[]> dictionary = ((IEnumerable<IMissionAchievement>) playerDailyMissions).Where<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.mission != null && missionTargets.Contains(x.mission.missionType) && x.isShow)).GroupBy<IMissionAchievement, MissionType>((Func<IMissionAchievement, MissionType>) (x => x.mission.missionType)).ToDictionary<IGrouping<MissionType, IMissionAchievement>, MissionType, IMissionAchievement[]>((Func<IGrouping<MissionType, IMissionAchievement>, MissionType>) (k => k.Key), (Func<IGrouping<MissionType, IMissionAchievement>, IMissionAchievement[]>) (v => this.sortMission(v.Key, (IEnumerable<IMissionAchievement>) v).ToArray<IMissionAchievement>()));
    if (targetWindow.missionDic == null)
      targetWindow.missionDic = new Dictionary<int, IMissionAchievement[]>();
    foreach (int type in types)
    {
      IMissionAchievement[] source;
      if (!dictionary.TryGetValue(targetWindow.scrollMissionType[(DailyMissionWindow.ScrollType) type], out source))
        source = new IMissionAchievement[0];
      targetWindow.missionDic[type] = source;
      targetWindow.dirBadge[type].SetActive(((IEnumerable<IMissionAchievement>) source).Any<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.isCleared && !x.isReceived)));
    }
    ((IEnumerable<DailyMissionList>) targetWindow.dirMissionListObject).ForEach<DailyMissionList>((Action<DailyMissionList>) (x => x.SetVisible(true)));
    int[] numArray = types;
    for (int index = 0; index < numArray.Length; ++index)
    {
      int index1 = numArray[index];
      targetWindow.dirMissionListObject[index1].Initialize(targetWindow, index1, targetWindow.controller.panelPrefab, targetWindow.missionDic[index1]);
      if (isTabBtnDisplay)
        targetWindow.dirBadge[index1].SetActive(((IEnumerable<IMissionAchievement>) targetWindow.missionDic[index1]).Any<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.isCleared && !x.isReceived)));
      if (!PerformanceConfig.GetInstance().IsTuningMissionInitialize)
      {
        IEnumerator e = targetWindow.dirMissionListObject[index1].Create(targetWindow.controller);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    numArray = (int[]) null;
    targetWindow.isProcessingMissionList = false;
    targetWindow.StartCoroutine(targetWindow.DisplayMissionList(targetWindow.scrollType));
  }

  private IEnumerable<IMissionAchievement> sortMission(
    MissionType missionType,
    IEnumerable<IMissionAchievement> sortDat)
  {
    return missionType != MissionType.guild ? sortDat : (IEnumerable<IMissionAchievement>) sortDat.OrderBy<IMissionAchievement, int>((Func<IMissionAchievement, int>) (x =>
    {
      if (x.isReceived)
        return 3;
      if (x.isCleared)
        return 0;
      return x.isOwnCleared ? 1 : 2;
    }));
  }

  public IEnumerator Init(DailyMissionController controller, bool resetCurrentTab = true)
  {
    DailyMissionWindow dailyMissionWindow1 = this;
    dailyMissionWindow1.controller = controller;
    dailyMissionWindow1.dailyQuest.SetActive(false);
    ((Component) dailyMissionWindow1.weeelyMissionButton).gameObject.SetActive(false);
    if (dailyMissionWindow1.helpCategory == null)
      dailyMissionWindow1.helpCategory = Array.Find<HelpCategory>(MasterData.HelpCategoryList, (Predicate<HelpCategory>) (x => x.name == "ミッション"));
    foreach (DailyMissionList dailyMissionList in dailyMissionWindow1.dirMissionListObject)
      dailyMissionList.Clear();
    IEnumerator e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Future<WebAPI.Response.DailymissionIndex> future = WebAPI.DailymissionIndex((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      DailyMissionWindow dailyMissionWindow2 = dailyMissionWindow1;
      PlayerAffiliation current = PlayerAffiliation.Current;
      int num = (current != null ? (current.isGuildMember() ? 1 : 0) : 0) == 0 ? 1 : 0;
      dailyMissionWindow2.isGuildNotAffiliation = num != 0;
      if (Object.op_Inequality((Object) dailyMissionWindow1.widgetGuildMain, (Object) null))
      {
        if (!dailyMissionWindow1.originalGuildMainAnchorAbsolute.HasValue)
          dailyMissionWindow1.originalGuildMainAnchorAbsolute = new int?(((UIRect) dailyMissionWindow1.widgetGuildMain).topAnchor.absolute);
        ((UIRect) dailyMissionWindow1.widgetGuildMain).topAnchor.absolute = dailyMissionWindow1.isGuildNotAffiliation ? dailyMissionWindow1.objGuildNotAffiliationOffsetY : dailyMissionWindow1.originalGuildMainAnchorAbsolute.Value;
      }
      List<IMissionAchievement> lstMission = ((IEnumerable<PlayerDailyMissionAchievement>) future.Result.player_daily_missions).Select<PlayerDailyMissionAchievement, IMissionAchievement>((Func<PlayerDailyMissionAchievement, IMissionAchievement>) (dm => Util.Create(dm))).ToList<IMissionAchievement>();
      GuildMissionInfo[] playerGuildMissions = future.Result.player_guild_missions;
      if ((playerGuildMissions != null ? playerGuildMissions.Length : 0) > 0)
        lstMission.AddRange(((IEnumerable<GuildMissionInfo>) future.Result.player_guild_missions).Select<GuildMissionInfo, IMissionAchievement>((Func<GuildMissionInfo, IMissionAchievement>) (gm => Util.Create(gm))));
      if (resetCurrentTab)
        dailyMissionWindow1.scrollType = DailyMissionWindow.ScrollType.Daily;
      int[] types = new int[4]{ 0, 1, 2, 3 };
      e1 = dailyMissionWindow1.InitMissionList(lstMission.ToArray(), types, true);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      dailyMissionWindow1.InitializePointRewardSection(lstMission.ToArray());
    }
  }

  public void InitializePointRewardSection(IMissionAchievement[] playerDailyMissions)
  {
    this.resetPointRewardBoxes();
    PlayerDailyMissionPointDaily daily = SMManager.Get<PlayerDailyMissionPoint>().daily;
    this.currentDailyPoint = daily.point.Value;
    this.pointAcquiredTodayText.SetTextLocalize(this.currentDailyPoint);
    int num = ((IEnumerable<IMissionAchievement>) playerDailyMissions).Where<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.mission.missionType == MissionType.daily)).Sum<IMissionAchievement>((Func<IMissionAchievement, int>) (x => x.mission.point));
    int?[] rewardPoints = new int?[this.dailyMissionPointRewardItems.Count + 2];
    rewardPoints[0] = new int?(0);
    rewardPoints[rewardPoints.Length - 1] = new int?(num);
    for (int i = 0; i < this.dailyMissionPointRewardItems.Count; i++)
    {
      PointRewardBox pointRewardBox = this.dailyPointRewardBoxList.FirstOrDefault<PointRewardBox>((Func<PointRewardBox, bool>) (x => x.box_type == i + 1));
      if (pointRewardBox != null)
      {
        ((Component) this.dailyMissionPointRewardItems[i]).gameObject.SetActive(true);
        this.dailyMissionPointRewardItems[i].Init(this.controller, pointRewardBox, this.currentDailyPoint, ((IEnumerable<int?>) daily.received_rewards).Contains<int?>(new int?(pointRewardBox.ID)), new Action(this.UpdateDailyPointRewardItems));
      }
      else
        ((Component) this.dailyMissionPointRewardItems[i]).gameObject.SetActive(false);
      rewardPoints[i + 1] = pointRewardBox?.point;
    }
    this.SetDailyMissionPointGauge(this.currentDailyPoint, rewardPoints);
    PlayerDailyMissionPointWeekly weekly = SMManager.Get<PlayerDailyMissionPoint>().weekly;
    this.currentWeeklyPoint = weekly.point.Value;
    int?[] receivedWeeklyRewardBoxIDs = weekly.received_rewards;
    this.weeklyMissionButtonBadge.SetActive(this.weeklyPointRewardBoxList.Any<PointRewardBox>((Func<PointRewardBox, bool>) (x => this.currentWeeklyPoint >= x.point && !((IEnumerable<int?>) receivedWeeklyRewardBoxIDs).Contains<int?>(new int?(x.ID)))));
    ((UIButtonColor) this.weeelyMissionButton).isEnabled = this.weeklyPointRewardBoxList.Count > 0;
    this.UpdateDailyMissionTabBadge();
  }

  private void resetPointRewardBoxes()
  {
    DateTime nowTime = ServerTime.NowAppTimeAddDelta();
    this.dailyPointRewardBoxList_ = ((IEnumerable<PointRewardBox>) MasterData.PointRewardBoxList).Where<PointRewardBox>((Func<PointRewardBox, bool>) (x => x.type == 1 && x.start_at.HasValue && x.start_at.Value <= nowTime && x.end_at.HasValue && x.end_at.Value >= nowTime)).ToList<PointRewardBox>();
    this.weeklyPointRewardBoxList_ = ((IEnumerable<PointRewardBox>) MasterData.PointRewardBoxList).Where<PointRewardBox>((Func<PointRewardBox, bool>) (x => x.type == 2 && x.start_at.HasValue && x.start_at.Value <= nowTime && x.end_at.HasValue && x.end_at.Value >= nowTime)).ToList<PointRewardBox>();
  }

  private void UpdateDailyPointRewardItems()
  {
    for (int i = 0; i < this.dailyMissionPointRewardItems.Count; i++)
    {
      PointRewardBox pointRewardBox = this.dailyPointRewardBoxList.FirstOrDefault<PointRewardBox>((Func<PointRewardBox, bool>) (x => x.box_type == i + 1));
      if (pointRewardBox != null)
      {
        ((Component) this.dailyMissionPointRewardItems[i]).gameObject.SetActive(true);
        this.dailyMissionPointRewardItems[i].Init(this.controller, pointRewardBox, this.currentDailyPoint, ((IEnumerable<int?>) SMManager.Get<PlayerDailyMissionPoint>().daily.received_rewards).Contains<int?>(new int?(pointRewardBox.ID)), new Action(this.UpdateDailyPointRewardItems));
      }
      else
        ((Component) this.dailyMissionPointRewardItems[i]).gameObject.SetActive(false);
    }
    this.UpdateDailyMissionTabBadge();
    this.updateBulkReceiveButton(this.scrollType, false);
  }

  private void UpdateDailyMissionTabBadge()
  {
    bool flag1 = ((IEnumerable<IMissionAchievement>) this.missionDic[0]).Any<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.isCleared && !x.isReceived));
    PlayerDailyMissionPointWeekly weeklyData = SMManager.Get<PlayerDailyMissionPoint>().weekly;
    bool flag2 = this.weeklyPointRewardBoxList.Any<PointRewardBox>((Func<PointRewardBox, bool>) (x =>
    {
      int? point1 = weeklyData.point;
      int point2 = x.point;
      return point1.GetValueOrDefault() >= point2 & point1.HasValue && !((IEnumerable<int?>) weeklyData.received_rewards).Contains<int?>(new int?(x.ID));
    }));
    PlayerDailyMissionPointDaily dailyData = SMManager.Get<PlayerDailyMissionPoint>().daily;
    bool flag3 = this.dailyPointRewardBoxList.Any<PointRewardBox>((Func<PointRewardBox, bool>) (x =>
    {
      int? point3 = dailyData.point;
      int point4 = x.point;
      return point3.GetValueOrDefault() >= point4 & point3.HasValue && !((IEnumerable<int?>) dailyData.received_rewards).Contains<int?>(new int?(x.ID));
    }));
    this.dirBadge[0].SetActive(flag1 | flag2 | flag3);
  }

  private void SetDailyMissionPointGauge(int currentPoint, int?[] rewardPoints)
  {
    float[] numArray = new float[7]
    {
      0.0f,
      0.08f,
      0.297f,
      0.502f,
      0.715f,
      0.928f,
      1f
    };
    int index1 = 1;
    for (int index2 = 1; index2 < rewardPoints.Length; ++index2)
    {
      if (rewardPoints[index2].HasValue && currentPoint <= rewardPoints[index2].Value)
      {
        index1 = index2;
        break;
      }
    }
    int index3 = 0;
    for (int index4 = index1 - 1; index4 >= 0; --index4)
    {
      if (rewardPoints[index4].HasValue)
      {
        index3 = index4;
        break;
      }
    }
    this.dailyMissionPointGauge.value = numArray[index3] + (float) (((double) numArray[index1] - (double) numArray[index3]) * ((double) currentPoint - (double) rewardPoints[index3].Value)) / (float) (rewardPoints[index1].Value - rewardPoints[index3].Value);
  }

  private IEnumerator DailymissionBulkReceive()
  {
    DailyMissionWindow window = this;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 1;
    common.isLoading = true;
    window.not_received_pointrewards.Clear();
    window.resultPointRewards.Clear();
    IMissionAchievement[] datUpdate;
    IEnumerator e1;
    if (window.scrollType == DailyMissionWindow.ScrollType.Daily)
    {
      PlayerDailyMissionPointDaily old = SMManager.Get<PlayerDailyMissionPoint>().daily;
      List<PointRewardBox> leftoverPointRewards = window.dailyPointRewardBoxList.OrderBy<PointRewardBox, int>((Func<PointRewardBox, int>) (x => x.box_type)).Where<PointRewardBox>((Func<PointRewardBox, bool>) (y => !((IEnumerable<int?>) old.received_rewards).Contains<int?>(new int?(y.ID)))).ToList<PointRewardBox>();
      Future<WebAPI.Response.DailymissionReceiveDailyAll> future = WebAPI.DailymissionReceiveDailyAll((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        common.DailyMissionController.Hide();
      }));
      e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      common.isLoading = false;
      common.loadingMode = 0;
      if (future.Result == null)
      {
        yield break;
      }
      else
      {
        datUpdate = ((IEnumerable<PlayerDailyMissionAchievement>) future.Result.player_daily_missions).Select<PlayerDailyMissionAchievement, IMissionAchievement>((Func<PlayerDailyMissionAchievement, IMissionAchievement>) (x => Util.Create(x))).ToArray<IMissionAchievement>();
        window.received_missionsList = ((IEnumerable<int>) future.Result.received_missions).ToList<int>();
        int newPoint = future.Result.points.daily.point.Value;
        List<int> list = ((IEnumerable<int>) future.Result.received_points).ToList<int>();
        foreach (PointRewardBox pointRewardBox in leftoverPointRewards.Where<PointRewardBox>((Func<PointRewardBox, bool>) (x => x.point <= newPoint)))
        {
          if (list.Contains(pointRewardBox.ID))
            window.resultPointRewards.AddRange((IEnumerable<PointReward>) pointRewardBox.rewards);
          else
            window.not_received_pointrewards.Add(pointRewardBox.ID);
        }
        leftoverPointRewards = (List<PointRewardBox>) null;
        future = (Future<WebAPI.Response.DailymissionReceiveDailyAll>) null;
      }
    }
    else
    {
      Future<WebAPI.Response.DailymissionBulkReceive> future = WebAPI.DailymissionBulkReceive((int) (window.scrollType + 1), (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        common.DailyMissionController.Hide();
      }));
      e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      common.isLoading = false;
      common.loadingMode = 0;
      if (future.Result == null)
      {
        yield break;
      }
      else
      {
        datUpdate = ((IEnumerable<PlayerDailyMissionAchievement>) future.Result.player_daily_missions).Select<PlayerDailyMissionAchievement, IMissionAchievement>((Func<PlayerDailyMissionAchievement, IMissionAchievement>) (dm => Util.Create(dm))).ToArray<IMissionAchievement>();
        window.received_missionsList = ((IEnumerable<int>) future.Result.received_missions).ToList<int>();
        future = (Future<WebAPI.Response.DailymissionBulkReceive>) null;
      }
    }
    int[] types = new int[1]{ (int) window.scrollType };
    HashSet<MissionType> missionTargets = new HashSet<MissionType>(((IEnumerable<int>) types).Select<int, MissionType>((Func<int, MissionType>) (i => this.scrollMissionType[(DailyMissionWindow.ScrollType) i])));
    Dictionary<MissionType, IMissionAchievement[]> dictionary1 = ((IEnumerable<IMissionAchievement>) window.oldPlayerDailyMissions).Where<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.mission != null && missionTargets.Contains(x.mission.missionType) && x.isShow)).GroupBy<IMissionAchievement, MissionType>((Func<IMissionAchievement, MissionType>) (x => x.mission.missionType)).ToDictionary<IGrouping<MissionType, IMissionAchievement>, MissionType, IMissionAchievement[]>((Func<IGrouping<MissionType, IMissionAchievement>, MissionType>) (k => k.Key), (Func<IGrouping<MissionType, IMissionAchievement>, IMissionAchievement[]>) (v => this.sortMission(v.Key, (IEnumerable<IMissionAchievement>) v).ToArray<IMissionAchievement>()));
    Dictionary<int, IMissionAchievement[]> dictionary2 = new Dictionary<int, IMissionAchievement[]>();
    foreach (int key in types)
    {
      IMissionAchievement[] missionAchievementArray;
      if (!dictionary1.TryGetValue(window.scrollMissionType[(DailyMissionWindow.ScrollType) key], out missionAchievementArray))
        missionAchievementArray = new IMissionAchievement[0];
      dictionary2[key] = missionAchievementArray;
    }
    Dictionary<MissionType, IMissionAchievement[]> dictionary3 = ((IEnumerable<IMissionAchievement>) datUpdate).Where<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.mission != null && missionTargets.Contains(x.mission.missionType) && x.isShow)).GroupBy<IMissionAchievement, MissionType>((Func<IMissionAchievement, MissionType>) (x => x.mission.missionType)).ToDictionary<IGrouping<MissionType, IMissionAchievement>, MissionType, IMissionAchievement[]>((Func<IGrouping<MissionType, IMissionAchievement>, MissionType>) (k => k.Key), (Func<IGrouping<MissionType, IMissionAchievement>, IMissionAchievement[]>) (v => this.sortMission(v.Key, (IEnumerable<IMissionAchievement>) v).ToArray<IMissionAchievement>()));
    Dictionary<int, IMissionAchievement[]> dictionary4 = new Dictionary<int, IMissionAchievement[]>();
    foreach (int key in types)
    {
      IMissionAchievement[] missionAchievementArray;
      if (!dictionary3.TryGetValue(window.scrollMissionType[(DailyMissionWindow.ScrollType) key], out missionAchievementArray))
        missionAchievementArray = new IMissionAchievement[0];
      dictionary4[key] = missionAchievementArray;
    }
    window.notReceiveList.Clear();
    List<IMissionAchievement> missionAchievementList = new List<IMissionAchievement>();
    foreach (IMissionAchievement missionAchievement1 in dictionary2[(int) window.scrollType])
    {
      foreach (IMissionAchievement missionAchievement2 in dictionary4[(int) window.scrollType])
      {
        if (missionAchievement1.mission_id == missionAchievement2.mission_id && missionAchievement1.isCleared && missionAchievement2.isCleared)
          window.notReceiveList.Add(missionAchievement2.mission_id);
      }
      foreach (int receivedMissions in window.received_missionsList)
      {
        if (receivedMissions == missionAchievement1.mission_id)
          missionAchievementList.Add(missionAchievement1);
      }
    }
    window.rewardModelList.Clear();
    foreach (IMissionAchievement missionAchievement in missionAchievementList)
    {
      for (int index = 0; index < missionAchievement.rewards.Length; ++index)
        window.rewardModelList.Add(new DailyMission0272Panel.RewardViewModel(missionAchievement.rewards[index]));
    }
    for (int index = 0; index < window.received_missionsList.Count; ++index)
      window.notReceiveList.Remove(window.received_missionsList[index]);
    if (window.rewardModelList.Any<DailyMission0272Panel.RewardViewModel>() || window.resultPointRewards.Any<PointReward>())
    {
      Future<GameObject> clearPrefab = Res.Prefabs.battle.DailyMission_Collective.Load<GameObject>();
      e1 = clearPrefab.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject result = clearPrefab.Result;
      Singleton<PopupManager>.GetInstance().open(result).GetComponent<DailyMission0271Collective>().Initialize(window);
      NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null))
        instance.playSE("SE_0534");
      clearPrefab = (Future<GameObject>) null;
    }
    else if (window.isNotReceivedRewards)
      ModalWindow.Show("ミッション報酬受け取り", "報酬が上限に達して受け取れないミッションがあります", (Action) (() => { }));
    window.dirMissionListObject[(int) window.scrollType].ResetPosition();
    e1 = window.InitMissionList(datUpdate, types, false);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    window.InitializePointRewardSection(datUpdate);
  }

  private bool isNotReceivedRewards
  {
    get => this.notReceiveList.Any<int>() || this.not_received_pointrewards.Any<int>();
  }

  private IEnumerator GuildmissionBulkReceive()
  {
    DailyMissionWindow window = this;
    IMissionAchievement[] datUpdate = (IMissionAchievement[]) null;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 1;
    common.isLoading = true;
    window.received_missionsList = (List<int>) null;
    int[] types = new int[1]{ (int) window.scrollType };
    Future<WebAPI.Response.GuildmissionBulkReceive> future = WebAPI.GuildmissionBulkReceive((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    common.isLoading = false;
    common.loadingMode = 0;
    if (future.Result != null)
    {
      datUpdate = ((IEnumerable<GuildMissionInfo>) future.Result.player_guild_missions).Select<GuildMissionInfo, IMissionAchievement>((Func<GuildMissionInfo, IMissionAchievement>) (gm => Util.Create(gm))).ToArray<IMissionAchievement>();
      window.received_missionsList = ((IEnumerable<int>) future.Result.received_missions).ToList<int>();
      HashSet<MissionType> missionTargets = new HashSet<MissionType>(((IEnumerable<int>) types).Select<int, MissionType>((Func<int, MissionType>) (i => this.scrollMissionType[(DailyMissionWindow.ScrollType) i])));
      Dictionary<MissionType, IMissionAchievement[]> dictionary1 = ((IEnumerable<IMissionAchievement>) window.oldPlayerGuildMissions).Where<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.mission != null && missionTargets.Contains(x.mission.missionType) && x.isShow)).GroupBy<IMissionAchievement, MissionType>((Func<IMissionAchievement, MissionType>) (x => x.mission.missionType)).ToDictionary<IGrouping<MissionType, IMissionAchievement>, MissionType, IMissionAchievement[]>((Func<IGrouping<MissionType, IMissionAchievement>, MissionType>) (k => k.Key), (Func<IGrouping<MissionType, IMissionAchievement>, IMissionAchievement[]>) (v => this.sortMission(v.Key, (IEnumerable<IMissionAchievement>) v).ToArray<IMissionAchievement>()));
      Dictionary<int, IMissionAchievement[]> dictionary2 = new Dictionary<int, IMissionAchievement[]>();
      IMissionAchievement[] missionAchievementArray;
      foreach (int key in types)
      {
        if (!dictionary1.TryGetValue(window.scrollMissionType[(DailyMissionWindow.ScrollType) key], out missionAchievementArray))
          missionAchievementArray = new IMissionAchievement[0];
        dictionary2[key] = missionAchievementArray;
      }
      Dictionary<MissionType, IMissionAchievement[]> dictionary3 = ((IEnumerable<IMissionAchievement>) datUpdate).Where<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.mission != null && missionTargets.Contains(x.mission.missionType) && x.isShow)).GroupBy<IMissionAchievement, MissionType>((Func<IMissionAchievement, MissionType>) (x => x.mission.missionType)).ToDictionary<IGrouping<MissionType, IMissionAchievement>, MissionType, IMissionAchievement[]>((Func<IGrouping<MissionType, IMissionAchievement>, MissionType>) (k => k.Key), (Func<IGrouping<MissionType, IMissionAchievement>, IMissionAchievement[]>) (v => this.sortMission(v.Key, (IEnumerable<IMissionAchievement>) v).ToArray<IMissionAchievement>()));
      Dictionary<int, IMissionAchievement[]> dictionary4 = new Dictionary<int, IMissionAchievement[]>();
      missionAchievementArray = (IMissionAchievement[]) null;
      foreach (int key in types)
      {
        if (!dictionary3.TryGetValue(window.scrollMissionType[(DailyMissionWindow.ScrollType) key], out missionAchievementArray))
          missionAchievementArray = new IMissionAchievement[0];
        dictionary4[key] = missionAchievementArray;
      }
      window.not_received_pointrewards.Clear();
      window.notReceiveList.Clear();
      List<IMissionAchievement> missionAchievementList = new List<IMissionAchievement>();
      foreach (IMissionAchievement missionAchievement1 in dictionary2[(int) window.scrollType])
      {
        foreach (IMissionAchievement missionAchievement2 in dictionary4[(int) window.scrollType])
        {
          if (missionAchievement1.mission_id == missionAchievement2.mission_id && missionAchievement1.isCleared && missionAchievement2.isCleared && !missionAchievement1.isReceived && !missionAchievement2.isReceived)
            window.notReceiveList.Add(missionAchievement2.mission_id);
        }
        foreach (int receivedMissions in window.received_missionsList)
        {
          if (receivedMissions == missionAchievement1.mission_id)
            missionAchievementList.Add(missionAchievement1);
        }
      }
      window.rewardModelList.Clear();
      foreach (IMissionAchievement missionAchievement in missionAchievementList)
      {
        for (int index = 0; index < missionAchievement.rewards.Length; ++index)
          window.rewardModelList.Add(new DailyMission0272Panel.RewardViewModel(missionAchievement.rewards[index]));
      }
      window.resultPointRewards.Clear();
      for (int index = 0; index < window.received_missionsList.Count; ++index)
        window.notReceiveList.Remove(window.received_missionsList[index]);
      if (window.received_missionsList.Count == 0 && window.notReceiveList.Count > 0)
        ModalWindow.Show("ミッション報酬受け取り", "報酬が上限に達して受け取れないミッションがあります", (Action) (() => { }));
      else if (window.rewardModelList.Count > 0)
      {
        Future<GameObject> clearPrefab = Res.Prefabs.battle.DailyMission_Collective.Load<GameObject>();
        e1 = clearPrefab.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GameObject result = clearPrefab.Result;
        Singleton<PopupManager>.GetInstance().open(result).GetComponent<DailyMission0271Collective>().Initialize(window);
        NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
        if (Object.op_Inequality((Object) instance, (Object) null))
          instance.playSE("SE_0534");
        clearPrefab = (Future<GameObject>) null;
      }
      window.dirMissionListObject[(int) window.scrollType].ResetPosition();
      e1 = window.InitMissionList(datUpdate, types, false);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public void CollectivePopupEnd() => this.StartCoroutine(this.OpenResultPopup());

  private IEnumerator OpenResultPopup()
  {
    Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_027_5_collective__anim_popup01").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefab.Result;
    Singleton<PopupManager>.GetInstance().open(result, isNonSe: true, isNonOpenAnime: true).GetComponent<DailyMission0272CollectivePopup>().Initialize(this.rewardModelList, this.resultPointRewards, (this.isNotReceivedRewards ? 1 : 0) != 0, new GameObject[2]
    {
      this.controller.dailyMissionCollectiveListPrefab,
      this.controller.missionPointRewardDetailItemPrefab
    });
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
  }

  public void IbtnDaily()
  {
    if (this.isProcessingMissionList || this.scrollType == DailyMissionWindow.ScrollType.Daily)
      return;
    this.scrollType = DailyMissionWindow.ScrollType.Daily;
    this.StartCoroutine(this.DisplayMissionList(this.scrollType));
  }

  public void IbtnGame()
  {
    if (this.isProcessingMissionList || this.scrollType == DailyMissionWindow.ScrollType.Game)
      return;
    this.scrollType = DailyMissionWindow.ScrollType.Game;
    this.StartCoroutine(this.DisplayMissionList(this.scrollType));
  }

  public void IbtnPeriod()
  {
    if (this.isProcessingMissionList || this.scrollType == DailyMissionWindow.ScrollType.Period)
      return;
    this.scrollType = DailyMissionWindow.ScrollType.Period;
    this.StartCoroutine(this.DisplayMissionList(this.scrollType));
  }

  public void IbtnGuild()
  {
    if (this.isProcessingMissionList || this.scrollType == DailyMissionWindow.ScrollType.Guild)
      return;
    this.scrollType = DailyMissionWindow.ScrollType.Guild;
    this.StartCoroutine(this.DisplayMissionList(this.scrollType));
  }

  public void IbtnBulkReceive()
  {
    if (this.scrollType == DailyMissionWindow.ScrollType.Guild)
      this.StartCoroutine(this.GuildmissionBulkReceive());
    else
      this.StartCoroutine(this.DailymissionBulkReceive());
  }

  public void onClickedHelp()
  {
    if (this.helpCategory == null || this.IsPushAndSet())
      return;
    Help0152Scene component = ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Help0152Scene>();
    if (Object.op_Inequality((Object) component, (Object) null) && component.menu.titleName.Equals("ミッション"))
    {
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "help015_2")
        Help0152Scene.ChangeScene(false, this.helpCategory);
      else
        Help0152Scene.ChangeScene(true, this.helpCategory);
    }
  }

  public void OnClickWeeklyMissionButton() => this.StartCoroutine(this.OpenWeeklyMissionPopup());

  private IEnumerator OpenWeeklyMissionPopup()
  {
    DailyMissionWindow dailyMissionWindow = this;
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(dailyMissionWindow.controller.weeklyMissionPointRewardPopupPrefab).GetComponent<WeeklyMissionPointRewardPopupController>().Init(dailyMissionWindow.controller, dailyMissionWindow.currentWeeklyPoint, dailyMissionWindow.weeklyPointRewardBoxList, new Action(dailyMissionWindow.UpdateWeeklyMissionButtonBadge), new Action(dailyMissionWindow.OnClickWeeklyMissionButton));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void UpdateWeeklyMissionButtonBadge()
  {
    this.weeklyMissionButtonBadge.SetActive(this.weeklyPointRewardBoxList.Any<PointRewardBox>((Func<PointRewardBox, bool>) (x => this.currentWeeklyPoint >= x.point && !((IEnumerable<int?>) SMManager.Get<PlayerDailyMissionPoint>().weekly.received_rewards).Contains<int?>(new int?(x.ID)))));
    this.UpdateDailyPointRewardItems();
  }

  public override void onBackButton()
  {
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
  }

  public enum ScrollType
  {
    Daily,
    Game,
    Period,
    Guild,
  }
}
