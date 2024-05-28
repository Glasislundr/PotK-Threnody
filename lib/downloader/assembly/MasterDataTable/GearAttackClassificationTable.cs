// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearAttackClassificationTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearAttackClassificationTable
  {
    public int ID;
    public string name;
    public int skill_BattleskillSkill;

    public static GearAttackClassificationTable Parse(MasterDataReader reader)
    {
      return new GearAttackClassificationTable()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
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
