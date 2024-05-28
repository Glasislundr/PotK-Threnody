// Decompiled with JetBrains decompiler
// Type: PopupUnityValueDetailStageItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/PopupUnityValueDetail/StageItem")]
public class PopupUnityValueDetailStageItem : MonoBehaviour
{
  [SerializeField]
  private UILabel eventQuestTitle;
  [SerializeField]
  private UILabel eventQuestSubTitle;
  [SerializeField]
  private UILabel storyQuestChapterTitle;
  [SerializeField]
  private UILabel storyQuestStageTitle;
  [SerializeField]
  private UILabel AP;
  [SerializeField]
  private GameObject questNotOpened;
  [SerializeField]
  private GameObject disabledCover;
  [SerializeField]
  private GameObject disabledQuestSelect;
  private QuestConverterData stageData;
  private Action<Action> beforeChangeSceneAction;
  private Action<PopupUtility.SceneTo> onChangeOtherScene;
  private bool isAvailable;

  public void Initialize(
    QuestConverterData stageData,
    Action<Action> beforeChangeSceneAction,
    Action<PopupUtility.SceneTo> changeOtherScene,
    bool disableChangeQuest)
  {
    this.stageData = stageData;
    this.beforeChangeSceneAction = beforeChangeSceneAction;
    this.onChangeOtherScene = changeOtherScene;
    if (stageData.type == CommonQuestType.Story)
    {
      QuestStoryL questStoryL = (QuestStoryL) null;
      string str = string.Empty;
      if (MasterData.QuestStoryL.TryGetValue(stageData.id_L, out questStoryL))
        str = questStoryL.name;
      this.storyQuestChapterTitle.text = str;
      this.storyQuestStageTitle.text = stageData.title_S;
    }
    else if (stageData.type == CommonQuestType.Extra)
    {
      this.eventQuestTitle.text = stageData.title_M;
      this.eventQuestSubTitle.text = stageData.title_S;
    }
    this.AP.text = stageData.lost_ap.ToString();
    if (stageData.canPlay && !disableChangeQuest)
    {
      this.isAvailable = true;
      this.questNotOpened.SetActive(false);
      this.disabledQuestSelect.SetActive(false);
      this.disabledCover.SetActive(false);
    }
    else
    {
      this.isAvailable = false;
      if (!stageData.canPlay)
      {
        this.questNotOpened.SetActive(true);
        this.disabledQuestSelect.SetActive(false);
      }
      else
      {
        this.questNotOpened.SetActive(false);
        this.disabledQuestSelect.SetActive(true);
      }
      this.disabledCover.SetActive(true);
    }
  }

  public void Initialize(string title1, string title2, int ap, CommonQuestType type)
  {
    this.isAvailable = false;
    this.questNotOpened.SetActive(true);
    this.disabledQuestSelect.SetActive(false);
    this.disabledCover.SetActive(true);
    switch (type)
    {
      case CommonQuestType.Story:
        this.storyQuestChapterTitle.text = title1;
        this.storyQuestStageTitle.text = title2;
        break;
      case CommonQuestType.Extra:
        this.eventQuestTitle.text = title1;
        this.eventQuestSubTitle.text = title2;
        break;
    }
    this.AP.text = ap.ToString();
  }

  public void OnClick()
  {
    if (!this.isAvailable)
      return;
    StageAvailibilityCheckHelper orAddComponent = ((Component) this).gameObject.GetOrAddComponent<StageAvailibilityCheckHelper>();
    bool storyOnly = QuestStageMenuBase.checkForStoryOnly(this.stageData.type, this.stageData.id_S);
    bool Event = this.stageData.type == CommonQuestType.Extra;
    this.StartCoroutine(orAddComponent.PopupJudge(this.stageData, new Action<Action>(this.OnBeforeSceneChange), new Action<PopupUtility.SceneTo>(this.OnChangeOtherScene), Event, storyOnly));
  }

  private void OnBeforeSceneChange(Action endAsyncWait)
  {
    Singleton<NGSceneManager>.GetInstance().SaveCurrentChangeSceneParam();
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    if (this.beforeChangeSceneAction != null)
    {
      Action<Action> changeSceneAction = this.beforeChangeSceneAction;
      if (changeSceneAction != null)
        changeSceneAction(endAsyncWait);
    }
    else if (endAsyncWait != null)
      endAsyncWait();
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    instance.IsFromPopupStageList = true;
    if (this.stageData == null)
      return;
    instance.QuestType = new CommonQuestType?(this.stageData.type);
  }

  private void OnChangeOtherScene(PopupUtility.SceneTo to)
  {
    Action<PopupUtility.SceneTo> changeOtherScene = this.onChangeOtherScene;
    if (changeOtherScene != null)
      changeOtherScene(to);
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
