// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitInitialParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitInitialParam
  {
    public int ID;
    public int hp_initial;
    public int strength_initial;
    public int vitality_initial;
    public int intelligence_initial;
    public int mind_initial;
    public int agility_initial;
    public int dexterity_initial;
    public int lucky_initial;
    public int level_max;

    public static UnitInitialParam Parse(MasterDataReader reader)
    {
      return new UnitInitialParam()
      {
        ID = reader.ReadInt(),
        hp_initial = reader.ReadInt(),
        strength_initial = reader.ReadInt(),
        vitality_initial = reader.ReadInt(),
        intelligence_initial = reader.ReadInt(),
        mind_initial = reader.ReadInt(),
        agility_initial = reader.ReadInt(),
        dexterity_initial = reader.ReadInt(),
        lucky_initial = reader.ReadInt(),
        level_max = reader.ReadInt()
      };
    }
  }
}
