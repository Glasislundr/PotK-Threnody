// Decompiled with JetBrains decompiler
// Type: Quest00240723Menu
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
public class Quest00240723Menu : BackButtonMenuBase
{
  public const int FirstLostRagnarok_L_ID = 19;
  public const int FirstIntegralNoah_L_ID = 36;
  public const int FirstEverAfter_L_ID = 57;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private UISprite SlcBeginerSupport;
  public List<GameObject> StoryM;
  private int originID;
  private int tweenFinishCount;
  private float minStartDelay;
  private GameObject StoryButtons;
  private PlayerStoryQuestS[] StoryData;
  private Quest00240723Menu.StoryCondition thisCondition;
  [SerializeField]
  private GameObject StoryButtonParent;
  [SerializeField]
  private GameObject ibtnHardSwitch;
  [HideInInspector]
  public int passLdata;
  [HideInInspector]
  public bool restart;
  [HideInInspector]
  public bool from2_5;
  private const int STORY_BTN_TWEEN_START = 41;
  private const int STORY_BTN_TWEEN_END = 42;
  private Quest00240723Menu.StoryMode currentStoryMode;
  private int lastL_id_lostRagnarok = -1;
  private int lastL_id_heaven = -1;
  private int lastL_id = -1;
  private Quest00240723Menu.StoryMode lastCurrentStoryMode = Quest00240723Menu.StoryMode.None;

  public IEnumerator Init(
    PlayerStoryQuestS[] StoryData,
    int ChoiceLnum,
    bool reStart = false,
    bool mustCreateBG = false)
  {
    Quest00240723Menu quest00240723Menu = this;
    quest00240723Menu.StoryData = StoryData;
    quest00240723Menu.restart = reStart;
    ((Component) quest00240723Menu.SlcBeginerSupport).gameObject.SetActive(Quest00240723Menu.IsBeginnerSupport());
    quest00240723Menu.currentStoryMode = quest00240723Menu.GetStoryMode(StoryData, ChoiceLnum);
    PlayerStoryQuestS[] StoryDataM = StoryData.M((int) quest00240723Menu.currentStoryMode, ChoiceLnum);
    quest00240723Menu.ibtnHardSwitch.SetActive(quest00240723Menu.SetActiveHardSwitch(StoryData, StoryDataM, ChoiceLnum));
    if (quest00240723Menu.ibtnHardSwitch.activeSelf && !quest00240723Menu.restart)
    {
      Quest0024KillModeButton component = quest00240723Menu.ibtnHardSwitch.GetComponent<Quest0024KillModeButton>();
      component.Init(quest00240723Menu.thisCondition == Quest00240723Menu.StoryCondition.HARD);
      component.SwitchKillMode(quest00240723Menu.thisCondition == Quest00240723Menu.StoryCondition.HARD);
    }
    int L_id = ChoiceLnum;
    IEnumerator e;
    if (!quest00240723Menu.restart)
    {
      BGChange bgChange = ((Component) quest00240723Menu).GetComponent<BGChange>();
      bgChange.getCurrentBG();
      if (Object.op_Equality((Object) bgChange.Current, (Object) null) | mustCreateBG)
      {
        QuestStoryL questStoryL;
        if (quest00240723Menu.thisCondition == Quest00240723Menu.StoryCondition.HARD && MasterData.QuestStoryL.TryGetValue(L_id, out questStoryL) && questStoryL.origin_id.HasValue)
          L_id = questStoryL.origin_id.Value;
        e = bgChange.QuestBGprefabCreate(L_id, mustCreate: mustCreateBG);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      float fade_duration = 1f;
      if (quest00240723Menu.lastCurrentStoryMode != Quest00240723Menu.StoryMode.None && (quest00240723Menu.lastCurrentStoryMode != quest00240723Menu.currentStoryMode || quest00240723Menu.lastL_id == L_id))
        fade_duration = 0.0f;
      bgChange.CrossToL(fade_duration: fade_duration);
      if (quest00240723Menu.from2_5)
        ((Component) quest00240723Menu).GetComponent<BGChange>().BlackHangingBackGround(quest00240723Menu.thisCondition == Quest00240723Menu.StoryCondition.HARD, true);
      foreach (Component component in quest00240723Menu.StoryButtonParent.transform)
        Object.DestroyImmediate((Object) component.gameObject);
      int id = quest00240723Menu.thisCondition == Quest00240723Menu.StoryCondition.HARD ? quest00240723Menu.originID : ChoiceLnum;
      Future<GameObject> btnsF = quest00240723Menu.GetPassStoryBtnObj(id);
      e = btnsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00240723Menu.StoryButtons = btnsF.Result.Clone(quest00240723Menu.StoryButtonParent.transform);
      quest00240723Menu.StoryM = quest00240723Menu.StoryButtons.GetComponent<Quest0024StoryButtons>().StoryButtns;
      Future<GameObject> prefab = Res.Prefabs.quest002_4.story_btns.dir_Story.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      foreach (GameObject gameObject in quest00240723Menu.StoryM)
        prefab.Result.Clone(gameObject.transform);
      bgChange = (BGChange) null;
      btnsF = (Future<GameObject>) null;
      prefab = (Future<GameObject>) null;
    }
    foreach (var data in quest00240723Menu.StoryM.Select((s, i) => new
    {
      s = s,
      i = i
    }))
      quest00240723Menu.InitStoryChoiceButton(data.s, data.i, !quest00240723Menu.restart, StoryDataM);
    quest00240723Menu.passLdata = ChoiceLnum;
    e = quest00240723Menu.StoryChoiceButtonSetting(StoryData, StoryDataM);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (quest00240723Menu.restart)
      quest00240723Menu.ibtnHardSwitch.GetComponent<Quest0024KillModeButton>().SwitchStartTweensPlay(quest00240723Menu.thisCondition == Quest00240723Menu.StoryCondition.HARD);
    if (((IEnumerable<PlayerStoryQuestS>) StoryDataM).Count<PlayerStoryQuestS>() > 0)
      quest00240723Menu.TxtTitle.SetText(StoryDataM[0].quest_story_s.quest_l.name);
    quest00240723Menu.restart = false;
    quest00240723Menu.IsPush = false;
    yield return (object) new WaitForEndOfFrame();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    quest00240723Menu.lastL_id = L_id;
    quest00240723Menu.lastCurrentStoryMode = quest00240723Menu.currentStoryMode;
  }

  public static bool IsBeginnerSupport() => SMManager.Get<Player>().level <= 20;

  private Future<GameObject> GetPassStoryBtnObj(int id)
  {
    string path = this.currentStoryMode != Quest00240723Menu.StoryMode.EverAfter ? (this.currentStoryMode != Quest00240723Menu.StoryMode.IntegralNoah ? (this.currentStoryMode != Quest00240723Menu.StoryMode.LostRagnarok ? string.Format("Prefabs/quest002_4/story_btns/{0}/Story_btn", (object) id) : string.Format("Prefabs/quest002_4/story_btns_LostRagnarok/{0}/Story_btn", (object) (id - 19 + 1))) : string.Format("Prefabs/quest002_4/story_btns_IntegralNoah/{0}/Story_btn", (object) (id - 36 + 1))) : string.Format("Prefabs/quest002_4/story_btns_EverAfter/{0}/Story_btn", (object) (id - 57 + 1));
    return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().Load<GameObject>(path) : Res.Prefabs.quest002_4.story_btns._1.Story_btn.Load<GameObject>();
  }

  private void InitStoryChoiceButton(
    GameObject btnContainer,
    int index,
    bool isStart,
    PlayerStoryQuestS[] StoryDataM)
  {
    Quest0024StoryButton componentInChildren = ((Component) btnContainer.transform).GetComponentInChildren<Quest0024StoryButton>();
    componentInChildren.Lock();
    if (StoryDataM.Length > index)
    {
      int bonusCategory = StoryDataM[index].bonus_category;
      componentInChildren.SetBonus(bonusCategory);
    }
    if (!isStart)
      return;
    TweenPosition component1 = ((Component) componentInChildren).GetComponent<TweenPosition>();
    TweenAlpha component2 = ((Component) componentInChildren).GetComponent<TweenAlpha>();
    float num = (float) (0.5 + (double) index * 0.10000000149011612);
    ((UITweener) component1).delay = num;
    ((UITweener) component2).delay = num;
    ((UITweener) component1).onFinished.Clear();
    EventDelegate.Set(((UITweener) component1).onFinished, new EventDelegate(new EventDelegate.Callback(componentInChildren.playButtonReverseTween))
    {
      oneShot = true
    });
    ((Behaviour) componentInChildren.ibtnStory).enabled = false;
  }

  private IEnumerator StoryChoiceButtonSetting(
    PlayerStoryQuestS[] StoryData,
    PlayerStoryQuestS[] StoryDataM)
  {
    QuestBG.AnimApply[] source3 = new QuestBG.AnimApply[5]
    {
      QuestBG.AnimApply.Button1,
      QuestBG.AnimApply.Button2,
      QuestBG.AnimApply.Button3,
      QuestBG.AnimApply.Button4,
      QuestBG.AnimApply.Button5
    };
    this.StoryM.ForEach<GameObject, PlayerStoryQuestS, QuestBG.AnimApply>((IEnumerable<PlayerStoryQuestS>) StoryDataM, (IEnumerable<QuestBG.AnimApply>) source3, (Action<GameObject, PlayerStoryQuestS, QuestBG.AnimApply>) ((storyM, storyDataM, animApply) =>
    {
      PlayerStoryQuestS[] source = StoryData.S((int) this.currentStoryMode, this.passLdata, storyDataM.quest_story_s.quest_m_QuestStoryM);
      bool clearflag = ((IEnumerable<PlayerStoryQuestS>) source).Count<PlayerStoryQuestS>() == ((IEnumerable<PlayerStoryQuestS>) ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.is_clear)).ToArray<PlayerStoryQuestS>()).Count<PlayerStoryQuestS>();
      Quest0024StoryButton storyButton = ((Component) storyM.transform).GetComponentInChildren<Quest0024StoryButton>();
      storyButton.UnLock(clearflag, source[0].is_new);
      storyButton.onClick = new EventDelegate((EventDelegate.Callback) (() =>
      {
        this.StoryM.ForEach((Action<GameObject>) (x => ((Behaviour) x.GetComponentInChildren<Quest0024StoryButton>().ibtnStory).enabled = false));
        storyButton.changeClickMySelf();
        this.ChangeScene(storyButton.Mnumber);
      }));
      storyButton.PathNumber = source[0].quest_story_s.quest_m.number_m;
      storyButton.Mnumber = source[0].quest_story_s.quest_m_QuestStoryM;
    }));
    for (int i = 0; i < ((IEnumerable<PlayerStoryQuestS>) StoryDataM).Count<PlayerStoryQuestS>(); ++i)
    {
      IEnumerator e = ((Component) this.StoryM[i].transform).GetComponentInChildren<Quest0024StoryButton>().InitButton(this.thisCondition == Quest00240723Menu.StoryCondition.HARD, this.currentStoryMode);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    int[] array = ((IEnumerable<PlayerStoryQuestS>) StoryDataM).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_m_QuestStoryM)).Distinct<int>().OrderBy<int, int>((Func<int, int>) (y => y)).ToArray<int>();
    for (int index = 0; index < this.StoryM.Count; ++index)
    {
      if (((Component) this.StoryM[index].transform).GetComponentInChildren<Quest0024StoryButton>().isActive())
      {
        if (index >= array.Length)
          break;
        PlayerStoryQuestS[] playerStoryQuestSArray = StoryData.S((int) this.currentStoryMode, this.passLdata, array[index]);
        if (playerStoryQuestSArray.Length != 0)
          this.MissionAchievementRate(playerStoryQuestSArray[0], ((Component) this.StoryM[index].transform).GetComponentInChildren<Quest0024StoryButton>());
      }
    }
  }

  private void MissionAchievementRate(PlayerStoryQuestS quest, Quest0024StoryButton button)
  {
    int num = 0;
    int nowCount = 0;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 1)).ToArray<PlayerMissionHistory>();
    QuestStoryMission[] array2 = ((IEnumerable<QuestStoryMission>) MasterData.QuestStoryMissionList).Where<QuestStoryMission>((Func<QuestStoryMission, bool>) (x => x.quest_s.quest_m_QuestStoryM == quest.quest_story_s.quest_m_QuestStoryM)).ToArray<QuestStoryMission>();
    foreach (QuestStoryMission questStoryMission in array2)
      nowCount += ((IEnumerable<PlayerMissionHistory>) array1).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(questStoryMission.ID) ? 1 : 0;
    int allCount = num + array2.Length;
    button.MissionAchevement(nowCount, allCount);
  }

  public void XLButton() => this.XLChangeScene();

  public void ChangeScene(int passMdata)
  {
    Debug.Log((object) ("M:" + (object) this.passLdata + "\nL:" + (object) passMdata));
    Quest0022Scene.ChangeScene0022(false, this.passLdata, passMdata);
  }

  public void XLChangeScene()
  {
    if (this.IsPushAndSet())
      return;
    Debug.Log((object) ("\nXL:" + (object) this.passLdata));
    this.StartCoroutine(this.ExecXLChangeScene());
  }

  private IEnumerator ExecXLChangeScene()
  {
    Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().CloudAnim(true);
    if (this.thisCondition == Quest00240723Menu.StoryCondition.HARD)
      this.passLdata = this.originID;
    Quest0025Scene.changeScene0025(false, new Quest0025Scene.Quest0025Param((int) this.currentStoryMode, this.passLdata, this.thisCondition == Quest00240723Menu.StoryCondition.HARD));
    yield break;
  }

  public void ClickHardSwitch()
  {
    bool flag = this.thisCondition == Quest00240723Menu.StoryCondition.NORMAL;
    this.ibtnHardSwitch.GetComponent<Quest0024KillModeButton>().ClickToKillMode(flag);
    ((Component) this).GetComponent<BGChange>().BlackHangingBackGround(flag);
    this.SwitchQuestStory(flag);
    this.thisCondition = flag ? Quest00240723Menu.StoryCondition.HARD : Quest00240723Menu.StoryCondition.NORMAL;
    this.StoryChoiceButtonTween(41);
  }

  private bool SetActiveHardSwitch(
    PlayerStoryQuestS[] StoryData,
    PlayerStoryQuestS[] StoryDataNow,
    int ChoiceLnum)
  {
    this.originID = 0;
    PlayerStoryQuestS playerStoryQuestS = StoryDataNow == null || StoryDataNow.Length == 0 ? (PlayerStoryQuestS) null : StoryDataNow[0];
    bool flag1 = false;
    if (playerStoryQuestS != null)
      flag1 = StoryDataNow[0].quest_story_s.quest_l.quest_mode == CommonQuestMode.kill_mode;
    bool flag2 = ((IEnumerable<PlayerStoryQuestS>) StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.quest_mode == CommonQuestMode.kill_mode)).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.origin_id.Value == ChoiceLnum));
    if (!flag1)
      this.thisCondition = Quest00240723Menu.StoryCondition.NORMAL;
    else if (flag1)
    {
      this.thisCondition = Quest00240723Menu.StoryCondition.HARD;
      this.originID = playerStoryQuestS.quest_story_s.quest_l.origin_id.Value;
    }
    return flag1 | flag2;
  }

  private void SwitchQuestStory(bool toHard)
  {
    if (Object.op_Equality((Object) this.StoryButtons, (Object) null))
      return;
    UITweener uiTweener = ((IEnumerable<UITweener>) this.StoryButtons.GetComponents<UITweener>()).Where<UITweener>((Func<UITweener, bool>) (x => x.tweenGroup == 41)).First<UITweener>();
    if (toHard)
    {
      int passHard = ((IEnumerable<PlayerStoryQuestS>) this.StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.origin_id.HasValue && x.quest_story_s.quest_l.origin_id.Value == this.passLdata)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL)).First<int>();
      EventDelegate.Set(uiTweener.onFinished, (EventDelegate.Callback) (() => this.StartCoroutine(this.SwitchModeInitialze(this.StoryData, passHard, true, true))));
    }
    else
    {
      int passNormal = ((IEnumerable<PlayerStoryQuestS>) this.StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.ID == this.passLdata)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l.origin_id.Value)).First<int>();
      EventDelegate.Set(uiTweener.onFinished, (EventDelegate.Callback) (() => this.StartCoroutine(this.SwitchModeInitialze(this.StoryData, passNormal, false, true))));
    }
  }

  private IEnumerator SwitchModeInitialze(
    PlayerStoryQuestS[] StoryData,
    int passdata,
    bool toHard,
    bool reStart)
  {
    IEnumerator e = this.Init(StoryData, passdata, reStart);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.StoryChoiceButtonTween(42);
  }

  public void StoryChoiceButtonTween(int tweengroup)
  {
    if (Object.op_Equality((Object) this.StoryButtons, (Object) null))
      return;
    ((IEnumerable<UITweener>) this.StoryButtons.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != tweengroup)
        return;
      x.ResetToBeginning();
      x.PlayForward();
    }));
  }

  private Quest00240723Menu.StoryMode GetStoryMode(PlayerStoryQuestS[] StoryData, int L)
  {
    if (StoryData == null)
      return L < 19 ? Quest00240723Menu.StoryMode.Heaven : Quest00240723Menu.StoryMode.LostRagnarok;
    PlayerStoryQuestS playerStoryQuestS = ((IEnumerable<PlayerStoryQuestS>) StoryData).FirstOrDefault<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l_QuestStoryL == L));
    if (playerStoryQuestS != null)
      return (Quest00240723Menu.StoryMode) playerStoryQuestS.quest_story_s.quest_xl_QuestStoryXL;
    return L < 19 ? Quest00240723Menu.StoryMode.Heaven : Quest00240723Menu.StoryMode.LostRagnarok;
  }

  public void IbtnStoryChangeHeaven()
  {
    Singleton<CommonRoot>.GetInstance().GetHeavenCommonFooter().onButtonQuest();
  }

  public void IbtnStoryChangeLostRagnarok()
  {
    Singleton<CommonRoot>.GetInstance().GetHeavenCommonFooter().onButtonQuest();
  }

  private IEnumerator ChangeStoryMode(PlayerStoryQuestS[] stories, int L)
  {
    float startTime = Time.time;
    Singleton<NGSceneManager>.GetInstance().sceneBase.endTweens();
    while (!Singleton<NGSceneManager>.GetInstance().sceneBase.isTweenFinished && (double) Time.time - (double) startTime <= (double) Singleton<NGSceneManager>.GetInstance().sceneBase.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) new WaitForSeconds(0.1f);
    if (Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>(), (Object) null))
      Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().current = (GameObject) null;
    Quest00240723Scene.ChangeScene0024(false, L, false, false);
  }

  public virtual void IbtnBack() => Debug.Log((object) "click default event IbtnBack");

  public virtual void IbtnStory01() => Debug.Log((object) "click default event IbtnStory01");

  public virtual void IbtnStory02() => Debug.Log((object) "click default event IbtnStory02");

  public virtual void IbtnStory03() => Debug.Log((object) "click default event IbtnStory03");

  public virtual void IbtnStory04() => Debug.Log((object) "click default event IbtnStory04");

  public virtual void IbtnStory05() => Debug.Log((object) "click default event IbtnStory05");

  public override void onBackButton()
  {
    if (Singleton<NGSceneManager>.GetInstance().backScene())
      return;
    MypageScene.ChangeScene();
  }

  public enum StoryMode
  {
    None = -1, // 0xFFFFFFFF
    Heaven = 1,
    LostRagnarok = 4,
    IntegralNoah = 6,
    EverAfter = 7,
  }

  public enum StoryCondition
  {
    NORMAL,
    HARD,
  }
}
