// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthQuestExtraStoryPlayback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthQuestExtraStoryPlayback
  {
    public int ID;
    public int episode_EarthQuestEpisode;
    public int timing_StoryPlaybackTiming;
    public int? stage_enemy_BattleStageEnemy;
    public int attack_timing_type;
    public int script_ScriptScript;
    public string title;

    public static EarthQuestExtraStoryPlayback Parse(MasterDataReader reader)
    {
      return new EarthQuestExtraStoryPlayback()
      {
        ID = reader.ReadInt(),
        episode_EarthQuestEpisode = reader.ReadInt(),
        timing_StoryPlaybackTiming = reader.ReadInt(),
        stage_enemy_BattleStageEnemy = reader.ReadIntOrNull(),
        attack_timing_type = reader.ReadInt(),
        script_ScriptScript = reader.ReadInt(),
        title = reader.ReadString(true)
      };
    }

    public EarthQuestEpisode episode
    {
      get
      {
        EarthQuestEpisode episode;
        if (!MasterData.EarthQuestEpisode.TryGetValue(this.episode_EarthQuestEpisode, out episode))
          Debug.LogError((object) ("Key not Found: MasterData.EarthQuestEpisode[" + (object) this.episode_EarthQuestEpisode + "]"));
        return episode;
      }
    }

    public StoryPlaybackTiming timing => (StoryPlaybackTiming) this.timing_StoryPlaybackTiming;

    public BattleStageEnemy stage_enemy
    {
      get
      {
        if (!this.stage_enemy_BattleStageEnemy.HasValue)
          return (BattleStageEnemy) null;
        BattleStageEnemy stageEnemy;
        if (!MasterData.BattleStageEnemy.TryGetValue(this.stage_enemy_BattleStageEnemy.Value, out stageEnemy))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStageEnemy[" + (object) this.stage_enemy_BattleStageEnemy.Value + "]"));
        return stageEnemy;
      }
    }

    public ScriptScript script
    {
      get
      {
        ScriptScript script;
        if (!MasterData.ScriptScript.TryGetValue(this.script_ScriptScript, out script))
          Debug.LogError((object) ("Key not Found: MasterData.ScriptScript[" + (object) this.script_ScriptScript + "]"));
        return script;
      }
    }
  }
}
