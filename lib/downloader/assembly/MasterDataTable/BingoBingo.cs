// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BingoBingo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BingoBingo
  {
    public int ID;
    public DateTime? end_at;
    public string complete_reward_group_ids;
    public string header_image_name;
    public int priority;

    public static BingoBingo Parse(MasterDataReader reader)
    {
      return new BingoBingo()
      {
        ID = reader.ReadInt(),
        end_at = reader.ReadDateTimeOrNull(),
        complete_reward_group_ids = reader.ReadStringOrNull(true),
        header_image_name = reader.ReadStringOrNull(true),
        priority = reader.ReadInt()
      };
    }
  }
}
