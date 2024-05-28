// Decompiled with JetBrains decompiler
// Type: MasterDataTable.OverkillersSlotRelease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class OverkillersSlotRelease
  {
    public int ID;
    public int slot1_unity_value;
    public int slot1_gem;
    public int slot1_materials_no;
    public int slot2_unity_value;
    public int slot2_gem;
    public int slot2_materials_no;
    public int slot3_unity_value;
    public int slot3_gem;
    public int slot3_materials_no;
    public int slot4_unity_value;
    public int slot4_gem;
    public int slot4_materials_no;
    public static readonly int MaxSlot = 4;
    private int? num_slot_;

    public static OverkillersSlotRelease Parse(MasterDataReader reader)
    {
      return new OverkillersSlotRelease()
      {
        ID = reader.ReadInt(),
        slot1_unity_value = reader.ReadInt(),
        slot1_gem = reader.ReadInt(),
        slot1_materials_no = reader.ReadInt(),
        slot2_unity_value = reader.ReadInt(),
        slot2_gem = reader.ReadInt(),
        slot2_materials_no = reader.ReadInt(),
        slot3_unity_value = reader.ReadInt(),
        slot3_gem = reader.ReadInt(),
        slot3_materials_no = reader.ReadInt(),
        slot4_unity_value = reader.ReadInt(),
        slot4_gem = reader.ReadInt(),
        slot4_materials_no = reader.ReadInt()
      };
    }

    public int same_character_id => this.ID;

    public int num_slot
    {
      get
      {
        if (this.num_slot_.HasValue)
          return this.num_slot_.Value;
        int num = 1;
        if (this.slot1_unity_value < this.slot2_unity_value)
        {
          ++num;
          if (this.slot2_unity_value < this.slot3_unity_value)
          {
            ++num;
            if (this.slot3_unity_value < this.slot4_unity_value)
              ++num;
          }
        }
        this.num_slot_ = new int?(num);
        return this.num_slot_.Value;
      }
    }

    public static OverkillersSlotRelease find(int same_character_id)
    {
      return Array.Find<OverkillersSlotRelease>(MasterData.OverkillersSlotReleaseList, (Predicate<OverkillersSlotRelease>) (x => x.same_character_id == same_character_id));
    }

    public OverkillersSlotRelease.Conditions[] getConditions()
    {
      OverkillersSlotRelease.Conditions[] conditions = new OverkillersSlotRelease.Conditions[this.num_slot];
      for (int slot_no = 0; slot_no < conditions.Length; ++slot_no)
        conditions[slot_no] = this.getConditions(slot_no);
      return conditions;
    }

    public OverkillersSlotRelease.Conditions getConditions(int slot_no)
    {
      return new OverkillersSlotRelease.Conditions(this, slot_no);
    }

    public class Conditions
    {
      public int slot { get; private set; }

      public int unity_value { get; private set; }

      public int gem { get; private set; }

      public OverkillersMaterial[] materials { get; private set; }

      public PlayerMaterialUnit[] getPlayerMaterials()
      {
        PlayerMaterialUnit[] array = SMManager.Get<PlayerMaterialUnit[]>();
        PlayerMaterialUnit[] playerMaterials = new PlayerMaterialUnit[this.materials.Length];
        for (int index = 0; index < playerMaterials.Length; ++index)
        {
          int id = this.materials[index].material_UnitUnit.Value;
          playerMaterials[index] = Array.Find<PlayerMaterialUnit>(array, (Predicate<PlayerMaterialUnit>) (x => x._unit == id));
          if (playerMaterials[index] == null)
          {
            playerMaterials[index] = new PlayerMaterialUnit();
            playerMaterials[index].id = index;
            playerMaterials[index]._unit = id;
          }
        }
        return playerMaterials;
      }

      public Conditions(OverkillersSlotRelease master, int slot) => this.reset(master, slot);

      public void reset(OverkillersSlotRelease master, int slot_no)
      {
        this.slot = slot_no;
        switch (slot_no)
        {
          case 0:
            this.unity_value = master.slot1_unity_value;
            this.gem = master.slot1_gem;
            this.materials = OverkillersMaterial.getMaterials(master.slot1_materials_no);
            break;
          case 1:
            this.unity_value = master.slot2_unity_value;
            this.gem = master.slot2_gem;
            this.materials = OverkillersMaterial.getMaterials(master.slot2_materials_no);
            break;
          case 2:
            this.unity_value = master.slot3_unity_value;
            this.gem = master.slot3_gem;
            this.materials = OverkillersMaterial.getMaterials(master.slot3_materials_no);
            break;
          case 3:
            this.unity_value = master.slot4_unity_value;
            this.gem = master.slot4_gem;
            this.materials = OverkillersMaterial.getMaterials(master.slot4_materials_no);
            break;
        }
      }
    }
  }
}
