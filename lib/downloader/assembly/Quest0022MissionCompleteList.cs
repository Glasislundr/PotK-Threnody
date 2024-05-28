// Decompiled with JetBrains decompiler
// Type: Quest0022MissionCompleteList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0022MissionCompleteList : Quest0022MissionList
{
  private GameObject prefabEffectComp;
  private string rewardMessage;

  public IEnumerator SetParameter(
    QuestStoryMission mission,
    string rewardTitle,
    bool clearFlag,
    bool battleFlag)
  {
    yield return (object) this.SetParameter(mission.name, rewardTitle, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, battleFlag, CommonQuestType.Story);
  }

  public IEnumerator SetParameter(
    QuestExtraMission mission,
    string rewardTitle,
    bool clearFlag,
    bool battleFlag)
  {
    yield return (object) this.SetParameter(mission.name, rewardTitle, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, battleFlag, CommonQuestType.Extra);
  }

  public IEnumerator SetParameter(
    QuestSeaMission mission,
    string rewardTitle,
    bool clearFlag,
    bool battleFlag)
  {
    yield return (object) this.SetParameter(mission.name, rewardTitle, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, battleFlag, CommonQuestType.Sea);
  }

  private IEnumerator SetParameter(
    string missionName,
    string rewardTitle,
    bool clearFlag,
    MasterDataTable.CommonRewardType rewardType,
    int rewardId,
    int quantity,
    bool battleFlag,
    CommonQuestType questType)
  {
    Quest0022MissionCompleteList missionCompleteList = this;
    yield return (object) missionCompleteList.SetValue(missionName, clearFlag, rewardType, rewardId, quantity, battleFlag, new CommonQuestType?(questType));
    if (missionCompleteList.IsClear)
    {
      Future<GameObject> res = Res.Prefabs.battle.QuestMission_Complete.Load<GameObject>();
      IEnumerator e = res.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      missionCompleteList.prefabEffectComp = res.Result;
      missionCompleteList.rewardMessage = rewardTitle ?? "";
      res = (Future<GameObject>) null;
    }
  }

  public IEnumerator CompletedPopup()
  {
    Quest0022MissionCompleteList missionCompleteList = this;
    if (!Object.op_Equality((Object) missionCompleteList.prefabEffectComp, (Object) null))
    {
      GameObject go = Singleton<PopupManager>.GetInstance().open(missionCompleteList.prefabEffectComp);
      IEnumerator e = go.GetComponent<BattleUI05ResultMissionComplete>().SetClearBonus(new Hashtable[1]
      {
        new Hashtable()
        {
          {
            (object) "reward_id",
            (object) missionCompleteList.RewardId
          },
          {
            (object) "reward_type",
            (object) missionCompleteList.RewardType
          },
          {
            (object) "reward_message",
            (object) missionCompleteList.rewardMessage
          }
        }
      });
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<NGSoundManager>.GetInstance().playSE("SE_0535");
      while (go.activeInHierarchy)
        yield return (object) null;
    }
  }
}
