// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PunitiveExpeditionEventReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PunitiveExpeditionEventReward
  {
    public int ID;
    public int period;
    public int point_type_EventPointType;
    public int must_point;
    public int point;
    public int reward_type_id_CommonRewardType;
    public int reward_id;
    public int reward_quantity;
    public string display_text1;
    public string display_text2;
    public string image_name;
    public int alignment;
    public bool is_guild_reward;

    public static PunitiveExpeditionEventReward Parse(MasterDataReader reader)
    {
      return new PunitiveExpeditionEventReward()
      {
        ID = reader.ReadInt(),
        period = reader.ReadInt(),
        point_type_EventPointType = reader.ReadInt(),
        must_point = reader.ReadInt(),
        point = reader.ReadInt(),
        reward_type_id_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadInt(),
        display_text1 = reader.ReadString(true),
        display_text2 = reader.ReadString(true),
        image_name = reader.ReadString(true),
        alignment = reader.ReadInt()
      };
    }

    public EventPointType point_type => (EventPointType) this.point_type_EventPointType;

    public CommonRewardType reward_type_id
    {
      get => (CommonRewardType) this.reward_type_id_CommonRewardType;
    }

    public void ConvertGuildReward(PunitiveExpeditionEventGuildReward guildReward)
    {
      this.alignment = guildReward.alignment;
      this.display_text1 = guildReward.display_text1;
      this.display_text2 = guildReward.display_text2;
      this.ID = guildReward.ID;
      this.image_name = guildReward.image_name;
      this.is_guild_reward = true;
      this.must_point = 0;
      this.period = guildReward.period;
      this.point = guildReward.point;
      this.point_type_EventPointType = 3;
      this.reward_id = guildReward.reward_id;
      this.reward_quantity = guildReward.reward_quantity;
      this.reward_type_id_CommonRewardType = guildReward.reward_type_id_CommonRewardType;
    }
  }
}
