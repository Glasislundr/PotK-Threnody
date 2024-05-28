// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaAlbum
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaAlbum
  {
    public int ID;
    public string name;
    public DateTime? start_at;
    public DateTime? end_at;
    public int complete_reward_group_id;

    public static SeaAlbum Parse(MasterDataReader reader)
    {
      return new SeaAlbum()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        complete_reward_group_id = reader.ReadInt()
      };
    }
  }
}
