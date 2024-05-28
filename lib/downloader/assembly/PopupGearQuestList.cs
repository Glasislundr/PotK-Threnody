// Decompiled with JetBrains decompiler
// Type: PopupGearQuestList
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
[AddComponentMenu("Popup/GearQuest/Menu")]
public class PopupGearQuestList : BackButtonPopupBase
{
  [SerializeField]
  private UIScrollView scroll_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private GameObject objNoList_;
  private PlayerUnit playerUnit_;
  private List<object> quests_;
  private QuestConverterData currentData_;
  private Action<Action> onBeforeChangeScene_;
  private Action<PopupUtility.SceneTo> onChangeOtherScene_;
  private bool isDisabledSelect_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit/Popup_DropQuest").Load<GameObject>();
  }

  public static void open(
    GameObject prefab,
    PlayerUnit playerUnit,
    Action<Action> eventBeforeChangeScene,
    Action<PopupUtility.SceneTo> eventChangeOtherScene,
    bool bDisabledSelect)
  {
    List<object> objectList = new List<object>();
    UnitRecommend unitRecommend;
    if (MasterData.UnitRecommend.TryGetValue(playerUnit.unit.same_character_id, out unitRecommend))
    {
      if (unitRecommend.extra_quests != null && unitRecommend.extra_quests.Length != 0)
        objectList.AddRange((IEnumerable<object>) unitRecommend.extra_quests);
      if (unitRecommend.sea_quests != null && unitRecommend.sea_quests.Length != 0)
        objectList.AddRange((IEnumerable<object>) unitRecommend.sea_quests);
      if (unitRecommend.story_quests != null && unitRecommend.story_quests.Length != 0)
        objectList.AddRange((IEnumerable<object>) unitRecommend.story_quests);
    }
    PopupGearQuestList component = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupGearQuestList>();
    component.playerUnit_ = playerUnit;
    component.quests_ = objectList;
    component.onBeforeChangeScene_ = eventBeforeChangeScene;
    component.onChangeOtherScene_ = eventChangeOtherScene;
    component.isDisabledSelect_ = bDisabledSelect;
  }

  private IEnumerator Start()
  {
    PopupGearQuestList popupGearQuestList = this;
    popupGearQuestList.setTopObject(((Component) popupGearQuestList).gameObject);
    ((Component) popupGearQuestList).GetComponent<UIRect>().alpha = 0.0f;
    bool bError = false;
    IEnumerator e = popupGearQuestList.doInitialize((Action<WebAPI.Response.UserError>) (err =>
    {
      WebAPI.DefaultUserErrorCallback(err);
      MypageScene.ChangeSceneOnError();
      bError = true;
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (bError)
      Singleton<PopupManager>.GetInstance().closeAll();
    else
      Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupGearQuestList).gameObject);
  }

  private IEnumerator doInitialize(Action<WebAPI.Response.UserError> callbackError)
  {
    PopupGearQuestList popupGearQuestList = this;
    if (popupGearQuestList.quests_.Any<object>())
    {
      popupGearQuestList.objNoList_.SetActive(false);
      Future<GameObject> ldItem = GearQuestScrollItem.createLoader();
      IEnumerator e = ldItem.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      int sameCharacterId = popupGearQuestList.playerUnit_.unit.same_character_id;
      string responsedKey = string.Format("UnitAcquireGearQuest_{0}", (object) sameCharacterId);
      if (!WebAPI.IsResponsedAtRecent(responsedKey))
      {
        CommonRoot cRoot = Singleton<CommonRoot>.GetInstance();
        cRoot.ShowLoadingLayer(1, true);
        Future<WebAPI.Response.UnitAcquireGearQuest> wapi = WebAPI.UnitAcquireGearQuest(sameCharacterId, callbackError);
        e = wapi.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        cRoot.HideLoadingLayer();
        if (wapi.Result == null)
        {
          yield break;
        }
        else
        {
          WebAPI.SetLatestResponsedAt(responsedKey);
          cRoot = (CommonRoot) null;
          wapi = (Future<WebAPI.Response.UnitAcquireGearQuest>) null;
        }
      }
      foreach (object quest in popupGearQuestList.quests_)
        popupGearQuestList.initializeItem(ldItem.Result, quest);
      // ISSUE: method pointer
      popupGearQuestList.grid_.onReposition = new UIGrid.OnReposition((object) popupGearQuestList, __methodptr(\u003CdoInitialize\u003Eb__12_0));
      popupGearQuestList.grid_.repositionNow = true;
    }
    else
    {
      popupGearQuestList.objNoList_.SetActive(true);
      ((Behaviour) popupGearQuestList.scroll_).enabled = false;
    }
  }

  private void initializeItem(GameObject prefab, object quest)
  {
    GearQuestScrollItem component = prefab.Clone(((Component) this.grid_).transform).GetComponent<GearQuestScrollItem>();
    switch (quest)
    {
      case QuestStoryS questS1:
        component.initialize(questS1);
        break;
      case QuestSeaS questS2:
        component.initialize(questS2);
        break;
      case QuestExtraS questS3:
        component.initialize(questS3);
        break;
    }
    if (this.onBeforeChangeScene_ != null)
      component.setButton(new Action<QuestConverterData>(this.changeScene), this.isDisabledSelect_);
    else
      component.setButton((Action<QuestConverterData>) null, this.isDisabledSelect_);
  }

  private void changeScene(QuestConverterData questData)
  {
    if (this.IsPush)
      return;
    this.currentData_ = questData;
    StageAvailibilityCheckHelper orAddComponent = ((Component) this).gameObject.GetOrAddComponent<StageAvailibilityCheckHelper>();
    bool storyOnly = QuestStageMenuBase.checkForStoryOnly(questData.type, questData.id_S);
    bool Event = questData.type == CommonQuestType.Extra;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(orAddComponent.PopupJudge(questData, new Action<Action>(this.onBeforeSceneChange), new Action<PopupUtility.SceneTo>(this.onChangeOtherScene), Event, storyOnly));
  }

  private void onBeforeSceneChange(Action endAsyncWait)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.onBeforeChangeScene_(endAsyncWait);
  }

  private void onChangeOtherScene(PopupUtility.SceneTo to)
  {
    Action<PopupUtility.SceneTo> changeOtherScene = this.onChangeOtherScene_;
    if (changeOtherScene != null)
      changeOtherScene(to);
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
