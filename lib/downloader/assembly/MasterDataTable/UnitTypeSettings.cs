// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitTypeSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitTypeSettings
  {
    public int ID;
    public int? unit_UnitUnit;
    public int attack_type_GearAttackType;

    public static UnitTypeSettings Parse(MasterDataReader reader)
    {
      return new UnitTypeSettings()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadIntOrNull(),
        attack_type_GearAttackType = reader.ReadInt()
      };
    }

    public UnitUnit unit
    {
      get
      {
        if (!this.unit_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit.Value, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit.Value + "]"));
        return unit;
      }
    }

    public GearAttackType attack_type => (GearAttackType) this.attack_type_GearAttackType;
  }
}
