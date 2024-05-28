// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestCharacterS
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
  public class QuestCharacterS
  {
    public int ID;
    public string name;
    public int unit_UnitUnit;
    public int quest_m_QuestCharacterM;
    public int priority;
    public int? has_reward;
    public int lost_ap;
    public int stage_BattleStage;
    public bool disable_continue;
    public DateTime? start_at;
    public int gender_restriction_UnitGender;
    public bool story_only;

    public static QuestCharacterS Parse(MasterDataReader reader)
    {
      return new QuestCharacterS()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        unit_UnitUnit = reader.ReadInt(),
        quest_m_QuestCharacterM = reader.ReadInt(),
        priority = reader.ReadInt(),
        has_reward = reader.ReadIntOrNull(),
        lost_ap = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        disable_continue = reader.ReadBool(),
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

    public QuestCharacterM quest_m
    {
      get
      {
        QuestCharacterM questM;
        if (!MasterData.QuestCharacterM.TryGetValue(this.quest_m_QuestCharacterM, out questM))
          Debug.LogError((object) ("Key not Found: MasterData.QuestCharacterM[" + (object) this.quest_m_QuestCharacterM + "]"));
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

    public StoryPlaybackCharacterDetail[] CharacterDetails()
    {
      return ((IEnumerable<StoryPlaybackCharacterDetail>) MasterData.StoryPlaybackCharacterDetailList).Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.quest.ID == this.ID)).ToArray<StoryPlaybackCharacterDetail>();
    }

    public StoryPlaybackCharacterDetail GetCharacterDetail(StoryPlaybackTiming timing)
    {
      return ((IEnumerable<StoryPlaybackCharacterDetail>) this.CharacterDetails()).SingleOrDefault<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == timing));
    }

    public static Dictionary<int, bool> createPlayableQuestIDsMap(PlayerCharacterQuestS[] quests)
    {
      return ((IEnumerable<PlayerCharacterQuestS>) quests).Aggregate<PlayerCharacterQuestS, Dictionary<int, bool>>(new Dictionary<int, bool>(), (Func<Dictionary<int, bool>, PlayerCharacterQuestS, Dictionary<int, bool>>) ((prod, next) =>
      {
        prod[next.quest_character_s.quest_m.ID] = true;
        return prod;
      }));
    }

    public string GetBackgroundPath()
    {
      return this.quest_m != null && this.quest_m.background != null && !string.IsNullOrEmpty(this.quest_m.background.background_name) ? string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) this.quest_m.background.background_name) : Consts.GetInstance().DEFULAT_BACKGROUND;
    }

    public static bool CheckIsReleased(DateTime? datetime)
    {
      return !datetime.HasValue || datetime.Value.CompareTo(ServerTime.NowAppTime()) <= 0;
    }
  }
}
