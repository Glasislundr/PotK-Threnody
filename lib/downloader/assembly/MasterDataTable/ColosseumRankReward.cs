// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ColosseumRankReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ColosseumRankReward
  {
    public int ID;
    public int rank_id;
    public int reward_type_CommonRewardType;
    public int reward_id;
    public int reward_value;
    public string reward_title;
    public string reward_description;

    public static ColosseumRankReward Parse(MasterDataReader reader)
    {
      return new ColosseumRankReward()
      {
        ID = reader.ReadInt(),
        rank_id = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_value = reader.ReadInt(),
        reward_title = reader.ReadString(true),
        reward_description = reader.ReadString(true)
      };
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
