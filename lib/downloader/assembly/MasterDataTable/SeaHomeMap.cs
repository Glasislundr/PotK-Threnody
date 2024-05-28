// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaHomeMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaHomeMap
  {
    public int ID;
    public DateTime? start_at;
    public DateTime? end_at;
    public int time_zone_SeaHomeTimeZone;
    public int map_BattleMap;
    public string bgm_cuesheet_name;
    public string bgm_cue_name;

    public static SeaHomeMap Parse(MasterDataReader reader)
    {
      return new SeaHomeMap()
      {
        ID = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        time_zone_SeaHomeTimeZone = reader.ReadInt(),
        map_BattleMap = reader.ReadInt(),
        bgm_cuesheet_name = reader.ReadString(true),
        bgm_cue_name = reader.ReadString(true)
      };
    }

    public SeaHomeTimeZone time_zone
    {
      get
      {
        SeaHomeTimeZone timeZone;
        if (!MasterData.SeaHomeTimeZone.TryGetValue(this.time_zone_SeaHomeTimeZone, out timeZone))
          Debug.LogError((object) ("Key not Found: MasterData.SeaHomeTimeZone[" + (object) this.time_zone_SeaHomeTimeZone + "]"));
        return timeZone;
      }
    }

    public BattleMap map
    {
      get
      {
        BattleMap map;
        if (!MasterData.BattleMap.TryGetValue(this.map_BattleMap, out map))
          Debug.LogError((object) ("Key not Found: MasterData.BattleMap[" + (object) this.map_BattleMap + "]"));
        return map;
      }
    }

    public bool WithIn(DateTime now)
    {
      if (this.start_at.HasValue && (!this.start_at.HasValue || !(this.start_at.Value <= now)))
        return false;
      if (!this.end_at.HasValue)
        return true;
      return this.end_at.HasValue && this.end_at.Value >= now;
    }
  }
}
