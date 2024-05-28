// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearReisouSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearReisouSkill
  {
    public int ID;
    public int gear_GearGear;
    public int skill_BattleskillSkill;
    public int skill_level;
    public int release_rank;
    public int skill_group;
    public int awake_weapon_group;

    public static GearReisouSkill Parse(MasterDataReader reader)
    {
      return new GearReisouSkill()
      {
        ID = reader.ReadInt(),
        gear_GearGear = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        skill_level = reader.ReadInt(),
        release_rank = reader.ReadInt(),
        skill_group = reader.ReadInt(),
        awake_weapon_group = reader.ReadInt()
      };
    }

    public GearGear gear
    {
      get
      {
        GearGear gear;
        if (!MasterData.GearGear.TryGetValue(this.gear_GearGear, out gear))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.gear_GearGear + "]"));
        return gear;
      }
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
