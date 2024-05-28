// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestHarmonyS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestHarmonyS
  {
    public int ID;
    public string name;
    public int unit_UnitUnit;
    public int target_unit_UnitUnit;
    public int? target_unit2_UnitUnit;
    public int quest_m_QuestHarmonyM;
    public int priority;
    public int? has_reward;
    public int lost_ap;
    public int stage_BattleStage;
    public bool disable_continue;
    public int unit_x;
    public int unit_y;
    public float unit_scale;
    public int target_unit_x;
    public int target_unit_y;
    public float target_unit_scale;
    public int target_unit2_x;
    public int target_unit2_y;
    public float target_unit2_scale;
    public DateTime? start_at;
    public int gender_restriction_UnitGender;
    public bool story_only;

    public static QuestHarmonyS Parse(MasterDataReader reader)
    {
      return new QuestHarmonyS()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        unit_UnitUnit = reader.ReadInt(),
        target_unit_UnitUnit = reader.ReadInt(),
        target_unit2_UnitUnit = reader.ReadIntOrNull(),
        quest_m_QuestHarmonyM = reader.ReadInt(),
        priority = reader.ReadInt(),
        has_reward = reader.ReadIntOrNull(),
        lost_ap = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        disable_continue = reader.ReadBool(),
        unit_x = reader.ReadInt(),
        unit_y = reader.ReadInt(),
        unit_scale = reader.ReadFloat(),
        target_unit_x = reader.ReadInt(),
        target_unit_y = reader.ReadInt(),
        target_unit_scale = reader.ReadFloat(),
        target_unit2_x = reader.ReadInt(),
        target_unit2_y = reader.ReadInt(),
        target_unit2_scale = reader.ReadFloat(),
        start_at = reader.ReadDateTimeOrNull(),
        gender_restriction_UnitGender = reader.ReadInt(),
        story_only = reader.ReadBool()
      };
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public UnitUnit target_unit
    {
      get
      {
        UnitUnit targetUnit;
        if (!MasterData.UnitUnit.TryGetValue(this.target_unit_UnitUnit, out targetUnit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.target_unit_UnitUnit + "]"));
        return targetUnit;
      }
    }

    public UnitUnit target_unit2
    {
      get
      {
        if (!this.target_unit2_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit targetUnit2;
        if (!MasterData.UnitUnit.TryGetValue(this.target_unit2_UnitUnit.Value, out targetUnit2))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.target_unit2_UnitUnit.Value + "]"));
        return targetUnit2;
      }
    }

    public QuestHarmonyM quest_m
    {
      get
      {
        QuestHarmonyM questM;
        if (!MasterData.QuestHarmonyM.TryGetValue(this.quest_m_QuestHarmonyM, out questM))
          Debug.LogError((object) ("Key not Found: MasterData.QuestHarmonyM[" + (object) this.quest_m_QuestHarmonyM + "]"));
        return questM;
      }
    }

    public BattleStage stage
    {
      get
      {
        BattleStage stage;
        if (!MasterData.BattleStage.TryGetValue(this.stage_BattleStage, out stage))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStage[" + (object) this.stage_BattleStage + "]"));
        return stage;
      }
    }

    public UnitGender gender_restriction => (UnitGender) this.gender_restriction_UnitGender;

    public StoryPlaybackHarmonyDetail[] HarmonyDetail()
    {
      return ((IEnumerable<StoryPlaybackHarmonyDetail>) MasterData.StoryPlaybackHarmonyDetailList).Where<StoryPlaybackHarmonyDetail>((Func<StoryPlaybackHarmonyDetail, bool>) (x => x.quest.ID == this.ID)).ToArray<StoryPlaybackHarmonyDetail>();
    }

    public StoryPlaybackHarmonyDetail GetHarmonyDetail(StoryPlaybackTiming timing)
    {
      return ((IEnumerable<StoryPlaybackHarmonyDetail>) this.HarmonyDetail()).SingleOrDefault<StoryPlaybackHarmonyDetail>((Func<StoryPlaybackHarmonyDetail, bool>) (x => x.timing == timing));
    }

    public static Dictionary<int, bool> createPlayableQuestIDsMap(PlayerHarmonyQuestS[] quests)
    {
      return ((IEnumerable<PlayerHarmonyQuestS>) quests).Aggregate<PlayerHarmonyQuestS, Dictionary<int, bool>>(new Dictionary<int, bool>(), (Func<Dictionary<int, bool>, PlayerHarmonyQuestS, Dictionary<int, bool>>) ((prod, next) =>
      {
        prod[next.quest_harmony_s.quest_m.ID] = true;
        return prod;
      }));
    }

    public string GetBackgroundPath()
    {
      return this.quest_m != null && this.quest_m.background != null && !string.IsNullOrEmpty(this.quest_m.background.background_name) ? string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) this.quest_m.background.background_name) : Consts.GetInstance().DEFULAT_BACKGROUND;
    }

    public static PlayerHarmonyQuestS[] SelectReleased(PlayerHarmonyQuestS[] quests)
    {
      return ((IEnumerable<PlayerHarmonyQuestS>) quests).Where<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (questDetail => QuestCharacterS.CheckIsReleased(questDetail.quest_harmony_s.start_at))).ToArray<PlayerHarmonyQuestS>();
    }
  }
}
