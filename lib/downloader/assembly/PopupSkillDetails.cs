// Decompiled with JetBrains decompiler
// Type: PopupSkillDetails
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
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class PopupSkillDetails : BackButtonPopupBase
{
  [SerializeField]
  [Tooltip("タイトルテーブル")]
  private PopupSkillDetails.TitleTable[] titles_;
  [SerializeField]
  private UIScrollView scroll_;
  [SerializeField]
  [Tooltip("これを親にスクロール物を置く")]
  private GameObject scrollNode_;
  [SerializeField]
  [Tooltip("横スワイプ用")]
  private BoxCollider scrollCollider_;
  [SerializeField]
  [Tooltip("ホイールスクロール監視")]
  private WheelScrollLog wheelScrollLog_;
  [SerializeField]
  private UICenterOnChild uiCenter_;
  [SerializeField]
  [Tooltip("オリジナルスクロールアイテム")]
  private PopupSkillDetail detail_;
  [SerializeField]
  [Tooltip("変更ボタン")]
  private GameObject btnChange_;
  [SerializeField]
  [Tooltip("←")]
  private GameObject arrowLeft_;
  [SerializeField]
  [Tooltip("→")]
  private GameObject arrowRight_;
  [SerializeField]
  [Tooltip("最少幅")]
  private int minWidth_ = 640;
  [SerializeField]
  private int marginWidth_ = 64;
  private bool initializing_ = true;
  private int current_;
  private bool isEnemy_;
  private bool isAutoClose_;
  private PopupSkillDetails.Param[] params_;
  private PopupSkillDetails.Cache[] caches_;
  private Action onClosed_;
  private int center_;
  private PopupSkillDetail[] details_;
  private const int NUM_DETAIL = 4;
  private UIPanel parentPanel_;
  private int? cellWidth_;
  private NGBattleManager battleManager_;
  private BE env_;
  private BL.Phase battlePhase_;
  private Queue<IEnumerator> queCoroutine_ = new Queue<IEnumerator>();
  private bool? isSea_;
  private Dictionary<UnitParameter.SkillGroup, string> dicTitle_;
  private GameObject prefabElementIcon;

  public static Future<GameObject> createPrefabLoader(bool isSea)
  {
    return new ResourceObject(isSea ? "Prefabs/UnitGUIs/PopupSkillDetailsSea" : "Prefabs/UnitGUIs/PopupSkillDetails").Load<GameObject>();
  }

  public static void show(
    GameObject prefab,
    PopupSkillDetails.Param param,
    bool isEnemy = false,
    Action onClosed = null,
    bool isAutoClose = false)
  {
    Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupSkillDetails>().initialize(param, isEnemy, onClosed, isAutoClose);
  }

  public static void show(
    GameObject prefab,
    PopupSkillDetails.Param[] param,
    int current,
    bool isEnemy = false,
    Action onClosed = null,
    bool isAutoClose = false)
  {
    Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupSkillDetails>().initialize(param, current, isEnemy, onClosed, isAutoClose);
  }

  private UIPanel parentPanel
  {
    get
    {
      return !Object.op_Inequality((Object) this.parentPanel_, (Object) null) ? (this.parentPanel_ = ((Component) ((Component) this).transform.parent).GetComponent<UIPanel>()) : this.parentPanel_;
    }
  }

  private int cellWidth
  {
    get
    {
      return !this.cellWidth_.HasValue ? (this.cellWidth_ = new int?(Mathf.CeilToInt(this.parentPanel.width))).Value : this.cellWidth_.Value;
    }
  }

  private int gridWidth => Mathf.Max(this.minWidth_, this.cellWidth) + this.marginWidth_;

  private NGBattleManager battleManager
  {
    get
    {
      return !Object.op_Inequality((Object) this.battleManager_, (Object) null) ? (this.battleManager_ = Singleton<NGBattleManager>.GetInstance()) : this.battleManager_;
    }
  }

  private BE env
  {
    get
    {
      return this.env_ == null ? (this.env_ = Object.op_Inequality((Object) this.battleManager, (Object) null) ? this.battleManager.environment : (BE) null) : this.env_;
    }
  }

  private bool isSea
  {
    get
    {
      return this.isSea_ ?? (this.isSea_ = new bool?(Singleton<NGGameDataManager>.GetInstance().IsSea)).Value;
    }
  }

  private Dictionary<UnitParameter.SkillGroup, string> dicTitle
  {
    get
    {
      return this.dicTitle_ ?? (this.dicTitle_ = ((IEnumerable<PopupSkillDetails.TitleTable>) this.titles_).ToDictionary<PopupSkillDetails.TitleTable, UnitParameter.SkillGroup, string>((Func<PopupSkillDetails.TitleTable, UnitParameter.SkillGroup>) (k => k.group), (Func<PopupSkillDetails.TitleTable, string>) (v => v.name)));
    }
  }

  private void initialize(
    PopupSkillDetails.Param param,
    bool isEnemy,
    Action onClosed,
    bool isAutoClose)
  {
    this.isEnemy_ = isEnemy;
    this.onClosed_ = onClosed;
    this.isAutoClose_ = isAutoClose;
    this.params_ = new PopupSkillDetails.Param[1]{ param };
    this.preInitialize();
  }

  private void initialize(
    PopupSkillDetails.Param[] param,
    int current,
    bool isEnemy,
    Action onClosed,
    bool isAutoClose)
  {
    this.current_ = current;
    this.isEnemy_ = isEnemy;
    this.onClosed_ = onClosed;
    this.isAutoClose_ = isAutoClose;
    this.params_ = param;
    this.preInitialize();
  }

  private void preInitialize() => this.setTopObject(((Component) this).gameObject);

  private IEnumerator Start()
  {
    PopupSkillDetails popupSkillDetails = this;
    bool bWait = true;
    while (popupSkillDetails.params_ == null)
    {
      bWait = false;
      yield return (object) null;
    }
    if (bWait)
      yield return (object) null;
    popupSkillDetails.initializeDetails();
    popupSkillDetails.StartCoroutine("doWaitCoroutine");
    while (popupSkillDetails.caches_[popupSkillDetails.current_].initiailze)
      yield return (object) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupSkillDetails).gameObject);
  }

  private void initializeDetails()
  {
    this.caches_ = new PopupSkillDetails.Cache[this.params_.Length];
    this.details_ = new PopupSkillDetail[Mathf.Min(4, this.params_.Length)];
    if (this.isAutoClose_)
      this.battlePhase_ = this.env.core.phaseState.state;
    this.arrowLeft_.SetActive(false);
    this.arrowRight_.SetActive(false);
    this.details_[0] = this.detail_;
    ((Behaviour) this.scroll_).enabled = this.params_.Length > 1;
    Vector3 size = this.scrollCollider_.size;
    size.x = (float) this.gridWidth;
    this.scrollCollider_.size = size;
    if (((Behaviour) this.scroll_).enabled)
    {
      GameObject gameObject1 = ((Component) this.detail_).gameObject;
      for (int index = 1; index < this.details_.Length; ++index)
      {
        this.details_[index] = gameObject1.Clone().GetComponent<PopupSkillDetail>();
        ((Component) this.details_[index]).gameObject.SetActive(false);
      }
      int index1 = Mathf.Max(0, this.current_ - 1);
      int num = Mathf.Min(this.params_.Length - 1, this.current_ + 1);
      int index2 = 0;
      while (index1 <= num)
      {
        this.details_[index2].resetParam(this.dicTitle, this.params_[index1], index1);
        GameObject gameObject2 = ((Component) this.details_[index2]).gameObject;
        this.resetParent(gameObject2, this.scrollNode_.transform);
        this.resetPosition(gameObject2, index1);
        this.resetIcons(this.details_[index2]);
        gameObject2.SetActive(true);
        ++index1;
        ++index2;
      }
      this.center_ = ((IEnumerable<PopupSkillDetail>) this.details_).FirstIndexOrNull<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => x.index == this.current_)).Value;
      this.resetScrollPosition();
      foreach (PopupSkillDetail detail in (IEnumerable<PopupSkillDetail>) ((IEnumerable<PopupSkillDetail>) this.details_).Where<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => ((Component) x).gameObject.activeSelf)).OrderBy<PopupSkillDetail, int>((Func<PopupSkillDetail, int>) (y => y.index != this.current_ ? 1 : 0)))
        this.resetIcons(detail);
    }
    else
    {
      this.detail_.resetParam(this.dicTitle, this.params_[0], 0);
      this.resetIcons(this.detail_);
      this.current_ = 0;
      this.center_ = 0;
    }
    this.resetCenter(this.current_);
    this.initializing_ = false;
  }

  private void resetParent(GameObject go, Transform trsParent)
  {
    go.transform.parent = trsParent;
    go.transform.localScale = Vector3.one;
    go.transform.localPosition = Vector3.zero;
    go.transform.localRotation = Quaternion.identity;
  }

  private void resetScrollPosition()
  {
    Vector3 localPosition = ((Component) this.scroll_).transform.localPosition;
    float x = ((Component) this.details_[this.center_]).transform.localPosition.x;
    localPosition.x = x * -1f;
    ((Component) this.scroll_).transform.localPosition = localPosition;
    UIPanel component = ((Component) this.scroll_).GetComponent<UIPanel>();
    Vector2 clipOffset = component.clipOffset;
    clipOffset.x = x;
    component.clipOffset = clipOffset;
    this.uiCenter_.CenterOn(((Component) this.details_[this.center_]).transform);
  }

  protected override void Update()
  {
    if (this.initializing_ || this.IsPush)
      return;
    base.Update();
    if (!((Behaviour) this.scroll_).enabled)
      return;
    if (this.isAutoClose_ && this.env.core.phaseState.state != this.battlePhase_)
    {
      this.IsPushAndSetEnd();
      Singleton<PopupManager>.GetInstance().closeAll();
    }
    else
    {
      GameObject gameObject = this.uiCenter_.centeredObject;
      int nextCurrent;
      if (Object.op_Equality((Object) gameObject, (Object) ((Component) this.details_[this.center_]).gameObject) && (this.scroll_.isDragging || (double) this.wheelScrollLog_.amount != 0.0))
      {
        float num = ((Component) this.scroll_).transform.localPosition.x * -1f - ((Component) this.details_[this.center_]).transform.localPosition.x;
        if ((double) Mathf.Abs(num) >= (double) this.gridWidth / 2.0)
        {
          nextCurrent = (double) num >= 0.0 ? this.details_[this.center_].index + 1 : this.details_[this.center_].index - 1;
          if (nextCurrent >= 0 && nextCurrent < this.params_.Length)
          {
            int? nullable = ((IEnumerable<PopupSkillDetail>) this.details_).FirstIndexOrNull<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => x.index == nextCurrent));
            if (nullable.HasValue)
            {
              gameObject = ((Component) this.details_[nullable.Value]).gameObject;
              this.uiCenter_.CenterOn(gameObject.transform);
              this.scroll_.DisableSpring();
            }
          }
        }
      }
      this.wheelScrollLog_.resetAmount();
      nextCurrent = this.current_;
      if (Object.op_Inequality((Object) gameObject, (Object) null) && Object.op_Inequality((Object) gameObject, (Object) ((Component) this.details_[this.center_]).gameObject))
      {
        PopupSkillDetail component = gameObject.GetComponent<PopupSkillDetail>();
        nextCurrent = component.index;
        int next = -1;
        if (this.current_ < component.index)
        {
          if (component.index < this.params_.Length - 1)
            next = component.index + 1;
        }
        else if (this.current_ > component.index && component.index > 0)
          next = component.index - 1;
        if (next >= 0 && !((IEnumerable<PopupSkillDetail>) this.details_).Any<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => x.index == next)))
        {
          PopupSkillDetail detail = this.details_[this.getFarDetail(component.index)];
          int num1 = detail.index < 0 ? 1 : 0;
          detail.resetParam(this.dicTitle, this.params_[next], next);
          if (num1 != 0)
          {
            this.resetParent(((Component) detail).gameObject, this.scrollNode_.transform);
            this.resetPosition(((Component) detail).gameObject, detail.index);
            ((Component) detail).gameObject.SetActive(true);
          }
          else
            this.resetPosition(((Component) detail).gameObject, detail.index);
          int num2 = 0;
          int num3 = 0;
          foreach (PopupSkillDetail popupSkillDetail in (IEnumerable<PopupSkillDetail>) ((IEnumerable<PopupSkillDetail>) this.details_).OrderBy<PopupSkillDetail, int>((Func<PopupSkillDetail, int>) (x => x.index)))
          {
            if (popupSkillDetail.index >= 0)
              ((Component) popupSkillDetail).transform.SetSiblingIndex(num3++);
            this.details_[num2++] = popupSkillDetail;
          }
          this.resetIcons(detail);
        }
      }
      if (this.current_ != nextCurrent)
      {
        this.center_ = ((IEnumerable<PopupSkillDetail>) this.details_).FirstIndexOrNull<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => x.index == nextCurrent)).Value;
        this.current_ = nextCurrent;
        this.resetCenter(nextCurrent);
        this.sePaging();
      }
      this.checkResizeScreenWidth();
    }
  }

  [Conditional("UNITY_EDITOR")]
  [Conditional("UNITY_STANDALONE_WIN")]
  private void checkResizeScreenWidth()
  {
    int num = Mathf.CeilToInt(this.parentPanel.width);
    if (this.cellWidth == num)
      return;
    this.cellWidth_ = new int?(num);
    foreach (PopupSkillDetail detail in this.details_)
      this.resetPosition(((Component) detail).gameObject, detail.index);
    this.resetScrollPosition();
  }

  private void OnDestroy()
  {
    if (this.details_ == null)
      return;
    foreach (Object @object in ((IEnumerable<PopupSkillDetail>) this.details_).Where<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => Object.op_Inequality((Object) x, (Object) null))).Select<PopupSkillDetail, GameObject>((Func<PopupSkillDetail, GameObject>) (y => ((Component) y).gameObject)))
      Object.Destroy(@object);
    this.details_ = (PopupSkillDetail[]) null;
  }

  private void resetCenter(int index)
  {
    this.btnChange_.SetActive(this.params_[index].onClickedChange != null);
    if (this.params_.Length <= 1)
      return;
    this.arrowLeft_.SetActive(index > 0);
    this.arrowRight_.SetActive(index < this.params_.Length - 1);
  }

  private void resetIcons(PopupSkillDetail detail)
  {
    PopupSkillDetails.Cache cache = this.caches_[detail.index];
    if (cache == null)
      this.caches_[detail.index] = cache = new PopupSkillDetails.Cache();
    if (cache.initiailze)
    {
      detail.resetIcons((Sprite) null, (Sprite) null, (Sprite) null);
      this.queCoroutine_.Enqueue(this.doResetIcons(detail, cache, detail.index));
    }
    else
      detail.resetIcons(cache.icon, cache.genreL, cache.genreR);
  }

  private IEnumerator doWaitCoroutine()
  {
    while (true)
    {
      while (this.queCoroutine_.Any<IEnumerator>())
      {
        IEnumerator e = this.queCoroutine_.Dequeue();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      yield return (object) null;
    }
  }

  private IEnumerator doResetIcons(
    PopupSkillDetail target,
    PopupSkillDetails.Cache cache,
    int index)
  {
    if (cache.initiailze)
    {
      PopupSkillDetails.Param obj = this.params_[index];
      switch (obj.group)
      {
        case UnitParameter.SkillGroup.Leader:
          yield return (object) this.doLoadLeader(cache, obj.skill, this.isEnemy_);
          break;
        case UnitParameter.SkillGroup.JobAbility:
          yield return (object) this.doLoadJobAbility(cache, obj.skill);
          break;
        default:
          yield return (object) this.doLoadSkill(cache, obj.skill);
          break;
      }
      cache.initiailze = false;
    }
    if (target.index == index)
      target.resetIcons(cache.icon, cache.genreL, cache.genreR);
  }

  private void resetPosition(GameObject go, int index)
  {
    Vector3 localPosition = ((Component) this.detail_).transform.localPosition;
    go.transform.localPosition = new Vector3((float) (this.gridWidth * index), localPosition.y, localPosition.z);
  }

  private int getFarDetail(int index)
  {
    int num1 = 0;
    int farDetail1 = -1;
    for (int farDetail2 = 0; farDetail2 < this.details_.Length; ++farDetail2)
    {
      int index1 = this.details_[farDetail2].index;
      if (index1 < 0)
        return farDetail2;
      int num2 = Math.Abs(index1 - index);
      if (num1 < num2)
      {
        num1 = num2;
        farDetail1 = farDetail2;
      }
    }
    return farDetail1;
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedChange()
  {
    if (((Behaviour) this.scroll_).enabled && this.scroll_.isDragging || this.IsPushAndSetEnd())
      return;
    this.params_[this.current_].onClickedChange();
  }

  public void onClickedClose()
  {
    if (this.IsPushAndSetEnd())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Action onClosed = this.onClosed_;
    if (onClosed == null)
      return;
    onClosed();
  }

  public void onClickedLeft()
  {
    if (this.IsPush)
      return;
    this.changeCenter(-1);
  }

  public void onClickedRight()
  {
    if (this.IsPush)
      return;
    this.changeCenter(1);
  }

  private bool IsPushAndSetEnd()
  {
    if (this.IsPushAndSet())
      return true;
    if (((Behaviour) this.scroll_).enabled)
    {
      ((Behaviour) this.scroll_).enabled = false;
      this.uiCenter_.CenterOn(((Component) this.details_[this.center_]).transform);
    }
    return false;
  }

  private void changeCenter(int dir)
  {
    int next = this.current_ + dir;
    if (this.current_ == next || next < 0 || next >= this.params_.Length)
      return;
    PopupSkillDetail popupSkillDetail = ((IEnumerable<PopupSkillDetail>) this.details_).FirstOrDefault<PopupSkillDetail>((Func<PopupSkillDetail, bool>) (x => x.index == next));
    if (!Object.op_Inequality((Object) popupSkillDetail, (Object) null))
      return;
    this.uiCenter_.CenterOn(((Component) popupSkillDetail).transform);
  }

  private IEnumerator doLoadSkill(PopupSkillDetails.Cache cache, BattleskillSkill s)
  {
    Future<Sprite> ld = s.LoadBattleSkillIcon();
    yield return (object) ld.Wait();
    cache.icon = ld.Result;
    ld = (Future<Sprite>) null;
    this.loadGenre(cache, s.genre1, s.genre2);
  }

  private IEnumerator doLoadLeader(PopupSkillDetails.Cache cache, BattleskillSkill s, bool bEnemy)
  {
    Future<Sprite> ld = s.LoadBattleSkillIcon(new BattleFuncs.InvestSkill()
    {
      skill = s,
      isEnemyIcon = bEnemy
    });
    yield return (object) ld.Wait();
    cache.icon = ld.Result;
    ld = (Future<Sprite>) null;
    this.loadGenre(cache, s.genre1, s.genre2);
  }

  private IEnumerator doLoadJobAbility(PopupSkillDetails.Cache cache, BattleskillSkill s)
  {
    Future<Sprite> ld = s.LoadBattleSkillIcon(new BattleFuncs.InvestSkill()
    {
      skill = s
    });
    yield return (object) ld.Wait();
    cache.icon = ld.Result;
    ld = (Future<Sprite>) null;
    this.loadGenre(cache, s.genre1, s.genre2);
  }

  private void loadGenre(PopupSkillDetails.Cache cache, BattleskillGenre? gL, BattleskillGenre? gR)
  {
    cache.genreL = gL.HasValue ? SkillGenreIcon.loadSprite(gL.Value, this.isSea) : (Sprite) null;
    cache.genreR = gR.HasValue ? SkillGenreIcon.loadSprite(gR.Value, this.isSea) : (Sprite) null;
  }

  private void sePaging()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.playSE(this.isSea ? "SE_1043" : "SE_1005");
  }

  public class Param
  {
    public UnitParameter.SkillGroup group { get; private set; }

    public object data { get; private set; }

    public BattleskillSkill skill { get; private set; }

    public int? level { get; private set; }

    public string message { get; private set; }

    public string remainingSkillAcquisition { get; private set; }

    public Action onClickedChange { get; private set; }

    public Param(PlayerUnitLeader_skills s)
    {
      this.group = UnitParameter.SkillGroup.Leader;
      this.data = (object) s;
      this.skill = s.skill;
      this.level = new int?(s.level);
    }

    public Param(PlayerUnitSkills s, UnitParameter.SkillGroup g)
    {
      this.group = g;
      this.data = (object) s;
      this.skill = s.skill;
      this.level = new int?(s.level);
    }

    public Param(BL.Skill s, UnitParameter.SkillGroup g)
    {
      this.group = g;
      this.data = (object) s;
      this.skill = s.skill;
      this.level = new int?(s.level);
    }

    public Param(GearGearSkill s)
    {
      this.group = UnitParameter.SkillGroup.Equip;
      this.data = (object) s;
      this.skill = s.skill;
      this.level = new int?(s.skill_level);
    }

    public Param(GearReisouSkill s)
    {
      this.group = UnitParameter.SkillGroup.Equip;
      this.data = (object) s;
      this.skill = s.skill;
      this.level = new int?(s.skill_level);
    }

    public Param(PlayerAwakeSkill s, string msgBottom = null, bool levelMax = false)
    {
      this.group = UnitParameter.SkillGroup.Extra;
      this.data = (object) s;
      BattleskillSkill battleskillSkill;
      if (MasterData.BattleskillSkill.TryGetValue(s.skill_id, out battleskillSkill))
      {
        this.skill = battleskillSkill;
        this.level = new int?(s.level);
        if (levelMax)
          this.level = new int?();
      }
      this.message = msgBottom;
    }

    public Param(BattleskillSkill s, UnitParameter.SkillGroup g, int? lv, string msgBottom)
    {
      this.group = g;
      this.data = (object) s;
      this.skill = s;
      this.level = lv;
      this.message = msgBottom;
    }

    public Param(OverkillersSkillRelease s, int? unityValue = null)
    {
      this.group = UnitParameter.SkillGroup.Overkillers;
      this.data = (object) s;
      this.skill = s.skill;
      this.message = Consts.GetInstance().popup_004_OverkillersSkill_Supplement;
      if (unityValue.HasValue && unityValue.Value >= s.unity_value)
        this.level = new int?(this.skill.upper_level);
      else
        this.message = Consts.Format(Consts.GetInstance().popup_004_OverkillersSkill_ReleaseCondition, (IDictionary) new Hashtable()
        {
          {
            (object) "unity",
            (object) s.unity_value
          }
        }) + "\n" + this.message;
    }

    public Param(PlayerUnitJob_abilities s)
    {
      this.group = UnitParameter.SkillGroup.JobAbility;
      this.data = (object) s.master;
      this.skill = s.skill;
      this.level = new int?(s.level);
    }

    public Param(BattleskillSkill s, UnitParameter.SkillGroup g, int? lv = null)
    {
      this.group = g;
      this.data = (object) s;
      this.skill = s;
      this.level = lv;
    }

    public static PopupSkillDetails.Param createByUnitView(
      UnitSkillAwake s,
      PlayerUnit unit,
      Action onClickedChange = null,
      bool bDisabledStatus = false)
    {
      BattleskillSkill skill = s.skill;
      PopupSkillDetails.Param byUnitView = new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Extra);
      Consts instance = Consts.GetInstance();
      if ((double) unit.trust_rate < (double) unit.trust_max_rate)
      {
        byUnitView.message = Consts.Format(unit.unit.IsSea ? instance.popup_004_ExtraSkill_affection_condition : instance.popup_004_ExtraSkill_affection_condition_second, (IDictionary) new Hashtable()
        {
          {
            (object) "percent",
            (object) s.need_affection
          }
        });
      }
      else
      {
        byUnitView.level = new int?(skill.upper_level);
        byUnitView.message = instance.popup_004_ExtraSkill_affection_complete;
      }
      if (!bDisabledStatus)
      {
        int num1 = Mathf.CeilToInt(unit.trust_max_rate / instance.TRUST_RATE_LEVEL_SIZE);
        int num2 = 0;
        if ((double) unit.trust_rate != (double) unit.trust_max_rate)
          num2 = num1 - Mathf.FloorToInt(unit.trust_rate / instance.TRUST_RATE_LEVEL_SIZE);
        byUnitView.remainingSkillAcquisition = Consts.Format(instance.popup_Skill_Detail_Remaining_Skill_Acquisition, (IDictionary) new Hashtable()
        {
          {
            (object) "count",
            (object) num2
          },
          {
            (object) "max_count",
            (object) num1
          }
        });
      }
      byUnitView.onClickedChange = onClickedChange;
      return byUnitView;
    }

    public static PopupSkillDetails.Param createBySkillView(
      PlayerAwakeSkill s,
      Action onClickedChange = null)
    {
      string msgBottom = Consts.Format(Consts.GetInstance().popup_004_ExtraSkill_affection_condition_category, (IDictionary) new Hashtable()
      {
        {
          (object) "condition",
          (object) AwakeSkillCategory.GetEquipableText(s.masterData.awake_skill_category_id)
        }
      });
      return new PopupSkillDetails.Param(s, msgBottom)
      {
        onClickedChange = onClickedChange
      };
    }

    public static PopupSkillDetails.Param createBySEASkillView(
      PlayerUnitSkills s,
      bool bUnlocked,
      int num)
    {
      return PopupSkillDetails.Param.createBySEASkillView(s.skill, bUnlocked, num);
    }

    public static PopupSkillDetails.Param createBySEASkillView(BL.Skill s, int num)
    {
      return PopupSkillDetails.Param.createBySEASkillView(s.skill, true, num);
    }

    public static PopupSkillDetails.Param createBySEASkillView(
      BattleskillSkill s,
      bool bUnlocked,
      int num)
    {
      Consts instance = Consts.GetInstance();
      string msgBottom = bUnlocked ? instance.SEA_SKILL_DETAIL_MESSAGE : instance.SEA_SKILL_DETAIL_UNLOCK_CONDITIONS + "\n" + instance.SEA_SKILL_DETAIL_MESSAGE;
      return new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.SEA, !bUnlocked || num <= 0 ? new int?() : new int?(num), msgBottom);
    }

    public static PopupSkillDetails.Param createByShopSkillView(
      PlayerAwakeSkill s,
      Action onClickedChange = null)
    {
      string msgBottom = Consts.Format(Consts.GetInstance().popup_004_ExtraSkill_affection_condition_category, (IDictionary) new Hashtable()
      {
        {
          (object) "condition",
          (object) AwakeSkillCategory.GetEquipableText(s.masterData.awake_skill_category_id)
        }
      });
      return new PopupSkillDetails.Param(s, msgBottom, true)
      {
        onClickedChange = onClickedChange
      };
    }
  }

  [Serializable]
  private class TitleTable
  {
    public UnitParameter.SkillGroup group;
    public string name;
  }

  private class Cache
  {
    public bool initiailze = true;
    public Sprite icon;
    public Sprite genreL;
    public Sprite genreR;
  }
}
