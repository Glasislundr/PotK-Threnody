// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsPlaybackStoryDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsPlaybackStoryDetail
  {
    public int ID;
    public string name;
    public int stage_id;
    public int story_id;
    public int timing_StoryPlaybackTiming;
    public int? stage_enemy_id;
    public int attack_timing_type;
    public int script_id;
    public int timing_parameter_0;
    public int timing_parameter_1;
    public int timing_parameter_2;
    public int timing_parameter_3;

    public static CorpsPlaybackStoryDetail Parse(MasterDataReader reader)
    {
      return new CorpsPlaybackStoryDetail()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        stage_id = reader.ReadInt(),
        story_id = reader.ReadInt(),
        timing_StoryPlaybackTiming = reader.ReadInt(),
        stage_enemy_id = reader.ReadIntOrNull(),
        attack_timing_type = reader.ReadInt(),
        script_id = reader.ReadInt(),
        timing_parameter_0 = reader.ReadInt(),
        timing_parameter_1 = reader.ReadInt(),
        timing_parameter_2 = reader.ReadInt(),
        timing_parameter_3 = reader.ReadInt()
      };
    }

    public StoryPlaybackTiming timing => (StoryPlaybackTiming) this.timing_StoryPlaybackTiming;

    public Tuple<StoryPlaybackTiming, int, object[]> toTuple()
    {
      object[] objArray;
      switch (this.timing)
      {
        case StoryPlaybackTiming.turn_start:
          objArray = new object[3]
          {
            (object) this.attack_timing_type,
            (object) this.timing_parameter_0,
            (object) this.timing_parameter_1
          };
          break;
        case StoryPlaybackTiming.in_area:
          objArray = new object[5]
          {
            (object) this.attack_timing_type,
            (object) (this.timing_parameter_1 - 1),
            (object) (this.timing_parameter_0 - 1),
            (object) this.timing_parameter_3,
            (object) this.timing_parameter_2
          };
          break;
        case StoryPlaybackTiming.defeat_player:
          if (this.stage_enemy_id.HasValue)
          {
            objArray = new object[1]
            {
              (object) this.stage_enemy_id.Value
            };
            break;
          }
          objArray = new object[0];
          Debug.LogError((object) ("ScriptID = " + this.script_id.ToString() + ", StoryPlaybackTiming.defeat_player のパラメータ不足"));
          break;
        case StoryPlaybackTiming.wave_clear:
          objArray = new object[1]
          {
            (object) this.timing_parameter_0
          };
          break;
        default:
          if (this.stage_enemy_id.HasValue)
          {
            objArray = new object[2]
            {
              (object) this.attack_timing_type,
              (object) this.stage_enemy_id.Value
            };
            break;
          }
          objArray = new object[0];
          break;
      }
      return Tuple.Create<StoryPlaybackTiming, int, object[]>(this.timing, this.script_id, objArray);
    }
  }
}
