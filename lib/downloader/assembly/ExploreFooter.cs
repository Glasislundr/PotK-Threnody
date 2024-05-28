// Decompiled with JetBrains decompiler
// Type: ExploreFooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExploreFooter : MonoBehaviour
{
  [SerializeField]
  private GameObject mUnitIconPrefab;
  [SerializeField]
  private GameObject[] mUnitIconAnchors;
  [Space(8f)]
  [SerializeField]
  private NGTweenGaugeFillAmount mProgressGauge;
  [SerializeField]
  private UILabel mProgressLbl;
  [Space(8f)]
  [SerializeField]
  private Transform mChallengeButtonAnchor;
  [Space(8f)]
  [SerializeField]
  private NGxScroll mLogScrollView;
  [Space(8f)]
  [SerializeField]
  private GameObject mBoostAnchor;
  [Space(8f)]
  [SerializeField]
  private UILabel mWinRateLbl;
  [Space(8f)]
  [SerializeField]
  private Transform mBoxInEffectTarget;
  [SerializeField]
  private GameObject mBoxMaxBadge;
  private ExploreSceneManager mManager;
  private IEnumerator mUpdateLbl;
  private UIButton mChallengeButton;
  private Animator mChallengeEffectAnime;
  private GameObject exploreChallengePopupPrefab;
  private GameObject logItemPrefab;
  private List<GameObject> mUnitIcons = new List<GameObject>();
  private LogData mLastGotLog;
  private int mLastProgress;

  private void Awake()
  {
    this.mManager = Singleton<ExploreSceneManager>.GetInstance();
    this.mUpdateLbl = this.updateProgressLabel(0.33f);
  }

  public IEnumerator Initialize()
  {
    yield return (object) this.LoadResources();
    this.updateProgressBar();
    this.updateWinRateLabel();
    this.updateBoxMaxBadge();
    this.RefreshLog();
  }

  private void Update()
  {
    this.updateProgressBar();
    this.updateWinRateLabel();
    this.updateBoxMaxBadge();
    this.updateLog();
  }

  public IEnumerator UpdateDeckUnitIconsAsync()
  {
    PlayerUnit[] deck = Singleton<ExploreDataManager>.GetInstance().GetExploreUnits();
    this.destroyAllUnitIcons();
    for (int i = 0; i < this.mUnitIconAnchors.Length; ++i)
    {
      UnitIcon icon = this.mUnitIconPrefab.CloneAndGetComponent<UnitIcon>(this.mUnitIconAnchors[i].transform);
      this.mUnitIcons.Add(((Component) icon).gameObject);
      ((UIButtonColor) icon.Button).isEnabled = false;
      if (i >= deck.Length || deck[i] == (PlayerUnit) null)
      {
        icon.SetEmpty();
      }
      else
      {
        yield return (object) icon.SetUnit(deck[i], deck[i].unit.GetElement(), false);
        icon.setLevelText(deck[i]);
        icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        icon = (UnitIcon) null;
      }
    }
  }

  private void destroyAllUnitIcons()
  {
    foreach (GameObject mUnitIcon in this.mUnitIcons)
    {
      mUnitIcon.transform.parent = (Transform) null;
      Object.Destroy((Object) mUnitIcon);
    }
    this.mUnitIcons.Clear();
  }

  private void updateProgressBar()
  {
    if (Object.op_Equality((Object) this.mProgressGauge, (Object) null))
      return;
    int progress = Singleton<ExploreDataManager>.GetInstance().Progress;
    this.mProgressGauge.setValue(progress, 1000000, false, -1f, -1f);
    if (this.mManager.IsSceneActive)
      this.setChallengeEnable(progress == 1000000);
    this.mLastProgress = progress;
    this.mBoostAnchor.SetActive(false);
  }

  private void setChallengeEnable(bool enable)
  {
    if (Singleton<NGGameDataManager>.GetInstance().challenge_point < 1)
      enable = false;
    if (!((UIButtonColor) this.mChallengeButton).isEnabled & enable)
    {
      if (!((UIButtonColor) this.mChallengeButton).isEnabled)
        this.mManager.PlaySe("SE_2407");
      this.mChallengeEffectAnime.Play("explore_BtnChallenge_Idle");
      ((UIButtonColor) this.mChallengeButton).isEnabled = true;
    }
    else
    {
      if (!((UIButtonColor) this.mChallengeButton).isEnabled || enable)
        return;
      this.mChallengeEffectAnime.Play("explore_BtnChallenge_Disable");
      ((UIButtonColor) this.mChallengeButton).isEnabled = false;
    }
  }

  private IEnumerator LoadResources()
  {
    ExploreFooter exploreFooter = this;
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Inequality((Object) exploreFooter.mProgressGauge, (Object) null))
    {
      f = new ResourceObject("Prefabs/explore033_Top/popup_ExploreChallenge").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exploreFooter.exploreChallengePopupPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) exploreFooter.mChallengeButtonAnchor, (Object) null))
    {
      f = new ResourceObject("Prefabs/explore033_Top/Btn_Challenge").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = f.Result.Clone(exploreFooter.mChallengeButtonAnchor);
      exploreFooter.mChallengeEffectAnime = gameObject.GetComponent<Animator>();
      exploreFooter.mChallengeButton = gameObject.GetComponentInChildren<UIButton>();
      exploreFooter.mChallengeButton.onClick.Add(new EventDelegate((MonoBehaviour) exploreFooter, "OnChallengeButton"));
      ((UIButtonColor) exploreFooter.mChallengeButton).isEnabled = false;
      f = (Future<GameObject>) null;
    }
    f = new ResourceObject("Prefabs/explore033_Top/dir_explore_log").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exploreFooter.logItemPrefab = f.Result;
    f = (Future<GameObject>) null;
  }

  private IEnumerator updateProgressLabel(float span)
  {
    while (true)
    {
      this.mProgressLbl.SetTextLocalize((int) ((double) Singleton<ExploreDataManager>.GetInstance().Progress / 1000000.0 * 100.0));
      yield return (object) new WaitForSeconds(span);
    }
  }

  public void updateWinRateLabel()
  {
    if (Object.op_Equality((Object) this.mWinRateLbl, (Object) null))
      return;
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    if (!instance.IsWinRateUpdate)
      return;
    this.mWinRateLbl.SetTextLocalize(instance.GetWinRate());
    instance.IsWinRateUpdate = false;
  }

  public void updateBoxMaxBadge()
  {
    if (Object.op_Equality((Object) this.mBoxMaxBadge, (Object) null))
      return;
    this.mBoxMaxBadge.SetActive(false);
    if (!Singleton<ExploreDataManager>.GetInstance().ExploreBox.IsRewardsMax)
      return;
    this.mBoxMaxBadge.SetActive(true);
  }

  public void OnChallengeButton()
  {
    if (!this.mManager.Task.IsStateWait && !this.mManager.Task.IsStateExlore && !this.mManager.Task.IsStateLostWait || Singleton<PopupManager>.GetInstance().isOpen || Singleton<NGGameDataManager>.GetInstance().challenge_point < 1)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_2413");
    this.StartCoroutine(this.openChallengePopup());
  }

  public IEnumerator openChallengePopup()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ExploreFooter footer = this;
    GameObject clone;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Singleton<PopupManager>.GetInstance().dismiss();
      clone.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(clone, isCloned: true, isNonSe: true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isNonSe: true);
    clone = footer.exploreChallengePopupPrefab.Clone();
    clone.SetActive(false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) clone.GetComponent<ExploreChallengePopup>().Initialize(footer);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator changeExplore033ChallengeScene(
    ChallengeNpc gladiator,
    GameCore.ColosseumResult battle_result)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    yield return (object) null;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Explore033ChallengeScene.changeScene(gladiator, battle_result);
  }

  public void RefreshLog()
  {
    this.mLogScrollView.Clear();
    foreach (LogData logData in Singleton<ExploreDataManager>.GetInstance().GetAllLog())
    {
      ExploreLogItem component = this.logItemPrefab.CloneAndGetComponent<ExploreLogItem>();
      this.mLogScrollView.Add(((Component) component).gameObject, true);
      component.SetMessage(logData.Message, logData.Color);
      this.mLastGotLog = logData;
    }
    this.mLogScrollView.ResolvePosition(((Component) this.mLogScrollView.grid).transform.childCount - 1);
  }

  private void updateLog()
  {
    if (Object.op_Equality((Object) this.logItemPrefab, (Object) null))
      return;
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    if (instance.IsLatestLog(this.mLastGotLog))
      return;
    foreach (LogData newLogData in instance.GetNewLogDatas(this.mLastGotLog))
    {
      this.mLastGotLog = newLogData;
      ExploreLogItem component = this.logItemPrefab.CloneAndGetComponent<ExploreLogItem>();
      this.mLogScrollView.Add(((Component) component).gameObject, true);
      component.SetMessage(this.mLastGotLog.Message, this.mLastGotLog.Color);
      if (((Component) this.mLogScrollView.grid).transform.childCount > 20)
      {
        Transform child = ((Component) this.mLogScrollView.grid).transform.GetChild(0);
        child.SetParent((Transform) null);
        Object.Destroy((Object) ((Component) child).gameObject);
      }
    }
    this.mLogScrollView.ResolvePosition(((Component) this.mLogScrollView.grid).transform.childCount - 1);
  }

  public Vector3 GetExploreBoxButtonWorldPos() => this.mBoxInEffectTarget.position;

  private void OnEnable()
  {
    this.updateProgressBar();
    this.updateWinRateLabel();
    if (!Object.op_Inequality((Object) this.mProgressGauge, (Object) null))
      return;
    this.StartCoroutine(this.mUpdateLbl);
  }

  private void OnDisable()
  {
    if (!Object.op_Inequality((Object) this.mProgressGauge, (Object) null))
      return;
    this.StopCoroutine(this.mUpdateLbl);
  }
}
