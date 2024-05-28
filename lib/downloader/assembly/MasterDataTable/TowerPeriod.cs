// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TowerPeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TowerPeriod
  {
    public int ID;
    public int tower_id;
    public DateTime? start_at;
    public DateTime? end_at;
    public DateTime? end_at_disp;
    public string tower_name;
    public int floor_id_TowerFloorName;
    public int direction;
    public int banner_id;

    public static TowerPeriod Parse(MasterDataReader reader)
    {
      return new TowerPeriod()
      {
        ID = reader.ReadInt(),
        tower_id = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        end_at_disp = reader.ReadDateTimeOrNull(),
        tower_name = reader.ReadStringOrNull(true),
        floor_id_TowerFloorName = reader.ReadInt(),
        direction = reader.ReadInt(),
        banner_id = reader.ReadInt()
      };
    }

    public TowerFloorName floor_id
    {
      get
      {
        TowerFloorName floorId;
        if (!MasterData.TowerFloorName.TryGetValue(this.floor_id_TowerFloorName, out floorId))
          Debug.LogError((object) ("Key not Found: MasterData.TowerFloorName[" + (object) this.floor_id_TowerFloorName + "]"));
        return floorId;
      }
    }
  }
}
