// Decompiled with JetBrains decompiler
// Type: Quest0022MissionComplete
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
public class Quest0022MissionComplete : MonoBehaviour
{
  [SerializeField]
  private UILabel MissionAchievementCount;
  [SerializeField]
  private GameObject MissionAchievementComplete;
  [SerializeField]
  private UILabel MissionAchievementAll;
  [SerializeField]
  private UniqueIconsSetStory MissionCompleteItem;
  private Quest0022MissionComplete.RewardInfo rewardItemData;
  private GameObject itemDetailPopupF;

  public IEnumerator missionCompleteRate(QuestConverterData quest, int M)
  {
    this.rewardItemData = (Quest0022MissionComplete.RewardInfo) null;
    this.itemDetailPopupF = (GameObject) null;
    int num = 0;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 1)).ToArray<PlayerMissionHistory>();
    QuestStoryMission[] array2 = ((IEnumerable<QuestStoryMission>) MasterData.QuestStoryMissionList).Where<QuestStoryMission>((Func<QuestStoryMission, bool>) (x => x.quest_s.quest_m_QuestStoryM == quest.id_M)).ToArray<QuestStoryMission>();
    foreach (QuestStoryMission questStoryMission in array2)
      num += ((IEnumerable<PlayerMissionHistory>) array1).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(questStoryMission.ID) ? 1 : 0;
    int length = array2.Length;
    this.MissionAchievementComplete.SetActive(length == num);
    this.MissionAchievementAll.SetTextLocalize(length);
    this.MissionAchievementCount.SetTextLocalize("[ffff00]" + num.ToString() + "[-]");
    QuestStoryMissionReward[] missionRewardList = MasterData.QuestStoryMissionRewardList;
    QuestStoryMissionReward storyMissionReward = ((IEnumerable<QuestStoryMissionReward>) missionRewardList).FirstOrDefault<QuestStoryMissionReward>((Func<QuestStoryMissionReward, bool>) (x => x.quest_m_QuestStoryM == M));
    if (missionRewardList != null)
    {
      this.rewardItemData = new Quest0022MissionComplete.RewardInfo(storyMissionReward.reward_id, storyMissionReward.reward_type);
      foreach (Component component in ((Component) this.MissionCompleteItem).transform)
        Object.DestroyImmediate((Object) component.gameObject);
      IEnumerator e = ((Component) this.MissionCompleteItem).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail(storyMissionReward.reward_type, storyMissionReward.reward_id, storyMissionReward.quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator missionCompleteRate_sea(QuestConverterData quest, int M)
  {
    this.rewardItemData = (Quest0022MissionComplete.RewardInfo) null;
    this.itemDetailPopupF = (GameObject) null;
    int num = 0;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 9)).ToArray<PlayerMissionHistory>();
    QuestSeaMission[] array2 = ((IEnumerable<QuestSeaMission>) MasterData.QuestSeaMissionList).Where<QuestSeaMission>((Func<QuestSeaMission, bool>) (x => x.quest_s.quest_m_QuestSeaM == quest.id_M)).ToArray<QuestSeaMission>();
    foreach (QuestSeaMission questSeaMission in array2)
      num += ((IEnumerable<PlayerMissionHistory>) array1).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(questSeaMission.ID) ? 1 : 0;
    int length = array2.Length;
    this.MissionAchievementComplete.SetActive(length == num);
    this.MissionAchievementAll.SetTextLocalize(length);
    this.MissionAchievementCount.SetTextLocalize("[ffff00]" + num.ToString() + "[-]");
    QuestSeaMissionReward seaMissionReward = ((IEnumerable<QuestSeaMissionReward>) MasterData.QuestSeaMissionRewardList).FirstOrDefault<QuestSeaMissionReward>((Func<QuestSeaMissionReward, bool>) (x => x.quest_m_QuestSeaM == M));
    if (seaMissionReward == null)
    {
      ((Component) ((Component) this.MissionCompleteItem).transform.parent).gameObject.SetActive(false);
    }
    else
    {
      this.rewardItemData = new Quest0022MissionComplete.RewardInfo(seaMissionReward.reward_id, seaMissionReward.reward_type);
      foreach (Component component in ((Component) this.MissionCompleteItem).transform)
        Object.DestroyImmediate((Object) component.gameObject);
      IEnumerator e = ((Component) this.MissionCompleteItem).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail(seaMissionReward.reward_type, seaMissionReward.reward_id, seaMissionReward.quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onDetail() => this.StartCoroutine(this.setDetailPopup());

  private IEnumerator setDetailPopup()
  {
    if (this.rewardItemData != null)
    {
      IEnumerator e;
      if (Object.op_Equality((Object) this.itemDetailPopupF, (Object) null))
      {
        Future<GameObject> popupF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
        e = popupF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.itemDetailPopupF = popupF.Result;
        popupF = (Future<GameObject>) null;
      }
      e = Singleton<PopupManager>.GetInstance().open(this.itemDetailPopupF).GetComponent<ItemDetailPopupBase>().SetInfo(this.rewardItemData.reward_type, this.rewardItemData.reward_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public class RewardInfo
  {
    public int reward_id;
    public MasterDataTable.CommonRewardType reward_type;

    public RewardInfo(int id, MasterDataTable.CommonRewardType type)
    {
      this.reward_id = id;
      this.reward_type = type;
    }
  }
}
