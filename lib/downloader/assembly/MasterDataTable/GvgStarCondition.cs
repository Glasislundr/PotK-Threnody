// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GvgStarCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GvgStarCondition
  {
    public int ID;
    public int breakaway_condition_GvgBreakawayCondition;
    public bool player_gauge_condition;
    public int player_gauge_value;
    public bool enemy_gauge_condition;
    public int enemy_gauge_value;
    public bool remain_turn_condition;
    public int remain_turn_value;
    public int star_num;

    public static GvgStarCondition Parse(MasterDataReader reader)
    {
      return new GvgStarCondition()
      {
        ID = reader.ReadInt(),
        breakaway_condition_GvgBreakawayCondition = reader.ReadInt(),
        player_gauge_condition = reader.ReadBool(),
        player_gauge_value = reader.ReadInt(),
        enemy_gauge_condition = reader.ReadBool(),
        enemy_gauge_value = reader.ReadInt(),
        remain_turn_condition = reader.ReadBool(),
        remain_turn_value = reader.ReadInt(),
        star_num = reader.ReadInt()
      };
    }

    public GvgBreakawayCondition breakaway_condition
    {
      get => (GvgBreakawayCondition) this.breakaway_condition_GvgBreakawayCondition;
    }
  }
}
