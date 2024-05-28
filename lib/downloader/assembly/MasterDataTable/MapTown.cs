// Decompiled with JetBrains decompiler
// Type: MasterDataTable.MapTown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class MapTown
  {
    public int ID;
    public string name;
    public int cost_capacity;
    public int stage_id;
    public int point;
    public int turns;
    public int annihilation_point;
    public string description;

    public static MapTown Parse(MasterDataReader reader)
    {
      return new MapTown()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        cost_capacity = reader.ReadInt(),
        stage_id = reader.ReadInt(),
        point = reader.ReadInt(),
        turns = reader.ReadInt(),
        annihilation_point = reader.ReadInt(),
        description = reader.ReadString(true)
      };
    }
  }
}
