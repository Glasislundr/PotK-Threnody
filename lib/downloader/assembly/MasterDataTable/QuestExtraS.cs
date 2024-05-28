// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraS
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
  public class QuestExtraS
  {
    public int ID;
    public string name;
    public int quest_l_QuestExtraL;
    public int quest_m_QuestExtraM;
    public int number_s;
    public int banner_id;
    public int? has_reward;
    public int lost_ap;
    public int priority;
    public int stage_BattleStage;
    public int? wave_QuestWave;
    public int extra_quest_area_CommonExtraQuestArea;
    public bool disable_continue;
    public string seek_index;
    public int gender_restriction_UnitGender;
    public bool story_only;

    public static QuestExtraS Parse(MasterDataReader reader)
    {
      return new QuestExtraS()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_l_QuestExtraL = reader.ReadInt(),
        quest_m_QuestExtraM = reader.ReadInt(),
        number_s = reader.ReadInt(),
        banner_id = reader.ReadInt(),
        has_reward = reader.ReadIntOrNull(),
        lost_ap = reader.ReadInt(),
        priority = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        wave_QuestWave = reader.ReadIntOrNull(),
        extra_quest_area_CommonExtraQuestArea = reader.ReadInt(),
        disable_continue = reader.ReadBool(),
        seek_index = reader.ReadString(true),
        gender_restriction_UnitGender = reader.ReadInt(),
        story_only = reader.ReadBool()
      };
    }

    public QuestExtraL quest_l
    {
      get
      {
        QuestExtraL questL;
        if (!MasterData.QuestExtraL.TryGetValue(this.quest_l_QuestExtraL, out questL))
          Debug.LogError((object) ("Key not Found: MasterData.QuestExtraL[" + (object) this.quest_l_QuestExtraL + "]"));
        return questL;
      }
    }

    public QuestExtraM quest_m
    {
      get
      {
        QuestExtraM questM;
        if (!MasterData.QuestExtraM.TryGetValue(this.quest_m_QuestExtraM, out questM))
          Debug.LogError((object) ("Key not Found: MasterData.QuestExtraM[" + (object) this.quest_m_QuestExtraM + "]"));
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

    public QuestWave wave
    {
      get
      {
        if (!this.wave_QuestWave.HasValue)
          return (QuestWave) null;
        QuestWave wave;
        if (!MasterData.QuestWave.TryGetValue(this.wave_QuestWave.Value, out wave))
          Debug.LogError((object) ("Key not Found: MasterData.QuestWave[" + (object) this.wave_QuestWave.Value + "]"));
        return wave;
      }
    }

    public CommonExtraQuestArea extra_quest_area
    {
      get => (CommonExtraQuestArea) this.extra_quest_area_CommonExtraQuestArea;
    }

    public UnitGender gender_restriction => (UnitGender) this.gender_restriction_UnitGender;

    public StoryPlaybackExtraDetail[] ExtraDetails()
    {
      return ((IEnumerable<StoryPlaybackExtraDetail>) MasterData.StoryPlaybackExtraDetailList).Where<StoryPlaybackExtraDetail>((Func<StoryPlaybackExtraDetail, bool>) (x => x.extra.quest.ID == this.ID)).ToArray<StoryPlaybackExtraDetail>();
    }

    public StoryPlaybackExtraDetail GetExtraDetail(StoryPlaybackTiming timing)
    {
      return ((IEnumerable<StoryPlaybackExtraDetail>) this.ExtraDetails()).SingleOrDefault<StoryPlaybackExtraDetail>((Func<StoryPlaybackExtraDetail, bool>) (x => x.timing == timing));
    }

    public string GetBackgroundPath()
    {
      return this.quest_m != null && this.quest_m.background != null && !string.IsNullOrEmpty(this.quest_m.background.background_name) ? string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) this.quest_m.background.background_name) : Consts.GetInstance().DEFULAT_BACKGROUND;
    }

    public QuestExtraS.SeekType n_seek_type
    {
      get
      {
        switch (this.seek_index)
        {
          case "m":
          case "M":
            return QuestExtraS.SeekType.M;
          case "l":
          case "L":
            return QuestExtraS.SeekType.L;
          default:
            return QuestExtraS.SeekType.None;
        }
      }
    }

    public QuestExtraLL quest_ll
    {
      get
      {
        switch (this.n_seek_type)
        {
          case QuestExtraS.SeekType.M:
            return this.quest_m.quest_ll;
          case QuestExtraS.SeekType.L:
            return this.quest_l.quest_ll;
          default:
            return (QuestExtraLL) null;
        }
      }
    }

    public QuestExtra.SeekType seek_type => QuestExtra.toSeekType(this.n_seek_type);

    public enum SeekType
    {
      None,
      M,
      L,
    }
  }
}
