// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallSkillGaugeRate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallSkillGaugeRate
  {
    public int ID;
    public int skill_BattleskillSkill;
    public float value;

    public static CallSkillGaugeRate Parse(MasterDataReader reader)
    {
      return new CallSkillGaugeRate()
      {
        ID = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        value = reader.ReadFloat()
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
