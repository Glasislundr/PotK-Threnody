// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearBuildupLogic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearBuildupLogic
  {
    private List<int> _material_list;
    public int ID;
    public int base_param;
    public int base_rate;
    public int material_rank1;
    public int material_rank2;
    public int material_rank3;
    public int material_rank4;
    public int material_rank5;

    private void Init()
    {
      if (this._material_list != null)
        return;
      this._material_list = new List<int>();
      this._material_list.Add(this.material_rank1);
      this._material_list.Add(this.material_rank2);
      this._material_list.Add(this.material_rank3);
      this._material_list.Add(this.material_rank4);
      this._material_list.Add(this.material_rank5);
    }

    public int MaterialRankCount()
    {
      this.Init();
      return this._material_list.Count;
    }

    public int MaterialRank(int gear_level)
    {
      this.Init();
      if (this._material_list.Count < gear_level)
        gear_level = this._material_list.Count;
      if (gear_level == 0)
        gear_level = 1;
      return this._material_list[gear_level - 1];
    }

    public static GearBuildupLogic Parse(MasterDataReader reader)
    {
      return new GearBuildupLogic()
      {
        ID = reader.ReadInt(),
        base_param = reader.ReadInt(),
        base_rate = reader.ReadInt(),
        material_rank1 = reader.ReadInt(),
        material_rank2 = reader.ReadInt(),
        material_rank3 = reader.ReadInt(),
        material_rank4 = reader.ReadInt(),
        material_rank5 = reader.ReadInt()
      };
    }
  }
}
