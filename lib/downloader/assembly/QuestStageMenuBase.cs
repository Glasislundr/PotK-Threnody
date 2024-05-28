// Decompiled with JetBrains decompiler
// Type: QuestStageMenuBase
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
public class QuestStageMenuBase : BackButtonMenuBase
{
  [SerializeField]
  private Quest0022MissionComplete complete;
  [SerializeField]
  protected UIButton btnBack;
  [SerializeField]
  private GameObject MissionAchievement;
  [SerializeField]
  private GameObject ButtonMission;
  [SerializeField]
  private UIWidget Character;
  [SerializeField]
  private NGxMaskSpriteWithScale SubBG;
  [SerializeField]
  private bool MissionActiveFlag;
  [SerializeField]
  protected int PassData;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected GameObject Container;
  [SerializeField]
  protected UILabel TxtGenderRestriction;
  [SerializeField]
  private UILabel missionClearCount;
  [SerializeField]
  private GameObject star;
  [SerializeField]
  private GameObject starEmpty;
  [SerializeField]
  private GameObject dirStar;
  [HideInInspector]
  public bool ButtonMove;
  [HideInInspector]
  public bool SceneStart;
  [HideInInspector]
  protected bool isFirstTime;
  private GameObject missionWindow;
  public GameObject missionCompleteItem;
  public GameObject missionItem;
  private GameObject EntryInfo;
  protected List<QuestConverterData> StageDataS;
  protected QuestStageEntryInfo EntryInfoScript;
  protected bool hardmode;
  public NGHorizontalScrollParts indicator;
  public Quest0022DetailDisplay container;
  public UIScrollView ScrollView;
  public List<GameObject> hscrollButtons;
  public Transform Middle;
  public Transform dirMission;
  public UIGrid grid;
  public GameObject nowCenterObj;
  public int initCenter;
  public SpringPanel.OnFinished onFinished;
  public List<TweenAlpha> startAllalphatween;
  public List<TweenPosition> startAllpostween;
  private QuestDetailManager detailManager_;
  protected Stack<int> stackSelectedSID_ = new Stack<int>();
  private bool activeIbtnMission;

  protected QuestDetailManager detailManager
  {
    get
    {
      if (this.detailManager_ == null)
        this.detailManager_ = new QuestDetailManager();
      return this.detailManager_;
    }
  }

  protected void UpdateButton()
  {
    this.HscrollButtonsAction();
    if (!this.ScrollView.isDragging && !this.ButtonMove)
      return;
    this.StartCoroutine(this.isDragTweenStart(true));
  }

  public void HscrollButtonsAction()
  {
    foreach (GameObject hscrollButton in this.hscrollButtons)
    {
      this.HscrollButtonTween(hscrollButton);
      this.HscrollButtonCenterChange(hscrollButton);
    }
  }

  private void HscrollButtonTween(GameObject targetButton)
  {
    if (this.ScrollView.isDragging || this.ButtonMove || Object.op_Inequality((Object) targetButton, (Object) this.nowCenterObj))
    {
      Quest0022Hscroll component = targetButton.GetComponent<Quest0022Hscroll>();
      if ((double) Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x + component.defaultPosition()) < (double) component.spaceValue())
        component.ChangeToneConditionJudge(Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x) - Mathf.Abs(component.defaultPosition()));
      else
        component.ChangeToneConditionJudge(Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x + component.defaultPosition()));
    }
    else
      this.CenterAnimation();
  }

  private void HscrollButtonCenterChange(GameObject targetButton)
  {
    if (this.isFirstTime || Object.op_Equality((Object) this.nowCenterObj, (Object) ((Component) ((Component) this.grid).transform).GetComponent<UICenterOnChild>().centeredObject) || !Object.op_Equality((Object) targetButton, (Object) ((Component) ((Component) this.grid).transform).GetComponent<UICenterOnChild>().centeredObject))
      return;
    this.CenterObjectAction(targetButton);
  }

  public static List<QuestConverterData> Convert(PlayerStoryQuestS[] s)
  {
    List<QuestConverterData> questConverterDataList = new List<QuestConverterData>();
    foreach (PlayerStoryQuestS story in s)
      questConverterDataList.Add(new QuestConverterData(story));
    return questConverterDataList;
  }

  public static List<QuestConverterData> Convert(PlayerExtraQuestS[] e, bool? isOpen)
  {
    List<QuestConverterData> questConverterDataList = new List<QuestConverterData>();
    foreach (PlayerExtraQuestS extra in e)
    {
      QuestConverterData questConverterData = new QuestConverterData(extra);
      if (isOpen.HasValue)
        questConverterData.canPlay = isOpen.Value && questConverterData.canPlay;
      questConverterDataList.Add(questConverterData);
    }
    return questConverterDataList;
  }

  public List<QuestConverterData> Convert(PlayerSeaQuestS[] s)
  {
    List<QuestConverterData> questConverterDataList = new List<QuestConverterData>();
    foreach (PlayerSeaQuestS story in s)
      questConverterDataList.Add(new QuestConverterData(story));
    return questConverterDataList;
  }

  public IEnumerator InitStoryQuest(
    PlayerStoryQuestS[] StoryDataS,
    int L,
    int M,
    int S,
    bool hard,
    bool forcus)
  {
    this.StageDataS = QuestStageMenuBase.Convert(StoryDataS);
    IEnumerator e = this.Init(this.StageDataS, L, M, S, hard, forcus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitSeaQuest(PlayerSeaQuestS[] StoryDataS, int L, int M, int S, bool forcus)
  {
    this.StageDataS = this.Convert(StoryDataS);
    IEnumerator e = this.Init(this.StageDataS, L, M, S, true, forcus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitExtraQuest(
    PlayerExtraQuestS[] ExtraDataS,
    int L,
    int M,
    int S,
    bool forcus,
    bool? isOpen = null)
  {
    this.StageDataS = QuestStageMenuBase.Convert(ExtraDataS, isOpen);
    IEnumerator e = this.Init(this.StageDataS, L, M, S, false, forcus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(
    List<QuestConverterData> StageDataS,
    int L,
    int M,
    int S,
    bool hard,
    bool forcus)
  {
    QuestStageMenuBase questStageMenuBase = this;
    questStageMenuBase.InitVal(StageDataS, L, M, S, hard, forcus);
    IEnumerator e = questStageMenuBase.UnitCreate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = questStageMenuBase.subBackGroundCrate(StageDataS[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (hard)
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        questStageMenuBase.StartCoroutine(questStageMenuBase.complete.missionCompleteRate_sea(StageDataS[0], M));
      else
        questStageMenuBase.StartCoroutine(questStageMenuBase.complete.missionCompleteRate(StageDataS[0], M));
    }
    e = questStageMenuBase.InitHscroll();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) questStageMenuBase.EntryInfo, (Object) null) && Object.op_Inequality((Object) questStageMenuBase.Container, (Object) null))
    {
      Future<GameObject> entryInfoPrefab = Res.Prefabs.Quest.dir_Entryinfo.Load<GameObject>();
      e = entryInfoPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      questStageMenuBase.EntryInfo = entryInfoPrefab.Result.Clone(questStageMenuBase.Container.transform);
      questStageMenuBase.EntryInfoScript = questStageMenuBase.EntryInfo.GetComponent<QuestStageEntryInfo>();
      entryInfoPrefab = (Future<GameObject>) null;
    }
    questStageMenuBase.CenterObjectAction(questStageMenuBase.hscrollButtons[questStageMenuBase.initCenter]);
    questStageMenuBase.hscrollButtons[questStageMenuBase.initCenter].GetComponent<QuestHScrollButton>().centerAnimation(false);
    questStageMenuBase.StartCoroutine(questStageMenuBase.MissionWindowCreate(StageDataS[0].type == CommonQuestType.Extra));
    questStageMenuBase.indicator.SeEnable = true;
    ((Behaviour) questStageMenuBase.btnBack).enabled = true;
    // ISSUE: method pointer
    ((Component) questStageMenuBase.grid).GetComponent<UICenterOnChild>().onFinished = new SpringPanel.OnFinished((object) questStageMenuBase, __methodptr(\u003CInit\u003Eb__52_0));
  }

  protected void pushSelectedSID(int sId) => this.stackSelectedSID_.Push(sId);

  protected bool popSelectedSID(ref int sId, ref bool bFocus, bool bOverwrite = false)
  {
    bool flag = false;
    if (this.stackSelectedSID_.Any<int>())
    {
      int num = this.stackSelectedSID_.Pop();
      if (bOverwrite || sId == -1 || !bFocus)
      {
        flag = true;
        sId = num;
        bFocus = true;
      }
    }
    return flag;
  }

  public void InitVal(
    List<QuestConverterData> qclistS,
    int L,
    int M,
    int S,
    bool hard,
    bool forcus)
  {
    this.hardmode = hard;
    this.ButtonMove = false;
    this.PassData = L;
    if (S == -1 || !forcus)
    {
      this.initCenter = qclistS.Count - 1 < 0 ? 0 : qclistS.Count - 1;
    }
    else
    {
      int? nullable = qclistS.FirstIndexOrNull<QuestConverterData>((Func<QuestConverterData, bool>) (x => x.id_S == S));
      this.initCenter = !nullable.HasValue || !forcus ? (qclistS.Count - 1 < 0 ? 0 : qclistS.Count - 1) : nullable.Value;
    }
    this.MissionActiveFlag = false;
    if (Object.op_Inequality((Object) this.MissionAchievement, (Object) null))
      this.MissionAchievement.SetActive(hard);
    this.TxtTitle.SetTextLocalize(qclistS[0].title_M);
    this.hscrollButtons = new List<GameObject>();
    this.missionWindow = (GameObject) null;
    this.activeIbtnMission = false;
    if (!Object.op_Inequality((Object) this.ButtonMission, (Object) null))
      return;
    ((UIButtonColor) this.ButtonMission.GetComponent<UIButton>()).isEnabled = true;
  }

  public IEnumerator UnitCreate()
  {
    foreach (Component component in ((Component) this.Character).transform)
      Object.Destroy((Object) component.gameObject);
    PlayerUnit playerUnit1;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      PlayerSeaDeck[] playerSeaDeckArray = SMManager.Get<PlayerSeaDeck[]>();
      PlayerUnit playerUnit2 = ((IEnumerable<PlayerUnit>) playerSeaDeckArray[Persist.seaDeckOrganized.Data.number].player_units).FirstOrDefault<PlayerUnit>();
      if ((object) playerUnit2 == null)
        playerUnit2 = ((IEnumerable<PlayerUnit>) playerSeaDeckArray[0].player_units).First<PlayerUnit>();
      playerUnit1 = playerUnit2;
    }
    else
    {
      PlayerDeck[] playerDeckArray = SMManager.Get<PlayerDeck[]>();
      PlayerUnit playerUnit3 = ((IEnumerable<PlayerUnit>) playerDeckArray[Persist.deckOrganized.Data.number].player_units).FirstOrDefault<PlayerUnit>();
      if ((object) playerUnit3 == null)
        playerUnit3 = ((IEnumerable<PlayerUnit>) playerDeckArray[0].player_units).First<PlayerUnit>();
      playerUnit1 = playerUnit3;
    }
    IEnumerator e = playerUnit1.unit.LoadQuestWithMask(playerUnit1.job_id, ((Component) this.Character).transform, ((Component) this.Character).GetComponent<UIWidget>().depth, Res.GUI._002_2_sozai.mask_chara.Load<Texture2D>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator subBackGroundCrate(QuestConverterData quest)
  {
    string path = string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) quest.sub_bg_name);
    Future<Sprite> bgSprite = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    IEnumerator e = bgSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Texture2D> loadMask = Res.GUI._002_2_sozai.bg_mask_quest_stage_select.Load<Texture2D>();
    e = loadMask.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SubBG.MainUI2DSprite.sprite2D = bgSprite.Result;
    this.SubBG.maskTexture = loadMask.Result;
    this.SubBG.xOffsetPixel = (int) quest.offset_x;
    this.SubBG.yOffsetPixel = (int) quest.offset_y;
    this.SubBG.scale = quest.scale;
    this.SubBG.FitMask();
  }

  public IEnumerator MissionWindowCreate(bool eventquest)
  {
    QuestStageMenuBase menu = this;
    bool isSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    Future<GameObject> prefabf = (Future<GameObject>) null;
    prefabf = !isSea ? Res.Prefabs.quest002_2.dir_Mission_List.Load<GameObject>() : new ResourceObject("Prefabs/quest002_2_sea/dir_Mission_List_sea").Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.missionWindow = prefabf.Result.Clone(menu.dirMission);
    prefabf = !isSea ? new ResourceObject("Prefabs/quest002_2/dir_Misson_List_Item").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2_sea/dir_Misson_List_Item_sea").Load<GameObject>();
    e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.missionItem = prefabf.Result;
    prefabf = !isSea ? new ResourceObject("Prefabs/quest002_2/dir_Misson_List_Item_Comp").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2_sea/dir_Misson_List_Item_Comp_sea").Load<GameObject>();
    e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.missionCompleteItem = prefabf.Result;
    Quest0022MissionDescriptions component = menu.missionWindow.GetComponent<Quest0022MissionDescriptions>();
    PlayerMissionHistory[] array;
    if (!eventquest)
    {
      if (isSea)
      {
        component.missionDataSea = MasterData.QuestSeaMissionList;
        array = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 9)).ToArray<PlayerMissionHistory>();
      }
      else
      {
        component.missionDataS = MasterData.QuestStoryMissionList;
        array = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 1)).ToArray<PlayerMissionHistory>();
      }
    }
    else
    {
      component.missionDataE = MasterData.QuestExtraMissionList;
      array = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 3)).ToArray<PlayerMissionHistory>();
    }
    component.hists = array;
    if (Object.op_Implicit((Object) component.description.btnClose))
      EventDelegate.Set(component.description.btnClose.onClick, new EventDelegate.Callback(menu.onCloseMission));
    menu.StartCoroutine(component.InitMissionDescription(menu, menu.hscrollButtons[menu.initCenter].GetComponent<Quest0022Hscroll>().MissionNum, menu.hscrollButtons[menu.initCenter].GetComponent<Quest0022Hscroll>().id()));
  }

  public IEnumerator InitHscroll()
  {
    QuestStageMenuBase questStageMenuBase = this;
    questStageMenuBase.isFirstTime = true;
    bool eventquest = questStageMenuBase.StageDataS[0].type == CommonQuestType.Extra;
    Future<GameObject> Hscroll = eventquest ? Res.Prefabs.quest002_20.hscroll_640_14.Load<GameObject>() : (!Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.quest002_2.hscroll_640.Load<GameObject>() : Res.Prefabs.quest002_2_sea.hscroll_640_sea.Load<GameObject>());
    IEnumerator e = Hscroll.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = Hscroll.Result;
    questStageMenuBase.indicator.destroyParts();
    for (int index = 0; index < questStageMenuBase.StageDataS.Count; ++index)
    {
      GameObject gameObject = questStageMenuBase.indicator.instantiateParts(result);
      questStageMenuBase.hscrollButtons.Add(gameObject);
    }
    for (int i = questStageMenuBase.StageDataS.Count - 1; i >= 0; --i)
    {
      e = questStageMenuBase.initHscrollObj(questStageMenuBase.hscrollButtons[i], questStageMenuBase.StageDataS[i], questStageMenuBase.grid.cellWidth, questStageMenuBase.initCenter, new Action(questStageMenuBase.tweenSettingDefault), eventquest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    questStageMenuBase.indicator.resetScrollView();
    questStageMenuBase.indicator.setItemPositionQuick(questStageMenuBase.initCenter);
    foreach (GameObject hscrollButton in questStageMenuBase.hscrollButtons)
    {
      if (Object.op_Inequality((Object) hscrollButton, (Object) questStageMenuBase.hscrollButtons[questStageMenuBase.initCenter]))
        hscrollButton.GetComponent<Quest0022Hscroll>().NotTouch(true);
    }
    questStageMenuBase.hscrollButtons.ForEach((Action<GameObject>) (x => x.GetComponent<Quest0022Hscroll>().onSetValue()));
    questStageMenuBase.setMissionData();
  }

  private IEnumerator initHscrollObj(
    GameObject go,
    QuestConverterData StageData,
    float gridWidth,
    int center,
    Action act,
    bool eventquest)
  {
    QuestStageMenuBase menu = this;
    bool storyOnly = QuestStageMenuBase.checkForStoryOnly(StageData.type, StageData.id_S);
    IEnumerator e = go.GetComponent<Quest0022Hscroll>().Init(menu, StageData, gridWidth, center, act, eventquest, storyOnly, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.pushSelectedSID(StageData.id_S);
      Quest00221Scene.changeDetailScene(this.detailManager.getData(StageData.type, StageData.id_S, StageData.wave != null), true);
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static bool checkForStoryOnly(CommonQuestType questType, int numS)
  {
    bool flag = false;
    switch (questType)
    {
      case CommonQuestType.Story:
        QuestStoryS questStoryS;
        if (MasterData.QuestStoryS.TryGetValue(numS, out questStoryS))
        {
          flag = questStoryS.story_only;
          break;
        }
        break;
      case CommonQuestType.Extra:
        QuestExtraS questExtraS;
        if (MasterData.QuestExtraS.TryGetValue(numS, out questExtraS))
        {
          flag = questExtraS.story_only;
          break;
        }
        break;
      case CommonQuestType.Sea:
        QuestSeaS questSeaS;
        if (MasterData.QuestSeaS.TryGetValue(numS, out questSeaS))
        {
          flag = questSeaS.story_only;
          break;
        }
        break;
    }
    return flag;
  }

  public void setMissionData()
  {
    switch (this.StageDataS[0].type)
    {
      case CommonQuestType.Story:
        this.setMissionData(MasterData.QuestStoryMissionList, ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 1)).ToArray<PlayerMissionHistory>());
        break;
      case CommonQuestType.Extra:
        this.setMissionData(MasterData.QuestExtraMissionList, ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 3)).ToArray<PlayerMissionHistory>());
        break;
      case CommonQuestType.Sea:
        this.setMissionData(MasterData.QuestSeaMissionList, ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 9)).ToArray<PlayerMissionHistory>());
        break;
    }
  }

  public void setMissionData(QuestStoryMission[] missionData, PlayerMissionHistory[] hists)
  {
    for (int i = 0; i < this.StageDataS.Count; i++)
    {
      Quest0022Hscroll component = this.hscrollButtons[i].GetComponent<Quest0022Hscroll>();
      component.missionList = new List<bool>();
      List<bool> missionList = component.missionList;
      QuestStoryMission[] array = ((IEnumerable<QuestStoryMission>) missionData).Where<QuestStoryMission>((Func<QuestStoryMission, bool>) (x => this.StageDataS[i].id_S == x.quest_s_QuestStoryS)).ToArray<QuestStoryMission>();
      int length = array.Length;
      int num = 0;
      if (length > 0)
      {
        for (int index = 0; index < length; ++index)
        {
          missionList.Add(((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (y => y.mission_id)).Contains<int>(array[index].ID));
          if (missionList[index])
            ++num;
        }
      }
      component.MissionNum = length;
      component.MissionClearNum = num;
    }
  }

  public void setMissionData(QuestExtraMission[] missionData, PlayerMissionHistory[] hists)
  {
    for (int i = 0; i < this.StageDataS.Count; i++)
    {
      Quest0022Hscroll component = this.hscrollButtons[i].GetComponent<Quest0022Hscroll>();
      component.missionList = new List<bool>();
      List<bool> missionList = component.missionList;
      QuestExtraMission[] array = ((IEnumerable<QuestExtraMission>) missionData).Where<QuestExtraMission>((Func<QuestExtraMission, bool>) (x => this.StageDataS[i].id_S == x.quest_s_QuestExtraS)).ToArray<QuestExtraMission>();
      int length = array.Length;
      int num = 0;
      if (length > 0)
      {
        for (int index = 0; index < length; ++index)
        {
          missionList.Add(((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (y => y.mission_id)).Contains<int>(array[index].ID));
          if (missionList[index])
            ++num;
        }
      }
      component.MissionNum = length;
      component.MissionClearNum = num;
    }
  }

  public void setMissionData(QuestSeaMission[] missionData, PlayerMissionHistory[] hists)
  {
    for (int i = 0; i < this.StageDataS.Count; i++)
    {
      Quest0022Hscroll component = this.hscrollButtons[i].GetComponent<Quest0022Hscroll>();
      component.missionList = new List<bool>();
      List<bool> missionList = component.missionList;
      QuestSeaMission[] array = ((IEnumerable<QuestSeaMission>) missionData).Where<QuestSeaMission>((Func<QuestSeaMission, bool>) (x => this.StageDataS[i].id_S == x.quest_s_QuestSeaS)).ToArray<QuestSeaMission>();
      int length = array.Length;
      int num = 0;
      if (length > 0)
      {
        for (int index = 0; index < length; ++index)
        {
          missionList.Add(((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (y => y.mission_id)).Contains<int>(array[index].ID));
          if (missionList[index])
            ++num;
        }
      }
      component.MissionNum = length;
      component.MissionClearNum = num;
    }
  }

  public void CenterObjectAction(GameObject targetButton)
  {
    Quest0022Hscroll component = targetButton.GetComponent<Quest0022Hscroll>();
    this.hscrollButtons.ForEach((Action<GameObject>) (x => x.GetComponent<Quest0022Hscroll>().NotTouch(true)));
    component.NotTouch(false);
    if (Object.op_Inequality((Object) targetButton, (Object) this.nowCenterObj))
    {
      this.nowCenterObj = targetButton;
      int missionNum = this.nowCenterObj.GetComponent<Quest0022Hscroll>().MissionNum;
      bool flag1 = missionNum > 0;
      this.ButtonMission.SetActive(flag1);
      this.dirStar.SetActive(flag1);
      if (Object.op_Inequality((Object) this.missionWindow, (Object) null))
        this.missionWindow.SetActive(flag1);
      int num = this.nowCenterObj.GetComponent<Quest0022Hscroll>().missionList.Count<bool>((Func<bool, bool>) (x => x));
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        this.missionClearCount.SetTextLocalize("[008AFF]" + (object) num + "[-][4E4E4E]/" + (object) missionNum + "[-]");
      else
        this.missionClearCount.SetTextLocalize("[ffff00]" + (object) num + "[-]/" + (object) missionNum);
      bool flag2 = num == missionNum;
      this.star.SetActive(flag2);
      this.starEmpty.SetActive(!flag2);
      QuestScoreCampaignProgress[] source = SMManager.Get<QuestScoreCampaignProgress[]>();
      foreach (QuestConverterData questConverterData in this.StageDataS)
      {
        QuestConverterData stageDataS = questConverterData;
        QuestScoreCampaignProgress qscp = (QuestScoreCampaignProgress) null;
        if (source != null && source.Length != 0)
          qscp = ((IEnumerable<QuestScoreCampaignProgress>) source).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == stageDataS.id_L));
        if (component.id() == stageDataS.id_S)
          this.container.InitDetailDisplay(stageDataS, component.stageNumber(), qscp);
      }
    }
    this.SetTextLimitation(component.id());
  }

  protected virtual void SetTextLimitation(int s_id)
  {
    this.EntryInfoScript.IsDisplay = false;
    ((Component) this.TxtGenderRestriction).gameObject.SetActive(false);
  }

  public void startMissionSet()
  {
    if (this.activeIbtnMission)
      return;
    this.activeIbtnMission = true;
    this.StartCoroutine(this.ibtnMission());
  }

  private IEnumerator ibtnMission()
  {
    QuestStageMenuBase menu = this;
    menu.MissionActiveFlag = !menu.MissionActiveFlag;
    if (menu.MissionActiveFlag)
    {
      IEnumerator e = menu.missionWindow.GetComponent<Quest0022MissionDescriptions>().InitMissionDescription(menu, menu.nowCenterObj.GetComponent<Quest0022Hscroll>().MissionNum, menu.nowCenterObj.GetComponent<Quest0022Hscroll>().id());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Quest0022MissionDescriptions component = menu.missionWindow.GetComponent<Quest0022MissionDescriptions>();
    component.StartTweenClick(menu.MissionActiveFlag);
    menu.activeIbtnMission = false;
    if (Object.op_Inequality((Object) component.description, (Object) null) && Object.op_Inequality((Object) component.description.btnClose, (Object) null))
      ((UIButtonColor) menu.ButtonMission.GetComponent<UIButton>()).isEnabled = !menu.MissionActiveFlag;
  }

  public void ibtRankingEventReward()
  {
    QuestScoreCampaignProgress campaignProgress = ((IEnumerable<QuestScoreCampaignProgress>) SMManager.Get<QuestScoreCampaignProgress[]>()).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == this.StageDataS[0].id_L));
    QuestScoreCampaignProgressScore_achivement_rewards achivement_reward = ((IEnumerable<QuestScoreCampaignProgressScore_achivement_rewards>) campaignProgress.score_achivement_rewards).FirstOrDefault<QuestScoreCampaignProgressScore_achivement_rewards>((Func<QuestScoreCampaignProgressScore_achivement_rewards, bool>) (x => x.quest_extra_m == this.StageDataS[0].id_M));
    QuestExtraM questExtraM;
    if (achivement_reward == null || !MasterData.QuestExtraM.TryGetValue(achivement_reward.quest_extra_m, out questExtraM))
      return;
    int questMscoreFromMid = campaignProgress.GetQuestMScoreFromMID(questExtraM.ID);
    Quest002272Scene.ChangeScene(true, achivement_reward, campaignProgress.player.score_achivement_reward_cleared, questExtraM.name, questMscoreFromMid);
  }

  public IEnumerator isDragTweenStart(bool flag)
  {
    QuestStageMenuBase menu = this;
    if (menu.isFirstTime)
      menu.isFirstTime = false;
    if (menu.ButtonMove != flag)
    {
      menu.container.StartTween(flag);
      if (menu.MissionActiveFlag)
      {
        if (!flag)
        {
          IEnumerator e = menu.missionWindow.GetComponent<Quest0022MissionDescriptions>().InitMissionDescription(menu, menu.nowCenterObj.GetComponent<Quest0022Hscroll>().MissionNum, menu.nowCenterObj.GetComponent<Quest0022Hscroll>().id());
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        menu.missionWindow.GetComponent<Quest0022MissionDescriptions>().StartTween(flag);
      }
    }
    menu.ButtonMove = flag;
  }

  public void CenterAnimation()
  {
    GameObject target = this.nowCenterObj;
    target.GetComponent<Quest0022Hscroll>().centerAnimation(true);
    this.hscrollButtons.ForEach((Action<GameObject>) (x =>
    {
      if (!Object.op_Inequality((Object) x, (Object) target))
        return;
      x.GetComponent<Quest0022Hscroll>().centerAnimation(false);
    }));
  }

  public void tweenSettingDefault()
  {
    this.startAllpostween.ForEach((Action<TweenPosition>) (x => ((UITweener) x).tweenGroup = Math.Abs(((UITweener) x).tweenGroup)));
    this.startAllalphatween.ForEach((Action<TweenAlpha>) (x => ((UITweener) x).tweenGroup = Math.Abs(((UITweener) x).tweenGroup)));
  }

  public void tweenSettingNoAnim()
  {
    this.startAllpostween.ForEach((Action<TweenPosition>) (x =>
    {
      if (((UITweener) x).tweenGroup != 11 && ((UITweener) x).tweenGroup != 12)
        return;
      ((UITweener) x).tweenGroup = -Math.Abs(((UITweener) x).tweenGroup);
      ((Component) x).gameObject.transform.localPosition = x.to;
    }));
    this.startAllalphatween.ForEach((Action<TweenAlpha>) (x =>
    {
      if (((UITweener) x).tweenGroup != 11 && ((UITweener) x).tweenGroup != 12)
        return;
      ((UITweener) x).tweenGroup = -Math.Abs(((UITweener) x).tweenGroup);
      ((UIRect) ((Component) x).GetComponent<UIWidget>()).alpha = x.to;
    }));
  }

  public virtual void IbtnBack() => this.backScene();

  public override void onBackButton() => this.IbtnBack();

  public virtual void onEndScene()
  {
    ((Component) this.grid).GetComponent<UICenterOnChild>().onFinished = (SpringPanel.OnFinished) null;
    if (!Object.op_Inequality((Object) this.missionWindow, (Object) null))
      return;
    this.MissionActiveFlag = false;
    this.activeIbtnMission = false;
    Object.Destroy((Object) this.missionWindow);
  }

  public virtual void onCloseMission()
  {
    if (!this.MissionActiveFlag || this.activeIbtnMission)
      return;
    this.activeIbtnMission = true;
    this.StartCoroutine(this.ibtnMission());
  }

  protected virtual void OnEnable()
  {
    if (!this.ScrollView.isDragging)
      return;
    this.ScrollView.Press(false);
  }
}
