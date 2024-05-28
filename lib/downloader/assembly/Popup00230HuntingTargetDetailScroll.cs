// Decompiled with JetBrains decompiler
// Type: Popup00230HuntingTargetDetailScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Popup00230HuntingTargetDetailScroll : MonoBehaviour
{
  [SerializeField]
  private GameObject[] txtTargetQuestTypes;
  [SerializeField]
  private UILabel txtTargeQuest;
  [SerializeField]
  private GameObject slcQuestNotOpen;
  [SerializeField]
  private GameObject ibtnToQuest;
  private EnemyDetailInfoQuest_infos questInfo;

  private string GetQuestStageName(EnemyDetailInfoQuest_infos info)
  {
    this.questInfo = info;
    string questStageName = string.Empty;
    CommonQuestType commonQuestType = CommonQuestType.Earth;
    if (info.quest_type_id.HasValue)
      commonQuestType = (CommonQuestType) info.quest_type_id.Value;
    if (info.quest_s_id.HasValue && info.quest_m_id.HasValue)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      if (commonQuestType == CommonQuestType.Story)
        str1 = MasterData.QuestStoryM[info.quest_m_id.Value].name;
      else if (commonQuestType == CommonQuestType.Extra)
        str1 = MasterData.QuestExtraM[info.quest_m_id.Value].name;
      else if (commonQuestType == CommonQuestType.Character)
        str1 = MasterData.QuestCharacterM[info.quest_m_id.Value].name;
      else if (commonQuestType == CommonQuestType.Harmony)
        str1 = MasterData.QuestHarmonyM[info.quest_m_id.Value].name;
      if (commonQuestType == CommonQuestType.Story)
        str2 = MasterData.QuestStoryS[info.quest_s_id.Value].name;
      else if (commonQuestType == CommonQuestType.Extra)
        str2 = MasterData.QuestExtraS[info.quest_s_id.Value].name;
      else if (commonQuestType == CommonQuestType.Character)
        str2 = MasterData.QuestCharacterS[info.quest_s_id.Value].name;
      else if (commonQuestType == CommonQuestType.Harmony)
        str2 = MasterData.QuestHarmonyS[info.quest_s_id.Value].name;
      questStageName = str1 + str2;
    }
    else if (info.quest_m_id.HasValue)
    {
      switch (commonQuestType)
      {
        case CommonQuestType.Story:
          questStageName = MasterData.QuestStoryM[info.quest_s_id.Value].name;
          break;
        case CommonQuestType.Character:
          questStageName = MasterData.QuestCharacterM[info.quest_s_id.Value].name;
          break;
        case CommonQuestType.Extra:
          questStageName = MasterData.QuestExtraM[info.quest_s_id.Value].name;
          break;
        case CommonQuestType.Harmony:
          questStageName = MasterData.QuestHarmonyM[info.quest_s_id.Value].name;
          break;
      }
    }
    else if (info.quest_s_id.HasValue)
    {
      switch (commonQuestType)
      {
        case CommonQuestType.Story:
          questStageName = MasterData.QuestStoryS[info.quest_s_id.Value].name;
          break;
        case CommonQuestType.Character:
          questStageName = MasterData.QuestCharacterS[info.quest_s_id.Value].name;
          break;
        case CommonQuestType.Extra:
          questStageName = MasterData.QuestExtraS[info.quest_s_id.Value].name;
          break;
        case CommonQuestType.Harmony:
          questStageName = MasterData.QuestHarmonyS[info.quest_s_id.Value].name;
          break;
      }
    }
    else
      questStageName = string.Empty;
    return questStageName;
  }

  public void Init(EnemyDetailInfoQuest_infos info)
  {
    if (info.is_play)
    {
      this.slcQuestNotOpen.SetActive(false);
      this.ibtnToQuest.SetActive(true);
    }
    else
    {
      this.slcQuestNotOpen.SetActive(true);
      this.ibtnToQuest.SetActive(false);
    }
    if (info.quest_type_id.HasValue)
    {
      switch (info.quest_type_id.Value)
      {
        case 1:
          ((IEnumerable<GameObject>) this.txtTargetQuestTypes).ToggleOnce(0);
          break;
        case 2:
        case 4:
          ((IEnumerable<GameObject>) this.txtTargetQuestTypes).ToggleOnce(2);
          break;
        case 3:
          ((IEnumerable<GameObject>) this.txtTargetQuestTypes).ToggleOnce(1);
          break;
      }
    }
    else
      ((IEnumerable<GameObject>) this.txtTargetQuestTypes).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    this.txtTargeQuest.SetTextLocalize(this.GetQuestStageName(info));
  }

  public void OnQUestButton()
  {
    if (this.questInfo.quest_type_id.HasValue)
    {
      switch (this.questInfo.quest_type_id.Value)
      {
        case 1:
          QuestStoryS questStoryS = MasterData.QuestStoryS[this.questInfo.quest_s_id.Value];
          if (this.questInfo.quest_s_id.HasValue)
          {
            Quest0022Scene.ChangeScene0022(false, questStoryS.quest_l.ID, questStoryS.quest_m.ID, questStoryS.ID);
            break;
          }
          Quest0022Scene.ChangeScene0022(false, questStoryS.quest_l.ID, questStoryS.quest_m.ID);
          break;
        case 3:
          if (this.questInfo.quest_s_id.HasValue)
          {
            Quest00220Scene.ChangeScene00220(this.questInfo.quest_s_id.Value, isForces: true);
            break;
          }
          if (this.questInfo.quest_m_id.HasValue)
          {
            Quest00219Scene.ChangeScene(this.questInfo.quest_s_id.Value, false);
            break;
          }
          break;
      }
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  private enum TargetType
  {
    TARGET_TYPE_QUEST,
    TARGET_TYPE_EXTRA,
    TARGET_TYPE_OTHER,
  }
}
