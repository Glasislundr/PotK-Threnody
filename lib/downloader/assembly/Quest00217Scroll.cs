// Decompiled with JetBrains decompiler
// Type: Quest00217Scroll
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
[AddComponentMenu("Scenes/quest002_17/VScrollItem")]
public class Quest00217Scroll : BannerBase
{
  [SerializeField]
  protected Quest00217Scroll.ColorSet colorNormal;
  [SerializeField]
  protected Quest00217Scroll.ColorSet colorDisabled;
  [SerializeField]
  public UISprite Clear;
  [SerializeField]
  public UISprite New;
  [SerializeField]
  protected FloatButton Button;
  [SerializeField]
  protected GameObject Highlighting;
  [SerializeField]
  protected GameObject ClearedToday;
  [SerializeField]
  protected UIUnityMaskRenderer EffectRenderer;
  [SerializeField]
  [Tooltip("長押し説明が有る時にON")]
  protected GameObject objHasDescriptions;
  [SerializeField]
  private GameObject LevelLimit;
  [SerializeField]
  private UILabel LevelLimitText;
  [SerializeField]
  private GameObject dummyCollider;
  [SerializeField]
  protected GameObject dirQuestLock;
  [SerializeField]
  protected UILabel txtLock;
  protected bool initialized;
  protected Quest00217Scroll.Parameter initData;
  protected CampaignQuest.RankingEventTerm rankingEventTerm;
  protected bool inside_ = true;
  protected bool enabledCountdown;
  protected bool effect_;
  protected GameObject objEffect_;
  protected DateTime timeGoal;
  protected int lastSeconds;
  private DateTime lastServerTime;
  private float lastLocalTime;
  protected string loadPath;
  protected bool loadEnabledHighlighting;

  public CampaignQuest.RankingEventTerm RankingEventTerm => this.rankingEventTerm;

  public virtual IEnumerator InitScroll(Quest00217Scroll.Parameter param, DateTime serverTime)
  {
    Quest00217Scroll quest00217Scroll = this;
    quest00217Scroll.Setup(param, serverTime);
    IEnumerator e = quest00217Scroll.SetAndCreate_BannerSprite(quest00217Scroll.loadPath, quest00217Scroll.IdleSprite, quest00217Scroll.Highlighting, quest00217Scroll.loadEnabledHighlighting);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void Setup(Quest00217Scroll.Parameter param, DateTime serverTime)
  {
    this.initialized = false;
    this.initData = param;
    this.inside_ = ((Component) this).gameObject.activeSelf;
    this.rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
    this.SetEndTime(param.lastEndTime.HasValue ? param.lastEndTime.Value : param.extra.today_day_end_at);
    this.resetLocalTime(serverTime);
    ((Behaviour) this.Button).enabled = true;
    bool flag1 = param.isNew;
    bool flag2 = param.isClear;
    bool flag3 = param.isHighlighting;
    if (param.isNotice && param.startTime.HasValue && param.startTime.Value > serverTime)
    {
      flag2 = false;
      flag1 = false;
      flag3 = false;
      ((Behaviour) this.Button).enabled = false;
      this.SetEndTimeAsStart(param.startTime.Value);
      this.startCountdown(param.startTime.Value - serverTime);
    }
    QuestExtra.SeekType seekType = param.seek != QuestExtra.SeekType.None ? param.seek : QuestExtra.toSeekType(param.extra.seek_index);
    QuestExtraS questExtraS = param.extra.quest_extra_s;
    this.loadPath = this.SetSpritePath(questExtraS, questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM, seekType, questExtraS.quest_ll?.ID);
    this.loadEnabledHighlighting = flag3;
    if (this.initData.entryConditionID == 0)
      this.SetScrollButtonCondition(param.extra, serverTime, seekType);
    ((Component) this.Clear).gameObject.SetActive(flag2);
    if (!param.lastEndTime.HasValue)
    {
      QuestScoreCampaignProgress campaignProgress = Array.Find<QuestScoreCampaignProgress>(SMManager.Get<QuestScoreCampaignProgress[]>(), (Predicate<QuestScoreCampaignProgress>) (x => x.quest_extra_l == param.extra.quest_extra_s.quest_l.ID));
      if (campaignProgress != null)
      {
        if (campaignProgress.is_open && serverTime < campaignProgress.end_at)
        {
          this.EndTime = campaignProgress.end_at;
          this.rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
        }
        else if (!campaignProgress.is_open && serverTime < campaignProgress.final_at)
        {
          this.EndTime = campaignProgress.final_at;
          this.rankingEventTerm = CampaignQuest.RankingEventTerm.aggregate;
          ((Behaviour) this.Button).enabled = false;
        }
        else if (!campaignProgress.is_open && serverTime < campaignProgress.latest_end_at)
        {
          this.EndTime = campaignProgress.latest_end_at;
          this.rankingEventTerm = CampaignQuest.RankingEventTerm.receive;
        }
      }
    }
    this.SetTime(serverTime, this.rankingEventTerm);
    this.LevelLimit.SetActive(false);
    this.dummyCollider.SetActive(false);
    QuestExtraM m = param.extra.quest_extra_s.quest_m;
    QuestExtraReleaseConditionsPlayer conditionsPlayer = ((IEnumerable<QuestExtraReleaseConditionsPlayer>) MasterData.QuestExtraReleaseConditionsPlayerList).FirstOrDefault<QuestExtraReleaseConditionsPlayer>((Func<QuestExtraReleaseConditionsPlayer, bool>) (x => x.quest_m == m));
    if (conditionsPlayer != null && conditionsPlayer.comparison_operator == ">=")
    {
      int level = SMManager.Observe<Player>().Value.level;
      int? playerLevel = conditionsPlayer.player_level;
      int valueOrDefault = playerLevel.GetValueOrDefault();
      if (level < valueOrDefault & playerLevel.HasValue)
      {
        this.LevelLimit.SetActive(true);
        this.LevelLimitText.text = conditionsPlayer.player_level.ToString();
        flag1 = false;
        this.isConditonEffective = false;
        ((UIButtonColor) this.BtnFormation).isEnabled = false;
        this.dummyCollider.SetActive(true);
      }
    }
    if (Object.op_Inequality((Object) this.Highlighting, (Object) null))
      this.Highlighting.SetActive(false);
    ((Component) this.New).gameObject.SetActive(flag1);
    this.setActiveHasDescriptions(false);
    this.setClearedToday(param.isClearedToday);
    this.setQuestLock();
    this.initialized = true;
  }

  protected string SetSpritePath(
    QuestExtraS masterS,
    int L,
    int M,
    QuestExtra.SeekType seek_type,
    int? LL)
  {
    int num;
    switch (seek_type)
    {
      case QuestExtra.SeekType.M:
        num = M;
        break;
      case QuestExtra.SeekType.LL:
        num = LL.Value;
        break;
      default:
        num = L;
        break;
    }
    int id = num;
    return BannerBase.GetSpriteIdlePathQuest(masterS, id, seek_type);
  }

  public IEnumerator SetAndCreate_BannerSprite()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00217Scroll quest00217Scroll = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) quest00217Scroll.SetAndCreate_BannerSprite(quest00217Scroll.loadPath, quest00217Scroll.IdleSprite, quest00217Scroll.Highlighting, quest00217Scroll.loadEnabledHighlighting);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator SetAndCreate_BannerSprite(
    string path,
    UI2DSprite obj,
    GameObject highlighting,
    bool enabledHighlighting)
  {
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
      path = string.Format("Prefabs/Banners/ExtraQuest/M/1/Specialquest_idle");
    Future<Texture2D> future = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      obj.sprite2D = sprite;
      if (Object.op_Inequality((Object) highlighting, (Object) null))
      {
        this.EffectRenderer.SetTexture("_MaskTex", ((UIWidget) obj).mainTexture);
        this.Highlighting.SetActive(enabledHighlighting);
      }
    }
  }

  protected void SetScrollButtonCondition(
    PlayerExtraQuestS extra,
    DateTime serverTime,
    QuestExtra.SeekType seekType)
  {
    EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene(extra, ((Component) this).gameObject, serverTime, seekType)));
  }

  public void changeScene(
    PlayerExtraQuestS extra,
    GameObject obj,
    DateTime serverTime,
    QuestExtra.SeekType seekType)
  {
    this.StartCoroutine(this.QuestTimeCompare(extra, obj, serverTime, seekType));
  }

  public IEnumerator QuestTimeCompare(
    PlayerExtraQuestS StageData,
    GameObject obj,
    DateTime serverTime,
    QuestExtra.SeekType seekType)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    serverTime = ServerTime.NowAppTimeAddDelta();
    this.resetLocalTime(serverTime);
    DateTime dateTime = serverTime;
    DateTime? nullable = this.initData.lastEndTime.HasValue ? this.initData.lastEndTime : new DateTime?(StageData.today_day_end_at);
    if ((nullable.HasValue ? (dateTime < nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
    {
      if (this.initData.isSkipSortie)
      {
        QuestExtraS questExtraS = StageData.quest_extra_s;
        Quest002201Scene.changeScene(false, questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM);
      }
      else
      {
        switch (seekType)
        {
          case QuestExtra.SeekType.L:
            if (Array.Find<QuestScoreCampaignProgress>(SMManager.Get<QuestScoreCampaignProgress[]>(), (Predicate<QuestScoreCampaignProgress>) (x => x.quest_extra_l == StageData.quest_extra_s.quest_l_QuestExtraL)) != null)
            {
              Quest00226Scene.ChangeScene(StageData._quest_extra_s);
              break;
            }
            Quest00219Scene.ChangeScene(StageData._quest_extra_s);
            break;
          case QuestExtra.SeekType.LL:
            Quest00218Scene.changeScene(StageData.quest_ll.ID);
            break;
          default:
            QuestExtraS questExtraS1 = StageData.quest_extra_s;
            Quest00220Scene.ChangeScene00220(false, questExtraS1.quest_l_QuestExtraL, questExtraS1.quest_m_QuestExtraM, true);
            break;
        }
      }
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
      time_popup = (Future<GameObject>) null;
    }
  }

  protected void startCountdown(TimeSpan tspan)
  {
    this.timeGoal = DateTime.Now.Add(tspan);
    this.lastSeconds = tspan.Seconds - 1;
    this.enabledCountdown = true;
  }

  protected void resetLocalTime(DateTime serverTime)
  {
    this.lastServerTime = serverTime;
    this.lastLocalTime = Time.time;
  }

  protected DateTime nowLocalTime
  {
    get => this.lastServerTime.AddSeconds((double) Time.time - (double) this.lastLocalTime);
  }

  private void Update() => this.updateCountdown(false);

  protected virtual void updateCountdown(bool immediate)
  {
    if (!this.enabledCountdown)
      return;
    TimeSpan tspan = this.timeGoal - DateTime.Now;
    if (tspan.Ticks <= 0L)
    {
      this.enabledCountdown = false;
      if (!immediate)
        this.duplicateEffectFadeOut(((Component) this).gameObject, ((Component) this).gameObject, 0.5f);
      ((Component) this.Clear).gameObject.SetActive(this.initData.isClear);
      ((Component) this.New).gameObject.SetActive(this.initData.isNew);
      if (Object.op_Inequality((Object) this.Highlighting, (Object) null))
        this.Highlighting.SetActive(this.initData.isHighlighting);
      this.SetEndTime(this.initData.lastEndTime.HasValue ? this.initData.lastEndTime.Value : this.initData.extra.today_day_end_at);
      this.SetTime(this.nowLocalTime, this.rankingEventTerm);
      if (immediate)
        ((Behaviour) this.Button).enabled = true;
      else
        this.fadeIn(0.5f);
    }
    else
    {
      if (!(this.lastSeconds != tspan.Seconds | immediate))
        return;
      this.lastSeconds = tspan.Seconds;
      this.updateTime(tspan, immediate ? 0.0f : 0.5f);
    }
  }

  protected virtual GameObject duplicateEffectFadeOut(
    GameObject parentObj,
    GameObject originalObj,
    float duration)
  {
    GameObject gameObject = NGUITools.AddChild(parentObj, originalObj);
    this.objEffect_ = gameObject;
    gameObject.SetActive(true);
    Object.Destroy((Object) gameObject.GetComponent<Quest002171Scroll>());
    gameObject.AddComponent<Quest00217ScrollFadeOut>().init(duration, 100, new EventDelegate((EventDelegate.Callback) (() =>
    {
      Object.Destroy((Object) this.objEffect_);
      this.objEffect_ = (GameObject) null;
    })));
    return gameObject;
  }

  protected virtual void fadeIn(float duration)
  {
    UIWidget uiWidget = Quest00217ScrollFadeOut.setWidget(((Component) this).gameObject, 0);
    if (!Object.op_Inequality((Object) uiWidget, (Object) null))
      return;
    ((UIRect) uiWidget).alpha = 0.0f;
    this.effect_ = true;
    TweenAlpha ta = TweenAlpha.Begin(((Component) this).gameObject, duration, 1f);
    ((UITweener) ta).SetOnFinished((EventDelegate.Callback) (() =>
    {
      Object.Destroy((Object) ta);
      Object.Destroy((Object) uiWidget);
      ((Behaviour) this.Button).enabled = true;
      this.effect_ = false;
    }));
  }

  private void OnEnable()
  {
    if (!this.initialized)
      return;
    if (this.enabledCountdown)
      this.updateCountdown(true);
    else
      this.SetTime(this.nowLocalTime, this.rankingEventTerm);
  }

  private void OnDisable()
  {
    if (Object.op_Inequality((Object) this.objEffect_, (Object) null))
    {
      Object.Destroy((Object) this.objEffect_);
      this.objEffect_ = (GameObject) null;
    }
    if (this.effect_)
    {
      UIWidget component1 = ((Component) this).gameObject.GetComponent<UIWidget>();
      TweenAlpha component2 = ((Component) this).gameObject.GetComponent<TweenAlpha>();
      component2.value = 1f;
      Object.Destroy((Object) component2);
      Object.Destroy((Object) component1);
      ((Behaviour) this.Button).enabled = true;
      this.effect_ = false;
    }
    this.terminateAnimation();
  }

  public void onInside()
  {
    this.inside_ = true;
    ((Component) this).gameObject.SetActive(true);
  }

  public void onOutside()
  {
    this.inside_ = false;
    ((Component) this).gameObject.SetActive(false);
  }

  public void setEffectNoDescription()
  {
    if (Object.op_Equality((Object) this.Highlighting, (Object) null))
      return;
    Animator component = this.Highlighting.GetComponent<Animator>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    bool activeSelf = this.Highlighting.activeSelf;
    this.Highlighting.SetActive(true);
    component.Play("Banner_Effect_long_tap");
    this.StartCoroutine(this.coWaitEffectNoDescription(component, activeSelf));
  }

  private IEnumerator coWaitEffectNoDescription(Animator animator, bool activeObject)
  {
    yield return (object) new WaitForAnimation(animator, timeout: 2f);
    if (!activeObject)
      ((Component) animator).gameObject.SetActive(false);
  }

  public void setActiveHasDescriptions(bool bActive = true)
  {
    if (!Object.op_Inequality((Object) this.objHasDescriptions, (Object) null))
      return;
    this.objHasDescriptions.SetActive(bActive);
  }

  private void setClearedToday(bool bActive = true)
  {
    if (Object.op_Equality((Object) this.ClearedToday, (Object) null) || this.ClearedToday.activeSelf == bActive)
      return;
    this.ClearedToday.SetActive(bActive);
    Quest00217Scroll.ColorSet colorSet = bActive ? this.colorDisabled : this.colorNormal;
    ((UIWidget) ((Component) this.Button).gameObject.GetComponent<UI2DSprite>()).color = ((UIButtonColor) this.Button).defaultColor = colorSet.default_;
    ((UIButtonColor) this.Button).hover = colorSet.hover_;
    ((UIButtonColor) this.Button).pressed = colorSet.pressed_;
  }

  protected void setQuestLock()
  {
    bool flag = this.initData.entryConditionID != 0;
    if (Object.op_Equality((Object) this.dirQuestLock, (Object) null))
      return;
    this.dirQuestLock.SetActive(flag);
    if (flag)
      this.setQuestLockText();
    if (Object.op_Inequality((Object) this.ClearedToday, (Object) null) && this.ClearedToday.activeSelf)
      return;
    Quest00217Scroll.ColorSet colorSet = flag ? this.colorDisabled : this.colorNormal;
    ((UIWidget) ((Component) this.Button).gameObject.GetComponent<UI2DSprite>()).color = ((UIButtonColor) this.Button).defaultColor = colorSet.default_;
    ((UIButtonColor) this.Button).hover = colorSet.hover_;
    ((UIButtonColor) this.Button).pressed = colorSet.pressed_;
  }

  protected virtual void setQuestLockText()
  {
    this.txtLock.SetTextLocalize(MasterData.QuestExtraEntryConditions[this.initData.entryConditionID].text);
  }

  public class Parameter
  {
    private object data_;
    public QuestExtraDescription[] descriptions;
    public bool isClear;
    public bool isNew;
    public QuestExtra.SeekType seek;
    public bool isNotice;
    public DateTime? startTime;
    public DateTime? lastEndTime;
    public bool isHighlighting;
    public bool isClearedToday;
    public bool isSkipSortie;
    public int entryConditionID;

    public Parameter(PlayerExtraQuestS data) => this.data_ = (object) data;

    public Parameter(EventInfo data) => this.data_ = (object) data;

    public Parameter(SM.TowerPeriod data) => this.data_ = (object) data;

    public Parameter(CorpsPeriod data) => this.data_ = (object) data;

    public Parameter()
    {
    }

    public void setMainData(PlayerExtraQuestS data) => this.data_ = (object) data;

    public PlayerExtraQuestS extra => this.data_ as PlayerExtraQuestS;

    public EventInfo eventInfo => this.data_ as EventInfo;

    public SM.TowerPeriod towerInfo => this.data_ as SM.TowerPeriod;

    public CorpsPeriod corpsInfo => this.data_ as CorpsPeriod;
  }

  [Serializable]
  protected class ColorSet
  {
    public Color default_;
    public Color hover_;
    public Color pressed_;
  }
}
