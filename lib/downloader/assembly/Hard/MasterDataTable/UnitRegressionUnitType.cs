// Decompiled with JetBrains decompiler
// Type: Hard.MasterDataTable.UnitRegressionUnitType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;

#nullable disable
namespace Hard.MasterDataTable
{
  public struct UnitRegressionUnitType
  {
    public int unit_id;
    public UnitTypeEnum unit_type;
    public UnitTypeEnum target_type;

    public bool IsMatch(PlayerUnit pu)
    {
      return this.unit_id == pu._unit && this.unit_type == (UnitTypeEnum) pu._unit_type;
    }
  }
}
