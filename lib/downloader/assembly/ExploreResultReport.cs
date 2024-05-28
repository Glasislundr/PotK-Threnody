// Decompiled with JetBrains decompiler
// Type: ExploreResultReport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExploreResultReport : BackButtonMenuBase
{
  [SerializeField]
  private UILabel mDungeonNameLbl;
  [SerializeField]
  private UILabel mDungeonFloorNumLbl;
  [SerializeField]
  private UILabel mDefeatEnemyNumLbl;
  [SerializeField]
  private UILabel mExploreElapsedTime;
  [SerializeField]
  private UILabel mGottenZenyLbl;
  [SerializeField]
  private UILabel mGottenPlayerExpLbl;
  [SerializeField]
  private UILabel mNextLevelUpExpLbl;
  [SerializeField]
  private GameObject mPlayerExpGauge;
  [SerializeField]
  private UIGrid mItemBoxGrid;
  [SerializeField]
  private UI2DSprite mUnitImage;
  [SerializeField]
  private Animator mAnimator;
  private GameObject mRewardIconPrefab;
  private GameObject mPlayerLevelupPrefab;
  private LoginReportInfo mReportInfo;
  private GaugeRunner mExpGaugeRunner;
  private bool mPlaySeDirty;

  public bool IsFinish { get; private set; }

  public IEnumerator Initialize()
  {
    ExploreDataManager exploreData = Singleton<ExploreDataManager>.GetInstance();
    Player player = SMManager.Get<Player>();
    this.mReportInfo = exploreData.LoginReportInfo;
    this.mDungeonNameLbl.SetTextLocalize(exploreData.FloorData.name);
    this.mDungeonFloorNumLbl.SetTextLocalize(exploreData.NowFloor.ToString() + "階");
    this.mDefeatEnemyNumLbl.SetTextLocalize(this.mReportInfo.DefeatEnemyCount.ToString() + "体");
    string empty = string.Empty;
    this.mExploreElapsedTime.SetTextLocalize(this.mReportInfo.CalcTimeSpan.ToString(this.mReportInfo.CalcTimeSpan.TotalHours >= 1.0 ? (this.mReportInfo.CalcTimeSpan.TotalDays >= 1.0 ? "d'日'hh'時間'mm'分'" : "hh'時間'mm'分'") : "mm'分'"));
    this.mGottenZenyLbl.SetTextLocalize(this.mReportInfo.Zeny);
    this.mGottenPlayerExpLbl.SetTextLocalize("+" + (object) this.mReportInfo.PlayerExp);
    this.mNextLevelUpExpLbl.SetTextLocalize(player.exp_next);
    IEnumerator e = this.SetupLevelUpGauge();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<int> rewardsId = this.mReportInfo.RewardsId;
    e = this.CreateExploreBoxView(rewardsId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (rewardsId.Count > 0)
      this.mPlaySeDirty = true;
    e = this.LoadUnitSprite(exploreData.GetExploreMainUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mAnimator.Play("explore_Result_In");
  }

  private IEnumerator SetupLevelUpGauge()
  {
    if (Object.op_Equality((Object) this.mPlayerLevelupPrefab, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/battle/PlayerLevelUpPrefab").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mPlayerLevelupPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    Vector3 localScale = this.mPlayerExpGauge.transform.localScale;
    localScale.x = this.mReportInfo.ExpGaugeStartValue;
    this.mPlayerExpGauge.transform.localScale = localScale;
  }

  private IEnumerator CreateExploreBoxView(List<int> rewardsId)
  {
    ExploreResultReport exploreResultReport = this;
    IEnumerator e;
    if (Object.op_Equality((Object) exploreResultReport.mRewardIconPrefab, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/RewardIcon/createIconObject").Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exploreResultReport.mRewardIconPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    foreach (int key in rewardsId)
    {
      ExploreDropReward exploreDropReward = MasterData.ExploreDropReward[key];
      CreateIconObject icon = exploreResultReport.mRewardIconPrefab.CloneAndGetComponent<CreateIconObject>(((Component) exploreResultReport.mItemBoxGrid).transform);
      e = icon.CreateThumbnail(exploreDropReward.reward_type, exploreDropReward.reward_id, exploreDropReward.reward_quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      UnitIcon component = icon.GetIcon().GetComponent<UnitIcon>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.RarityCenter();
      icon.addComponentUniqueIconDragScrollView();
      icon = (CreateIconObject) null;
    }
    // ISSUE: method pointer
    exploreResultReport.mItemBoxGrid.onReposition = new UIGrid.OnReposition((object) exploreResultReport, __methodptr(\u003CCreateExploreBoxView\u003Eb__22_0));
    exploreResultReport.mItemBoxGrid.Reposition();
  }

  private IEnumerator LoadUnitSprite(PlayerUnit unit)
  {
    int jobID = unit.job_id;
    Future<GameObject> future = unit.unit.LoadMypage();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject imgObj = future.Result.Clone(((Component) this.mUnitImage).transform);
    e = unit.unit.SetLargeSpriteSnap(jobID, imgObj, 2);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit.unit.SetLargeSpriteWithMask(jobID, imgObj, Res.GUI._020_19_1_sozai.mask_Chara.Load<Texture2D>(), 2, 146, 36);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator ShowExpAnimation()
  {
    ExploreResultReport exploreResultReport = this;
    yield return (object) new WaitForSeconds(1.43f);
    exploreResultReport.mExpGaugeRunner = new GaugeRunner(exploreResultReport.mPlayerExpGauge, exploreResultReport.mReportInfo.ExpGaugeStartValue, exploreResultReport.mReportInfo.ExpGaugeFinishValue, exploreResultReport.mReportInfo.ExpGaugeLoopNum, new Func<GameObject, int, IEnumerator>(exploreResultReport.OnExpGaugeMax), duration: 0.5f);
    IEnumerator e = GaugeRunner.Run(exploreResultReport.mExpGaugeRunner);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void Update()
  {
    if (this.mPlaySeDirty)
    {
      AnimatorStateInfo animatorStateInfo = this.mAnimator.GetCurrentAnimatorStateInfo(0);
      if (((AnimatorStateInfo) ref animatorStateInfo).IsName("explore_Result_In_Reward"))
      {
        this.mPlaySeDirty = false;
        Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1021");
      }
    }
    base.Update();
  }

  public override void onBackButton()
  {
    if (this.mExpGaugeRunner == null)
      return;
    if (this.mExpGaugeRunner.isRunning)
      this.mExpGaugeRunner.Skip();
    else
      this.IsFinish = true;
  }

  private GameObject OpenLvUpPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }

  private IEnumerator OnExpGaugeMax(GameObject obj, int count)
  {
    if (this.mReportInfo.BeforePlayerLevel + count + 1 == this.mReportInfo.AfterPlayerLevel)
    {
      yield return (object) new WaitForSeconds(0.1f);
      GaugeRunner.PauseSE();
      Battle020171Menu component = this.OpenLvUpPopup(this.mPlayerLevelupPrefab).GetComponent<Battle020171Menu>();
      Player player = SMManager.Get<Player>();
      component.SetLv(this.mReportInfo.BeforePlayerLevel, this.mReportInfo.AfterPlayerLevel);
      component.SetName(player.name);
      List<string> self = new List<string>();
      self.Add(Consts.GetInstance().BATTLE_RESULT_RECOVERY_AP);
      int num1;
      if ((num1 = player.ap_max - this.mReportInfo.BeforePlayerApMax) > 0)
        self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_AP, (IDictionary) new Hashtable()
        {
          {
            (object) "value",
            (object) num1
          }
        }));
      int num2;
      if ((num2 = player.max_cost - this.mReportInfo.BeforePlayerMaxCost) > 0)
        self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_DECK_COST, (IDictionary) new Hashtable()
        {
          {
            (object) "value",
            (object) num2
          }
        }));
      int num3;
      if ((num3 = player.max_friends - this.mReportInfo.BeforePlayerMaxFriend) > 0)
        self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_FRIEND_COUNT, (IDictionary) new Hashtable()
        {
          {
            (object) "value",
            (object) num3
          }
        }));
      component.SetExplanetion(self.Join("\n"));
      bool onFinished = false;
      component.SetCallback((Action) (() => onFinished = true));
      while (!onFinished)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.1f);
      GaugeRunner.ResumeSE();
      this.mAnimator.Play("explore_Result_Complete");
    }
  }
}
