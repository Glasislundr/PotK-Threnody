// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpRankingCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpRankingCondition
  {
    public int ID;
    public int? rank_upper;
    public int? rank_lower;
    public int? round_number;
    public string disp_text;
    public string image_name;
    public int priority;

    public static PvpRankingCondition Parse(MasterDataReader reader)
    {
      return new PvpRankingCondition()
      {
        ID = reader.ReadInt(),
        rank_upper = reader.ReadIntOrNull(),
        rank_lower = reader.ReadIntOrNull(),
        round_number = reader.ReadIntOrNull(),
        disp_text = reader.ReadString(true),
        image_name = reader.ReadString(true),
        priority = reader.ReadInt()
      };
    }
  }
}
