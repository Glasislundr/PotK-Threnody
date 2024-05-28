// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildBaseBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildBaseBonus
  {
    public int ID;
    public int base_type_GuildBaseType;
    public int base_rank;
    public int bonus_type_GuildBaseBonusType;
    public int bonus_grade;
    public int bonus_amount;

    public static GuildBaseBonus Parse(MasterDataReader reader)
    {
      return new GuildBaseBonus()
      {
        ID = reader.ReadInt(),
        base_type_GuildBaseType = reader.ReadInt(),
        base_rank = reader.ReadInt(),
        bonus_type_GuildBaseBonusType = reader.ReadInt(),
        bonus_grade = reader.ReadInt(),
        bonus_amount = reader.ReadInt()
      };
    }

    public GuildBaseType base_type => (GuildBaseType) this.base_type_GuildBaseType;

    public GuildBaseBonusType bonus_type => (GuildBaseBonusType) this.bonus_type_GuildBaseBonusType;
  }
}
