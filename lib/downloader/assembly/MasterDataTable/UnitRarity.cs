// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitRarity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitRarity
  {
    public int ID;
    public string name;
    public int index;
    public int sell_rarity_medal;
    public int skill_levelup_rate;
    public float indicator_level_rate;
    public int reincarnation_level;
    public float trust_rate;

    public static UnitRarity Parse(MasterDataReader reader)
    {
      return new UnitRarity()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        index = reader.ReadInt(),
        sell_rarity_medal = reader.ReadInt(),
        skill_levelup_rate = reader.ReadInt(),
        indicator_level_rate = reader.ReadFloat(),
        reincarnation_level = reader.ReadInt(),
        trust_rate = reader.ReadFloat()
      };
    }
  }
}
