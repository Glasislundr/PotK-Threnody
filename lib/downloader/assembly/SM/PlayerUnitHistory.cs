// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitHistory : KeyCompare
  {
    public int broken;
    public DateTime created_at;
    public int defeat;
    public int?[] job_ids;
    public int over_killers_release_status;
    public int?[] skill_ids;
    public int unit_id;

    public bool HasEvolutionUnitSkill(UnitUnit unit)
    {
      return ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit.ID == unit.ID)).Select<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.ID)).Any<int>((Func<int, bool>) (x => ((IEnumerable<int?>) this.skill_ids).Contains<int?>(new int?(x))));
    }

    public bool HasEvolutionCharacterSkill(UnitUnit unit)
    {
      foreach (BattleskillSkill battleskillSkill in ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.unit.ID == unit.ID && x.skill_after_evolution > 0)).Select<UnitSkillCharacterQuest, BattleskillSkill>((Func<UnitSkillCharacterQuest, BattleskillSkill>) (x => x.skillOfEvolution)).ToList<BattleskillSkill>())
      {
        if (((IEnumerable<int?>) this.skill_ids).Contains<int?>(new int?(battleskillSkill.ID)))
          return true;
      }
      return false;
    }

    public bool isUnlockedSEASkill
    {
      get => this.over_killers_release_status >= PlayerUnit.SEASkillUnlockConditions - 1;
    }

    public PlayerUnitHistory()
    {
    }

    public PlayerUnitHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.broken = (int) (long) json[nameof (broken)];
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.defeat = (int) (long) json[nameof (defeat)];
      this.job_ids = ((IEnumerable<object>) json[nameof (job_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.over_killers_release_status = (int) (long) json[nameof (over_killers_release_status)];
      this.skill_ids = ((IEnumerable<object>) json[nameof (skill_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.unit_id = (int) (long) json[nameof (unit_id)];
    }
  }
}
