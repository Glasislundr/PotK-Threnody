// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildRaid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildRaid
  {
    public int ID;
    public int period_id;
    public int lap;
    public int order;
    public string stage_name;
    public int boss_id;
    public int stage_id;
    public int damage_reward_id;
    public int kill_reward_id;
    public int log_threshold_score;
    public float image_offset_y;
    public float boss_model_scale;
    public float boss_model_offset_x;
    public float boss_model_offset_y;
    public float boss_model_offset_z;

    public static GuildRaid Parse(MasterDataReader reader)
    {
      return new GuildRaid()
      {
        ID = reader.ReadInt(),
        period_id = reader.ReadInt(),
        lap = reader.ReadInt(),
        order = reader.ReadInt(),
        stage_name = reader.ReadString(true),
        boss_id = reader.ReadInt(),
        stage_id = reader.ReadInt(),
        damage_reward_id = reader.ReadInt(),
        kill_reward_id = reader.ReadInt(),
        log_threshold_score = reader.ReadInt(),
        image_offset_y = reader.ReadFloat(),
        boss_model_scale = reader.ReadFloat(),
        boss_model_offset_x = reader.ReadFloat(),
        boss_model_offset_y = reader.ReadFloat(),
        boss_model_offset_z = reader.ReadFloat()
      };
    }

    public BattleStageEnemy getBoss() => MasterData.BattleStageEnemy[this.boss_id];

    public List<int> getDamageRewardRatiosList()
    {
      List<int> rewardRatiosList = new List<int>();
      string damageRatio = MasterData.GuildRaidDamageRewardSet[this.damage_reward_id].damage_ratio;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in damageRatio.Split(chArray))
      {
        double result = 0.0;
        if (double.TryParse(str.Trim(), out result))
          rewardRatiosList.Add((int) result);
      }
      return rewardRatiosList;
    }

    public List<GuildRaid.RaidReward> getDamageRewardsList()
    {
      List<GuildRaid.RaidReward> damageRewardsList = new List<GuildRaid.RaidReward>();
      string damageRewardId = MasterData.GuildRaidDamageRewardSet[this.damage_reward_id].damage_reward_id;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in damageRewardId.Split(chArray))
      {
        double result = 0.0;
        if (double.TryParse(str.Trim(), out result))
        {
          GuildRaidDamageReward masterData = MasterData.GuildRaidDamageReward[(int) result];
          damageRewardsList.Add(new GuildRaid.RaidReward(masterData));
        }
      }
      return damageRewardsList;
    }

    public List<GuildRaid.RaidReward> getKillRewardsList()
    {
      List<GuildRaid.RaidReward> killRewardsList = new List<GuildRaid.RaidReward>();
      string killRewardId = MasterData.GuildRaidKillRewardSet[this.kill_reward_id].kill_reward_id;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in killRewardId.Split(chArray))
      {
        double result = 0.0;
        if (double.TryParse(str.Trim(), out result))
        {
          GuildRaidKillReward masterData = MasterData.GuildRaidKillReward[(int) result];
          killRewardsList.Add(new GuildRaid.RaidReward(masterData));
        }
      }
      return killRewardsList;
    }

    public List<GuildRaid.RaidReward> getRaidEndlessKillRewardsList()
    {
      List<GuildRaid.RaidReward> endlessKillRewardsList = new List<GuildRaid.RaidReward>();
      foreach (KeyValuePair<int, GuildRaidEndlessKillReward> keyValuePair in MasterData.GuildRaidEndlessKillReward.Where<KeyValuePair<int, GuildRaidEndlessKillReward>>((Func<KeyValuePair<int, GuildRaidEndlessKillReward>, bool>) (x => x.Value.raid == this.ID)).ToList<KeyValuePair<int, GuildRaidEndlessKillReward>>())
        endlessKillRewardsList.Add(new GuildRaid.RaidReward(keyValuePair.Value));
      return endlessKillRewardsList;
    }

    public class RaidReward : GameCore.Reward
    {
      [SerializeField]
      private string title;

      public RaidReward(GuildRaidKillReward masterData)
        : base(masterData.entity_type, masterData.reward_id, masterData.reward_value)
      {
        this.title = masterData.reward_title;
      }

      public RaidReward(GuildRaidDamageReward masterData)
        : base(masterData.entity_type, masterData.reward_id, masterData.reward_value)
      {
        this.title = masterData.reward_title;
      }

      public RaidReward(GuildRaidEndlessKillReward masterData)
        : base(masterData.entity_type, masterData.rewardID, masterData.num)
      {
        this.title = masterData.reward_title;
      }

      public string Title => this.title;
    }
  }
}
