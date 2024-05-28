// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestStoryS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestStoryS
  {
    public int ID;
    public string name;
    public int priority;
    public int quest_xl_QuestStoryXL;
    public int quest_l_QuestStoryL;
    public int quest_m_QuestStoryM;
    public int number_s;
    public int? has_reward;
    public int lost_ap;
    public int stage_BattleStage;
    public bool disable_continue;
    public int gender_restriction_UnitGender;
    public bool story_only;

    public static QuestStoryS Parse(MasterDataReader reader)
    {
      return new QuestStoryS()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        priority = reader.ReadInt(),
        quest_xl_QuestStoryXL = reader.ReadInt(),
        quest_l_QuestStoryL = reader.ReadInt(),
        quest_m_QuestStoryM = reader.ReadInt(),
        number_s = reader.ReadInt(),
        has_reward = reader.ReadIntOrNull(),
        lost_ap = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        disable_continue = reader.ReadBool(),
        gender_restriction_UnitGender = reader.ReadInt(),
        story_only = reader.ReadBool()
      };
    }

    public QuestStoryXL quest_xl
    {
      get
      {
        QuestStoryXL questXl;
        if (!MasterData.QuestStoryXL.TryGetValue(this.quest_xl_QuestStoryXL, out questXl))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryXL[" + (object) this.quest_xl_QuestStoryXL + "]"));
        return questXl;
      }
    }

    public QuestStoryL quest_l
    {
      get
      {
        QuestStoryL questL;
        if (!MasterData.QuestStoryL.TryGetValue(this.quest_l_QuestStoryL, out questL))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryL[" + (object) this.quest_l_QuestStoryL + "]"));
        return questL;
      }
    }

    public QuestStoryM quest_m
    {
      get
      {
        QuestStoryM questM;
        if (!MasterData.QuestStoryM.TryGetValue(this.quest_m_QuestStoryM, out questM))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryM[" + (object) this.quest_m_QuestStoryM + "]"));
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

    public StoryPlaybackStoryDetail GetStoryDetail(StoryPlaybackTiming timing)
    {
      return ((IEnumerable<StoryPlaybackStoryDetail>) this.StoryDetails()).SingleOrDefault<StoryPlaybackStoryDetail>((Func<StoryPlaybackStoryDetail, bool>) (x => x.timing == timing));
    }

    public StoryPlaybackStoryDetail[] StoryDetails()
    {
      return ((IEnumerable<StoryPlaybackStoryDetail>) MasterData.StoryPlaybackStoryDetailList).Where<StoryPlaybackStoryDetail>((Func<StoryPlaybackStoryDetail, bool>) (x => x.quest_s_id.ID == this.ID)).ToArray<StoryPlaybackStoryDetail>();
    }

    public string GetBackgroundPath()
    {
      return this.quest_m != null && this.quest_m.background != null && !string.IsNullOrEmpty(this.quest_m.background.background_name) ? string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) this.quest_m.background.background_name) : Consts.GetInstance().DEFULAT_BACKGROUND;
    }
  }
}
