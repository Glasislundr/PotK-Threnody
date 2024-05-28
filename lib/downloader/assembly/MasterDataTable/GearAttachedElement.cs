// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearAttachedElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearAttachedElement
  {
    public static readonly int DefaultSkillID = 1110000001;
    private static BattleskillSkill defaultSkill_;
    public int ID;
    public int gear_GearGear;
    public int element_BattleskillSkill;

    public static BattleskillSkill DefaultSkill
    {
      get
      {
        BattleskillSkill defaultSkill = GearAttachedElement.defaultSkill_;
        if (defaultSkill != null)
          return defaultSkill;
        return !MasterData.BattleskillSkill.TryGetValue(GearAttachedElement.DefaultSkillID, out GearAttachedElement.defaultSkill_) ? (BattleskillSkill) null : GearAttachedElement.defaultSkill_;
      }
    }

    public static BattleskillSkill GetSkillByGearID(int gearId)
    {
      return Array.Find<GearAttachedElement>(MasterData.GearAttachedElementList, (Predicate<GearAttachedElement>) (x => x.gear_GearGear == gearId))?.element ?? GearAttachedElement.DefaultSkill;
    }

    public static GearAttachedElement Parse(MasterDataReader reader)
    {
      return new GearAttachedElement()
      {
        ID = reader.ReadInt(),
        gear_GearGear = reader.ReadInt(),
        element_BattleskillSkill = reader.ReadInt()
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

    public BattleskillSkill element
    {
      get
      {
        BattleskillSkill element;
        if (!MasterData.BattleskillSkill.TryGetValue(this.element_BattleskillSkill, out element))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.element_BattleskillSkill + "]"));
        return element;
      }
    }
  }
}
