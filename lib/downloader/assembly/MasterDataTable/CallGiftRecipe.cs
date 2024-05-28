// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallGiftRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallGiftRecipe
  {
    public int ID;
    public int? material1_gear_id_GearGear;
    public int? material2_gear_id_GearGear;
    public int? material3_gear_id_GearGear;
    public int? material4_gear_id_GearGear;
    public int? material5_gear_id_GearGear;
    public int? material1_gear_count;
    public int? material2_gear_count;
    public int? material3_gear_count;
    public int? material4_gear_count;
    public int? material5_gear_count;
    public int success_gear_id_GearGear;
    public int failure_gear_id_GearGear;
    public int success_ratio;
    public int cost_money;
    public int failure_gear_count;

    public static CallGiftRecipe Parse(MasterDataReader reader)
    {
      return new CallGiftRecipe()
      {
        ID = reader.ReadInt(),
        material1_gear_id_GearGear = reader.ReadIntOrNull(),
        material2_gear_id_GearGear = reader.ReadIntOrNull(),
        material3_gear_id_GearGear = reader.ReadIntOrNull(),
        material4_gear_id_GearGear = reader.ReadIntOrNull(),
        material5_gear_id_GearGear = reader.ReadIntOrNull(),
        material1_gear_count = reader.ReadIntOrNull(),
        material2_gear_count = reader.ReadIntOrNull(),
        material3_gear_count = reader.ReadIntOrNull(),
        material4_gear_count = reader.ReadIntOrNull(),
        material5_gear_count = reader.ReadIntOrNull(),
        success_gear_id_GearGear = reader.ReadInt(),
        failure_gear_id_GearGear = reader.ReadInt(),
        success_ratio = reader.ReadInt(),
        cost_money = reader.ReadInt(),
        failure_gear_count = reader.ReadInt()
      };
    }

    public GearGear material1_gear_id
    {
      get
      {
        if (!this.material1_gear_id_GearGear.HasValue)
          return (GearGear) null;
        GearGear material1GearId;
        if (!MasterData.GearGear.TryGetValue(this.material1_gear_id_GearGear.Value, out material1GearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.material1_gear_id_GearGear.Value + "]"));
        return material1GearId;
      }
    }

    public GearGear material2_gear_id
    {
      get
      {
        if (!this.material2_gear_id_GearGear.HasValue)
          return (GearGear) null;
        GearGear material2GearId;
        if (!MasterData.GearGear.TryGetValue(this.material2_gear_id_GearGear.Value, out material2GearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.material2_gear_id_GearGear.Value + "]"));
        return material2GearId;
      }
    }

    public GearGear material3_gear_id
    {
      get
      {
        if (!this.material3_gear_id_GearGear.HasValue)
          return (GearGear) null;
        GearGear material3GearId;
        if (!MasterData.GearGear.TryGetValue(this.material3_gear_id_GearGear.Value, out material3GearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.material3_gear_id_GearGear.Value + "]"));
        return material3GearId;
      }
    }

    public GearGear material4_gear_id
    {
      get
      {
        if (!this.material4_gear_id_GearGear.HasValue)
          return (GearGear) null;
        GearGear material4GearId;
        if (!MasterData.GearGear.TryGetValue(this.material4_gear_id_GearGear.Value, out material4GearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.material4_gear_id_GearGear.Value + "]"));
        return material4GearId;
      }
    }

    public GearGear material5_gear_id
    {
      get
      {
        if (!this.material5_gear_id_GearGear.HasValue)
          return (GearGear) null;
        GearGear material5GearId;
        if (!MasterData.GearGear.TryGetValue(this.material5_gear_id_GearGear.Value, out material5GearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.material5_gear_id_GearGear.Value + "]"));
        return material5GearId;
      }
    }

    public GearGear success_gear_id
    {
      get
      {
        GearGear successGearId;
        if (!MasterData.GearGear.TryGetValue(this.success_gear_id_GearGear, out successGearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.success_gear_id_GearGear + "]"));
        return successGearId;
      }
    }

    public GearGear failure_gear_id
    {
      get
      {
        GearGear failureGearId;
        if (!MasterData.GearGear.TryGetValue(this.failure_gear_id_GearGear, out failureGearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.failure_gear_id_GearGear + "]"));
        return failureGearId;
      }
    }
  }
}
