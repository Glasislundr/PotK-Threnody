// Decompiled with JetBrains decompiler
// Type: Mypage051Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Mypage051Menu : MypageMenuBase
{
  private readonly List<int> baseVoiceIDList = new List<int>()
  {
    52,
    53,
    54
  };
  [SerializeField]
  protected GameObject dir_Roulette;
  [SerializeField]
  private GameObject slc_Circle_Base;
  [SerializeField]
  private CircularMotionPositionSet CirculButton;
  [SerializeField]
  private MypageSlidePanelDragged Story_Container;
  [SerializeField]
  private MypageSlidePanelDragged Event_Container;

  public override IEnumerator InitSceneAsync()
  {
    Mypage051Menu mypage051Menu = this;
    mypage051Menu.sm = Singleton<NGSoundManager>.GetInstance();
    IEnumerator e = mypage051Menu.CreateCharcterAnimation();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypage051Menu.CreateJogDial();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator onStartSceneAsync(bool isCloudAnim, bool isReservedEventScript)
  {
    Mypage051Menu mypage051Menu = this;
    mypage051Menu.CharaAnimation = false;
    mypage051Menu.InitTween();
    mypage051Menu.JogDialSetting();
    IEnumerator e = mypage051Menu.CharcterAnimationSetting(isCloudAnim, MypageMenuBase.EARTH_IN_BG_TWEENGROUP);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene()
  {
    if (Object.op_Inequality((Object) this.sm, (Object) null) && ((Behaviour) this.sm).enabled)
      this.sm.stopVoice();
    this.CirculButton.End();
    if (!Object.op_Inequality((Object) this.CharaAnimObj, (Object) null))
      return;
    this.CharaAnimObj.SetActive(false);
  }

  private IEnumerator CreateJogDial()
  {
    Future<GameObject> f = Res.Prefabs.mypage.CircleAnimation_Story.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) this.Story_Container, (Object) null) && !Singleton<EarthDataManager>.GetInstance().questProgress.isCleared)
      this.Story_Container.SetEffect(f.Result, "051_story");
    if (Object.op_Inequality((Object) this.Event_Container, (Object) null))
    {
      if (Singleton<EarthDataManager>.GetInstance().GetEnableEarthExtraQuestList().Count == 0)
      {
        this.Event_Container.ChangeState(false);
      }
      else
      {
        this.Event_Container.SetEffect(f.Result, "event");
        this.Event_Container.ChangeState(true);
      }
    }
  }

  protected override IEnumerator CharcterAnimationSetting(bool isCloudAnim, int tweenID)
  {
    Mypage051Menu mypage051Menu = this;
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !x.unit.IsMaterialUnit)).ToArray<PlayerUnit>();
    if (array.Length == 0)
    {
      MypageMenuBase.LeaderID = 0;
      if (isCloudAnim)
      {
        Singleton<CommonRoot>.GetInstance().StartBGTween(tweenID);
        mypage051Menu.AddStartTweenDelay(0.0f, MypageMenuBase.START_TWEENGROUP_TOP);
        mypage051Menu.AddStartTweenDelay(0.0f, MypageMenuBase.START_TWEEN_GROUP_JOGDIAL);
      }
      else
        mypage051Menu.AddStartTweenDelay(0.0f, MypageMenuBase.START_TWEENGROUP);
    }
    else
    {
      int index = Random.Range(0, array.Length);
      PlayerUnit unit = array[index];
      int id = unit.unit.ID;
      Future<Sprite> LeaderF = unit.unit.LoadSpriteLarge();
      IEnumerator e = LeaderF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      MypageMenuBase.PlayTouchVoiceList = mypage051Menu.baseVoiceIDList.Shuffle<int>().ToList<int>();
      mypage051Menu.NextPlayVoiceIndex = 0;
      ((IEnumerable<UI2DSprite>) mypage051Menu.CharaSprites).ForEach<UI2DSprite>((Action<UI2DSprite>) (x =>
      {
        UIButton uiButton = (UIButton) null;
        if (Object.op_Inequality((Object) x, (Object) null))
        {
          x.sprite2D = LeaderF.Result;
          uiButton = ((Component) x).gameObject.GetComponent<UIButton>();
        }
        if (!Object.op_Inequality((Object) uiButton, (Object) null))
          return;
        EventDelegate.Set(uiButton.onClick, (EventDelegate.Callback) (() => this.CharaTouchVoice()));
      }));
      ((IEnumerable<UILabel>) mypage051Menu.CharaName).ForEach<UILabel>((Action<UILabel>) (x => x.SetTextLocalize(unit.unit.name)));
      float addDelayTime = mypage051Menu.SetCharacterAnimControllerWithGetAddDelayTime(false, id);
      if (isCloudAnim)
      {
        Singleton<CommonRoot>.GetInstance().StartBGTween(tweenID);
        mypage051Menu.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEENGROUP_TOP);
        mypage051Menu.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEEN_GROUP_JOGDIAL);
      }
      else
        mypage051Menu.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEENGROUP);
    }
  }

  protected override void AddStartTweenDelay(float addTime, int groupID)
  {
    List<MypageSlidePanelDragged> buttons = this.CirculButton.GetTargets;
    ((IEnumerable<UITweener>) ((Component) this).GetComponentsInChildren<UITweener>(true)).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != groupID)
        return;
      x.delay += addTime;
      EventDelegate.Add(x.onFinished, (EventDelegate.Callback) (() => x.delay -= addTime));
      foreach (Component component in buttons)
      {
        if (Object.op_Equality((Object) component.transform, (Object) ((Component) x).gameObject.transform))
        {
          EventDelegate.Add(x.onFinished, (EventDelegate.Callback) (() => this.ButtonTweenFinished()));
          break;
        }
      }
    }));
    this.ButtonCount = buttons.Count;
  }

  private void JogDialSetting()
  {
    this.CirculButton.Init(this.Story_Container);
    this.Story_Container.ChangeState(!Singleton<EarthDataManager>.GetInstance().questProgress.isCleared);
  }

  public void SetJogDialActive(bool active)
  {
    this.MainButtonRoot.SetActive(active);
    this.dir_Roulette.SetActive(active);
    this.slc_Circle_Base.SetActive(active);
  }

  private IEnumerator ShowBattleRestartPopup()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_050_20__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  protected override void AllButtonTweenFinished()
  {
    base.AllButtonTweenFinished();
    this.CirculButton.condition = CircularMotionPositionSet.CirculCondition.START;
    if (!Persist.earthBattleEnvironment.Exists)
      return;
    this.StartCoroutine(this.ShowBattleRestartPopup());
  }

  public override IEnumerator CloudAnimationStart()
  {
    Mypage051Menu mypage051Menu = this;
    Singleton<CommonRoot>.GetInstance().StartBGTween(MypageMenuBase.EARTH_OUT_BG_TWEENGROUP);
    mypage051Menu.PlayTween(MypageMenuBase.END_TWEENGROUP_TOP);
    IEnumerator e = Singleton<CommonRoot>.GetInstance().StartCloudAnim(MypageCloudAnim.EarthOut, MypageCloudAnim.HeavenIn, (Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Singleton<NGSceneManager>.GetInstance().clearStack();
      EarthDataManager.DestoryInstance();
      SMManager.Swap();
      MasterDataCache.SetGameMode(MasterDataCache.GameMode.HEAVEN);
      MasterDataCache.GetList<int, UnitUnit>("UnitUnit", new Func<MasterDataReader, UnitUnit>(UnitUnit.Parse), (Func<UnitUnit, int>) (x => x.ID));
      MasterDataCache.GetList<int, UnitVoicePattern>("UnitVoicePattern", new Func<MasterDataReader, UnitVoicePattern>(UnitVoicePattern.Parse), (Func<UnitVoicePattern, int>) (x => x.ID));
      MasterDataCache.GetList<int, TipsTips>("TipsTips", new Func<MasterDataReader, TipsTips>(TipsTips.Parse), (Func<TipsTips, int>) (x => x.ID));
      MasterDataCache.GetList<int, UnitCharacter>("UnitCharacter", new Func<MasterDataReader, UnitCharacter>(UnitCharacter.Parse), (Func<UnitCharacter, int>) (x => x.ID));
      MasterDataCache.GetList<int, QuestStoryS>("QuestStoryS", new Func<MasterDataReader, QuestStoryS>(QuestStoryS.Parse), (Func<QuestStoryS, int>) (x => x.ID));
      MasterDataCache.GetList<int, QuestStoryL>("QuestStoryL", new Func<MasterDataReader, QuestStoryL>(QuestStoryL.Parse), (Func<QuestStoryL, int>) (x => x.ID));
      GC.Collect();
      GC.WaitForPendingFinalizers();
      Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy();
      MypageScene.ChangeScene(fromEarthTop: true);
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetFirstTweenDelegate()
  {
    this.AddStartTweenDelay(0.0f, MypageMenuBase.START_TWEEN_GROUP_JOGDIAL);
  }
}
