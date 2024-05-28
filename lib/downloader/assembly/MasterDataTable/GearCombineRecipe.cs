// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearCombineRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearCombineRecipe
  {
    public int ID;
    public int material1_gear_id;
    public int? material2_gear_id;
    public int? material3_gear_id;
    public int? material4_gear_id;
    public int? material5_gear_id;
    public int? material1_gear_rank;
    public int? material2_gear_rank;
    public int? material3_gear_rank;
    public int? material4_gear_rank;
    public int? material5_gear_rank;
    public int combined_gear_id;
    public string extension;
    public DateTime? start_at;
    public DateTime? end_at;
    public int priority;

    public static GearCombineRecipe Parse(MasterDataReader reader)
    {
      return new GearCombineRecipe()
      {
        ID = reader.ReadInt(),
        material1_gear_id = reader.ReadInt(),
        material2_gear_id = reader.ReadIntOrNull(),
        material3_gear_id = reader.ReadIntOrNull(),
        material4_gear_id = reader.ReadIntOrNull(),
        material5_gear_id = reader.ReadIntOrNull(),
        material1_gear_rank = reader.ReadIntOrNull(),
        material2_gear_rank = reader.ReadIntOrNull(),
        material3_gear_rank = reader.ReadIntOrNull(),
        material4_gear_rank = reader.ReadIntOrNull(),
        material5_gear_rank = reader.ReadIntOrNull(),
        combined_gear_id = reader.ReadInt(),
        extension = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        priority = reader.ReadInt()
      };
    }

    public List<string> ResourcePaths()
    {
      List<string> stringList = new List<string>();
      GearGear gearGear1;
      if (MasterData.GearGear.TryGetValue(this.combined_gear_id, out gearGear1))
        stringList.AddRange((IEnumerable<string>) gearGear1.ResourcePaths());
      HashSet<int> intSet = new HashSet<int>()
      {
        this.material1_gear_id
      };
      if (this.material2_gear_id.HasValue)
        intSet.Add(this.material2_gear_id.Value);
      if (this.material3_gear_id.HasValue)
        intSet.Add(this.material3_gear_id.Value);
      if (this.material4_gear_id.HasValue)
        intSet.Add(this.material4_gear_id.Value);
      if (this.material5_gear_id.HasValue)
        intSet.Add(this.material5_gear_id.Value);
      foreach (int num in intSet)
      {
        int gId = num;
        GearGear gearGear2 = Array.Find<GearGear>(MasterData.GearGearList, (Predicate<GearGear>) (x => x.group_id == gId));
        if (gearGear2 != null)
          stringList.AddRange((IEnumerable<string>) gearGear2.ResourcePaths());
      }
      return stringList;
    }
  }
}
