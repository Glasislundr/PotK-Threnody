// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ExploreFloor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ExploreFloor
  {
    public int ID;
    public int period_id;
    public int floor;
    public string name;
    public int required_seconds;
    public int drop_deck_id;
    public int encount_ratio;
    public int map_distant_id;
    public int map_near_id;
    public int map_statue_id;
    public int map_statue_sub_id;
    public int map_fire_id;
    public int folder_path;

    public static ExploreFloor Parse(MasterDataReader reader)
    {
      return new ExploreFloor()
      {
        ID = reader.ReadInt(),
        period_id = reader.ReadInt(),
        floor = reader.ReadInt(),
        name = reader.ReadString(true),
        required_seconds = reader.ReadInt(),
        drop_deck_id = reader.ReadInt(),
        encount_ratio = reader.ReadInt(),
        map_distant_id = reader.ReadInt(),
        map_near_id = reader.ReadInt(),
        map_statue_id = reader.ReadInt(),
        map_statue_sub_id = reader.ReadInt(),
        map_fire_id = reader.ReadInt(),
        folder_path = reader.ReadInt()
      };
    }
  }
}
