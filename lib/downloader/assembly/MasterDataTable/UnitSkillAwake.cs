// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitSkillAwake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitSkillAwake
  {
    public int ID;
    public int character_id;
    public float need_affection;
    public int skill_BattleskillSkill;

    public static UnitSkillAwake Parse(MasterDataReader reader)
    {
      return new UnitSkillAwake()
      {
        ID = reader.ReadInt(),
        character_id = reader.ReadInt(),
        need_affection = reader.ReadFloat(),
        skill_BattleskillSkill = reader.ReadInt()
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
  }
}
