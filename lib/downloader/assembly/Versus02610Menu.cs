// Decompiled with JetBrains decompiler
// Type: Versus02610Menu
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
public class Versus02610Menu : Versus026MatchBase
{
  [SerializeField]
  private UILabel txtClassName;
  [SerializeField]
  private UILabel txtDraw;
  [SerializeField]
  private UILabel txtWin;
  [SerializeField]
  private UILabel txtLose;
  [SerializeField]
  private UILabel txtMyRank;
  [SerializeField]
  private UILabel txtRanking;
  [SerializeField]
  private UILabel txtMatchRemain;
  [SerializeField]
  private UILabel txtRankPoint;
  [SerializeField]
  private UILabel txtCondition;
  [SerializeField]
  private UISprite slcRankGaugeBlue;
  [SerializeField]
  private UISprite slcRankGaugeGreen;
  [SerializeField]
  private UISprite slcRankGaugeYellow;
  [SerializeField]
  private UISprite slcRankGaugeRed;
  [SerializeField]
  private UISprite slcRankGaugeNow;
  [SerializeField]
  private GameObject objEffectParent;
  [SerializeField]
  private UIButton btnSeasonFinish;
  [SerializeField]
  private UIButton btnRanking;
  [SerializeField]
  private UISprite slcBtnRanking;
  [SerializeField]
  private UIButton btnRewardInfo;
  [SerializeField]
  private UISprite slcBtnRewardInfo;
  [SerializeField]
  private UIButton btnHowto;
  [SerializeField]
  private UISprite slcBtnHowto;
  [SerializeField]
  private GameObject btnComingSoonRanking;
  [SerializeField]
  private GameObject dynRewardThum;
  [SerializeField]
  private GameObject dynRemainLampGrid;
  private static readonly int PVP_TUTORIAL_CLASS_MATCH_END_PAGE = 200;
  private static bool isContinue = false;
  private Versus02610Scene scene;
  private WebAPI.Response.PvpRanking responsePvpRanking;
  protected GameObject LampIconPrefab;
  private bool isUpdate;
  private bool isPlayingEffect;

  public static bool IsContinue
  {
    set => Versus02610Menu.isContinue = value;
  }

  public Transform GetEffectParent => this.objEffectParent.transform;

  public bool IsUpdate => this.isUpdate;

  public WebAPI.Response.PvpBoot UpdatePvpInfo => this.pvpInfo;

  public void SetPlayingEffect(bool isPlay) => this.isPlayingEffect = isPlay;

  public void setScene(Versus02610Scene scene) => this.scene = scene;

  public override IEnumerator Init(PvpMatchingTypeEnum type, WebAPI.Response.PvpBoot pvpInfo)
  {
    Versus02610Menu versus02610Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = versus02610Menu.\u003C\u003En__0(type, pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02610Menu.InitializeDisplay();
    yield return (object) versus02610Menu.requestAPIPvpRanking();
    yield return (object) versus02610Menu.createRewardTumnails();
    yield return (object) versus02610Menu.LoadLampIconPrefab();
    versus02610Menu.createMatchRemainLamp();
    if (Persist.pvpInfo.Data.currentPage < Versus02610Menu.PVP_TUTORIAL_CLASS_MATCH_END_PAGE)
    {
      versus02610Menu.isEnableSeasonFinishButton(false);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton(Player.Current.IsClassMatchRanking() ? "pvp_class2" : "pvp_class1", new Dictionary<string, Func<Transform, UIButton>>()
      {
        {
          "versus_classmatch3",
          new Func<Transform, UIButton>(versus02610Menu.\u003CInit\u003Eb__42_0)
        },
        {
          "versus_classmatch5",
          (Func<Transform, UIButton>) (root =>
          {
            Transform childInFind = root.GetChildInFind("Bottom");
            ((Component) childInFind.GetChildInFind("ibtn_Ranking")).gameObject.SetActive(Player.Current.IsClassMatchRanking());
            ((Component) childInFind.GetChildInFind("ibtn_Ranking_comingsoon")).gameObject.SetActive(!Player.Current.IsClassMatchRanking());
            return (UIButton) null;
          })
        }
      }, new Action(versus02610Menu.\u003CInit\u003Eb__42_2));
    }
    else
    {
      e = versus02610Menu.SetRankingOrSeasonPopup();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (((IEnumerable<RankingGroup>) versus02610Menu.responsePvpRanking.ranking_groups).Count<RankingGroup>() <= 0)
    {
      ((UIButtonColor) versus02610Menu.btnRanking).isEnabled = false;
      ((UIWidget) versus02610Menu.slcBtnRanking).color = Color.gray;
      ((UIButtonColor) versus02610Menu.btnRewardInfo).isEnabled = false;
      ((UIWidget) versus02610Menu.slcBtnRewardInfo).color = Color.gray;
      ((UIButtonColor) versus02610Menu.btnHowto).isEnabled = false;
      ((UIWidget) versus02610Menu.slcBtnHowto).color = Color.gray;
    }
    else
    {
      ((UIButtonColor) versus02610Menu.btnRanking).isEnabled = true;
      ((UIWidget) versus02610Menu.slcBtnRanking).color = Color.white;
      ((UIButtonColor) versus02610Menu.btnRewardInfo).isEnabled = true;
      ((UIWidget) versus02610Menu.slcBtnRewardInfo).color = Color.white;
      ((UIButtonColor) versus02610Menu.btnHowto).isEnabled = true;
      ((UIWidget) versus02610Menu.slcBtnHowto).color = Color.white;
    }
    Persist.pvpInfo.Data.lastMatchingType = type;
    Persist.pvpInfo.Flush();
  }

  private void createMatchRemainLamp()
  {
    this.dynRemainLampGrid.transform.Clear();
    Player player = SMManager.Get<Player>();
    int num = 32;
    for (int index = 0; index < player.mp_max; ++index)
    {
      GameObject gameObject = this.LampIconPrefab.Clone(this.dynRemainLampGrid.transform);
      gameObject.transform.localPosition = new Vector3((float) (-num * (player.mp_max - 1) / 2 + num * index), 0.0f);
      if (index >= player.mp)
      {
        ((Component) gameObject.transform.GetChildInFind("slc_icon_Multi_Lamp_01_on")).gameObject.SetActive(false);
      }
      else
      {
        UISprite component = gameObject.GetComponent<UISprite>();
        if (Object.op_Inequality((Object) component, (Object) null))
          ((Behaviour) component).enabled = false;
      }
    }
  }

  protected IEnumerator LoadLampIconPrefab()
  {
    if (Object.op_Equality((Object) this.LampIconPrefab, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/versus026_12/slc_icon_Multi_Lamp").Load<GameObject>();
      yield return (object) prefabF.Wait();
      this.LampIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator SetRankingOrSeasonPopup()
  {
    Versus02610Menu versus02610Menu = this;
    IEnumerator e;
    if (versus02610Menu.pvpInfo.rank_done)
    {
      if (!versus02610Menu.IsPushAndSet())
      {
        e = versus02610Menu.RankingEndPopup();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else
    {
      e = versus02610Menu.InitSeasonEndPopup();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator RankingEndPopup()
  {
    Versus02610Menu menu = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.PvpRankingClose> f = WebAPI.PvpRankingClose((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result != null)
    {
      Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_10__anim_popup01.Load<GameObject>();
      e1 = pF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject gameObject = pF.Result.Clone(menu.GetEffectParent);
      menu.isPlayingEffect = true;
      e1 = gameObject.GetComponent<Popup02689Menu>().Init(menu.pvpInfo, f.Result, menu);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  private IEnumerator InitSeasonEndPopup()
  {
    Versus02610Menu versus02610Menu = this;
    bool flag = versus02610Menu.pvpInfo.max_addition_matches > versus02610Menu.pvpInfo.remaining_addition_matches && versus02610Menu.pvpInfo.season_remaining_matches > 0;
    if (versus02610Menu.pvpInfo.is_season_done && !Versus02610Menu.isContinue && !flag)
    {
      if (!versus02610Menu.IsPushAndSet())
      {
        IEnumerator e = versus02610Menu.SeasonEndPopup();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        versus02610Menu.isEnableSeasonFinishButton(true);
      }
    }
    else
    {
      versus02610Menu.isEnableSeasonFinishButton(false);
      if (((IEnumerable<RankingGroup>) versus02610Menu.responsePvpRanking.ranking_groups).Count<RankingGroup>() > 0)
      {
        RankingGroup rankingGroup = versus02610Menu.responsePvpRanking.ranking_groups[0];
        if (Persist.pvpRankMatch.Data.lastRankMatchPeriodId != rankingGroup.period_id)
          yield return (object) versus02610Menu.dispWeekRank();
      }
    }
  }

  public override void IbtnWarExperience()
  {
    if (this.IsPushAndSet())
      return;
    base.IbtnWarExperience();
    Versus02613Scene.ChangeTopScene(true, this.pvpInfo.pvp_class_record, this.pvpInfo.best_class, SMManager.Get<Player>().id, false);
  }

  public void IbtnClassInfo()
  {
    if (this.IsPushAndSet())
      return;
    Debug.Log((object) "===ChangeScene 2612");
    Versus02611Scene.ChangeScene(true, this.pvpInfo);
  }

  public void IbtnRanking()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ChangeSceneRanking());
  }

  public void IbtnSeasonFinish()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.SeasonEndPopup());
  }

  public void IbtnHowto()
  {
    if (this.IsPushAndSet())
      return;
    this.dispHowto();
  }

  public void IbtnRewardInfo()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ChangeSceneRewardInfo());
  }

  protected override string SetRoomKey(string key) => base.SetRoomKey(key);

  private void InitializeDisplay()
  {
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_002610TITLE);
    this.txtClassName.SetText(MasterData.PvpClassKind[this.pvpInfo.current_class].name);
    this.txtDraw.SetText(this.pvpInfo.pvp_class_record.current_season_draw_count.ToLocalizeNumberText());
    this.txtWin.SetText(this.pvpInfo.pvp_class_record.current_season_win_count.ToLocalizeNumberText());
    this.txtLose.SetText(this.pvpInfo.pvp_class_record.current_season_loss_count.ToLocalizeNumberText());
    this.txtMyRank.SetText(MasterData.PvpRankingKind[this.pvpInfo.current_rank].name);
    bool flag = Player.Current.IsClassMatchRanking();
    string text1 = !flag || this.pvpInfo.ranking <= 0 ? instance.COMMON_NOVALUE : this.pvpInfo.ranking.ToLocalizeNumberText();
    string text2 = flag ? this.pvpInfo.ranking_pt.ToLocalizeNumberText() : instance.COMMON_NOVALUE;
    this.txtRanking.SetText(text1);
    this.txtRankPoint.SetText(text2);
    ((Component) this.btnRanking).gameObject.SetActive(flag);
    this.btnComingSoonRanking.gameObject.SetActive(!flag);
    ((UIButtonColor) this.btnRanking).isEnabled = Player.Current.IsClassMatchShowRanking();
    this.txtMatchRemain.SetTextLocalize(this.GetMatchRemainText());
    this.SetTxtCondition(this.txtCondition);
    this.SetRankGauge();
  }

  private string GetMatchRemainText()
  {
    Player player = SMManager.Get<Player>();
    Consts.GetInstance();
    return Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_TOP, (IDictionary) new Hashtable()
    {
      {
        (object) "remainTime",
        (object) this.SetRemainTime(player.mp_full_recovery_at.Hour, player.mp_full_recovery_at.Minute)
      }
    });
  }

  private string SetRemainTime(int hour, int minute)
  {
    Consts instance = Consts.GetInstance();
    return Consts.Format(minute < 10 ? instance.VERSUS_00261REMAIN_TIME_SUB : instance.VERSUS_00261REMAIN_TIME, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (hour),
        (object) hour.ToLocalizeNumberText()
      },
      {
        (object) nameof (minute),
        (object) minute.ToLocalizeNumberText()
      }
    });
  }

  private void SetTxtCondition(UILabel l)
  {
    PvpClassKind pvpClassKind = MasterData.PvpClassKind[this.pvpInfo.current_class];
    int currentSeasonWinCount = this.pvpInfo.pvp_class_record.current_season_win_count;
    PvpClassKind.Condition condition1 = pvpClassKind.ClassCondition(currentSeasonWinCount);
    PvpClassKind.Condition condition2 = pvpClassKind.NextCondition(condition1);
    Consts instance = Consts.GetInstance();
    switch (condition2)
    {
      case PvpClassKind.Condition.STAY:
      case PvpClassKind.Condition.STAY_TOPCLASS:
        Color color1 = Color32.op_Implicit(new Color32((byte) 10, (byte) 223, (byte) 29, byte.MaxValue));
        ((UIWidget) l).color = color1;
        l.SetText(Consts.Format(instance.VERSUS_002610CONDITION_DOWN, (IDictionary) new Hashtable()
        {
          {
            (object) "win",
            (object) (pvpClassKind.stay_point - currentSeasonWinCount).ToLocalizeNumberText()
          }
        }));
        break;
      case PvpClassKind.Condition.UP:
        Color color2 = Color32.op_Implicit(new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue));
        ((UIWidget) l).color = color2;
        l.SetText(Consts.Format(instance.VERSUS_002610CONDITION_STAY, (IDictionary) new Hashtable()
        {
          {
            (object) "win",
            (object) (pvpClassKind.up_point - currentSeasonWinCount).ToLocalizeNumberText()
          }
        }));
        break;
      case PvpClassKind.Condition.TITLE:
      case PvpClassKind.Condition.TITLE_TOPCLASS:
        if (condition1 == PvpClassKind.Condition.TITLE || condition1 == PvpClassKind.Condition.TITLE_TOPCLASS)
        {
          Color color3 = Color32.op_Implicit(new Color32(byte.MaxValue, (byte) 105, (byte) 0, byte.MaxValue));
          ((UIWidget) l).color = color3;
          l.SetText(instance.VERSUS_002610CONDITION_TITLE);
          break;
        }
        Color color4 = Color32.op_Implicit(new Color32(byte.MaxValue, (byte) 105, (byte) 0, byte.MaxValue));
        ((UIWidget) l).color = color4;
        l.SetText(Consts.Format(instance.VERSUS_002610CONDITION_UP, (IDictionary) new Hashtable()
        {
          {
            (object) "win",
            (object) (pvpClassKind.title_point - currentSeasonWinCount).ToLocalizeNumberText()
          }
        }));
        break;
    }
  }

  private void SetRankGauge()
  {
    int width = ((UIWidget) this.slcRankGaugeRed).width;
    int num = 10;
    PvpClassKind pvpClassKind = MasterData.PvpClassKind[this.pvpInfo.current_class];
    ((UIWidget) this.slcRankGaugeBlue).width = (pvpClassKind.stay_point - 1) * width / num;
    ((UIWidget) this.slcRankGaugeGreen).width = (pvpClassKind.up_point - 1) * width / num;
    ((UIWidget) this.slcRankGaugeYellow).width = (pvpClassKind.title_point - 1) * width / num;
    ((Component) this.slcRankGaugeBlue).gameObject.SetActive(pvpClassKind.stay_point > 0);
    ((Component) this.slcRankGaugeGreen).gameObject.SetActive(pvpClassKind.up_point - pvpClassKind.stay_point > 0);
    ((Component) this.slcRankGaugeYellow).gameObject.SetActive(pvpClassKind.title_point - pvpClassKind.up_point > 0);
    ((Component) this.slcRankGaugeRed).gameObject.SetActive(true);
    ((Component) this.slcRankGaugeNow).transform.localPosition = new Vector3(((Component) this.slcRankGaugeRed).transform.localPosition.x + (float) (Mathf.Clamp(this.pvpInfo.pvp_class_record.current_season_win_count, 0, num) * width / num), ((Component) this.slcRankGaugeNow).transform.localPosition.y);
  }

  private IEnumerator ShowMPAlertPopup()
  {
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_026_10_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result);
  }

  private IEnumerator ShowAggregatePopup()
  {
    Consts instance1 = Consts.GetInstance();
    IEnumerator e = PopupCommon.Show(instance1.VERSUS_002610AGGREGATE_TITLE, instance1.VERSUS_002610AGGREGATE_DESCRIPTION, (Action) (() =>
    {
      NGSceneManager instance2 = Singleton<NGSceneManager>.GetInstance();
      instance2.clearStack();
      instance2.destroyCurrentScene();
      Versus0261Scene.ChangeScene0261(false);
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override bool IsMatchingBeginCheck()
  {
    return SMManager.Get<Player>().mp > 0 && !this.pvpInfo.rank_aggregate;
  }

  protected override IEnumerator ErrorMathcingBegin()
  {
    Versus02610Menu versus02610Menu = this;
    IEnumerator e;
    if (SMManager.Get<Player>().mp <= 0)
    {
      e = versus02610Menu.ShowMPAlertPopup();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (versus02610Menu.pvpInfo.rank_aggregate)
    {
      e = versus02610Menu.ShowAggregatePopup();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SeasonEndPopup()
  {
    Versus02610Menu menu = this;
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_8__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Popup02688Menu>().InitCoroutine(menu.pvpInfo, menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator requestAPIPvpRanking()
  {
    Future<WebAPI.Response.PvpRanking> f = WebAPI.PvpRanking((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.responsePvpRanking = f.Result;
  }

  private bool isBelongRankingCondition(int condition_id)
  {
    PvpRankingCondition rankingCondition = MasterData.PvpRankingCondition[condition_id];
    if (condition_id == 7 || condition_id == 12)
      return false;
    if (this.pvpInfo.ranking == 0)
    {
      if (!rankingCondition.rank_lower.HasValue)
        return true;
      int? rankLower = rankingCondition.rank_lower;
      int num = 0;
      return rankLower.GetValueOrDefault() == num & rankLower.HasValue;
    }
    int? rankUpper = rankingCondition.rank_upper;
    int ranking1 = this.pvpInfo.ranking;
    if (rankUpper.GetValueOrDefault() <= ranking1 & rankUpper.HasValue)
    {
      int? rankLower = rankingCondition.rank_lower;
      int ranking2 = this.pvpInfo.ranking;
      if (rankLower.GetValueOrDefault() >= ranking2 & rankLower.HasValue || !rankingCondition.rank_lower.HasValue)
        return true;
    }
    return false;
  }

  private IEnumerator createRewardTumnails()
  {
    this.dynRewardThum.transform.Clear();
    if (this.responsePvpRanking != null && ((IEnumerable<RankingGroup>) this.responsePvpRanking.ranking_groups).Count<RankingGroup>() > 0)
    {
      RankingGroup data = this.responsePvpRanking.ranking_groups[0];
      PvpClassRankingReward[] array = ((IEnumerable<PvpClassRankingReward>) MasterData.PvpClassRankingRewardList).Where<PvpClassRankingReward>((Func<PvpClassRankingReward, bool>) (x => x.term_id == data.period_id && x.ranking_kind_PvpRankingKind == this.responsePvpRanking.rank_kind && this.isBelongRankingCondition(x.ranking_category_PvpRankingCondition))).ToArray<PvpClassRankingReward>();
      int num = 3;
      int biasX = 128;
      int i = 0;
      PvpClassRankingReward[] classRankingRewardArray = array;
      for (int index = 0; index < classRankingRewardArray.Length; ++index)
      {
        PvpClassRankingReward classRankingReward = classRankingRewardArray[index];
        Vector3 pos;
        // ISSUE: explicit constructor call
        ((Vector3) ref pos).\u002Ector((float) (-biasX * (num - 1) / 2 + biasX * i), 0.0f);
        yield return (object) this.CreateRewardIcon(this.dynRewardThum, new QuestScoreAchivementRewardReceived()
        {
          reward_quantity = classRankingReward.reward_quantity,
          id = classRankingReward.ID,
          reward_id = classRankingReward.reward_id,
          reward_type_id = classRankingReward.reward_type_CommonRewardType
        }, pos);
        if (++i >= num)
          break;
      }
      classRankingRewardArray = (PvpClassRankingReward[]) null;
    }
  }

  private IEnumerator CreateRewardIcon(
    GameObject parent,
    QuestScoreAchivementRewardReceived reward,
    Vector3 pos)
  {
    CreateIconObject target = parent.GetOrAddComponent<CreateIconObject>();
    yield return (object) target.CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
    target.GetIcon().transform.localPosition = pos;
  }

  private IEnumerator ChangeSceneRanking()
  {
    Versus02610Menu versus02610Menu = this;
    if (versus02610Menu.responsePvpRanking != null)
    {
      if (versus02610Menu.responsePvpRanking.rank_aggregate)
      {
        IEnumerator e = versus02610Menu.ShowAggregatePopup();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        Debug.Log((object) "===ChangeScene 2614");
        Versus02614Scene.ChangeScene(true, versus02610Menu.pvpInfo, versus02610Menu.responsePvpRanking.ranking_groups);
      }
    }
  }

  private IEnumerator ChangeSceneRewardInfo()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Versus02610Menu versus02610Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (versus02610Menu.responsePvpRanking == null)
      return false;
    Debug.Log((object) "===ChangeScene 2615");
    Versus02615Scene.ChangeScene(true, versus02610Menu.responsePvpRanking.ranking_groups, versus02610Menu.pvpInfo.campaign_rewards);
    return false;
  }

  private IEnumerator SceneUpdate(bool isUIUpdate = true)
  {
    Versus02610Menu versus02610Menu = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    versus02610Menu.isUpdate = true;
    versus02610Menu.IsPush = false;
    Future<WebAPI.Response.PvpBoot> future = WebAPI.PvpBoot((Action<WebAPI.Response.UserError>) (e =>
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
      WebAPI.Response.PvpBoot result = future.Result;
      versus02610Menu.pvpInfo = result;
      versus02610Menu.scene.updatePvpInfo();
      if (isUIUpdate)
      {
        e1 = versus02610Menu.Init(PvpMatchingTypeEnum.class_match, result);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        yield return (object) new WaitForSeconds(0.5f);
      }
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  public void isEnableSeasonFinishButton(bool isEnable)
  {
    ((Component) this.btnSeasonFinish).gameObject.SetActive(isEnable);
    ((Component) this.btnBreakRepair).gameObject.SetActive(!isEnable);
    ((Component) this.btnStartMatch).gameObject.SetActive(!isEnable);
    if (isEnable)
      return;
    this.SetMatchButton();
  }

  public void StartSceneUpdate(Action act = null, bool isUIUpdate = true)
  {
    this.StartCoroutine(this.SceneUpdate(isUIUpdate));
    if (act == null)
      return;
    act();
  }

  public IEnumerator dispWeekRank(Popup02689Menu menu = null)
  {
    Versus02610Menu menu1 = this;
    Future<GameObject> pF = new ResourceObject("Prefabs/versus026_12/week_AffiliationRank").Load<GameObject>();
    yield return (object) pF.Wait();
    yield return (object) Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Versus02610WeekRankPopup>().Init(menu1, menu1.pvpInfo.current_rank, menu);
  }

  public void dispHowto()
  {
    if (((IEnumerable<RankingGroup>) this.responsePvpRanking.ranking_groups).Count<RankingGroup>() <= 0)
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    RankingGroup rankingGroup = this.responsePvpRanking.ranking_groups[0];
    Persist.pvpRankMatch.Data.lastRankMatchPeriodId = rankingGroup.period_id;
    Persist.pvpRankMatch.Flush();
    Quest00228Scene.ChangeScene(rankingGroup.period_id, true);
  }

  public override void onBackButton()
  {
    if (this.isPlayingEffect)
      return;
    this.IbtnBack();
  }
}
