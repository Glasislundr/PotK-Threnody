// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnitGrowth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnitGrowth
  {
    public int ID;
    public float hp_growth;
    public float strength_growth;
    public float intelligence_growth;
    public float vitality_growth;
    public float mind_growth;
    public float agility_growth;
    public float dexterity_growth;
    public float lucky_growth;

    public static UnitUnitGrowth Parse(MasterDataReader reader)
    {
      return new UnitUnitGrowth()
      {
        ID = reader.ReadInt(),
        hp_growth = reader.ReadFloat(),
        strength_growth = reader.ReadFloat(),
        intelligence_growth = reader.ReadFloat(),
        vitality_growth = reader.ReadFloat(),
        mind_growth = reader.ReadFloat(),
        agility_growth = reader.ReadFloat(),
        dexterity_growth = reader.ReadFloat(),
        lucky_growth = reader.ReadFloat()
      };
    }
  }
}
