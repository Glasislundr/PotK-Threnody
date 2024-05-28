// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildRaidPeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildRaidPeriod
  {
    public int ID;
    public string period_name;
    public DateTime? start_at;
    public DateTime? end_at;
    public int button_id;
    public int banner_id;
    public string bg_path;

    public static GuildRaidPeriod Parse(MasterDataReader reader)
    {
      return new GuildRaidPeriod()
      {
        ID = reader.ReadInt(),
        period_name = reader.ReadStringOrNull(true),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        button_id = reader.ReadInt(),
        banner_id = reader.ReadInt(),
        bg_path = reader.ReadStringOrNull(true)
      };
    }
  }
}
