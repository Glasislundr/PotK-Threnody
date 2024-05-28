// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreBonusRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreBonusRule : KeyCompare
  {
    public int? target_skill_id;
    public int bonus_type;
    public int? target_job_id;
    public int? target_unit_id;

    public QuestScoreBonusRule()
    {
    }

    public QuestScoreBonusRule(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (target_skill_id)] != null)
      {
        nullable1 = (long?) json[nameof (target_skill_id)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.target_skill_id = nullable2;
      this.bonus_type = (int) (long) json[nameof (bonus_type)];
      int? nullable3;
      if (json[nameof (target_job_id)] != null)
      {
        nullable1 = (long?) json[nameof (target_job_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.target_job_id = nullable3;
      int? nullable4;
      if (json[nameof (target_unit_id)] != null)
      {
        nullable1 = (long?) json[nameof (target_unit_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.target_unit_id = nullable4;
    }
  }
}
