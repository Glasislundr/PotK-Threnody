// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillTiming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleskillTiming
  {
    public int ID;
    public int skill_id_BattleskillSkill;
    public int skill_type_BattleskillTimingLogic;

    public static BattleskillTiming Parse(MasterDataReader reader)
    {
      return new BattleskillTiming()
      {
        ID = reader.ReadInt(),
        skill_id_BattleskillSkill = reader.ReadInt(),
        skill_type_BattleskillTimingLogic = reader.ReadInt()
      };
    }

    public BattleskillSkill skill_id
    {
      get
      {
        BattleskillSkill skillId;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_id_BattleskillSkill, out skillId))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_id_BattleskillSkill + "]"));
        return skillId;
      }
    }

    public BattleskillTimingLogic skill_type
    {
      get
      {
        BattleskillTimingLogic skillType;
        if (!MasterData.BattleskillTimingLogic.TryGetValue(this.skill_type_BattleskillTimingLogic, out skillType))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillTimingLogic[" + (object) this.skill_type_BattleskillTimingLogic + "]"));
        return skillType;
      }
    }
  }
}
