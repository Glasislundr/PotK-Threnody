// Decompiled with JetBrains decompiler
// Type: Sea030_questMenu
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
[AddComponentMenu("Scenes/Sea030Quest/Menu")]
public class Sea030_questMenu : BackButtonMenuBase
{
  [SerializeField]
  public ScrollViewSpecifyBounds scrollView;
  [SerializeField]
  private Sea030_questMenu.MapChangeButtonInfo[] mapChangeButtons;
  [SerializeField]
  [Tooltip("マップが３つ以上に増えた時の展開用")]
  private UIGrid gridMapChangeButtons_;
  [SerializeField]
  private Animator animTransition;
  [SerializeField]
  private Sea030_questMenu.TransitionInfo[] transitionInfos;
  [SerializeField]
  private Sea030_questMenu.TransitionDetail[] transitionDetails;
  private GameObject unitIconPrefab;
  private GameObject transitionEffectPrefab_;
  private Dictionary<int, GameObject> dicMapPrefab_ = new Dictionary<int, GameObject>();
  [SerializeField]
  private GameObject dyn_leader_icon;
  private bool isInitialized;
  private QuestSeaS focusS_;
  private bool forceFirstInitialize_;
  private int currentXL_;
  private int nextXL_;
  private Dictionary<int, PlayerSeaQuestS[]> dicQuests_;
  private Dictionary<int, Sea030QuestMap> maps_ = new Dictionary<int, Sea030QuestMap>();
  private Sea030_questMenu.MapChangeButtonInfo[] cacheMapChangeButtons_;
  private const int ENABLED_GRID_MAPCHANGEBUTTONS = 3;
  private bool isEnabledGridMapChangeButtons_;

  private IEnumerator LoadLeaderUnit()
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    int?[] playerUnitIDs = SMManager.Get<PlayerSeaDeck[]>()[0].player_unit_ids;
    if (playerUnitIDs != null && playerUnitIDs.Length != 0 && playerUnitIDs[0].HasValue)
    {
      PlayerUnit unit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnitIDs[0].Value));
      if (!(unit == (PlayerUnit) null))
      {
        foreach (Component component in this.dyn_leader_icon.transform)
          Object.Destroy((Object) component.gameObject);
        UnitIcon unitScript = this.unitIconPrefab.Clone(this.dyn_leader_icon.transform).GetComponent<UnitIcon>();
        e = unitScript.setSimpleUnit(unit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unitScript.setLevelText(unit);
        unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        unitScript.Favorite = false;
        unitScript.Gray = false;
      }
    }
  }

  public int GetCurrentSelectMapID()
  {
    int index;
    if ((index = Array.FindIndex<Sea030_questMenu.MapChangeButtonInfo>(this.cacheMapChangeButtons_, (Predicate<Sea030_questMenu.MapChangeButtonInfo>) (x => x.XL == this.nextXL_)) + 1) >= this.cacheMapChangeButtons_.Length)
      index = 0;
    return this.cacheMapChangeButtons_[index].XL;
  }

  public IEnumerator Init(PlayerSeaQuestS[] StoryData, bool forceInitialize = false, int? focusSid = null)
  {
    Sea030_questMenu sea030QuestMenu = this;
    if (StoryData == null || StoryData.Length == 0)
    {
      sea030QuestMenu.IsPush = false;
    }
    else
    {
      if (sea030QuestMenu.cacheMapChangeButtons_ == null)
      {
        List<int> activeXLs = ((IEnumerable<PlayerSeaQuestS>) StoryData).Select<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_xl_QuestSeaXL)).Distinct<int>().ToList<int>();
        sea030QuestMenu.cacheMapChangeButtons_ = ((IEnumerable<Sea030_questMenu.MapChangeButtonInfo>) sea030QuestMenu.mapChangeButtons).Where<Sea030_questMenu.MapChangeButtonInfo>((Func<Sea030_questMenu.MapChangeButtonInfo, bool>) (x => activeXLs.Contains(x.XL))).ToArray<Sea030_questMenu.MapChangeButtonInfo>();
        sea030QuestMenu.isEnabledGridMapChangeButtons_ = sea030QuestMenu.cacheMapChangeButtons_.Length >= 3;
        if (sea030QuestMenu.isEnabledGridMapChangeButtons_)
        {
          ((IEnumerable<Sea030_questMenu.MapChangeButtonInfo>) sea030QuestMenu.mapChangeButtons).Select<Sea030_questMenu.MapChangeButtonInfo, GameObject>((Func<Sea030_questMenu.MapChangeButtonInfo, GameObject>) (x => x.button)).SetActives(false);
          Vector3 localScale = ((Component) sea030QuestMenu.gridMapChangeButtons_).transform.localScale;
          foreach (Sea030_questMenu.MapChangeButtonInfo info in (IEnumerable<Sea030_questMenu.MapChangeButtonInfo>) ((IEnumerable<Sea030_questMenu.MapChangeButtonInfo>) sea030QuestMenu.cacheMapChangeButtons_).OrderByDescending<Sea030_questMenu.MapChangeButtonInfo, int>((Func<Sea030_questMenu.MapChangeButtonInfo, int>) (x => x.XL)))
          {
            info.button.SetActive(true);
            info.button.transform.parent = ((Component) sea030QuestMenu.gridMapChangeButtons_).transform;
            info.button.transform.localScale = Vector3.one;
            ((UIWidget) info.button.GetComponent<UISprite>()).autoResizeBoxCollider = false;
            BoxCollider component = info.button.GetComponent<BoxCollider>();
            component.center = new Vector3(component.center.x * localScale.x, component.center.y * localScale.y, 0.0f);
            component.size = new Vector3(component.size.x * localScale.x, component.size.y * localScale.y, 1f);
            sea030QuestMenu.setEventChangeMap(info);
          }
          sea030QuestMenu.gridMapChangeButtons_.Reposition();
        }
      }
      sea030QuestMenu.focusS_ = (QuestSeaS) null;
      if (focusSid.HasValue)
        MasterData.QuestSeaS.TryGetValue(focusSid.Value, out sea030QuestMenu.focusS_);
      sea030QuestMenu.forceFirstInitialize_ = forceInitialize;
      sea030QuestMenu.dicQuests_ = ((IEnumerable<PlayerSeaQuestS>) StoryData).Select<PlayerSeaQuestS, Tuple<PlayerSeaQuestS, QuestSeaS>>((Func<PlayerSeaQuestS, Tuple<PlayerSeaQuestS, QuestSeaS>>) (x => Tuple.Create<PlayerSeaQuestS, QuestSeaS>(x, x.quest_sea_s))).Where<Tuple<PlayerSeaQuestS, QuestSeaS>>((Func<Tuple<PlayerSeaQuestS, QuestSeaS>, bool>) (y => y.Item2 != null)).ToLookup<Tuple<PlayerSeaQuestS, QuestSeaS>, int>((Func<Tuple<PlayerSeaQuestS, QuestSeaS>, int>) (z => z.Item2.quest_xl_QuestSeaXL)).ToDictionary<IGrouping<int, Tuple<PlayerSeaQuestS, QuestSeaS>>, int, PlayerSeaQuestS[]>((Func<IGrouping<int, Tuple<PlayerSeaQuestS, QuestSeaS>>, int>) (g => g.Key), (Func<IGrouping<int, Tuple<PlayerSeaQuestS, QuestSeaS>>, PlayerSeaQuestS[]>) (g => g.OrderBy<Tuple<PlayerSeaQuestS, QuestSeaS>, int>((Func<Tuple<PlayerSeaQuestS, QuestSeaS>, int>) (e => e.Item2.quest_l_QuestSeaL)).ThenBy<Tuple<PlayerSeaQuestS, QuestSeaS>, int>((Func<Tuple<PlayerSeaQuestS, QuestSeaS>, int>) (f => f.Item2.quest_m_QuestSeaM)).ThenBy<Tuple<PlayerSeaQuestS, QuestSeaS>, int>((Func<Tuple<PlayerSeaQuestS, QuestSeaS>, int>) (h => h.Item2.priority)).Select<Tuple<PlayerSeaQuestS, QuestSeaS>, PlayerSeaQuestS>((Func<Tuple<PlayerSeaQuestS, QuestSeaS>, PlayerSeaQuestS>) (d => d.Item1)).ToArray<PlayerSeaQuestS>()));
      sea030QuestMenu.StartCoroutine(sea030QuestMenu.LoadLeaderUnit());
      IEnumerator e1 = ServerTime.WaitSync();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      sea030QuestMenu.IsPush = true;
      if (sea030QuestMenu.isEnabledGridMapChangeButtons_)
      {
        foreach (Sea030_questMenu.MapChangeButtonInfo cacheMapChangeButton in sea030QuestMenu.cacheMapChangeButtons_)
          ((UIButtonColor) cacheMapChangeButton.button.GetComponent<UIButton>()).isEnabled = true;
      }
      else
      {
        foreach (Sea030_questMenu.MapChangeButtonInfo mapChangeButton in sea030QuestMenu.mapChangeButtons)
        {
          mapChangeButton.button.SetActive(false);
          sea030QuestMenu.setEventChangeMap(mapChangeButton);
        }
      }
      if (sea030QuestMenu.focusS_ != null)
        sea030QuestMenu.nextXL_ = sea030QuestMenu.focusS_.quest_xl_QuestSeaXL;
      else if (sea030QuestMenu.currentXL_ != 0 && !forceInitialize)
      {
        sea030QuestMenu.nextXL_ = sea030QuestMenu.currentXL_;
      }
      else
      {
        int key = ((IEnumerable<Sea030_questMenu.MapChangeButtonInfo>) sea030QuestMenu.cacheMapChangeButtons_).Max<Sea030_questMenu.MapChangeButtonInfo>((Func<Sea030_questMenu.MapChangeButtonInfo, int>) (x => x.XL));
        PlayerSeaQuestS[] source;
        if (sea030QuestMenu.dicQuests_.TryGetValue(key, out source) && ((IEnumerable<PlayerSeaQuestS>) source).Any<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.is_clear)) && MasterData.QuestSeaS.TryGetValue(Persist.seaLastSortie.Data.clearedS, out sea030QuestMenu.focusS_))
        {
          sea030QuestMenu.nextXL_ = sea030QuestMenu.focusS_.quest_xl_QuestSeaXL;
        }
        else
        {
          sea030QuestMenu.nextXL_ = key;
          PlayerSeaQuestS[] array;
          if (sea030QuestMenu.dicQuests_.TryGetValue(sea030QuestMenu.nextXL_, out array) && array.Length != 0)
          {
            int index = Array.FindLastIndex<PlayerSeaQuestS>(array, (Predicate<PlayerSeaQuestS>) (x => !x.is_clear));
            if (index < 0)
              index = 0;
            sea030QuestMenu.focusS_ = array[index].quest_sea_s;
          }
        }
      }
      sea030QuestMenu.StartCoroutine(sea030QuestMenu.doLoadMap());
      yield return (object) sea030QuestMenu.doChangeMap();
      sea030QuestMenu.isInitialized = true;
      sea030QuestMenu.IsPush = false;
      sea030QuestMenu.StartCoroutine(sea030QuestMenu.doWaitStartMapEffect());
    }
  }

  private void setEventChangeMap(Sea030_questMenu.MapChangeButtonInfo info)
  {
    UIButton component = info.button.GetComponent<UIButton>();
    if (!this.isEnabledGridMapChangeButtons_ && Array.FindIndex<Sea030_questMenu.MapChangeButtonInfo>(this.cacheMapChangeButtons_, (Predicate<Sea030_questMenu.MapChangeButtonInfo>) (x => x.XL == info.XL)) < 0)
      component.onClick.Clear();
    else
      EventDelegate.Set(component.onClick, (EventDelegate.Callback) (() =>
      {
        if (this.IsPushAndSet())
          return;
        this.changeMap(info.XL);
      }));
  }

  private IEnumerator doWaitStartMapEffect()
  {
    Sea030_questMenu sea030QuestMenu = this;
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    Sea030_questMenu.TransitionInfo transitionInfo = Array.Find<Sea030_questMenu.TransitionInfo>(sea030QuestMenu.transitionInfos, new Predicate<Sea030_questMenu.TransitionInfo>(sea030QuestMenu.\u003CdoWaitStartMapEffect\u003Eb__30_0));
    if (transitionInfo != null && transitionInfo.firstActives.Length != 0)
      ((IEnumerable<GameObject>) transitionInfo.firstActives).SetActives(true);
    sea030QuestMenu.invokeCurrentMap((Action<Sea030QuestMap>) (cm => cm.activateEffects(true)));
  }

  private IEnumerator doLoadMap()
  {
    if (this.nextXL_ != 0 && !this.dicMapPrefab_.ContainsKey(this.nextXL_))
    {
      Future<GameObject> ld = new ResourceObject(string.Format("Prefabs/sea030_quest/map_{0}", (object) this.nextXL_)).Load<GameObject>();
      yield return (object) ld.Wait();
      this.dicMapPrefab_[this.nextXL_] = ld.Result;
    }
  }

  private IEnumerator doChangeMap()
  {
    Sea030_questMenu sea030QuestMenu = this;
    if (!sea030QuestMenu.maps_.ContainsKey(sea030QuestMenu.nextXL_))
    {
      GameObject self;
      while (!sea030QuestMenu.dicMapPrefab_.TryGetValue(sea030QuestMenu.nextXL_, out self))
        yield return (object) null;
      Sea030QuestMap component = self.Clone(((Component) sea030QuestMenu.scrollView).transform).GetComponent<Sea030QuestMap>();
      sea030QuestMenu.maps_[sea030QuestMenu.nextXL_] = component;
      ((Component) component).gameObject.SetActive(false);
    }
    Sea030QuestMap m;
    if (sea030QuestMenu.maps_.TryGetValue(sea030QuestMenu.nextXL_, out m))
    {
      GameObject go = ((Component) m).gameObject;
      go.GetComponent<UIRect>().alpha = 0.0f;
      go.SetActive(true);
      ((Component) sea030QuestMenu.scrollView).transform.localScale = Vector3.one;
      PlayerSeaQuestS[] StoryData;
      if (!sea030QuestMenu.dicQuests_.TryGetValue(sea030QuestMenu.nextXL_, out StoryData))
        StoryData = new PlayerSeaQuestS[0];
      int questLQuestSeaL = sea030QuestMenu.focusS_ != null ? sea030QuestMenu.focusS_.quest_l_QuestSeaL : 0;
      int questMQuestSeaM = sea030QuestMenu.focusS_ != null ? sea030QuestMenu.focusS_.quest_m_QuestSeaM : 0;
      yield return (object) m.Init(StoryData, sea030QuestMenu.forceFirstInitialize_, questLQuestSeaL, questMQuestSeaM);
      go.GetComponent<UIRect>().alpha = 1f;
      sea030QuestMenu.scrollView.ClearBounds();
      sea030QuestMenu.scrollView.AddBounds((IEnumerable<UIWidget>) m.scrollRange);
      sea030QuestMenu.resetScrollViewPosition(Object.op_Inequality((Object) m.lastButton, (Object) null) ? m.lastButton.transform.localPosition : Vector3.zero);
      sea030QuestMenu.forceFirstInitialize_ = false;
      go = (GameObject) null;
    }
    Sea030QuestMap sea030QuestMap;
    if (sea030QuestMenu.currentXL_ != sea030QuestMenu.nextXL_ && sea030QuestMenu.maps_.TryGetValue(sea030QuestMenu.currentXL_, out sea030QuestMap))
    {
      GameObject gameObject = ((Component) sea030QuestMap).gameObject;
      gameObject.SetActive(false);
      Object.Destroy((Object) gameObject);
      sea030QuestMenu.maps_.Remove(sea030QuestMenu.currentXL_);
    }
    sea030QuestMenu.currentXL_ = sea030QuestMenu.nextXL_;
    sea030QuestMenu.focusS_ = (QuestSeaS) null;
    sea030QuestMenu.StartCoroutine(sea030QuestMenu.doWaitStartChangeButton());
  }

  private IEnumerator doWaitStartChangeButton()
  {
    Sea030_questMenu sea030QuestMenu = this;
    while (sea030QuestMenu.IsPush)
      yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    int index1 = Array.FindIndex<Sea030_questMenu.MapChangeButtonInfo>(sea030QuestMenu.cacheMapChangeButtons_, new Predicate<Sea030_questMenu.MapChangeButtonInfo>(sea030QuestMenu.\u003CdoWaitStartChangeButton\u003Eb__33_0));
    if (sea030QuestMenu.isEnabledGridMapChangeButtons_)
      ((UIButtonColor) sea030QuestMenu.cacheMapChangeButtons_[index1].button.GetComponent<UIButton>()).isEnabled = false;
    else if (sea030QuestMenu.cacheMapChangeButtons_.Length > 1)
    {
      int index2;
      if ((index2 = index1 + 1) >= sea030QuestMenu.cacheMapChangeButtons_.Length)
        index2 = 0;
      sea030QuestMenu.cacheMapChangeButtons_[index2].button.SetActive(true);
    }
  }

  private void resetScrollViewPosition(Vector3 pos)
  {
    GameObject gameObject = ((Component) this.scrollView).gameObject;
    UIPanel component = gameObject.GetComponent<UIPanel>();
    Transform transform = gameObject.transform;
    Vector3 localPosition = transform.localPosition;
    Vector4 baseClipRegion = component.baseClipRegion;
    Vector2 clipOffset = component.clipOffset;
    localPosition.x = -pos.x;
    localPosition.y = -pos.y;
    transform.localPosition = localPosition;
    clipOffset.x = pos.x - baseClipRegion.x;
    clipOffset.y = pos.y - baseClipRegion.y;
    component.clipOffset = clipOffset;
    this.scrollView.RestrictWithinBounds(true, true, true);
  }

  public void onTeamEditButton()
  {
    if (this.IsPushAndSet())
      return;
    Unit0046Scene.changeScene(true);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.onEndScene();
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Sea030HomeScene.ChangeScene(false);
  }

  private void changeMap(int XL)
  {
    this.IsPush = true;
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false, isNonSe: true);
    if (this.isEnabledGridMapChangeButtons_)
    {
      Sea030_questMenu.MapChangeButtonInfo changeButtonInfo = Array.Find<Sea030_questMenu.MapChangeButtonInfo>(this.cacheMapChangeButtons_, (Predicate<Sea030_questMenu.MapChangeButtonInfo>) (x => x.XL == this.currentXL_));
      if (changeButtonInfo != null)
        ((UIButtonColor) changeButtonInfo.button.GetComponent<UIButton>()).isEnabled = true;
      ((UIButtonColor) Array.Find<Sea030_questMenu.MapChangeButtonInfo>(this.cacheMapChangeButtons_, (Predicate<Sea030_questMenu.MapChangeButtonInfo>) (x => x.XL == XL)).button.GetComponent<UIButton>()).isEnabled = false;
    }
    else
    {
      foreach (Sea030_questMenu.MapChangeButtonInfo cacheMapChangeButton in this.cacheMapChangeButtons_)
        cacheMapChangeButton.button.SetActive(false);
    }
    this.nextXL_ = XL;
    this.StartCoroutine(this.doStartTransition());
  }

  private IEnumerator doStartTransition()
  {
    this.StartCoroutine(this.doLoadMap());
    Sea030_questMenu.TransitionInfo info = Array.Find<Sea030_questMenu.TransitionInfo>(this.transitionInfos, (Predicate<Sea030_questMenu.TransitionInfo>) (x => x.XL == this.nextXL_));
    return Object.op_Inequality((Object) this.animTransition, (Object) null) && info != null ? this.doTransition(info, Array.Find<Sea030_questMenu.TransitionDetail>(this.transitionDetails, (Predicate<Sea030_questMenu.TransitionDetail>) (x => x.from == this.currentXL_ && x.to == this.nextXL_))) : this.doNoneTransition();
  }

  private IEnumerator doTransition(
    Sea030_questMenu.TransitionInfo info,
    Sea030_questMenu.TransitionDetail detail)
  {
    Sea030_questMenu sea030QuestMenu = this;
    Singleton<NGSoundManager>.GetInstance().PlaySe(detail != null ? detail.seName : info.seName);
    sea030QuestMenu.playAnime(sea030QuestMenu.animTransition, detail != null ? detail.animeOut : info.animeOut.status);
    yield return (object) new WaitForAnimation(sea030QuestMenu.animTransition, waitframeHashset: 2);
    ((Behaviour) sea030QuestMenu.animTransition).enabled = false;
    yield return (object) sea030QuestMenu.doChangeMap();
    sea030QuestMenu.playAnime(sea030QuestMenu.animTransition, detail != null ? detail.animeIn : info.animeIn.status);
    AnimatorStateInfo animatorStateInfo;
    do
    {
      yield return (object) null;
      sea030QuestMenu.scrollView.RestrictWithinBounds(true, true, true);
      animatorStateInfo = sea030QuestMenu.animTransition.GetCurrentAnimatorStateInfo(0);
    }
    while ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0);
    ((Behaviour) sea030QuestMenu.animTransition).enabled = false;
    ((Component) sea030QuestMenu.scrollView).transform.localScale = Vector3.one;
    sea030QuestMenu.scrollView.RestrictWithinBounds(true, true, true);
    Singleton<PopupManager>.GetInstance().dismiss();
    sea030QuestMenu.IsPush = false;
  }

  private IEnumerator doNoneTransition()
  {
    Sea030_questMenu sea030QuestMenu = this;
    IEnumerator e = sea030QuestMenu.doChangeMap();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().dismiss();
    sea030QuestMenu.IsPush = false;
  }

  private void playAnime(Animator anime, string statusName)
  {
    if (!string.IsNullOrEmpty(statusName))
      anime.Play(statusName);
    ((Behaviour) this.animTransition).enabled = true;
  }

  private void invokeCurrentMap(Action<Sea030QuestMap> act)
  {
    Sea030QuestMap sea030QuestMap;
    if (!this.maps_.TryGetValue(this.currentXL_, out sea030QuestMap))
      return;
    act(sea030QuestMap);
  }

  public void onEndScene()
  {
    if (this.currentXL_ == 0)
      return;
    Sea030_questMenu.TransitionInfo transitionInfo = Array.Find<Sea030_questMenu.TransitionInfo>(this.transitionInfos, (Predicate<Sea030_questMenu.TransitionInfo>) (x => x.XL == this.currentXL_));
    if (transitionInfo != null && transitionInfo.firstActives.Length != 0)
      ((IEnumerable<GameObject>) transitionInfo.firstActives).SetActives(false);
    this.invokeCurrentMap((Action<Sea030QuestMap>) (cm => cm.activateEffects(false)));
  }

  public enum storyGroup
  {
    Pool,
    Beach,
    Jungle,
    Unify,
    Beach2,
    Beach3,
    Tower,
    Seabed01,
    Seabed02,
    Seabed03,
    Seabed04,
    Seabed05,
    Seabed06,
    Pledge01,
    Pledge02,
    Pledge03,
    Pledge04,
    Pledge05,
    Pledge06,
  }

  [Serializable]
  public class StoryGroupInfo
  {
    public Sea030_questMenu.storyGroup group;
    public int L;
    public GameObject storyButton;
    public GameObject[] storyM;
    public GameObject[] line;
    public GameObject[] tween;
    [NonSerialized]
    public PlayerSeaQuestS[] storyData;
  }

  [Serializable]
  public class MapChangeButtonInfo
  {
    public int XL;
    public GameObject button;
  }

  [Serializable]
  public class AnimeInfo
  {
    public string status;
  }

  [Serializable]
  public class TransitionInfo
  {
    public int XL;
    public string seName;
    public Sea030_questMenu.AnimeInfo animeOut;
    public Sea030_questMenu.AnimeInfo animeIn;
    public GameObject[] firstActives;
  }

  [Serializable]
  public class TransitionDetail
  {
    [Tooltip("開始XL")]
    public int from;
    [Tooltip("目標XL")]
    public int to;
    public string seName;
    public string animeOut;
    public string animeIn;
  }
}
