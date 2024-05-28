// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreBonusTimetable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreBonusTimetable : KeyCompare
  {
    public string breakthrough_1_ratio;
    public string breakthrough_0_ratio;
    public DateTime start_at;
    public string color_code;
    public QuestScoreBonusRule[] rules;
    public string message_text;
    public string breakthrough_3_ratio;
    public DateTime end_at;
    public string breakthrough_4_ratio;
    public int quest_s_id;
    public string breakthrough_2_ratio;
    private List<int> _targetUnitIds;
    private List<int> _targetSkillIds;
    private List<int> _targetJobIds;

    public QuestScoreBonusTimetable()
    {
    }

    public QuestScoreBonusTimetable(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.breakthrough_1_ratio = (string) json[nameof (breakthrough_1_ratio)];
      this.breakthrough_0_ratio = (string) json[nameof (breakthrough_0_ratio)];
      this.start_at = DateTime.Parse((string) json[nameof (start_at)]);
      this.color_code = (string) json[nameof (color_code)];
      List<QuestScoreBonusRule> questScoreBonusRuleList = new List<QuestScoreBonusRule>();
      foreach (object json1 in (List<object>) json[nameof (rules)])
        questScoreBonusRuleList.Add(json1 == null ? (QuestScoreBonusRule) null : new QuestScoreBonusRule((Dictionary<string, object>) json1));
      this.rules = questScoreBonusRuleList.ToArray();
      this.message_text = (string) json[nameof (message_text)];
      this.breakthrough_3_ratio = (string) json[nameof (breakthrough_3_ratio)];
      this.end_at = DateTime.Parse((string) json[nameof (end_at)]);
      this.breakthrough_4_ratio = (string) json[nameof (breakthrough_4_ratio)];
      this.quest_s_id = (int) (long) json[nameof (quest_s_id)];
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

    public List<int> targeUnitIds
    {
      get
      {
        if (this._targetUnitIds == null)
          this.MakeTarget();
        return this._targetUnitIds;
      }
    }

    public List<int> targetSkillIds
    {
      get
      {
        if (this._targetSkillIds == null)
          this.MakeTarget();
        return this._targetSkillIds;
      }
    }

    public List<int> targetJobIds
    {
      get
      {
        if (this._targetJobIds == null)
          this.MakeTarget();
        return this._targetJobIds;
      }
    }

    private void MakeTarget()
    {
      this._targetUnitIds = new List<int>();
      this._targetSkillIds = new List<int>();
      this._targetJobIds = new List<int>();
      for (int index = 0; index < this.rules.Length; ++index)
      {
        switch (this.rules[index].bonus_type)
        {
          case 1:
            this._targetUnitIds.Add(this.rules[index].target_unit_id.Value);
            break;
          case 2:
            this._targetSkillIds.Add(this.rules[index].target_skill_id.Value);
            break;
          case 3:
            this._targetJobIds.Add(this.rules[index].target_job_id.Value);
            break;
        }
      }
    }
  }
}
