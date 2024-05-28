// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitXLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitXLevel
  {
    public int ID;
    public int level;
    public int from_exp;

    public static UnitXLevel Parse(MasterDataReader reader)
    {
      return new UnitXLevel()
      {
        ID = reader.ReadInt(),
        level = reader.ReadInt(),
        from_exp = reader.ReadInt()
      };
    }

    public int to_exp
    {
      get
      {
        int nlv = this.level + 1;
        UnitXLevel unitXlevel = Array.Find<UnitXLevel>(MasterData.UnitXLevelList, (Predicate<UnitXLevel>) (x => x.level == nlv));
        return unitXlevel == null ? this.from_exp : unitXlevel.from_exp - 1;
      }
    }

    public static int expToLevel(int exp) => UnitXLevel.expToData(exp).level;

    public static UnitXLevel expToData(int exp)
    {
      UnitXLevel[] unitXlevelList = MasterData.UnitXLevelList;
      int index = 0;
      while (index < unitXlevelList.Length && unitXlevelList[index].from_exp <= exp)
        ++index;
      if (index > 0)
        --index;
      return unitXlevelList[index];
    }
  }
}
