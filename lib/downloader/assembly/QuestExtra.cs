// Decompiled with JetBrains decompiler
// Type: QuestExtra
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public class QuestExtra
{
  public static QuestExtra.SeekType toSeekType(string str_seek)
  {
    switch (str_seek)
    {
      case "m":
      case "M":
        return QuestExtra.SeekType.M;
      case "l":
      case "L":
        return QuestExtra.SeekType.L;
      default:
        return QuestExtra.SeekType.None;
    }
  }

  public static QuestExtra.SeekType toSeekType(PlayerExtraQuestS.SeekType seek_type)
  {
    if (seek_type == PlayerExtraQuestS.SeekType.M)
      return QuestExtra.SeekType.M;
    return seek_type == PlayerExtraQuestS.SeekType.L ? QuestExtra.SeekType.L : QuestExtra.SeekType.None;
  }

  public static QuestExtra.SeekType toSeekType(QuestExtraS.SeekType seek_type)
  {
    if (seek_type == QuestExtraS.SeekType.M)
      return QuestExtra.SeekType.M;
    return seek_type == QuestExtraS.SeekType.L ? QuestExtra.SeekType.L : QuestExtra.SeekType.None;
  }

  public static void getStatusLL(
    QuestExtraLL target,
    PlayerExtraQuestS[] quests,
    HashSet<int> emphasis,
    out bool isNew,
    out bool isAllCleared,
    out bool isEmphasis,
    out bool isClearedToday,
    out int entryConditionID,
    out DateTime? lastEnd)
  {
    isNew = true;
    isAllCleared = true;
    isEmphasis = false;
    isClearedToday = true;
    lastEnd = new DateTime?();
    foreach (PlayerExtraQuestS playerExtraQuestS in ((IEnumerable<PlayerExtraQuestS>) quests).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_ll == target)))
    {
      if (!playerExtraQuestS.is_clear)
        isAllCleared = false;
      if (!playerExtraQuestS.is_new)
        isNew = false;
      if (!isEmphasis && emphasis.Contains(playerExtraQuestS._quest_extra_s))
        isEmphasis = true;
      if (playerExtraQuestS.daily_limit > 0)
      {
        int? remainBattleCount = playerExtraQuestS.remain_battle_count;
        if (!(0 < remainBattleCount.GetValueOrDefault() & remainBattleCount.HasValue))
          goto label_11;
      }
      isClearedToday = false;
label_11:
      if (lastEnd.HasValue)
      {
        DateTime todayDayEndAt = playerExtraQuestS.today_day_end_at;
        DateTime? nullable = lastEnd;
        if ((nullable.HasValue ? (todayDayEndAt > nullable.GetValueOrDefault() ? 1 : 0) : 0) == 0)
          continue;
      }
      lastEnd = new DateTime?(playerExtraQuestS.today_day_end_at);
    }
    entryConditionID = 0;
    foreach (ExtraQuestEntryCondition questEntryCondition in ((IEnumerable<ExtraQuestEntryCondition>) SMManager.Get<ExtraQuestEntryCondition[]>()).ToArray<ExtraQuestEntryCondition>())
    {
      if (questEntryCondition.banner_category == 4 && questEntryCondition.quest_id == target.ID)
      {
        entryConditionID = questEntryCondition.id;
        break;
      }
    }
  }

  public static void getStatusL(
    int LId,
    PlayerExtraQuestS[] quests,
    HashSet<int> emphasis,
    out bool isNew,
    out bool isAllCleared,
    out bool isEmphasis,
    out bool isClearedToday,
    out int entryConditionID)
  {
    isNew = true;
    isAllCleared = true;
    isEmphasis = false;
    isClearedToday = true;
    foreach (PlayerExtraQuestS playerExtraQuestS in ((IEnumerable<PlayerExtraQuestS>) quests).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s.EqualL(LId))))
    {
      if (!playerExtraQuestS.is_clear)
        isAllCleared = false;
      if (!playerExtraQuestS.is_new)
        isNew = false;
      if (!isEmphasis && emphasis.Contains(playerExtraQuestS._quest_extra_s))
        isEmphasis = true;
      if (playerExtraQuestS.daily_limit > 0)
      {
        int? remainBattleCount = playerExtraQuestS.remain_battle_count;
        if (!(0 < remainBattleCount.GetValueOrDefault() & remainBattleCount.HasValue))
          continue;
      }
      isClearedToday = false;
    }
    entryConditionID = 0;
    foreach (ExtraQuestEntryCondition questEntryCondition in ((IEnumerable<ExtraQuestEntryCondition>) SMManager.Get<ExtraQuestEntryCondition[]>()).ToArray<ExtraQuestEntryCondition>())
    {
      if (questEntryCondition.banner_category == 3 && questEntryCondition.quest_id == LId)
      {
        entryConditionID = questEntryCondition.id;
        break;
      }
    }
  }

  public static void getStatusM(
    int MId,
    PlayerExtraQuestS[] quests,
    HashSet<int> emphasis,
    out bool isNew,
    out bool isAllCleared,
    out bool isEmphasis,
    out bool isClearedToday,
    out bool isSkipSortie,
    out int entryConditionID)
  {
    isNew = true;
    isAllCleared = true;
    isEmphasis = false;
    isClearedToday = true;
    List<int> intList = new List<int>();
    foreach (PlayerExtraQuestS playerExtraQuestS in ((IEnumerable<PlayerExtraQuestS>) quests).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s.quest_m_QuestExtraM == MId)))
    {
      intList.Add(playerExtraQuestS._quest_extra_s);
      if (!playerExtraQuestS.is_clear)
        isAllCleared = false;
      if (!playerExtraQuestS.is_new)
        isNew = false;
      if (!isEmphasis && emphasis.Contains(playerExtraQuestS._quest_extra_s))
        isEmphasis = true;
      if (playerExtraQuestS.daily_limit > 0)
      {
        int? remainBattleCount = playerExtraQuestS.remain_battle_count;
        if (!(0 < remainBattleCount.GetValueOrDefault() & remainBattleCount.HasValue))
          continue;
      }
      isClearedToday = false;
    }
    isSkipSortie = intList.Any<int>() && UnityValueUpItemQuest.makeSkipSortieQuestUnityValueUp((IEnumerable<int>) intList).Count == intList.Count;
    entryConditionID = 0;
    foreach (ExtraQuestEntryCondition questEntryCondition in ((IEnumerable<ExtraQuestEntryCondition>) SMManager.Get<ExtraQuestEntryCondition[]>()).ToArray<ExtraQuestEntryCondition>())
    {
      if (questEntryCondition.banner_category == 2 && questEntryCondition.quest_id == MId)
      {
        entryConditionID = questEntryCondition.id;
        break;
      }
    }
  }

  public enum SeekType
  {
    None,
    M,
    L,
    LL,
  }
}
