// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BingoRewardGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BingoRewardGroup
  {
    public int ID;
    public int reward_group_id;
    public int reward_type_id_CommonRewardType;
    public int reward_id;
    public int reward_quantity;
    public string reward_message;
    public string background_image_name;

    public static BingoRewardGroup Parse(MasterDataReader reader)
    {
      return new BingoRewardGroup()
      {
        ID = reader.ReadInt(),
        reward_group_id = reader.ReadInt(),
        reward_type_id_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadInt(),
        reward_message = reader.ReadString(true),
        background_image_name = reader.ReadStringOrNull(true)
      };
    }

    public CommonRewardType reward_type_id
    {
      get => (CommonRewardType) this.reward_type_id_CommonRewardType;
    }
  }
}
