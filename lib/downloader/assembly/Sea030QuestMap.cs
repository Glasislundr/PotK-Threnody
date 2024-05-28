// Decompiled with JetBrains decompiler
// Type: Sea030QuestMap
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
[AddComponentMenu("Scenes/Sea030Quest/MapController")]
public class Sea030QuestMap : NGMenuBase
{
  private const int STORY_BTN_TWEEN_START = 41;
  private const int STORY_BTN_TWEEN_END = 42;
  [SerializeField]
  private int XL_;
  [SerializeField]
  private string buttonData_;
  [SerializeField]
  private UIWidget[] scrollRange_;
  [SerializeField]
  private Sea030_questMenu.StoryGroupInfo[] storyList;
  [SerializeField]
  private string towerButtonData_;
  [SerializeField]
  private Sea030_questMenu.StoryGroupInfo[] towerSpot;
  [SerializeField]
  private GameObject[] effects;
  [SerializeField]
  private GameObject tweenObject;
  [SerializeField]
  private float tweenDuration;
  [SerializeField]
  private AnimationCurve tweenCurve;
  [SerializeField]
  private float tweenDelay;
  private List<Sea030_questMenu.StoryGroupInfo> activeStories_ = new List<Sea030_questMenu.StoryGroupInfo>();
  private int originID;
  private int tweenFinishCount;
  private float minStartDelay;
  private GameObject StoryButtons;
  private bool isInitialized;
  private Vector3? tweenObjectScale;
  private UIScrollView scrollView_;
  private GameObject[] disabledEffects_;

  public int XL => this.XL_;

  public UIWidget[] scrollRange => this.scrollRange_;

  public TweenPosition currentTweenPos { get; private set; }

  public TweenScale currentTweenScale { get; private set; }

  public GameObject lastButton { get; private set; }

  private UIScrollView scrollView
  {
    get
    {
      return !Object.op_Inequality((Object) this.scrollView_, (Object) null) ? (this.scrollView_ = NGUITools.FindInParents<UIScrollView>(((Component) this).gameObject)) : this.scrollView_;
    }
  }

  private void InitStoryChoiceButton(
    GameObject btnContainer,
    int xl,
    int l,
    int index,
    bool isStart,
    PlayerSeaQuestS[] StoryDataM)
  {
    Sea030StoryButton componentInChildren = ((Component) btnContainer.transform).GetComponentInChildren<Sea030StoryButton>();
    componentInChildren.Lock();
    QuestSeaM[] array = ((IEnumerable<QuestSeaM>) MasterData.QuestSeaMList).Where<QuestSeaM>((Func<QuestSeaM, bool>) (x => x.quest_xl_QuestSeaXL == xl && x.quest_l_QuestSeaL == l)).OrderBy<QuestSeaM, int>((Func<QuestSeaM, int>) (x => x.priority)).ToArray<QuestSeaM>();
    if (array != null && array.Length > index)
      componentInChildren.StoryName = array[index].name;
    if (StoryDataM.Length > index)
    {
      int bonusCategory = StoryDataM[index].bonus_category;
      componentInChildren.SetBonus(bonusCategory);
    }
    componentInChildren.initTween();
  }

  private IEnumerator StoryChoiceButtonSetting(PlayerSeaQuestS[] StoryData, int Lid, int Mid)
  {
    List<Tuple<GameObject, bool>> source1 = new List<Tuple<GameObject, bool>>(this.storyList.Length);
    GameObject lastSelect = (GameObject) null;
    for (int i = 0; i < this.storyList.Length; i++)
    {
      Sea030_questMenu.StoryGroupInfo storyInst = this.storyList[i];
      PlayerSeaQuestS[] storyData = storyInst.storyData;
      if (storyData != null && storyData.Length != 0)
      {
        List<Tuple<GameObject, bool>> clearedList = new List<Tuple<GameObject, bool>>(storyInst.storyM.Length);
        int cnt = 0;
        ((IEnumerable<GameObject>) storyInst.storyM).ForEach<GameObject, PlayerSeaQuestS>((IEnumerable<PlayerSeaQuestS>) storyData, (Action<GameObject, PlayerSeaQuestS>) ((storyM, storyDataM) =>
        {
          PlayerSeaQuestS[] source2 = StoryData.S(this.XL_, storyInst.L, storyDataM.quest_sea_s.quest_m_QuestSeaM);
          bool clearflag = ((IEnumerable<PlayerSeaQuestS>) source2).All<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.is_clear));
          Sea030StoryButton storyButton = ((Component) storyM.transform).GetComponentInChildren<Sea030StoryButton>();
          clearedList.Add(Tuple.Create<GameObject, bool>(storyM, clearflag));
          if (storyInst.L == Lid && storyDataM.quest_sea_s.quest_m_QuestSeaM == Mid)
            lastSelect = storyM;
          storyButton.SetSprite(storyInst.group);
          storyButton.UnLock(clearflag, source2[0].is_new);
          if (storyInst.line.Length != 0 && Object.op_Inequality((Object) storyInst.line[cnt], (Object) null))
            storyInst.line[cnt].SetActive(true);
          storyButton.onClick = new EventDelegate((EventDelegate.Callback) (() =>
          {
            Singleton<CommonRoot>.GetInstance().isTouchBlockAutoClose = true;
            Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
            storyButton.changeClickMySelf();
            this.StoryChoiceButtonTween(41);
            this.TweenMap(storyButton.Lnumber, storyButton.Mnumber, storyButton.TweenIndex);
          }));
          QuestSeaS questSeaS = source2[0].quest_sea_s;
          storyButton.PathNumber = questSeaS.quest_m.number_m;
          storyButton.Mnumber = questSeaS.quest_m_QuestSeaM;
          storyButton.Lnumber = questSeaS.quest_l_QuestSeaL;
          storyButton.Lindex = i;
          storyButton.TweenIndex = cnt;
          GameObject slcPointerArea = storyButton.slc_pointer_area;
          if (Object.op_Inequality((Object) slcPointerArea, (Object) null))
            slcPointerArea.SetActive(!clearflag);
          ++cnt;
        }));
        int lastIndex = clearedList.FindLastIndex((Predicate<Tuple<GameObject, bool>>) (x => x.Item2));
        source1.Add(clearedList[lastIndex == clearedList.Count - 1 ? lastIndex : lastIndex + 1]);
        int[] array = ((IEnumerable<PlayerSeaQuestS>) storyData).Select<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_m_QuestSeaM)).Distinct<int>().OrderBy<int, int>((Func<int, int>) (y => y)).ToArray<int>();
        for (int index = 0; index < storyInst.storyM.Length && index < array.Length; ++index)
        {
          PlayerSeaQuestS[] playerSeaQuestSArray = StoryData.S(this.XL_, storyInst.L, array[index]);
          if (playerSeaQuestSArray.Length != 0)
            this.MissionAchievementRate(playerSeaQuestSArray[0], ((Component) storyInst.storyM[index].transform).GetComponentInChildren<Sea030StoryButton>());
        }
      }
    }
    if (Object.op_Inequality((Object) lastSelect, (Object) null))
      this.lastButton = lastSelect;
    else if (source1.Any<Tuple<GameObject, bool>>())
    {
      int lastIndex = source1.FindLastIndex((Predicate<Tuple<GameObject, bool>>) (y => y.Item2));
      this.lastButton = source1[lastIndex == source1.Count - 1 ? lastIndex : lastIndex + 1].Item1;
    }
    else
    {
      this.lastButton = (GameObject) null;
      yield break;
    }
  }

  private void MissionAchievementRate(PlayerSeaQuestS quest, Sea030StoryButton button)
  {
    int num = 0;
    int nowCount = 0;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 9)).ToArray<PlayerMissionHistory>();
    QuestSeaMission[] array2 = ((IEnumerable<QuestSeaMission>) MasterData.QuestSeaMissionList).Where<QuestSeaMission>((Func<QuestSeaMission, bool>) (x => x.quest_s.quest_m_QuestSeaM == quest.quest_sea_s.quest_m_QuestSeaM)).ToArray<QuestSeaMission>();
    foreach (QuestSeaMission questSeaMission in array2)
      nowCount += ((IEnumerable<PlayerMissionHistory>) array1).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(questSeaMission.ID) ? 1 : 0;
    int allCount = num + array2.Length;
    button.MissionAchevement(nowCount, allCount);
  }

  private void TweenMap(int L, int M, int tweenIndex)
  {
    GameObject gameObject = (GameObject) null;
    if (L != 0)
    {
      for (int index = 0; index < this.storyList.Length; ++index)
      {
        if (this.storyList[index].L == L)
        {
          gameObject = this.storyList[index].tween[tweenIndex];
          break;
        }
      }
    }
    else if (this.towerSpot.Length != 0)
      gameObject = this.towerSpot[0].tween[tweenIndex];
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    TweenPosition orAddComponent1 = this.tweenObject.GetOrAddComponent<TweenPosition>();
    TweenScale orAddComponent2 = this.tweenObject.GetOrAddComponent<TweenScale>();
    orAddComponent1.from = this.tweenObject.transform.localPosition;
    orAddComponent1.to = gameObject.transform.localPosition;
    TweenPosition tweenPosition = orAddComponent1;
    tweenPosition.to = Vector3.op_Subtraction(tweenPosition.to, ((Component) this.scrollView).transform.localPosition);
    orAddComponent2.from = this.tweenObject.transform.localScale;
    orAddComponent2.to = gameObject.transform.localScale;
    ((UITweener) orAddComponent1).duration = this.tweenDuration;
    if ((double) this.tweenDuration == 0.0)
      ((UITweener) orAddComponent1).duration = 0.0f;
    ((UITweener) orAddComponent1).delay = this.tweenDelay;
    ((UITweener) orAddComponent1).animationCurve = this.tweenCurve;
    ((UITweener) orAddComponent2).duration = this.tweenDuration;
    if ((double) this.tweenDuration == 0.0)
      ((UITweener) orAddComponent2).duration = 0.0f;
    ((UITweener) orAddComponent2).delay = this.tweenDelay;
    ((UITweener) orAddComponent2).animationCurve = this.tweenCurve;
    EventDelegate.Set(((UITweener) orAddComponent1).onFinished, (EventDelegate.Callback) (() => this.ChangeScene(L, M)));
    UITweener[] uiTweenerArray = new UITweener[2]
    {
      (UITweener) orAddComponent1,
      (UITweener) orAddComponent2
    };
    foreach (UITweener uiTweener in uiTweenerArray)
    {
      uiTweener.tweenFactor = 0.0f;
      uiTweener.PlayForward();
    }
    this.currentTweenPos = orAddComponent1;
    this.currentTweenScale = orAddComponent2;
  }

  private bool checkTowerOpen()
  {
    if (this.towerSpot.Length == 0 || SMManager.Get<SM.TowerPeriod[]>() == null || SMManager.Get<SM.TowerPeriod[]>().Length == 0)
      return false;
    DateTime dateTime = ServerTime.NowAppTime();
    return SMManager.Get<SM.TowerPeriod[]>()[0].final_at > dateTime;
  }

  public IEnumerator Init(PlayerSeaQuestS[] StoryData, bool forceInitialize = false, int Lid = 0, int Mid = 0)
  {
    Sea030QuestMap sea030QuestMap1 = this;
    if (StoryData == null)
    {
      sea030QuestMap1.IsPush = false;
    }
    else
    {
      if (sea030QuestMap1.tweenObjectScale.HasValue)
        sea030QuestMap1.tweenObject.transform.localScale = sea030QuestMap1.tweenObjectScale.Value;
      else if (Object.op_Inequality((Object) sea030QuestMap1.tweenObject, (Object) null))
        sea030QuestMap1.tweenObjectScale = new Vector3?(sea030QuestMap1.tweenObject.transform.localScale);
      bool isTowerOpen = sea030QuestMap1.checkTowerOpen();
      if (forceInitialize)
        sea030QuestMap1.isInitialized = false;
      IEnumerator e;
      if (!sea030QuestMap1.isInitialized)
      {
        if (Object.op_Implicit((Object) sea030QuestMap1.currentTweenPos))
        {
          sea030QuestMap1.currentTweenScale.value = sea030QuestMap1.currentTweenScale.from;
          sea030QuestMap1.currentTweenPos.value = sea030QuestMap1.currentTweenPos.from;
          sea030QuestMap1.currentTweenPos = (TweenPosition) null;
          sea030QuestMap1.currentTweenScale = (TweenScale) null;
        }
        sea030QuestMap1.activeStories_ = new List<Sea030_questMenu.StoryGroupInfo>();
        Future<GameObject> prefab;
        if (StoryData != null)
        {
          prefab = new ResourceObject("Prefabs/sea030_quest/" + sea030QuestMap1.buttonData_).Load<GameObject>();
          yield return (object) prefab.Wait();
          for (int index1 = 0; index1 < sea030QuestMap1.storyList.Length; ++index1)
          {
            Sea030_questMenu.StoryGroupInfo storyInst = sea030QuestMap1.storyList[index1];
            int[] array = ((IEnumerable<PlayerSeaQuestS>) StoryData).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s.quest_l_QuestSeaL == storyInst.L)).Select<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_m_QuestSeaM)).Distinct<int>().ToArray<int>();
            if (array.Length != 0)
            {
              int index2 = 0;
              foreach (GameObject gameObject in storyInst.storyM)
              {
                Sea030StoryButton componentInChildren = gameObject.GetComponentInChildren<Sea030StoryButton>();
                if (Object.op_Inequality((Object) componentInChildren, (Object) null))
                  sea030QuestMap1.destroyObject(((Component) componentInChildren).gameObject);
                if (index2 < storyInst.storyM.Length && index2 < array.Length)
                {
                  if (StoryData.S(sea030QuestMap1.XL_, storyInst.L, array[index2]).Length != 0)
                    prefab.Result.Clone(gameObject.transform);
                  ++index2;
                }
                else
                  break;
              }
              if (index2 > 0)
                sea030QuestMap1.activeStories_.Add(storyInst);
            }
          }
          prefab = (Future<GameObject>) null;
        }
        if (isTowerOpen)
        {
          prefab = new ResourceObject("Prefabs/sea030_quest/" + sea030QuestMap1.towerButtonData_).Load<GameObject>();
          e = prefab.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Transform transform = sea030QuestMap1.towerSpot[0].storyM[0].transform;
          Sea030StoryTowerButton componentInChildren = ((Component) transform).GetComponentInChildren<Sea030StoryTowerButton>();
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            sea030QuestMap1.destroyObject(((Component) componentInChildren).gameObject);
          prefab.Result.Clone(transform);
          if (!sea030QuestMap1.activeStories_.Contains(sea030QuestMap1.towerSpot[0]))
            sea030QuestMap1.activeStories_.Add(sea030QuestMap1.towerSpot[0]);
          prefab = (Future<GameObject>) null;
        }
      }
      else if (Object.op_Implicit((Object) sea030QuestMap1.currentTweenPos))
      {
        // ISSUE: reference to a compiler-generated method
        EventDelegate.Set(((UITweener) sea030QuestMap1.currentTweenPos).onFinished, new EventDelegate.Callback(sea030QuestMap1.\u003CInit\u003Eb__44_0));
        ((UITweener) sea030QuestMap1.currentTweenScale).PlayReverse();
        ((UITweener) sea030QuestMap1.currentTweenPos).PlayReverse();
      }
      else
        sea030QuestMap1.StoryChoiceButtonTween(42, 0.5f);
      if (StoryData != null)
      {
        for (int index3 = 0; index3 < sea030QuestMap1.storyList.Length; ++index3)
        {
          Sea030_questMenu.StoryGroupInfo storyInst = sea030QuestMap1.storyList[index3];
          int[] array = ((IEnumerable<PlayerSeaQuestS>) StoryData).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s.quest_l_QuestSeaL == storyInst.L)).Select<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_m_QuestSeaM)).Distinct<int>().ToArray<int>();
          if (array.Length != 0)
          {
            int index4 = 0;
            foreach (var data in ((IEnumerable<GameObject>) storyInst.storyM).Select((s, j) => new
            {
              s = s,
              j = j
            }))
            {
              if (index4 < storyInst.storyM.Length)
              {
                if (index4 < array.Length)
                {
                  if (StoryData.S(sea030QuestMap1.XL_, storyInst.L, array[index4]).Length != 0)
                  {
                    storyInst.storyData = StoryData.M(sea030QuestMap1.XL_, storyInst.L);
                    sea030QuestMap1.InitStoryChoiceButton(data.s, sea030QuestMap1.XL_, storyInst.L, data.j, !sea030QuestMap1.isInitialized, storyInst.storyData);
                  }
                  ++index4;
                }
                else
                  break;
              }
              else
                break;
            }
          }
        }
      }
      if (isTowerOpen)
      {
        Sea030QuestMap sea030QuestMap = sea030QuestMap1;
        Sea030StoryTowerButton storyButton = sea030QuestMap1.towerSpot[0].storyM[0].GetComponentInChildren<Sea030StoryTowerButton>();
        if (Object.op_Inequality((Object) storyButton, (Object) null))
        {
          storyButton.Lnumber = 0;
          storyButton.Mnumber = 1;
          storyButton.TweenIndex = 0;
          storyButton.initTween();
          UIButton componentInChildren = ((Component) storyButton).GetComponentInChildren<UIButton>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          {
            if (SMManager.Get<TowerQuestEntryCondition[]>().Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              EventDelegate.Set(componentInChildren.onClick, new EventDelegate.Callback(sea030QuestMap1.\u003CInit\u003Eb__44_6));
            }
            else
              EventDelegate.Set(componentInChildren.onClick, (EventDelegate.Callback) (() =>
              {
                Singleton<CommonRoot>.GetInstance().isTouchBlockAutoClose = true;
                Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
                storyButton.changeAnimDirection();
                sea030QuestMap.StoryChoiceButtonTween(41);
                sea030QuestMap.TweenMap(storyButton.Lnumber, storyButton.Mnumber, storyButton.TweenIndex);
              }));
          }
        }
      }
      if (StoryData != null)
      {
        e = sea030QuestMap1.StoryChoiceButtonSetting(StoryData, Lid, Mid);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (!sea030QuestMap1.isInitialized)
        sea030QuestMap1.StoryChoiceButtonTween(42, 0.5f);
      sea030QuestMap1.isInitialized = true;
      sea030QuestMap1.IsPush = false;
      sea030QuestMap1.StartCoroutine(sea030QuestMap1.doWaitStartEffects());
    }
  }

  private IEnumerator OpenTowerNoEntryPopup()
  {
    Sea030QuestMap sea030QuestMap = this;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/Popup_Common/popup_030_sea_common__anim_fade").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    GameObject go = Singleton<PopupManager>.GetInstance().open(result, isViewBack: false, clip: "SE_1044");
    go.SetActive(false);
    string text = MasterData.TowerEntryConditions[((IEnumerable<TowerQuestEntryCondition>) SMManager.Get<TowerQuestEntryCondition[]>()).ToArray<TowerQuestEntryCondition>()[0].id].text;
    yield return (object) go.GetComponent<Sea030CommonPopup>().initialize("解放条件", text);
    go.SetActive(true);
    sea030QuestMap.IsPush = false;
  }

  private void destroyObject(GameObject go)
  {
    go.transform.parent = (Transform) null;
    go.SetActive(false);
    Object.Destroy((Object) go);
  }

  private IEnumerator doWaitStartEffects()
  {
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    if (this.effects != null && this.effects.Length != 0)
      ((IEnumerable<GameObject>) this.effects).SetActives(true);
  }

  private void ChangeScene(int passLdata, int passMdata)
  {
    if (passLdata == 0)
      Tower029TopScene.ChangeScene(true);
    else
      Quest0022Scene.ChangeSceneSea(true, this.XL_, passLdata, passMdata);
    this.activateEffects(false);
  }

  private void StoryChoiceButtonTween(int tweengroup, float delay = 0.0f)
  {
    foreach (Sea030_questMenu.StoryGroupInfo activeStorey in this.activeStories_)
    {
      GameObject storyButton = activeStorey.storyButton;
      if (!Object.op_Equality((Object) storyButton, (Object) null))
        ((IEnumerable<UITweener>) storyButton.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
        {
          if (x.tweenGroup != tweengroup)
            return;
          x.delay = delay;
          x.ResetToBeginning();
          x.PlayForward();
        }));
    }
  }

  public void activateEffects(bool bEnable)
  {
    if (this.effects == null || this.effects.Length == 0)
      return;
    if (bEnable)
    {
      if (this.disabledEffects_ != null)
        ((IEnumerable<GameObject>) this.disabledEffects_).SetActives(true);
      this.disabledEffects_ = (GameObject[]) null;
    }
    else
    {
      if (this.disabledEffects_ != null)
        return;
      List<GameObject> source = new List<GameObject>();
      foreach (GameObject effect in this.effects)
      {
        if (!Object.op_Equality((Object) effect, (Object) null) && effect.activeSelf)
          source.AddRange(effect.transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).Where<GameObject>((Func<GameObject, bool>) (y => y.gameObject.activeSelf && Object.op_Inequality((Object) y.gameObject.GetComponent<ParticleSystem>(), (Object) null))));
      }
      if (!source.Any<GameObject>())
        return;
      this.disabledEffects_ = source.ToArray();
      ((IEnumerable<GameObject>) this.disabledEffects_).SetActives(false);
    }
  }
}
