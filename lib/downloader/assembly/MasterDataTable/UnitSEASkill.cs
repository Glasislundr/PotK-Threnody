// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitSEASkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitSEASkill
  {
    public int ID;
    public int? skill_1_BattleskillSkill;
    public int? skill_2_BattleskillSkill;
    public int? skill_3_BattleskillSkill;
    public int? skill_4_BattleskillSkill;
    public int script_id;
    public int illust_release_id;
    private const int LEVEL_SKILL = 1;

    public static UnitSEASkill Parse(MasterDataReader reader)
    {
      return new UnitSEASkill()
      {
        ID = reader.ReadInt(),
        skill_1_BattleskillSkill = reader.ReadIntOrNull(),
        skill_2_BattleskillSkill = reader.ReadIntOrNull(),
        skill_3_BattleskillSkill = reader.ReadIntOrNull(),
        skill_4_BattleskillSkill = reader.ReadIntOrNull(),
        script_id = reader.ReadInt(),
        illust_release_id = reader.ReadInt()
      };
    }

    public BattleskillSkill skill_1
    {
      get
      {
        if (!this.skill_1_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skill1;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_1_BattleskillSkill.Value, out skill1))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_1_BattleskillSkill.Value + "]"));
        return skill1;
      }
    }

    public BattleskillSkill skill_2
    {
      get
      {
        if (!this.skill_2_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skill2;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_2_BattleskillSkill.Value, out skill2))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_2_BattleskillSkill.Value + "]"));
        return skill2;
      }
    }

    public BattleskillSkill skill_3
    {
      get
      {
        if (!this.skill_3_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skill3;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_3_BattleskillSkill.Value, out skill3))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_3_BattleskillSkill.Value + "]"));
        return skill3;
      }
    }

    public BattleskillSkill skill_4
    {
      get
      {
        if (!this.skill_4_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skill4;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_4_BattleskillSkill.Value, out skill4))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_4_BattleskillSkill.Value + "]"));
        return skill4;
      }
    }

    public int same_character_id => this.ID;

    public bool hasSkills => this.skill_1_BattleskillSkill.HasValue;

    public PlayerUnitSkills getSkill(int index)
    {
      if (index < 0)
        return (PlayerUnitSkills) null;
      PlayerUnitSkills skill = this.createSkill(index);
      if (skill != null)
        return skill;
      for (int index1 = index - 1; index1 >= 0 && skill == null; --index1)
        skill = this.createSkill(index1);
      return skill;
    }

    private PlayerUnitSkills createSkill(int index)
    {
      BattleskillSkill battleskillSkill = (BattleskillSkill) null;
      switch (index)
      {
        case 0:
          battleskillSkill = this.skill_1;
          break;
        case 1:
          battleskillSkill = this.skill_2;
          break;
        case 2:
          battleskillSkill = this.skill_3;
          break;
        case 3:
          battleskillSkill = this.skill_4;
          break;
      }
      if (battleskillSkill == null)
        return (PlayerUnitSkills) null;
      return new PlayerUnitSkills()
      {
        skill_id = battleskillSkill.ID,
        level = 1
      };
    }
  }
}
