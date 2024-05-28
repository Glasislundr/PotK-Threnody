// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnitBuildupLimitRelease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnitBuildupLimitRelease
  {
    public int ID;
    public int hp_limit_release_cnt;
    public int strength_limit_release_cnt;
    public int vitality_limit_release_cnt;
    public int intelligence_limit_release_cnt;
    public int mind_limit_release_cnt;
    public int agility_limit_release_cnt;
    public int dexterity_limit_release_cnt;
    public int lucky_limit_release_cnt;

    public static UnitUnitBuildupLimitRelease Parse(MasterDataReader reader)
    {
      return new UnitUnitBuildupLimitRelease()
      {
        ID = reader.ReadInt(),
        hp_limit_release_cnt = reader.ReadInt(),
        strength_limit_release_cnt = reader.ReadInt(),
        vitality_limit_release_cnt = reader.ReadInt(),
        intelligence_limit_release_cnt = reader.ReadInt(),
        mind_limit_release_cnt = reader.ReadInt(),
        agility_limit_release_cnt = reader.ReadInt(),
        dexterity_limit_release_cnt = reader.ReadInt(),
        lucky_limit_release_cnt = reader.ReadInt()
      };
    }
  }
}
