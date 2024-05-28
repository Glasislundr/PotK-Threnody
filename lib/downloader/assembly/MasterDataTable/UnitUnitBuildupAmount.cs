// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnitBuildupAmount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnitBuildupAmount
  {
    public int ID;
    public int material_unit_id;
    public int rarity_UnitRarity;
    public int amount;

    public static UnitUnitBuildupAmount Parse(MasterDataReader reader)
    {
      return new UnitUnitBuildupAmount()
      {
        ID = reader.ReadInt(),
        material_unit_id = reader.ReadInt(),
        rarity_UnitRarity = reader.ReadInt(),
        amount = reader.ReadInt()
      };
    }

    public UnitRarity rarity
    {
      get
      {
        UnitRarity rarity;
        if (!MasterData.UnitRarity.TryGetValue(this.rarity_UnitRarity, out rarity))
          Debug.LogError((object) ("Key not Found: MasterData.UnitRarity[" + (object) this.rarity_UnitRarity + "]"));
        return rarity;
      }
    }
  }
}
