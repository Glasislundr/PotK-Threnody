// Decompiled with JetBrains decompiler
// Type: SM_QuestHMExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_QuestHMExtension
{
  public static PlayerHarmonyQuestM[] FilterDisabled(
    this PlayerHarmonyQuestM[] self,
    Dictionary<int, bool> playableQuestIDsMap)
  {
    return ((IEnumerable<PlayerHarmonyQuestM>) self).Select<PlayerHarmonyQuestM, PlayerHarmonyQuestM>((Func<PlayerHarmonyQuestM, PlayerHarmonyQuestM>) (quest =>
    {
      quest.player_quests = ((IEnumerable<PlayerHarmonyQuestS>) quest.player_quests).Where<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (questDetail => !questDetail.quest_harmony_s.start_at.HasValue || questDetail.quest_harmony_s.start_at.Value.CompareTo(ServerTime.NowAppTime()) <= 0)).ToArray<PlayerHarmonyQuestS>();
      if (!playableQuestIDsMap.ContainsKey(quest._quest_m_id))
        quest.is_playable = false;
      return quest;
    })).Where<PlayerHarmonyQuestM>((Func<PlayerHarmonyQuestM, bool>) (quest => quest.player_quests.Length != 0)).ToArray<PlayerHarmonyQuestM>();
  }
}
