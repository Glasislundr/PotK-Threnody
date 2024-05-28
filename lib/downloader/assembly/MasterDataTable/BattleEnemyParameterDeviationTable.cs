// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleEnemyParameterDeviationTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleEnemyParameterDeviationTable
  {
    public int ID;
    public float deviation_min_hp;
    public float deviation_min_strength;
    public float deviation_min_vitality;
    public float deviation_min_intelligence;
    public float deviation_min_mind;
    public float deviation_min_agility;
    public float deviation_min_dexterity;
    public float deviation_min_lucky;
    public float deviation_max_hp;
    public float deviation_max_strength;
    public float deviation_max_vitality;
    public float deviation_max_intelligence;
    public float deviation_max_mind;
    public float deviation_max_agility;
    public float deviation_max_dexterity;
    public float deviation_max_lucky;

    public static BattleEnemyParameterDeviationTable Parse(MasterDataReader reader)
    {
      return new BattleEnemyParameterDeviationTable()
      {
        ID = reader.ReadInt(),
        deviation_min_hp = reader.ReadFloat(),
        deviation_min_strength = reader.ReadFloat(),
        deviation_min_vitality = reader.ReadFloat(),
        deviation_min_intelligence = reader.ReadFloat(),
        deviation_min_mind = reader.ReadFloat(),
        deviation_min_agility = reader.ReadFloat(),
        deviation_min_dexterity = reader.ReadFloat(),
        deviation_min_lucky = reader.ReadFloat(),
        deviation_max_hp = reader.ReadFloat(),
        deviation_max_strength = reader.ReadFloat(),
        deviation_max_vitality = reader.ReadFloat(),
        deviation_max_intelligence = reader.ReadFloat(),
        deviation_max_mind = reader.ReadFloat(),
        deviation_max_agility = reader.ReadFloat(),
        deviation_max_dexterity = reader.ReadFloat(),
        deviation_max_lucky = reader.ReadFloat()
      };
    }
  }
}
