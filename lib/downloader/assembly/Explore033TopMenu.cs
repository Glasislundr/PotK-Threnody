// Decompiled with JetBrains decompiler
// Type: Explore033TopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033TopMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject mTransitionAnchor;
  [SerializeField]
  private GameObject mTransitionFullAnchor;
  [SerializeField]
  private ExploreFooter mExploreFooter;
  [SerializeField]
  private UILabel mTxtNumDungeon;
  [SerializeField]
  private UILabel mTxtTitleDungeon;
  [SerializeField]
  private UILabel mTxtTicketNum;
  [SerializeField]
  private UIButton mFloorSelectButton;
  [SerializeField]
  private Explore033MiniMap mMiniMap;
  [SerializeField]
  public Explore033TopMenu.FogSetting[] mFogSetting;
  private GameObject mDeckEditPopupPrefab;
  private GameObject mRankingPopupPrefab;
  private GameObject mRewardBoxPopupPrefab;
  private GameObject mFloorSelectPopupPrefab;

  public IEnumerator InitAsync()
  {
    yield return (object) this.loadDeckEditPopup();
    yield return (object) this.loadRewardBoxPopup();
    yield return (object) this.loadFloorSelectPopup();
    yield return (object) this.setupTransitionParts();
  }

  public IEnumerator onStartSceneAsync()
  {
    Explore033TopMenu explore033TopMenu = this;
    ExploreDataManager dataMgr = Singleton<ExploreDataManager>.GetInstance();
    yield return (object) explore033TopMenu.mExploreFooter.UpdateDeckUnitIconsAsync();
    explore033TopMenu.mTxtNumDungeon.SetTextLocalize(dataMgr.NowFloor);
    explore033TopMenu.mTxtTitleDungeon.SetTextLocalize(dataMgr.FloorData.name);
    int challengePoint = Singleton<NGGameDataManager>.GetInstance().challenge_point;
    explore033TopMenu.mTxtTicketNum.SetTextLocalize(challengePoint);
    if (challengePoint == 0)
      ((UIWidget) explore033TopMenu.mTxtTicketNum).color = Color.red;
    else
      ((UIWidget) explore033TopMenu.mTxtTicketNum).color = Color.white;
    if (MasterData.ExploreFloor[dataMgr.FrontFloorId].floor == 1)
      ((UIButtonColor) explore033TopMenu.mFloorSelectButton).isEnabled = false;
    explore033TopMenu.mMiniMap.UpdateFloorData();
    yield return (object) explore033TopMenu.mExploreFooter.Initialize();
    Singleton<ExploreSceneManager>.GetInstance().TopMenu = explore033TopMenu;
    Singleton<ExploreSceneManager>.GetInstance().Footer = explore033TopMenu.mExploreFooter;
  }

  public IEnumerator onBackSceneAsync()
  {
    yield return (object) this.mExploreFooter.UpdateDeckUnitIconsAsync();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  protected override void backScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    MypageScene.ChangeScene();
  }

  public void onHelpButton()
  {
    if (this.IsPushAndSet())
      return;
    Help0152Scene.ChangeScene(true, MasterData.HelpCategory[30]);
  }

  public void onRewardButton()
  {
    if (this.IsPushAndSet())
      return;
    Explore033RankingRewardScene.changeScene();
  }

  public void onDeckEditButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openDeckEditPopup());
  }

  public void onRankingButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openRankingPopup());
  }

  public void onRewardBoxButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openRewardBoxPopup());
  }

  public void onFloorSelectButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openFloorSelectPopup());
  }

  private IEnumerator setupTransitionParts()
  {
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/dir_transition");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = loader.Result;
    ExploreSceneManager instance = Singleton<ExploreSceneManager>.GetInstance();
    instance.ScreenEffect.SetTransitionObject(result.Clone(this.mTransitionAnchor.transform));
    instance.ScreenEffect.SetTransitionFullObject(result.Clone(this.mTransitionFullAnchor.transform));
  }

  private IEnumerator loadDeckEditPopup()
  {
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/popup_ExploreDeckSelect_anim_popup");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mDeckEditPopupPrefab = loader.Result;
  }

  private IEnumerator openDeckEditPopup()
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.mDeckEditPopupPrefab, isNonSe: true, isNonOpenAnime: true);
    IEnumerator e = popup.GetComponent<ExploreDeckEditPopup>().InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
  }

  private IEnumerator loadRankingPopup()
  {
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Ranking/popup_ExploreRanking_anim_popup");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mRankingPopupPrefab = loader.Result;
  }

  private IEnumerator openRankingPopup()
  {
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isNonSe: true);
    GameObject clone = this.mRankingPopupPrefab.Clone();
    clone.SetActive(false);
    ExploreRankingPopup exploreRankingAnim = clone.GetComponent<ExploreRankingPopup>();
    yield return (object) exploreRankingAnim.Initialize();
    Singleton<PopupManager>.GetInstance().dismiss();
    clone.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(clone, isCloned: true);
    exploreRankingAnim.setRankingInfo();
  }

  private IEnumerator loadRewardBoxPopup()
  {
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/popup_ExploreRewardBox_anim_popup");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mRewardBoxPopupPrefab = loader.Result;
  }

  private IEnumerator openRewardBoxPopup()
  {
    ExploreRewardBoxPopup exploreRewardBoxAnim = Singleton<PopupManager>.GetInstance().open(this.mRewardBoxPopupPrefab).GetComponent<ExploreRewardBoxPopup>();
    exploreRewardBoxAnim.dirMessageRewardsEmpty = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GoogleFeature/dir_noList"), ((Component) exploreRewardBoxAnim).transform);
    exploreRewardBoxAnim.dirMessageRewardsEmpty.transform.localPosition = new Vector3(0.0f, 87f, 0.0f);
    exploreRewardBoxAnim.dirMessageRewardsEmpty.SetActive(false);
    IEnumerator e = exploreRewardBoxAnim.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) exploreRewardBoxAnim.updateScrollPosition();
  }

  private IEnumerator loadFloorSelectPopup()
  {
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/popup_ExploreFloorSelect");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mFloorSelectPopupPrefab = loader.Result;
  }

  private IEnumerator openFloorSelectPopup()
  {
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(this.mFloorSelectPopupPrefab).GetComponent<ExploreFloorSelectPopup>().Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator ReopenPopup()
  {
    if (Singleton<ExploreDataManager>.GetInstance().IsPopupStateDeckEdit)
      yield return (object) this.openDeckEditPopup();
    else if (Singleton<ExploreDataManager>.GetInstance().IsPopupStateChallenge)
    {
      yield return (object) this.mExploreFooter.openChallengePopup();
    }
    else
    {
      Debug.Log((object) "invalid popupState");
      Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<ExploreSceneManager>.GetInstance().Pause(false);
  }

  [Serializable]
  public class FogSetting
  {
    public int folderPath;
    public float startDistance;
    public float endDistance;
  }
}
