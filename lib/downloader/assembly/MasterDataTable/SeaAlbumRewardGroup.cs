// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaAlbumRewardGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaAlbumRewardGroup
  {
    public int ID;
    public int reward_group_id;
    public int reward_type_id_CommonRewardType;
    public int reward_id;
    public int? reward_quantity;
    public string reward_title;
    public string reward_message;

    public static SeaAlbumRewardGroup Parse(MasterDataReader reader)
    {
      return new SeaAlbumRewardGroup()
      {
        ID = reader.ReadInt(),
        reward_group_id = reader.ReadInt(),
        reward_type_id_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadIntOrNull(),
        reward_title = reader.ReadString(true),
        reward_message = reader.ReadString(true)
      };
    }

    public CommonRewardType reward_type_id
    {
      get => (CommonRewardType) this.reward_type_id_CommonRewardType;
    }
  }
}
