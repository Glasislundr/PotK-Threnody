// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestSeaS
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
  public class QuestSeaS
  {
    public int ID;
    public string name;
    public int priority;
    public int quest_xl_QuestSeaXL;
    public int quest_l_QuestSeaL;
    public int quest_m_QuestSeaM;
    public int number_s;
    public int? has_reward;
    public int lost_ap;
    public int stage_BattleStage;
    public bool disable_continue;
    public int gender_restriction_UnitGender;
    public bool story_only;

    public static QuestSeaS Parse(MasterDataReader reader)
    {
      return new QuestSeaS()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        priority = reader.ReadInt(),
        quest_xl_QuestSeaXL = reader.ReadInt(),
        quest_l_QuestSeaL = reader.ReadInt(),
        quest_m_QuestSeaM = reader.ReadInt(),
        number_s = reader.ReadInt(),
        has_reward = reader.ReadIntOrNull(),
        lost_ap = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        disable_continue = reader.ReadBool(),
        gender_restriction_UnitGender = reader.ReadInt(),
        story_only = reader.ReadBool()
      };
    }

    public QuestSeaXL quest_xl
    {
      get
      {
        QuestSeaXL questXl;
        if (!MasterData.QuestSeaXL.TryGetValue(this.quest_xl_QuestSeaXL, out questXl))
          Debug.LogError((object) ("Key not Found: MasterData.QuestSeaXL[" + (object) this.quest_xl_QuestSeaXL + "]"));
        return questXl;
      }
    }

    public QuestSeaL quest_l
    {
      get
      {
        QuestSeaL questL;
        if (!MasterData.QuestSeaL.TryGetValue(this.quest_l_QuestSeaL, out questL))
          Debug.LogError((object) ("Key not Found: MasterData.QuestSeaL[" + (object) this.quest_l_QuestSeaL + "]"));
        return questL;
      }
    }

    public QuestSeaM quest_m
    {
      get
      {
        QuestSeaM questM;
        if (!MasterData.QuestSeaM.TryGetValue(this.quest_m_QuestSeaM, out questM))
          Debug.LogError((object) ("Key not Found: MasterData.QuestSeaM[" + (object) this.quest_m_QuestSeaM + "]"));
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

    public StoryPlaybackSeaDetail GetStoryDetail(StoryPlaybackTiming timing)
    {
      return ((IEnumerable<StoryPlaybackSeaDetail>) this.StoryDetails()).SingleOrDefault<StoryPlaybackSeaDetail>((Func<StoryPlaybackSeaDetail, bool>) (x => x.timing == timing));
    }

    public StoryPlaybackSeaDetail[] StoryDetails()
    {
      return ((IEnumerable<StoryPlaybackSeaDetail>) MasterData.StoryPlaybackSeaDetailList).Where<StoryPlaybackSeaDetail>((Func<StoryPlaybackSeaDetail, bool>) (x => x.quest_s_id.ID == this.ID)).ToArray<StoryPlaybackSeaDetail>();
    }

    public string GetBackgroundPath()
    {
      return this.quest_m != null && this.quest_m.background != null && !string.IsNullOrEmpty(this.quest_m.background.background_name) ? string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) this.quest_m.background.background_name) : Consts.GetInstance().DEFULAT_BACKGROUND;
    }
  }
}
