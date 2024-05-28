// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearReisouSkillWeaponGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearReisouSkillWeaponGroup
  {
    public int ID;
    public int group;
    public int gear_GearGear;

    public static GearReisouSkillWeaponGroup Parse(MasterDataReader reader)
    {
      return new GearReisouSkillWeaponGroup()
      {
        ID = reader.ReadInt(),
        group = reader.ReadInt(),
        gear_GearGear = reader.ReadInt()
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
  }
}
