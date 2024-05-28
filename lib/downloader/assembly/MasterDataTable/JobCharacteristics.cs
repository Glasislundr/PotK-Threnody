// Decompiled with JetBrains decompiler
// Type: MasterDataTable.JobCharacteristics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class JobCharacteristics
  {
    public int ID;
    public int skill_BattleskillSkill;
    public int? skill2_BattleskillSkill;
    public string level_pattern_id;
    public int levelmax_bonus_JobCharacteristicsLevelmaxBonus;
    public int levelmax_bonus_value;
    public int levelmax_bonus2_JobCharacteristicsLevelmaxBonus;
    public int levelmax_bonus_value2;
    public int levelmax_bonus3_JobCharacteristicsLevelmaxBonus;
    public int levelmax_bonus_value3;
    public string normal_description;
    public string lvmax_description;
    public int? xlevel_limits_XLevelLimits;
    private int[] levelup_patterns_;

    public static JobCharacteristics Parse(MasterDataReader reader)
    {
      return new JobCharacteristics()
      {
        ID = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        skill2_BattleskillSkill = reader.ReadIntOrNull(),
        level_pattern_id = reader.ReadStringOrNull(true),
        levelmax_bonus_JobCharacteristicsLevelmaxBonus = reader.ReadInt(),
        levelmax_bonus_value = reader.ReadInt(),
        levelmax_bonus2_JobCharacteristicsLevelmaxBonus = reader.ReadInt(),
        levelmax_bonus_value2 = reader.ReadInt(),
        levelmax_bonus3_JobCharacteristicsLevelmaxBonus = reader.ReadInt(),
        levelmax_bonus_value3 = reader.ReadInt(),
        normal_description = reader.ReadStringOrNull(true),
        lvmax_description = reader.ReadStringOrNull(true),
        xlevel_limits_XLevelLimits = reader.ReadIntOrNull()
      };
    }

    public BattleskillSkill skill
    {
      get
      {
        BattleskillSkill skill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_BattleskillSkill, out skill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_BattleskillSkill + "]"));
        return skill;
      }
    }

    public BattleskillSkill skill2
    {
      get
      {
        if (!this.skill2_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skill2;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill2_BattleskillSkill.Value, out skill2))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill2_BattleskillSkill.Value + "]"));
        return skill2;
      }
    }

    public JobCharacteristicsLevelmaxBonus levelmax_bonus
    {
      get => (JobCharacteristicsLevelmaxBonus) this.levelmax_bonus_JobCharacteristicsLevelmaxBonus;
    }

    public JobCharacteristicsLevelmaxBonus levelmax_bonus2
    {
      get => (JobCharacteristicsLevelmaxBonus) this.levelmax_bonus2_JobCharacteristicsLevelmaxBonus;
    }

    public JobCharacteristicsLevelmaxBonus levelmax_bonus3
    {
      get => (JobCharacteristicsLevelmaxBonus) this.levelmax_bonus3_JobCharacteristicsLevelmaxBonus;
    }

    public XLevelLimits xlevel_limits
    {
      get
      {
        if (!this.xlevel_limits_XLevelLimits.HasValue)
          return (XLevelLimits) null;
        XLevelLimits xlevelLimits;
        if (!MasterData.XLevelLimits.TryGetValue(this.xlevel_limits_XLevelLimits.Value, out xlevelLimits))
          Debug.LogError((object) ("Key not Found: MasterData.XLevelLimits[" + (object) this.xlevel_limits_XLevelLimits.Value + "]"));
        return xlevelLimits;
      }
    }

    public int[] levelup_patterns
    {
      get
      {
        return this.levelup_patterns_ ?? (this.levelup_patterns_ = this.stringToIntegers(this.level_pattern_id));
      }
    }

    private int[] stringToIntegers(string str)
    {
      if (string.IsNullOrEmpty(str))
        return new int[0];
      double result;
      return ((IEnumerable<string>) str.Split(',')).Select<string, int>((Func<string, int>) (s => !double.TryParse(s.Trim(), out result) ? 0 : (int) Math.Floor(result))).ToArray<int>();
    }
  }
}
