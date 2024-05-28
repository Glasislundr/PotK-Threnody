// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnitSupplement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnitSupplement
  {
    public int ID;
    public int default_weapon_proficiency_UnitProficiency;
    public int default_shield_proficiency_UnitProficiency;

    public static UnitUnitSupplement Parse(MasterDataReader reader)
    {
      return new UnitUnitSupplement()
      {
        ID = reader.ReadInt(),
        default_weapon_proficiency_UnitProficiency = reader.ReadInt(),
        default_shield_proficiency_UnitProficiency = reader.ReadInt()
      };
    }

    public UnitProficiency default_weapon_proficiency
    {
      get
      {
        UnitProficiency weaponProficiency;
        if (!MasterData.UnitProficiency.TryGetValue(this.default_weapon_proficiency_UnitProficiency, out weaponProficiency))
          Debug.LogError((object) ("Key not Found: MasterData.UnitProficiency[" + (object) this.default_weapon_proficiency_UnitProficiency + "]"));
        return weaponProficiency;
      }
    }

    public UnitProficiency default_shield_proficiency
    {
      get
      {
        UnitProficiency shieldProficiency;
        if (!MasterData.UnitProficiency.TryGetValue(this.default_shield_proficiency_UnitProficiency, out shieldProficiency))
          Debug.LogError((object) ("Key not Found: MasterData.UnitProficiency[" + (object) this.default_shield_proficiency_UnitProficiency + "]"));
        return shieldProficiency;
      }
    }
  }
}
