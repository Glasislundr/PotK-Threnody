// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleUnitLandformFootstep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleUnitLandformFootstep
  {
    public int ID;
    public int unit_footstep_type_UnitFootstepType;
    public int landform_footstep_type_BattleLandformFootstepType;
    public string footstep1;
    public string footstep2;

    public static BattleUnitLandformFootstep Parse(MasterDataReader reader)
    {
      return new BattleUnitLandformFootstep()
      {
        ID = reader.ReadInt(),
        unit_footstep_type_UnitFootstepType = reader.ReadInt(),
        landform_footstep_type_BattleLandformFootstepType = reader.ReadInt(),
        footstep1 = reader.ReadString(true),
        footstep2 = reader.ReadString(true)
      };
    }

    public UnitFootstepType unit_footstep_type
    {
      get
      {
        UnitFootstepType unitFootstepType;
        if (!MasterData.UnitFootstepType.TryGetValue(this.unit_footstep_type_UnitFootstepType, out unitFootstepType))
          Debug.LogError((object) ("Key not Found: MasterData.UnitFootstepType[" + (object) this.unit_footstep_type_UnitFootstepType + "]"));
        return unitFootstepType;
      }
    }

    public BattleLandformFootstepType landform_footstep_type
    {
      get
      {
        BattleLandformFootstepType landformFootstepType;
        if (!MasterData.BattleLandformFootstepType.TryGetValue(this.landform_footstep_type_BattleLandformFootstepType, out landformFootstepType))
          Debug.LogError((object) ("Key not Found: MasterData.BattleLandformFootstepType[" + (object) this.landform_footstep_type_BattleLandformFootstepType + "]"));
        return landformFootstepType;
      }
    }
  }
}
