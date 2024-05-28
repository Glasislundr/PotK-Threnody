// Decompiled with JetBrains decompiler
// Type: PopupUnityValueDetail
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
[AddComponentMenu("Popup/PopupUnityValueDetail/Main")]
public class PopupUnityValueDetail : BackButtonPopupBase
{
  [SerializeField]
  private GameObject noAvailableQuest;
  [SerializeField]
  [Tooltip("合算値")]
  private UILabel txtTotal_;
  [SerializeField]
  [Tooltip("純淘汰値")]
  private UILabel txtValue_;
  [SerializeField]
  [Tooltip("強化淘汰値(整数)")]
  private UILabel txtBuildup_;
  [SerializeField]
  [Tooltip("強化淘汰値(小数)")]
  private UILabel txtBuildupDec_;
  [SerializeField]
  private NGTweenGaugeScale gaugeBuildupDec_;
  [SerializeField]
  private UIScrollView stageListScrollView;
  [SerializeField]
  private UIGrid stageListGrid;
  [SerializeField]
  private GameObject specialityButton_;
  private bool isSoloLimited_;
  private PlayerUnit playerUnit_;
  private Dictionary<int, bool> dicSkipSortie_;

  public static Future<GameObject>[] createLoaders(bool isSea)
  {
    Future<GameObject>[] loaders = new Future<GameObject>[2];
    string str = isSea ? "_sea" : "";
    loaders[0] = new ResourceObject("Prefabs/unit/Popup_UnitTouta_Detail" + str).Load<GameObject>();
    loaders[1] = new ResourceObject("Prefabs/unit/dir_AvailableQuest_Menu" + str).Load<GameObject>();
    return loaders;
  }

  public static void show(
    GameObject prefab,
    GameObject stageItemPrefab,
    float unityValue,
    float buildupUnity,
    PlayerUnit playerUnit,
    Action<Action> beforeChangeSceneAction,
    Action<PopupUtility.SceneTo> changedOtherScene,
    bool disableChangeQuest)
  {
    PopupUnityValueDetail component = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupUnityValueDetail>();
    component.isSoloLimited_ = false;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(component.doInitialize(stageItemPrefab, unityValue, buildupUnity, playerUnit, beforeChangeSceneAction, changedOtherScene, disableChangeQuest));
  }

  public static void showSoloQuests(
    GameObject prefab,
    GameObject stageItemPrefab,
    float unityValue,
    float buildupUnity,
    PlayerUnit playerUnit,
    Action<Action> beforeChangeSceneAction,
    Action<PopupUtility.SceneTo> changedOtherScene)
  {
    PopupUnityValueDetail component = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupUnityValueDetail>();
    component.isSoloLimited_ = true;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(component.doInitialize(stageItemPrefab, unityValue, buildupUnity, playerUnit, beforeChangeSceneAction, changedOtherScene, false));
  }

  private IEnumerator doInitialize(
    GameObject stageItemPrefab,
    float unityValue,
    float buildupUnity,
    PlayerUnit playerUnit,
    Action<Action> beforeChangeSceneAction,
    Action<PopupUtility.SceneTo> changedOtherScene,
    bool disableChangeQuest)
  {
    PopupUnityValueDetail unityValueDetail = this;
    ((Component) unityValueDetail).gameObject.GetComponent<UIRect>().alpha = 0.0f;
    unityValueDetail.playerUnit_ = playerUnit;
    UnitUnit baseUnit = unityValueDetail.playerUnit_.unit;
    unityValueDetail.setTopObject(((Component) unityValueDetail).gameObject);
    float number = Mathf.Min(unityValue + buildupUnity, (float) PlayerUnit.GetUnityValueMax());
    unityValueDetail.txtTotal_.SetTextLocalize(number.GetInteger());
    unityValueDetail.txtValue_.SetTextLocalize(unityValue.ToString());
    int integer = buildupUnity.GetInteger();
    int decimalAsPercent = (double) number >= (double) PlayerUnit.GetUnityValueMax() ? 0 : buildupUnity.GetDecimalAsPercent();
    unityValueDetail.txtBuildup_.SetTextLocalize(integer);
    unityValueDetail.txtBuildupDec_.SetTextLocalize(decimalAsPercent);
    unityValueDetail.gaugeBuildupDec_.setValue(decimalAsPercent, 99, false);
    UnitUnit[] array1 = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.is_unity_value_up && x.FindValueUpPattern(baseUnit, (Func<UnitFamily[]>) (() => baseUnit.Families)) != null)).ToArray<UnitUnit>();
    unityValueDetail.noAvailableQuest.SetActive(false);
    unityValueDetail.dicSkipSortie_ = new Dictionary<int, bool>();
    if (array1.Length != 0)
    {
      HashSet<int> source = new HashSet<int>();
      for (int index = 0; index < array1.Length; ++index)
      {
        int currenMaterialUnitID = array1[index].ID;
        foreach (IGrouping<CommonQuestType, UnityValueUpItemQuest> grouping in ((IEnumerable<UnityValueUpItemQuest>) MasterData.UnityValueUpItemQuestList).Where<UnityValueUpItemQuest>((Func<UnityValueUpItemQuest, bool>) (x =>
        {
          if (x.material_unit_id_UnitUnit != currenMaterialUnitID)
            return false;
          return x.quest_type == CommonQuestType.Story || x.quest_type == CommonQuestType.Extra;
        })).GroupBy<UnityValueUpItemQuest, CommonQuestType>((Func<UnityValueUpItemQuest, CommonQuestType>) (y => y.quest_type)))
        {
          switch (grouping.Key)
          {
            case CommonQuestType.Story:
              if (!unityValueDetail.isSoloLimited_)
              {
                using (IEnumerator<UnityValueUpItemQuest> enumerator = grouping.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    UnityValueUpItemQuest current = enumerator.Current;
                    foreach (int questSiD in unityValueDetail.parseQuestSIDs(current.quest_sids))
                      source.Add(questSiD);
                  }
                  continue;
                }
              }
              else
                continue;
            case CommonQuestType.Extra:
              using (IEnumerator<UnityValueUpItemQuest> enumerator = grouping.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  UnityValueUpItemQuest current = enumerator.Current;
                  foreach (int questSiD in unityValueDetail.parseQuestSIDs(current.quest_sids))
                    unityValueDetail.dicSkipSortie_[questSiD] = current.is_skip_sortie;
                }
                continue;
              }
            default:
              continue;
          }
        }
      }
      int[] storySIDsList = source.ToArray<int>();
      int[] eventSIDsList = unityValueDetail.isSoloLimited_ ? unityValueDetail.dicSkipSortie_.Where<KeyValuePair<int, bool>>((Func<KeyValuePair<int, bool>, bool>) (x => x.Value)).Select<KeyValuePair<int, bool>, int>((Func<KeyValuePair<int, bool>, int>) (y => y.Key)).ToArray<int>() : unityValueDetail.dicSkipSortie_.Select<KeyValuePair<int, bool>, int>((Func<KeyValuePair<int, bool>, int>) (x => x.Key)).ToArray<int>();
      int? remainBattleCount;
      if (eventSIDsList.Length != 0)
      {
        if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra") && !WebAPI.IsResponsedAtRecent("QuestProgressLimited"))
        {
          Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
          Future<WebAPI.Response.QuestProgressLimited> extra = WebAPI.QuestProgressLimited((Action<WebAPI.Response.UserError>) (e =>
          {
            WebAPI.DefaultUserErrorCallback(e);
            MypageScene.ChangeSceneOnError();
          }));
          IEnumerator e1 = extra.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
          if (extra.Result == null)
          {
            yield break;
          }
          else
          {
            WebAPI.SetLatestResponsedAt("QuestProgressLimited");
            extra = (Future<WebAPI.Response.QuestProgressLimited>) null;
          }
        }
        PlayerExtraQuestS[] array2 = SMManager.Get<PlayerExtraQuestS[]>();
        List<PlayerExtraQuestS> playerExtraQuestSList = new List<PlayerExtraQuestS>();
        List<int> intList = new List<int>();
        for (int index = 0; index < eventSIDsList.Length; ++index)
        {
          int currentID = eventSIDsList[index];
          PlayerExtraQuestS playerExtraQuestS = Array.Find<PlayerExtraQuestS>(array2, (Predicate<PlayerExtraQuestS>) (x => x._quest_extra_s == currentID));
          if (playerExtraQuestS != null)
          {
            remainBattleCount = playerExtraQuestS.remain_battle_count;
            int num = 0;
            if (!(remainBattleCount.GetValueOrDefault() <= num & remainBattleCount.HasValue))
            {
              playerExtraQuestSList.Add(playerExtraQuestS);
              continue;
            }
          }
          intList.Add(currentID);
        }
        List<QuestConverterData> questConverterDataList = QuestStageMenuBase.Convert(playerExtraQuestSList.ToArray(), new bool?());
        for (int index = 0; index < questConverterDataList.Count; ++index)
        {
          QuestConverterData stageData = questConverterDataList[index];
          if (unityValueDetail.playerUnit_.id != 0)
          {
            stageData.is_skip_sortie = unityValueDetail.dicSkipSortie_[stageData.id_S];
            stageData.player_unit_id = unityValueDetail.playerUnit_.id;
          }
          stageItemPrefab.Clone(((Component) unityValueDetail.stageListGrid).transform).GetComponent<PopupUnityValueDetailStageItem>().Initialize(stageData, beforeChangeSceneAction, changedOtherScene, disableChangeQuest);
        }
        for (int index = 0; index < intList.Count; ++index)
        {
          int currentID = intList[index];
          QuestExtraS questExtraS = ((IEnumerable<QuestExtraS>) MasterData.QuestExtraSList).FirstOrDefault<QuestExtraS>((Func<QuestExtraS, bool>) (x => x.ID == currentID));
          stageItemPrefab.Clone(((Component) unityValueDetail.stageListGrid).transform).GetComponent<PopupUnityValueDetailStageItem>().Initialize(questExtraS.quest_m.name, questExtraS.name, questExtraS.lost_ap, CommonQuestType.Extra);
        }
      }
      if (storySIDsList.Length != 0)
      {
        PlayerStoryQuestS[] array3 = SMManager.Get<PlayerStoryQuestS[]>();
        List<PlayerStoryQuestS> playerStoryQuestSList = new List<PlayerStoryQuestS>();
        List<int> intList = new List<int>();
        for (int index = 0; index < storySIDsList.Length; ++index)
        {
          int currentID = storySIDsList[index];
          PlayerStoryQuestS playerStoryQuestS = Array.Find<PlayerStoryQuestS>(array3, (Predicate<PlayerStoryQuestS>) (x => x._quest_story_s == currentID));
          if (playerStoryQuestS != null)
          {
            remainBattleCount = playerStoryQuestS.remain_battle_count;
            int num = 0;
            if (!(remainBattleCount.GetValueOrDefault() <= num & remainBattleCount.HasValue))
            {
              playerStoryQuestSList.Add(playerStoryQuestS);
              continue;
            }
          }
          intList.Add(currentID);
        }
        List<QuestConverterData> questConverterDataList = QuestStageMenuBase.Convert(playerStoryQuestSList.ToArray());
        for (int index = 0; index < questConverterDataList.Count; ++index)
          stageItemPrefab.Clone(((Component) unityValueDetail.stageListGrid).transform).GetComponent<PopupUnityValueDetailStageItem>().Initialize(questConverterDataList[index], beforeChangeSceneAction, changedOtherScene, disableChangeQuest);
        for (int index = 0; index < intList.Count; ++index)
        {
          int currentID = intList[index];
          QuestStoryS questStoryS = ((IEnumerable<QuestStoryS>) MasterData.QuestStorySList).FirstOrDefault<QuestStoryS>((Func<QuestStoryS, bool>) (x => x.ID == currentID));
          PopupUnityValueDetailStageItem component = stageItemPrefab.Clone(((Component) unityValueDetail.stageListGrid).transform).GetComponent<PopupUnityValueDetailStageItem>();
          QuestStoryL questStoryL = (QuestStoryL) null;
          string str = string.Empty;
          if (MasterData.QuestStoryL.TryGetValue(questStoryS.quest_l_QuestStoryL, out questStoryL))
            str = questStoryL.name;
          string title1 = str;
          string name = questStoryS.name;
          int lostAp = questStoryS.lost_ap;
          component.Initialize(title1, name, lostAp, CommonQuestType.Story);
        }
      }
      if (storySIDsList.Length != 0 || eventSIDsList.Length != 0)
      {
        unityValueDetail.noAvailableQuest.SetActive(false);
        ((UIRect) ((Component) ((Component) unityValueDetail.stageListScrollView).transform.parent).GetComponent<UIWidget>()).alpha = 1f / 1000f;
        unityValueDetail.stageListGrid.Reposition();
        yield return (object) new WaitForSeconds(0.05f);
        unityValueDetail.stageListScrollView.ResetPosition();
        ((UIRect) ((Component) ((Component) unityValueDetail.stageListScrollView).transform.parent).GetComponent<UIWidget>()).alpha = 1f;
      }
      else
        unityValueDetail.noAvailableQuest.SetActive(true);
      storySIDsList = (int[]) null;
      eventSIDsList = (int[]) null;
    }
    else
      unityValueDetail.noAvailableQuest.SetActive(true);
    unityValueDetail.specialityButton_.SetActive(unityValueDetail.playerUnit_.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting > 0);
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) unityValueDetail).gameObject);
  }

  private IEnumerable<int> parseQuestSIDs(string sids)
  {
    return ((IEnumerable<string>) sids.Split(':')).Select<string, int>((Func<string, int>) (x => int.Parse(x.Replace(".0", ""))));
  }

  public void onSpecialityButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowSpecialityPopup());
  }

  private IEnumerator ShowSpecialityPopup()
  {
    PopupUnityValueDetail unityValueDetail = this;
    string path = Singleton<NGGameDataManager>.GetInstance().IsSea ? "Prefabs/popup/popup_004_specialty_details_sea" : "Prefabs/popup/popup_004_specialty_details";
    Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(f.Result).GetComponent<Popup004SpecialtyDetails>().Init(unityValueDetail.playerUnit_.unit.compose_max_unity_value_setting_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unityValueDetail.IsPush = false;
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
