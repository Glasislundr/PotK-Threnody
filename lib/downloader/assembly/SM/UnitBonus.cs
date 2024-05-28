// Decompiled with JetBrains decompiler
// Type: SM.UnitBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class UnitBonus : KeyCompare
  {
    public string breakthrough_1_ratio;
    public string breakthrough_0_ratio;
    public QuestSetting[] quest_setting_list;
    public string color_code;
    public string breakthrough_3_ratio;
    public int period_id;
    public string breakthrough_4_ratio;
    public int[] target_unit_id_list;
    public string breakthrough_2_ratio;

    public UnitBonus()
    {
    }

    public UnitBonus(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.breakthrough_1_ratio = (string) json[nameof (breakthrough_1_ratio)];
      this.breakthrough_0_ratio = (string) json[nameof (breakthrough_0_ratio)];
      List<QuestSetting> questSettingList = new List<QuestSetting>();
      foreach (object json1 in (List<object>) json[nameof (quest_setting_list)])
        questSettingList.Add(json1 == null ? (QuestSetting) null : new QuestSetting((Dictionary<string, object>) json1));
      this.quest_setting_list = questSettingList.ToArray();
      this.color_code = (string) json[nameof (color_code)];
      this.breakthrough_3_ratio = (string) json[nameof (breakthrough_3_ratio)];
      this.period_id = (int) (long) json[nameof (period_id)];
      this.breakthrough_4_ratio = (string) json[nameof (breakthrough_4_ratio)];
      this.target_unit_id_list = ((IEnumerable<object>) json[nameof (target_unit_id_list)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.breakthrough_2_ratio = (string) json[nameof (breakthrough_2_ratio)];
    }

    public string GetBreakthroughRate(int breakthrough)
    {
      switch (breakthrough)
      {
        case 1:
          return this.breakthrough_1_ratio;
        case 2:
          return this.breakthrough_2_ratio;
        case 3:
          return this.breakthrough_3_ratio;
        case 4:
          return this.breakthrough_4_ratio;
        default:
          return this.breakthrough_0_ratio;
      }
    }

    public Period eventPeriod
    {
      get
      {
        Period[] source = SMManager.Get<Period[]>();
        return source != null ? ((IEnumerable<Period>) source).FirstOrDefault<Period>((Func<Period, bool>) (x => x.period_id == this.period_id)) : (Period) null;
      }
    }

    public bool IsTargetQuestS(int quest_type, int quest_s_id)
    {
      return ((IEnumerable<QuestSetting>) this.quest_setting_list).Any<QuestSetting>((Func<QuestSetting, bool>) (x => x.quest_type_id == quest_type && x.quest_s_id == quest_s_id));
    }

    public bool IsTargetQuestM(int quest_type, int quest_m_id)
    {
      return ((IEnumerable<QuestSetting>) this.quest_setting_list).Any<QuestSetting>((Func<QuestSetting, bool>) (x => x.quest_type_id == quest_type && x.quest_s_id == quest_m_id));
    }

    public static UnitBonus[] getActiveUnitBonus(DateTime serverTime, int? questType = null, int? qusetID = null)
    {
      UnitBonus[] source = SMManager.Get<UnitBonus[]>();
      return source != null ? ((IEnumerable<UnitBonus>) source).Where<UnitBonus>((Func<UnitBonus, bool>) (x =>
      {
        if (x.eventPeriod == null || !(x.eventPeriod.start_at < serverTime) || !(x.eventPeriod.end_at > serverTime))
          return false;
        if (questType.HasValue && qusetID.HasValue && x.IsTargetQuestS(questType.Value, qusetID.Value))
          return true;
        return !questType.HasValue && !qusetID.HasValue;
      })).ToArray<UnitBonus>() : new UnitBonus[0];
    }
  }
}
