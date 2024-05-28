// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleMapFacilitySetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleMapFacilitySetting
  {
    public int ID;
    public int map_BattleStage;
    public int coordinate_x;
    public int coordinate_y;

    public static BattleMapFacilitySetting Parse(MasterDataReader reader)
    {
      return new BattleMapFacilitySetting()
      {
        ID = reader.ReadInt(),
        map_BattleStage = reader.ReadInt(),
        coordinate_x = reader.ReadInt(),
        coordinate_y = reader.ReadInt()
      };
    }

    public BattleStage map
    {
      get
      {
        BattleStage map;
        if (!MasterData.BattleStage.TryGetValue(this.map_BattleStage, out map))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStage[" + (object) this.map_BattleStage + "]"));
        return map;
      }
    }
  }
}
