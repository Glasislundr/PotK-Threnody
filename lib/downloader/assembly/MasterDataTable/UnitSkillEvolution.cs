// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitSkillEvolution
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
  public class UnitSkillEvolution
  {
    public int ID;
    public int unit_UnitUnit;
    public int before_skill_BattleskillSkill;
    public int level;
    public int after_skill_BattleskillSkill;

    public static UnitSkillEvolution Parse(MasterDataReader reader)
    {
      return new UnitSkillEvolution()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        before_skill_BattleskillSkill = reader.ReadInt(),
        level = reader.ReadInt(),
        after_skill_BattleskillSkill = reader.ReadInt()
      };
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public BattleskillSkill before_skill
    {
      get
      {
        BattleskillSkill beforeSkill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.before_skill_BattleskillSkill, out beforeSkill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.before_skill_BattleskillSkill + "]"));
        return beforeSkill;
      }
    }

    public BattleskillSkill after_skill
    {
      get
      {
        BattleskillSkill afterSkill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.after_skill_BattleskillSkill, out afterSkill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.after_skill_BattleskillSkill + "]"));
        return afterSkill;
      }
    }

    public static bool isEvolution(int unitID, int beforeSkillId, int afterSkillId)
    {
      return ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Any<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit.ID == unitID && x.before_skill.ID == beforeSkillId && x.after_skill.ID == afterSkillId));
    }

    public static UnitSkillEvolution getUnitSkillEvolution(
      int unitID,
      int beforeSkillId,
      int afterSkillId)
    {
      return ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).FirstOrDefault<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit.ID == unitID && x.before_skill.ID == beforeSkillId && x.after_skill.ID == afterSkillId));
    }
  }
}
