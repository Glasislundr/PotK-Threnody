// Decompiled with JetBrains decompiler
// Type: SM_PlayerExtraQuestSExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public static class SM_PlayerExtraQuestSExtension
{
  public static PlayerExtraQuestS[] Root(
    this IEnumerable<PlayerExtraQuestS> self,
    bool checkedMaster = false)
  {
    return (checkedMaster ? self : self.CheckMasterData()).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => SM_PlayerExtraQuestSExtension.equal_Root(a, b)))).ToArray<PlayerExtraQuestS>();
  }

  private static bool equal_Root(PlayerExtraQuestS a, PlayerExtraQuestS b)
  {
    QuestExtraLL questLl = a.quest_ll;
    if (questLl != null)
      return questLl == b.quest_ll;
    return b.quest_ll == null && SM_PlayerExtraQuestSExtension.equal_L_M(a, b);
  }

  public static PlayerExtraQuestS[] L(this IEnumerable<PlayerExtraQuestS> self, bool checkedMaster = false)
  {
    return (checkedMaster ? self.Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.seek_type == PlayerExtraQuestS.SeekType.L)) : self.Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.seek_type == PlayerExtraQuestS.SeekType.L)).CheckMasterData()).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => a.quest_extra_s.quest_l_QuestExtraL == b.quest_extra_s.quest_l_QuestExtraL))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.quest_m.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static PlayerExtraQuestS[] M(this IEnumerable<PlayerExtraQuestS> self, bool checkedMaster = false)
  {
    return (checkedMaster ? self.Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.seek_type == PlayerExtraQuestS.SeekType.M)) : self.Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.seek_type == PlayerExtraQuestS.SeekType.M)).CheckMasterData()).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => a.quest_extra_s.quest_m_QuestExtraM == b.quest_extra_s.quest_m_QuestExtraM))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.quest_m.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static PlayerExtraQuestS[] L_M(
    this IEnumerable<PlayerExtraQuestS> self,
    int ll,
    bool checkedMaster = false)
  {
    QuestExtraLL questLl;
    return (checkedMaster ? self : self.CheckMasterData()).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => (questLl = q.quest_extra_s.quest_ll) != null && questLl.ID == ll)).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => SM_PlayerExtraQuestSExtension.equal_L_M(a, b)))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.quest_m.priority)).ToArray<PlayerExtraQuestS>();
  }

  private static bool equal_L_M(PlayerExtraQuestS a, PlayerExtraQuestS b)
  {
    PlayerExtraQuestS.SeekType seekType = a.seek_type;
    if (seekType != b.seek_type)
      return false;
    return seekType != PlayerExtraQuestS.SeekType.L ? a.quest_extra_s.quest_m_QuestExtraM == b.quest_extra_s.quest_m_QuestExtraM : a.quest_extra_s.quest_l_QuestExtraL == b.quest_extra_s.quest_l_QuestExtraL;
  }

  public static PlayerExtraQuestS[] M(
    this IEnumerable<PlayerExtraQuestS> self,
    int l,
    bool checkedMaster = false)
  {
    return (checkedMaster ? self : self.CheckMasterData()).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.seek_type == PlayerExtraQuestS.SeekType.L && q.quest_extra_s.quest_l_QuestExtraL == l)).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => a.quest_extra_s.quest_m_QuestExtraM == b.quest_extra_s.quest_m_QuestExtraM))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.quest_m.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static PlayerExtraQuestS[] S(
    this IEnumerable<PlayerExtraQuestS> self,
    int l,
    int m,
    bool checkedMaster = false)
  {
    return (checkedMaster ? self : self.CheckMasterData()).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.quest_extra_s.quest_l_QuestExtraL == l && q.quest_extra_s.quest_m_QuestExtraM == m)).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => a._quest_extra_s == b._quest_extra_s))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static PlayerExtraQuestS[] L_ClearedStory(this PlayerExtraQuestS[] self)
  {
    Dictionary<int, StoryPlaybackExtra> dic = ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList).ToDictionary<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.ID));
    return ((IEnumerable<PlayerExtraQuestS>) self).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.is_clear && dic.ContainsKey(q._quest_extra_s))).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => a.quest_extra_s.quest_l_QuestExtraL == b.quest_extra_s.quest_l_QuestExtraL))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.quest_m.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static PlayerExtraQuestS[] M_ClearedStory(this PlayerExtraQuestS[] self, int l)
  {
    Dictionary<int, StoryPlaybackExtra> dic = ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList).ToDictionary<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.ID));
    return ((IEnumerable<PlayerExtraQuestS>) self).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.quest_extra_s.quest_l_QuestExtraL == l && q.is_clear && dic.ContainsKey(q._quest_extra_s))).Distinct<PlayerExtraQuestS>((IEqualityComparer<PlayerExtraQuestS>) new LambdaEqualityComparer<PlayerExtraQuestS>((Func<PlayerExtraQuestS, PlayerExtraQuestS, bool>) ((a, b) => a.quest_extra_s.quest_m_QuestExtraM == b.quest_extra_s.quest_m_QuestExtraM))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.quest_m.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static PlayerExtraQuestS[] S_ClearedStory(this PlayerExtraQuestS[] self, int l, int m)
  {
    Dictionary<int, StoryPlaybackExtra> dic = ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList).ToDictionary<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.ID));
    return ((IEnumerable<PlayerExtraQuestS>) self).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (q => q.quest_extra_s.quest_l_QuestExtraL == l && q.quest_extra_s.quest_m_QuestExtraM == m && q.is_clear && dic.ContainsKey(q._quest_extra_s))).OrderBy<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.priority)).ToArray<PlayerExtraQuestS>();
  }

  public static StoryPlaybackStory[] Stories(this PlayerExtraQuestS[] self)
  {
    Dictionary<int, StoryPlaybackStory> dic = ((IEnumerable<StoryPlaybackStory>) MasterData.StoryPlaybackStoryList).ToDictionary<StoryPlaybackStory, int>((Func<StoryPlaybackStory, int>) (x => x.quest.ID));
    return ((IEnumerable<PlayerExtraQuestS>) self).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.is_clear && dic.ContainsKey(x._quest_extra_s))).Select<PlayerExtraQuestS, StoryPlaybackStory>((Func<PlayerExtraQuestS, StoryPlaybackStory>) (x => dic[x._quest_extra_s])).OrderByDescending<StoryPlaybackStory, int>((Func<StoryPlaybackStory, int>) (x => x.priority)).ToArray<StoryPlaybackStory>();
  }

  public static bool HasStory(this PlayerExtraQuestS self)
  {
    return self.StoryDetails().Any<StoryPlaybackStoryDetail>();
  }

  public static List<StoryPlaybackStoryDetail> StoryDetails(this PlayerExtraQuestS self)
  {
    foreach (StoryPlaybackStory storyPlaybackStory in MasterData.StoryPlaybackStoryList)
    {
      StoryPlaybackStory storyPlayBack = storyPlaybackStory;
      if (storyPlayBack.quest.ID == self._quest_extra_s)
        return ((IEnumerable<StoryPlaybackStoryDetail>) MasterData.StoryPlaybackStoryDetailList).Where<StoryPlaybackStoryDetail>((Func<StoryPlaybackStoryDetail, bool>) (x => x.story.quest.ID == storyPlayBack.quest.ID)).ToList<StoryPlaybackStoryDetail>();
    }
    return new List<StoryPlaybackStoryDetail>();
  }

  public static PlayerQuestGate GetPlayerQuestGate(this PlayerExtraQuestS self)
  {
    PlayerQuestGate[] source = SMManager.Get<PlayerQuestGate[]>();
    return source == null ? (PlayerQuestGate) null : ((IEnumerable<PlayerQuestGate>) source).SingleOrDefault<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => ((IEnumerable<int>) x.quest_ids).Any<int>((Func<int, bool>) (y => y == self._quest_extra_s))));
  }

  public static IEnumerable<PlayerExtraQuestS> CheckMasterData(
    this IEnumerable<PlayerExtraQuestS> self)
  {
    return self.Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x =>
    {
      if (x == null)
        return false;
      if (MasterData.QuestExtraS.ContainsKey(x._quest_extra_s))
        return true;
      Debug.LogError((object) ("ExtraQuest ID" + (object) x._quest_extra_s + "がマスターデータから見つかりませんでした。"));
      return false;
    }));
  }
}
