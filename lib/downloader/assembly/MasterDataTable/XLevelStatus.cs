// Decompiled with JetBrains decompiler
// Type: MasterDataTable.XLevelStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class XLevelStatus
  {
    public int ID;
    public int hp_level_up_max;
    public int strength_level_up_max;
    public int vitality_level_up_max;
    public int intelligence_level_up_max;
    public int mind_level_up_max;
    public int agility_level_up_max;
    public int dexterity_level_up_max;
    public int lucky_level_up_max;

    public static XLevelStatus Parse(MasterDataReader reader)
    {
      return new XLevelStatus()
      {
        ID = reader.ReadInt(),
        hp_level_up_max = reader.ReadInt(),
        strength_level_up_max = reader.ReadInt(),
        vitality_level_up_max = reader.ReadInt(),
        intelligence_level_up_max = reader.ReadInt(),
        mind_level_up_max = reader.ReadInt(),
        agility_level_up_max = reader.ReadInt(),
        dexterity_level_up_max = reader.ReadInt(),
        lucky_level_up_max = reader.ReadInt()
      };
    }
  }
}
