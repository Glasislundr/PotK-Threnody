// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildGiftEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildGiftEvent
  {
    public int ID;
    public DateTime? start_at;
    public DateTime? end_at;
    public bool badge_flag;

    public static GuildGiftEvent Parse(MasterDataReader reader)
    {
      return new GuildGiftEvent()
      {
        ID = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        badge_flag = reader.ReadBool()
      };
    }
  }
}
