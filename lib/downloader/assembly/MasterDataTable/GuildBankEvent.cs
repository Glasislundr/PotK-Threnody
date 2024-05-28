// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildBankEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildBankEvent
  {
    public int ID;
    public int level;
    public DateTime? start_at;
    public DateTime? end_at;
    public bool badge_flag;

    public static GuildBankEvent Parse(MasterDataReader reader)
    {
      return new GuildBankEvent()
      {
        ID = reader.ReadInt(),
        level = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        badge_flag = reader.ReadBool()
      };
    }
  }
}
