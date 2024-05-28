// Decompiled with JetBrains decompiler
// Type: SM_PlayerStoryQuestSExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_PlayerStoryQuestSExtension
{
  public static PlayerStoryQuestS[] XL(this PlayerStoryQuestS[] self)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.quest_xl.ID == b.quest_story_s.quest_xl.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_xl.priority)).ToArray<PlayerStoryQuestS>();
  }

  public static PlayerStoryQuestS[] L(this PlayerStoryQuestS[] self, int xl)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q.quest_story_s.quest_xl.ID == xl)).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.quest_l.ID == b.quest_story_s.quest_l.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l.priority)).ToArray<PlayerStoryQuestS>();
  }

  public static PlayerStoryQuestS[] M(this PlayerStoryQuestS[] self, int xl, int l)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q.quest_story_s.quest_xl.ID == xl && q.quest_story_s.quest_l.ID == l)).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.quest_m.ID == b.quest_story_s.quest_m.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_m.priority)).ToArray<PlayerStoryQuestS>();
  }

  public static PlayerStoryQuestS[] S(this PlayerStoryQuestS[] self, int xl, int l, int m)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q.quest_story_s.quest_xl.ID == xl && q.quest_story_s.quest_l.ID == l && q.quest_story_s.quest_m.ID == m)).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.ID == b.quest_story_s.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.priority)).ToArray<PlayerStoryQuestS>();
  }

  public static StoryPlaybackStory[] Stories(this PlayerStoryQuestS[] self)
  {
    Dictionary<int, StoryPlaybackStory> dic = ((IEnumerable<StoryPlaybackStory>) MasterData.StoryPlaybackStoryList).ToDictionary<StoryPlaybackStory, int>((Func<StoryPlaybackStory, int>) (x => x.quest.ID));
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.is_clear && dic.ContainsKey(x.quest_story_s.ID))).Select<PlayerStoryQuestS, StoryPlaybackStory>((Func<PlayerStoryQuestS, StoryPlaybackStory>) (x => dic[x.quest_story_s.ID])).OrderByDescending<StoryPlaybackStory, int>((Func<StoryPlaybackStory, int>) (x => x.priority)).ToArray<StoryPlaybackStory>();
  }

  public static PlayerStoryQuestS[] DisplayScrollXL(this PlayerStoryQuestS[] self)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q.is_clear)).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.quest_xl.ID == b.quest_story_s.quest_xl.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_xl.priority)).ToArray<PlayerStoryQuestS>();
  }

  public static PlayerStoryQuestS[] DisplayScrollL(this PlayerStoryQuestS[] self, int xl)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q.is_clear && q.quest_story_s.quest_xl.ID == xl && !q.quest_story_s.quest_l.origin_id.HasValue)).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.quest_l.ID == b.quest_story_s.quest_l.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l.priority)).ToArray<PlayerStoryQuestS>();
  }

  public static PlayerStoryQuestS[] DisplayScrollM(this PlayerStoryQuestS[] self, int xl, int l)
  {
    return ((IEnumerable<PlayerStoryQuestS>) self).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q.is_clear && q.quest_story_s.quest_xl.ID == xl && q.quest_story_s.quest_l.ID == l)).Distinct<PlayerStoryQuestS>((IEqualityComparer<PlayerStoryQuestS>) new LambdaEqualityComparer<PlayerStoryQuestS>((Func<PlayerStoryQuestS, PlayerStoryQuestS, bool>) ((a, b) => a.quest_story_s.quest_m.ID == b.quest_story_s.quest_m.ID))).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_m.priority)).ToArray<PlayerStoryQuestS>();
  }
}
