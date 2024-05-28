// Decompiled with JetBrains decompiler
// Type: MasterDataTable.DailyMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class DailyMission
  {
    public int ID;
    public bool _enable;
    public int point;
    public int priority;
    public int num;
    public int limit_count;
    public int condition;
    public string name;
    public string detail;
    public string scene;
    public DateTime? start_at;
    public DateTime? end_at;
    public int mission_type_MissionType;

    public static DailyMission Parse(MasterDataReader reader)
    {
      return new DailyMission()
      {
        ID = reader.ReadInt(),
        _enable = reader.ReadBool(),
        point = reader.ReadInt(),
        priority = reader.ReadInt(),
        num = reader.ReadInt(),
        limit_count = reader.ReadInt(),
        condition = reader.ReadInt(),
        name = reader.ReadString(true),
        detail = reader.ReadString(true),
        scene = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        mission_type_MissionType = reader.ReadInt()
      };
    }

    public MissionType mission_type => (MissionType) this.mission_type_MissionType;
  }
}
