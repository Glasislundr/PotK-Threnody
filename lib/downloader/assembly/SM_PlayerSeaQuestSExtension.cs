// Decompiled with JetBrains decompiler
// Type: SM_PlayerSeaQuestSExtension
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
public static class SM_PlayerSeaQuestSExtension
{
  public static PlayerSeaQuestS[] XL(this PlayerSeaQuestS[] self)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.quest_xl.ID == b.quest_sea_s.quest_xl.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_xl.priority)).ToArray<PlayerSeaQuestS>();
  }

  public static PlayerSeaQuestS[] L(this PlayerSeaQuestS[] self, int xl)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (q => q.quest_sea_s.quest_xl.ID == xl)).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.quest_l.ID == b.quest_sea_s.quest_l.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_l.priority)).ToArray<PlayerSeaQuestS>();
  }

  public static PlayerSeaQuestS[] M(this PlayerSeaQuestS[] self, int xl, int l)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (q => q.quest_sea_s.quest_xl.ID == xl && q.quest_sea_s.quest_l.ID == l)).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.quest_m.ID == b.quest_sea_s.quest_m.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_m.priority)).ToArray<PlayerSeaQuestS>();
  }

  public static PlayerSeaQuestS[] S(this PlayerSeaQuestS[] self, int xl, int l, int m)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (q => q.quest_sea_s.quest_xl.ID == xl && q.quest_sea_s.quest_l.ID == l && q.quest_sea_s.quest_m.ID == m)).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.ID == b.quest_sea_s.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.priority)).ToArray<PlayerSeaQuestS>();
  }

  public static StoryPlaybackSea[] Stories(this PlayerSeaQuestS[] self)
  {
    Dictionary<int, StoryPlaybackSea> dic = ((IEnumerable<StoryPlaybackSea>) MasterData.StoryPlaybackSeaList).ToDictionary<StoryPlaybackSea, int>((Func<StoryPlaybackSea, int>) (x => x.quest.ID));
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.is_clear && dic.ContainsKey(x.quest_sea_s.ID))).Select<PlayerSeaQuestS, StoryPlaybackSea>((Func<PlayerSeaQuestS, StoryPlaybackSea>) (x => dic[x.quest_sea_s.ID])).OrderByDescending<StoryPlaybackSea, int>((Func<StoryPlaybackSea, int>) (x => x.priority)).ToArray<StoryPlaybackSea>();
  }

  public static PlayerSeaQuestS[] DisplayScrollXL(this PlayerSeaQuestS[] self)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (q => q.is_clear)).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.quest_xl.ID == b.quest_sea_s.quest_xl.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_xl.priority)).ToArray<PlayerSeaQuestS>();
  }

  public static PlayerSeaQuestS[] DisplayScrollL(this PlayerSeaQuestS[] self, int xl)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (q => q.is_clear && q.quest_sea_s.quest_xl.ID == xl && !q.quest_sea_s.quest_l.origin_id.HasValue)).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.quest_l.ID == b.quest_sea_s.quest_l.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_l.priority)).ToArray<PlayerSeaQuestS>();
  }

  public static PlayerSeaQuestS[] DisplayScrollM(this PlayerSeaQuestS[] self, int xl)
  {
    return ((IEnumerable<PlayerSeaQuestS>) self).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (q => q.is_clear && q.quest_sea_s.quest_xl.ID == xl)).Distinct<PlayerSeaQuestS>((IEqualityComparer<PlayerSeaQuestS>) new LambdaEqualityComparer<PlayerSeaQuestS>((Func<PlayerSeaQuestS, PlayerSeaQuestS, bool>) ((a, b) => a.quest_sea_s.quest_m.ID == b.quest_sea_s.quest_m.ID))).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.quest_m.priority)).ToArray<PlayerSeaQuestS>();
  }
}
