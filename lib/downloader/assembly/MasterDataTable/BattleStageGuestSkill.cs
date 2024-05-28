// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStageGuestSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleStageGuestSkill
  {
    public int ID;
    public int skill_group_id;
    public int skill_BattleskillSkill;
    public int skill_level;

    public static BattleStageGuestSkill Parse(MasterDataReader reader)
    {
      return new BattleStageGuestSkill()
      {
        ID = reader.ReadInt(),
        skill_group_id = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        skill_level = reader.ReadInt()
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
