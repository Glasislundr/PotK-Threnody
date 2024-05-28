// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildRaidRankingRewardCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildRaidRankingRewardCondition
  {
    public int ID;
    public int? high;
    public int? low;
    public string display_text;
    public string image_name;

    public static GuildRaidRankingRewardCondition Parse(MasterDataReader reader)
    {
      return new GuildRaidRankingRewardCondition()
      {
        ID = reader.ReadInt(),
        high = reader.ReadIntOrNull(),
        low = reader.ReadIntOrNull(),
        display_text = reader.ReadString(true),
        image_name = reader.ReadString(true)
      };
    }
  }
}
