// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnitFamily
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnitFamily
  {
    public int ID;
    public int unit_UnitUnit;
    public int element_UnitFamily;

    public static UnitUnitFamily Parse(MasterDataReader reader)
    {
      return new UnitUnitFamily()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        element_UnitFamily = reader.ReadInt()
      };
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public UnitFamily element => (UnitFamily) this.element_UnitFamily;
  }
}
