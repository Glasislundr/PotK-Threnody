// Decompiled with JetBrains decompiler
// Type: GameCore.SM_Extension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace GameCore
{
  public static class SM_Extension
  {
    public static Dictionary<int, (QuestScoreCampaignProgress[] data, int patterns)> CreateLLMap(
      this QuestScoreCampaignProgress[] self)
    {
      QuestExtraL questExtraL;
      return ((IEnumerable<QuestScoreCampaignProgress>) self).GroupBy<QuestScoreCampaignProgress, int>((Func<QuestScoreCampaignProgress, int>) (x => x.quest_extra_l.IsValid() && MasterData.QuestExtraL.TryGetValue(x.quest_extra_l, out questExtraL) && questExtraL.quest_ll_QuestExtraLL.IsValid() ? questExtraL.quest_ll_QuestExtraLL.Value : 0)).ToDictionary<IGrouping<int, QuestScoreCampaignProgress>, int, (QuestScoreCampaignProgress[], int)>((Func<IGrouping<int, QuestScoreCampaignProgress>, int>) (a => a.Key), (Func<IGrouping<int, QuestScoreCampaignProgress>, (QuestScoreCampaignProgress[], int)>) (b => (b.ToArray<QuestScoreCampaignProgress>(), b.Distinct<QuestScoreCampaignProgress>((IEqualityComparer<QuestScoreCampaignProgress>) new LambdaEqualityComparer<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, QuestScoreCampaignProgress, bool>) ((v1, v2) => v1.description.exEquals(v2.description)))).Count<QuestScoreCampaignProgress>())));
    }

    public static (int LL, int index, QuestScoreCampaignProgress[] data, int patterns) FindL(
      this Dictionary<int, (QuestScoreCampaignProgress[] data, int patterns)> self,
      int L)
    {
      foreach (KeyValuePair<int, (QuestScoreCampaignProgress[] data, int patterns)> keyValuePair in self)
      {
        int? nullable = ((IEnumerable<QuestScoreCampaignProgress>) keyValuePair.Value.data).FirstIndexOrNull<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == L));
        if (nullable.HasValue)
          return (keyValuePair.Key, nullable.Value, keyValuePair.Value.data, keyValuePair.Value.patterns);
      }
      return (0, -1, (QuestScoreCampaignProgress[]) null, 0);
    }

    private static bool exEquals(
      this QuestScoreCampaignDescriptionBlock self,
      QuestScoreCampaignDescriptionBlock target)
    {
      if (self == target)
        return true;
      if (self.title != target.title || self.bodies.Length != target.bodies.Length)
        return false;
      for (int index = 0; index < self.bodies.Length; ++index)
      {
        if (!self.bodies[index].exEquals(target.bodies[index]))
          return false;
      }
      return true;
    }

    private static bool exEquals(
      this QuestScoreCampaignDescriptionBlockBodies self,
      QuestScoreCampaignDescriptionBlockBodies target)
    {
      if (self.body == target.body)
      {
        int? nullable1 = self.kind;
        int? nullable2 = target.kind;
        if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
        {
          nullable2 = self.extra_position;
          nullable1 = target.extra_position;
          if (nullable2.GetValueOrDefault() == nullable1.GetValueOrDefault() & nullable2.HasValue == nullable1.HasValue)
          {
            nullable1 = self.extra_id;
            nullable2 = target.extra_id;
            if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
            {
              nullable2 = self.image_width;
              nullable1 = target.image_width;
              if (nullable2.GetValueOrDefault() == nullable1.GetValueOrDefault() & nullable2.HasValue == nullable1.HasValue)
              {
                nullable1 = self.image_height;
                nullable2 = target.image_height;
                if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue && self.image_url == target.image_url)
                {
                  nullable2 = self.extra_type;
                  nullable1 = target.extra_type;
                  return nullable2.GetValueOrDefault() == nullable1.GetValueOrDefault() & nullable2.HasValue == nullable1.HasValue;
                }
              }
            }
          }
        }
      }
      return false;
    }
  }
}
