// Decompiled with JetBrains decompiler
// Type: MasterDataTable.JobChangeMaterials
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class JobChangeMaterials
  {
    public int ID;
    public int? material1_UnitUnit;
    public int quantity1;
    public int? material2_UnitUnit;
    public int quantity2;
    public int? material3_UnitUnit;
    public int quantity3;
    public int? material4_UnitUnit;
    public int quantity4;
    public int? material5_UnitUnit;
    public int quantity5;
    public int cost;

    public static JobChangeMaterials Parse(MasterDataReader reader)
    {
      return new JobChangeMaterials()
      {
        ID = reader.ReadInt(),
        material1_UnitUnit = reader.ReadIntOrNull(),
        quantity1 = reader.ReadInt(),
        material2_UnitUnit = reader.ReadIntOrNull(),
        quantity2 = reader.ReadInt(),
        material3_UnitUnit = reader.ReadIntOrNull(),
        quantity3 = reader.ReadInt(),
        material4_UnitUnit = reader.ReadIntOrNull(),
        quantity4 = reader.ReadInt(),
        material5_UnitUnit = reader.ReadIntOrNull(),
        quantity5 = reader.ReadInt(),
        cost = reader.ReadInt()
      };
    }

    public UnitUnit material1
    {
      get
      {
        if (!this.material1_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit material1;
        if (!MasterData.UnitUnit.TryGetValue(this.material1_UnitUnit.Value, out material1))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material1_UnitUnit.Value + "]"));
        return material1;
      }
    }

    public UnitUnit material2
    {
      get
      {
        if (!this.material2_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit material2;
        if (!MasterData.UnitUnit.TryGetValue(this.material2_UnitUnit.Value, out material2))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material2_UnitUnit.Value + "]"));
        return material2;
      }
    }

    public UnitUnit material3
    {
      get
      {
        if (!this.material3_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit material3;
        if (!MasterData.UnitUnit.TryGetValue(this.material3_UnitUnit.Value, out material3))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material3_UnitUnit.Value + "]"));
        return material3;
      }
    }

    public UnitUnit material4
    {
      get
      {
        if (!this.material4_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit material4;
        if (!MasterData.UnitUnit.TryGetValue(this.material4_UnitUnit.Value, out material4))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material4_UnitUnit.Value + "]"));
        return material4;
      }
    }

    public UnitUnit material5
    {
      get
      {
        if (!this.material5_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit material5;
        if (!MasterData.UnitUnit.TryGetValue(this.material5_UnitUnit.Value, out material5))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material5_UnitUnit.Value + "]"));
        return material5;
      }
    }
  }
}
