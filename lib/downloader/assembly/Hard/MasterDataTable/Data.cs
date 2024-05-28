// Decompiled with JetBrains decompiler
// Type: Hard.MasterDataTable.Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
namespace Hard.MasterDataTable
{
  public static class Data
  {
    public static readonly UnitRegressionUnitType[] UnitRegressionUnitTypes = new UnitRegressionUnitType[4]
    {
      new UnitRegressionUnitType()
      {
        unit_id = 103013,
        unit_type = UnitTypeEnum.kouki,
        target_type = UnitTypeEnum.maki
      },
      new UnitRegressionUnitType()
      {
        unit_id = 502213,
        unit_type = UnitTypeEnum.maki,
        target_type = UnitTypeEnum.kouki
      },
      new UnitRegressionUnitType()
      {
        unit_id = 601513,
        unit_type = UnitTypeEnum.maki,
        target_type = UnitTypeEnum.kouki
      },
      new UnitRegressionUnitType()
      {
        unit_id = 302213,
        unit_type = UnitTypeEnum.kouki,
        target_type = UnitTypeEnum.maki
      }
    };
  }
}
