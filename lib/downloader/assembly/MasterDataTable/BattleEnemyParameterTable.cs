// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleEnemyParameterTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleEnemyParameterTable
  {
    public int ID;
    public int initial_hp;
    public int initial_strength;
    public int initial_vitality;
    public int initial_intelligence;
    public int initial_mind;
    public int initial_agility;
    public int initial_dexterity;
    public int initial_lucky;
    public float growth_rate_hp;
    public float growth_rate_strength;
    public float growth_rate_vitality;
    public float growth_rate_intelligence;
    public float growth_rate_mind;
    public float growth_rate_agility;
    public float growth_rate_dexterity;
    public float growth_rate_lucky;

    public static BattleEnemyParameterTable Parse(MasterDataReader reader)
    {
      return new BattleEnemyParameterTable()
      {
        ID = reader.ReadInt(),
        initial_hp = reader.ReadInt(),
        initial_strength = reader.ReadInt(),
        initial_vitality = reader.ReadInt(),
        initial_intelligence = reader.ReadInt(),
        initial_mind = reader.ReadInt(),
        initial_agility = reader.ReadInt(),
        initial_dexterity = reader.ReadInt(),
        initial_lucky = reader.ReadInt(),
        growth_rate_hp = reader.ReadFloat(),
        growth_rate_strength = reader.ReadFloat(),
        growth_rate_vitality = reader.ReadFloat(),
        growth_rate_intelligence = reader.ReadFloat(),
        growth_rate_mind = reader.ReadFloat(),
        growth_rate_agility = reader.ReadFloat(),
        growth_rate_dexterity = reader.ReadFloat(),
        growth_rate_lucky = reader.ReadFloat()
      };
    }
  }
}
