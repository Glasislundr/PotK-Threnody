// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ExploreFloorReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ExploreFloorReward
  {
    public int ID;
    public int period_id;
    public int floor;
    public int reward_type_CommonRewardType;
    public int reward_id;
    public int reward_quantity;
    public string reward_title;
    public string reward_message;

    public static ExploreFloorReward Parse(MasterDataReader reader)
    {
      return new ExploreFloorReward()
      {
        ID = reader.ReadInt(),
        period_id = reader.ReadInt(),
        floor = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadInt(),
        reward_title = reader.ReadString(true),
        reward_message = reader.ReadString(true)
      };
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
