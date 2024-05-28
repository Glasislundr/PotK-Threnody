// Decompiled with JetBrains decompiler
// Type: battle01718aMissionObj
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
public class battle01718aMissionObj : BattleBackButtonMonoBehaiviour
{
  [SerializeField]
  private battle01718aMissionList ActiveMissionObj;
  private GameObject missionListBattleItemPrefab;

  protected override IEnumerator Start_Battle()
  {
    battle01718aMissionObj battle01718aMissionObj = this;
    IEnumerator e = battle01718aMissionObj.init(battle01718aMissionObj.env.core.battleInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitStoryMission(PlayerStoryQuestS story)
  {
    battle01718aMissionObj battle01718aMissionObj = this;
    UIWidget alp = ((Component) battle01718aMissionObj).GetComponent<UIWidget>();
    ((UIRect) alp).alpha = 0.0f;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 1)).ToArray<PlayerMissionHistory>();
    QuestStoryMission[] array2 = ((IEnumerable<QuestStoryMission>) MasterData.QuestStoryMissionList).Where<QuestStoryMission>((Func<QuestStoryMission, bool>) (x => x.quest_s_QuestStoryS == story.quest_story_s.ID)).OrderBy<QuestStoryMission, int>((Func<QuestStoryMission, int>) (x => x.priority)).ToArray<QuestStoryMission>();
    if (Object.op_Inequality((Object) battle01718aMissionObj.ActiveMissionObj, (Object) null))
    {
      IEnumerator e = battle01718aMissionObj.ActiveMissionObj.InitValue(array1, array2, battle01718aMissionObj.missionListBattleItemPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIRect) alp).alpha = 1f;
    }
    else
      battle01718aMissionObj.Back();
  }

  private IEnumerator InitExtraMission(PlayerExtraQuestS extra)
  {
    battle01718aMissionObj battle01718aMissionObj = this;
    UIWidget alp = ((Component) battle01718aMissionObj).GetComponent<UIWidget>();
    ((UIRect) alp).alpha = 0.0f;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 3)).ToArray<PlayerMissionHistory>();
    QuestExtraMission[] array2 = ((IEnumerable<QuestExtraMission>) MasterData.QuestExtraMissionList).Where<QuestExtraMission>((Func<QuestExtraMission, bool>) (x => x.quest_s_QuestExtraS == extra.quest_extra_s.ID)).OrderBy<QuestExtraMission, int>((Func<QuestExtraMission, int>) (x => x.priority)).ToArray<QuestExtraMission>();
    if (Object.op_Inequality((Object) battle01718aMissionObj.ActiveMissionObj, (Object) null))
    {
      IEnumerator e = battle01718aMissionObj.ActiveMissionObj.InitValue(array1, array2, battle01718aMissionObj.missionListBattleItemPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIRect) alp).alpha = 1f;
    }
    else
      battle01718aMissionObj.Back();
  }

  private IEnumerator InitSeaMission(PlayerSeaQuestS story)
  {
    battle01718aMissionObj battle01718aMissionObj = this;
    UIWidget alp = ((Component) battle01718aMissionObj).GetComponent<UIWidget>();
    ((UIRect) alp).alpha = 0.0f;
    PlayerMissionHistory[] array1 = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 9)).ToArray<PlayerMissionHistory>();
    QuestSeaMission[] array2 = ((IEnumerable<QuestSeaMission>) MasterData.QuestSeaMissionList).Where<QuestSeaMission>((Func<QuestSeaMission, bool>) (x => x.quest_s_QuestSeaS == story.quest_sea_s.ID)).OrderBy<QuestSeaMission, int>((Func<QuestSeaMission, int>) (x => x.priority)).ToArray<QuestSeaMission>();
    if (Object.op_Inequality((Object) battle01718aMissionObj.ActiveMissionObj, (Object) null))
    {
      IEnumerator e = battle01718aMissionObj.ActiveMissionObj.InitValue(array1, array2, battle01718aMissionObj.missionListBattleItemPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIRect) alp).alpha = 1f;
    }
    else
      battle01718aMissionObj.Back();
  }

  public IEnumerator init(BattleInfo info)
  {
    battle01718aMissionObj battle01718aMissionObj = this;
    Future<GameObject> prefab = !battle01718aMissionObj.battleManager.isSea ? new ResourceObject("Prefabs/quest002_2/dir_Misson_List_Battle_Item").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2_sea/dir_Misson_List_Battle_Item_sea").Load<GameObject>();
    IEnumerator future = prefab.Wait();
    while (future.MoveNext())
      yield return future.Current;
    future = (IEnumerator) null;
    battle01718aMissionObj.missionListBattleItemPrefab = prefab.Result;
    IEnumerator e = info.isExtra ? battle01718aMissionObj.InitExtraMission(info.extraQuest) : (info.isSea ? battle01718aMissionObj.InitSeaMission(info.seaQuest) : battle01718aMissionObj.InitStoryMission(info.storyQuest));
    while (e.MoveNext())
      yield return e.Current;
  }

  public void Back() => this.battleManager.popupDismiss();

  public override void onBackButton() => this.Back();
}
